using System;

namespace ChessGame.Extensions
{
    public static class ArrayExtensions
    {
        public static bool FindInTwoDimensional<T>(this T[,] array, Predicate<T> predicate)
        {
            foreach (var item in array)
            {
                if (predicate(item)) return true;
            }

            return false;
        }

        public static T FindMatching<T>(this T[,] array, Predicate<T> predicate)
        {
            foreach (var item in array)
            {  
                if (predicate(item)) return item;
            }

            return default(T);
        }
    }
}
