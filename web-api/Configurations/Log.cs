using System;
using System.IO;

namespace web_api.Configurations
{
    public class Log
    {
        public static string getFullPath()
        {
            string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            string path = System.Configuration.ConfigurationManager.AppSettings["loja-path-log-file"];
            string fullPath = Path.Combine(path, fileName);
         
            return fullPath;
        }
    }
}