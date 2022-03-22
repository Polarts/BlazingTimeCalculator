public enum OperandType
{
    Number, Year, Month, Week, Day, Hour, Min, Sec, MSec
}

public class Operand : IMathComponent
{
    #region Properties

    public string Number { get; set; } = "";

    public OperandType? Type { get; set; } = null;

    /// <summary>
    /// Indicates whether this operand is allowed to have a type or not.
    /// </summary>
    public bool IsLocked { get; set; }

    #endregion

    #region Methods

    public TimeSpan ToTimeSpan() =>
        Type switch
        {
            OperandType.Year => TimeSpan.FromDays(int.Parse(Number) * 365),
            OperandType.Month => TimeSpan.FromDays(int.Parse(Number) * 30),
            OperandType.Week => TimeSpan.FromDays(int.Parse(Number) * 7),
            OperandType.Day => TimeSpan.FromDays(int.Parse(Number)),
            OperandType.Hour => TimeSpan.FromHours(int.Parse(Number)),
            OperandType.Min => TimeSpan.FromMinutes(int.Parse(Number)),
            OperandType.Sec => TimeSpan.FromSeconds(int.Parse(Number)),
            OperandType.MSec => TimeSpan.FromMilliseconds(int.Parse(Number)),
            _ => TimeSpan.FromTicks(int.Parse(Number))
        };

    public override string ToString()
    {
       return Number + " " + Type;
    }

    #endregion
}
