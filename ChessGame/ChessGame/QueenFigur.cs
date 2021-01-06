using System;
using System.Collections.Generic;
using System.Text;
using static ChessGame.Constants;

namespace ChessGame
{
    public class QueenFigur : ChessFigure
    {
        public QueenFigur(ColorEnum color) : base(QUEENSYMBOL, color, FigurTyp.Queen)
        {
        
        }
    }
}