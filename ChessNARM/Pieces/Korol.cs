using System.Collections.Generic;
using Шахматы;

namespace Шахматы.Pieces
{
    public class Korol : Piece
    {
        public override char Char => '♔';
        private Napravlenie[] directions = new Napravlenie[8];
        private bool canCastleLeft;
        private bool canCastleRight;
        public Korol(PlayerColor color) : base(color)
        {
            for (int i = 0; i < 8; i++)
            {
                directions[i] = null;
            }
        }
        public override IEnumerable<Doska.Yacheika> PossibleMoves
        {
            get
            {
                foreach (Napravlenie direction in directions)
                {
                    foreach (Doska.Yacheika yacheika in direction.GetPossibleMoves()) { yield return yacheika; }
                    if (canCastleLeft)
                    {
                        yield return Parent.Parent.GetCell(2, (Color == PlayerColor.White) ? 0 : 7);
                    }
                    if (canCastleRight)
                    {
                        yield return Parent.Parent.GetCell(6, (Color == PlayerColor.White) ? 0 : 7);
                    }
                }
            }
        }
        public override void Recalculate()
        {
            if (!Moved)
            {
                canCastleLeft = true;
                Doska.Yacheika leftRookCell = Parent.Parent.GetCell(0, (Color == PlayerColor.White) ? 0 : 7);
                if (leftRookCell.Piece == null || !(leftRookCell.Piece is Ladiya) || leftRookCell.Piece.Color != Color || leftRookCell.Piece.Moved)
                    canCastleLeft = false;
                else
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (Parent.Parent.GetCell(i, (Color == PlayerColor.White) ? 0 : 7).Piece != null)
                            canCastleLeft = false;
                    }
                }
                canCastleRight = true;
                Doska.Yacheika rightRookCell = Parent.Parent.GetCell(7, (Color == PlayerColor.White) ? 0 : 7);
                if (rightRookCell.Piece == null || !(rightRookCell.Piece is Ladiya) || rightRookCell.Piece.Color != Color || rightRookCell.Piece.Moved)
                    canCastleRight = false;
                else
                {
                    for (int i = 5; i <= 6; i++)
                    {
                        if (Parent.Parent.GetCell(i, (Color == PlayerColor.White) ? 0 : 7).Piece != null)
                            canCastleRight = false;
                    }
                }
            }
            directions[0] = new Napravlenie(this, 0, 1, 1);
            directions[1] = new Napravlenie(this, 0, -1, 1);
            directions[2] = new Napravlenie(this, -1, 0, 1);
            directions[3] = new Napravlenie(this, 1, 0, 1);
            directions[4] = new Napravlenie(this, -1, 1, 1);
            directions[5] = new Napravlenie(this, 1, 1, 1);
            directions[6] = new Napravlenie(this, -1, -1, 1);
            directions[7] = new Napravlenie(this, 1, -1, 1);
        }
        public override bool IsBlockedIfMove(Doska.Yacheika from, Doska.Yacheika to, Doska.Yacheika blocked)
        {
            foreach (Napravlenie direction in directions)
            {
                if (!direction.IsBlockedIfMove(from, to, blocked))
                    return false;
            }
            return true;
        }
    }
}
