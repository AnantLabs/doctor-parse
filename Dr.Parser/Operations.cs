using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Dr.Pacients;

namespace Dr.Parser
{
    public class Operations : Settings 
    {
        List<YamlNode> m_Operations = new List<YamlNode>();
        public Operations(string source)
        {
            TextReader reader = null;
            try
            {
                reader = new StringReader(source);
                Parse(ref reader);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        public Operations(TextReader source)
        {
            try
            {
                Parse(ref source);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (source != null)
                    source.Close();
            }
        }
        public Operations(FileInfo source)
        {
            if (!source.Exists)
            {
                return;
            }

            TextReader reader = null;
            try
            {
                reader = new StreamReader(source.FullName);
                Parse(ref reader);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public List<YamlNode> List
        {
            get { return m_Operations; }
        }

        private void Parse(ref TextReader reader)
        {
            YamlNode node = null;
            
            String sequence = string.Empty;
            String name = string.Empty;
            bool IsParametr = false;

            StringBuilder line = new StringBuilder();
            while (reader.Peek() >= 0)
            {
                line.Remove(0, line.Length);
                line.AppendLine(reader.ReadLine());

                Match match = Regex.Match(line.ToString(),@"^(.*?):");
                if (match.Success)
                {
                    name = match.Value.Replace(":", "").Trim();
                    if ((match.Value.Substring(0, 1) != " ") && (match.Value.Substring(0, 1) != "-"))
                    {
                        sequence = name;
                        continue;
                    }
                }
                String value = line.Remove(match.Index, match.Length).ToString();
                if (match.Success)
                {
                    if (name.Contains("-"))
                    {
                        IsParametr = true;
                        node.Parameters.Add(name.Replace("-", "").Trim(), value.Trim());
                    }
                    else
                    {
                        IsParametr = false;
                        node = new YamlNode(name);
                        node.Value = value.Trim();
                         switch (sequence)
                        {
                            case "Settings":
                                m_Settings.Add(node.Name,node);
                                break;
                            case "Operations":
                                m_Operations.Add(node);
                                break;
                        }
                    }
                }
                else
                {
                    if (IsParametr)
                        node.Parameters[name.Replace("-", "").Trim()] += value;
                    else
                        node.Value += value;
                }
                line.Remove(0, line.Length);
            } //while (reader.Peek() >= 0)
        }

    }
}