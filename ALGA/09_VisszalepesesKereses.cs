using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        protected int n;
        protected int[] M;
        protected T[,] R;
        protected Func<int, T, bool> ft;
        protected Func<int, T, T[], bool> fk;
        protected Func<T[], float> josag;

        public VisszalepesesOptimalizacio(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            M = m;
            R = r;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }
        public int LepesSzam { get; private set; }

        public void BackTrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            if (szint == n) return;
            int i = -1;
            while (i  < M[szint]-1)
            {
                LepesSzam++;
                i++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint,i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n-1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                for (int z = 0; z < E.Length; z++)
                                {
                                    O[z] = E[z];
                                }
                                
                            }
                            van = true;


                        }
                        else
                        {
                            BackTrack(szint + 1, ref E, ref van, ref O);
                        }
                            
                        
                    }
                }
                
            }
        }
        public T[] OptimalisMegoldas()
        {
            T[] O = new T[n];
            T[] E = new T[n];
            bool van = false;
            BackTrack(0, ref E, ref van, ref O);
            return O;
        }
    }
    public class VisszalepesesHatizsakPakolas 
    {
        protected HatizsakProblema problema;

        public VisszalepesesHatizsakPakolas(HatizsakProblema p)
        {
            this.problema = p;
        }
        public bool ft(int szint, bool lehet)
        {
            return true;
        }
        public bool fk(int szint, bool lehet, bool[] E)
        {
            int ossz = 0;
            for (int i = 0; i < problema.Egesz; i++)
            {
                if (E[i] || (i == szint && lehet)) 
                {
                    ossz += problema.W[i];
                }
            }

            return ossz <= problema.Wmax;
        }
        public float josag(bool[] E)
        {
            return problema.OsszErtek(E);
        }
        public int LepesSzam { get; private set; }
        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.Egesz];
            bool[,] R = new bool[problema.Egesz,2];
            for (int i = 0; i < problema.Egesz; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }
           
            VisszalepesesOptimalizacio<bool> opt = new VisszalepesesOptimalizacio<bool>(problema.Egesz, M, R, ft, fk, josag);
            int n = problema.Egesz;
            bool[] E = new bool[n];
            bool[] O = new bool[n];
            bool van = false;
            opt.BackTrack(0, ref E, ref van, ref O);
            return O;
            

        }
        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }

    }
    public class SzetvalasztasEsKorlatozas<T> : VisszalepesesOptimalizacio<T>
    {
        public SzetvalasztasEsKorlatozas(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag, Func<int, T[], float> fb) : base(n, m, r, ft, fk, josag)
        {
            this.fb = fb;
        }
        Func<int, T[], float> fb;

        public void BackTrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            int i= -1;
            while (i < M[szint]-1)
            {
                i++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                for (int z = 0; z < E.Length; z++)
                                {
                                    O[z] = E[z];
                                }

                            }
                            van = true;


                        }
                        else
                        {
                            if (josag(E)+fb(szint, E) > josag(O))
                            {
                                BackTrack(szint + 1, ref E, ref van, ref O);
                            }
                            
                        }
                    }
                }
            }
        }
    }
    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema p) : base(p)
        {
           
        }
        public float fb(int szint, bool[] E)
        {
            float ossz = 0;
            for (int i = szint+1; i < problema.Egesz; i++)
            {
                ossz += problema.P[i];
            }
            return ossz;
        }
        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.Egesz];
            bool[,] R = new bool[problema.Egesz, 2];
            for (int i = 0; i < problema.Egesz; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            SzetvalasztasEsKorlatozas<bool> opt = new SzetvalasztasEsKorlatozas<bool>(problema.Egesz, M, R, ft, fk, josag, fb);
            int n = problema.Egesz;
            bool[] E = new bool[n];
            bool[] O = new bool[n];
            bool van = false;
            opt.BackTrack(0, ref E, ref van, ref O);
            return O;


        }
    }
}
