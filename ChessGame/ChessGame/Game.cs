using System;
using System.Collections.Generic;
using System.Text;
using ChessGame.Extensions;
using ChessGame.Models;

namespace ChessGame
{
    public class Game
    {
        private ChessLogic logic;
        private Player[] players;
        private List<string> log;

        private ChessGameboard gameboard;

        private List<ChessFigure> whitePlayerFiguresOutOfGame;
        private List<ChessFigure> blackPlayerFiguresOutOfGame;

        private List<ChessFigure> allFigures;

        public Game()
        {
            allFigures = new List<ChessFigure>();
            log = new List<string>();

            blackPlayerFiguresOutOfGame = new List<ChessFigure>();
            whitePlayerFiguresOutOfGame = new List<ChessFigure>();

            logic = new ChessLogic();
            gameboard = new ChessGameboard();

            players = new Player[2];
        }

        public void Intro()
        {
            bool quit = false;
            bool newGame = false;

            while (!quit)
            {
                PrintIntroText(out quit, out newGame);

                if (newGame)
                {
                    Console.Clear();
                    Start(out quit);
                }
            }

            Console.Clear();
            Console.WriteLine(Constants.QUITSTRING);
            Console.WriteLine("");
        }

        private void PrintIntroText(out bool quit, out bool newGame)
        {
            quit = false;
            newGame = false;

            Console.Clear();

            ShowMenu();

            while (!quit && !newGame)
            {
                var input = Console.ReadKey().KeyChar;

                if (input == Constants.NEWGAME)
                {
                    newGame = true;
                }
                else if (input == Constants.QUIT)
                {
                    quit = true;
                }
                else if (input == Constants.HELP)
                {
                    DisplayInputFormat();

                    Console.WriteLine();
                    Console.Write("Press any Key to return to Menu ...");
                    Console.ReadKey();

                    Console.Clear();

                    ShowMenu();
                }
                else
                {
                    Console.Clear();

                    ShowMenu();
                }
            }
        }

        private void DisplayInputIsEmptyOrHasNotTheRightFormat()
        {
            Console.WriteLine("");
            Console.WriteLine("Input was Empty or has not the right Format!");
            Console.WriteLine("");
            DisplayInputFormat();
        }

        private void DisplayInputFormat()
        {
            Console.WriteLine("");
            Console.WriteLine("At first the Player has to choose the Field where the Figure is placed.");
            Console.WriteLine("At next the Player has to choose the Destination Field.");
            Console.WriteLine("To choose the Field the Player enters the Row, and then the Column");
            Console.WriteLine("");
        }

        private void ShowMenu()
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

