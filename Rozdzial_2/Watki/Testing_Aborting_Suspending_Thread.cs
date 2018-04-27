using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Watki
{
    public class Testing_Aborting_Suspending_Thread
    {
        public void ThreadSuspending()
        {
            var pi = new LiczbaPi_UzycieKlasyThread();
            var thread = new Thread(pi.UruchamianieObliczenPi);
            thread.Start();
            Thread.Sleep(300);
            thread.Suspend();
            Console.WriteLine("Naciśnij Enter, aby kontynuować działanie wątku...");
            Console.ReadLine();
            thread.Resume();
        }
    }
}
