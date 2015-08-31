using System;

namespace EccGenerator.Binding
{
	struct Token
	{
		string _name;

		public Token(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}

		public override string ToString()
		{
			return _name;
		}

	}
}
