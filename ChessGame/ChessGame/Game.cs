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

        private bool newGame;
        private bool quit;

        private bool figureIsChosen;
        private bool destinationFieldIsChosen;

        private char input;
        private bool inputIsCorrect;

        private Field selectedField;
        private Field destinationField;

        private uint figureId;

        private char row;
        private char column;

        public Game()
        {
            whitePlayerFigures = new List<ChessFigure>();
            blackPlayerFigures = new List<ChessFigure>();

            blackPlayerFiguresOutOfGame = new List<ChessFigure>();
            whitePlayerFiguresOutOfGame = new List<ChessFigure>();

            logic = new ChessLogic();
            gameboard = new ChessGameboard();

            players = new Player[2];

            newGame = false;
            quit = false;
        }

        public void start()
        {
            while (!quit) {

                printIntro();

                if (newGame)
                {
                    NewGame();
                }
            }

            Console.Clear();
            Console.WriteLine(Constants.QUITSTRING);
            Console.WriteLine("");
        }

        private void printIntro()
        {
            inputIsCorrect = false;

            Console.Clear();

            showMenu();

            while (!inputIsCorrect)
            {
                input = Console.ReadKey().KeyChar;

                if (input == Constants.NEWGAME)
                {
                    inputIsCorrect = true;
                    newGame = true;
                }
                else if (input == Constants.QUIT)
                {
                    inputIsCorrect = true;
                    quit = true;
                }
                else if (input == Constants.HELP)
                {
                    displayInputFormat();

                    Console.WriteLine();
                    Console.Write("Press any Key to return to Menu ...");
                    Console.ReadKey();

                    Console.Clear();

                    showMenu();
                }
                else
                {
                    Console.Clear();

                    showMenu();
                }
            }
        }

        private void displayInputIsEmptyOrHasNotTheRightFormat()
        {
            Console.WriteLine("");
            Console.WriteLine("Input was Empty or has not the right Format!");
            Console.WriteLine("");
            displayInputFormat();
        }

        private void displayInputFormat()
        {
            Console.WriteLine("");
            Console.WriteLine("At first the Player has to choose the Field where the Figure is placed.");
            Console.WriteLine("At next the Player has to choose the Destination Field.");
            Console.WriteLine("To choose the Field the Player enters the Row, and then the Column");
            Console.WriteLine("");
        }

        private void showMenu()
        {
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine(Constants.TITLE);
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine("");
            Console.WriteLine("Enter ...");
            Console.WriteLine("... " + Constants.NEWGAME + " to start a New Game");
            Console.WriteLine("... " + Constants.QUIT + " to quit the App");
            Console.WriteLine("... " + Constants.HELP + " to show help");
            Console.WriteLine("");
        }

        private void NewGame()
        {
            Console.BackgroundColor = ConsoleColor.Blue;

            resetGame();

            while (!isGameFinished & !isPlayerQuit)
            {
                Console.Clear();

                gameboard.printBoardWithFigures();

                while (!figureIsChosen)
                {
                    figureIsChosen = readField();
                }

                while (!destinationFieldIsChosen)
                {
                    destinationFieldIsChosen = readField();
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private bool readField()
        {
            inputIsCorrect = false;
            bool fieldIsChosen;

            while (!inputIsCorrect | isPlayerQuit)
            {

            }

            inputIsCorrect = false;

            while (!inputIsCorrect | isPlayerQuit)
            {

            }

            if (isPlayerQuit)
            {
                fieldIsChosen = false;
            }
            else
            {
                fieldIsChosen = logic.isFigureOccupiedAndFigureBelongsThePlayer(gameboard, playerTurn, row, column);
            }

            return fieldIsChosen;
        }

        private void resetGame()
        {
            turnNumber = 1;
            playerTurn = Constants.ColorEnum.WHITE;
            figureId = 0;

            figureIsChosen = false;
            destinationFieldIsChosen = false;

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