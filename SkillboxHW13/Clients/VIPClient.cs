﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class VIPClient : Client
    {
        public VIPClient()
        {
            CreditPercent = 5;
            DepositPercent = 15;
            Id = CommonId++;
        }
        public void OpenCreditAccount(double sum, int mounths)
        {
            base.OpenCreditAccount(this, sum, mounths);
        }
        public void OpenDepositAccount(double sum, int mounths, bool capitalized)
        {
            base.OpenDepositAccount(this,  sum,  mounths,  capitalized);
        }
    }
}
