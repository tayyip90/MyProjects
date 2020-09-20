using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class KnightFigur : ChessFigur
    {
        public KnightFigur(ColorEnum color) : base(KNIGHTSYMBOL, color) { }
    }
}