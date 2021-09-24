using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SkillboxHW13
{
    public class MenuLogWriter
    {
        public MenuLogWriter(string path)
        {
            if (!File.Exists(path))
            {
                File.Create("LogFile.json");
            }
        }
    }
}
