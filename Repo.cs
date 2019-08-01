
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;


namespace TestFileReaderWriter
{
    public sealed class Repo
    {
        public Repo()
        {
            this.lfiList = new List<LogFileInfo>();
            this.PopulateLogFiles();
            threadController = new MonitorThreadController(this);
            threadController.InitializeMonitors();
        }
        MonitorThreadController threadController;
        public List<LogFileInfo> lfiList { get; set; }

        public void PopulateLogFiles()
        {
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Repo {st}");
            Thread.Sleep(100);
        
            lock (Program.fileLock)
            {
                StreamReader sr = new StreamReader(@"config\LogFileNames.txt");
                while (true)
                {
                    var read = sr.ReadLine();
                    if (read == null)
                        break;
                    if (read[0] == '.')
                        read = Environment.CurrentDirectory + read.Substring(1);

                    var lfi = new LogFileInfo { fullName = read };
                    lfiList.Add(lfi);

                }
                sr.Dispose();
            }
        }
    }
}
