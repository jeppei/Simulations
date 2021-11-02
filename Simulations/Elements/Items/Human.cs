using Simulations.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools = Simulations.Tools.DotNetTools;

namespace Simulations.Elements.Items {


    internal class Human : Item {

        internal bool satisfied = false;
        internal override float GetScale() => 0.5f;
        internal override Brush GetBrush() => satisfied ? Brushes.Green : Brushes.Black;
        internal override int GetRowOffset() => Form1.rowSize/2 - Form1.margin;
        internal override int GetColOffset() => Form1.colSize/2 - Form1.margin;

        internal Human(Tile tile) : base(tile) {}

        internal class Move {

            internal int r;
            internal int c;
            internal double value;
            internal Move[] moveNeighbours;
            
            internal static Move UpRight   = new Move() { r = -1, c =  1 };
            internal static Move UpLeft    = new Move() { r = -1, c = -1 };
            internal static Move DownRight = new Move() { r =  1, c =  1 };
            internal static Move DownLeft  = new Move() { r =  1, c = -1 };

            internal static Move Right = new Move() { r =  0, c =  1, moveNeighbours = new Move[] { UpRight,   DownRight } };
            internal static Move Left  = new Move() { r =  0, c = -1, moveNeighbours = new Move[] { DownLeft,  UpLeft } };
            internal static Move Up    = new Move() { r = -1, c =  0, moveNeighbours = new Move[] { UpLeft,    UpRight} };
            internal static Move Down  = new Move() { r =  1, c =  0, moveNeighbours = new Move[] { DownRight, DownLeft} };

            private Move() { }
        }

        internal void MakeAMove(Board board) {
            List<Move> moves = new List<Move> { Move.Left, Move.Right, Move.Up, Move.Down };

            //moves[0].value = TunnelVision<Fruit, Human>(board, moves[0]);
            //moves[1].value = TunnelVision<Fruit, Human>(board, moves[1]);
            //moves[2].value = TunnelVision<Fruit, Human>(board, moves[2]);
            //moves[3].value = TunnelVision<Fruit, Human>(board, moves[3]);

            moves[0].value = RadiusVision<Fruit>(board, moves[0]);
            moves[1].value = RadiusVision<Fruit>(board, moves[1]);
            moves[2].value = RadiusVision<Fruit>(board, moves[2]);
            moves[3].value = RadiusVision<Fruit>(board, moves[3]);

            moves = moves.OrderBy(x => x.value).ToList();

            if (TryMove(board, moves[3])) return;
            if (TryMove(board, moves[2])) return;
            if (TryMove(board, moves[1])) return;
            if (TryMove(board, moves[0])) return;
        }

        private bool TryMove(Board board, Move move) {

            Tile nextTile = board.NextTile(tile, move);

            if (nextTile == null) return false;
            if (satisfied) return false;

            if (nextTile.IsEmpty()) {
                nextTile.AddContent(this);
                tile.RemoveContent(this);
                tile = nextTile;
                return true;
            }

            if (nextTile.TryGetItem<Fruit>(out Item fruit)) {
                nextTile.RemoveContent(fruit);

                nextTile.AddContent(this);
                tile.RemoveContent(this);
                tile = nextTile;
                satisfied = true;
                return true;
            }

            return false;
        }


        internal double TunnelVision<Want,Obstacle>(Board board, Move move) where Want:Item where Obstacle:Item{
            double value = Tools.DotNetTools.rnd.NextDouble();

            Tile currentTile = tile;
            Tile nextTile = board.NextTile(currentTile, move);

            while (nextTile != null && !nextTile.TryGetItem<Obstacle>(out Item _)) {
                if (nextTile.TryGetItem<Want>(out Item _)) value++;
                nextTile = board.NextTile(nextTile, move);
            }
            return value;
        }


        internal double RadiusVision<T>(Board board, Move move) where T:Item {
            double value = Tools.DotNetTools.rnd.NextDouble();
        
            List<Tile> currentTiles = new List<Tile>() { this.tile };
            double multiplier = Math.Pow(10, Math.Max(board.cols, board.rows));
            while (currentTiles.Count != 0) {
        
                List<Tile> nextTiles = new List<Tile>();
                Tile diag1 = board.NextTile(currentTiles.First(), move.moveNeighbours[0]);
                if (diag1 != null) {
                    nextTiles.Add(diag1);
                    if (diag1.TryGetItem<Fruit>(out Item _)) value++;
                }
                foreach (Tile tile in currentTiles) {
                    if (tile.TryGetItem<T>(out Item _)) value+=multiplier;
                    Tile nextTile = board.NextTile(tile, move);
                    if (nextTile != null) {
                        nextTiles.Add(nextTile);
                    }
                }
                Tile diag2 = board.NextTile(currentTiles.Last(), move.moveNeighbours[1]);
                
                if (diag2 != null) {
                    nextTiles.Add(diag2);
                    if (diag2.TryGetItem<T>(out Item _)) value++;
                }
                if (multiplier > 1) multiplier /= 10;
                currentTiles = nextTiles;
            }
            return value;
        }

    }
}
