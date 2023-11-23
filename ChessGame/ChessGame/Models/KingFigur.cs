using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame.Models
{
    public class KingFigur : ChessFigure
    {
        public KingFigur(ColorEnum color, uint figureId) : base(KINGSYMBOL, color, figureId)
        {

        }

        public override string ToString()
        {
            return $"King, id:{GetID()}";
        }
    }
}