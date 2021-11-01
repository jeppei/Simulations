using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Simulations.Elements.Items.Human;

namespace Simulations.Elements.Items {
    internal abstract class Item {
        internal Tile tile;

        internal Item(Tile tile) {
            this.tile = tile;
        }
    }
}
