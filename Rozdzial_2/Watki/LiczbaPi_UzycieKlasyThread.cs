using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Watki
{
    public class LiczbaPi_UzycieKlasyThread
    {
        private Random random = new Random();

        private double ObliczPi(long iloscProb)
        {
            double x, y;
            long iloscTrafien = 0;
            for (int i = 0; i < iloscProb; i++)
            {
                x = random.NextDouble();
                y = random.NextDouble();
                if (x * x + y * y < 1) ++iloscTrafien;
            }
            return 4.0 * iloscTrafien / iloscProb;
        }

        public void UruchamianieObliczenPi()
        {
            try
            {
                int czasPoczatkowy = Environment.TickCount;
                Console.WriteLine($"Uruchamianie obliczeń, wątek nr {Thread.CurrentThread.ManagedThreadId}");

                long iloscProb = 10000000L;
                double pi = ObliczPi(iloscProb);
                Console.WriteLine($"Pi={pi}, błąd={Math.Abs(Math.PI - pi)}, wątek nr={Thread.CurrentThread.ManagedThreadId}");

                int czasKoncowy = Environment.TickCount;
                int roznica = czasKoncowy - czasPoczatkowy;
                Console.WriteLine($"Czas obliczeń: {roznica.ToString()}");
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine($"Działanie wątku zostało przerwane: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Wyjątek: {e.Message}");
            }
        }
    }
}
