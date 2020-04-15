﻿using System;
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
            else { position = Tuple.Create(4, 18); }
            init_dicts();

        }
        public char getColor() { return color; }
        public Piece[] Blocks()
        {
            return blocks[color]();
        }
        //reduce y axis setter
        public void gravity()
        {
            position = Tuple.Create(position.Item1,position.Item2 - 1);
        }  
        //x axis setter
        public void move(int direction)
        {
            position = Tuple.Create(position.Item1 + direction, position.Item2);
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
            blocks['l'] = IBlock;
            blocks['y'] = OBlock;
            //TODO add other 5 blocks
        }
        private Piece[] IBlock() 
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
        }
        private Piece[] OBlock()
        {
            Piece[] output = new Piece[4];
            output[0] = position;
            output[1] = Tuple.Create(position.Item1, position.Item2 + 1);
            output[2] = Tuple.Create(position.Item1 + 1, position.Item2);
            output[3] = Tuple.Create(position.Item1 + 1, position.Item2 + 1);
            return output;
        }
        private Piece[] SBlock()
        {
            Piece[] output = new Piece[4];

            output[0] = position;
            
            if (rotation == 0)
            {
                output[1] = Tuple.Create(position.Item1 - 1, position.Item2 + 1);
                output[2] = Tuple.Create(position.Item1, position.Item2 + 1);
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2);
            }
            if (rotation == 1)
            {
                output[1] = Tuple.Create(position.Item1 + 1, position.Item2 + 1);
                output[2] = Tuple.Create(position.Item1, position.Item2 - 1);
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2);
            }
            if (rotation == 2)
            {
                output[1] = Tuple.Create(position.Item1 - 1, position.Item2 );
                output[2] = Tuple.Create(position.Item1, position.Item2 - 1);
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 - 1);
            }
            if (rotation == 3)
            {
                output[1] = Tuple.Create(position.Item1, position.Item2 + 1);
                output[2] = Tuple.Create(position.Item1 - 1, position.Item2);
                output[3] = Tuple.Create(position.Item1 - 1, position.Item2 - 1);
            }


            return output;
        }
        private Piece[] ZBlock()
        {
            Piece[] output = new Piece[4];

            output[0] = position;

            if (rotation == 0)
            {
                output[1] = Tuple.Create(position.Item1, position.Item2 - 1);
                output[2] = Tuple.Create(position.Item1, position.Item2 + 1);
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 + 1);
            }
            if (rotation == 1)
            {
                output[1] = Tuple.Create(position.Item1, position.Item2 + 1);
                output[2] = Tuple.Create(position.Item1 + 1, position.Item2);
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 - 1);
            }
            if (rotation == 2)
            {
                output[1] = Tuple.Create(position.Item1 - 1, position.Item2 - 1);
                output[2] = Tuple.Create(position.Item1, position.Item2 - 1);
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2);
            }
            if (rotation == 3)
            {
                output[1] = Tuple.Create(position.Item1 - 1, position.Item2 + 1);
                output[2] = Tuple.Create(position.Item1 - 1, position.Item2);
                output[3] = Tuple.Create(position.Item1, position.Item2 - 1);
            }

            return output;
        }

        private Piece[] LBlock()
        {
            Piece[] output = new Piece[4];

            output[0] = position;

            if (rotation % 2 == 0)
            {
                output[1] = Tuple.Create(position.Item1 - 1, position.Item2);
                output[2] = Tuple.Create(position.Item1 + 1, position.Item2);
            }
            if (rotation % 2 == 1)
            {
                output[1] = Tuple.Create(position.Item1, position.Item2 - 1);
                output[2] = Tuple.Create(position.Item1, position.Item2 + 1);
            }
            if (rotation == 0)
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 + 1);
            if (rotation == 1)
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 - 1);
            if (rotation == 2)
                output[3] = Tuple.Create(position.Item1 - 1, position.Item2 - 1);
            if (rotation == 3)
                output[3] = Tuple.Create(position.Item1 - 1, position.Item2 + 1);

            return output;
        }
            
        private Piece[] JBlock()
        {
            Piece[] output = new Piece[4];

            output[0] = position;

            if (rotation % 2 == 0)
            {
                output[1] = Tuple.Create(position.Item1 - 1, position.Item2);
                output[2] = Tuple.Create(position.Item1 + 1, position.Item2);
            }
            if (rotation % 2 == 1)
            {
                output[1] = Tuple.Create(position.Item1, position.Item2 - 1);
                output[2] = Tuple.Create(position.Item1, position.Item2 + 1);
            }
            if (rotation == 0)
                output[3] = Tuple.Create(position.Item1 - 1, position.Item2 + 1);
            if (rotation == 1)
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 + 1);
            if (rotation == 2)
                output[3] = Tuple.Create(position.Item1 + 1, position.Item2 - 1);
            if (rotation == 3)
                output[3] = Tuple.Create(position.Item1 - 1, position.Item2 - 1);

            return output;
        }
    }

}
