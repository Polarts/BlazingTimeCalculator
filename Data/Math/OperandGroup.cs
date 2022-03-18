public class OperandGroup : IMathComponent
{
    public List<Operand> Operands { get; set; } = new List<Operand>();

    public TimeSpan ToTimeSpan() 
    {
        return TimeSpan.FromMilliseconds(
            Operands.Sum(o => o.ToTimeSpan().TotalMilliseconds)
        );
    }
}