using System.Collections.Generic;
using Шахматы;
namespace Шахматы.Pieces
{
    public class Kon : Piece
    {
        private Doska.Yacheika[] possibleCells = new Doska.Yacheika[8];
        public Kon(PlayerColor color) : base(color)
        {
            for (int i = 0; i < 8; i++)
            {
                possibleCells[i] = null;
            }
        }
        public Kon(Piece promote) : this(promote.Color) { Moved = promote.Moved; }
        public override IEnumerable<Doska.Yacheika> PossibleMoves
        {
            get
            {
                foreach (Doska.Yacheika yacheika in possibleCells)
                {
                    if (yacheika != null && (yacheika.Piece == null || yacheika.Piece.Color != Color))
                        yield return yacheika;
                }
            }
        }
        public override void Recalculate()
        {
            possibleCells[0] = Parent.Open(-1, 2);//2 вверх 1 вниз
            possibleCells[1] = Parent.Open(-1, -2);//2 ыниз 1 
            possibleCells[2] = Parent.Open(1, 2);//2 вверх 1 вправо
            possibleCells[3] = Parent.Open(1, -2);//2 вниз 1 вправо
            possibleCells[4] = Parent.Open(-2, 1);//1 вверх 2 влево
            possibleCells[5] = Parent.Open(-2, -1);//1 вниз 2 влево
            possibleCells[6] = Parent.Open(2, 1);//1 вверх 2 вправо
            possibleCells[7] = Parent.Open(2, -1);//1 вниз 2 вправо

            for (int i = 0; i < 8; i++)
            {
                if (possibleCells[i] != null)
                    possibleCells[i].HitBy.Add(this);
            }
        }
        public override bool IsBlockedIfMove(Doska.Yacheika from, Doska.Yacheika to, Doska.Yacheika blocked)
        {
            for (int i = 0; i < 8; i++)
                if (possibleCells[i] == blocked)
                    return false;
            return true;
        }
        public override char Char => '♘';
    }
}
