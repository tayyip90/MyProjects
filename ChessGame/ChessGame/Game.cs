﻿using System;
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

        private bool newGame;
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

            newGame = false;
            quit = false;
        }

        public void start()
        {
            while (!quit)
            {
                printIntro();

                if (newGame)
                {
                    Console.Clear();
                    NewGame();
                    newGame = false;
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
            setNameOfPlayers();

            Console.BackgroundColor = ConsoleColor.Blue;

            resetGame();

            while (!isGameFinished & !quit)
            {
                while (!figureIsChosen & !destinationFieldIsChosen & !quit) {
                    // set reset turn false
                    if (isPlayerResetTurn) isPlayerResetTurn = false;

                    Console.WriteLine("Turn Number: " + turnNumber + " "+ players[playerTurn].getName() + "'s turn!" + " Color: " + players[playerTurn].getColor());
                    Console.WriteLine(string.Empty);

                    gameboard.printBoardWithFigures();

                    printTextSelectFigureForPlayer();

                    while (!figureIsChosen & !isPlayerResetTurn & !quit)
                    {
                        figureIsChosen = selectFigure();
                    }

                    printTextSelectDestinationField();

                    if (figureIsChosen)
                    {
                        while (!destinationFieldIsChosen & !isPlayerResetTurn & !quit)
                        {
                            destinationFieldIsChosen = selectDestination();
                        }
                    }
                }

                if(figureIsChosen & destinationFieldIsChosen)
                {
                    if (logic.ckeckWhetherMovementIsCorrect(gameboard.getBoard(), selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY))
                    {
                        string removedFigureText = string.Empty;
                        string logText = players[playerTurn].getName() + " / " + players[playerTurn].getColor() + " Player moved his ";

                        if (logic.isFieldOccupied(gameboard.getBoard(), destinationFieldY, destinationFieldX)){
                            if(playerTurn == 0)
                            {
                                removedFigureText = "removed the " + gameboard.getBoard()[destinationFieldY, destinationFieldX].getChessFigure().ToString() + " of " + players[1].getName();
                                blackPlayerFiguresOutOfGame.Add(gameboard.getBoard()[destinationFieldY, destinationFieldX].removeFigure());
                            }
                            else
                            {
                                removedFigureText = "removed the " + gameboard.getBoard()[destinationFieldY, destinationFieldX].getChessFigure().ToString() + " of " + players[0].getName();
                                whitePlayerFiguresOutOfGame.Add(gameboard.getBoard()[destinationFieldY, destinationFieldX].removeFigure());
                            }
                        }

                        logic.addFigureIdToFirstMoveOverList(gameboard.getBoard(), selectedFigureY, selectedFigureX);

                        logText += gameboard.getBoard()[selectedFigureY, selectedFigureX].getChessFigure().ToString() + " to "
                            + gameboard.getBoard()[destinationFieldY, destinationFieldX].getRow() + ", " + gameboard.getBoard()[destinationFieldY, destinationFieldX].getColumn();

                        if (removedFigureText != string.Empty) logText += " and " + removedFigureText;

                        gameboard.moveFigureToPosition(selectedFigureX, selectedFigureY, destinationFieldX, destinationFieldY);

                        log.Add(logText);

                        setNextPlayer();

                        logic.refreshPossibleMovementsDictionary(gameboard.getBoard());

                        if (whiteKingIsChecked)
                        {
                            blackPlayerWon = whiteKingIsChecked = logic.checkWhetherWhiteKingIsChecked(gameboard.getBoard());
                            isGameFinished = blackPlayerWon;
                        }
                        else
                        {
                            whiteKingIsChecked = logic.checkWhetherWhiteKingIsChecked(gameboard.getBoard());
                        }
                        

                        if (blackKingIsChecked)
                        {
                            whitePlayerWon = blackKingIsChecked = logic.checkWhetherBlackKingIsChecked(gameboard.getBoard());
                            isGameFinished = whitePlayerWon;
                        }
                        else
                        {
                            blackKingIsChecked = logic.checkWhetherBlackKingIsChecked(gameboard.getBoard());
                        }

                        Console.Clear();

                        printPossibleMovements();
                        displayLog();
                        printKingIsCheckedStatus();
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
                displayPlayerWon();
            }
        }

        private void displayPlayerWon()
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

        private void printKingIsCheckedStatus()
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

        private void setNameOfPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                setName(i);
            }
        }

        private void setName(int playernumber)
        {
            showTitle();
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

        private void showTitle()
        {
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine(Constants.TITLE);
            Console.WriteLine(Constants.PLACEHOLDERSTRINGSTARS);
            Console.WriteLine("");
        }

        private void printPossibleMovements()
        {
            List<string> possibleMovements = logic.getPossibleMovmentsOfFigures(allFigures);

            foreach(string line in possibleMovements)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine(string.Empty);
        }

        private void setNextPlayer()
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

        private void printTextSelectDestinationField()
        {
            Console.WriteLine("Select the Destination Field!");
        }

        private void printTextSelectFigureForPlayer()
        {
            Console.WriteLine("Select your Figure, that you want to move!");
        }

        private bool selectDestination()
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
                if(logic.isFieldOccupied(gameboard.getBoard(),rowNumber, columnNumber))
                {
                    if (logic.isEnemyField(gameboard.getBoard(), players[playerTurn].getColor(), rowNumber, columnNumber))
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

        private bool selectFigure()
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
                if(logic.isFieldOccupied(gameboard.getBoard(), rowNumber, columnNumber))
                {
                    if(logic.checkWhetherFigureBelongsPlayer(gameboard.getBoard(), players[playerTurn].getColor(), rowNumber, columnNumber))
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

        private void resetGame()
        {
            whitePlayerWon = false;
            blackPlayerWon = false;

            turnNumber = 1;
            playerTurn = 0;
            figureId = 0;

            figureIsChosen = false;
            destinationFieldIsChosen = false;

            isGameFinished = false;

            gameboard.resetGameboard();

            allFigures.Clear();
            log.Clear();

            whitePlayerFiguresOutOfGame.Clear();
            blackPlayerFiguresOutOfGame.Clear();

            createWhitePlayerFigures();
            createBlackPlayerFigures();

            logic.resetFirstMoveOverList();
            logic.refreshPossibleMovementsDictionary(gameboard.getBoard());

            printPossibleMovements();
            printKingIsCheckedStatus();
        }

        private void displayLog()
        {
            foreach(string turn in log)
            {
                Console.WriteLine(turn);
            }

            Console.WriteLine("");
        }

        private void createBlackPlayerFigures()
        {
            KingFigur blackPlayerKing = new KingFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKing.getID());
            allFigures.Add(blackPlayerKing);
            gameboard.setFigureToPosition(blackPlayerKing, Constants.Row.Eight, Constants.Column.D);

            QueenFigur blackPlayerQueen = new QueenFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerQueen.getID());
            allFigures.Add(blackPlayerQueen);
            gameboard.setFigureToPosition(blackPlayerQueen, Constants.Row.Eight, Constants.Column.E);

            BishopFigur blackPlayerBishop1 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            allFigures.Add(blackPlayerBishop1);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerBishop1.getID());
            gameboard.setFigureToPosition(blackPlayerBishop1, Constants.Row.Eight, Constants.Column.C);

            BishopFigur blackPlayerBishop2 = new BishopFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerBishop2.getID());
            allFigures.Add(blackPlayerBishop2);
            gameboard.setFigureToPosition(blackPlayerBishop2, Constants.Row.Eight, Constants.Column.F);

            KnightFigur blackPlayerKnight1 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKnight1.getID());
            allFigures.Add(blackPlayerKnight1);
            gameboard.setFigureToPosition(blackPlayerKnight1, Constants.Row.Eight, Constants.Column.B);

            KnightFigur blackPlayerKnight2 = new KnightFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerKnight2.getID());
            allFigures.Add(blackPlayerKnight2);
            gameboard.setFigureToPosition(blackPlayerKnight2, Constants.Row.Eight, Constants.Column.G);

            RookFigur blackPlayerRook1 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerRook1.getID());
            allFigures.Add(blackPlayerRook1);
            gameboard.setFigureToPosition(blackPlayerRook1, Constants.Row.Eight, Constants.Column.A);

            RookFigur blackPlayerRook2 = new RookFigur(Constants.ColorEnum.BLACK, ++figureId);
            logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerRook2.getID());
            allFigures.Add(blackPlayerRook2);
            gameboard.setFigureToPosition(blackPlayerRook2, Constants.Row.Eight, Constants.Column.H);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur blackPlayerPawn = new PawnFigur(Constants.ColorEnum.BLACK, ++figureId);
                logic.addFigureIdToPossibleMovementsBlackFiguresDictionary(blackPlayerPawn.getID());
                allFigures.Add(blackPlayerPawn);
                gameboard.setFigureToPosition(blackPlayerPawn, Constants.Row.Seven, columnForPawns);
                columnForPawns++;
            }
        }

        private void createWhitePlayerFigures()
        {
            KingFigur whitePlayerKing = new KingFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKing.getID());
            allFigures.Add(whitePlayerKing);
            gameboard.setFigureToPosition(whitePlayerKing, Constants.Row.One, Constants.Column.D);

            QueenFigur whitePlayerQueen = new QueenFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerQueen.getID());
            allFigures.Add(whitePlayerQueen);
            gameboard.setFigureToPosition(whitePlayerQueen, Constants.Row.One, Constants.Column.E);

            BishopFigur whitePlayerBishop1 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerBishop1.getID());
            allFigures.Add(whitePlayerBishop1);
            gameboard.setFigureToPosition(whitePlayerBishop1, Constants.Row.One, Constants.Column.C);
            BishopFigur whitePlayerBishop2 = new BishopFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerBishop2.getID());
            allFigures.Add(whitePlayerBishop2);
            gameboard.setFigureToPosition(whitePlayerBishop2, Constants.Row.One, Constants.Column.F);

            KnightFigur whitePlayerKnight1 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKnight1.getID());
            allFigures.Add(whitePlayerKnight1);
            gameboard.setFigureToPosition(whitePlayerKnight1, Constants.Row.One, Constants.Column.B);

            KnightFigur whitePlayerKnight2 = new KnightFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerKnight2.getID());
            allFigures.Add(whitePlayerKnight2);
            gameboard.setFigureToPosition(whitePlayerKnight2, Constants.Row.One, Constants.Column.G);

            RookFigur whitePlayerRook1 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerRook1.getID());
            allFigures.Add(whitePlayerRook1);
            gameboard.setFigureToPosition(whitePlayerRook1, Constants.Row.One, Constants.Column.A);

            RookFigur whitePlayerRook2 = new RookFigur(Constants.ColorEnum.WHITE, ++figureId);
            logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerRook2.getID());
            allFigures.Add(whitePlayerRook2);
            gameboard.setFigureToPosition(whitePlayerRook2, Constants.Row.One, Constants.Column.H);

            Constants.Column columnForPawns = Constants.Column.A;

            for (int i = 0; i < 8; i++)
            {
                PawnFigur whitePlayerPawn = new PawnFigur(Constants.ColorEnum.WHITE, ++figureId);
                logic.addFigureIdToPossibleMovementsWhiteFiguresDictionary(whitePlayerPawn.getID());
                allFigures.Add(whitePlayerPawn);
                gameboard.setFigureToPosition(whitePlayerPawn, Constants.Row.Two, columnForPawns);
                columnForPawns++;
            }
        }
    }
}