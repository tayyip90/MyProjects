using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class KingFigur : ChessFigur
    {
        private const string SYMBOL = "K";

        public KingFigur(string color) : base(SYMBOL, color)
        {

        }
    }
}