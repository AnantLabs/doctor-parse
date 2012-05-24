using System;
using System.Collections.Generic;
using System.IO;

namespace Dr.DataBase
{
    public class MSSQLServerSettings
    {
        public String Connection
        {
            set { m_Connection = value; }
            get { return m_Connection; }
        } private String m_Connection;

        public MSSQLServerSettings()
        {

        }
    }
}