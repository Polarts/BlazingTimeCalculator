namespace Data.Types.Math
{

    public class OperandGroup : IMathComponent
    {

        #region Constants

        const double DAYS_IN_MONTH = 365.0 / 12.0;
        const double WEEKS_IN_YEAR = 365.0 / 7.0;
        const double DAYS_IN_WEEK = 365.0 / WEEKS_IN_YEAR;

        #endregion

        #region Properties

        public List<Operand> Operands { get; set; } = new List<Operand>();

        /// <summary>
        /// Used to serve as the index of the operand in an equation.
        /// </summary>
        public int EquationIndex { get; set; }

        #endregion

        #region Methods

        public TimeSpan ToTimeSpan()
        {
            if (Operands.Count == 1 && Operands[0].Type is null)
                return TimeSpan.FromMilliseconds(double.Parse(Operands[0].Number));

            return TimeSpan.FromMilliseconds(
                Operands.Sum(o => o.ToTimeSpan().TotalMilliseconds)
            );
        }

        public static OperandGroup FromTimeSpan(TimeSpan timeSpan)
        {
            var group = new OperandGroup();
            int years = 0, months = 0, weeks = 0;
            if (timeSpan.TotalDays / 365 >= 1)
            {
                years = (int)(timeSpan.TotalDays / 365.0);
                if (years > 0)
                    group.Operands.Add(new Operand { Number = years.ToString(), Type = OperandType.Year });
            }
            if (timeSpan.TotalDays / DAYS_IN_MONTH >= 1)
            {
                months = (int)(timeSpan.TotalDays / DAYS_IN_MONTH) - years * 12;
                if (months > 0)
                    group.Operands.Add(new Operand { Number = months.ToString(), Type = OperandType.Month });
            }
            if (timeSpan.TotalDays / DAYS_IN_WEEK >= 1)
            {
                weeks = (int)System.Math.Floor(timeSpan.TotalDays / DAYS_IN_WEEK - months * 4 - years * 52);
                if (weeks > 0)
                    group.Operands.Add(new Operand { Number = weeks.ToString(), Type = OperandType.Week });
            }
            if (timeSpan.TotalDays > 0)
            {
                var days = (int)System.Math.Floor(timeSpan.TotalDays - weeks * 7 - months * 30 - years * 365);
                if (days > 0)
                    group.Operands.Add(new Operand { Number = days.ToString(), Type = OperandType.Day });
            }
            if (timeSpan.Hours > 0)
            {
                group.Operands.Add(new Operand { Number = timeSpan.Hours.ToString(), Type = OperandType.Hour });
            }
            if (timeSpan.Minutes > 0)
            {
                group.Operands.Add(new Operand { Number = timeSpan.Minutes.ToString(), Type = OperandType.Min });
            }
            if (timeSpan.Seconds > 0)
            {
                group.Operands.Add(new Operand { Number = timeSpan.Seconds.ToString(), Type = OperandType.Sec });
            }
            if (timeSpan.Milliseconds > 0)
            {
                group.Operands.Add(new Operand { Number = timeSpan.Milliseconds.ToString(), Type = OperandType.MSec });
            }

            return group;
        }

        public override string ToString()
        {
            return Operands
                .Select(x => x.ToString())
                .Aggregate("", (accum, curr) => accum + " " + curr);
        }

        #endregion
    }

}