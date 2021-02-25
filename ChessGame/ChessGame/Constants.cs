using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public static class Constants
    {
        public enum Column { A = 1, B, C, D, E, F, G, H }

        public enum Row { One = 1, Two, Three, Four, Five, Six, Seven, Eight }

        public enum FigurTyp { Pawn, Knight, Rook, Bishop, Queen, King }

        public enum ColorEnum { WHITE, BLACK }

        public const int GAMEBOARDHEIGHT = 8;

        public const int GAMEBOARDWIDTH = 8;

        public const char KINGSYMBOL = 'K';
        public const char QUEENSYMBOL = 'Q';
        public const char PAWNSYMBOL = 'P';
        public const char KNIGHTSYMBOL = 'N';
        public const char ROOKSYMBOL = 'R';
        public const char BISHOPSYMBOL = 'B';

        public const char RESETTURN = 'r';
        public const char QUIT = 'q';
        public const char NEWGAME = 'n';
        public const char HELP = 'h';

        public const string PLACEHOLDERSTRINGLINES = "-------------------------------------------------------------------------------------------------------------------------------------------------------";

        public const string PLACEHOLDERSTRINGSTARS =  "*******************************************************";
        public const string TITLE =             "* Chess Game                                          *";

        public const string QUITSTRING = "See you soon :'( ...";

        public const string SELECTEDFIELDISNOTOCCUPIED = "The Field you have selected is not occupied!";

        public const string SELECTEDFIELDISOCCUPIEDWITHENEMYFIGURE = "The Field you have selected is occupied, but the Figure belongs to the Enemy!";

        public static int convertRowCharToRowNumberForGameboard(char row)
        {
            int rowNumber = -1;

            switch (row)
            {
                case '1': rowNumber = 7; break;
                case '2': rowNumber = 6; break;
                case '3': rowNumber = 5; break;
                case '4': rowNumber = 4; break;
                case '5': rowNumber = 3; break;
                case '6': rowNumber = 2; break;
                case '7': rowNumber = 1; break;
                case '8': rowNumber = 0; break;
            }

            return rowNumber;
        }

        public static int convertRowEnumToRowNumberForGameboard(Row row)
        {
            int rowNumber = -1;

            switch (row)
            {
                case Row.One: rowNumber = 7; break;
                case Row.Two: rowNumber = 6; break;
                case Row.Three: rowNumber = 5; break;
                case Row.Four: rowNumber = 4; break;
                case Row.Five: rowNumber = 3; break;
                case Row.Six: rowNumber = 2; break;
                case Row.Seven: rowNumber = 1; break;
                case Row.Eight: rowNumber = 0; break;
            }

            return rowNumber;
        }

        public static int convertColumnCharToColumnNumberForGameboard(char column)
        {
            int columnNumber = -1;

            switch (column)
            {
                case 'a': 
                case 'A': columnNumber = 0; break;
                case 'b': 
                case 'B': columnNumber = 1; break;
                case 'c':  
                case 'C': columnNumber = 2; break;
                case 'd': 
                case 'D': columnNumber = 3; break;
                case 'e': 
                case 'E': columnNumber = 4; break;
                case 'f': 
                case 'F': columnNumber = 5; break;
                case 'g': 
                case 'G': columnNumber = 6; break;
                case 'h': 
                case 'H': columnNumber = 7; break;
            }

            return columnNumber;
        }

        public static int convertColumnEnumToColumnNumberForGameboard(Column column)
        {
            int columnNumber = -1;

            switch (column)
            {
                case Column.A: columnNumber = 0; break;
                case Column.B: columnNumber = 1; break;
                case Column.C: columnNumber = 2; break;
                case Column.D: columnNumber = 3; break;
                case Column.E: columnNumber = 4; break;
                case Column.F: columnNumber = 5; break;
                case Column.G: columnNumber = 6; break;
                case Column.H: columnNumber = 7; break;
            }

            return columnNumber;
        }
    }
}