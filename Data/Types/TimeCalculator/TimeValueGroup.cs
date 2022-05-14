using Data.Utils;

namespace Data.Types.TimeCalculator
{

    public class TimeValueGroup : ITimeMathComponent
    {

        #region Constants

        const double DAYS_IN_MONTH = 365.0 / 12.0;
        const double WEEKS_IN_YEAR = 365.0 / 7.0;
        const double DAYS_IN_WEEK = 365.0 / WEEKS_IN_YEAR;

        #endregion

        #region Properties

        public List<TimeValue> TimeValues { get; set; } = new List<TimeValue>();

        /// <summary>
        /// Used to serve as the index of the operand in an equation.
        /// </summary>
        public int EquationIndex { get; set; }

        #endregion

        #region Factories

        public static TimeValueGroup FromDictionary(Dictionary<TimeValueType, double> dict)
        {
            var result = new TimeValueGroup();
            foreach (var kvp in dict)
            {
                result.TimeValues.Add(new TimeValue
                {
                    Number = kvp.Value.ToString(),
                    Type = kvp.Key,
                });
            }
            return result;
        }

        #endregion

        #region Methods

        public Dictionary<TimeValueType, double>? ToValuesDict()
        {
            if (TimeValues.Count == 1 && TimeValues[0].Type == null) // if it's a multiplication's op2
                return null;

            var dict = new Dictionary<TimeValueType, double>();
            foreach (var value in TimeValues)
            {
                dict[value.Type!.Value] = double.Parse(value.Number);
            }

            return dict;
        }

        public TimeValue ToSingleValue()
        {
            if (TimeValues.Count == 1 && TimeValues[0].Type == null) // if it's a multiplication's op2
                return TimeValues[0];

            var lowestValueType = TimeValues.Max(v => v.Type) ?? TimeValueType.MSec;
            var sum = TimeValues.Select(v => TimeConverter.Convert(v, lowestValueType)).Sum(v => double.Parse(v?.Number ?? "0"));
            return new TimeValue
            {
                Number = sum.ToString(),
                Type = lowestValueType
            };

        }

        public TimeSpan ToTimeSpan()
        {
            if (TimeValues.Count == 1 && TimeValues[0].Type is null)
                return TimeSpan.FromMilliseconds(double.Parse(TimeValues[0].Number));

            return TimeSpan.FromMilliseconds(
                TimeValues.Sum(o => o.ToTimeSpan().TotalMilliseconds)
            );
        }

        public static TimeValueGroup FromTimeSpan(TimeSpan timeSpan)
        {
            var group = new TimeValueGroup();
            int years = 0, months = 0, weeks = 0;
            if (timeSpan.TotalDays / 365 >= 1)
            {
                years = (int)(timeSpan.TotalDays / 365.0);
                if (years > 0)
                    group.TimeValues.Add(new TimeValue { Number = years.ToString(), Type = TimeValueType.Year });
            }
            if (timeSpan.TotalDays / DAYS_IN_MONTH >= 1)
            {
                months = (int)(timeSpan.TotalDays / DAYS_IN_MONTH) - years * 12;
                if (months > 0)
                    group.TimeValues.Add(new TimeValue { Number = months.ToString(), Type = TimeValueType.Month });
            }
            if (timeSpan.TotalDays / DAYS_IN_WEEK >= 1)
            {
                weeks = (int)System.Math.Floor(timeSpan.TotalDays / DAYS_IN_WEEK - months * 4 - years * 52);
                if (weeks > 0)
                    group.TimeValues.Add(new TimeValue { Number = weeks.ToString(), Type = TimeValueType.Week });
            }
            if (timeSpan.TotalDays > 0)
            {
                var days = (int)System.Math.Floor(timeSpan.TotalDays - weeks * 7 - months * 30 - years * 365);
                if (days > 0)
                    group.TimeValues.Add(new TimeValue { Number = days.ToString(), Type = TimeValueType.Day });
            }
            if (timeSpan.Hours > 0)
            {
                group.TimeValues.Add(new TimeValue { Number = timeSpan.Hours.ToString(), Type = TimeValueType.Hour });
            }
            if (timeSpan.Minutes > 0)
            {
                group.TimeValues.Add(new TimeValue { Number = timeSpan.Minutes.ToString(), Type = TimeValueType.Min });
            }
            if (timeSpan.Seconds > 0)
            {
                group.TimeValues.Add(new TimeValue { Number = timeSpan.Seconds.ToString(), Type = TimeValueType.Sec });
            }
            if (timeSpan.Milliseconds > 0)
            {
                group.TimeValues.Add(new TimeValue { Number = timeSpan.Milliseconds.ToString(), Type = TimeValueType.MSec });
            }

            return group;
        }

        public override string ToString()
        {
            return TimeValues
                .Select(x => x.ToString())
                .Aggregate("", (accum, curr) => accum + " " + curr);
        }

        #endregion
    }

}