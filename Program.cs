using System;
using System.Text;

namespace TestFileReaderWriter
{
    class Program
    {
        public static readonly object fileLock = new object();
        static void Main(string[] args)
        {

            var r = new Repo();
            r.PopulateLogFiles();
            var tr = new TestRepo();
            
            var mtc = new MonitorThreadController(r);
            mtc.InitializeMonitors();

        }
       
    }
}
