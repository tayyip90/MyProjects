﻿using System;
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

        public void Reset()
        {
            isFieldOccupied = false;
            figureOnTheField = null;
        }
   }
}