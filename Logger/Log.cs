using System;

namespace Loger
{
    internal class Log
    {
        public string Message { get; private set; }
        public string Time { get; private set; }
                public Log(string message)
        {
            DateTime t = DateTime.Now;
            Time = t.ToShortDateString() + " " + t.ToLongTimeString();
            Message = message;
        }
    }
}
