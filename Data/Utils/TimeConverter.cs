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
                    [TimeValueType.MSec] = 24 * 60 * 60 * 1000,
                },
                [TimeValueType.Hour] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.MSec] = 60 * 60 * 1000,
                },
                [TimeValueType.Min] = new Dictionary<TimeValueType, double>
                {
                    [TimeValueType.MSec] = 60 * 1000,
                },
                [TimeValueType.Sec] = new Dictionary<TimeValueType, double>
                { 
                    [TimeValueType.MSec] = 1000,
                },
            };

        static TimeConverter()
        {
            foreach(var conversionRates in ConversionRatesMap.Values)
            {
                double value;
                if (conversionRates.TryGetValue(TimeValueType.Day, out value))
                {
                    var totalHours = conversionRates[TimeValueType.Hour] = value * 24;
                    conversionRates[TimeValueType.Min] = totalHours * 60;
                    conversionRates[TimeValueType.Sec] = totalHours * 60 * 60;
                    conversionRates[TimeValueType.MSec] = totalHours * 60 * 60 * 1000;
                }
                else if (conversionRates.TryGetValue(TimeValueType.MSec, out value))
                {
                    var timeSpan = TimeSpan.FromMilliseconds(value);
                    conversionRates[TimeValueType.Sec] = timeSpan.TotalSeconds;
                    conversionRates[TimeValueType.Min] = timeSpan.TotalMinutes;
                    conversionRates[TimeValueType.Hour] = timeSpan.TotalHours;
                    conversionRates[TimeValueType.Day] = timeSpan.TotalDays;
                    conversionRates[TimeValueType.Week] = timeSpan.TotalDays / 7;
                    conversionRates[TimeValueType.Month] = timeSpan.TotalDays / 30;
                    conversionRates[TimeValueType.Year] = timeSpan.TotalDays / 365;
                }
            }
        }

        public static TimeValue? Convert(TimeValue source, TimeValueType targetType)
        {
            if (ConversionRatesMap.TryGetValue(source.Type!.Value, out var conversionRates))
            {
                if (conversionRates.TryGetValue(targetType, out var rate))
                {
                    var number = double.Parse(source.Number) * rate;
                    Console.WriteLine($"{source.Number} {source.Type} to {targetType} at {rate}");
                    if (source.Type > TimeValueType.Day) 
                        // If the source type is less than day, rounding is required due to my lazy calculations
                        number = Math.Round(number);
                    
                    return new TimeValue
                    {
                        Number = number.ToString(),
                        Type = targetType,
                        IsLocked = source.IsLocked
                    };
                }
            }

            return null;
        }

        public static TimeValueGroup OptimizeUpwards(TimeValue source)
        {
            var sourceClone = (TimeValue)source.Clone();
            var relevantKeys = ConversionRatesMap.Keys.Where(k => k < source.Type).OrderBy(k => k);
            Dictionary<TimeValueType, double> resultsDict = new Dictionary<TimeValueType, double>();
            
            foreach(var key in relevantKeys)
            {
                var value = Convert(sourceClone, key);
                if (double.TryParse(value?.Number, out var number) && number > 0)
                {
                    var flooredNumber = Math.Floor(number); 
                    resultsDict[key] = flooredNumber;
                    var remainder = number - flooredNumber;
                    sourceClone.Number = remainder.ToString();
                    sourceClone.Type = key;
                }
            }

            return TimeValueGroup.FromDictionary(resultsDict);
        }
    }
}
