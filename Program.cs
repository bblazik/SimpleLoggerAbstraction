using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            var flogger = new FileLogger("") as ILogger;
            var clogger = new ConsoleLogger() as ILogger;
            var loggersComposite = new LoggerComposite();
            loggersComposite.Subscribe(flogger, clogger).Write("");
        }
    }

    class LoggerComposite : ILogger
    {
        public List<ILogger> Loggers { get; set; } = new List<ILogger>();

        public LoggerComposite Subscribe(params ILogger[] logger)
        {
            Loggers.AddRange(logger);
            return this;
        }

        public LoggerComposite Unsubscribe(ILogger logger)
        {
            Loggers.Remove(logger);
            return this;
        }

        public void Write(string s)
        {
            Loggers.ForEach(l => l.Write(s));
        }
    }

    interface ILogger
    {
        void Write(string s);
    }

    class ConsoleLogger : ILogger
    {
        public void Write(string s)
        {
            Console.WriteLine(s);
        }
    }

    class FileLogger : ILogger
    {
        public string Path { get; private set; }

        public FileLogger(string path)
        {
            Path = path;
        }
        
        public void Write(string s)
        {
            File.WriteAllText(Path, s);
        }
    }
}
