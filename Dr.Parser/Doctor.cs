﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;

using Dr.Pacients;

namespace Dr.Parser
{
    public enum DoctorsKeywords
    {

    }

    public delegate void DoctorExceptionHandler(DoctorExceptionArgs e);
    public class DoctorExceptionArgs
    {
        public DoctorExceptionArgs(string message) { Message = message; }
        public string Message { get; private set; }
    }

    public delegate void DoctorExecuteQueryHandler(DoctorExecuteQueryArgs e);
    public class DoctorExecuteQueryArgs
    {
        public DoctorExecuteQueryArgs(string query) { Query = query; }
        public string Query { get; private set; }
    }

    public delegate void DoctorFileHandler(DoctorFileArgs e);
    public class DoctorFileArgs
    {
        public DoctorFileArgs(FileInfo file, string data) { File = file; Data = data;}
        public FileInfo File { get; private set; }
        public string Data { get; private set; }
    }

    public class Doctor
    {
        public event DoctorExceptionHandler OnException;
        public event DoctorExecuteQueryHandler OnExecute;
        public event DoctorFileHandler OnFile;

        Dictionary<string, MethodInfo> methods;
        
        public Doctor()
        {
            methods = new Dictionary<string, MethodInfo>();
        }

        public void Do(string name,  Dictionary<string, string> options, ref Pacient p)
        {
            MethodInfo method = null;
            if (!methods.ContainsKey(name))
            {
                method = this.GetType().GetMethod("_" + name);
                if (method != null)
                    methods.Add(name, method);
                else
                    return;
            }
            else 
            {
                method = methods[name];
            }

            object[] parametrs = new object[2];
            try
            {
                method.Invoke((object)this, new object[] { p, options });
            }
            catch (Exception exc)
            {
                if (OnException != null)
                {
                    if (exc.InnerException.InnerException != null)
                        OnException(new DoctorExceptionArgs(string.Format("Name: {0}\r\n; Exception: {1}\r\n", name, exc.InnerException.InnerException.Message)));
                    else
                        OnException(new DoctorExceptionArgs(string.Format("Name: {0}\r\n; Exception: {1}\r\n", name, exc.Message)));
                }
            }
        }

        private bool _template(ref Pacient pacient, ref StringBuilder tpl, Regex ex, Boolean IsFirstMatch)
        {
            Boolean IsContains = false;
            if ((ex == null) && (tpl == null)) return IsContains;

            MatchCollection matches = ex.Matches(pacient.Body);
            Int32 mcount = (IsFirstMatch && matches.Count > 1) ? 1 : matches.Count;

            // matches
            for (int ctr = 0; ctr < mcount; ctr++)
            {
                //groups
                GroupCollection group = matches[ctr].Groups;
                for (var i = 1; i <= group.Count; i++)
                {
                    if (tpl.ToString().IndexOf("$" + i) >= 0)
                    {
                        IsContains = true;
                        tpl.Replace("$" + i, group[i].Value);
                    }
                }
                tpl.Replace("$index", matches[ctr].Index.ToString());
                tpl.Replace("$length", matches[ctr].Length.ToString());
            }
            tpl.Replace("$name", pacient.Name);
            return IsContains;
        }

        public void _sql(ref Pacient pacient, Dictionary<string, string> parameters)
        {
            Boolean IsFirstMatch = false;
            RegexOptions options = RegexOptions.None;
            if (parameters.ContainsKey("options") && !string.IsNullOrEmpty(parameters["options"]))
                RegExUtility.RegexOptionsParse(parameters["options"], ref options, ref IsFirstMatch);

            Regex ex = null;
            if (parameters.ContainsKey("regex") && !string.IsNullOrEmpty(parameters["regex"]))
                ex = new Regex(parameters["regex"], options);

            StringBuilder tpl = null;
            if (parameters.ContainsKey("query") && !string.IsNullOrEmpty(parameters["query"]))
                tpl = new StringBuilder(parameters["query"]);

            if (_template(ref pacient, ref tpl, ex, IsFirstMatch))
            {
                if(OnExecute != null)
                    OnExecute(new DoctorExecuteQueryArgs(tpl.ToString()));
            }
        }

        public void _remove(ref Pacient pacient, Dictionary<string, string> parameters)
        {
            RegexOptions options = RegexOptions.None;
            if(parameters.ContainsKey("options") && !string.IsNullOrEmpty(parameters["options"]))
                RegExUtility.RegexOptionsParse(parameters["options"],ref options);

            if (parameters.ContainsKey("regex") && !string.IsNullOrEmpty(parameters["regex"]))
            {
                Regex ex = new Regex(parameters["regex"], options);
                pacient.Body = ex.Replace(pacient.Body, string.Empty);
            }

            if (parameters.ContainsKey("pattern") && !string.IsNullOrEmpty(parameters["pattern"]))
            {
                pacient.Body = pacient.Body.Replace(parameters["pattern"], string.Empty);
            }
        }

        public void _replace(ref Pacient pacient, Dictionary<string, string> parameters)
        {
            RegexOptions options = RegexOptions.None;
            if (parameters.ContainsKey("options") && !string.IsNullOrEmpty(parameters["options"]))
                RegExUtility.RegexOptionsParse(parameters["options"], ref options);

            if ((parameters.ContainsKey("regex") && parameters.ContainsKey("value")) && 
                (!string.IsNullOrEmpty(parameters["regex"]) && !string.IsNullOrEmpty(parameters["value"]))
                )
            {
                Regex ex = new Regex(parameters["regex"], options);
                pacient.Body = ex.Replace(pacient.Body, parameters["value"]);
            }

            if ((parameters.ContainsKey("pattern") && parameters.ContainsKey("value")) &&
                (!string.IsNullOrEmpty(parameters["pattern"]) && !string.IsNullOrEmpty(parameters["value"]))
                )
            {
                pacient.Body = pacient.Body.Replace(parameters["pattern"], parameters["value"]);
            }
        }

        public void _append(ref Pacient pacient, Dictionary<string, string> parameters)
        {
            Boolean IsFirstMatch = false;
            RegexOptions options = RegexOptions.None;
            if (parameters.ContainsKey("options") && !string.IsNullOrEmpty(parameters["options"]))
                RegExUtility.RegexOptionsParse(parameters["options"], ref options,ref IsFirstMatch);

            Regex ex = null;
            if (parameters.ContainsKey("regex") && !string.IsNullOrEmpty(parameters["regex"]))
                ex = new Regex(parameters["regex"], options);

            StringBuilder tpl = null;
            if (parameters.ContainsKey("template") && !string.IsNullOrEmpty(parameters["template"]))
                tpl = new StringBuilder(parameters["template"]);

            if (_template(ref pacient, ref tpl, ex, IsFirstMatch))
            {
                if (parameters.ContainsKey("file") && !string.IsNullOrEmpty(parameters["file"]))
                {
                    FileInfo finfo = new FileInfo(parameters["file"].Replace("%CurrentDirectory%", Directory.GetCurrentDirectory() + @"\"));
                    if (OnFile != null)
                        OnFile(new DoctorFileArgs(finfo, tpl.ToString()));
                }
            }
        }
    }
}