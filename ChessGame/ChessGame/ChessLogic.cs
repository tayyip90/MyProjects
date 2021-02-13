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

        public bool ckeckWhetherMovementIsCorrect(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;

            //  if there is a movement then...
            if (selectedFigureX != destinationFieldX | selectedFigureY != destinationFieldY)
            {
                switch (gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure())
                {
                    case PawnFigur p:
                        if(checkMovementForPawnFigure(gameboard, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                        {

                        }
                        break;
                    case RookFigur r:
                        if(checkMovementForRookFigure(gameboard, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                        {

                        }
                        break;
                    case KnightFigur n:
                        if(checkMovementForKnightFigure(gameboard, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                        {

                        }
                        break;
                    case BishopFigur b:
                        if(checkMovementForBishopFigure(gameboard, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                        {

                        }
                        break;
                    case QueenFigur q:
                        if(checkMovementForQueenFigure(gameboard, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                        {

                        }
                        break;
                    case KingFigur k:
                        if(checkMovementForKingFigure(gameboard, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                        {

                        }
                        break;
                }
            }

            return movementIsCorrect;
        }

        private bool checkMovementForKingFigure(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            throw new NotImplementedException();
        }

        private bool checkMovementForQueenFigure(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            throw new NotImplementedException();
        }

        private bool checkMovementForBishopFigure(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            throw new NotImplementedException();
        }

        private bool checkMovementForKnightFigure(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            throw new NotImplementedException();
        }

        private bool checkMovementForRookFigure(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            throw new NotImplementedException();
        }

        private bool checkMovementForPawnFigure(ChessGameboard gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;

            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            if (movementY == 1 | movementY == -1)
            {
                if(movementX == 0)
                {
                    movementIsCorrect = true;
                }
                else
                {
                    if(movementX == -1 | movementX == 1)
                    {
                         
                    }
                }
            }

            return movementIsCorrect;
        }
    }
}