        private void Start(out bool quit)
        {
            uint turnNumber = 1,figureField = 0, destinationField = 0, playerTurn = 0;
            bool resetTurn = false, gameFinished = false, whiteKingIsChecked = false, blackKingIsChecked = false, blackPlayerWon = false, whitePlayerWon = false; 
            quit = false;

            SetNameOfPlayers();

            Console.BackgroundColor = ConsoleColor.Blue;

            ResetGame();

            while (!gameFinished && !quit)
            {
                do
                {
                    // set reset turn false
                    if (resetTurn) resetTurn = false;

                    Console.WriteLine("Turn Number: " + turnNumber + " " + players[playerTurn].GetName() + "'s turn!" + " Color: " + players[playerTurn].GetColor());
                    Console.WriteLine(string.Empty);

                    PrintPossibleMovements();
                    PrintKingIsCheckedStatus(whiteKingIsChecked, blackKingIsChecked);
                    PrintLog();
                    PrintWhiteFiguresOutOfGame();
                    PrintBlackFiguresOutOfGame();
                    PrintBoardWithFigures(gameboard);

                    PrintTextSelectFigureForPlayer();
                    figureField = SelectFigure(out quit, out resetTurn);
                    Console.WriteLine();

                    PrintTextSelectDestinationField();
                    destinationField = SelectDestination(out quit, out resetTurn);
                    Console.WriteLine();

                } while (resetTurn);

                if (!quit)
                {
                    if (logic.CkeckWhetherMovementIsCorrect(gameboard, players[playerTurn].GetColor(), figureField, destinationField))
                    {
                        string removedFigureText = string.Empty;

                        logic.AddFigureIdToFirstMoveOverList(gameboard.GetField(figureField).GetChessFigure().GetID());

                        string logText = players[playerTurn].GetName() + " / " + players[playerTurn].GetColor() + " Player moved his ";
                        logText += gameboard.GetField(figureField).GetChessFigure().ToString() + " to "
                            + gameboard.GetField(destinationField).GetRow() + ", " + gameboard.GetField(destinationField).GetColumn();

                        ChessFigure removedFigure = players[playerTurn].MoveFigureToPosition(ref gameboard, figureField, destinationField);
                        
                        if (removedFigure != null)
                        {
                            if (players[playerTurn].GetColor() == Constants.ColorEnum.WHITE)
                            {
                                removedFigureText = "removed the " + removedFigure.ToString() + " of black player!";
                                logic.removeFromPossibleMovementsBlackFigures(removedFigure.GetID());
                                blackPlayerFiguresOutOfGame.Add(removedFigure);
                            }
                            else
                            {
                                removedFigureText = "removed the " + removedFigure.ToString() + " of white player!";
                                logic.removeFromPossibleMovementsWhiteFigures(removedFigure.GetID());
                                whitePlayerFiguresOutOfGame.Add(removedFigure);
                            }
                        }

                        if (removedFigureText != string.Empty) logText += " and " + removedFigureText;

                        log.Add(logText);

                        whiteKingIsChecked = logic.CheckWhetherWhiteKingIsChecked(gameboard);
                        blackKingIsChecked = logic.CheckWhetherBlackKingIsChecked(gameboard);

                        if (whiteKingIsChecked && players[playerTurn].GetColor() == Constants.ColorEnum.WHITE)
                        {
                            blackPlayerWon = whiteKingIsChecked = logic.CheckWhetherWhiteKingIsChecked(gameboard);
                            gameFinished = blackPlayerWon;
                        }

                        if (blackKingIsChecked && players[playerTurn].GetColor() == Constants.ColorEnum.BLACK)
                        {
                            whitePlayerWon = blackKingIsChecked = logic.CheckWhetherBlackKingIsChecked(gameboard);
                            gameFinished = whitePlayerWon;
                        }

                        playerTurn = SetNextPlayer(playerTurn);

                        turnNumber++;

                        logic.RefreshPossibleMovementsDictionary(gameboard);

                        Console.Clear();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This is not a Correct movement!");
                        Console.Beep();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                    }
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;

            if (quit)
            {
                Console.WriteLine("Player has quit the Game!");
            }

            if (blackPlayerWon | whitePlayerWon)
            {
                DisplayPlayerWon(whitePlayerWon, blackPlayerWon);
            }
        }

        private void PrintBlackFiguresOutOfGame()
        {
            Console.WriteLine("Black Figures out of Game:");
            Console.WriteLine();
            foreach(ChessFigure figure in blackPlayerFiguresOutOfGame)
            {
                Console.WriteLine(figure);
            }
            Console.WriteLine();
        }

        private void PrintWhiteFiguresOutOfGame()
        {
            Console.WriteLine("White Figures out of Game:");
            Console.WriteLine();
            foreach (ChessFigure figure in whitePlayerFiguresOutOfGame)
            {
                Console.WriteLine(figure);
            }
            Console.WriteLine();
        }

        private void DisplayPlayerWon(bool whitePlayerWon, bool blackPlayerWon)
        {
            Console.Clear();
            Console.WriteLine(Constants.PLAYERWONTEXT);

            if (whitePlayerWon)
            {
                Console.WriteLine(players[0].GetName() + " has won!");
            }

            if(blackPlayerWon)
            {
                Console.WriteLine(players[1].GetName() + " has won!");
            }

            Console.ReadKey();
        }

        private void PrintKingIsCheckedStatus(bool whiteKingIsChecked, bool blackKingIsChecked)
        {
            if (whiteKingIsChecked)
            {
                Console.WriteLine("The White King is checked!");
            }
            else
            {
                Console.WriteLine("The White King is not checked!");
            }

            if (blackKingIsChecked)
            {
                Console.WriteLine("The Black King is checked!");
            }
            else
            {
                Console.WriteLine("The Black King is not checked!");
            }

            Console.WriteLine(string.Empty);
        }

        private void SetNameOfPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                SetName(i);
            }
        }

