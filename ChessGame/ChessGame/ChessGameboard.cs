using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessGameboard
    {
        private Field[,] Gameboard;

        public ChessGameboard(){
            Gameboard = new Field[Constants.GAMEBOARDHEIGHT,Constants.GAMEBOARDWIDTH];

            Constants.Row rowEnum = Constants.Row.Eight;
            Constants.ColorEnum color;

            for (int row = 0; row < Constants.GAMEBOARDHEIGHT; row++)
            {
                Constants.Column columnEnum = Constants.Column.A;

                if ((int)rowEnum % 2 == 0) {
                    color = Constants.ColorEnum.WHITE;
                } 
                else
                {
                    color = Constants.ColorEnum.BLACK;
                }

                for (int column = 0; column  < Constants.GAMEBOARDWIDTH; column++)
                {
                    Gameboard[row, column] = new Field(rowEnum, columnEnum, color);

                    columnEnum++;
                    if (color == Constants.ColorEnum.BLACK)
                    {
                        color = Constants.ColorEnum.WHITE;
                    }
                    else
                    {
                        color++;
                    }
                }

                rowEnum--;
            }
        }

        public void resetGameboard()
        {
            foreach(Field field in Gameboard)
            {
                field.Reset();
            }
        }

        public void setFigureToPosition(ChessFigure figure, Constants.Row row, Constants.Column column)
        {
            int rowIndex = Constants.GAMEBOARDHEIGHT - (int)row;
            int columnIndex = (int)column - 1;

            Gameboard[rowIndex, columnIndex].placeFigure(figure);
        }

        public Field[,] getBoard(){
            return this.Gameboard;
        }

        /// <summary>
        /// prints only Board without Figures
        /// </summary>
        public void printBoard()
        {
            string line;
            
            string headerLine = "       ";

            foreach (Constants.Column columnHeader in Enum.GetValues(typeof(Constants.Column)))
            {
                headerLine += string.Format("| {0,17} |", columnHeader) + " ";
            }

            headerLine += "      ";

            Console.WriteLine(headerLine);

            Constants.Row rowHeader = Constants.Row.Eight;


            for(int row = 0; row < Constants.GAMEBOARDHEIGHT; row++)
            {
                line = string.Empty;

                for (int column = 0; column < Constants.GAMEBOARDWIDTH; column++)
                {
                    line += Gameboard[row, column] + " ";
                }

                Console.WriteLine(string.Format("{0,6}",rowHeader) + " " + line + " " + string.Format("{0,6}", rowHeader));
                rowHeader--;
            }

            Console.WriteLine(headerLine);
        }

        /// <summary>
        /// prints only Board with Figures
        /// </summary>
        public void printBoardWithFigures()
        {
            string line;

            string headerLine = "       ";

            foreach (Constants.Column columnHeader in Enum.GetValues(typeof(Constants.Column)))
            {
                headerLine += string.Format("| {0,12} |", columnHeader) + " ";
            }

            headerLine += "      ";

            Console.WriteLine(headerLine);

            Constants.Row rowHeader = Constants.Row.Eight;


            for (int row = 0; row < Constants.GAMEBOARDHEIGHT; row++)
            {
                line = string.Empty;

                for (int column = 0; column < Constants.GAMEBOARDWIDTH; column++)
                {
                    line += Gameboard[row, column].getSymbol() + " ";
                }

                Console.WriteLine(string.Format("{0,6}", rowHeader) + " " + line + " " + string.Format("{0,6}", rowHeader));
                rowHeader--;
            }

            Console.WriteLine(headerLine);
        }
    }
}