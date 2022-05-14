namespace Data.Types.TimeCalculator
{

    public class TimeOperator : ITimeMathComponent
    {
        #region Properties

        public string? Type { get; set; }

        public TimeValueGroup? Operand1 { get; set; }

        public TimeValueGroup? Operand2 { get; set; }

        #endregion

        #region Methods

        public TimeSpan GetResult()
        {
            if (Operand1 is null || Operand2 is null)
                return TimeSpan.Zero;

            switch (Type)
            {
                case "+":
                    return Operand1.ToTimeSpan() + Operand2.ToTimeSpan();

                case "-":
                    return Operand1.ToTimeSpan() - Operand2.ToTimeSpan();

                case "x":
                    return TimeSpan.FromMilliseconds(Operand1.ToTimeSpan().TotalMilliseconds * int.Parse(Operand2.TimeValues[0].Number));

                case "/":
                    return TimeSpan.FromMilliseconds(Operand1.ToTimeSpan().TotalMilliseconds / int.Parse(Operand2.TimeValues[0].Number));

                default: return TimeSpan.Zero;

            }
        }

        #endregion
    }

}