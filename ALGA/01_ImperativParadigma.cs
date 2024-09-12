using System;
using System.Collections;

namespace OE.ALGA.Paradigmak
{
    public class TaroloMegteltKivetel : Exception
    {

    }
    public interface IVegrehajthato
    {
        public void Vegrehajtas()
        {
            
        }
    }
    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n;


        public FeladatTarolo(int lnt)
        {
            tarolo = new T[lnt];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n <= tarolo.Length - 1)
            {
                tarolo[n] = elem;
                n++;
                return;
            }
            throw new TaroloMegteltKivetel();
        }

        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        public IEnumerator<T> BejaroLetrehozas()
        {
            return new FeladatTaroloBejaro<T>(tarolo, n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BejaroLetrehozas();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public interface IFuggo : IVegrehajthato
    {
        public bool FuggosegTeljesul { get; }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IFuggo
    {
        public FuggoFeladatTarolo(int lnt) : base(lnt)
        {
        }
        public override void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                T vizsgalt = tarolo[i];
                if (vizsgalt.FuggosegTeljesul == true)
                {
                    vizsgalt.Vegrehajtas();
                }
            }
        }
    }



    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {

        T[] tarolo;
        int n;
        int aktualis;

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public T Current { get { return tarolo[aktualis]; } }

        T IEnumerator<T>.Current => Current;
        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (aktualis >= n - 1) return false;
            aktualis++;
            return true;
        }

        public void Reset()
        {
            aktualis = -1;
        }
    }
}
	
