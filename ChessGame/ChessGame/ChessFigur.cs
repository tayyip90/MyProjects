using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public abstract class ChessFigur
    {
        private char figurSymbol;
        private ColorEnum color;

        public ChessFigur(char symbol, ColorEnum color)
        {
            this.figurSymbol = symbol;
            this.color = color;
        }

        public ColorEnum getColor()
        {
            return this.color;
        }

        public char getSymbol()
        {
            return this.figurSymbol;
        }
    }
}