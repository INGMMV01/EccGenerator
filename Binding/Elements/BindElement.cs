using System;
using System.Text;
using System.Collections;
using EccGenerator.Structures;

namespace EccGenerator.Binding.Elements
{
	abstract class BindElement
	{
		#region Constantes

		protected const string TOKEN_INSTANCE	  = "instance";
		protected const string TOKEN_INSTANCES	  = "instances";
		protected const string TOKEN_DECLARATION  = "declaration";
		protected const string TOKEN_DECLARATIONS = "declarations";
		protected const string TOKEN_PROPERTIES	  = "properties";
		protected const string TOKEN_PROPERTY	  = "property";

		protected const string TOKEN_PARAMINIT	  = "paraminit";
		protected const string TOKEN_PARAMS		  = "params";
		protected const string TOKEN_PARAMNUMBER  = "paramnumber";

		protected const string TOKEN_NAME		 = "name";
		protected const string TOKEN_CLAVEOBJETO = "claveobjeto";
		protected const string METATOKEN_PKEYS	 = "primarykeys";
		protected const string TOKEN_SPSELECT	 = "spselect";
		protected const string TOKEN_SPINSERT	 = "spinsert";
		protected const string TOKEN_SPUPDATE	 = "spupdate";
		protected const string TOKEN_SPDELETE	 = "spdelete";

		protected const string TOKEN_BASEDATOS	 = "basedatos";
		protected const string TOKEN_CONFIGFILE	 = "configfile";

		protected const string TOKEN_PROPTYPEPARAMS = "proptypeparams";
		protected const string TOKEN_PROPNAME		= "propname";
		protected const string TOKEN_PROPTYPE		= "proptype";
		protected const string TOKEN_FIELDNAME		= "fieldname";
		protected const string TOKEN_FIELPREFIX		= "fieldprefix";
		protected const string TOKEN_CHARLENGTH		= "charlength";
		protected const string TOKEN_PRECISION		= "precision";
		protected const string TOKEN_SCALE			= "scale";
		#endregion

		ArrayList fragments;
		Hashtable extendedInfo;

		internal BindElement() : this(null) {}

		internal BindElement(IDictionary extendedProperties)
		{
			fragments = new ArrayList();

			if(extendedProperties != null)
			{
				foreach(object key in extendedProperties.Keys)
				{
					ExtendedInfo.Add(key, extendedProperties[key]);
				}
			}
		}

		protected virtual object GetObject(string token)
		{
			object ret = null;

			switch(token)
			{
				case TOKEN_INSTANCE:
				case TOKEN_INSTANCES:
				case TOKEN_DECLARATION:
				case TOKEN_DECLARATIONS:
				case TOKEN_PROPERTIES:

				case TOKEN_PROPERTY:
				case TOKEN_NAME:
				case TOKEN_CLAVEOBJETO:
				case TOKEN_SPSELECT:
				case TOKEN_SPINSERT:
				case TOKEN_SPUPDATE:
				case TOKEN_SPDELETE:

				case TOKEN_PROPTYPEPARAMS:
				case TOKEN_PROPNAME:
				case TOKEN_PROPTYPE:
				case TOKEN_FIELDNAME:
				case TOKEN_FIELPREFIX:

				default:
					if(ExtendedInfo.Contains(token))
					{
						ret = ExtendedInfo[token];
					}
					else
					{
						ret = new Token(token);
					}
					break;


			}

			return ret;
		}

		protected void ParsePattern()
		{
			StringBuilder sb = new StringBuilder();

			string pattern = PatternString;
			bool token = false;

			foreach(char chr in pattern)
			{
				if(chr == '%')
				{
					if(token)
					{
						fragments.Add(GetObject(sb.ToString().ToLower()));
					}
					else
					{
						fragments.Add(sb.ToString());
					}
					sb.Length = 0;
					token = !token;
				}
				else
				{
					sb.Append(chr);
				}
			}
			fragments.Add(sb);
		}

		protected ArrayList Fragments
		{
			get { return fragments; }
		}

		protected Hashtable ExtendedInfo
		{
			get 
			{
				if(extendedInfo == null) extendedInfo = new Hashtable();
				return extendedInfo;
			}
		}
		
		public abstract string PatternString { get; }


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach(object obj in fragments)
			{
				sb.Append(obj == null ? "" :  System.Web.HttpUtility.HtmlDecode(obj.ToString()));
			}
			return sb.ToString();
		}

		protected static Field FindFieldByTableName(FieldArray fields, string name)
		{
			foreach(Field field in fields)
			{
				if(field.Name == name)
					return field;
			}
			return null;
		}
	}
}