using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessLogic
    {
        public bool isFieldOccupied(ChessGameboard gameboard, int rowNumber, int columnNumber)
        {
            return gameboard.getBoard()[rowNumber, columnNumber].getIsFieldOccupied();
        }

        public bool checkWhetherFigureBelongsPlayer(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int rowNumber, int columnNumber)
        {
            bool sameColor = false;

            if (gameboard.getBoard()[rowNumber, columnNumber].getChessFigure().getColor() == playerTurn)
            {
                sameColor = true;
            }

            return sameColor;
        }

        public bool isEnemyField(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int rowNumber, int columnNumber)
        {
            bool fieldIsOccupiedWithEnemyFigure = false;

            if (gameboard.getBoard()[rowNumber, columnNumber].getIsFieldOccupied())
            {
                if (gameboard.getBoard()[rowNumber, columnNumber].getChessFigure().getColor() != playerTurn)
                {
                    fieldIsOccupiedWithEnemyFigure = true;
                }
            }

            return fieldIsOccupiedWithEnemyFigure;
        }
    }
}