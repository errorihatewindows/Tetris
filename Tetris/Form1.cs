using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
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

        char lastInput = '.';

        SoundPlayer Music = new SoundPlayer();
        SoundPlayer Sounds = new SoundPlayer();



        public Form1()
        {
            InitializeComponent();
            game = new Game(this);

            DoubleBuffered = true;
        }

        //Returns last KeyStroke
        public char get_Input()
        {
            char tempInput = lastInput;
            //Clear last Key Input
            lastInput = '.';

            //Return last Input
            return tempInput;
        }

        //plays Sounds
        public void playSound(string Sound)
        {
            //TODO: multiple WAV audio
        }

        //draws full Board
        public void Draw_Board(Graphics l)
        {
            Board board = game.GetBoard();
            //draw BG for playing Area
            Draw_Background(l);

            display_score(game.GetScore(), l);

            //Draws each Piece int the Board
            foreach (KeyValuePair<Piece, char> position in board)
            {
                if (position.Key.Item1 < 0 || position.Key.Item1 > 9 || position.Key.Item2 < 0 || position.Key.Item2 > 19) { continue; }
                    
                Draw_Piece(position.Key.Item1, position.Key.Item2, board[position.Key], l);
            }

        }

        //draws Pieces on given Board
        private void Draw_Piece(int x, int y, char Color, Graphics l)
        {

            //relative in absolute Koordinaten
            x = (x * 30) + (Size.Width / 2 - 150);
            y = ((19 - y) * 30) + 100;

            if (Color == 'r')
                l.DrawImage(red, x, y);
            if (Color == 'b')
                l.DrawImage(blue, x, y);
            if (Color == 'y')
                l.DrawImage(yellow, x, y);
            if (Color == 'g')
                l.DrawImage(green, x, y);
            if (Color == 'o')
                l.DrawImage(orange, x, y);
            if (Color == 'p')
                l.DrawImage(purple, x, y);
            if (Color == 'l')
                l.DrawImage(lightblue, x, y);
        }

        //Draws BG
        private void Draw_Background(Graphics l)
        {

            //BG Color
            Brush brush = new SolidBrush(Color.FromArgb(6, 26, 51));
            l.FillRectangle(brush, (Size.Width / 2 - 150), 100, 300, 600);

            //Draw BG Grid
            Pen pen = new Pen(Color.FromArgb(1, 35, 94), 2);
            for (int j = 100; j < 700; j += 30)
                l.DrawLine(pen, (Size.Width / 2 - 150), j, (Size.Width / 2 + 150), j);
            for (int i = (Size.Width / 2 - 150); i < (Size.Width / 2 + 150); i += 30)
                l.DrawLine(pen, i, 100, i, 700);

            //Load UI
            l.DrawImage(Interface, (Size.Width / 2 - (150 + 191)), (100 - 17));
        }

        //Displays Socre
        public void display_score(int score, Graphics l)
        {
            string Score = Convert.ToString(score);

            for (int i = 0; i < Score.Length; i++)
            {
                //Load Image
                Bitmap Num = new Bitmap(Score[i].ToString() + ".png");
                //Scale Down image
                Bitmap newNum = new Bitmap(Num, new Size(20, 20));
                
                l.DrawImage(newNum, (Size.Width / 2) + 180 + (25 * i), 602); 
            }
        }

        //Glow animation
        public void Tile_glow(int x, int y)
        {
            Graphics glow = CreateGraphics();
            string color = "";
            
            Board board = game.GetBoard();
            Piece position = Tuple.Create(x, y);

            //relative in absolute Koordinaten
            x = (x * 30) + (Size.Width / 2 - 150);
            y = ((19 - y) * 30) + 100;

            if (board[position] == 'l')
                color = "lightblue";
            if (board[position] == 'b')
                color = "blue";
            if (board[position] == 'g')
                color = "green";
            if (board[position] == 'o')
                color = "orange";
            if (board[position] == 'p')
                color = "purple";
            if (board[position] == 'y')
                color = "yellow";
            if (board[position] == 'r')
                color = "red";

            Bitmap Picture;
            for (int i = 1; i < 21; i++)
            {      
                Picture = new Bitmap("Glow\\" + color + " (" + i.ToString() + ").png");
                glow.DrawImage(Picture, x, y);
                Invalidate(new Rectangle(0, 0, 300, 300));
                util.wait(75);
            }         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           game.start_timer();          
        }

        //Toggle Fullscreen F11
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Store Users last Input
            if (e.KeyCode == Keys.Up)
                lastInput = 'U';
            if (e.KeyCode == Keys.Down)
                lastInput = 'D';
            if (e.KeyCode == Keys.Right)
                lastInput = 'R';
            if (e.KeyCode == Keys.Left)
                lastInput = 'L';

            //Toggle Fullscreen
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

        //Users Last Input
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Store Users last Input
            if (e.KeyChar == (char)Keys.Up)
                lastInput = 'U';
            if (e.KeyChar == (char)Keys.Down)
                lastInput = 'D';
            if (e.KeyChar == (char)Keys.Right)
                lastInput = 'R';
            if (e.KeyChar == (char)Keys.Left)
                lastInput = 'L';
        }
    }
}
