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
        private const int margin = 10;
        private const int colSize = 50;
        private const int rowSize = 50;
        private const int interval = 400;

        private static readonly Brush runningHumanColor = Brushes.Black;
        private static readonly Brush standingHumanColor = Brushes.Green;
        private static readonly Brush fruitColor = Brushes.Red;
        private static readonly Brush indexColor = Brushes.Gray;

        // Data
        private const int rows = 18;
        private const int cols = 20;
        private const int noHumans = 20;
        private const int noFruits = 20;

        private static Board board;
        private readonly List<Human> humans;
        private readonly List<Fruit> fruits; 


        public Form1() {
            InitializeComponent();

            board = new Board(rows, cols);
            
            humans = board.GenerateItems<Human>(typeof(Human), noHumans);
            fruits = board.GenerateItems<Fruit>(typeof(Fruit), noFruits);
            
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

            foreach (Fruit fruit in fruits) {
                DrawRectangle(e, fruit.tile.row, fruit.tile.col, fruitColor);
            }

            foreach (Human human in humans) {
                Brush brush = human.satisfied ? standingHumanColor : runningHumanColor;
                DrawRectangle(e, human.tile.row, human.tile.col, brush);
            }
        }

        private void DrawRectangle(PaintEventArgs e, int r, int c, Brush brush) {
            
            int xStart = margin+margin + (c * colSize);
            int yStart = margin+margin + (r * rowSize);
            int boxWidth = colSize-margin-margin;
            int boxHeight = rowSize-margin-margin;
            e.Graphics.FillRectangle(brush, xStart, yStart, boxWidth, boxHeight);
        }

        private void Timer_tick(object sender, EventArgs e) {
            
            foreach (Human h in humans) {
                h.MakeAMove(board);
            }
            Invalidate();

        }
    }
}
