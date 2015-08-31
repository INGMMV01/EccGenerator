using System;
using System.Collections;

using EccGenerator.Patterns;
using EccGenerator.Structures;

namespace EccGenerator.Binding.Elements
{
	class BindInstance : BindElement
	{
		Field field;

		internal BindInstance(Field field): this(field, null) {}

		internal BindInstance(Field field, IDictionary extendedProperties) : base(extendedProperties)
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
				case  TOKEN_PROPTYPEPARAMS:
					ret = field.InstanceParams;
					break;
				case  TOKEN_FIELPREFIX:
					ret = field.Type.Prefix;
					break;
				case TOKEN_PROPTYPE:
					ret = field.FullTypeName;
					break;
				case TOKEN_CHARLENGTH:
					ret = field.CharLength;
					break;
				case TOKEN_PRECISION:
					ret = field.NumericPrecision;
					break;
				case TOKEN_SCALE:
					ret = field.NumericScale;
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
				return Pattern.InstancePattern;
			}
		}

	}

}
