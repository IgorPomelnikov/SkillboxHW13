using System;
using System.IO;
using System.Text;

namespace Loger
{
    public class MenuLogWriter
    {
        object o = new object();
        string _path;
        string _fileName = "LogFile.txt";
        public MenuLogWriter(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    FileStream fileStream = new FileStream(path + _fileName, FileMode.OpenOrCreate);
                    fileStream.Close();
                    _path = path;
                }
            }
            catch (Exception)
            {
                _path = "C:/Temp/";

                Directory.CreateDirectory(_path);
                File.Create(_path + _fileName);
            }
        }

        public void WriteLog(string message)
        {
            lock (o)
            {
                using (StreamWriter s = new(_path + _fileName, true, Encoding.UTF8))
                {
                    s.WriteLine($"{DateTime.Now.ToString()}: {message}");
                }
            }
        }
    }
}
