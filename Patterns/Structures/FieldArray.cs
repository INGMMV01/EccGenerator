using System;
using System.Text;
using System.Collections;

namespace EccGenerator.Structures
{
	public class FieldArray : ArrayList
	{
		public FieldArray() : base() { }

		public int Add(Field item)
		{
			return base.Add(item);
		}

		new public Field this[int idx]
		{
			get { return (Field)base[idx]; }
			set { base[idx] = value; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach(Field fld in this)
			{
				sb.Append(fld.ToString() + "\n");
			}
			return sb.ToString();
		}

	}
}
