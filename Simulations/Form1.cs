using Simulations.Elements.Items;
using Simulations.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulations {
    public partial class Form1 : Form {

        // Graphics
        internal const int margin = 10;
        internal const int colSize = 50;
        internal const int rowSize = 50;
        private const int interval = 400;

        private static readonly Brush indexColor = Brushes.Gray;

        // Data
        private const int rows = 18;
        private const int cols = 20;
        private const int noHumans = 20;
        private const int noFruits = 20;

        private static Board board;
        private static List<Human> humans;


        public Form1() {
            InitializeComponent();

            board = new Board(rows, cols);
            
            humans = board.GenerateItems<Human>(typeof(Human), noHumans);
            board.GenerateItems<Fruit>(typeof(Fruit), noFruits);
            
            int height = 2*margin + rowSize*(rows+1) - 14;
            int width = 2*margin + colSize*(cols+1) - 36;
            Size = new Size(width, height);
            timer.Interval = interval;
        }

        // Graphics
        private void Form1_Paint(object sender, PaintEventArgs e) {

            Pen pen = new Pen(Brushes.Black);
            
            int height = rowSize*rows;
            int width = colSize*cols;
            for (int x = 0; x <= width; x += colSize ) { 
                e.Graphics.DrawLine(pen, new Point(margin + x, margin), new Point(margin + x, margin + height));
            }
            for (int y = 0; y <= height; y += rowSize) { 
                e.Graphics.DrawLine(pen, new Point(margin, margin + y), new Point(margin + width, margin + y));
            }
            
            Font font = new Font("Arial", 10);
            for (int r = 0; r < height; r += rowSize) {
                for (int c = 0; c < width; c +=colSize) {
                    e.Graphics.DrawString($"({r/rowSize},{c/colSize})", font, indexColor, margin + c, margin + r);
                }
            }

            foreach (Human human in humans) {
                human.MakeAMove(board);
            }

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {

                    foreach (Item item in board[r,c].content) {
                        DrawRectangle(e, item);
                    }
                }
            }
            //foreach (Human human in humans) {
            //    Brush brush = human.satisfied ? standingHumanColor : runningHumanColor;
            //    DrawRectangle(e, human);
            //}
            //
            //foreach (Fruit fruit in fruits) {
            //    DrawRectangle(e, human);
            //}
        }

        private void DrawRectangle(PaintEventArgs e, Item item) {
            
            if (item.GetScale() < 0 || 1 < item.GetScale()) throw new Exception("Scale must be between 0 and 1 inclusive");
            int xStart = item.GetColOffset() + margin + margin + (item.tile.col * colSize);
            int yStart = item.GetRowOffset() + margin + margin + (item.tile.row * rowSize);
            float boxWidth = item.GetScale() * (colSize-margin-margin);
            float boxHeight = item.GetScale() * (rowSize-margin-margin);
            e.Graphics.FillRectangle(item.GetBrush(), xStart, yStart, boxWidth, boxHeight);
            
        }

        private void Timer_tick(object sender, EventArgs e) {
            
            Invalidate();

        }
    }
}
