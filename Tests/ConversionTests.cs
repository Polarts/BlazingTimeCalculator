using Data.Types.TimeCalculator;
using Data.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void HoursToDaysTest()
        {
            TimeValue val = new TimeValue
            {
                Number = "48",
                Type = TimeValueType.Hour
            };
            Assert.AreEqual(TimeConverter.Convert(val, TimeValueType.Day)!.Number, "2");
        }

        [TestMethod]
        public void DaysToWeeksTest()
        {
            TimeValue val = new TimeValue
            {
                Number = "75",
                Type = TimeValueType.Day
            };
            Assert.AreEqual(TimeConverter.Convert(val, TimeValueType.Month)!.Number, "2.5");
        }

        [TestMethod]
        public void HoursToMonthsTest()
        {
            TimeValue val = new TimeValue
            {
                Number = "1440",
                Type = TimeValueType.Hour
            };
            Assert.AreEqual(TimeConverter.Convert(val, TimeValueType.Month)!.Number, "2");
        }
    }
}
