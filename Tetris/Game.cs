using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Piece = System.Tuple<int, int>;
using Board = System.Collections.Generic.Dictionary<System.Tuple<int, int>, char>;

namespace Tetris
{
    public class Game
    {
        private int tickCount = 0;
        private const int FPS = 30;
        private int Score;
        private Board board = new Board();
        private Timer Game_Timer = new Timer();
        private Pieces nextPiece;
        private Pieces currentPiece;
        private int LinestoClear;
        private int Level;
        //util function classes
        Random rand = new Random();
        Form1 drawing;
        public Game(Form1 form)
        {
            drawing = form;
            //initialize game timer
            Game_Timer.Interval = (Convert.ToInt32(1000 / FPS));
            Game_Timer.Tick += new EventHandler(Game_Tick);
            nextPiece = new Pieces(util.colors[rand.Next(7)]);
            Generate_Board();
            Score = 0;
            Level = 10;
            LinestoClear = 10;
        }
        //-----------------
        //getter
        public Board GetBoard()
        {
            if (currentPiece == null) { return board; }
            Board tempBoard = new Board(board);
            if (currentPiece != null)
            {
                //draws Piece on the board
                foreach (Piece current in currentPiece.Blocks())    
                    tempBoard[current] = currentPiece.getColor();
            }
            return tempBoard;
        }

        public Board GetNextPiece()
        {
            Board output = new Board();
            //handle nextPiece not beeing set
            if (nextPiece == null) { output[Tuple.Create(0, 0)] = '.'; return output; }
            foreach (Piece current in nextPiece.Blocks())
            {
                int x = current.Item1 - nextPiece.getPosition().Item1;
                int y = current.Item2 - nextPiece.getPosition().Item2;
                output[Tuple.Create(x,y)] = nextPiece.getColor();
            }
            return output;
        }

        public int GetScore() { return Score; }
        public int GetLevel() { return Level; }
        public int GetLines() { return LinestoClear; }
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
        private bool in_board(Piece position)
        {
            if (position.Item1 < 0 || position.Item2 < 0 ||
                position.Item1 > 9 || position.Item2 > 19)
            { return false; }
            return true;
        }
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
            drawing.playSound("stop");
            drawing.playSound("gameover");
            MessageBox.Show("You are fucking bad at TETRIS");
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
                check_lines();
            }
        }
        //handles roation
        private void Rotate()
        {
            Piece[] relatives = currentPiece.wallkick_relatives();
            Piece[] old = currentPiece.Blocks();
            currentPiece.Rotate();
            int sucess = -1;
            for (int i = 0; i < 5; i++)
            {
                bool fail = false;
                foreach (Piece check in currentPiece.Blocks())
                {
                    Piece current= Tuple.Create(check.Item1 + relatives[i].Item1,
                                                check.Item2 + relatives[i].Item2);
                    //out of board
                    if (!in_board(current))
                    { fail = true; break; }
                    //piece blocks
                    if (board[current] != '.') { fail = true; break; }
                }
                //check passed, perform this kick
                if (!fail) 
                {
                    sucess = i;
                    currentPiece.updatePosition(relatives[i]);
                    break;
                }
            }
            if (sucess == -1) { currentPiece.Rotate(true); }
        }
        //moves piece by 1 or -1
        private void move(int direction)
        {
            bool fail = false;
            Piece[] old = currentPiece.Blocks();
            currentPiece.move(direction);
            foreach (Piece current in currentPiece.Blocks())
            {
                //out of border
                if (!in_board(current)) { fail = true; break; }
                //piece in da way
                if (board[current] != '.') { fail = true; break; }
            }
            if (fail) { currentPiece.move(-direction); }
        }
        //deletes a line and moves all pieces down
        private void delete_line(int line)
        {
            //move lines down, this will overwrite the line that is overwritten
            for (int y = line + 1; y < 20; y++) {
                for (int x = 0; x < 10; x++) { 
                    board[Tuple.Create(x, y - 1)] = board[Tuple.Create(x, y)]; } }
            //set last line to '.'
            for (int x = 0; x < 10; x++) { board[Tuple.Create(x, 19)] = '.'; }
        }
        private void check_lines()
        {
            //deleted lines from the TOP
            int deleted = 0;
            for (int y = 19; y >= 0; y--)
            {
                bool filled = true; //assume filled
                for (int x = 0; x < 10; x++)
                    if (board[Tuple.Create(x,y)] == '.') { filled = false; }
                if (filled)
                {                    
                    delete_line(y);
                    deleted++;
                }
            }
            if (deleted > 0) 
            {
                Score += util.points[deleted - 1] * (Level + 1);
                LinestoClear = LinestoClear - deleted;
                if (LinestoClear <= 0)
                {
                    Level++;
                    LinestoClear = 10;
                }
                Console.WriteLine("you removed {0} Lines: total Points {1}", deleted, Score);
                drawing.playSound("line");
            }
        }
        private int FramesPerGrid(int Level)
        {
            int FramesPerGrid = 0;
            if (Level <= 8)
                FramesPerGrid = 48 - (5 * Level);
            if (Level == 9)
                FramesPerGrid = 6;
            if (Level >= 10 && Level <= 18)
                FramesPerGrid = 5 - (int)((Level - 10) / 3);
            if (Level >= 19 && Level <= 28)
                FramesPerGrid = 2;
            if (Level >= 29)
                FramesPerGrid = 1;

            return FramesPerGrid; 
        }
        private void Game_Tick(Object myObject, EventArgs myEventArgs)
        {
                           
            //piece got killed last tick
            if (currentPiece == null)
            {
                currentPiece = nextPiece;   
                nextPiece = new Pieces(util.colors[rand.Next(7)]);
                drawing.Invalidate();
                return;
            }
            //user input
            char input = drawing.get_Input();
            bool changed = true;    //resets to false if the switch defaults
            switch (input)
            {
                case 'L':
                    move(-1); break;
                case 'R':
                    move(1); break;
                case 'U':
                    //TODO add handler, rotates are not always valid
                    Rotate(); break;
                case 'D':
                    gravity(); break;
                default:
                    changed = false; break;
            }
            tickCount++;
            if (currentPiece == null) { drawing.Invalidate(); return; }
            //only apply natural gravity every x ticks
            if (tickCount >= FramesPerGrid(Level))
            {
                gravity();
                tickCount = 0;
                changed = true;
                Console.WriteLine(Level);
                Console.WriteLine(FramesPerGrid(Level));
                Console.WriteLine(LinestoClear);
            }
            if (changed) { drawing.Invalidate(); }
        }

    }
}