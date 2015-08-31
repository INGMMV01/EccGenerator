using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

using EccGenerator.Structures;

namespace EccGenerator.Patterns
{
	public sealed class Pattern
	{
		const string TAG_CLASS		= "class";
		const string TAG_ENGINE		= "engine";

		const string TAG_PROPERTY	 = "property";
		const string TAG_INSTANCE	 = "instance";
		const string TAG_DECLARATION = "declaration";
		const string TAG_TYPES		 = "types";
		const string TAG_TYPE		 = "type";
		const string TAG_CLAVEOBJETO = "claveobjeto";

		const string TAG_CONFIGFILE = "config/file";
		const string TAG_CONFIGBBDD = "config/bbdd";

		const string ATTR_NAME   = "name";
		const string ATTR_PREFIX = "prefix";
		const string ATTR_PARAMS = "params";
		const string ATTR_DBPARAMS = "dbparams";
		const string ATTR_DBTYPE = "dbtype";
		const string ATTR_BASETYPE = "basetype";
		const string ATTR_NONNULLABLE = "nonnullable";
		
		const string XPATH_ROOT = "/pattern/";
		
		const string XML_RELATIVE_PATH = "\\xml\\patterns.xml";

		private static StringDictionary entities;
		private static SortedList types;
		private static SortedList dbtypes;
		private static SortedList basetypes;
		private static bool isLoaded = false;

		static Pattern()
		{
			string dir = System.IO.Directory.GetCurrentDirectory();

			if(File.Exists(dir + XML_RELATIVE_PATH))
			{
				LoadPattern(dir + XML_RELATIVE_PATH);
			}
		}

		private Pattern() { }

		public static void LoadPattern(string filepath)
		{
			entities = new StringDictionary();
			types = new SortedList();
			basetypes = new SortedList();
			dbtypes = new SortedList();

			string dir = System.IO.Directory.GetCurrentDirectory();
			FileStream fs = new FileStream(filepath, FileMode.Open);

			XmlDocument dom = new XmlDocument();

			dom.Load(fs);		
			
			string[] tags = new string[]
				{
					TAG_CLASS,
					TAG_ENGINE,
					TAG_INSTANCE,
					TAG_DECLARATION,
					TAG_PROPERTY,
					TAG_CONFIGFILE,
					TAG_CONFIGBBDD
				};

			foreach(string tag in tags)
			{
				XmlNode node = dom.SelectSingleNode(XPATH_ROOT + tag);
				entities.Add(tag, node.InnerXml);
			}

			XmlNodeList nodes = dom.SelectNodes(XPATH_ROOT + TAG_TYPES + "/" + TAG_TYPE);
			foreach(XmlNode n in nodes)
			{
				TypeEntry te = new TypeEntry();
				
				te.name = n.Attributes[ATTR_NAME].Value;

				XmlAttribute attr = n.Attributes[ATTR_PREFIX];
				if(attr != null) te.prefix = attr.Value;
				
				attr = n.Attributes[ATTR_PARAMS];
				if(attr != null) te.parameters = attr.Value;

				attr = n.Attributes[ATTR_DBPARAMS];
				if(attr != null) te.dbparameters = attr.Value;

				attr = n.Attributes[ATTR_DBTYPE];
				if(attr != null) te.dbtype = attr.Value;
				
				attr = n.Attributes[ATTR_BASETYPE];
				if(attr != null) te.basetype = attr.Value;

				attr = n.Attributes[ATTR_NONNULLABLE];
				if(attr != null) te.nonNullable = (attr != null && attr.Value.ToLower() == "true");

				if(!types.Contains(te.Name)) types.Add(te.Name, te);
				if(!dbtypes.Contains(te.DBType)) dbtypes.Add(te.DBType, te);
				if(!basetypes.Contains(te.BaseType)) basetypes.Add(te.BaseType, te);
				
			}

			fs.Close();

			isLoaded = true;
		}

		public static bool IsLoaded
		{
			get { return isLoaded; }
		}

		public static string GetEntity(string name)
		{
			return entities[name];
		}

		public static string ConfigFile
		{
			get { return entities[TAG_CONFIGFILE]; }
		}

		public static string DataBase
		{
			get { return entities[TAG_CONFIGBBDD]; }
		}

		public static string ClassPattern
		{
			get { return entities[TAG_CLASS]; }
		}

		public static string EnginePattern
		{
			get { return entities[TAG_ENGINE]; }
		}

		public static string InstancePattern
		{
			get { return entities[TAG_INSTANCE]; }
		}

		public static string DeclarationPattern
		{
			get { return entities[TAG_DECLARATION]; }
		}

		public static string PropertyPattern
		{
			get { return entities[TAG_PROPERTY]; }
		}

		public static TypeEntry GetType(string name)
		{
			return (TypeEntry)types[name];
		}

		public static TypeEntry GetDBType(string name)
		{
			return (TypeEntry)dbtypes[name];
		}

		public static TypeEntry GetBaseType(string name)
		{
			return (TypeEntry)basetypes[name];
		}

		public static TypeEntry[] GetTypes()
		{
			TypeEntry[] entries = new TypeEntry[types.Count];

			int i = 0;
			foreach(TypeEntry te in types.Values)
			{
				entries[i] = te;
				i++;
			}

			return entries;
		}
	}
}
