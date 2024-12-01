using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            Suly = suly;
        }
        public float Suly { get; set; }
    }
    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n;
        float[,] M;
        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = float.NaN;
                }
            }
        }
        public float CsucsokSzama => n;

        public int ElekSzama { get 
            {
                int s = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i,j] != float.NaN)
                        {
                            s++;
                        }
                    }
                }
                return s;
            }
            
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> halmaz = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    halmaz.Beszur(i);
                }
                return halmaz;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek { get 
            {
                FaHalmaz<SulyozottEgeszGrafEl> halmaz = new FaHalmaz<SulyozottEgeszGrafEl> ();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            halmaz.Beszur(new SulyozottEgeszGrafEl(i, j, M[i,j]));
                        }
                    }
                }
                return halmaz;
            } 
        }

        int Graf<int, SulyozottEgeszGrafEl>.CsucsokSzama => n;

        public float Suly(int honnan, int hova)
        {
            if (!float.IsNaN(M[honnan, hova]))
            {
                return M[honnan, hova];
            }
            throw new NincsElKivetel();
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> halmaz = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                if (!float.IsNaN(M[csucs, i]))
                {
                    halmaz.Beszur(i);
                }
            }
            return halmaz;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan,hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(M[honnan,hova]);
        }
        
    }
    public class Utkereses
    {

        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> graf, V start)
        {
            Szotar<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(graf.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(graf.CsucsokSzama);
            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(graf.CsucsokSzama, (ez, ennel) => L.Kiolvas(ez) < L.Kiolvas(ennel));

            graf.Csucsok.Bejar(x =>
            {
                L.Beir(x, float.MaxValue);
                S.Sorba(x);
            });
            L.Beir(start, 0);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                graf.Szomszedai(u).Bejar(x => 
                {
                    if ((L.Kiolvas(u) + graf.Suly(u, x)) < L.Kiolvas(x))
                    {
                        L.Beir(x, L.Kiolvas(u) + graf.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }
            return L;
        }
    }
    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E>graf, V start) where V : IComparable
        {
            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(graf.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(graf.CsucsokSzama);
            PrioritasosSor<V> S = new KupacPrioritasosSor<V>(graf.CsucsokSzama, (ez, ennel) => K.Kiolvas(ez) < K.Kiolvas(ennel));
            FaHalmaz<V> bennevan = new FaHalmaz<V>();
            graf.Csucsok.Bejar(x =>
            {
                K.Beir(x, float.MaxValue);
                P.Beir(x, default(V));
                S.Sorba(x);
                bennevan.Beszur(x);

            });
            K.Beir(start, 0);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                bennevan.Torol(u);
                graf.Szomszedai(u).Bejar(x =>
                {
                    if (bennevan.Eleme(x) && graf.Suly(u, x) < K.Kiolvas(x))
                    {
                        K.Beir(x, graf.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }
            return P;
        }
        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> graf) where E : SulyozottGrafEl<V>, IComparable where V : IComparable
        {
            FaHalmaz<E> A = new FaHalmaz<E>();
            Szotar<V, FaHalmaz<V>> halmazok = new HasitoSzotarTulcsordulasiTerulettel<V, FaHalmaz<V>>(graf.CsucsokSzama);
            PrioritasosSor<E> S = new KupacPrioritasosSor<E>(graf.ElekSzama, (ez, ennel) => ez.Suly < ennel.Suly);
            graf.Csucsok.Bejar(x =>
            {
                var halmaz = new FaHalmaz<V>();
                halmaz.Beszur(x);
                halmazok.Beir(x, halmaz);
            });
            graf.Elek.Bejar(el => S.Sorba(el));
            while (!S.Ures)
            {
                E u = S.Sorbol();
                var i = u.Honnan;
                var j = u.Hova;
                if (halmazok.Kiolvas(i) != halmazok.Kiolvas(j))
                {
                    A.Beszur(u);
                    var halmaz1 = halmazok.Kiolvas(i);
                    var halmaz2 = halmazok.Kiolvas(j);
                    halmaz1.Bejar(x =>
                    {
                        halmazok.Beir(x, halmaz2);
                        halmaz2.Beszur(x);
                    });
                }
                
            }
            return A;
        }
    }
}