using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class ChessFigur
    {
        private string figurSymbol;
        private ColorEnum color;

        public ChessFigur(string symbol, ColorEnum color)
        {
            this.figurSymbol = symbol;
            this.color = color;
        }

        public ColorEnum getColor()
        {
            return this.color;
        }

        public string getSymbol()
        {
            return this.figurSymbol;
        }
    }
}