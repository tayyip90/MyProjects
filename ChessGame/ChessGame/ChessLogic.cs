using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ChessGame.Extensions;
using ChessGame.Models;

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

        public void removeFromPossibleMovementsWhiteFigures(uint figureId)
        {
            possibleMovementsWhiteFigures.Remove(figureId);
        }

        public void removeFromPossibleMovementsBlackFigures(uint figureId)
        {
            possibleMovementsBlackFigures.Remove(figureId);
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

        public void AddFigureIdToFirstMoveOverList(uint figureId)
        {
            if (!firstMoveOver.Contains(figureId)) {
                firstMoveOver.Add(figureId);
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

        public bool IsFieldOccupied(ChessGameboard gameboard, uint fieldId)
        {
            return gameboard.IsFieldOccupied(fieldId);
        }

        public bool CheckWhetherFigureBelongsPlayer(ChessGameboard gameboard, Constants.ColorEnum color, uint fieldId)
        {
            return gameboard.GetBoard().FindInTwoDimensional(field => field.GetFieldID() == fieldId && field.GetChessFigure().GetColor() == color);
        }

        public bool IsEnemyField(ChessGameboard gameboard, Constants.ColorEnum color, uint fieldId)
        {
            bool enemyField = false;

            if(IsFieldOccupied(gameboard, fieldId))
            {
                enemyField = !CheckWhetherFigureBelongsPlayer(gameboard, color, fieldId);
            }

            return enemyField;
        }

        public bool CkeckWhetherMovementIsCorrect(ChessGameboard gameboard, Constants.ColorEnum actualPlayerColor, uint figureField, uint destinationField)
        {
            if (gameboard.GetField(figureField) == null) return false;
            if (gameboard.GetField(destinationField) == null) return false;

            uint figureId = gameboard.GetField(figureField).GetChessFigure().GetID();

            if (actualPlayerColor != gameboard.GetField(figureField).GetChessFigure().GetColor()) return false;

            bool isCorrect;

            if (actualPlayerColor == Constants.ColorEnum.WHITE)
            {
                isCorrect = possibleMovementsWhiteFigures[figureId].Contains(destinationField);
            }
            else
            {
                isCorrect = possibleMovementsBlackFigures[figureId].Contains(destinationField);
            }

            return isCorrect;
        }

        public void RefreshPossibleMovementsDictionary(ChessGameboard gameboard)
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

                    if (gameboard.GetField(i,j).IsFieldOccupied())
                    {
                        bool isFirstMovmentOver = firstMoveOver.Contains(gameboard.GetField(i,j).GetChessFigure().GetID());
                        Constants.ColorEnum color = gameboard.GetField(i,j).GetChessFigure().GetColor();
                        uint figureId = gameboard.GetField(i,j).GetChessFigure().GetID();

                        switch (gameboard.GetField(i,j).GetChessFigure())
                        {
                            case PawnFigur p:

                                if (!isFirstMovmentOver)
                                {
                                    if (color == Constants.ColorEnum.BLACK)
                                    {
                                        if (!IsEnemyField(gameboard, color, gameboard.GetField(i + 2, j).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 2, j).GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        if (!IsEnemyField(gameboard, color, gameboard.GetField(i - 2, j).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 2, j).GetFieldID());
                                        }
                                    }
                                }

                                if(i-1 >= 0)
                                {
                                    if(!IsFieldOccupied(gameboard, gameboard.GetField(i - 1, j).GetFieldID()))
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j).GetFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i - 1, j - 1).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j - 1).GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i - 1, j + 1).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j + 1).GetFieldID());
                                        }
                                    }
                                }

                                if(i+1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (!IsFieldOccupied(gameboard, gameboard.GetField(i + 1, j).GetFieldID()))
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j).GetFieldID());
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i + 1, j - 1).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j - 1).GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i + 1, j + 1).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j + 1).GetFieldID());
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
                                        if(IsFieldOccupied(gameboard, gameboard.GetField(i + k, j).GetFieldID()))
                                        {
                                            if(IsEnemyField(gameboard, color, gameboard.GetField(i + k, j).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; i + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i + k, j).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i + k, j).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; k + j < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i , j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i , j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; j + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i , j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i , j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                        }
                                    }
                                }

                                break;

                            case KnightFigur n:
                                
                                if(i + 2 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if(j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i + 2 , j  - 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i + 2 , j  - 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 2 , j  - 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 2 , j  - 1).GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField( i + 2, j + 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField( i + 2, j + 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 2, j + 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 2, j + 1).GetFieldID());
                                        }
                                    }
                                }

                                if (i + 1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (j - 2 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField( i + 1, j - 2).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField( i + 1, j - 2).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField( i + 1, j - 2).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField( i + 1, j - 2).GetFieldID());
                                        }
                                    }

                                    if (j + 2 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i + 1, j + 2).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i + 1, j + 2).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j + 2).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j + 2).GetFieldID());
                                        }
                                    }
                                }

                                if (i - 2 >= 0)
                                {
                                    if (j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - 2, j - 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - 2, j - 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 2, j - 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 2, j - 1).GetFieldID());
                                        }
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - 2, j + 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - 2, j + 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 2, j + 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 2, j + 1).GetFieldID());
                                        }
                                    }
                                }

                                if (i - 1 >= 0)
                                {
                                    if (j - 2 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - 1, j - 2).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - 1, j - 2).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j - 2).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j - 2).GetFieldID());
                                        }
                                    }

                                    if (j + 2 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - 1, j + 2).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - 1, j + 2).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j + 2).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j + 2).GetFieldID());
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
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i + k, j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i + k, j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j + k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i + k, j - k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i + k, j - k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j - k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j - k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - k, j - k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - k, j - k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j - k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j - k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - k, j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - k, j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j + k).GetFieldID());
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
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i + k, j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i + k, j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j + k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i + k, j - k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i + k, j - k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j - k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j - k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j - k >= 0; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - k, j - k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - k, j - k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j - k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j - k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i - k >= 0 & j + k < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i - k, j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i - k, j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - k, j + k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; i + k < Constants.GAMEBOARDHEIGHT; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i + k, j).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i + k, j).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; i + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i + k, j).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i + k, j).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + k, j).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = 1; k + j < Constants.GAMEBOARDWIDTH; k++)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i , j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i , j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                        }
                                    }
                                }

                                foundFieldWithFigure = false;

                                for (int k = -1; j + k >= 0; k--)
                                {
                                    if (!foundFieldWithFigure)
                                    {
                                        if (IsFieldOccupied(gameboard,  gameboard.GetField(i , j + k).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color,  gameboard.GetField(i , j + k).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                            }

                                            foundFieldWithFigure = true;
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i , j + k).GetFieldID());
                                        }
                                    }
                                }

                                break;
                            case KingFigur k:

                                if (i + 1 < Constants.GAMEBOARDHEIGHT)
                                {
                                    if (IsFieldOccupied(gameboard, gameboard.GetField(i + 1, j).GetFieldID()))
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i + 1, j).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j).GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j).GetFieldID());
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i + 1, j + 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i + 1, j + 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j + 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j + 1).GetFieldID());
                                        }
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i + 1, j - 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i + 1, j - 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j - 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i + 1, j - 1).GetFieldID());
                                        }
                                    }
                                }

                                if (j + 1 < Constants.GAMEBOARDWIDTH)
                                {
                                    if (IsFieldOccupied(gameboard, gameboard.GetField(i, j + 1).GetFieldID()))
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i, j + 1).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i, j + 1).GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i, j + 1).GetFieldID());
                                    }
                                }

                                if (j - 1 >= 0)
                                {
                                    if (IsFieldOccupied(gameboard, gameboard.GetField(i, j - 1).GetFieldID()))
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i, j - 1).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i, j - 1).GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i, j - 1).GetFieldID());
                                    }
                                }

                                if (i - 1 >= 0)
                                {
                                    if (IsFieldOccupied(gameboard, gameboard.GetField(i - 1, j).GetFieldID()))
                                    {
                                        if (IsEnemyField(gameboard, color, gameboard.GetField(i - 1, j).GetFieldID()))
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j).GetFieldID());
                                        }
                                    }
                                    else
                                    {
                                        AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j).GetFieldID());
                                    }

                                    if (j + 1 < Constants.GAMEBOARDWIDTH)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i - 1, j + 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i - 1, j + 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j + 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j + 1).GetFieldID());
                                        }
                                    }

                                    if (j - 1 >= 0)
                                    {
                                        if (IsFieldOccupied(gameboard, gameboard.GetField(i - 1, j - 1).GetFieldID()))
                                        {
                                            if (IsEnemyField(gameboard, color, gameboard.GetField(i - 1, j - 1).GetFieldID()))
                                            {
                                                AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j - 1).GetFieldID());
                                            }
                                        }
                                        else
                                        {
                                            AddFieldIdToPossibleMovementsDictionary(figureId, color, gameboard.GetField(i - 1, j - 1).GetFieldID());
                                        }
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }

        public bool CheckWhetherBlackKingIsChecked(ChessGameboard gameboard)
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

        private uint FindFieldIdOfKingWithColor(ChessGameboard gameboard, Constants.ColorEnum color)
        {
            uint fieldId = 0;

            for(int i = 0; i < Constants.GAMEBOARDHEIGHT; i++)
            {
                for(int j = 0; j < Constants.GAMEBOARDWIDTH; j++)
                {
                    if(IsFieldOccupied(gameboard, gameboard.GetField(i, j).GetFieldID()))
                    {
                        switch (gameboard.GetField(i, j).GetChessFigure())
                        {
                            case KingFigur k:
                                if(gameboard.GetField(i,j).GetChessFigure().GetColor() == color)
                                {
                                    fieldId = gameboard.GetField(i,j).GetFieldID();
                                }
                                break;
                        }
                    }
                }
            }

            return fieldId;
        }

        public bool CheckWhetherWhiteKingIsChecked(ChessGameboard gameboard)
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
                ChessFigure figure = allFigures.Find(x => x.GetID() == key);

                possibleMovementOfFigure = string.Empty;
                possibleMovementOfFigure += "Figure ID: " + figure.GetID();

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
                ChessFigure figure = allFigures.Find(x => x.GetID() == key);

                possibleMovementOfFigure = string.Empty;
                possibleMovementOfFigure += "Figure ID: " + figure.GetID();

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

        public bool CheckFieldId(ChessGameboard gameboard, uint fieldId)
        {
            return gameboard.GetBoard().FindInTwoDimensional(field => field.GetFieldID() == fieldId);
        }
    }
}