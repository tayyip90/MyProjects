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

        public void addFigureIdToFirstMoveOverList(Field[,] gameboard, int selectedFigureY, int selectedFigureX)
        {
            if (!firstMoveOver.Contains(gameboard[selectedFigureY, selectedFigureX].getChessFigure().getID())) {
                firstMoveOver.Add(gameboard[selectedFigureY, selectedFigureX].getChessFigure().getID());
            }
        }

        public void addFigureIdToPossibleMovementsDictionary(uint figureId)
        {
            if (!possibleMovements.ContainsKey(figureId))
            {
                possibleMovements.Add(figureId, new List<uint>());
            }
        }

        public void addFieldIdToPossibleMovementsDictionary(uint figureId, uint fieldId)
        {
            if (!possibleMovements[figureId].Contains(fieldId))
            {
                possibleMovements[figureId].Add(fieldId);
            }
        }

        public bool isFieldOccupied(Field[,] gameboard, int rowNumber, int columnNumber)
        {
            return gameboard[rowNumber, columnNumber].getIsFieldOccupied();
        }

        public bool checkWhetherFigureBelongsPlayer(Field[,] gameboard, Constants.ColorEnum playerTurn, int rowNumber, int columnNumber)
        {
            bool sameColor = false;

            if (gameboard[rowNumber, columnNumber].getChessFigure().getColor() == playerTurn)
            {
                sameColor = true;
            }

            return sameColor;
        }

        public bool isEnemyField(Field[,] gameboard, Constants.ColorEnum playerTurn, int rowNumber, int columnNumber)
        {
            bool enemyField = false;

            if(isFieldOccupied(gameboard, rowNumber, columnNumber))
            {
                enemyField = !checkWhetherFigureBelongsPlayer(gameboard, playerTurn, rowNumber, columnNumber);
            }

            return enemyField;
        }

        public bool ckeckWhetherMovementIsCorrect(Field[,] gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            uint figureId = gameboard[selectedFigureY, selectedFigureX].getChessFigure().getID();
            uint fieldId = gameboard[destinationFieldY, destinationFieldX].getFieldID();

            return possibleMovements[figureId].Contains(fieldId);
        }

        public void refreshPossibleMovementsDictionary(Field[,] gameboard)
        {
            foreach (List<uint> fieldIds in possibleMovements.Values)
            {
                fieldIds.Clear();
            }

            for(int i = 0; i < Constants.GAMEBOARDHEIGHT; i++)
            {
                for(int j = 0; j < Constants.GAMEBOARDWIDTH; j++)
                {
                    if(gameboard[i, j].getIsFieldOccupied())
                    {
                        bool isFirstMovmentOver = firstMoveOver.Contains(gameboard[i, j].getChessFigure().getID());
                        Constants.ColorEnum color = gameboard[i, j].getChessFigure().getColor();
                        uint figureId = gameboard[i, j].getChessFigure().getID();

                        switch (gameboard[i, j].getChessFigure())
                        {
                            case PawnFigur p:
                                bool enemyField;

                                if (!isFirstMovmentOver)
                                {
                                    if (color == Constants.ColorEnum.BLACK)
                                    {
                                        enemyField = isEnemyField(gameboard, color, i + 2, j);

                                        if (!enemyField)
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + 2, j].getFieldID());
                                        }
                                    }
                                    else
                                    {
                                        enemyField = isEnemyField(gameboard, color, i - 2, j);

                                        if (!enemyField)
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i - 2, j].getFieldID());
                                        }
                                    }
                                }

                                if(i-1 >= 0)
                                {
                                    if(!isFieldOccupied(gameboard, i - 1, j))
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i - 1, j].getFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (isEnemyField(gameboard, color, i - 1, j - 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i - 1, j - 1].getFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isEnemyField(gameboard, color, i - 1, j + 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i - 1, j + 1].getFieldID());
                                        }
                                    }
                                }

                                if(i+1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (!isFieldOccupied(gameboard, i + 1, j))
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + 1, j].getFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (isEnemyField(gameboard, color, i + 1, j - 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + 1, j - 1].getFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isEnemyField(gameboard, color, i + 1, j + 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + 1, j + 1].getFieldID());
                                        }
                                    }
                                }

                                break;

                            case RookFigur r:

                                bool foundFieldWithFigure = false;

                                for(int k = 1; i + k < Constants.GAMEBOARDHEIGHT; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if(isFieldOccupied(gameboard, i+k, j))
                                        {
                                            if(isEnemyField(gameboard, color, i+k, j))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + k, j].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + k, j].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; i + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i + k, j))
                                        {
                                            if (isEnemyField(gameboard, color, i + k, j))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + k, j].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i + k, j].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; k + j < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i, j + k))
                                        {
                                            if (isEnemyField(gameboard, color, i, j + k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i, j + k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; j + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i, j + k))
                                        {
                                            if (isEnemyField(gameboard, color, i, j + k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, gameboard[i, j + k].getFieldID());
                                        }
                                    }
                                }

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
    }
}