        private void SetName(int playernumber)
        {
            ShowTitle();
            string name = string.Empty;
            do
            {
                Console.WriteLine("Please Enter your name Player " + (playernumber + 1));
                name = Console.ReadLine();
                Console.WriteLine(string.Empty);
                if (name == string.Empty)
                {
                    Console.WriteLine("It is not allowed to enter a empty name!");
                    Console.WriteLine(string.Empty);
                }
            }
            while (name == string.Empty);

            if (playernumber == 0)
            {
                players[playernumber] = new Player(name, Constants.ColorEnum.WHITE);
            }
            else
            {
                players[playernumber] = new Player(name, Constants.ColorEnum.BLACK);
            }

            Console.Clear();
        }

        private void ShowTitle()
        {
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine(Constants.TITLE);
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine("");
        }

        private void PrintPossibleMovements()
        {
            List<string> possibleMovements = logic.GetPossibleMovmentsOfFigures(allFigures);

            foreach(string line in possibleMovements)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine(string.Empty);
        }

        private uint SetNextPlayer(uint playerTurn)
        {
            return playerTurn == 0 ? (uint)1 : (uint)0;
        }

        private void PrintTextSelectDestinationField()
        {
            Console.WriteLine("Select the Destination Field!");
        }

        private void PrintTextSelectFigureForPlayer()
        {
            Console.WriteLine("Select your Figure, that you want to move!");
        }

        private uint SelectDestination(out bool quit, out bool resetTurn)
        {
            bool inputIsCorrect = false;
            quit = false;
            resetTurn = false;
            uint fieldId = 0;

            while (!inputIsCorrect & !quit & !resetTurn)
            {
                Console.WriteLine("Type in the FieldId of the Destination Field!");
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    if (input.Length == 1 && input.StartsWith(Constants.QUIT))
                    {
                        quit = true;
                    }
                    else if (input.Length == 1 && input.StartsWith(Constants.RESETTURN))
                    {
                        resetTurn = true;
                    }
                    else
                    {
                        if (uint.TryParse(input, out fieldId) && gameboard.GetBoard().FindInTwoDimensional(field => field.GetFieldID() == fieldId)) inputIsCorrect = true;
                        else Console.WriteLine("please enter a existing fieldId!");
                    }
                }
                else
                {
                    Console.WriteLine("input was incorrect!");
                    Console.WriteLine();
                }
            }

            return fieldId;
        }

        private uint SelectFigure(out bool quit, out bool resetTurn)
        {
            bool inputIsCorrect = false;
            quit = false;
            resetTurn = false;
            uint fieldId = 0;

            while (!inputIsCorrect & !quit & !resetTurn)
            {
                Console.WriteLine("Type in the FieldId, where your Figure is occupied!");
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    if (input.Length == 1 && input.StartsWith(Constants.QUIT))
                    {
                        quit = true;
                    }
                    else if (input.Length == 1 && input.StartsWith(Constants.RESETTURN))
                    {
                        resetTurn = true;
                    }
                    else
                    {
                        if (uint.TryParse(input, out fieldId) && gameboard.GetBoard().FindInTwoDimensional(field => field.GetFieldID() == fieldId)) inputIsCorrect = true;
                        else Console.WriteLine("please enter a existing fieldId!");
                    }
                }
                else
                {
                    Console.WriteLine("input was incorrect!");
                    Console.WriteLine();
                }
            }

