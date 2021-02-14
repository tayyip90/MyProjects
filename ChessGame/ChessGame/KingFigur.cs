using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class KingFigur : ChessFigure
    {
        public KingFigur(ColorEnum color, uint figureId) : base(KINGSYMBOL, color, figureId)
        {

        }
    }
}