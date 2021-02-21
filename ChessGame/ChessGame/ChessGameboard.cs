using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ChessGameboard
    {
        /*      
         *      gameboard is a array with 8 height and 8 width
         *      
         *      gameboard[0, 0] is up left
         *      
         *      +------+------+------+------+------+------+------+------+
         *      | 8, A | 8, B | 8, C | 8, D | 8, E | 8, F | 8, G | 8, H |
         *      +------+------+------+------+------+------+------+------+
         *      | 7, A | 7, B | 7, C | 7, D | 7, E | 7, F | 7, G | 7, H |
         *      +------+------+------+------+------+------+------+------+
         *      | 6, A | 6, B | 6, C | 6, D | 6, E | 6, F | 6, G | 6, H |  
         *      +------+------+------+------+------+------+------+------+
         *      | 5, A | 5, B | 5, C | 5, D | 5, E | 5, F | 5, G | 5, H |
         *      +------+------+------+------+------+------+------+------+
         *      | 4, A | 4, B | 4, C | 4, D | 4, E | 4, F | 4, G | 4, H |   
         *      +------+------+------+------+------+------+------+------+
         *      | 3, A | 3, B | 3, C | 3, D | 3, E | 3, F | 3, G | 3, H |
         *      +------+------+------+------+------+------+------+------+
         *      | 2, A | 2, B | 2, C | 2, D | 2, E | 2, F | 2, G | 2, H |  
         *      +------+------+------+------+------+------+------+------+
         *      | 1, A | 1, B | 1, C | 1, D | 1, E | 1, F | 1, G | 1, H |
         *      +------+------+------+------+------+------+------+------+
         */

        private Field[,] Gameboard;

        public ChessGameboard(){
            uint id = 1;

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
                    Gameboard[row, column] = new Field(id, rowEnum, columnEnum, color);
                    id++;

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

        public void setFigureToPosition(ChessFigure figure, int rowNumber, int columnNumber)
        {
            Gameboard[rowNumber, columnNumber].placeFigure(figure);
        }

        public void setFigureToPosition(ChessFigure figure, Constants.Row row, Constants.Column column)
        {
            int rowNumber, columnNumber;

            rowNumber = Constants.convertRowEnumToRowNumberForGameboard(row);
            columnNumber = Constants.convertColumnEnumToColumnNumberForGameboard(column);

            if (rowNumber != -1 | columnNumber != -1)
            {
                Gameboard[Constants.convertRowEnumToRowNumberForGameboard(row), Constants.convertColumnEnumToColumnNumberForGameboard(column)].placeFigure(figure);
            }
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

        public void moveFigureToPosition(int selectedFigureX, int selectedFigureY, int destinationFieldX, int destinationFieldY)
        {
            Gameboard[destinationFieldY, destinationFieldX].placeFigure(Gameboard[selectedFigureY, selectedFigureX].removeFigure()); 
        }
    }
}