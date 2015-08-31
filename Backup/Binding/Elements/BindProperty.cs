using System;
using System.Collections;

using EccGenerator.Patterns;
using EccGenerator.Structures;

namespace EccGenerator.Binding.Elements
{
	class BindProperty : BindElement
	{
		Field field;

		internal BindProperty(Field field): this(field, null) {}

		internal BindProperty(Field field, IDictionary extendedProperties) : base(extendedProperties)
		{
			this.field = field;
			ParsePattern();
		}

		protected override object GetObject(string token)
		{
			object ret = null;

			switch(token)
			{
				case  TOKEN_FIELDNAME:
				case  TOKEN_PROPNAME:
					ret = field.Alias;
					break;
				case  TOKEN_PROPTYPE:
					ret = field.FullTypeName;
					break;
				case  TOKEN_FIELPREFIX:
					ret = field.Type.Prefix;
					break;
				default:
					ret = base.GetObject(token);
					break;
			}

			return ret;
		}

		public override string PatternString
		{
			get
			{
				return Pattern.PropertyPattern;
			}
		}

	}
}
