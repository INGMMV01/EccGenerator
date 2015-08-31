using System;

namespace EccGenerator.Structures
{
	public class TypeEntry
	{
		internal string name;
		public string Name 
		{ 
			get { return name; }
		}

		internal string prefix;
		public string Prefix
		{
			get { return prefix; }
		}

		internal string parameters;
		public string Parameters
		{
			get { return parameters; }
		}

		internal string dbparameters;
		public string DBParameters
		{
			get { return dbparameters; }
		}

		internal string dbtype;
		public string DBType
		{
			get { return dbtype; }
		}

		internal string basetype;
		public string BaseType
		{
			get { return basetype; }
		}

		internal bool nonNullable;
		public bool NonNullable
		{
			get { return nonNullable; }
		}

		public override string ToString()
		{
			return Name;
		}
	}
}