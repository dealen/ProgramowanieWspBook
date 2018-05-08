using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Watki
{
    public class LiczbaPi_ThreadPool
    {
        private Random _random = new Random();
        private const int ileWatkow = 100;
        //private double PI = 0; // zmienna współdzielona
        private const long iloscProbWWatku = 10000000L;
        private long calkowitaIloscTrafien = 0L;

        private long całkowitaIlośćPrób = 0L;

        private EventWaitHandle[] ewt = new EventWaitHandle[ileWatkow];

        private EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset);
        
        public void Run()
        {
            int czasPoczatkowy = Environment.TickCount;

            Timer timer = new Timer((object sender) =>
            {
                Console.WriteLine($"Ilośc prób: {Interlocked.Read(ref calkowitaIloscTrafien).ToString()} / {(ileWatkow * iloscProbWWatku).ToString()}");
            }, null, 0, 1000);

            WaitCallback metodaWatku = UruchamianieObliczenPi;
            ThreadPool.SetMaxThreads(30, 100);
            for (int i = 0; i < ileWatkow; i++)
            {
                ewt[i] = new EventWaitHandle(false, EventResetMode.AutoReset);
                ThreadPool.QueueUserWorkItem(metodaWatku, i);
            }

            for (int i = 0; i < ileWatkow; i++)
            {
                ewt[i].WaitOne();
            }

            
            //czekanie na zakończenie wątków
            //int ileDostepnychWatkowPuli = 0; // nieuzywane wątki puli
            //int ileWszystkichWątkówPuli = 0; // wszystkie watki puli
            //int ileDzialajacychWatkowPuli = 0; // uzywane watki puli
            //int tmp = 0;
            //do
            //{
            //    ThreadPool.GetAvailableThreads(out ileDostepnychWatkowPuli, out tmp);6
            //    ThreadPool.GetMaxThreads(out ileWszystkichWątkówPuli, out tmp);
            //    ileDzialajacychWatkowPuli = ileWszystkichWątkówPuli - ileDostepnychWatkowPuli;
            //    Console.WriteLine($"Ilość aktywnych wątków w puli = {ileDzialajacychWatkowPuli}");
            //    Thread.Sleep(1000);
            //} while (ileDzialajacychWatkowPuli > 0);


            //PI /= ileWatkow;
            double pi = 4.0 * calkowitaIloscTrafien / (iloscProbWWatku * ileWatkow);

            //Console.WriteLine($"Wszystkie wątki zakończyły działanie.\nUśrednione Pi={PI}, błąd={Math.Abs(Math.PI - PI)}");
            Console.WriteLine($"Wszystkie wątki zakończyły działanie.\nUśrednione Pi={pi}, błąd={Math.Abs(Math.PI - pi)}");

            int czasKoncowy = Environment.TickCount;
            int roznica = czasKoncowy - czasPoczatkowy;
            Console.WriteLine($"Czas obliczeń: {roznica.ToString()}");
            timer.Change(-1, Timeout.Infinite);
            timer.Dispose();
        }

        private void UruchamianieObliczenPi(object parameter)
        {
            try
            {
                waitHandle.WaitOne(500);
                Console.WriteLine("waitHandle.WaitOne(500); ");
                int? index = parameter as int?;
                var indexValue = index.HasValue ? index.Value.ToString() : "---";
                Console.WriteLine($"Uruchamianie obliczeń, wątek nr {Thread.CurrentThread.ManagedThreadId}, index {indexValue}...");

                //long iloscProb = 1000000000L / ileWatkow;
                //double pi = ObliczPiDouble(iloscProb);
                //PI += pi;
                //Console.WriteLine($"Pi={pi}, błąd={Math.Abs(PI - pi)}, wątek nr {Thread.CurrentThread.ManagedThreadId}");

                long iloscTrafien = ObliczPiLong(iloscProbWWatku);
                Interlocked.Add(ref calkowitaIloscTrafien, iloscTrafien);

                ewt[index.Value].Set();
            }
            catch (ThreadAbortException tae)
            {
                Console.WriteLine($"Działanie wątku zostało przerwane. ({tae.Message})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyjątek. ({ex.Message})");
            }
        }

        private long ObliczPiLong(long iloscProb)
        {
            Random rnd = new Random(_random.Next() & DateTime.Now.Millisecond);
            double x, y;
            long iloscTrafien = 0;
            for (int i = 0; i < iloscProb; i++)
            {
                x = rnd.NextDouble();
                y = rnd.NextDouble();
                if (x * x + y * y < 1) ++iloscTrafien;
                Interlocked.Increment(ref calkowitaIloscTrafien);
            }
            return iloscTrafien;
        }

        private double ObliczPiDouble(long iloscProb)
        {
            Random rnd = new Random(_random.Next() & DateTime.Now.Millisecond);
            double x, y;
            long iloscTrafien = 0;
            for (int i = 0; i < iloscProb; i++)
            {
                x = rnd.NextDouble();
                y = rnd.NextDouble();
                if (x * x + y * y < 1) ++iloscTrafien;
            }
            return 4.0 * iloscTrafien / iloscProb;
        }
    }
}
