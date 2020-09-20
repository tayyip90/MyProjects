using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class RookFigur : ChessFigur
    {
        public RookFigur(ColorEnum color) : base(ROOKSYMBOL, color)
        {

        }
    }
}