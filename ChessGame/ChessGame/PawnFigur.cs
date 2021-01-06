using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class PawnFigur : ChessFigure
    {
        public PawnFigur(ColorEnum color) : base(PAWNSYMBOL, color, FigurTyp.Pawn)
        {

        }
    }
}