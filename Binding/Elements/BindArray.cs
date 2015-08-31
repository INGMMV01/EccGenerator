using System;
using System.Text;
using System.Collections;

namespace EccGenerator.Binding.Elements
{
	class BindArray : ArrayList
	{
		public BindArray() : base() {}
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(object prop in this)
			{
				sb.Append(prop.ToString());
			}
			return sb.ToString();
		}
	}
}
