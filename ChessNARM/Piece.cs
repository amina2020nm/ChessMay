using System.Collections.Generic;
namespace Шахматы
{
    public abstract class Piece
    {
        public PlayerColor Color { private set; get; }
        public bool Moved { protected set; get; }
        public abstract IEnumerable<Doska.Yacheika> PossibleMoves { get; }
        public List<Doska.Yacheika> LegalMoves { private set; get; }
        public Doska.Yacheika Parent { private set; get; }
        public Piece(PlayerColor color)
        {
            Color = color;
            Moved = false;
            LegalMoves = new List<Doska.Yacheika>();
        }
        public void OnPlace(Doska.Yacheika yacheika)
        {
            Parent = yacheika;
        }
        public void OnMove(Doska.Yacheika yacheika)
        {
            Parent = yacheika;
            Moved = true;
        }
        public abstract void Recalculate();
        public abstract bool IsBlockedIfMove(Doska.Yacheika from, Doska.Yacheika to, Doska.Yacheika blocked);
        public abstract char Char { get; }
        protected virtual bool canHit(Doska.Yacheika yacheika)
        {
            return yacheika != null && yacheika.Piece != null && yacheika.Piece.Color != Color;
        }
    }
}
