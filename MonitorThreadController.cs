using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using System.Linq;

namespace TestFileReaderWriter
{
    public class MonitorThreadController
    {
        //public static event EventHandler<FileChanged> fileChanged;

        public MonitorThreadController(Repo repo)
        {
            this.repo = repo;
          
        }
        private Repo repo;

        public bool RetainThreading { get; set; }
        public void InitializeMonitors()
        {
            RetainThreading = true;
            int i = 0;
            foreach (var l in repo.lfiList)
            {
                Console.WriteLine("starting monitor thread");
                ThreadPool.QueueUserWorkItem(StartMonitor, l.index);
                i++;
            }
        }

        public void StartMonitor(object state)
        {
            object array = state as object;
            int fileId = Convert.ToInt32(state);
            var lfi = repo.lfiList.First(a => a.index == fileId);
            string logFileName = lfi.fullName;
            long fileLength = lfi.length;

            FileStream fs;
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"StartMonitor {st}");


            Console.WriteLine("Monitor started:");
            while (RetainThreading)
            {
                
                lock (Program.fileLock)
                {
                    using (fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        fs.Seek(fileLength, 0);
                        if (fileLength != fs.Length)
                        {
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                bool hasData = true;
                                var lines = new List<string>();
                                while (hasData)
                                {
                                    var line = sr.ReadLine();
                                    if (line == null)
                                    {
                                        hasData = false;
                                        break;
                                    }
                                    else
                                    {
                                        lines.Add(line);
                                    }
                                }
                                Console.WriteLine("notifying change:");
                                fileLength = fs.Length;
                            }
                        }
                        Thread.Sleep(200);
                    }
                }
            }
        }
    }
}


