using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

using Dr.Pacients;

namespace Dr.Parser
{
    public enum MultiThreadMode 
    {
        Unknown,
        Default,
        Pool,
        Background
    }

    public class Settings
    {
        protected Dictionary<string, YamlNode> m_Settings = new Dictionary<string, YamlNode>();

        public Settings()
        {
            m_MultiThread = MultiThreadMode.Unknown;
        }

        public string Pacients
        {
            get
            {
                if (m_Settings["Pacients"] != null)
                    return m_Settings["Pacients"].Value.Trim();
                else
                    return string.Empty;
            }
        }

        public Dictionary<string, string> PacientsSettings
        {
            get { return m_Settings["Pacients"].Parameters; }
        }

        public string DataBase
        {
            get
            {
                if (m_Settings["DataBase"] != null)
                    return m_Settings["DataBase"].Value.Trim();
                else
                    return string.Empty;
            }
        }

        public Dictionary<string, string> DataBaseSettings
        {
            get { return m_Settings["DataBase"].Parameters; }
        }

        private MultiThreadMode m_MultiThread;
        public MultiThreadMode MultiThread
        {
            get
            {
                if (m_MultiThread == MultiThreadMode.Unknown)
                    if (m_Settings["MultiThread"] == null)
                        m_MultiThread = MultiThreadMode.Default;
                    else
                        switch(m_Settings["MultiThread"].Value.ToUpper())
                        {
                            case "POOL":
                                m_MultiThread = MultiThreadMode.Pool;
                                break;
                            case "BACKGROUND":
                                m_MultiThread = MultiThreadMode.Background;
                                break;
                            default:
                                m_MultiThread = MultiThreadMode.Default;
                                break;
                        }

                return m_MultiThread;
            }
        }

        public int MaxThreads
        {
            get
            {
                if (m_Settings["MaxThreads"] != null)
                    return Convert.ToInt32(m_Settings["MaxThreads"].Value.Trim());
                return 0;
            }
        }

        public int MinThreads
        {
            get
            {
                if (m_Settings["MinThreads"] != null)
                    return Convert.ToInt32(m_Settings["MinThreads"].Value.Trim());
                return 0;
            }
        }

    }
}