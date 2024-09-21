using OE.ALGA.Paradigmak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igen
{
    namespace OE.ALGA.Paradigmak 
    {
        public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
        {

            public Func<T, bool> BejaroFeltetel { get; set; }
            public FeltetelesFeladatTarolo(int lnt) : base(lnt)
            {
            }

            public void FeltetelesVegrehajtas(Func<T, bool> feltetel)
            {
                for (int i = 0; i < n; i++) 
                {
                    if (feltetel(tarolo[i]))
                    {
                        tarolo[i].Vegrehajtas();
                    }
                }
            }
            public IEnumerator<T> GetEnumerator()
            {
                Func<T, bool> ABejaroFeltetel = BejaroFeltetel ?? (_ => true);
                return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, ABejaroFeltetel);
            }



        }
        class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
        {
            T[] tarolo;
            int n;
            int aktualis = -1;
            Func<T, bool> feltetel;

            public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> feltetel)
            {
                this.tarolo = tarolo;
                this.n = n;
                this.feltetel = feltetel;
            }
            public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n)
            {
                this.tarolo = tarolo;
                this.n = n;
                this.feltetel = feltetel;
            }

            public T Current 
            { get 
                {
                    return tarolo[aktualis];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }


            //[1,2,3,4] !!!!!!!!!!!!!!
            public bool MoveNext()
            {
                for (int i = aktualis + 1; i < n; i++)
                {
                    if (feltetel(tarolo[i]))
                    {
                        aktualis = i;
                        return true;
                    }
                }
                return false;
            }

            public void Reset()
            {
                aktualis = -1;
            }
        }
    }
    
}
