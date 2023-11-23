using ChessGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Player
    {
        private string name;
        private Constants.ColorEnum color;
        public Player(string name, Constants.ColorEnum color)
        {
            this.name = name;
            this.color = color;
        }

        public Constants.ColorEnum GetColor()
        {
            return this.color;
        }

        public string GetName()
        {
            return this.name;
        }

        public ChessFigure MoveFigureToPosition(ref ChessGameboard gameboard, uint figureField, uint destinationField)
        {
            ChessFigure removedFigure = gameboard.GetField(destinationField).RemoveFigure();
            gameboard.GetField(destinationField).PlaceFigure(gameboard.GetField(figureField).RemoveFigure());

            return removedFigure;
        }
    }
}