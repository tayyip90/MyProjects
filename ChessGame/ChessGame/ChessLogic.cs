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
            ResetFirstMoveOverList();
        }

        public void ResetFirstMoveOverList()
        {
            firstMoveOver.Clear();
        }

        public void ResetPossibleMovementsBlackFiguresDictionary()
        {
            possibleMovementsBlackFigures.Clear();
        }

        public void ResetPossibleMovementsWhiteFiguresDictionary()
        {
            possibleMovementsWhiteFigures.Clear();
        }

        public void AddFigureIdToFirstMoveOverList(Field[,] gameboard, int selectedFigureY, int selectedFigureX)
        {
            if (!firstMoveOver.Contains(gameboard[selectedFigureY, selectedFigureX].GetChessFigure().getID())) {
                firstMoveOver.Add(gameboard[selectedFigureY, selectedFigureX].GetChessFigure().getID());
            }
        }

        public void AddFigureIdToPossibleMovementsBlackFiguresDictionary(uint figureId)
        {
            if (!possibleMovementsBlackFigures.ContainsKey(figureId))
            {
                possibleMovementsBlackFigures.Add(figureId, new List<uint>());
            }
        }

        public void AddFigureIdToPossibleMovementsWhiteFiguresDictionary(uint figureId)
        {
            if (!possibleMovementsWhiteFigures.ContainsKey(figureId))
            {
                possibleMovementsWhiteFigures.Add(figureId, new List<uint>());
            }
        }

        public void AddFieldIdToPossibleMovementsWhiteFiguresDictionary(uint figureId, uint fieldId)
        {
            if (!possibleMovementsWhiteFigures[figureId].Contains(fieldId))
            {
                possibleMovementsWhiteFigures[figureId].Add(fieldId);
            }
        }

        public void AddFieldIdToPossibleMovementsBlackFiguresDictionary(uint figureId, uint fieldId)
        {
            if (!possibleMovementsBlackFigures[figureId].Contains(fieldId))
            {
                possibleMovementsBlackFigures[figureId].Add(fieldId);
            }
        }

        public void AddFieldIdToPossibleMovementsDictionary(uint figureId, Constants.ColorEnum color ,uint fieldId)
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

        public bool IsFieldOccupied(Field[,] gameboard, int rowNumber, int columnNumber)
        {
            return gameboard[rowNumber, columnNumber].GetIsFieldOccupied();
        }

        public bool CheckWhetherFigureBelongsPlayer(Field[,] gameboard, Constants.ColorEnum color, int rowNumber, int columnNumber)
        {
            bool sameColor = false;

            if (gameboard[rowNumber, columnNumber].GetChessFigure().getColor() == color)
            {
                sameColor = true;
            }

            return sameColor;
        }

        public bool IsEnemyField(Field[,] gameboard, Constants.ColorEnum color, int rowNumber, int columnNumber)
        {
            bool enemyField = false;

            if(IsFieldOccupied(gameboard, rowNumber, columnNumber))
            {
                enemyField = !CheckWhetherFigureBelongsPlayer(gameboard, color, rowNumber, columnNumber);
            }

            return enemyField;
        }

        public bool CkeckWhetherMovementIsCorrect(Field[,] gameboard, int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            uint figureId = gameboard[selectedFigureY, selectedFigureX].GetChessFigure().getID();
            Constants.ColorEnum color = gameboard[selectedFigureY, selectedFigureX].GetChessFigure().getColor();
            uint fieldId = gameboard[destinationFieldY, destinationFieldX].GetFieldID();
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

        public void RefreshPossibleMovementsDictionary(Field[,] gameboard)
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

                    if (gameboard[i, j].GetIsFieldOccupied())
                    {
                        bool isFirstMovmentOver = firstMoveOver.Contains(gameboard[i, j].GetChessFigure().getID());
                        Constants.ColorEnum color = gameboard[i, j].GetChessFigure().getColor();
                        uint figureId = gameboard[i, j].GetChessFigure().getID();

                        switch (gameboard[i, j].GetChessFigure())
                        {
                            case PawnFigur p:
                                bool enemyField;

                                if (!isFirstMovmentOver)
                                {
                                    if (color == Constants.ColorEnum.BLACK)
                                    {
                                        enemyField = IsEnemyField(gameboard, color, i + 2, j);

                                        if (!enemyField)
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j].GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        enemyField = IsEnemyField(gameboard, color, i - 2, j);

                                        if (!enemyField)
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j].GetFieldID());
                                        }
                                    }
                                }

                                if(i-1 >= 0)
                                {
                                    if(!IsFieldOccupied(gameboard, i - 1, j))
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j].GetFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsEnemyField(gameboard, color, i - 1, j - 1))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 1].GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsEnemyField(gameboard, color, i - 1, j + 1))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 1].GetFieldID());
                                        }
                                    }
                                }

                                if(i+1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (!IsFieldOccupied(gameboard, i + 1, j))
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j].GetFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsEnemyField(gameboard, color, i + 1, j - 1))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 1].GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsEnemyField(gameboard, color, i + 1, j + 1))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 1].GetFieldID());
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
                                        if(IsFieldOccupied(gameboard, i+k, j))
                                        {
                                            if(IsEnemyField(gameboard, color, i+k, j))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; i + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i + k, j))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; k + j < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; j + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                        }
                                    }
                                }

                                break;

                            case KnightFigur n:
                                
                                if(i + 2 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if(j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, i + 2, j - 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i + 2, j - 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j - 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j - 1].GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, i + 2, j + 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i + 2, j + 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j + 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 2, j + 1].GetFieldID());
                                        }
                                    }
                                }

                                if (i + 1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (j - 2 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, i + 1, j - 2))
                                        {
                                            if (IsEnemyField(gameboard, color, i + 1, j - 2))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 2].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 2].GetFieldID());
                                        }
                                    }

                                    if (j + 2 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, i + 1, j + 2))
                                        {
                                            if (IsEnemyField(gameboard, color, i + 1, j + 2))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 2].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 2].GetFieldID());
                                        }
                                    }
                                }

                                if (i - 2 >= 0)
                                {
                                    if (j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, i - 2, j - 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i - 2, j - 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j - 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j - 1].GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, i - 2, j + 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i - 2, j + 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j + 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 2, j + 1].GetFieldID());
                                        }
                                    }
                                }

                                if (i - 1 >= 0)
                                {
                                    if (j - 2 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, i - 1, j - 2))
                                        {
                                            if (IsEnemyField(gameboard, color, i - 1, j - 2))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 2].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 2].GetFieldID());
                                        }
                                    }

                                    if (j + 2 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, i - 1, j + 2))
                                        {
                                            if (IsEnemyField(gameboard, color, i - 1, j + 2))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 2].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 2].GetFieldID());
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
                                        if (IsFieldOccupied(gameboard, i + k, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i + k, j - k))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j - k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i - k, j - k))
                                        {
                                            if (IsEnemyField(gameboard, color, i - k, j - k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i - k, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i - k, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].GetFieldID());
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
                                        if (IsFieldOccupied(gameboard, i + k, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j + k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i + k, j - k))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j - k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j - k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i - k, j - k))
                                        {
                                            if (IsEnemyField(gameboard, color, i - k, j - k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j - k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i - k, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i - k, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - k, j + k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i + k, j))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; i + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i + k, j))
                                        {
                                            if (IsEnemyField(gameboard, color, i + k, j))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + k, j].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; k + j < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; j + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, i, j + k))
                                        {
                                            if (IsEnemyField(gameboard, color, i, j + k))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + k].GetFieldID());
                                        }
                                    }
                                }

                                break;
                            case KingFigur k:

                                if (i + 1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (IsFieldOccupied(gameboard, i + 1, j))
                                    {
                                        if (IsEnemyField(gameboard, color, i + 1, j))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j].GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j].GetFieldID());
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, i + 1, j + 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i + 1, j + 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j + 1].GetFieldID());
                                        }
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, i + 1, j - 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i + 1, j - 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i + 1, j - 1].GetFieldID());
                                        }
                                    }
                                }

                                if (j + 1 < Constants.GAMEBOARDWIDTH)
                                {
                                    if (IsFieldOccupied(gameboard, i, j + 1))
                                    {
                                        if (IsEnemyField(gameboard, color, i, j + 1))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + 1].GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j + 1].GetFieldID());
                                    }
                                }

                                if (j - 1 >= 0)
                                {
                                    if (IsFieldOccupied(gameboard, i, j - 1))
                                    {
                                        if (IsEnemyField(gameboard, color, i, j - 1))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j - 1].GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i, j  - 1].GetFieldID());
                                    }
                                }

                                if (i - 1 >= 0)
                                {
                                    if (IsFieldOccupied(gameboard, i - 1, j))
                                    {
                                        if (IsEnemyField(gameboard, color, i - 1, j))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j].GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j].GetFieldID());
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, i - 1, j + 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i - 1, j + 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j + 1].GetFieldID());
                                        }
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, i - 1, j - 1))
                                        {
                                            if (IsEnemyField(gameboard, color, i - 1, j - 1))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 1].GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard[i - 1, j - 1].GetFieldID());
                                        }
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }

        public bool CheckWhetherBlackKingIsChecked(Field[,] gameboard)
        {
            bool isChecked = false;

            uint fieldId = FindFieldIdOfKingWithColor(gameboard, Constants.ColorEnum.BLACK);

            isChecked = CanEnemyMoveOnField(fieldId, Constants.ColorEnum.BLACK);

            return isChecked;
        }

        private bool CanEnemyMoveOnField(uint fieldId, Constants.ColorEnum color)
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

        private uint FindFieldIdOfKingWithColor(Field[,] gameboard, Constants.ColorEnum color)
        {
            uint fieldId = 0;

            for(int i = 0; i < Constants.GAMEBOARDHEIGHT; i++)
            {
                for(int j = 0; j < Constants.GAMEBOARDWIDTH; j++)
                {
                    if(IsFieldOccupied(gameboard, i, j))
                    {
                        switch (gameboard[i,j].GetChessFigure())
                        {
                            case KingFigur k:
                                if(gameboard[i, j].GetChessFigure().getColor() == color)
                                {
                                    fieldId = gameboard[i, j].GetFieldID();
                                }
                                break;
                        }
                    }
                }
            }

            return fieldId;
        }

        public bool CheckWhetherWhiteKingIsChecked(Field[,] gameboard)
        {
            bool isChecked = false;

            uint fieldId = FindFieldIdOfKingWithColor(gameboard, Constants.ColorEnum.WHITE);

            isChecked = CanEnemyMoveOnField(fieldId, Constants.ColorEnum.WHITE);

            return isChecked;
        }

        public List<string> GetPossibleMovmentsOfFigures(List<ChessFigure> allFigures){
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