using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

using Dr.Plugin;

namespace Dr.DataBase
{
    public class DataBaseProvider : PluginProvider , IDataBaseProvider
    {
        public DataBaseProvider(string name, Dictionary<string, string> settings)
        {
            if (PlugIn("Dr.DataBase", name, settings))
            {

            }
        }

        private MethodInfo methodExecute;
        public bool Execute(string query)
        {
            if (methodExecute == null)
                methodExecute = InstanceType.GetMethod("Execute");

            return (bool)methodExecute.Invoke(Instance, new object[] { (object)query });
        }
    }
}
