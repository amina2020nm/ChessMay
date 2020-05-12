using System;
using System.Linq;
namespace Шахматы
{
    public enum PlayerColor { White, Black }
    public enum PlayerState { Idle, Holding, AwaitPromote, GameOver }
    public enum PromoteOptions { Queen = 0, Rook = 1, Bishop = 2, Knight = 3 }
    public class Chess
    {
        public bool Running { private set; get; }
        private PlayerState playerState;
        private PromoteOptions promoteOption; //Выбранный в настоящее время вариант продвижения
        private PlayerColor currentPlayer;// True для белого, false для черного
        private int cursorX, cursorY;// Координаты для курсора на доске
        private Doska board;
        private Doska.Yacheika holdedNode = null; // ячейка текущего элемента
        private Doska.Yacheika moveTo = null;// Куда двигаться
        public Chess()
        {
            Running = true;
            board = new Doska();
            currentPlayer = PlayerColor.White;
            turnStart();
        }
        public void Update()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.LeftArrow && cursorX > 0 && playerState != PlayerState.AwaitPromote)
                    cursorX--;
                else if (keyInfo.Key == ConsoleKey.RightArrow && cursorX < 7 && playerState != PlayerState.AwaitPromote)
                    cursorX++;
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (playerState != PlayerState.AwaitPromote && cursorY < 7)
                        cursorY++;
                    else if ((int)promoteOption > 0)
                        promoteOption--;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (playerState != PlayerState.AwaitPromote && cursorY > 0)
                        cursorY--;
                    else if ((int)promoteOption < 3)
                        promoteOption++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                    interact();
                else if (keyInfo.Key == ConsoleKey.D)
                    debugInteract();
                else if (keyInfo.Key == ConsoleKey.Escape)
                    cancel();
            }
        }
        public void Draw(ConsoleGraphics g)//Рисует игру
        {
            g.FillArea(new CChar(' ', ConsoleColor.Black, ConsoleColor.DarkGray), 10, 5, 8, 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Doska.Yacheika yacheika = board.GetCell(i, j);
                    if (yacheika.Piece != null)
                    {
                        g.DrawTransparent(yacheika.Piece.Char, (yacheika.Piece.Color == PlayerColor.White) ? ConsoleColor.White : ConsoleColor.Black, 10 + i, 5 + (7 - j));
                        if (yacheika.Piece.LegalMoves.Count == 0)
                        {
                            g.SetBackground(ConsoleColor.DarkRed, 10 + i, 5 + (7 - j));
                        }
                    }
                    if (yacheika.HitBy.Contains(debugPiece))
                        g.SetBackground(ConsoleColor.DarkMagenta, 10 + i, 5 + (7 - j));
                }
            }
            if (holdedNode != null && playerState == PlayerState.Holding)
            {
                foreach (Doska.Yacheika move in holdedNode.Piece.LegalMoves)// Подсветка возможных ходов
                {
                    g.SetBackground(ConsoleColor.DarkGreen, 10 + move.X, 5 + (7 - move.Y));
                }
            }
            g.SetBackground(ConsoleColor.DarkYellow, 10 + cursorX, 5 + (7 - cursorY));// курсор- желтый
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1) g.LightenBackground(10 + i, 5 + j);
                }
            }
            if (playerState == PlayerState.AwaitPromote)// Меню параметров продвижения
            {
                g.DrawTextTrasparent("Dama", promoteOption == PromoteOptions.Queen ? ConsoleColor.Yellow : ConsoleColor.White, 22, 7);
                g.DrawTextTrasparent("LAdiya", promoteOption == PromoteOptions.Rook ? ConsoleColor.Yellow : ConsoleColor.White, 22, 9);
                g.DrawTextTrasparent("Slon", promoteOption == PromoteOptions.Bishop ? ConsoleColor.Yellow : ConsoleColor.White, 22, 11);
                g.DrawTextTrasparent("Kon", promoteOption == PromoteOptions.Knight ? ConsoleColor.Yellow : ConsoleColor.White, 22, 13);
            }
            else
            {
                g.ClearArea(22, 7, 6, 7);
            }
        }
        // kогда пользователь нажимает клавишу ввода
        private void interact()
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                    holdedNode = board.GetCell(cursorX, cursorY);

                    if (holdedNode.Piece == null || holdedNode.Piece.Color != currentPlayer || holdedNode.Piece.LegalMoves.Count == 0)
                    {
                        holdedNode = null;
                        return;
                    }
                    else playerState = PlayerState.Holding;
                    break;
                case PlayerState.Holding:
                    playerState = PlayerState.Holding;
                    moveTo = board.GetCell(cursorX, cursorY);
                    if (!holdedNode.Piece.LegalMoves.Contains(moveTo))
                    {
                        moveTo = null;
                        return;
                    }
                    if (board.IsPromotable(holdedNode, moveTo))
                        showPromote();
                    else
                        turnOver();
                    break;
                case PlayerState.AwaitPromote:
                    turnOver();
                    break;
                case PlayerState.GameOver:
                    Running = false;
                    break;
            }
        }
        private Piece debugPiece;
        private void debugInteract() { debugPiece = board.GetCell(cursorX, cursorY).Piece; }
        private void cancel()//если esc
        {
            playerState = PlayerState.Idle;
            holdedNode = null;
        }
        // Вызывается при каждом ходе
        private void turnStart() { board.TurnStart(currentPlayer); }
        private void showPromote()
        {
            playerState = PlayerState.AwaitPromote;
            promoteOption = PromoteOptions.Queen;
        }
        private void turnOver()// Вызывается, когда ход передается другому игроку.
        {
            board.Move(holdedNode, moveTo, promoteOption);
            holdedNode = null;
            moveTo = null;
            playerState = PlayerState.Idle;
            currentPlayer = currentPlayer == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
            turnStart();
        }
    }
}
