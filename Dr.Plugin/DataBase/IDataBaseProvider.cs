using System;
using System.Collections.Generic;
using System.Text;

namespace Dr.DataBase
{
    public interface IDataBaseProvider
    {
        bool Execute(string query);
    }
}
