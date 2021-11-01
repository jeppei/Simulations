using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Tools {
    internal static class DotNetTools {

        internal readonly static Random rnd = new Random();
        
        public static IList<T> Shuffle<T>(IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
