using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;

using EccGenerator.Patterns;
using EccGenerator.Structures;

namespace EccGenerator.Sql
{
	public enum SPType
	{
		Select,
		Insert,
		Update,
		Delete
	}

	public sealed class SqlExtractor
	{
		#region Constantes

		const int SCHEMA_TABLE_CATALOG = 0;
		const int SCHEMA_TABLE_SCHEMA = 1;
		const int SCHEMA_TABLE_NAME = 2;
		const int SCHEMA_COLUMN_NAME = 3;
		const int SCHEMA_COLUMN_GUID = 4;
		const int SCHEMA_COLUMN_PROPID = 5;
		const int SCHEMA_ORDINAL_POSITION = 6;
		const int SCHEMA_COLUMN_HASDEFAULT = 7;
		const int SCHEMA_COLUMN_DEFAULT = 8;
		const int SCHEMA_COLUMN_FLAGS = 9;
		const int SCHEMA_IS_NULLABLE = 10;
		const int SCHEMA_DATA_TYPE = 11;
		const int SCHEMA_TYPE_GUID = 12;
		const int SCHEMA_CHARACTER_MAXIMUM_LENGTH = 13;
		const int SCHEMA_CHARACTER_OCTET_LENGTH = 14;
		const int SCHEMA_NUMERIC_PRECISION = 15;
		const int SCHEMA_NUMERIC_SCALE = 16;
		const int SCHEMA_DATETIME_PRECISION = 17;
		const int SCHEMA_CHARACTER_SET_CATALOG = 18;
		const int SCHEMA_CHARACTER_SET_SCHEMA = 19;
		const int SCHEMA_CHARACTER_SET_NAME = 20;
		const int SCHEMA_COLLATION_CATALOG = 21;
		const int SCHEMA_COLLATION_SCHEMA = 22;
		const int SCHEMA_COLLATION_NAME = 23;
		const int SCHEMA_DOMAIN_CATALOG = 24;
		const int SCHEMA_DOMAIN_SCHEMA = 25;
		const int SCHEMA_DOMAIN_NAME = 26;
		const int SCHEMA_DESCRIPTION = 27;
		const int SCHEMA_COLUMN_LCID = 28;
		const int SCHEMA_COLUMN_COMPFLAGS = 29;
		const int SCHEMA_COLUMN_SORTID = 30;
		const int SCHEMA_COLUMN_TDSCOLLATION = 31;
		const int SCHEMA_IS_COMPUTED = 32;

		#endregion

		private SqlExtractor() { }

		static SqlExtractor() 
		{
			
		}

		public static string[] GetTables()
		{
			using(SqlConnection cnn = new SqlConnection(connectionString))
			{
				SqlCommand comm = new SqlCommand();

				comm.CommandText = "SELECT name FROM sysobjects WHERE type='U' ORDER BY name";
				comm.Connection = cnn;
				comm.CommandType = CommandType.Text;
				
				cnn.Open();	

				SqlDataReader dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
				
				
				ArrayList arr = new ArrayList();

				while(dr.Read())
				{
					arr.Add(dr.GetString(0));
				}

				dr.Close();

				return (string[])arr.ToArray(typeof(string));
			}
		}

		public static string[] GetPrimaryKeys(string tablename)
		{
			using(OleDbConnection cnn = new OleDbConnection("Provider=SQLOLEDB;" + connectionString))
			{
				string[] strcnn = connectionString.Split(';');
				string catalog = null;

				foreach(string str in strcnn)
				{
					string[] subs = str.Split('=');
					if(subs[0].ToLower() == "initial catalog") 
					{
						catalog = subs[1];
						break;
					}
				}

				cnn.Open();
				DataTable tbl = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,new object[]{ catalog, "dbo", tablename });
				
				ArrayList arr = new ArrayList();

				foreach(DataRow dtr in tbl.Rows)
				{
					arr.Add(dtr[SCHEMA_COLUMN_NAME].ToString());
				}

				cnn.Close();
				
				return (string[])arr.ToArray(typeof(string));
			}
		}

