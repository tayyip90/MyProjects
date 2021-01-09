using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class BishopFigur : ChessFigure
    {
        public BishopFigur(ColorEnum color, uint figureId) : base(BISHOPSYMBOL, color, FigurTyp.Bishop, figureId) { }
    }
}