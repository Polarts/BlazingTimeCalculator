using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Types.Time
{
    public enum TimeValueType
    {
        Year, Month, Week, Day, Hour, Min, Sec, MSec
    }

    public class TimeValue
    {
        #region Fields

        private Dictionary<TimeValueType, double> values = new Dictionary<TimeValueType, double>();

        #endregion

        #region Ctor & Indexer

        public TimeValue()
        {
            foreach (var timeVal in Enum.GetValues(typeof(TimeValueType)))
            {
                values.Add((TimeValueType)timeVal, 0);
            }
        }

        public double this[TimeValueType type]
        {
            get => values[type];
            set => values[type] = value;
        }

        #endregion

        #region Methods

        public void AddTimeSpan(TimeSpan ts)
        {
            values[TimeValueType.MSec] = ts.Milliseconds;
            values[TimeValueType.Sec] += ts.Seconds;
            values[TimeValueType.Min] += ts.Minutes;
            values[TimeValueType.Hour] += ts.Hours;
            values[TimeValueType.Day] += ts.Days;
        }

        public void OptimizeUpwards()
        {
            if (values[TimeValueType.MSec] >= 1000)
            {
                var msecTS = TimeSpan.FromMilliseconds(values[TimeValueType.MSec]);
                AddTimeSpan(msecTS);
            }

            if (values[TimeValueType.Sec] >= 60)
            {
                var secTS = TimeSpan.FromSeconds(values[TimeValueType.Sec]);
                AddTimeSpan(secTS);
            }

            if (values[TimeValueType.Min] >= 60)
            {
                var minTS = TimeSpan.FromMinutes(values[TimeValueType.Min]);
                AddTimeSpan(minTS);
            }

            if (values[TimeValueType.Hour] >= 24)
            {
                var hourTS = TimeSpan.FromHours(values[TimeValueType.Hour]);
                AddTimeSpan(hourTS);
            }

            if (values[TimeValueType.Day] >= 365)
            {
                var years = values[TimeValueType.Day] / 365;
                values[TimeValueType.Year] += Math.Floor(years);
                values[TimeValueType.Day] = (years - Math.Floor(years)) * 365;
            }

            if (values[TimeValueType.Day] >= 30)
            {
                var months = values[TimeValueType.Day] / 30;
                values[TimeValueType.Month] += Math.Floor(months);
                values[TimeValueType.Day] = (months - Math.Floor(months)) * 30;
            }

            if (values[TimeValueType.Day] >= 7)
            {
                var weeks = values[TimeValueType.Day] / 7;
                values[TimeValueType.Week] += Math.Floor(weeks);
                values[TimeValueType.Day] = (weeks - Math.Floor(weeks)) * 7;
            }

            if (values[TimeValueType.Week] >= 52)
            {
                var years = values[TimeValueType.Week] / 52;
                values[TimeValueType.Year] += Math.Floor(years);
                values[TimeValueType.Week] = (years - Math.Floor(years)) * 52;
            }

            if (values[TimeValueType.Week] >= 4)
            {
                var months = values[TimeValueType.Week] / 4;
                values[TimeValueType.Month] += Math.Floor(months);
                values[TimeValueType.Week] = (months - Math.Floor(months)) * 52;
            }

            if (values[TimeValueType.Month] >= 12)
            {
                var years = values[TimeValueType.Month] / 12;
                values[TimeValueType.Year] += Math.Floor(years);
                values[TimeValueType.Month] = (years - Math.Floor(years)) * 12;
            }
        }

        public void OptimizeDownwards()
        {
            if (values[TimeValueType.Year] < 0)
            {
                var months = values[TimeValueType.Year] / 12;

            }
        }

        #region Operators

        public static TimeValue operator +(TimeValue operand1, TimeValue operand2)
        {
            var result = new TimeValue();

            foreach (var key in operand1.values.Keys)
            {
                result[key] = operand1[key] + operand2[key];
            }

            result.OptimizeUpwards();
            return result;
        }

        public static TimeValue operator -(TimeValue operand1, TimeValue operand2)
        {
            var result = new TimeValue();

            foreach (var key in operand1.values.Keys)
            {
                result[key] = operand1[key] - operand2[key];
            }

            result.OptimizeDownwards();
            return result;
        }

        #endregion

        #endregion
    }
}
