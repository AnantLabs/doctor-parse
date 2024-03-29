﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dr.Pacients
{
    public interface IPacientCollection 
    {
        Pacient Next { get; }
        Pacient Prev { get; }
        int Count { get; }

        bool Save(Pacient pacient);
    }
}