		public static FieldArray GetTableFields(string tablename)
		{
			using(OleDbConnection cnn = new OleDbConnection("Provider=SQLOLEDB;" + connectionString))
			{
				string[] strcnn = connectionString.Split(';');
				string catalog = null;

				foreach(string str in strcnn)
				{
					string[] subs = str.Split('=');
					if(subs[0].ToLower() == "initial catalog") 
					{
						catalog = subs[1];
						break;
					}
				}

				FieldArray ret = new FieldArray();
				
				cnn.Open();
				DataTable tbl = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,new object[]{ catalog, "dbo", tablename });
				
				foreach(DataRow row in tbl.Rows)
				{
					Field fld = new Field();

					SetOledbType(fld, row.ItemArray);

					if(fld.Type != null)
					{
						fld.Name = (string)row.ItemArray[SCHEMA_COLUMN_NAME];
						fld.Nullable = (bool)row.ItemArray[SCHEMA_IS_NULLABLE];

						ret.Add(fld);
					}
				}
				cnn.Close();
				
				return ret;
			}
		}

		public static FieldArray GetTableFields_old(string tablename)
		{
			using(SqlConnection cnn = new SqlConnection(connectionString))
			{
				SqlCommand comm = new SqlCommand();
				DataTable dtt;

				comm.CommandText = "SELECT TOP 0 * FROM " + tablename + " FOR XML AUTO, XMLDATA";
				comm.Connection = cnn;
				comm.CommandType = CommandType.Text;
				
				cnn.Open();	

				XmlReader xr = comm.ExecuteXmlReader();
				
				DataSet ds = new DataSet();
				ds.ReadXml(xr);
				dtt = ds.Tables[0];
				
				FieldArray ret = new FieldArray();

				foreach(DataColumn dtc in dtt.Columns)
				{
					TypeEntry te = Pattern.GetBaseType(dtc.DataType.Name);

					if(te != null)
					{
						Field fld = new Field();
						fld.Name = dtc.ColumnName;
						fld.Type = te;

						ret.Add(fld);
					}
				}

				cnn.Close();

				return ret;
			}
		}

		#region Create Procedures

		public static string CreateSelectProcedure(string aplicacion, string tablename, FieldArray fields, string entidad)
		{
			/*
			 CREATE PROCEDURE [dbo].[RPOSXXX_entidad_S]
				@alias1 AS dbotype,
				@alias2 AS dbotype
			AS
			 BEGIN
				SELECT	field1 AS alias1,
						field2 AS alias2,
						field3 AS alias3
				FROM	tablename
				WHERE	(field1 = @alias1 OR @alias1 IS NULL)
				  AND	(field2 = @alias2 OR @alias2 IS NULL)
			 END
			 */
			const string PARAM_FORMAT = "\n\t@{0} AS {1},";
			const string ALIAS_FORMAT = "\n\t\t\t{0} AS {1},";
			const string WHERE_FORMAT = "({0} = @{1} OR @{1} IS NULL)\n\t  AND ";

			string spName = GetSPName(aplicacion,entidad,SPType.Select);
			string SP_FORMAT =
					"CREATE PROCEDURE [dbo].[" + spName + "] \n" +
					"{0}\n" +
					"AS\n" +
					"BEGIN\n" +
					"\tSELECT {1} " + 
					"\n\tFROM " + tablename + " \n" +
					"\n\tWHERE {2} \n" +
					"END;";

			string paramPart = "";
			string wherePart = "";
			string selectPart = "";

			foreach(Field fld in fields)
			{
				if(fld.Alias == null || fld.Alias == "") fld.Alias = fld.Name;
				selectPart += string.Format(ALIAS_FORMAT, fld.Name, fld.Alias);
			}

			string[] pks = GetPrimaryKeys(tablename);
			
			foreach(string pk in pks)
			{
				Field field = null;
				foreach(Field fld in fields)
				{
					if(fld.Name == pk)
					{	
						field = fld;
						break;
					}
				}

				if(field != null)
				{
					string nParam = string.Format(PARAM_FORMAT, field.Alias, field.DBDefinition);
					paramPart += nParam;

					string nWhere = string.Format(WHERE_FORMAT, field.Name, field.Alias);
					wherePart += nWhere;
				}
			}

			paramPart = CleanSPPart(paramPart);
			selectPart = CleanSPPart(selectPart);
			wherePart = CleanSPPart(wherePart);

			if(wherePart.Length > 0)wherePart = wherePart.Remove(wherePart.Length - 4, 4);

			return string.Format(SP_FORMAT, paramPart, selectPart, wherePart);
		}

		public static string CreateInsertProcedure(string aplicacion, string tablename, FieldArray fields, string entidad)
		{
			/*
			 CREATE PROCEDURE [dbo].[RPOSXXX_entidad_I]
				@alias1 AS dbotype,
				@alias2 AS dbotype
			 AS
			 BEGIN
				INSERT INTO tablename
						(field1,field2,field3)
				VALUES	(@alias1,@alias2,@alias3)
			 END
			 */
			const string PARAM_FORMAT = "\n\t@{0} AS {1},";
			const string FIELD_FORMAT = "\n\t\t\t{0},";
			const string VALUE_FORMAT = "\n\t\t\t@{0},";

			string spName = GetSPName(aplicacion,entidad,SPType.Insert);

			string SP_FORMAT =
				"CREATE PROCEDURE [dbo].[" + spName + "] \n" +
				"{0}\n" +
				"AS\n" +
				"BEGIN\n" +
				"\tINSERT INTO " + tablename +  " \n" +
				"\t\t\t({1})\n" +
				"\t\t\tVALUES({2}) \n" +
				"END;";

			string paramPart = "";
			string fieldsPart = "";
			string valuesPart = "";
			
			foreach(Field fld in fields)
			{
				if(fld.Alias == null || fld.Alias == "") fld.Alias = fld.Name;

				paramPart += string.Format(PARAM_FORMAT, fld.Alias, fld.DBDefinition);
				fieldsPart += string.Format(FIELD_FORMAT, fld.Name);
				valuesPart += string.Format(VALUE_FORMAT, fld.Alias);
			}

			paramPart = CleanSPPart(paramPart);
			fieldsPart = CleanSPPart(fieldsPart);
			valuesPart = CleanSPPart(valuesPart);

			return string.Format(SP_FORMAT, paramPart, fieldsPart, valuesPart);
		}

		public static string CreateUpdateProcedure(string aplicacion, string tablename, FieldArray fields, string entidad)
		{
			/*
			 CREATE PROCEDURE [dbo].[RPOSXXX_entidad_I]
				@alias1 AS dbotype,
				@alias2 AS dbotype
			 AS
			 BEGIN
				UPDATE tablename
				SET	   field2 = @alias2,
					   field3 = @alias3
				WHERE  field1 = @alias1
			 END
			 */
			const string PARAM_FORMAT = "\n\t@{0} AS {1},";
			const string SET_FORMAT = "\n\t\t\t\t{0} = @{1},";
			const string WHERE_FORMAT = "{0} = @{1}\n\t\t\t  AND ";

			string spName = GetSPName(aplicacion,entidad,SPType.Update);

			string SP_FORMAT =
				"CREATE PROCEDURE [dbo].[" + spName + "] \n" +
				"\t{0}\n" +
				"AS\n" +
				"BEGIN\n" +
				"\tUPDATE " + tablename +  " \n" +
				"\t\t\tSET\n" +
				"\t\t\t{1}\n" +
				"\t\t\tWHERE {2} \n" +
				"END;";

			string paramPart = "";
			string wherePart = "";
			string setPart = "";

			FieldArray nfields = new FieldArray();

			foreach(Field fld in fields)
			{
				if(fld.Alias == null || fld.Alias == "") fld.Alias = fld.Name;
				nfields.Add(fld);
			}

			string[] pks = GetPrimaryKeys(tablename);
			
			foreach(string pk in pks)
			{
				Field field = null;
				foreach(Field fld in fields)
				{
					if(fld.Name == pk)
					{	
						nfields.Remove(fld);
						field = fld;
						break;
					}
				}

				if(field != null)
				{
					string nParam = string.Format(PARAM_FORMAT, field.Alias, field.DBDefinition);
					paramPart += nParam;

					string nWhere = string.Format(WHERE_FORMAT, field.Name, field.Alias);
					wherePart += nWhere;
				}
			}

			foreach(Field fld in nfields)
			{
				string nParam = string.Format(PARAM_FORMAT, fld.Alias, fld.DBDefinition);
				paramPart += nParam;

				setPart += string.Format(SET_FORMAT, fld.Name, fld.Alias);
			}

			paramPart = CleanSPPart(paramPart);
			setPart = CleanSPPart(setPart);
			wherePart = CleanSPPart(wherePart);

			if(wherePart.Length > 0)wherePart = wherePart.Remove(wherePart.Length - 4, 4);

			return string.Format(SP_FORMAT, paramPart, setPart, wherePart);
		}

		public static string CreateDeleteProcedure(string aplicacion, string tablename, FieldArray fields, string entidad)
		{
			/*
			 CREATE PROCEDURE [dbo].[RPOSXXX_entidad_I]
				@alias1 AS dbotype,
				@alias2 AS dbotype
			 AS
			 BEGIN
				DELETE FROM tablename
				WHERE  field1 = @alias1
			 END
			 */
			const string PARAM_FORMAT = "\n\t@{0} AS {1},";
			const string WHERE_FORMAT = "{0} = @{1}\n\t\t\t  AND ";

			string spName = GetSPName(aplicacion,entidad,SPType.Delete);

			string SP_FORMAT =
				"CREATE PROCEDURE [dbo].[" + spName + "] \n" +
				"\t{0}\n" +
				"AS\n" +
				"BEGIN\n" +
				"\tDELETE FROM " + tablename +  " \n" +
				"\tWHERE {1} \n" +
				"END;";

			string paramPart = "";
			string wherePart = "";

			foreach(Field fld in fields)
			{
				if(fld.Alias == null || fld.Alias == "") fld.Alias = fld.Name;
			}

			string[] pks = GetPrimaryKeys(tablename);
			
			foreach(string pk in pks)
			{
				Field field = null;
				foreach(Field fld in fields)
				{
					if(fld.Name == pk)
					{	
						field = fld;
						break;
					}
				}

				if(field != null)
				{
					string nParam = string.Format(PARAM_FORMAT, field.Alias, field.DBDefinition);
					paramPart += nParam;
					string nWhere = string.Format(WHERE_FORMAT, field.Name, field.Alias);
					wherePart += nWhere;
				}
			}

			paramPart = CleanSPPart(paramPart);
			wherePart = CleanSPPart(wherePart);

			if(wherePart.Length > 0)wherePart = wherePart.Remove(wherePart.Length - 4, 4);

			return string.Format(SP_FORMAT, paramPart, wherePart);
		}

		#endregion
		
		#region Utilidades

		private static string CleanSPPart(string spPart)
		{
			if(spPart == null)
				spPart = "";
			else
				spPart = spPart.Trim();

			if(spPart.Length > 0)
			{
				if(spPart[spPart.Length - 1] == ',') spPart = spPart.Remove(spPart.Length - 1, 1);
			}

			return spPart;
		}

		private static void SetOledbType(Field field, object[] rowdata)
		{
			string tname = (string)rowdata[SCHEMA_COLUMN_NAME];
			OleDbType type = (OleDbType)rowdata[SCHEMA_DATA_TYPE];
			
			switch(type)
			{
				case OleDbType.BigInt:
					field.Type = Pattern.GetDBType("BIGINT");
					break;

				case OleDbType.SmallInt:
					field.Type = Pattern.GetDBType("SMALLINT");
					break;

				case OleDbType.Integer:
					field.Type = Pattern.GetDBType("INT");
					break;

				case OleDbType.Char:
					long charLen = (long)rowdata[SCHEMA_CHARACTER_MAXIMUM_LENGTH];

					if(charLen < 51)
					{
						field.Type = Pattern.GetDBType("CHAR");
					}
					else if (charLen < 256)
					{
						field.Type = Pattern.GetDBType("VARCHAR");
					}
					else
					{
							field.Type = Pattern.GetDBType("TEXT");
					}
					field.CharLength = charLen;
					break;

				case OleDbType.Numeric:
					int precision = (int)rowdata[SCHEMA_NUMERIC_PRECISION];
					short scale = (short)rowdata[SCHEMA_NUMERIC_SCALE];

					field.Type = Pattern.GetDBType("DECIMAL");
					field.NumericPrecision = precision;
					field.NumericScale = scale;
					break;

				case OleDbType.DBDate:
				case OleDbType.DBTime:
				case OleDbType.DBTimeStamp:
					field.Type = Pattern.GetDBType("DATETIME");
					break;

				default:
					break;
			}
		}

		public static string GetSPName(string aplicacion, string entidad, SPType sptype)
		{
			const string FORMATO = "{0}_{1}_{2}";
			string postfix = "";

			switch(sptype)
			{
				case SPType.Select:
					postfix = "S";
					break;
				case SPType.Delete:
					postfix = "D";
					break;
				case SPType.Insert:
					postfix = "I";
					break;
				case SPType.Update:
					postfix = "U";
					break;
			}

			return string.Format(FORMATO,aplicacion.ToUpper(),entidad,postfix);
		}

        private static string connectionString
        {
            get
            {
                return System.Configuration.ConfigurationSettings.AppSettings.Get("DSNBD");

                //return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IndicadoresBolsaDB;IntegratedSecurity=True";
            }
        }

		#endregion

	}
}
