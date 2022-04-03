using Data.Types.TimeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Utils
{
    public static class TimeConverter
    {
        public static Dictionary<TimeValueType, Dictionary<TimeValueType, double>> ConversionRatesMap =
            new Dictionary<TimeValueType, Dictionary<TimeValueType, double>>
            {
                [TimeValueType.Year] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.Day] = 365,
                    [TimeValueType.Month] = 12,
                    [TimeValueType.Week] = 52,
                },
                [TimeValueType.Month] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.Day] = 30,
                    [TimeValueType.Week] = 4,
                },
                [TimeValueType.Week] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.Day] = 7,
                },
                [TimeValueType.Day] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.Hour] = 24,
                    [TimeValueType.Min] = 24 * 60,
                    [TimeValueType.Sec] = 24 * 60 * 60,
                    [TimeValueType.MSec] = 24 * 60 * 60 * 1000,
                },
                [TimeValueType.Hour] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.Min] = 60,
                    [TimeValueType.Sec] = 60 * 60,
                    [TimeValueType.MSec] = 60 * 60 * 1000,
                },
                [TimeValueType.Min] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.Sec] = 60,
                    [TimeValueType.MSec] = 60 * 1000,
                },
                [TimeValueType.Sec] = new Dictionary<TimeValueType, double>
                { 
                    [TimeValueType.MSec] = 1000,
                },
            };

        public static TimeValue Convert(TimeValue source, TimeValueType targetType)
        {
            if (ConversionRatesMap.TryGetValue(source.Type!.Value, out var conversionRates))
            {
                if (conversionRates.TryGetValue(targetType, out var rate))
                {
                    var number = double.Parse(source.Number) * rate;
                    return new TimeValue
                    {
                        Number = number.ToString("0.0"),
                        Type = targetType,
                        IsLocked = source.IsLocked
                    };
                }
                else
                {
                    if (source.Type < TimeValueType.Day && conversionRates.TryGetValue(TimeValueType.Day, out var daysRate))
                    {
                        double daysValue = double.Parse(source.Number) * daysRate;
                        double? finalValue = null;
                        switch (targetType)
                        {
                            case TimeValueType.Hour:
                                finalValue = daysValue * 24;
                                break;

                            case TimeValueType.Min:
                                finalValue = daysValue * 24 * 60;
                                break;

                            case TimeValueType.Sec:
                                finalValue = daysValue * 24 * 60 * 60;
                                break;

                            case TimeValueType.MSec:
                                finalValue = daysValue * 24 * 60 * 60 * 1000;
                                break;
                        }
                        if (finalValue != null)
                        {
                            return new TimeValue
                            {
                                Number = finalValue!.Value.ToString("0.0"),
                                Type = targetType,
                                IsLocked = source.IsLocked
                            };
                        }
                    } 
                    else 
                    {
                        double? finalValue = null;
                        switch (targetType)
                        {

                        }
                    }
                }
            }

            return new TimeValue
            {
                Number = source.Number,
                Type = targetType,
                IsLocked = source.IsLocked
            };
        }
    }
}
