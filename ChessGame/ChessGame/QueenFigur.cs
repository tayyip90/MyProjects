using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class QueenFigur : ChessFigure
    {
        public QueenFigur(ColorEnum color, uint figureId) : base(QUEENSYMBOL, color, figureId)
        {
        
        }

        public override string ToString()
        {
            return "Queen Figure";
        }
    }
}