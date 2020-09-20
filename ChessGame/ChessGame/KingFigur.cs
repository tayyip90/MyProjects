using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class KingFigur : ChessFigur
    {
        public KingFigur(ColorEnum color) : base(KINGSYMBOL, color)
        {

        }
    }
}