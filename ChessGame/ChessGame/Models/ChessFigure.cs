using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame.Models
{
    public abstract class ChessFigure
    {
        private char figurSymbol;
        private ColorEnum color;
        private uint figureId;

        public ChessFigure(char symbol, ColorEnum color, uint figureId)
        {
            figurSymbol = symbol;
            this.color = color;
            this.figureId = figureId;
        }

        public ColorEnum GetColor()
        {
            return color;
        }

        public char GetSymbol()
        {
            return figurSymbol;
        }

        public uint GetID()
        {
            return figureId;
        }
    }
}