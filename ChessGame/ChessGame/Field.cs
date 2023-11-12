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
            string symbol = string.Empty;

            if (isFieldOccupied)
            {
                if (figureOnTheField != null)
                {
                    symbol = string.Format("|{0,2},{1,1},{2,5}|", GetFieldID(), figureOnTheField.getSymbol(), figureOnTheField.getColor());
                }
            }
            else
            {
                symbol = string.Format("|{0,2},{1,7}|", GetFieldID(), "Empty");
            }

            return symbol;
        }

        public uint GetFieldID()
        {
            return fieldID;
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
