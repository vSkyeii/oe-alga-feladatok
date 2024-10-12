using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> where T : IComparable
    {
        public T tart;
        public FaElem<T> bal;
        public FaElem<T> jobb;

        public FaElem(T tart, FaElem<T> bal, FaElem<T> jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }
    public class FaHalmaz<T> : Halmaz<T> where T : IComparable
    {
        FaElem<T> gyoker;

        public FaHalmaz()
        {
        }
        protected static FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null) return new FaElem<T>(ertek, null, null);
            else
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabaBeszur(p.jobb, ertek);
                    }
                }
                   
            }
            return p;
        }
        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        private static bool ReszFaEleme(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    return ReszFaEleme(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        return ReszFaEleme(p.jobb, ertek);
                    }
                    else
                        return true;
                }

            }
            return false;
        }

        public bool Eleme(T ertek)
        {
            return ReszFaEleme(gyoker, ertek);
        }
        private static FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else 
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabolTorol(p.jobb, ertek);
                    }
                    else
                    {
                        if (p.bal == null)
                        {
                            p = p.jobb;

                        }
                        else
                        {
                            if (p.jobb == null)
                            {
                                p = p.bal;
                            }
                            else
                                p.bal = KetGyerekesTorles(p, p.bal);
                        }
                    }

                }
                return p;

            }
            throw new NincsElemKivetel();
        }
        private static FaElem<T> KetGyerekesTorles(FaElem<T> e, FaElem<T> r)
        {
            if (r.jobb != null)
            {
                r.jobb = KetGyerekesTorles(e, r.jobb);
                return r;
            }
            e.tart = r.tart;
            r = r.bal;
            return r;
        }

        public void Torol(T ertek)
        {
            ReszfabolTorol(gyoker, ertek);
        }

        private void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }
        private void ReszfaBejarasInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPreOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }
        private void ReszfaBejarasPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }

    }
}
