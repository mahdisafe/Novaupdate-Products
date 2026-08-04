using System;
using System.IO;

namespace BU50_API.BL
{
    public static class Logger
    {
        public static void LogError(string message)
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch
            {
                // Ignore errors
            }
        }
    }
}

