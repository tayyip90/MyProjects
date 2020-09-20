using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class BishopFigur : ChessFigur
    {
        public BishopFigur(ColorEnum color) : base(BISHOPSYMBOL, color) { }
    }
}