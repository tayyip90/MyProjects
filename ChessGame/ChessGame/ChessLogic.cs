﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessLogic
    {
        private List<uint> pawnsfirstMoveOver;

        public ChessLogic()
        {
            pawnsfirstMoveOver = new List<uint>();
            resetPawnsFirstMoveOverList();
        }

        public void resetPawnsFirstMoveOverList()
        {
            pawnsfirstMoveOver.Clear();
        }

        public void addPawnFirstMoveOver(uint figureId)
        {
            if (!pawnsfirstMoveOver.Contains(figureId)){
                pawnsfirstMoveOver.Add(figureId);
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
                if(!pawnsfirstMoveOver.Contains(gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure().getID()) & movementX == 0)
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
                addPawnFirstMoveOver(gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure().getID());
            }

            return movementIsCorrect;
        }
    }
}