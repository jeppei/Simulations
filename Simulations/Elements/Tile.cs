using Simulations.Elements.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Item = Simulations.Elements.Items.Item;

namespace Simulations {

    internal class Tile {
        internal int row;
        internal int col;
        private List<Item> content = new List<Item>();

        internal Tile(int row, int col) {
            this.row = row;
            this.col = col;
        }
        
        internal void AddContent(Item item) {
            if (content.Count > 0) throw new Exception("Tiles can onl contains one item");
            content.Add(item);
        }

        internal bool IsEmpty() {
            return content.Count == 0;
        }

        internal bool TryGetItem<T>(out Item item) where T:Item{ 
            item = content.Find(x => x.GetType() == typeof(T));
            return item != null;
        }

        internal void Clear() {
            content = new List<Item>();
        }

        internal void RemoveContent(Item item) {
            content.Remove(item);
        }
    }
}
