﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public abstract class BankAccount
    {
        public double Count { get; protected set; } = 0;
        public DateTime Opened { get; protected set; }
        protected static int CommonId { get; set; } = 0;

        public  int Id { get; protected set; }

    }
}
