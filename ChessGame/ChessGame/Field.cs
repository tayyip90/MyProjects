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

        public uint getFieldID()
        {
            return fieldID;
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
                    symbol = string.Format("| {0,5}, {1,5}, {2,5} |", figureOnTheField.getID() , figureOnTheField.getSymbol(), figureOnTheField.getColor());
                }
            }
            else
            {
                symbol = string.Format("| {0,5}, {1, 12} |", fieldID , "Empty");
            }

            return symbol;
        }

        public void placeFigure(ChessFigure figure)
        {
            figureOnTheField = figure;
            isFieldOccupied = true;
        }

        public bool getIsFieldOccupied()
        {
            return isFieldOccupied;
        }

        public ChessFigure getChessFigure()
        {
            return this.figureOnTheField;
        }

        public ChessFigure removeFigure()
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

        public Constants.Row getRow()
        {
            return this.row;
        }

        public Constants.Column getColumn()
        {
            return this.column;
        }
    }
}
