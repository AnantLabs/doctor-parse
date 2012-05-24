using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

using Dr.Plugin;

namespace Dr.Pacients
{
    public class PacientsProvider : PluginProvider, IPacientCollection
    {
        public PacientsProvider(string name, Dictionary<string,string> settings) 
        {
            if (PlugIn("Dr.Pacients", name, settings))
            {

            }
        }

        private PropertyInfo prNext;
        public Pacient Next
        {
            get
            {
                if (prNext == null)
                    prNext = InstanceType.GetProperty("Next");

                return prNext.GetValue(Instance, null) as Pacient;
            }
        }

        private PropertyInfo prPrev;
        public Pacient Prev
        {
            get
            {
                if (prPrev == null)
                    prPrev = InstanceType.GetProperty("Prev");

                return prPrev.GetValue(Instance, null) as Pacient;
            }
        }

        private PropertyInfo prCount;
        public int Count
        {
            get
            {
                if (prCount == null)
                    prCount = InstanceType.GetProperty("Count");

                return (int)prCount.GetValue(Instance, null);
            }
        }

        private  MethodInfo methodSave;
        public bool Save(Pacient pacient)
        {
            if(methodSave == null)
                methodSave = InstanceType.GetMethod("Save");

            return (bool)methodSave.Invoke(Instance, new object[] { (object)pacient });
        }
    }
}
