using System;
using System.Threading;
using Watki;

namespace ThreadingBookSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Rozdzial2();

            Console.ReadLine();
        }

        private static void Rozdzial2()
        {
            var piSync = new LiczbaPi_Synchronicznie();
            piSync.UruchamianieObliczenPi();

            var piThread = new LiczbaPi_UzycieKlasyThread();
            var thread = new Thread(piThread.UruchamianieObliczenPi);
            thread.Start();
            Thread.Sleep(1000);
            thread.Abort();
            Console.WriteLine("Napis po wywołaniu thread.Start();");

            var suspending = new Testing_Aborting_Suspending_Thread();
            suspending.ThreadSuspending();

            var wieleWatkow = new LiczbaPi_WieleWatkow();
            wieleWatkow.Main();

            var joining = new LiczbaPi_Join();
            joining.Run();

            var threadPooling = new LiczbaPi_ThreadPool();
            threadPooling.Run();
        }
    }
}
