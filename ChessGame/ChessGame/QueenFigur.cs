using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class QueenFigur : ChessFigur
    {
        public QueenFigur(ColorEnum color) : base(QUEENSYMBOL, color)
            {}
    }
}