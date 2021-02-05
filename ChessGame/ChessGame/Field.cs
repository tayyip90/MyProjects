using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
   public class Field
   {
        private bool isFieldOccupied;
        private ChessFigure figureOnTheField;
        private Constants.Row row;
        private Constants.Column column; 
        private Constants.ColorEnum color;

        public Field(Constants.Row row, Constants.Column column, Constants.ColorEnum color)
        {
            Reset();
            this.row = row;
            this.column = column;
            this.color = color;
        }

        public override string ToString()
        {
            return string.Format("| {0,5}, {1,1} C: {2,5} |", row, column, color); 
        }

        /// <summary>
        /// Method to print the Field in the Console.
        /// </summary>
        /// <returns>a string that represents actual state of Field</returns>
        public string getSymbol()
        {
            string symbol = string.Empty;

            if (isFieldOccupied)
            {
                if(figureOnTheField != null)
                {
                    symbol = string.Format("| {0,5}, {1,5} |", figureOnTheField.getSymbol(), figureOnTheField.getColor());
                }
            }
            else
            {
                symbol = string.Format("| {0, 12} |", "Empty");
            }

            return symbol;
        }

        public void placeFigure(ChessFigure figure)
        {
            if (isFieldOccupied) {
                Console.WriteLine("Field is Occupied!");
            }
            else
            {
                figureOnTheField = figure;
                isFieldOccupied = true;
            }
        }

        public bool getIsFieldOccupied()
        {
            return isFieldOccupied;
        }

        public ChessFigure getChessFigure()
        {
            return this.figureOnTheField;
        }

        public void Reset()
        {
            isFieldOccupied = false;
            figureOnTheField = null;
        }
   }
}
