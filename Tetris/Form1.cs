using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Piece = System.Tuple<int, int>;
using Board = System.Collections.Generic.Dictionary<System.Tuple<int, int>, char>;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private Game game;
        Bitmap lightblue = new Bitmap(@"lightblue.png");
        Bitmap blue = new Bitmap(@"blue.png");
        Bitmap purple = new Bitmap(@"purple.png");
        Bitmap red = new Bitmap(@"red.png");
        Bitmap yellow = new Bitmap(@"yellow.png");
        Bitmap orange = new Bitmap(@"orange.png");
        Bitmap green = new Bitmap(@"green.png");

        Bitmap Interface = new Bitmap(@"UI.png");

        public Form1()
        {
            InitializeComponent();
            game = new Game(this);

            DoubleBuffered = true;

        }

        //draws full Board
        public void Draw_Board(Graphics l)
        {
            Board board = game.GetBoard();
            //draw BG for playing Area
            Draw_Background(l);

            //Draws each Piece int the Board
            foreach (KeyValuePair<Piece, char> position in board)
                Draw_Piece(position.Key.Item1, position.Key.Item2, board[position.Key], l);

        }

        //draws Pieces on given Board
        private void Draw_Piece(int x, int y, char Color, Graphics l)
        {

            //relative in absolute Koordinaten
            x = (x * 30) + (Size.Width / 2 - 150);
            y = ((19 - y) * 30) + 100;

            if (Color == 'r' || Color == 'R')
                l.DrawImage(red, x, y);
            if (Color == 'b' || Color == 'B')
                l.DrawImage(blue, x, y);
            if (Color == 'y' || Color == 'Y')
                l.DrawImage(yellow, x, y);
            if (Color == 'g' || Color == 'G')
                l.DrawImage(green, x, y);
            if (Color == 'o' || Color == 'O')
                l.DrawImage(orange, x, y);
            if (Color == 'p' || Color == 'P')
                l.DrawImage(purple, x, y);
            if (Color == 'l' || Color == 'L')
                l.DrawImage(lightblue, x, y);
        }

        //Draws BG
        private void Draw_Background(Graphics l)
        {
            //BG Color
            l.FillRectangle(Brushes.CadetBlue, (Size.Width / 2 - 150), 100, 300, 600);

            //Draw BG Grid
            Pen pen = Pens.DarkSlateGray;
            for (int j = 100; j < 700; j += 30)
                l.DrawLine(pen, (Size.Width / 2 - 150), j, (Size.Width / 2 + 150), j);
            for (int i = (Size.Width / 2 - 150); i < (Size.Width / 2 + 150); i += 30)
                l.DrawLine(pen, i, 100, i, 700);

            //Load UI
            l.DrawImage(Interface, (Size.Width / 2 - (150 + 191)), (100 - 17));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game.start_timer();
        }

        //Toggle Fullscreen F11
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    WindowState = FormWindowState.Normal;
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                WindowState = FormWindowState.Normal;
            }


        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Draw_Board(e.Graphics);
        }
    }
}
