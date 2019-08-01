using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoxExtensions;
namespace TestFileReaderWriter
{
    public sealed class TestRepo
    {
        public TestRepo()
        {
            ThreadPool.QueueUserWorkItem(beginWriting, 1);
        }
        public void beginWriting(object o)
        {
            var st = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"{st}");
            var fn = Environment.CurrentDirectory + "\\Test\\testFile5.txt";
            File.Delete(fn);
            File.Create(fn);
            while (true)
            {
                lock (Program.fileLock)
                {
                    using (var fs = new FileStream(fn, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                    {

                        int cnt = 0;
                        if (cnt == 0)
                            fs.SetLength(0);
                        int sleepX = 100;

                        fs.Write($"write {DateTime.Now} \r\n".ToBytes());
                        fs.Flush();

                        Thread.Sleep(sleepX);
                        cnt++;
                        if (cnt == 20)
                        {
                            sleepX = 30000;
                        }
                        Console.WriteLine("writing test file");
                    }
                }

            }
            


        }
    }
}
