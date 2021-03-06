using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class KnightFigur : ChessFigure
    {
        public KnightFigur(ColorEnum color, uint figureId) : base(KNIGHTSYMBOL, color, figureId) { }

        public override string ToString()
        {
            return "Knight Figure";
        }
    }
}