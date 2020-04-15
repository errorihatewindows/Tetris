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
        private const int FPS = 1;
        private Board board = new Board();
        private Timer Game_Timer = new Timer();
        private Pieces currentPiece;
        //util function classes
        Random rand = new Random();
        Form drawing;
        public Game(Form form)
        {
            drawing = form;
            //initialize game timer
            Game_Timer.Interval = (Convert.ToInt32(1000 / FPS));
            Game_Timer.Tick += new EventHandler(Game_Tick);
            Generate_Board();

            currentPiece = new Pieces('p');
            gravity();
            gravity();
            gravity();

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
                if (board[check] != '.') { Kill = true; break; }
            }
            //kills the piece, applying its last valid coordinates as stable blocks
            if (Kill)
            {
                //set blocks as final
                foreach (Piece block in old) { board[block] = currentPiece.getColor(); }
                currentPiece = null;
            }
        }
        private void Game_Tick(Object myObject, EventArgs myEventArgs)
        {
            /*
            //piece got killed last tick
            if (currentPiece == null)
            {
                currentPiece = new Pieces('l');
                return;
            }
            tickCount++;
            //only apply natural gravity every x ticks
            if (tickCount >= 1)
            {
                gravity();
                tickCount = 0;
            }*/
            currentPiece.Rotate();
            drawing.Invalidate();
        }

    }
}