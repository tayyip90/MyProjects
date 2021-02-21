using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessLogic
    {
        private List<uint> firstMoveOver;
        Dictionary<uint, List<uint>> possibleMovements;

        public ChessLogic()
        {
            firstMoveOver = new List<uint>();
            possibleMovements = new Dictionary<uint, List<uint>>();
            resetFirstMoveOverList();
        }

        public void resetFirstMoveOverList()
        {
            firstMoveOver.Clear();
        }

        public void resetPossibleMovementsDictionary()
        {
            possibleMovements.Clear();
        }

        public void addFigureIdToFirstMoveOverList(uint figureId)
        {
            if (!firstMoveOver.Contains(figureId)){
                firstMoveOver.Add(figureId);
            }
        }

        public void addToPossibleMovementsDictionary(uint figureId, uint fieldId)
        {
            if (!possibleMovements[figureId].Contains(fieldId))
            {
                possibleMovements[figureId].Add(figureId);
            }
        }

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

        public bool ckeckWhetherMovementIsCorrect(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;

            //  if there is a movement then...
            if (selectedFigureX != destinationFieldX | selectedFigureY != destinationFieldY)
            {
                switch (gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure())
                {
                    case PawnFigur p:
                        movementIsCorrect = checkMovementForPawnFigure(gameboard, playerTurn, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);
                        break;
                    case RookFigur r:
                        movementIsCorrect = checkMovementForRookFigure(gameboard, playerTurn, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);
                        break;
                    case KnightFigur n:
                        movementIsCorrect = checkMovementForKnightFigure(gameboard, playerTurn, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);
                        break;
                    case BishopFigur b:
                        movementIsCorrect = checkMovementForBishopFigure(gameboard, playerTurn, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);
                        break;
                    case QueenFigur q:
                        movementIsCorrect = checkMovementForQueenFigure(gameboard, playerTurn, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);
                        break;
                    case KingFigur k:
                        movementIsCorrect = checkMovementForKingFigure(gameboard, playerTurn, selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);
                        break;
                }
            }

            return movementIsCorrect;
        }

        private bool checkMovementForKingFigure(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;
            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            return movementIsCorrect;
        }

        public void refreshPossibleMovementsDictionary(Field[,] gameboard)
        {
            for(uint i = 0; i < 8; i++)
            {
                for(uint j = 0; j < 8; j++)
                {
                    if(gameboard[i, j].getIsFieldOccupied())
                    {
                        bool isFirstMovmentOver = firstMoveOver.Contains(gameboard[i, j].getChessFigure().getID());
                        Constants.ColorEnum color = gameboard[i, j].getChessFigure().getColor();

                        switch (gameboard[i, j].getChessFigure())
                        {
                            case PawnFigur p:
                                    
                                break;
                            case RookFigur r:
                                
                                break;
                            case KnightFigur n:
                                
                                break;
                            case BishopFigur b:
                               
                                break;
                            case QueenFigur q:
                                
                                break;
                            case KingFigur k:
                                
                                break;
                        }
                    }
                }
            }
        }

        private bool checkMovementForQueenFigure(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;
            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            return movementIsCorrect;
        }

        private bool checkMovementForBishopFigure(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;
            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            return movementIsCorrect;
        }

        private bool checkMovementForKnightFigure(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;
            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            if(movementY == 1 | movementY == -1)
            {
                if(movementX == 2 | movementX == -2)
                {
                    movementIsCorrect = true;
                }
            }

            if(movementY == 2 | movementY == -2)
            {
                if(movementX == 1 | movementX == -1)
                {
                    movementIsCorrect = true;
                }
            }
                

            return movementIsCorrect;
        }

        private bool checkMovementForRookFigure(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;
            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            return movementIsCorrect;
        }

        private bool checkMovementForPawnFigure(ChessGameboard gameboard, Constants.ColorEnum playerTurn, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            bool movementIsCorrect = false;
            int movementX, movementY;

            movementX = destinationFieldX - selectedFigureX;
            movementY = destinationFieldY - selectedFigureY;

            if (movementY == 2 | movementY == -2)
            { 
                if(!firstMoveOver.Contains(gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure().getID()) & movementX == 0)
                {
                    movementIsCorrect = true;
                }
            }

            if (movementY == 1 | movementY == -1)
            {
                if(movementX == 0)
                {
                    if (!isEnemyField(gameboard, playerTurn, destinationFieldY, destinationFieldX)) movementIsCorrect = true;
                }
                else
                {
                    if(movementX == -1 | movementX == 1)
                    {
                        if(isEnemyField(gameboard, playerTurn, destinationFieldY, destinationFieldX)) movementIsCorrect = true;
                    }
                }
            }

            if (movementIsCorrect)
            {
                addFigureIdToFirstMoveOverList(gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure().getID());
            }

            return movementIsCorrect;
        }
    }
}