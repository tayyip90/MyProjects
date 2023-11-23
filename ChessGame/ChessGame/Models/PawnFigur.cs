using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame.Models
{
    public class PawnFigur : ChessFigure
    {
        public PawnFigur(ColorEnum color, uint figureId) : base(PAWNSYMBOL, color, figureId)
        {

        }

        public override string ToString()
        {
            return $"Pawn, id:{GetID()}";
        }
    }
}