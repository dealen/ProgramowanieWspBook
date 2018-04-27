using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watki
{
    public class LiczbaPi_Synchronicznie
    {
        private Random random = new Random();

        public double ObliczPi(long iloscProb)
        {
            double x, y;
            long iloscTrafien = 0;

            for (long i = 0; i < iloscProb; i++)
            {
                x = random.NextDouble();
                y = random.NextDouble();
                if (x * x + y * y < 1) ++iloscTrafien;
            }
            return 4.0 * iloscTrafien / iloscProb;
        }

        public void UruchamianieObliczenPi()
        {
            int czasPoczatkowy = Environment.TickCount;

            long iloscProb = 10000000L;
            double pi = ObliczPi(iloscProb);
            Console.WriteLine($"Pi={pi}, błąd={Math.Abs(Math.PI - pi)}");

            int czasKoncowy = Environment.TickCount;
            int roznica = czasKoncowy - czasPoczatkowy;
            Console.WriteLine($"Czas obliczeń: {roznica.ToString()}");
        }
    }
}
