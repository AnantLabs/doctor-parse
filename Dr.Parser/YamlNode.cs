using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dr.Parser
{
    public class YamlNode
    {
        private String m_Name;
        private String m_Value;
        private Type m_Type;
        private YamlNode m_Parrent;
        private YamlNode[] m_Childs;

        private Dictionary<string, string> m_Parameters;

        public YamlNode(String name)
        {
            m_Name = name;
            m_Type = typeof(System.String);
            m_Parameters = new Dictionary<string, string>();
            m_Childs = null;
            m_Parrent = null;
        }

        public String Name
        {
            get { return m_Name; }
        }

        public String Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;

                if (Regex.IsMatch(value, @"^\d+$"))
                    m_Type = typeof(System.Int32);
                else if (Regex.IsMatch(value.ToLower(), @"^(on|of|no|yes|true|false)$"))
                    m_Type = typeof(System.Boolean);
                else
                    m_Type = typeof(System.String);
            }
        }

        public Type Type
        {
            get { return m_Type; }
        }


        public Dictionary<string, string> Parameters
        {
            get { return m_Parameters; }
        }
    }
}