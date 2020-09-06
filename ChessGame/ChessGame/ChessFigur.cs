using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessFigur
    {
        private string figurSymbol;
        private string color;

        public ChessFigur(string symbol, string color)
        {
            this.figurSymbol = symbol;
            this.color = color;
        }

        public string getColor()
        {
            return this.color;
        }

        public string getSymbol()
        {
            return this.figurSymbol;
        }
    }
}