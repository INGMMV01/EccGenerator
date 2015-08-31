using System;
using System.Collections;

namespace EccGenerator.Structures
{
	public class TypeEntryArray : ArrayList
	{
		public TypeEntryArray() : base() { }

		public int Add(TypeEntry item)
		{
			return base.Add(item);
		}

		new public TypeEntry this[int idx]
		{
			get { return (TypeEntry)base[idx]; }
			set { base[idx] = value; }
		}
	}
}
