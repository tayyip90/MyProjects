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
    }
}