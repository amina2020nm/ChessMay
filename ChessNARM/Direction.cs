using System;
using System.Collections.Generic;
using System.Linq;
namespace Шахматы
{
    public class Napravlenie//Все возможные ходы и проверка линии хода
    {
        public Piece Piece { private set; get; }/// Часть на которую может двигаться
        public int X { private set; get; }
        public int Y { private set; get; }
        private List<Doska.Yacheika> possibleMoves;//То, куда можно сходить, шаги, которые можно сделать в этом направлении
        public IEnumerable<Doska.Yacheika> GetPossibleMoves(bool enemyHittable = true)
        {
            if (possibleMoves.Count == 0)
                yield break; //указывает, что последовательность больше не имеет элементов.
            for (int i = 0; i < possibleMoves.Count - 1; i++)
            {
                yield return possibleMoves[i];//определяет возвращаемый элемент
            }
            if (possibleMoves.Last().Piece == null)
                yield return possibleMoves.Last();//определяет возвращаемый элемент
            else if (enemyHittable && possibleMoves.Last().Piece.Color != Piece.Color)
                yield return possibleMoves.Last();//определяет возвращаемый элемент
        }
        public int GetPossibleMoveCount(bool enemyHittable = true)/// Количество возможных ходов
        {
            if (possibleMoves.Count == 0)
                return 0;
            if (possibleMoves.Last().Piece == null)
                return possibleMoves.Count;
            else if (!enemyHittable || possibleMoves.Last().Piece.Color == Piece.Color)
                return possibleMoves.Count - 1;
            else
                return possibleMoves.Count;
        }
        public int DesiredCount { private set; get; }
        private bool updateHitGraph;
        public Napravlenie(Piece piece, int x, int y, int desiredCount = 8, bool updateHitGraph = true)
        {
            Piece = piece;
            X = x;
            Y = y;
            DesiredCount = desiredCount;
            this.updateHitGraph = updateHitGraph;
            possibleMoves = new List<Doska.Yacheika>();
            possibleMoves.AddRange(piece.Parent.OpenLineOfSight(x, y, desiredCount));
            foreach (Doska.Yacheika move in possibleMoves)
            {
                if (updateHitGraph)
                    move.HitBy.Add(Piece);
            }
        }
        public bool IsBlockedIfMove(Doska.Yacheika from, Doska.Yacheika to, Doska.Yacheika blocked)
        {
            if (possibleMoves.Contains(blocked) && !possibleMoves.Contains(to))
            {
                return false;
            }
            else if (possibleMoves.Contains(from))
            {
                int toIndex = possibleMoves.IndexOf(to);
                if (0 <= toIndex && toIndex < possibleMoves.Count - 1)
                    return true;
                else
                {
                    // Если мы двинулись дальше
                    foreach (Doska.Yacheika move in from.OpenLineOfSight(X, Y, DesiredCount - possibleMoves.Count))
                    {
                        if (move == to) //перешел на новый путь
                            return true;
                        if (move == blocked) //заблокирован
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
