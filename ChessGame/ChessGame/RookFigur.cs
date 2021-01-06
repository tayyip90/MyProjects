using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class RookFigur : ChessFigure
    {
        public RookFigur(ColorEnum color) : base(ROOKSYMBOL, color, FigurTyp.Rook)
        {

        }
    }
}