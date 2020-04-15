using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Piece = System.Tuple<int, int>;
using Board = System.Collections.Generic.Dictionary<System.Tuple<int, int>, char>;

namespace Tetris
{
    public class Game
    {
        private int tickCount = 0;
        private const int FPS = 40;
        private Board board = new Board();
        private Timer Game_Timer = new Timer();
        private Pieces currentPiece;
        //util function classes
        Random rand = new Random();
        Form1 drawing;
        public Game(Form1 form)
        {
            drawing = form;
            //initialize game timer
            Game_Timer.Interval = (Convert.ToInt32(1000 / FPS));
            Game_Timer.Tick += new EventHandler(Game_Tick);
            Generate_Board();
        }
        //-----------------
        //getter
        public Board GetBoard()
        {
            Board tempBoard = new Board(board);
            if (currentPiece != null)
            {
                //draws Piece on the board
                foreach (Piece current in currentPiece.Blocks())
                    tempBoard[current] = currentPiece.getColor();
            }
            return tempBoard;
        }
        public bool is_running() { return Game_Timer.Enabled; }
        //------------------
        //setter
        public void start_timer()
        {
            Game_Timer.Enabled = true;
            Game_Timer.Start();
        }

        public void stop_timer()
        {
            Game_Timer.Enabled = false;
            Game_Timer.Stop();
        }
        //--------------------
        //private functions
        private void Generate_Board()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    board[Tuple.Create(x, y)] = '.';
                }
            }
        }
        private void loose()
        {
            currentPiece = null;
            stop_timer();
            MessageBox.Show("あなたは失う");
        }
        //handles gravity, setting blocks as final, Tetris check and removing the current piece
        private void gravity()
        {
            bool Kill = false;
            Piece[] old = currentPiece.Blocks();
            currentPiece.gravity();
            //mark piece for kill based on the position after update
            foreach (Piece check in currentPiece.Blocks())
            {
                //floor collision
                if (check.Item2 < 0) { Kill = true; break; }
                //collision with another piece
                if (check.Item2 > 19) { continue; }
                if (board[check] != '.') { Kill = true; break; }
            }
            //kills the piece, applying its last valid coordinates as stable blocks
            if (Kill)
            {
                //set blocks as final
                foreach (Piece block in old)
                {
                    if (block.Item2 > 19) { loose(); return; }
                    board[block] = currentPiece.getColor();
                }
                currentPiece = null;
            }
        }
        private void Game_Tick(Object myObject, EventArgs myEventArgs)
        {

            Console.WriteLine(drawing.get_Input());

            //piece got killed last tick
            if (currentPiece == null)
            {
                currentPiece = new Pieces(util.colors[rand.Next(7)]);
                drawing.Invalidate();
                return;
            }
            tickCount++;
            //only apply natural gravity every x ticks
            if (tickCount >= 1)
            {
                gravity();
                tickCount = 0;
            }
            drawing.Invalidate();
        }

    }
}