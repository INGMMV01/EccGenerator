using System;
using System.Collections;

using EccGenerator.Binding.Elements;
using EccGenerator.Structures;

namespace EccGenerator.Binding
{
	public sealed class Binder
	{
		private Binder()
		{
			
		}

		public static string BindClass(string entidad, FieldArray fields, IDictionary extendedProperties)
		{
			BindClass bindclass = new BindClass(fields, entidad, extendedProperties);

			return bindclass.ToString();
		}

		public static string BindEngine(string entidad, FieldArray fields, IDictionary extendedProperties)
		{
			BindEngine bindengine = new BindEngine(entidad, fields, extendedProperties);

			return bindengine.ToString();
		}
	}
}