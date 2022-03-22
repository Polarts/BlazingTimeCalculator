public class Operator : IMathComponent
{
    #region Properties

    public string? Type { get; set; }

    public OperandGroup? Operand1 { get; set; }

    public OperandGroup? Operand2 { get; set; }

    #endregion

    #region Methods

    public TimeSpan GetResult()
    {
        if (Operand1 is null || Operand2 is null)
            return TimeSpan.Zero;

        switch(Type)
        {
            case "+":
                return Operand1.ToTimeSpan() + Operand2.ToTimeSpan();

            case "-":
                return Operand1.ToTimeSpan() - Operand2.ToTimeSpan();

            case "x":
                return TimeSpan.FromMilliseconds(Operand1.ToTimeSpan().TotalMilliseconds * Operand2.ToTimeSpan().TotalMilliseconds);

            case "/":
                return TimeSpan.FromMilliseconds(Operand1.ToTimeSpan().TotalMilliseconds / Operand2.ToTimeSpan().TotalMilliseconds);

            default: return TimeSpan.Zero;

        }
    }

    #endregion
}