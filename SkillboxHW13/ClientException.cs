﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class ClientException : Exception
    {
        public override string Message { get => "Ошибка клиента!"; }
    }
}