using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public abstract class ChessFigure
    {
        private char figurSymbol;
        private ColorEnum color;
        private FigurTyp typ;

        public ChessFigure(char symbol, ColorEnum color, FigurTyp typ)
        {
            this.figurSymbol = symbol;
            this.color = color;
            this.typ = typ;
        }

        public ColorEnum getColor()
        {
            return this.color;
        }

        public char getSymbol()
        {
            return this.figurSymbol;
        }

        public FigurTyp GetTyp()
        {
            return this.typ;
        }
    }
}