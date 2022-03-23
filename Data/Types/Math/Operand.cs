namespace Data.Types.Math
{
    public enum OperandType
    {
        Year, Month, Week, Day, Hour, Min, Sec, MSec
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

        public TimeSpan ToTimeSpan()
        {
            if (int.TryParse(Number, out var value))
            {
                switch (Type)
                {
                    case OperandType.Year:
                        return TimeSpan.FromDays(value * 365);

                    case OperandType.Month:
                        return TimeSpan.FromDays(value * 30);

                    case OperandType.Week:
                        return TimeSpan.FromDays(value * 7);

                    case OperandType.Day:
                        return TimeSpan.FromDays(value);

                    case OperandType.Hour:
                        return TimeSpan.FromHours(value);

                    case OperandType.Min:
                        return TimeSpan.FromMinutes(value);

                    case OperandType.Sec:
                        return TimeSpan.FromSeconds(value);

                    case OperandType.MSec:
                        return TimeSpan.FromMilliseconds(value);

                    default: return TimeSpan.Zero;
                }
            }
            return TimeSpan.Zero;
        } 

        public override string ToString()
        {
            return Number + " " + Type;
        }

        #endregion
    }

}