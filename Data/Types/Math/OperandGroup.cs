namespace Data.Types.Math
{

    public class OperandGroup : IMathComponent
    {
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
                group.Operands.Add(new Operand { Number = years.ToString(), Type = OperandType.Year });
            }
            if (timeSpan.TotalDays / 30 >= 1)
            {
                months = (int)(timeSpan.TotalDays / 30.0) - years * 12;
                if (months > 0)
                    group.Operands.Add(new Operand { Number = months.ToString(), Type = OperandType.Month });
            }
            if (timeSpan.TotalDays / 7 >= 1)
            {
                weeks = (int)System.Math.Floor(timeSpan.TotalDays / 7.0 - months * 4 - years * 52);
                if (weeks > 0)
                    group.Operands.Add(new Operand { Number = weeks.ToString(), Type = OperandType.Week });
            }
            if (timeSpan.TotalDays > 0)
            {
                var days = (int)System.Math.Round(timeSpan.TotalDays - weeks * 7 - months * 30 - years * 365);
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