            return fieldId;
        }

        private void ResetGame()
        {
            gameboard.ResetGameboard();

            allFigures.Clear();
            log.Clear();

            whitePlayerFiguresOutOfGame.Clear();
            blackPlayerFiguresOutOfGame.Clear();

            CreateFigures();

            logic.ResetFirstMoveOverList();
            logic.RefreshPossibleMovementsDictionary(gameboard);
        }

        private void CreateFigures()
        {
            uint figureId = 0;
            figureId = CreateWhitePlayerFigures(figureId);
            figureId = CreateBlackPlayerFigures(figureId);
        }

        private void PrintLog()
        {
            Console.WriteLine("Log:");
            Console.WriteLine();
            foreach (string logEntry in log)
            {
                Console.WriteLine(logEntry);
            }

            Console.WriteLine("");
        }

        private uint CreateBlackPlayerFigures(uint figureId)
        {
            KingFigur blackPlayerKing = new KingFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKing.GetID());
            allFigures.Add(blackPlayerKing);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.D).PlaceFigure(blackPlayerKing);

            QueenFigur blackPlayerQueen = new QueenFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerQueen.GetID());
            allFigures.Add(blackPlayerQueen);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.E).PlaceFigure(blackPlayerQueen);

            BishopFigur blackPlayerBishop1 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            allFigures.Add(blackPlayerBishop1);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerBishop1.GetID());
            gameboard.GetField(Constants.Row.Eight, Constants.Column.C).PlaceFigure(blackPlayerBishop1);

            BishopFigur blackPlayerBishop2 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerBishop2.GetID());
            allFigures.Add(blackPlayerBishop2);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.F).PlaceFigure(blackPlayerBishop2);

            KnightFigur blackPlayerKnight1 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKnight1.GetID());
            allFigures.Add(blackPlayerKnight1);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.B).PlaceFigure(blackPlayerKnight1);

            KnightFigur blackPlayerKnight2 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKnight2.GetID());
            allFigures.Add(blackPlayerKnight2);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.G).PlaceFigure(blackPlayerKnight2);

            RookFigur blackPlayerRook1 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerRook1.GetID());
            allFigures.Add(blackPlayerRook1);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.A).PlaceFigure(blackPlayerRook1);

            RookFigur blackPlayerRook2 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerRook2.GetID());
            allFigures.Add(blackPlayerRook2);
            gameboard.GetField(Constants.Row.Eight, Constants.Column.H).PlaceFigure(blackPlayerRook2);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur blackPlayerPawn = new PawnFigur(Constants.ColorEnum.BLACK, ++figureId);
                logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerPawn.GetID());
                allFigures.Add(blackPlayerPawn);
                gameboard.GetField(Constants.Row.Seven, columnForPawns).PlaceFigure(blackPlayerPawn);

                columnForPawns++;
            }

            return figureId;
        }

        private uint CreateWhitePlayerFigures(uint figureId)
        {
            KingFigur whitePlayerKing = new KingFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKing.GetID());
            allFigures.Add(whitePlayerKing);
            gameboard.GetField(Constants.Row.One, Constants.Column.D).PlaceFigure(whitePlayerKing);

            QueenFigur whitePlayerQueen = new QueenFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerQueen.GetID());
            allFigures.Add(whitePlayerQueen);
            gameboard.GetField(Constants.Row.One, Constants.Column.E).PlaceFigure(whitePlayerQueen);

            BishopFigur whitePlayerBishop1 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerBishop1.GetID());
            allFigures.Add(whitePlayerBishop1);
            gameboard.GetField(Constants.Row.One, Constants.Column.C).PlaceFigure(whitePlayerBishop1);

            BishopFigur whitePlayerBishop2 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerBishop2.GetID());
            allFigures.Add(whitePlayerBishop2);
            gameboard.GetField(Constants.Row.One, Constants.Column.F).PlaceFigure(whitePlayerBishop2);

            KnightFigur whitePlayerKnight1 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKnight1.GetID());
            allFigures.Add(whitePlayerKnight1);
            gameboard.GetField(Constants.Row.One, Constants.Column.B).PlaceFigure(whitePlayerKnight1);

            KnightFigur whitePlayerKnight2 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKnight2.GetID());
            allFigures.Add(whitePlayerKnight2);
            gameboard.GetField(Constants.Row.One, Constants.Column.G).PlaceFigure(whitePlayerKnight2);

            RookFigur whitePlayerRook1 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerRook1.GetID());
            allFigures.Add(whitePlayerRook1);
            gameboard.GetField(Constants.Row.One, Constants.Column.A).PlaceFigure(whitePlayerRook1);

            RookFigur whitePlayerRook2 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerRook2.GetID());
            allFigures.Add(whitePlayerRook2);
            gameboard.GetField(Constants.Row.One, Constants.Column.H).PlaceFigure(whitePlayerRook2);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur whitePlayerPawn = new PawnFigur(Constants.ColorEnum.WHITE, ++figureId);
                logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerPawn.GetID());
                allFigures.Add(whitePlayerPawn);
                gameboard.GetField(Constants.Row.Two, columnForPawns).PlaceFigure(whitePlayerPawn);

                columnForPawns++;
            }

            return figureId;
        }

        /// <summary>
        /// prints only Board without Figures
        /// </summary>
        public void PrintBoard(ChessGameboard gameboard)
        {
            string line;

            string headerLine = "       ";

            foreach (Constants.Column columnHeader in Enum.GetValues(typeof(Constants.Column)))
            {
                headerLine += string.Format("| {0,17} |", columnHeader) + " ";
            }

            headerLine += "      ";

            Console.WriteLine(headerLine);

            Constants.Row rowHeader = Constants.Row.Eight;


            for (int row = 0; row < Constants.GAMEBOARDHEIGHT; row++)
            {
                line = string.Empty;

                for (int column = 0; column < Constants.GAMEBOARDWIDTH; column++)
                {
                    line += gameboard.GetBoard()[row, column] + " ";
                }

                Console.WriteLine(string.Format("{0,6}", rowHeader) + " " + line + " " + string.Format("{0,6}", rowHeader));
                rowHeader--;
            }

            Console.WriteLine(headerLine);
        }

        /// <summary>
        /// prints only Board with Figures
        /// </summary>
        public void PrintBoardWithFigures(ChessGameboard gameboard)
        {
            string line;

            string headerLine = "       ";

            string horizontalBorder = string.Empty;

            foreach (Constants.Column columnHeader in Enum.GetValues(typeof(Constants.Column)))
            {
                headerLine += string.Format("|{0,10}|", columnHeader) + " ";
            }

            headerLine += "        ";

            for (int i = 0; i < headerLine.Length; i++)
            {
                horizontalBorder += "-";
            }

            Console.WriteLine(horizontalBorder);
            Console.WriteLine(headerLine);
            Console.WriteLine(horizontalBorder);

            Constants.Row rowHeader = Constants.Row.Eight;


            for (int row = 0; row < Constants.GAMEBOARDHEIGHT; row++)
            {
                line = string.Empty;

                for (int column = 0; column < Constants.GAMEBOARDWIDTH; column++)
                {
                    line += gameboard.GetBoard()[row, column] + " ";
                }

                Console.WriteLine(string.Format("{0,6}", rowHeader) + " " + line + " " + string.Format("{0,6}", rowHeader));
                Console.WriteLine(horizontalBorder);
                rowHeader--;
            }

            Console.WriteLine(headerLine);
            Console.WriteLine(horizontalBorder);
        }
    }
}