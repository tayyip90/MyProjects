using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class KnightFigur : ChessFigure
    {
        public KnightFigur(ColorEnum color) : base(KNIGHTSYMBOL, color, FigurTyp.Knight) { }
    }
}