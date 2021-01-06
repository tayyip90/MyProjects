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
    }
}