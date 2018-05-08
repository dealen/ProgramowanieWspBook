using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Watki
{
    public class LiczbaPi_Join
    {
        private Random _random = new Random();
        private const int ileWatkow = 10;
        private double PI = 0; // zmienna współdzielona

        public void Run()
        {
            int czasPoczatkowy = Environment.TickCount;

            Thread[] tt = new Thread[ileWatkow];
            for (int i = 0; i < ileWatkow; ++i)
            {
                tt[i] = new Thread(UruchamianieObliczenPi);
                tt[i].Priority = ThreadPriority.Lowest;
                tt[i].Start(i);
            }

            // czekanie na zakończenie wątków
            foreach (Thread t in tt)
            {
                t.Join();
                Console.WriteLine($"Zakończył działanie wątek o numerze {t.ManagedThreadId}");
            }
            PI /= ileWatkow;
            Console.WriteLine($"Wszystkie wątki zakończyły działanie.\nUśrednione Pi={PI}, błąd={Math.Abs(Math.PI - PI)}");

            int czasKoncowy = Environment.TickCount;
            int roznica = czasKoncowy - czasPoczatkowy;
            Console.WriteLine($"Czas obliczeń: {roznica.ToString()}");
        }

        private void UruchamianieObliczenPi(object parameter)
        {
            try
            {
                int? index = parameter as int?;
                var indexValue = index.HasValue ? index.Value.ToString() : "---";
                Console.WriteLine($"Uruchamianie obliczeń, wątek nr {Thread.CurrentThread.ManagedThreadId}, index {indexValue}...");

                long iloscProb = 1000000000L / ileWatkow;
                double pi = ObliczPi(iloscProb);
                PI += pi;
                Console.WriteLine($"Pi={pi}, błąd={Math.Abs(PI - pi)}, wątek nr {Thread.CurrentThread.ManagedThreadId}");
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

        private double ObliczPi(long iloscProb)
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
