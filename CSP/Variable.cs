namespace CSP
{
    public class Variable
    {
        public int Nr { get; set; }
        public int Value { get; set; } = -1;
        public bool IsSet { get; set; }
        public Domain Domain { get; set; }
        public Propagation Propagation { get; set; }

        public void Set(int value)
        {
            IsSet = true;
            Value = value;
            Domain.Values.Remove(value);
        }

        public void UnSet(int value)
        {
            IsSet = false;
            Value = -1;
            Domain.Values.Add(value);
        }

        public void UndoEliminations()
        {
            Propagation.UndoEliminations();
        }
    }
}
