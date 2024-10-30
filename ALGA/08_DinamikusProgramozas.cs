using OE.ALGA.Adatszerkezetek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        HatizsakProblema problema;

        public int LepesSzam { get; private set; }
        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }
        public int[,] TablazatFeltoltes()
        {
            int[,] F = new int[problema.Egesz+1, problema.Wmax+1];
            for (int i = 0; i <= problema.Egesz; i++)
            {
                F[i, 0] = 0;
            }
            for (int i = 0; i <= problema.Wmax; i++)
            {
                F[0, i] = 0;
            }
            for (int i = 1; i <= problema.Egesz; i++)
            {
                for (int j = 1; j <= problema.Wmax; j++)
                {
                    if (j >= problema.W[i-1])
                    {
                        F[i, j] = Math.Max(F[i - 1, j], (F[i - 1, j- problema.W[i-1]] + (int)problema.P[i-1]));
                    }
                    else
                        F[i, j] = F[i - 1, j];
                }
                LepesSzam++;
            }
            return F;
        }
        public int OptimalisErtek()
        {
            return TablazatFeltoltes()[problema.Egesz, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            int[,] F = TablazatFeltoltes();
            bool[] O = new bool[F.GetLength(0)-1];
            for (int z = 0; z < F.GetLength(0)-1; z++)
            {
                O[z] = false;
            }
            int t = problema.Egesz;
            int h = problema.Wmax;
            while (t > 0 || h > 0)
            {
                if (F[t, h] != F[t-1, h])
                {
                    O[t-1] = true;
                    h = h - problema.W[t -1];
                }
                t--;
                LepesSzam++;
            }
            return O;
        }
    }
}
