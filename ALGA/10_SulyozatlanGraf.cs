using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl : GrafEl<int>, IComparable
    {
        public int Honnan {  get; }

        public int Hova {  get; }

        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public int CompareTo(object? obj)
        {
            if (obj != null && obj is EgeszGrafEl b)
            {
                if (Honnan != b.Honnan)
                {
                    return Honnan.CompareTo(b.Honnan);
                }
                else
                {
                    return Hova.CompareTo(b.Hova);
                }
            }
            throw new Exception();
        }
    }
    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        int n;
        bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            this.M = new bool[n,n];
        }

        public int CsucsokSzama { get { return n; } }

        public int ElekSzama
        {
            get
            {
                int ossz = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i,j] == true)
                        {
                            ossz++;
                        }
                    }
                }
                return ossz;
            }
        }

        public Halmaz<int> Csucsok 
        {
            get
            {
                Halmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                Halmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j] == true)
                        {
                            elek.Beszur(new EgeszGrafEl(i, j));
                        }
                    }
                }
                return elek;
            }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            Halmaz<int> halmaz = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                if (M[csucs, i])
                {
                    halmaz.Beszur(i);
                }
            }
            return halmaz;

        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return M[honnan, hova];
        }
    }
    public class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            Halmaz<V> elert = new FaHalmaz<V>();
            Sor<V> S = new LancoltSor<V>();
            S.Sorba(start);
            elert.Beszur(start);
            while (!S.Ures)
            {
                var k = S.Sorbol();
                muvelet(k);
                FaHalmaz<V> szomszed = (FaHalmaz<V>)g.Szomszedai(k);
                szomszed.Bejar(szomszed =>
                {
                    if (!elert.Eleme(szomszed))
                    {
                        S.Sorba(szomszed);
                        elert.Beszur(szomszed);
                    }

                });

            }
            return elert;
        }
        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            Halmaz<V> elert = new FaHalmaz<V>();
            MelysegiBejarasRekurzio(g, start, elert, muvelet);
            return elert;

        }
        public static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet) where V : IComparable
        {
            F.Beszur(k);
            muvelet(k);
            FaHalmaz<V> szomszed = (FaHalmaz<V>)g.Szomszedai(k);
            szomszed.Bejar(szomszed =>
            {
                if (!F.Eleme(szomszed))
                {
                 MelysegiBejarasRekurzio(g, szomszed, F, muvelet);   
                }

            });
        }
    }
}
