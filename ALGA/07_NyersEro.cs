using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int Egesz { get; }
        public int Wmax { get; }
        public int[] W { get; }
        public float[] P { get; }

        public HatizsakProblema(int egesz, int wmax, int[] w, float[] p)
        {
            Egesz = egesz;
            Wmax = wmax;
            W = w;
            P = p;
        }
        public int OsszSuly(bool[] t)
        {
            int ossz = 0;
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == true)
                {
                    ossz += W[i];
                }

            }
            return ossz;
        }
        public float OsszErtek(bool[] t)
        {
            float ossz = 0;
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == true)
                {
                    ossz += P[i];
                }
            }
            return ossz;
        }
        public bool Ervenyes(bool[] t)
        {
            return OsszSuly(t) <= Egesz;
        }
    }
    public class NyersEro<T>
    {
        int m;
        Func<int, T> generator;
        Func<T, float> josag;
        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }
        public T OptimalisMegoldas()
        {
            T o = generator(1);
            LepesSzam = 0;
            for (int i = 2; i < m; i++)
            {
                T x = generator(i);
                if (josag(x) > josag(o))
                {
                    
                    o = x;
                }
                LepesSzam++;

            }
            return o;
        }
    }
    public class NyersEroHatizsakPakolas
    {
        HatizsakProblema problema;
        public int LepesSzam { get; private set; }

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }
        public bool[] Generator(int i)
        {
            int szam = i - 1;
            bool[]K = new bool[problema.Egesz];
            for (int j = 0; j < problema.Egesz; j++)
            {
                K[j] = (szam/2^(j-1)) % 2 == 1;
            }
            return K;
        }

        public float Josag(bool[] t)
        {
            if (problema.Ervenyes(t))
            {
                return problema.OsszErtek(t);
            }
            return -1;
        }

        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> e = new NyersEro<bool[]>(2^problema.Egesz, Generator, Josag);
            bool[] vissza = e.OptimalisMegoldas();
            LepesSzam = e.LepesSzam;
            return vissza;
            
        }
        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }
}
