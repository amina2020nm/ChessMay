using System.Collections.Generic;
using Шахматы;
namespace Шахматы.Pieces
{
    public class Peshka : Piece
    {
        private Napravlenie forward = null;
        private Doska.Yacheika[] hits = new Doska.Yacheika[2];
        public Peshka(PlayerColor color) : base(color)
        {
            hits[0] = hits[1] = null;
        }
        public override IEnumerable<Doska.Yacheika> PossibleMoves
        {
            get
            {
                foreach (Doska.Yacheika yacheika in forward.GetPossibleMoves(false))
                {
                    yield return yacheika;
                }

                if (canHit(hits[0]))
                    yield return hits[0];
                if (canHit(hits[1]))
                    yield return hits[1];
            }
        }
        public override void Recalculate()
        {
            forward = new Napravlenie(this, 0, (Color == PlayerColor.White) ? 1 : -1, Moved ? 1 : 2, false);
            hits[0] = Parent.Open(-1, (Color == PlayerColor.White) ? 1 : -1);
            hits[1] = Parent.Open(1, (Color == PlayerColor.White) ? 1 : -1);
            if (hits[0] != null)
                hits[0].HitBy.Add(this);
            if (hits[1] != null)
                hits[1].HitBy.Add(this);
        }
        public override bool IsBlockedIfMove(Doska.Yacheika from, Doska.Yacheika to, Doska.Yacheika blocked)
        {
            return hits[0] != blocked && hits[1] != blocked;
        }
        public override char Char => '♙';
        protected override bool canHit(Doska.Yacheika yacheika)
        {
            return base.canHit(yacheika) || (yacheika != null && yacheika == yacheika.Parent.EnPassant);
        }
    }
}
