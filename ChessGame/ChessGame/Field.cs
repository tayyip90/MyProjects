using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
   public class Field
   {
        private uint fieldID;
        private bool isFieldOccupied;
        private ChessFigure figureOnTheField;
        private Constants.Row row;
        private Constants.Column column; 
        private Constants.ColorEnum color;

        public Field(uint id, Constants.Row row, Constants.Column column, Constants.ColorEnum color)
        {
            Reset();
            fieldID = id;
            this.row = row;
            this.column = column;
            this.color = color;
        }

        public override string ToString()
        {
            return string.Format("| {0,5}, {1,1} C: {2,5} |", row, column, color); 
        }

        public uint GetFieldID()
        {
            return fieldID;
        }

        /// <summary>
        /// Method to print the Field in the Console.
        /// </summary>
        /// <returns>a string that represents actual state of Field</returns>
        public string GetSymbol()
        {
            string symbol = string.Empty;

            if (isFieldOccupied)
            {
                if(figureOnTheField != null)
                {
                    symbol = string.Format("| {0,5}, {1,5}, {2,5} |", GetFieldID(), figureOnTheField.getSymbol(), figureOnTheField.getColor());
                }
            }
            else
            {
                symbol = string.Format("| {0,5}, {1, 12} |", GetFieldID(), "Empty");
            }

            return symbol;
        }

        public void PlaceFigure(ChessFigure figure)
        {
            figureOnTheField = figure;
            isFieldOccupied = true;
        }

        public bool GetIsFieldOccupied()
        {
            return isFieldOccupied;
        }

        public ChessFigure GetChessFigure()
        {
            return this.figureOnTheField;
        }

        public ChessFigure RemoveFigure()
        {
            ChessFigure figureToRemove = this.figureOnTheField;
            this.Reset();
            return figureToRemove;
        }

        public void Reset()
        {
            isFieldOccupied = false;
            figureOnTheField = null;
        }

        public Constants.Row GetRow()
        {
            return this.row;
        }

        public Constants.Column GetColumn()
        {
            return this.column;
        }
    }
}
