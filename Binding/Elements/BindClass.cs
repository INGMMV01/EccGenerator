using System;
using System.Collections;

using EccGenerator.Patterns;
using EccGenerator.Structures;

namespace EccGenerator.Binding.Elements
{
	class BindClass : BindElement
	{
		BindArray declarations;
		BindArray instances;
		BindArray properties;
		FieldArray fields;

		string _name;

		internal BindClass(FieldArray fields, string name) : this(fields, name, null) {}

		internal BindClass(FieldArray fields, string name, IDictionary extendedProperties) : base(extendedProperties)
		{
			this.fields = fields;

			declarations = new BindArray();
			instances = new BindArray();
			properties = new BindArray();

			_name = name;

			Parse(fields);

			ParsePattern();
		}

		private void Parse(FieldArray fields)
		{
			foreach(Field field in fields)
			{
				BindDeclaration decl = new BindDeclaration(field, this.ExtendedInfo);
				declarations.Add(decl);

				BindInstance inst = new BindInstance(field, this.ExtendedInfo);
				instances.Add(inst);

				BindProperty prop = new BindProperty(field, this.ExtendedInfo);
				properties.Add(prop);
			}
		}

		protected override object GetObject(string token)
		{
			object ret = null;

			switch(token)
			{
				case TOKEN_BASEDATOS:
					ret = Pattern.DataBase;
					break;

				case TOKEN_CONFIGFILE:
					ret = Pattern.ConfigFile;
					break;

				case TOKEN_INSTANCES:
					ret = instances;
					break;
				case  TOKEN_DECLARATIONS:
					ret = declarations;
					break;
				case  TOKEN_PROPERTIES:
					ret = properties;
					break;
				case  TOKEN_NAME:
					ret = Name;
					break;
				case TOKEN_CLAVEOBJETO:
					const string FORMAT = "{0}{1}.ToString()";
					string[] keys = (string[])ExtendedInfo[METATOKEN_PKEYS];
					string clave = "";

					if(keys != null)
					{
						foreach(string key in keys)
						{
							Field field = FindFieldByTableName(fields, key);

							if(field != null)
							{
								if(clave.Length > 0) clave += " + ";
								clave += string.Format(FORMAT,field.Type.Prefix,field.Alias);
							}
						}
					}
					ret = clave;
					break;
				default:
					ret = base.GetObject(token);
					break;
			}

			return ret;
		}
		
		public string Name
		{
			get { return _name; }
		}

		public override string PatternString
		{
			get
			{
				return Pattern.ClassPattern;
			}
		}

	}
}
