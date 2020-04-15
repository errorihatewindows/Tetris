using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Piece = System.Tuple<int, int>;
using Board = System.Collections.Generic.Dictionary<System.Tuple<int, int>, char>;

namespace Tetris
{
    //a Piece on the move
    class Pieces
    {
        //delegates
        private delegate Piece[] Rotating();
        //variables
        private Piece position;
        private char color;
        private int rotation;
        private Dictionary<char, Rotating> blocks = new Dictionary<char, Rotating>();
        private Dictionary<int, Piece> rotating = new Dictionary<int, Piece>();
        public Pieces(char Color)
        {
            color = Color;
            rotation = 0;
            if (color == 'l') { position = Tuple.Create(5, 19); }
            else { position = Tuple.Create(0, 18); }
            init_dicts();

        }
        public char getColor() { return color; }
        public Piece[] Blocks()
        {
            return blocks[color]();
        }
        //applies downward movement, doesnt check collision
        public void gravity()
        {
            position = Tuple.Create(position.Item1,position.Item2 - 1);
        }  
        public void Rotate()
        {
            rotation = (rotation + 1) % 4;
            //I piece moves focus while rotating
            if (color == 'l') 
            {
                position = Tuple.Create(
                    position.Item1 + rotating[rotation].Item1,
                    position.Item2 + rotating[rotation].Item2);
            }
            //TODO add reverse rotate option
        }
        //inits all the diffrent dicts used in current piece
        private void init_dicts()
        {
            init_blocks();
            rotating[0] = Tuple.Create(1, 0);
            rotating[1] = Tuple.Create(0, -1);
            rotating[2] = Tuple.Create(-1, 0);
            rotating[3] = Tuple.Create(0, 1);
        }
        //Block function for the blocks dictionary
        private void init_blocks()
        {
            blocks['l'] = IBlocks;
            //TODO add other 6 blocks
        }
        private Piece[] IBlocks() 
        {
            int[] relative = { -2, -1, 0, 1 };
            int sign;   //contains -1 or 1
            //1 and 2 are in negative direction
            if (rotation % 3 == 0) { sign = 1; }
            else { sign = -1; }
            Piece[] output = new Piece[4];
            for (int i = 0; i < 4; i++)
            {
                //0 or 2 horizontally
                if (rotation % 2 == 0) { output[i] = Tuple.Create(position.Item1 + relative[i] * sign, position.Item2); }
                else { output[i] = Tuple.Create(position.Item1, position.Item2 + relative[i] * sign); }
            }
            return output;
        }/*not yet finished
        private Piece[] OBlock() { }
        private Piece[] SBlock() { }
        private Piece[] ZBlock() { }
        private Piece[] LBlock() { }
        private Piece[] JBlock() { }
        private Piece[] TBlock() { }*/
    }

}
