using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulations.Elements.Items {
    class Fruit : Item{
        internal override float GetScale() => 0.5f;
        internal override Brush GetBrush() => Brushes.Red;

        internal override int GetRowOffset() => 0;
        internal override int GetColOffset() => 0;

        internal Fruit(Tile tile) : base(tile) {}
    }
}
