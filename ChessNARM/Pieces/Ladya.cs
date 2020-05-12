using System.Collections.Generic;
using Шахматы;
namespace Шахматы.Pieces
{
    public class Ladiya : Piece
    {
        private Napravlenie[] directions = new Napravlenie[4];
        public Ladiya(PlayerColor color) : base(color)
        {
            for (int i = 0; i < 4; i++)
            {
                directions[i] = null;
            }
        }
        public Ladiya(Piece promote) : this(promote.Color)
        {
            Moved = promote.Moved;
        }
        public override IEnumerable<Doska.Yacheika> PossibleMoves
        {
            get
            {
                foreach (Napravlenie direction in directions)
                {
                    foreach (Doska.Yacheika yacheika in direction.GetPossibleMoves()) { yield return yacheika; }
                }
            }
        }
        public override void Recalculate()
        {
            directions[0] = new Napravlenie(this, 0, 1);
            directions[1] = new Napravlenie(this, 0, -1);
            directions[2] = new Napravlenie(this, -1, 0);
            directions[3] = new Napravlenie(this, 1, 0);
        }
        public override bool IsBlockedIfMove(Doska.Yacheika from, Doska.Yacheika to, Doska.Yacheika blocked)
        {
            foreach (Napravlenie direction in directions)
            {
                if (!direction.IsBlockedIfMove(from, to, blocked)) return false;
            }
            return true;
        }
        public override char Char => '♖';
    }
}