using Simulations.Elements.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using static Simulations.Elements.Items.Human;
using Item = Simulations.Elements.Items.Item;

namespace Simulations.Items {
    internal class Board {

        internal int rows;
        internal int cols;
        private readonly Tile[,] tiles;

        internal Tile this[int row, int col] {
            get => tiles[row, col];
            set => tiles[row, col] = value;
        }

        internal Board(int rows, int cols) {
            
            this.rows = rows;
            this.cols = cols;
            tiles = new Tile[rows, cols];
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    tiles[r, c] = new Tile(r, c);
                }
            }
        }

        internal Tile NextTile(Tile tile, Move move) {
            int r = tile.row + move.r;
            int c = tile.col + move.c;
            if (IsIndexInsideBoard(r, c)) {
                return this[r, c];
            }
            return null;

        }

        private bool IsIndexInsideBoard(int r, int c) {
            return r != rows && r != -1 && 
                   c != cols && c != -1;
        }

        internal List<T> GenerateItems<T>(Type type, int noItems) where T:Item {
            int created = 0;
            List<T> items = new List<T>();
            while (created != noItems) {
                int c = Tools.DotNetTools.rnd.Next(0, cols);
                int r = Tools.DotNetTools.rnd.Next(0, rows);
                if (tiles[r, c].IsEmpty()) {
                    Item item = (type == typeof(Human)) ? (Item)new Human(tiles[r, c]) :
                                                          (Item)new Fruit(tiles[r, c]);
                    tiles[r, c].AddContent(item);
                    items.Add((T)item);
                    created++;
                }
            }
            return items;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void PrintBoard() {
            Console.WriteLine();
            Console.WriteLine(new string('-', 2 * cols + 1));
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    Console.Write($"|{tiles[r, c]}");
                }
                Console.WriteLine("|");
                Console.WriteLine(new string('-', 2 * cols + 1));
            }
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
