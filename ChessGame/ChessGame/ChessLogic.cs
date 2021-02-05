using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessLogic
    {
        public bool isFigureOccupiedAndFigureBelongsThePlayer(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int rowNumber, int columnNumber)
        {
            bool fieldIsOccupiedWithTheSameColor = false;

            if(gameboard.getBoard()[rowNumber, columnNumber].getIsFieldOccupied())
            {
                if(gameboard.getBoard()[rowNumber, columnNumber].getChessFigure().getColor() == playerTurn)
                {
                    fieldIsOccupiedWithTheSameColor = true;
                }
            }

            return fieldIsOccupiedWithTheSameColor;
        }

        public bool isEmptyOrEnemyField(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int rowNumber, int columnNumber)
        {
            bool fieldIsEmptyOrOccupiedWithEnemyFigure = false;

            if (gameboard.getBoard()[rowNumber, columnNumber].getIsFieldOccupied())
            {
                if (gameboard.getBoard()[rowNumber, columnNumber].getChessFigure().getColor() != playerTurn)
                {
                    fieldIsEmptyOrOccupiedWithEnemyFigure = true;
                }
            }
            else
            {
                fieldIsEmptyOrOccupiedWithEnemyFigure = true;
            }

            return fieldIsEmptyOrOccupiedWithEnemyFigure;
        }
    }
}