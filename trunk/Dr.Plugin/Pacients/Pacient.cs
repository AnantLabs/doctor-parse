using System;
using System.Collections.Generic;
using System.Text;

namespace Dr.Pacients
{
    public class Pacient
    {
        StringBuilder m_Body;
        public String Body
        {
            get { return m_Body.ToString(); }
            set { m_Body = new StringBuilder(value); }
        }

        String m_Name;
        public String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public Pacient()
        {
            m_Body = new StringBuilder();
        }

        public Pacient(String body)
        {
            m_Body = new StringBuilder(body);
        }

        public byte[] ToByte()
        {
            return this.ToByte(Encoding.Default);
        }

        public byte[] ToByte(Encoding encoding)
        {
            return encoding.GetBytes(Body);
        }

        public override string ToString()
        {
            return  Body;
        }

    }
}
