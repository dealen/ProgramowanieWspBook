using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Watki
{
    public class LiczbaPi_WieleWatkow
    {
        private Random random = new Random();
        private int ileWatkow = 10;

        public void Main()
        {
            Thread[] tt = new Thread[ileWatkow];
            for (int i = 0; i < ileWatkow; i++)
            {
                tt[i] = new Thread(UruchamianieObliczenPi);
                tt[i].Priority = ThreadPriority.Lowest;
                tt[i].Start();
            }
        }

        private double ObliczPi(long iloscProb)
        {
            Random rnd = new Random(random.Next() & DateTime.Now.Millisecond);
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

        private void UruchamianieObliczenPi()
        {
            try
            {
                int czasPoczatkowy = Environment.TickCount;
                Console.WriteLine($"Uruchamianie obliczeń, wątek nr {Thread.CurrentThread.ManagedThreadId}");

                long iloscProb = 10000000L / ileWatkow;
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
