using System;

namespace EccGenerator.Structures
{
	public class Field
	{
		TypeEntry type;
		public TypeEntry Type
		{
			get { return type; }
			set { type = value; }
		}

		string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		string alias;
		public string Alias
		{
			get { return alias; }
			set { alias = value; }
		}

		bool nullable;
		public bool Nullable
		{
			get { return nullable; }
			set { nullable = value; }
		}

		public override string ToString()
		{
			return Name + (Alias != null ? " AS " + Alias : "") + "\t\t: " + Type.ToString();
		}

		long charLength = 0;
		public long CharLength
		{
			get { return charLength; }
			set { charLength = value; }
		}

		int numericPrecision = 0;
		public int NumericPrecision
		{
			get { return numericPrecision; }
			set { numericPrecision = value; }
		}
		
		short numericScale = 0;
		public short NumericScale
		{
			get { return numericScale; }
			set { numericScale = value; }
		}

		public string FullTypeName
		{
			get
			{
				string ret = this.Type.Name;

				if(Nullable && !Type.NonNullable) ret += "?";

				return ret;
			}
		}

		public string DBDefinition
		{
			get
			{
				string ret = this.Type.DBType;

				if(type.DBParameters != null)
				{
					string prm = type.DBParameters.Replace("charlength", charLength.ToString()).Replace("precision",numericPrecision.ToString()).Replace("scale",numericScale.ToString());

					ret += "(" + prm + ")";
				}

				return ret;
			}
		}

		public string InstanceParams
		{
			get
			{
				string ret = "";

				if(type.Parameters != null)
				{
					ret = type.Parameters.Replace("charlength", charLength.ToString()).Replace("precision",numericPrecision.ToString()).Replace("scale",numericScale.ToString());
				}

				return ret;
			}
		}

	}
}
