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
        private uint figureId;

        public ChessFigure(char symbol, ColorEnum color, uint figureId)
        {
            this.figurSymbol = symbol;
            this.color = color;
            this.figureId = figureId;
        }

        public ColorEnum getColor()
        {
            return this.color;
        }

        public char getSymbol()
        {
            return this.figurSymbol;
        }

        public uint getID()
        {
            return this.figureId;
        }
    }
}