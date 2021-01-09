using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Game
    {
        private ChessLogic logic;
        private Player[] players;

        private ChessGameboard gameboard;

        private List<ChessFigure> whitePlayerFiguresOutOfGame;
        private List<ChessFigure> blackPlayerFiguresOutOfGame;

        private List<ChessFigure> whitePlayerFigures;
        private List<ChessFigure> blackPlayerFigures;

        private int turnNumber;
        private Constants.ColorEnum playerTurn;

        private bool isGameFinished;
        private bool isPlayerQuit;

        private string input;
        private bool inputIsCorrect;

        private uint figureId;

        public Game()
        {
            whitePlayerFigures = new List<ChessFigure>();
            blackPlayerFigures = new List<ChessFigure>();

            blackPlayerFiguresOutOfGame = new List<ChessFigure>();
            whitePlayerFiguresOutOfGame = new List<ChessFigure>();

            logic = new ChessLogic();
            gameboard = new ChessGameboard();

            players = new Player[2];

            printIntro();
        }

        public void printIntro()
        {
            inputIsCorrect = false;
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine(Constants.TITLE);
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            showMenu();

            char[] inputAsCharArray = null;

            while (!inputIsCorrect)
            {
                input = Console.ReadLine();

                inputAsCharArray = (input.ToLower()).ToCharArray();

                if (input.Length == 1 & (inputAsCharArray[0] == Constants.NEWGAME || inputAsCharArray[0] == Constants.QUIT))
                {
                    inputIsCorrect = true;                
                }
                else
                {
                    showMenu();
                }
            }

            if (inputAsCharArray != null)
            {
                if (inputAsCharArray[0] == Constants.NEWGAME)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Blue;

                    newGame();

                    while (!isGameFinished & !isPlayerQuit)
                    {
                        gameboard.printBoardWithFigures();

                        input = Console.ReadLine();
                        inputAsCharArray = (input.ToLower()).ToCharArray();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(Constants.QUITSTRING);
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("Sorry Something went wrong!");
            }
        }

        public void showMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("Enter ...");
            Console.WriteLine("... " + Constants.NEWGAME + " to start a New Game");
            Console.WriteLine("... " + Constants.QUIT + " to quit the App");
            Console.WriteLine("");
        }

        public void newGame()
        {
            turnNumber = 1;
            playerTurn = Constants.ColorEnum.WHITE;
            figureId = 0;

            isPlayerQuit = false;
            isGameFinished = false;

            gameboard.resetGameboard();

            whitePlayerFigures.Clear();
            blackPlayerFigures.Clear();

            whitePlayerFiguresOutOfGame.Clear();
            blackPlayerFiguresOutOfGame.Clear();

            createWhitePlayerFigures();
            createBlackPlayerFigures();
        }

        private void createBlackPlayerFigures()
        {
            KingFigur blackPlayerKing = new KingFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerKing);
            gameboard.setFigureToPosition(blackPlayerKing, Constants.Row.Eight, Constants.Column.D);

            QueenFigur blackPlayerQueen = new QueenFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerQueen);
            gameboard.setFigureToPosition(blackPlayerQueen, Constants.Row.Eight, Constants.Column.E);

            BishopFigur blackPlayerBishop1 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerBishop1);
            gameboard.setFigureToPosition(blackPlayerBishop1, Constants.Row.Eight, Constants.Column.C);
            BishopFigur blackPlayerBishop2 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerBishop2);
            gameboard.setFigureToPosition(blackPlayerBishop2, Constants.Row.Eight, Constants.Column.F);

            KnightFigur blackPlayerKnight1 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerKnight1);
            gameboard.setFigureToPosition(blackPlayerKnight1, Constants.Row.Eight, Constants.Column.B);
            KnightFigur blackPlayerKnight2 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerKnight2);
            gameboard.setFigureToPosition(blackPlayerKnight2, Constants.Row.Eight, Constants.Column.G);

            RookFigur blackPlayerRook1 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerRook1);
            gameboard.setFigureToPosition(blackPlayerRook1, Constants.Row.Eight, Constants.Column.A);
            RookFigur blackPlayerRook2 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            blackPlayerFigures.Add(blackPlayerRook2);
            gameboard.setFigureToPosition(blackPlayerRook2, Constants.Row.Eight, Constants.Column.H);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur blackPlayerPawn = new PawnFigur(Constants.ColorEnum.BLACK, ++figureId);
                blackPlayerFigures.Add(blackPlayerPawn);
                gameboard.setFigureToPosition(blackPlayerPawn, Constants.Row.Seven, columnForPawns);
                columnForPawns++;
            }
        }

        private void createWhitePlayerFigures()
        {
            KingFigur whitePlayerKing = new KingFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerKing);
            gameboard.setFigureToPosition(whitePlayerKing, Constants.Row.One, Constants.Column.D);

            QueenFigur whitePlayerQueen = new QueenFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerQueen);
            gameboard.setFigureToPosition(whitePlayerQueen, Constants.Row.One, Constants.Column.E);

            BishopFigur whitePlayerBishop1 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerBishop1);
            gameboard.setFigureToPosition(whitePlayerBishop1, Constants.Row.One, Constants.Column.C);
            BishopFigur whitePlayerBishop2 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerBishop2);
            gameboard.setFigureToPosition(whitePlayerBishop2, Constants.Row.One, Constants.Column.F);

            KnightFigur whitePlayerKnight1 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerKnight1);
            gameboard.setFigureToPosition(whitePlayerKnight1, Constants.Row.One, Constants.Column.B);
            KnightFigur whitePlayerKnight2 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerKnight2);
            gameboard.setFigureToPosition(whitePlayerKnight2, Constants.Row.One, Constants.Column.G);

            RookFigur whitePlayerRook1 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerRook1);
            gameboard.setFigureToPosition(whitePlayerRook1, Constants.Row.One, Constants.Column.A);
            RookFigur whitePlayerRook2 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            whitePlayerFigures.Add(whitePlayerRook2);
            gameboard.setFigureToPosition(whitePlayerRook2, Constants.Row.One, Constants.Column.H);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur whitePlayerPawn = new PawnFigur(Constants.ColorEnum.WHITE, ++figureId);
                whitePlayerFigures.Add(whitePlayerPawn);
                gameboard.setFigureToPosition(whitePlayerPawn, Constants.Row.Two, columnForPawns);
                columnForPawns++;
            }
        }
    }
}