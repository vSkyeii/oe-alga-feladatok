using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }
    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        SzotarElem<K, T>[] E;
        Func<K, int> h;
        LancoltLista<SzotarElem<K, T>> U;

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            U = new LancoltLista<SzotarElem<K, T>>();
            h = (x => Math.Abs(hasitoFuggveny(x) % E.Length));
        }
        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x => x.GetHashCode())
        {
        }

        private SzotarElem<K,T> KulcsKeres(K kulcs)
        {
            
            if (E[h(kulcs)] != null && E[h(kulcs)].kulcs.Equals(kulcs))
            {
                return E[h(kulcs)];
            }
            else
            {
                if (U != null)
                {
                    SzotarElem<K, T> e = null;
                    U.Bejar(x =>
                    {
                        if (x.kulcs.Equals(kulcs))
                        {
                            e = x;
                        }
                    });
                    if (e != null) return e;
                }
                
            }
            return null;
        }
        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T> letezik = KulcsKeres(kulcs);
            if (letezik != null)
            {
                letezik.tart = ertek;
            }
            else
            {
                SzotarElem<K, T> uj = new SzotarElem<K, T>(kulcs, ertek);
                if (E[h(kulcs)] == null)
                {
                    E[h(kulcs)] = uj;
                }
                else
                    U.Hozzafuz(uj);
            }
            

        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T> letezik = KulcsKeres(kulcs);
            if (letezik != null)
            {
                return letezik.tart;
            }
            throw new HibasKulcsKivetel();
        }

        public void Torol(K kulcs)
        {
            if (E[h(kulcs)] != null && E[h(kulcs)].kulcs.Equals(kulcs))
            {
                E[h(kulcs)] = null;
            }
            else
            {
                SzotarElem<K, T> e = null;
                U.Bejar(x =>
                {
                    if (x.kulcs.Equals(kulcs))
                    {
                        e = x;
                    }
                });
                if (e != null) U.Torol(e);
            }
        }
    }
}
