namespace Data.Types.TimeCalculator
{
    public enum TimeValueType
    {
        Year, Month, Week, Day, Hour, Min, Sec, MSec
    }

    public class TimeValue : ITimeMathComponent
    {
        #region Properties

        public string Number { get; set; } = "";

        public TimeValueType? Type { get; set; } = null;

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
                    case TimeValueType.Year:
                        return TimeSpan.FromDays(value * 365);

                    case TimeValueType.Month:
                        return TimeSpan.FromDays(value * 30);

                    case TimeValueType.Week:
                        return TimeSpan.FromDays(value * 7);

                    case TimeValueType.Day:
                        return TimeSpan.FromDays(value);

                    case TimeValueType.Hour:
                        return TimeSpan.FromHours(value);

                    case TimeValueType.Min:
                        return TimeSpan.FromMinutes(value);

                    case TimeValueType.Sec:
                        return TimeSpan.FromSeconds(value);

                    case TimeValueType.MSec:
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