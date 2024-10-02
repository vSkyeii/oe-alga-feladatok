using OE.ALGA.Adatszerkezetek;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T> kov;

        public LancElem(T tart, LancElem<T> kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }
    public class LancoltVerem<T> : Verem<T>
    {

        LancElem<T> fej;

        public LancoltVerem()
        {
            this.fej = null;
        }

        public bool Ures
        {
            get
            {
                if (fej == null)
                {
                    return true;
                }
                return false;
            }
        }

        public T Felso()
        {
            if (fej != null) return fej.tart;
            throw new NincsElemKivetel();
        }

        public void Verembe(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);
            fej = uj;

        }

        public T Verembol()
        {
            if (fej != null)
            {
                T ertek = fej.tart;
                LancElem<T> q = fej;
                fej = fej.kov;
                return ertek;
            }
            else
                throw new NincsElemKivetel();
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        LancElem<T> fej;
        LancElem<T> vege;

        public LancoltSor()
        {
            this.fej = null;
            this.vege = null;
        }

        public bool Ures => fej == null;
        

        public T Elso()
        {
            if (fej != null)
            {
                return fej.tart;
            }
            throw new NincsElemKivetel();
        }

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (vege != null)
            {
                vege.kov = uj;
            }
            else
                fej = uj;
            vege = uj;
        }

        public T Sorbol()
        {
            if (fej != null)
            {
                T ertek = fej.tart;
                LancElem<T> q = fej;
                fej = fej.kov;
                if (fej == null)
                {
                    vege = null;
                }
                return ertek;


            }
            throw new NincsElemKivetel();

        }
    }

    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        LancElem<T> fej;
        public int Elemszam { get; private set; }

        public LancoltLista()
        {
            this.fej = null;
            Elemszam = 0;
        }

        public void Bejar(Action<T> muvelet)
        {
            LancElem<T> p = fej;
            while (p != null)
            {
                muvelet(p.tart);
                p = p.kov;
            }
        }

        public void Hozzafuz(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (fej == null)
            {
                fej = uj;
                Elemszam++;
            }
            else
            {
                LancElem<T> p = fej;
                while (p.kov != null)
                {
                    p = p.kov;
                }
                p.kov = uj;
                Elemszam++;
            }
            
        }

        public T Kiolvas(int index)
        {
            LancElem<T> p = fej;
            int i = 0;
            while (p.kov != null && i < index)
            {
                p = p.kov;
                i++;
            }
            if (p != null)
            {
                return p.tart;
            }
            throw new NincsElemKivetel();

        }

        public void Modosit(int index, T ertek)
        {
            LancElem<T> p = fej;
            int i = 0;
            while (p.kov != null && i < index)
            {
                p = p.kov;
                i++;
            }
            if (p != null)
            {
                p.tart = ertek;
            }
            else
                throw new NincsElemKivetel();
        }

        public void Torol(T ertek)
        {
            LancElem<T> p = fej;
            LancElem<T> e = null;
            while (p.kov != null)
            {
                while (p.kov != null && !(p.tart.Equals(ertek)))
                {
                    e = p;
                    p = p.kov;
                }
                if (p != null)
                {
                    LancElem<T> q = p.kov;
                    if (e == null)
                    {
                        fej = q;
                        Elemszam--;
                    }
                    else
                    {
                        e.kov = q;
                        Elemszam--;
                    }
                        

                    p = q;
                }
            } 
        }
        public void Beszur(int index, T ertek)
        {
            if (fej == null || index == 0)
            {
                LancElem<T> uj = new LancElem<T>(ertek, null);
                if (fej != null)
                {
                    LancElem<T> elso = fej;
                    fej = uj;
                    uj.kov = elso;
                }
                
                fej = uj;
                Elemszam++;
                
            }
            else
            {
                LancElem<T> p = fej;
                int i = 1;
                while (p.kov != null && i < index)
                {
                    p = p.kov;
                    i++;
                }
                if (i <= index)
                {
                    LancElem<T> uj = new LancElem<T>(ertek, p.kov);
                    p.kov = uj;
                }
                else
                    throw new NincsElemKivetel();


            }


        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        LancElem<T> fej;
        LancElem<T> aktualis;


        public LancoltListaBejaro(LancElem<T> fej)
        {
            aktualis = null;
            this.fej = fej;
        }

        public T Current => aktualis.tart;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (aktualis == null && fej != null)
            {
                aktualis = fej;
                return true;
            }
            if (aktualis != null && aktualis.kov != null)
            {
                aktualis = aktualis.kov;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            aktualis = null;
        }
    }
}
