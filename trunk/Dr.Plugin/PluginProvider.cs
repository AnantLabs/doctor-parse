using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using PlugInPlay;

namespace Dr.Plugin
{
    public class PluginProvider
    {
        protected object m_Instance;
        protected Type InstanceType;

        public object Instance
        {
            get { return m_Instance; }
        }

        public PluginProvider()
        {
            m_Instance = null;
            InstanceType = null;
        }

        public bool PlugIn(string nspace, string type, Dictionary<string, string> settings)
        {
            string dllPath = Path.Combine(Directory.GetCurrentDirectory(), nspace + "." + type + ".dll");
            if (File.Exists(dllPath))
            {
                object InstanceSettings = PnP.CreateInstance(nspace + "." + type + "Settings", dllPath);
                if (InstanceSettings != null)
                {
                    // Settings property
                    PropertyInfo[] properties = InstanceSettings.GetType().GetProperties();
                    foreach (PropertyInfo p in properties)
                    {
                        if (p.CanWrite)
                        {
                            if (p.PropertyType == typeof(System.String))
                            {
                                p.SetValue(InstanceSettings, settings[p.Name].Trim(), null);
                            }
                            else
                            {
                                p.SetValue(InstanceSettings, Convert.ChangeType(settings[p.Name].Trim(), p.PropertyType), null);
                            }
                        }
                    }
                    ///////////////////////////
                    m_Instance = PnP.CreateInstance(nspace + "." + type, dllPath, new object[] { InstanceSettings });
                    if (m_Instance != null)
                    {
                        InstanceType = m_Instance.GetType();
                    }
                    else
                        return false;
                }
            }
            return true;
        }
    }
}