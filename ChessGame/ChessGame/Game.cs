using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Game
    {
        private ChessLogic logic;
        private Player[] players;
        private int playerTurn;
        private List<string> log;

        private ChessGameboard gameboard;

        private List<ChessFigure> whitePlayerFiguresOutOfGame;
        private List<ChessFigure> blackPlayerFiguresOutOfGame;

        private List<ChessFigure> allFigures;

        private bool blackKingIsChecked;
        private bool whiteKingIsChecked;

        private bool blackPlayerWon;
        private bool whitePlayerWon;

        private int turnNumber;

        private bool isGameFinished;
        private bool isPlayerResetTurn;

        private bool startGame;
        private bool quit;

        private bool figureIsChosen;
        private bool destinationFieldIsChosen;

        private char input;
        private bool inputIsCorrect;

        private int selectedFigureX;
        private int selectedFigureY;
        private int destinationFieldX;
        private int destinationFieldY;

        private uint figureId;

        private char row;
        private char column;

        public Game()
        {
            allFigures = new List<ChessFigure>();
            log = new List<string>();

            blackPlayerFiguresOutOfGame = new List<ChessFigure>();
            whitePlayerFiguresOutOfGame = new List<ChessFigure>();

            logic = new ChessLogic();
            gameboard = new ChessGameboard();

            players = new Player[2];

            startGame = false;
            quit = false;
        }

        public void Intro()
        {
            while (!quit)
            {
                PrintIntro();

                if (startGame)
                {
                    Console.Clear();
                    Start();
                    startGame = false;
                }
            }

            Console.Clear();
            Console.WriteLine(Constants.QUITSTRING);
            Console.WriteLine("");
        }

        private void PrintIntro()
        {
            inputIsCorrect = false;

            Console.Clear();

            ShowMenu();

            while (!inputIsCorrect)
            {
                input = Console.ReadKey().KeyChar;

                if (input == Constants.NEWGAME)
                {
                    inputIsCorrect = true;
                    startGame = true;
                }
                else if (input == Constants.QUIT)
                {
                    inputIsCorrect = true;
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

        private void Start()
        {
            SetNameOfPlayers();

            Console.BackgroundColor = ConsoleColor.Blue;

            ResetGame();

            while (!isGameFinished & !quit)
            {
                while (!figureIsChosen & !destinationFieldIsChosen & !quit) {
                    // set reset turn false
                    if (isPlayerResetTurn) isPlayerResetTurn = false;

                    Console.WriteLine("Turn Number: " + turnNumber + " "+ players[playerTurn].getName() + "'s turn!" + " Color: " + players[playerTurn].getColor());
                    Console.WriteLine(string.Empty);

                    gameboard.PrintBoardWithFigures();

                    PrintTextSelectFigureForPlayer();

                    while (!figureIsChosen & !isPlayerResetTurn & !quit)
                    {
                        figureIsChosen = SelectFigure();
                    }

                    PrintTextSelectDestinationField();

                    if (figureIsChosen)
                    {
                        while (!destinationFieldIsChosen & !isPlayerResetTurn & !quit)
                        {
                            destinationFieldIsChosen = SelectDestination();
                        }
                    }
                }

                if(figureIsChosen & destinationFieldIsChosen)
                {
                    if (logic.CkeckWhetherMovementIsCorrect(gameboard.GetBoard(), selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                    {
                        string removedFigureText = string.Empty;
                        string logText = players[playerTurn].getName() + " / " + players[playerTurn].getColor() + " Player moved his ";

                        if (logic.IsFieldOccupied(gameboard.GetBoard(), destinationFieldY, destinationFieldX)){
                            if(playerTurn == 0)
                            {
                                removedFigureText = "removed the " + gameboard.GetBoard()[destinationFieldY, destinationFieldX].GetChessFigure().ToString() + " of " + players[1].getName();
                                blackPlayerFiguresOutOfGame.Add(gameboard.GetBoard()[destinationFieldY, destinationFieldX].RemoveFigure());
                            }
                            else
                            {
                                removedFigureText = "removed the " + gameboard.GetBoard()[destinationFieldY, destinationFieldX].GetChessFigure().ToString() + " of " + players[0].getName();
                                whitePlayerFiguresOutOfGame.Add(gameboard.GetBoard()[destinationFieldY, destinationFieldX].RemoveFigure());
                            }
                        }

                        logic.AddFigureIdToFirstMoveOverList(gameboard.GetBoard(), selectedFigureY, selectedFigureX);

                        logText += gameboard.GetBoard()[selectedFigureY, selectedFigureX].GetChessFigure().ToString() + " to "
                            + gameboard.GetBoard()[destinationFieldY, destinationFieldX].GetRow() + ", " + gameboard.GetBoard()[destinationFieldY, destinationFieldX].GetColumn();

                        if (removedFigureText != string.Empty) logText += " and " + removedFigureText;

                        gameboard.MoveFigureToPosition(selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);

                        log.Add(logText);

                        SetNextPlayer();

                        logic.RefreshPossibleMovementsDictionary(gameboard.GetBoard());

                        if (whiteKingIsChecked)
                        {
                            blackPlayerWon = whiteKingIsChecked = logic.CheckWhetherWhiteKingIsChecked(gameboard.GetBoard());
                            isGameFinished = blackPlayerWon;
                        }
                        else
                        {
                            whiteKingIsChecked = logic.CheckWhetherWhiteKingIsChecked(gameboard.GetBoard());
                        }
                        

                        if (blackKingIsChecked)
                        {
                            whitePlayerWon = blackKingIsChecked = logic.CheckWhetherBlackKingIsChecked(gameboard.GetBoard());
                            isGameFinished = whitePlayerWon;
                        }
                        else
                        {
                            blackKingIsChecked = logic.CheckWhetherBlackKingIsChecked(gameboard.GetBoard());
                        }

                        Console.Clear();

                        PrintPossibleMovements();
                        DisplayLog();
                        PrintKingIsCheckedStatus();
                    }
                    else
                    {
                        Console.WriteLine("This is not a Correct movement!");
                    }
                }

                figureIsChosen = false;
                destinationFieldIsChosen = false;
            }

            Console.BackgroundColor = ConsoleColor.Black;

            if (quit)
            {
                Console.WriteLine("Player has quit the Game!");
            }

            if (blackPlayerWon | whitePlayerWon)
            {
                DisplayPlayerWon();
            }
        }

        private void DisplayPlayerWon()
        {
            Console.Clear();
            Console.WriteLine(Constants.PLAYERWONTEXT);

            if (whitePlayerWon)
            {
                Console.WriteLine(players[0].getName() + " has won!");
            }
            if(blackPlayerWon)
            {
                Console.WriteLine(players[1].getName() + " has won!");
            }

            Console.ReadKey();
        }

        private void PrintKingIsCheckedStatus()
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

        private void SetNextPlayer()
        {
            if(playerTurn == 0)
            {
                playerTurn = 1;
            }
            else
            {
                playerTurn = 0;
            }

            turnNumber++;
        }

        private void PrintTextSelectDestinationField()
        {
            Console.WriteLine("Select the Destination Field!");
        }

        private void PrintTextSelectFigureForPlayer()
        {
            Console.WriteLine("Select your Figure, that you want to move!");
        }

        private bool SelectDestination()
        {
            inputIsCorrect = false;
            bool fieldIsChosen = false;
            row = ' ';
            column = ' ';
            int rowNumber = -1;
            int columnNumber = -1;

            while (!inputIsCorrect & !quit & !isPlayerResetTurn)
            {
                Console.WriteLine("Type in the row of the Destination Field!");
                Console.WriteLine("Please Type a Number between 1 to 8!");

                input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (input == Constants.QUIT)
                {
                    quit = true;
                }
                else if (input == Constants.RESETTURN)
                {
                    isPlayerResetTurn = true;
                    figureIsChosen = false;
                    destinationFieldIsChosen = false;
                }
                else
                {
                    rowNumber = Constants.convertRowCharToRowNumberForGameboard(input);

                    if (rowNumber != -1) inputIsCorrect = true;
                }
            }

            inputIsCorrect = false;

            while (!inputIsCorrect & !quit & !isPlayerResetTurn)
            {
                Console.WriteLine("Type in the column, where your Figure is occupied!");
                Console.WriteLine("Please Type a character between a to h!");

                input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (input == Constants.QUIT)
                {
                    quit = true;
                }
                else if (input == Constants.RESETTURN)
                {
                    isPlayerResetTurn = true;
                    figureIsChosen = false;
                    destinationFieldIsChosen = false;
                }
                else
                {
                    columnNumber = Constants.convertColumnCharToColumnNumberForGameboard(input);

                    if (columnNumber != -1) inputIsCorrect = true;
                }
            }

            if (quit | isPlayerResetTurn)
            {
                fieldIsChosen = false;
            }
            else
            {
                if(logic.IsFieldOccupied(gameboard.GetBoard(),rowNumber, columnNumber))
                {
                    if (logic.IsEnemyField(gameboard.GetBoard(), players[playerTurn].getColor(), rowNumber, columnNumber))
                    {
                        fieldIsChosen = true;
                        destinationFieldX = columnNumber;
                        destinationFieldY = rowNumber;
                    }
                    else
                    {
                        figureIsChosen = false;
                        Console.WriteLine("The Destination Field is Occupied with your own Figure!");
                    }
                }
                else
                {
                    fieldIsChosen = true;
                    destinationFieldX = columnNumber;
                    destinationFieldY = rowNumber;
                }
            }

            return fieldIsChosen;
        }

        private bool SelectFigure()
        {
            inputIsCorrect = false;
            bool fieldIsChosen = false;
            row = ' ';
            column = ' ';
            int rowNumber = -1;
            int columnNumber = -1;

            while (!inputIsCorrect & !quit & !isPlayerResetTurn)
            {
                Console.WriteLine("Type in the row, where your Figure is occupied!");
                Console.WriteLine("Please Type a Number between 1 to 8!");

                input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (input == Constants.QUIT)
                {
                    quit = true;
                }
                else if (input == Constants.RESETTURN)
                {
                    isPlayerResetTurn = true;
                    figureIsChosen = false;
                    destinationFieldIsChosen = false;
                }
                else
                {
                    rowNumber = Constants.convertRowCharToRowNumberForGameboard(input);

                    if (rowNumber != -1) inputIsCorrect = true;
                }
            }

            inputIsCorrect = false;

            while (!inputIsCorrect & !quit & !isPlayerResetTurn)
            {
                Console.WriteLine("Type in the column, where your Figure is occupied!");
                Console.WriteLine("Please Type a character between a to h!");

                input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (input == Constants.QUIT)
                {
                    quit = true;
                }
                else if (input == Constants.RESETTURN)
                {
                    isPlayerResetTurn = true;
                    figureIsChosen = false;
                    destinationFieldIsChosen = false;
                }
                else
                {
                    columnNumber = Constants.convertColumnCharToColumnNumberForGameboard(input);

                    if (columnNumber != -1) inputIsCorrect = true;
                }
            }

            if (quit | isPlayerResetTurn)
            {
                fieldIsChosen = false;
            }
            else
            {
                if(logic.IsFieldOccupied(gameboard.GetBoard(), rowNumber, columnNumber))
                {
                    if(logic.CheckWhetherFigureBelongsPlayer(gameboard.GetBoard(), players[playerTurn].getColor(), rowNumber, columnNumber))
                    {
                        fieldIsChosen = true;
                        selectedFigureX = columnNumber;
                        selectedFigureY = rowNumber;
                    }
                    else
                    {
                        Console.WriteLine("This Figure belongs the Enemy!");
                    }
                }
                else
                {
                    Console.WriteLine("There is no Figure to select!");
                }
            }

            return fieldIsChosen;
        }

        private void ResetGame()
        {
            whitePlayerWon = false;
            blackPlayerWon = false;

            turnNumber = 1;
            playerTurn = 0;
            figureId = 0;

            figureIsChosen = false;
            destinationFieldIsChosen = false;

            isGameFinished = false;

            gameboard.ResetGameboard();

            allFigures.Clear();
            log.Clear();

            whitePlayerFiguresOutOfGame.Clear();
            blackPlayerFiguresOutOfGame.Clear();

            CreateWhitePlayerFigures();
            CreateBlackPlayerFigures();

            logic.ResetFirstMoveOverList();
            logic.RefreshPossibleMovementsDictionary(gameboard.GetBoard());

            PrintPossibleMovements();
            PrintKingIsCheckedStatus();
        }

        private void DisplayLog()
        {
            foreach(string turn in log)
            {
                Console.WriteLine(turn);
            }

            Console.WriteLine("");
        }

        private void CreateBlackPlayerFigures()
        {
            KingFigur blackPlayerKing = new KingFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKing.getID());
            allFigures.Add(blackPlayerKing);
            gameboard.SetFigureToPosition(blackPlayerKing, Constants.Row.Eight, Constants.Column.D);

            QueenFigur blackPlayerQueen = new QueenFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerQueen.getID());
            allFigures.Add(blackPlayerQueen);
            gameboard.SetFigureToPosition(blackPlayerQueen, Constants.Row.Eight, Constants.Column.E);

            BishopFigur blackPlayerBishop1 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            allFigures.Add(blackPlayerBishop1);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerBishop1.getID());
            gameboard.SetFigureToPosition(blackPlayerBishop1, Constants.Row.Eight, Constants.Column.C);

            BishopFigur blackPlayerBishop2 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerBishop2.getID());
            allFigures.Add(blackPlayerBishop2);
            gameboard.SetFigureToPosition(blackPlayerBishop2, Constants.Row.Eight, Constants.Column.F);

            KnightFigur blackPlayerKnight1 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKnight1.getID());
            allFigures.Add(blackPlayerKnight1);
            gameboard.SetFigureToPosition(blackPlayerKnight1, Constants.Row.Eight, Constants.Column.B);

            KnightFigur blackPlayerKnight2 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKnight2.getID());
            allFigures.Add(blackPlayerKnight2);
            gameboard.SetFigureToPosition(blackPlayerKnight2, Constants.Row.Eight, Constants.Column.G);

            RookFigur blackPlayerRook1 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerRook1.getID());
            allFigures.Add(blackPlayerRook1);
            gameboard.SetFigureToPosition(blackPlayerRook1, Constants.Row.Eight, Constants.Column.A);

            RookFigur blackPlayerRook2 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerRook2.getID());
            allFigures.Add(blackPlayerRook2);
            gameboard.SetFigureToPosition(blackPlayerRook2, Constants.Row.Eight, Constants.Column.H);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur blackPlayerPawn = new PawnFigur(Constants.ColorEnum.BLACK, ++figureId);
                logic.AddFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerPawn.getID());
                allFigures.Add(blackPlayerPawn);
                gameboard.SetFigureToPosition(blackPlayerPawn, Constants.Row.Seven, columnForPawns);
                columnForPawns++;
            }
        }

        private void CreateWhitePlayerFigures()
        {
            KingFigur whitePlayerKing = new KingFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKing.getID());
            allFigures.Add(whitePlayerKing);
            gameboard.SetFigureToPosition(whitePlayerKing, Constants.Row.One, Constants.Column.D);

            QueenFigur whitePlayerQueen = new QueenFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerQueen.getID());
            allFigures.Add(whitePlayerQueen);
            gameboard.SetFigureToPosition(whitePlayerQueen, Constants.Row.One, Constants.Column.E);

            BishopFigur whitePlayerBishop1 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerBishop1.getID());
            allFigures.Add(whitePlayerBishop1);
            gameboard.SetFigureToPosition(whitePlayerBishop1, Constants.Row.One, Constants.Column.C);
            BishopFigur whitePlayerBishop2 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerBishop2.getID());
            allFigures.Add(whitePlayerBishop2);
            gameboard.SetFigureToPosition(whitePlayerBishop2, Constants.Row.One, Constants.Column.F);

            KnightFigur whitePlayerKnight1 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKnight1.getID());
            allFigures.Add(whitePlayerKnight1);
            gameboard.SetFigureToPosition(whitePlayerKnight1, Constants.Row.One, Constants.Column.B);

            KnightFigur whitePlayerKnight2 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKnight2.getID());
            allFigures.Add(whitePlayerKnight2);
            gameboard.SetFigureToPosition(whitePlayerKnight2, Constants.Row.One, Constants.Column.G);

            RookFigur whitePlayerRook1 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerRook1.getID());
            allFigures.Add(whitePlayerRook1);
            gameboard.SetFigureToPosition(whitePlayerRook1, Constants.Row.One, Constants.Column.A);

            RookFigur whitePlayerRook2 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerRook2.getID());
            allFigures.Add(whitePlayerRook2);
            gameboard.SetFigureToPosition(whitePlayerRook2, Constants.Row.One, Constants.Column.H);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur whitePlayerPawn = new PawnFigur(Constants.ColorEnum.WHITE, ++figureId);
                logic.AddFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerPawn.getID());
                allFigures.Add(whitePlayerPawn);
                gameboard.SetFigureToPosition(whitePlayerPawn, Constants.Row.Two, columnForPawns);
                columnForPawns++;
            }
        }
    }
}