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
        private const int FPS = 30;
        private Board board = new Board();
        private Timer Game_Timer = new Timer();
        Random rand = new Random();
        Form drawing;
        public Game(Form form)
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
            return board;
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
        private void Game_Tick(Object myObject, EventArgs myEventArgs)
        {
            List<Piece> valid = new List<Piece>();
            foreach (KeyValuePair<Piece, char> kvp in board)
            {
                if (kvp.Value == '.') { valid.Add(kvp.Key); }
            }

            if (valid.Count != 0)
                board[valid[rand.Next(valid.Count)]] = util.colors[rand.Next(util.colors.Length)];

            drawing.Invalidate();
        }
    }
}