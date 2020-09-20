using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Game
    {
        private ChessLogic logic;
        private Player[] players;

        public Game(Player playerOne, Player playerTwo)
        {
            logic = new ChessLogic();
            players = new Player[2];

            players[0] = playerOne;
            players[1] = playerTwo;
        }
    }
}