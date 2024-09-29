using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> : Verem<T>
    {
        T[] E;
        int n = 0;

        public TombVerem(int length)
        {
            E = new T[length];
        }

        public bool Ures { 
            get 
            {
                return n == 0;
            } 
        }

        public T Felso()
        {
            return E[n-1];
        }

        public void Verembe(T ertek)
        {
            if (n >= E.Length) throw new NincsHelyKivetel();
            E[n++] = ertek;
        }

        public T Verembol()
        {
            if (Ures) throw new NincsElemKivetel();
            T ertek = E[n-1];
            n--;
            return ertek;
        }
    }

    public class TombSor<T> : Sor<T>
    {
        T[] E;
        int n = 0;
        int u = 0;
        int e = 0;

        public TombSor(int length)
        {
            E = new T[length];
            
        }

        public bool Ures 
        {
            get
            {
                return n == 0;
            }
        }

        public T Elso()
        {
          return E[((e+1) % E.Length)];
            
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                n++;
                u = ((u+1) % E.Length);
                E[u] = ertek;

            }
            else
                throw new NincsHelyKivetel();




        }

        public T Sorbol()
        {
            if (n > 0)
            {
                n--;
                e = ((e+1) % E.Length);
                T ertek = E[e];
                return ertek;

            }
            else
                throw new NincsElemKivetel();
        }
    }

    public class TombLista<T> : IEnumerable<T>, Lista<T>
    {
        T[] E;
        int n = 0;

        public TombLista()
        {
            E = new T[n+1];
        }

        public int Elemszam { get => n; }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (index+1 <= n + 1)
            {
                if (n == E.Length)
                {
                    T[] seged = new T[n * 2];
                    for (int i = 0; i < E.Length; i++)
                    {
                        seged[i] = E[i];
                    }
                    seged[n] = ertek;
                    E = seged;
                }
                
                for (int i = n; i > index; i--)
                {
                    E[i] = E[i - 1];
                }
                E[index] = ertek;
                n++;
            }
            else
                throw new HibasIndexKivetel();
            
        }

        

        public void Hozzafuz(T ertek)
        {

            Beszur(n, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index <= n) return E[index];
            else
                throw new HibasIndexKivetel();
        }

        public void Modosit(int index, T ertek)
        {
            if (index <= n) E[index] = ertek;
            else
                throw new HibasIndexKivetel();
        }

        public void Torol(T ertek)
        {
            int db = 0;
            for (int i = 0; i < n; i++)
            {
                
                if (E[i].Equals(ertek))
                {
                    db++;
                }
                else
                {
                    E[i - db] = E[i];
                }
            }
            n = n - db;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {

        T[] E;
        int n;
        int idx = -1;

        public TombListaBejaro(T[] e, int n)
        {
            E = e;
            this.n = n;
        }

        public T Current { get { return E[idx]; } }

        object IEnumerator.Current => Current;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (idx < n-1)
            {
                idx++;
                return true;
            }
            return false;

        }

        public void Reset()
        {
            idx = -1;
        }
    }
}
