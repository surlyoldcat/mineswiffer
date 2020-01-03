namespace mineswiffer
{
    public class Cell
    {
        const string HIDDEN = "X";
        const string CLEARED = "_";
        const string MINED = "*";

        public Cell(bool hasMine)
        {
            IsRevealed = false;
            HasMine = hasMine;
        }
        public bool IsRevealed {get; private set;}
        public bool HasMine {get; private set;}

        public void Reveal()
        {
            IsRevealed = true;
        }

        public override string ToString()
        {
            if (!IsRevealed)
            {
                return HIDDEN;
            }
            else
            {
                return HasMine ? MINED : CLEARED;
            }
        }
    }
}