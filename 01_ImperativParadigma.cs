using System;
namespace OE.ALGA.Paradigmak

    
{
    public class TárolóMegteltKivéte : Exception 
    {

    }
    public class IVégrehajtható
    {
        public void Végrehajtás
        {

        }
    }
    public class FeladatTároló<T> where T : IVégrehajtható
    {
        T[] tároló;
        int n;


        public FeladatTároló(int lnt)
        {
            tároló = new T[lnt];
            n = 0;
        }
        public void Felvesz(T elem)
        {
            if(n<= tároló.Length - 1)
            {
                tároló[n] = elem;
                n++;
                return;
            }
            throw new TárolóMegteltKivéte();
        }
        public void MindentVégrehajt()
        {
            foreach (var item in tároló)
            {
                item.Végrehajtás();
            }
        }
    }
}
	
