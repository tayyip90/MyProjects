using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class KingFigur : ChessFigure
    {
        public KingFigur(ColorEnum color) : base(KINGSYMBOL, color, FigurTyp.King)
        {

        }
    }
}