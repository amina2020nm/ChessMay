using Шахматы.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Шахматы
{
    public class Doska// Структура и алгоритмы шахматной доски
    {
        public class Yacheika//Представляет одну шахматную ячейку
        {
            public Doska Parent { private set; get; }
            public int X, Y;
            public Piece Piece;
            public List<Piece> HitBy;// Все, кто могут поразить эту клетку.
            public Yacheika(Doska parent, int x, int y)
            {
                Parent = parent;
                HitBy = new List<Piece>();
                X = x;
                Y = y;
            }
            public IEnumerable<Yacheika> OpenLineOfSight(int dirX, int dirY, int desiredCount = 1)
            {
                for (int i = 1; i <= desiredCount; i++)
                {
                    //Запрос элемента для ячейки, если пустая ячейка отсутствует, и мы должны остановиться
                    Yacheika yacheika = Parent.GetCell(X + dirX * i, Y + dirY * i);
                    if (yacheika == null) yield break;
                    yield return yacheika;
                    //Остановиться в любом случае, так как линия заблокирована
                    if (yacheika.Piece != null)
                        yield break;
                }
            }
            public Yacheika Open(int x, int y)
            {
                //Запрос элемента для ячейки, если пустая ячейка отсутствует, и мы не должны возвращать
                Yacheika yacheika = Parent.GetCell(X + x, Y + y);
                return yacheika ?? null;
            }
        }
        private Yacheika[,] cells; //информация о ячейках и связях между ними
        public Yacheika EnPassant { private set; get; }
        public Yacheika EnPassantCapture { private set; get; } //Ячейка, в которой пешка будет захвачена
        private List<Piece> pieces = new List<Piece>();
        private Piece blackKing;
        private Piece whiteKing;
        private bool inCheck;
        public Doska() { Reset(); }
        public Yacheika GetCell(int x, int y)// Получить ячейку по абсолютным координатам
        {
            if (x < 0 || cells.GetLength(0) <= x || y < 0 || cells.GetLength(1) <= y) return null;

            return cells[x, y];
        }
        private void addPiece(Yacheika yacheika, Piece piece)// Добавляет фигуру в начале игры
        {
            yacheika.Piece = piece;
            pieces.Add(piece);
            piece.OnPlace(yacheika);
        }
        public void Reset()//Сбрoc
        {
            cells = new Yacheika[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cells[i, j] = new Yacheika(this, i, j);
                }
            }
            pieces.Clear();
            EnPassant = null;
            EnPassantCapture = null;
            addPiece(cells[0, 0], new Ladiya(PlayerColor.White));
            addPiece(cells[1, 0], new Kon(PlayerColor.White));
            addPiece(cells[2, 0], new Slon(PlayerColor.White));
            addPiece(cells[3, 0], new Dama(PlayerColor.White));
            addPiece(cells[4, 0], (whiteKing = new Korol(PlayerColor.White)));
            addPiece(cells[5, 0], new Slon(PlayerColor.White));
            addPiece(cells[6, 0], new Kon(PlayerColor.White));
            addPiece(cells[7, 0], new Ladiya(PlayerColor.White));
            addPiece(cells[0, 1], new Peshka(PlayerColor.White));
            addPiece(cells[1, 1], new Peshka(PlayerColor.White));
            addPiece(cells[2, 1], new Peshka(PlayerColor.White));
            addPiece(cells[3, 1], new Peshka(PlayerColor.White));
            addPiece(cells[4, 1], new Peshka(PlayerColor.White));
            addPiece(cells[5, 1], new Peshka(PlayerColor.White));
            addPiece(cells[6, 1], new Peshka(PlayerColor.White));
            addPiece(cells[7, 1], new Peshka(PlayerColor.White));
            addPiece(cells[0, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[1, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[2, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[3, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[4, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[5, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[6, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[7, 6], new Peshka(PlayerColor.Black));
            addPiece(cells[0, 7], new Ladiya(PlayerColor.Black));
            addPiece(cells[1, 7], new Kon(PlayerColor.Black));
            addPiece(cells[2, 7], new Slon(PlayerColor.Black));
            addPiece(cells[3, 7], new Dama(PlayerColor.Black));
            addPiece(cells[4, 7], (blackKing = new Korol(PlayerColor.Black)));
            addPiece(cells[5, 7], new Slon(PlayerColor.Black));
            addPiece(cells[6, 7], new Kon(PlayerColor.Black));
            addPiece(cells[7, 7], new Ladiya(PlayerColor.Black));
            foreach (Piece piece in pieces) { piece.Recalculate(); }
        }
        public bool TurnStart(PlayerColor currentPlayer)
        {
            inCheck = IsInCheck(currentPlayer, false);
            bool anyLegalMove = false;
            foreach (Yacheika yacheika in cells)// Очистить списки попаданий в ячейки
            {
                yacheika.HitBy.Clear();
            }
            foreach (Piece piece in pieces)// Пересчитаем возможные ходы и списки попаданий для ячеек
            {
                piece.Recalculate();
            }
            foreach (Piece piece in pieces)
            {
                piece.LegalMoves.Clear();
                foreach (Yacheika move in piece.PossibleMoves)
                {
                    if (piece.Color == currentPlayer && isMoveLegal(piece, move))
                    {
                        piece.LegalMoves.Add(move);
                        anyLegalMove = true;
                    }
                }
            }
            return anyLegalMove;
        }
        private bool isMoveLegal(Piece piece, Yacheika move)//Подтверждает, если ход является доступным для данной фигуры
        {
            Piece currentKing = piece.Color == PlayerColor.White ? whiteKing : blackKing;
            if (piece is Korol)
            {
                foreach (Piece hitter in move.HitBy)
                {
                    if (hitter.Parent != move && hitter.Color != piece.Color)
                        return false;
                }
                // Подтвердить рокировку
                if (Math.Abs(move.X - piece.Parent.X) == 2)
                {
                    if (inCheck)
                        return false;
                    foreach (Piece hitter in GetCell(move.X > piece.Parent.X ? move.X - 1 : move.X + 1, move.Y).HitBy)
                    {
                        if (hitter.Color != piece.Color)
                            return false;
                    }
                }
            }
            else // Некороль
            {
                if (inCheck)
                {
                    foreach (Piece hitter in currentKing.Parent.HitBy)
                    {
                        if (hitter.Color == currentKing.Color) continue;
                        if (hitter.Parent == move) continue;
                        if (hitter.IsBlockedIfMove(piece.Parent, move, currentKing.Parent)) continue;
                        return false;
                    }
                }
                foreach (Piece hitter in piece.Parent.HitBy)
                {
                    if (hitter.Color == currentKing.Color) continue;
                    if (hitter.Parent == move) continue;
                    if (!hitter.IsBlockedIfMove(piece.Parent, move, currentKing.Parent))
                        return false;
                }
            }
            return true;
        }
        public bool IsInCheck(PlayerColor player, bool useCache = true)// Проверяет, его король может быть поражен?
        {
            if (useCache)
                return inCheck;
            if (player == PlayerColor.White)
                return whiteKing.Parent.HitBy.Any(hitter => hitter.Color != player);
            else
                return blackKing.Parent.HitBy.Any(hitter => hitter.Color != player);
        }
        // Перемещаем фигуру из одной ячейки в другую, после того как эта функция будет вызвана
        public void Move(Yacheika from, Yacheika to, PromoteOptions promoteOption)
        {
            if (to.Piece != null)
                pieces.Remove(to.Piece);
            to.Piece = from.Piece;
            from.Piece = null;
            if (to == EnPassant && to.Piece is Peshka)
            {
                pieces.Remove(EnPassantCapture.Piece);
                EnPassantCapture.Piece = null;
            }
            if (to.Piece is Korol && to.X - from.X == 2) // Рокировка вправо
            {
                Move(GetCell(7, to.Y), GetCell(to.X - 1, to.Y), promoteOption);// Перемещаем ладью
            }
            if (to.Piece is Korol && to.X - from.X == -2)// Рокировка влево
            {
                Move(GetCell(0, to.Y), GetCell(to.X + 1, to.Y), promoteOption); // Перемещаем ладью
            }
            if (to.Piece is Peshka && to.Y == (to.Piece.Color == PlayerColor.White ? 7 : 0))
            {
                Piece promoted = null;
                switch (promoteOption)
                {
                    case PromoteOptions.Queen:
                        promoted = new Dama(to.Piece);
                        break;
                    case PromoteOptions.Rook:
                        promoted = new Ladiya(to.Piece);
                        break;
                    case PromoteOptions.Bishop:
                        promoted = new Slon(to.Piece);
                        break;
                    case PromoteOptions.Knight:
                        promoted = new Kon(to.Piece);
                        break;
                }
                //Обновляем список 
                pieces.Remove(to.Piece);
                to.Piece = promoted;
                promoted.OnPlace(to);
                pieces.Add(promoted);
            }
            // Пересчитаем возможные ходы
            to.Piece.OnMove(to);
            to.Piece.Recalculate();
            EnPassant = null;
            EnPassantCapture = null;
            if (to.Piece is Peshka && Math.Abs(to.Y - from.Y) == 2)
            {
                EnPassant = GetCell(to.X, (from.Y > to.Y) ? from.Y - 1 : from.Y + 1);
                EnPassantCapture = to;
            }
        }
        /// Продвигается ли фигура ?
        public bool IsPromotable(Yacheika from, Yacheika to)
        {
            return from.Piece is Peshka && to.Y == (from.Piece.Color == PlayerColor.White ? 7 : 0);
        }
    }
}
