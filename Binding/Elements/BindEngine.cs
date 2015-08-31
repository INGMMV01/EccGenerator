using System;
using System.Collections;

using EccGenerator.Structures;
using EccGenerator.Patterns;

namespace EccGenerator.Binding.Elements
{
	class BindEngine : BindElement
	{

		FieldArray fields;
		string _name;

		internal BindEngine(string name, FieldArray fields): this(name, fields, null) {}

		internal BindEngine(string name, FieldArray fields, IDictionary extendedProperties) : base(extendedProperties)
		{
			this.fields = fields;
	
			_name = name;

			ParsePattern();
		}


		protected override object GetObject(string token)
		{
			object ret;
			int count;
			string[] keys = (string[])ExtendedInfo[METATOKEN_PKEYS];

			switch(token)
			{
				case TOKEN_NAME:
					ret = Name;
					break;

				case TOKEN_PARAMS:
					const string FORMAT_D = "{0} p{1}, ";
					string prm = "";

					if(keys != null)
					{
						foreach(string key in keys)
						{
							Field field = FindFieldByTableName(fields, key);

							if(field != null)
							{
								string type = field.Type.Name;
								if(!field.Type.NonNullable) type += "?";

								prm += string.Format(FORMAT_D,type,field.Alias);
							}
						}
						if(prm.Length > 0) prm = prm.Remove(prm.Length - 2, 2);
					}
					ret = prm;
					break;

				case TOKEN_PARAMNUMBER:
					count = 0;

					if(keys != null)
					{
						foreach(string key in keys)
						{
							Field field = FindFieldByTableName(fields, key);

							if(field != null)
							{
								count++;
							}
						}
					}
					ret = count;
					break;

				case TOKEN_PARAMINIT:
					const string FORMAT_I = "\t\tspParameters[{0}] = new ParametroEngine(p{1});\n";
					string prmi = "";
					if(keys != null)
					{
						count = 0;

						foreach(string key in keys)
						{
							Field field = FindFieldByTableName(fields, key);

							if(field != null)
							{
								prmi += string.Format(FORMAT_I,count,field.Alias);
								count++;
							}
						}
					}
					ret = prmi;
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
				return Pattern.EnginePattern;
			}
		}
	}

}
