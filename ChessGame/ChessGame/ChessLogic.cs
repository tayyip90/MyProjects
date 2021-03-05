using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessLogic
    {
        private List<uint> firstMoveOver;
        Dictionary<uint, List<uint>> possibleMovementsWhiteFigures;
        Dictionary<uint, List<uint>> possibleMovementsBlackFigures;

        public ChessLogic()
        {
            firstMoveOver = new List<uint>();
            possibleMovementsWhiteFigures = new Dictionary<uint, List<uint>>();
            possibleMovementsBlackFigures = new Dictionary<uint, List<uint>>();
            resetFirstMoveOverList();
        }

        public void resetFirstMoveOverList()
        {
            firstMoveOver.Clear();
        }

        public void resetPossibleMovementsBlackFiguresDictionary()
        {
            possibleMovementsBlackFigures.Clear();
        }

        public void resetPossibleMovementsWhiteFiguresDictionary()
        {
            possibleMovementsWhiteFigures.Clear();
        }

        public void addFigureIdToFirstMoveOverList(Field[,] gameboard, int selectedFigureY, int selectedFigureX)
        {
            if (!firstMoveOver.Contains(gameboard[selectedFigureY, selectedFigureX].getChessFigure().getID())) {
                firstMoveOver.Add(gameboard[selectedFigureY, selectedFigureX].getChessFigure().getID());
            }
        }

        public void addFigureIdToPossibleMovementsBlackFiguresDictionary(uint figureId)
        {
            if (!possibleMovementsBlackFigures.ContainsKey(figureId))
            {
                possibleMovementsBlackFigures.Add(figureId, new List<uint>());
            }
        }

        public void addFigureIdToPossibleMovementsWhiteFiguresDictionary(uint figureId)
        {
            if (!possibleMovementsWhiteFigures.ContainsKey(figureId))
            {
                possibleMovementsWhiteFigures.Add(figureId, new List<uint>());
            }
        }

        public void addFieldIdToPossibleMovementsWhiteFiguresDictionary(uint figureId, uint fieldId)
        {
            if (!possibleMovementsWhiteFigures[figureId].Contains(fieldId))
            {
                possibleMovementsWhiteFigures[figureId].Add(fieldId);
            }
        }

        public void addFieldIdToPossibleMovementsBlackFiguresDictionary(uint figureId, uint fieldId)
        {
            if (!possibleMovementsBlackFigures[figureId].Contains(fieldId))
            {
                possibleMovementsBlackFigures[figureId].Add(fieldId);
            }
        }

        public void addFieldIdToPossibleMovementsDictionary(uint figureId, Constants.ColorEnum color ,uint fieldId)
        {
            if (color == Constants.ColorEnum.WHITE)
            {
                if (!possibleMovementsWhiteFigures[figureId].Contains(fieldId))
                {
                    possibleMovementsWhiteFigures[figureId].Add(fieldId);
                }
            }
            else
            {
                if (!possibleMovementsBlackFigures[figureId].Contains(fieldId))
                {
                    possibleMovementsBlackFigures[figureId].Add(fieldId);
                }
            }
        }

        public bool isFieldOccupied(Field[,] gameboard, int rowNumber, int columnNumber)
        {
            return gameboard[rowNumber, columnNumber].getIsFieldOccupied();
        }

        public bool checkWhetherFigureBelongsPlayer(Field[,] gameboard, Constants.ColorEnum color, int rowNumber, int columnNumber)
        {
            bool sameColor = false;

            if (gameboard[rowNumber, columnNumber].getChessFigure().getColor() == color)
            {
                sameColor = true;
            }

            return sameColor;
        }

        public bool isEnemyField(Field[,] gameboard, Constants.ColorEnum color, int rowNumber, int columnNumber)
        {
            bool enemyField = false;

            if(isFieldOccupied(gameboard, rowNumber, columnNumber))
            {
                enemyField = !checkWhetherFigureBelongsPlayer(gameboard, color, rowNumber, columnNumber);
            }

            return enemyField;
        }

        public bool ckeckWhetherMovementIsCorrect(Field[,] gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            uint figureId = gameboard[selectedFigureY, selectedFigureX].getChessFigure().getID();
            Constants.ColorEnum color = gameboard[selectedFigureY, selectedFigureX].getChessFigure().getColor();
            uint fieldId = gameboard[destinationFieldY, destinationFieldX].getFieldID();
            bool isCorrect = false;

            if(color == Constants.ColorEnum.WHITE)
            {
                isCorrect = possibleMovementsWhiteFigures[figureId].Contains(fieldId);
            }
            else
            {
                isCorrect = possibleMovementsBlackFigures[figureId].Contains(fieldId);
            }

            return isCorrect;
        }

        public void refreshPossibleMovementsDictionary(Field[,] gameboard)
        {
            foreach (List<uint> fieldIds in possibleMovementsBlackFigures.Values)
            {
                fieldIds.Clear();
            }

            foreach (List<uint> fieldIds in possibleMovementsWhiteFigures.Values)
            {
                fieldIds.Clear();
            }

            for (int i = 0; i < Constants.GAMEBOARDHEIGHT; i++)
            {
                for(int j = 0; j < Constants.GAMEBOARDWIDTH; j++)
                {
                    bool foundFieldWithFigure;

                    if (gameboard[i, j].getIsFieldOccupied())
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
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j].getFieldID());
                                        }
                                    }
                                    else
                                    {
                                        enemyField = isEnemyField(gameboard, color, i - 2, j);

                                        if (!enemyField)
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j].getFieldID());
                                        }
                                    }
                                }

                                if(i-1 >= 0)
                                {
                                    if(!isFieldOccupied(gameboard, i - 1, j))
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j].getFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (isEnemyField(gameboard, color, i - 1, j - 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 1].getFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isEnemyField(gameboard, color, i - 1, j + 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 1].getFieldID());
                                        }
                                    }
                                }

                                if(i+1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (!isFieldOccupied(gameboard, i + 1, j))
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j].getFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (isEnemyField(gameboard, color, i + 1, j - 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 1].getFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isEnemyField(gameboard, color, i + 1, j + 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 1].getFieldID());
                                        }
                                    }
                                }

                                break;

                            case RookFigur r:

                                foundFieldWithFigure = false;

                                for(int k = 1; i + k < Constants.GAMEBOARDHEIGHT; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if(isFieldOccupied(gameboard, i+k, j))
                                        {
                                            if(isEnemyField(gameboard, color, i+k, j))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
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
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
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
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
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
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
                                        }
                                    }
                                }

                                break;

                            case KnightFigur n:
                                
                                if(i + 2 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if(j - 1 >= 0)
                                    {
                                        if (isFieldOccupied(gameboard, i + 2, j - 1))
                                        {
                                            if (isEnemyField(gameboard, color, i + 2, j - 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j - 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j - 1].getFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isFieldOccupied(gameboard, i + 2, j + 1))
                                        {
                                            if (isEnemyField(gameboard, color, i + 2, j + 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j + 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j + 1].getFieldID());
                                        }
                                    }
                                }

                                if (i + 1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (j - 2 >= 0)
                                    {
                                        if (isFieldOccupied(gameboard, i + 1, j - 2))
                                        {
                                            if (isEnemyField(gameboard, color, i + 1, j - 2))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 2].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 2].getFieldID());
                                        }
                                    }

                                    if (j + 2 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isFieldOccupied(gameboard, i + 1, j + 2))
                                        {
                                            if (isEnemyField(gameboard, color, i + 1, j + 2))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 2].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 2].getFieldID());
                                        }
                                    }
                                }

                                if (i - 2 >= 0)
                                {
                                    if (j - 1 >= 0)
                                    {
                                        if (isFieldOccupied(gameboard, i - 2, j - 1))
                                        {
                                            if (isEnemyField(gameboard, color, i - 2, j - 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j - 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j - 1].getFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isFieldOccupied(gameboard, i - 2, j + 1))
                                        {
                                            if (isEnemyField(gameboard, color, i - 2, j + 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j + 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j + 1].getFieldID());
                                        }
                                    }
                                }

                                if (i - 1 >= 0)
                                {
                                    if (j - 2 >= 0)
                                    {
                                        if (isFieldOccupied(gameboard, i - 1, j - 2))
                                        {
                                            if (isEnemyField(gameboard, color, i - 1, j - 2))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 2].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 2].getFieldID());
                                        }
                                    }

                                    if (j + 2 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isFieldOccupied(gameboard, i - 1, j + 2))
                                        {
                                            if (isEnemyField(gameboard, color, i - 1, j + 2))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 2].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 2].getFieldID());
                                        }
                                    }
                                }

                                break;
                            case BishopFigur b:

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i + k, j + k))
                                        {
                                            if (isEnemyField(gameboard, color, i + k, j + k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i + k, j - k))
                                        {
                                            if (isEnemyField(gameboard, color, i + k, j - k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i - k, j - k))
                                        {
                                            if (isEnemyField(gameboard, color, i - k, j - k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i - k, j + k))
                                        {
                                            if (isEnemyField(gameboard, color, i - k, j + k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].getFieldID());
                                        }
                                    }
                                }

                                break;
                            case QueenFigur q:

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i + k, j + k))
                                        {
                                            if (isEnemyField(gameboard, color, i + k, j + k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i + k, j - k))
                                        {
                                            if (isEnemyField(gameboard, color, i + k, j - k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i - k, j - k))
                                        {
                                            if (isEnemyField(gameboard, color, i - k, j - k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i - k, j + k))
                                        {
                                            if (isEnemyField(gameboard, color, i - k, j + k))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].getFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (isFieldOccupied(gameboard, i + k, j))
                                        {
                                            if (isEnemyField(gameboard, color, i + k, j))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
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
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].getFieldID());
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
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
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
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].getFieldID());
                                        }
                                    }
                                }

                                break;
                            case KingFigur k:

                                if (i + 1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (isFieldOccupied(gameboard, i + 1, j))
                                    {
                                        if (isEnemyField(gameboard, color, i + 1, j))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j].getFieldID());
                                        }
                                    }
                                    else
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j].getFieldID());
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isFieldOccupied(gameboard, i + 1, j + 1))
                                        {
                                            if (isEnemyField(gameboard, color, i + 1, j + 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 1].getFieldID());
                                        }
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (isFieldOccupied(gameboard, i + 1, j - 1))
                                        {
                                            if (isEnemyField(gameboard, color, i + 1, j - 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 1].getFieldID());
                                        }
                                    }
                                }

                                if (j + 1 < Constants.GAMEBOARDWIDTH)
                                {
                                    if (isFieldOccupied(gameboard, i, j + 1))
                                    {
                                        if (isEnemyField(gameboard, color, i, j + 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + 1].getFieldID());
                                        }
                                    }
                                    else
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + 1].getFieldID());
                                    }
                                }

                                if (j - 1 >= 0)
                                {
                                    if (isFieldOccupied(gameboard, i, j - 1))
                                    {
                                        if (isEnemyField(gameboard, color, i, j - 1))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j - 1].getFieldID());
                                        }
                                    }
                                    else
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j  - 1].getFieldID());
                                    }
                                }

                                if (i - 1 >= 0)
                                {
                                    if (isFieldOccupied(gameboard, i - 1, j))
                                    {
                                        if (isEnemyField(gameboard, color, i - 1, j))
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j].getFieldID());
                                        }
                                    }
                                    else
                                    {
                                        addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j].getFieldID());
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (isFieldOccupied(gameboard, i - 1, j + 1))
                                        {
                                            if (isEnemyField(gameboard, color, i - 1, j + 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 1].getFieldID());
                                        }
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (isFieldOccupied(gameboard, i - 1, j - 1))
                                        {
                                            if (isEnemyField(gameboard, color, i - 1, j - 1))
                                            {
                                                addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 1].getFieldID());
                                            }
                                        }
                                        else
                                        {
                                            addFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 1].getFieldID());
                                        }
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }

        public bool checkWhetherBlackKingIsChecked(Field[,] gameboard)
        {
            bool isChecked = false;

            uint fieldId = findFieldIdOfKingWithColor(gameboard, Constants.ColorEnum.BLACK);

            isChecked = canEnemyMoveOnField(fieldId, Constants.ColorEnum.BLACK);

            return isChecked;
        }

        private bool canEnemyMoveOnField(uint fieldId, Constants.ColorEnum color)
        {
            bool fieldCanBeReached = false;

            switch (color)
            {
                case Constants.ColorEnum.BLACK:

                    foreach (List<uint> fieldIds in possibleMovementsWhiteFigures.Values)
                    {
                        if (!fieldCanBeReached)
                        {
                            fieldCanBeReached = fieldIds.Contains(fieldId);
                        }
                    }

                    break;
                case Constants.ColorEnum.WHITE:

                    foreach (List<uint> fieldIds in possibleMovementsBlackFigures.Values)
                    {
                        if (!fieldCanBeReached)
                        {
                            fieldCanBeReached = fieldIds.Contains(fieldId);
                        }
                    }

                    break;
            }

            return fieldCanBeReached;
        }

        private uint findFieldIdOfKingWithColor(Field[,] gameboard, Constants.ColorEnum color)
        {
            uint fieldId = 0;

            for(int i = 0; i < Constants.GAMEBOARDHEIGHT; i++)
            {
                for(int j = 0; j < Constants.GAMEBOARDWIDTH; j++)
                {
                    if(isFieldOccupied(gameboard, i, j))
                    {
                        switch (gameboard[i,j].getChessFigure())
                        {
                            case KingFigur k:
                                if(gameboard[i, j].getChessFigure().getColor() == color)
                                {
                                    fieldId = gameboard[i, j].getFieldID();
                                }
                                break;
                        }
                    }
                }
            }

            return fieldId;
        }

        public bool checkWhetherWhiteKingIsChecked(Field[,] gameboard)
        {
            bool isChecked = false;

            uint fieldId = findFieldIdOfKingWithColor(gameboard, Constants.ColorEnum.WHITE);

            isChecked = canEnemyMoveOnField(fieldId, Constants.ColorEnum.WHITE);

            return isChecked;
        }

        public List<string> getPossibleMovmentsOfFigures(List<ChessFigure> allFigures){
            List<string> possibleMovementsList = new List<string>();
            string possibleMovementOfFigure;

            possibleMovementsList.Add("Possible Movements of White Figures");
            possibleMovementsList.Add(string.Empty);

            foreach (uint key in possibleMovementsWhiteFigures.Keys)
            {
                ChessFigure figure = allFigures.Find(x => x.getID() == key);

                possibleMovementOfFigure = string.Empty;
                possibleMovementOfFigure += "Figure ID: " + figure.getID();

                switch (figure)
                {
                    case PawnFigur p:
                        possibleMovementOfFigure += " , Figure Typ: Pawn, Possible Fields: ";
                        break;
                    case KingFigur k:
                        possibleMovementOfFigure += " , Figure Typ: King, Possible Fields: ";
                        break;
                    case KnightFigur n:
                        possibleMovementOfFigure += " , Figure Typ: Knight, Possible Fields: ";
                        break;
                    case RookFigur r:
                        possibleMovementOfFigure += " , Figure Typ: Rook, Possible Fields: ";
                        break;
                    case QueenFigur q:
                        possibleMovementOfFigure += " , Figure Typ: Queen, Possible Fields: ";
                        break;
                    case BishopFigur b:
                        possibleMovementOfFigure += " , Figure Typ: Bishop, Possible Fields: ";
                        break;
                }

                foreach (uint fieldId in possibleMovementsWhiteFigures[key])
                {
                    possibleMovementOfFigure += " " + fieldId + ",";
                }

                possibleMovementsList.Add(possibleMovementOfFigure);
            }

            possibleMovementsList.Add(string.Empty);
            possibleMovementsList.Add("Possible Movements of Black Figures");
            possibleMovementsList.Add(string.Empty);

            foreach (uint key in possibleMovementsBlackFigures.Keys)
            {
                ChessFigure figure = allFigures.Find(x => x.getID() == key);

                possibleMovementOfFigure = string.Empty;
                possibleMovementOfFigure += "Figure ID: " + figure.getID();

                switch (figure)
                {
                    case PawnFigur p:
                        possibleMovementOfFigure += " , Figure Typ: Pawn, Possible Fields: ";
                        break;
                    case KingFigur k:
                        possibleMovementOfFigure += " , Figure Typ: King, Possible Fields: ";
                        break;
                    case KnightFigur n:
                        possibleMovementOfFigure += " , Figure Typ: Knight, Possible Fields: ";
                        break;
                    case RookFigur r:
                        possibleMovementOfFigure += " , Figure Typ: Rook, Possible Fields: ";
                        break;
                    case QueenFigur q:
                        possibleMovementOfFigure += " , Figure Typ: Queen, Possible Fields: ";
                        break;
                    case BishopFigur b:
                        possibleMovementOfFigure += " , Figure Typ: Bishop, Possible Fields: ";
                        break;
                }

                foreach (uint fieldId in possibleMovementsBlackFigures[key])
                {
                    possibleMovementOfFigure += " " + fieldId + ",";
                }

                possibleMovementsList.Add(possibleMovementOfFigure);
            }

            return possibleMovementsList;
        }
    }
}