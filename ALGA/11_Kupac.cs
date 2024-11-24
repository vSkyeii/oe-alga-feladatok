using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritas;

        public Kupac(T[] e, int n, Func<T,T, bool> nagyobbPrioritas)
        {
            E = e;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }
        static protected int Bal(int i) { return 2 * i; }
        static protected int Jobb(int i) { return (2 * i)+1; }
        static public int Szulo(int i) { return i/2; }
        protected void Kupacol(int i)
        {
            int b = Bal(i);
            int j = Jobb(i);
            int max = i;
            if (b < n && nagyobbPrioritas(E[b], E[i]))
            {
                max = b;
            }
            if (j<n && nagyobbPrioritas(E[j], E[max]))
            {
                max = j;
            }
            if (max != i)
            {
                T sw = E[i];
                E[i] = E[max];
                E[max] = sw;
                Kupacol(max);
            }
        }
        protected void KupacotEpit()
        {
            for (int i = (n/2)-1; i >=0; i--)
            {
                Kupacol(i);
            }
        }
    }
    public class KupacRendezes<T> : Kupac<T> where T : IComparable
    {
        public KupacRendezes(T[] e) : base(e, e.Length, (x, y) => x.CompareTo(y) >0 )
        {

        }
        public void Rendezes()
        {
            KupacotEpit();
            for (int i = n-1; i >=0; i--)
            {
                T sw = E[0];
                E[0] = E[i];
                E[i] = sw;
                n--;
                Kupacol(0);
            }
        }
    }
    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int n, Func<T, T, bool> nagyobbPrioritas) : base(new T[n], 0, nagyobbPrioritas)
        {
        }

        public bool Ures => n == 0;

        public void KulcsotFelvisz(int i)
        {
            int sz = Szulo(i);
            if (sz>= 0 && nagyobbPrioritas(E[i], E[sz]))
            {
                T sw = E[i];
                E[i]  = E[sz];
                E[sz] = sw;
                KulcsotFelvisz(sz);
            }
        }
        public T Elso()
        {
            if (!Ures)
            {
                return E[0];
            }
            else
                throw new NincsElemKivetel();
        }

        public void Frissit(T elem)
        {
            int i = 0;
            while (i < n && !(E[i].Equals(elem)))
            {
                i++;
            }
            if (i<n)
            {
                KulcsotFelvisz(i);
                Kupacol(i);
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                n++;
                E[n-1] = ertek;
                KulcsotFelvisz(n-1);
                
                
            }
            else
                throw new NincsHelyKivetel();
        }

        public T Sorbol()
        {
            if (!Ures)
            {
                T sw = E[0];
                E[0] = E[n-1];
                E[n-1] = sw;
                n--;
                Kupacol(0);
                return sw;
            }
            else
                throw new NincsElemKivetel();
        }
    }
}
