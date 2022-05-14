using Data.Types.TimeCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class MathTests
    {
        static TimeValueGroup operand1 = new TimeValueGroup
        {
            TimeValues = new List<TimeValue>
                {
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Year,
                    },
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Day,
                    },
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Hour
                    }
                }
        };

        static TimeSpan operand1TS = TimeSpan.FromHours(1) + TimeSpan.FromDays(366);

        [TestMethod]
        public void TestToTimeSpanConversion()
        {
            Assert.AreEqual(operand1.ToTimeSpan(), operand1TS);
        }

        [TestMethod]
        public void TestFromTimeSpanConversion()
        {
            Assert.AreEqual(operand1.ToString(), TimeValueGroup.FromTimeSpan(operand1TS).ToString());
        }

        [TestMethod]
        public void TestMultiplication()
        {
            var operand2 = new TimeValueGroup
            {
                TimeValues = new List<TimeValue>
                {
                    new TimeValue
                    {
                        Number = "2",
                        Type = null,
                    }
                }
            };
            var equation = new TimeEquation
            {
                MathComponents = new List<ITimeMathComponent>
                {
                    operand1,
                    new TimeOperator
                    {
                        Type = "x",
                        Operand1 = operand1,
                        Operand2 = operand2,
                    },
                    operand2
                }
            };

            equation.Calculate();

            var expectedResult = TimeSpan.FromHours(2) + TimeSpan.FromDays(366 * 2);

            Assert.AreEqual(expectedResult, equation.Result?.ToTimeSpan());
        }

        [TestMethod]
        public void TestDivision()
        {
            var operand2 = new TimeValueGroup
            {
                TimeValues = new List<TimeValue>
                {
                    new TimeValue
                    {
                        Number = "2",
                        Type = null,
                    }
                }
            };
            var equation = new TimeEquation
            {
                MathComponents = new List<ITimeMathComponent>
                {
                    operand1,
                    new TimeOperator
                    {
                        Type = "/",
                        Operand1 = operand1,
                        Operand2 = operand2,
                    },
                    operand2
                }
            };

            equation.Calculate();

            var expectedResult = TimeSpan.FromMilliseconds(operand1TS.TotalMilliseconds / 2);

            Assert.AreEqual(expectedResult, equation.Result?.ToTimeSpan());
        }

        [TestMethod]
        public void TestAddition()
        {
            var operand2 = new TimeValueGroup
            {
                TimeValues = new List<TimeValue>
                {
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Day
                    },
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Month
                    }
                }
            };

            var equation = new TimeEquation
            {
                MathComponents = new List<ITimeMathComponent>
                {
                    operand1,
                    new TimeOperator
                    {
                        Type = "+",
                        Operand1 = operand1,
                        Operand2 = operand2,
                    },
                    operand2
                }
            };


            var expectedResult = TimeSpan.FromHours(1) + TimeSpan.FromDays(397);

            equation.Calculate();

            Assert.AreEqual(expectedResult, equation.Result!.ToTimeSpan());
        }

        [TestMethod]
        public void TestSubtraction()
        {
            var operand2 = new TimeValueGroup
            {
                TimeValues = new List<TimeValue>
                {
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Day
                    },
                    new TimeValue
                    {
                        Number = "1",
                        Type = TimeValueType.Month
                    }
                }
            };

            var equation = new TimeEquation
            {
                MathComponents = new List<ITimeMathComponent>
                {
                    operand1,
                    new TimeOperator
                    {
                        Type = "-",
                        Operand1 = operand1,
                        Operand2 = operand2,
                    },
                    operand2
                }
            };


            var expectedResult = TimeSpan.FromHours(1) + TimeSpan.FromDays(335);

            equation.Calculate();

            Assert.AreEqual(expectedResult, equation.Result!.ToTimeSpan());
        }

        [TestMethod]
        public void TestComplexEquation()
        {
            List<TimeValueGroup> operands = new List<TimeValueGroup>
            {
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "20", Type = TimeValueType.Day },
                    }
                },
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "2" }
                    }
                },
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "3", Type = TimeValueType.Day }
                    }
                }
            };

            var equation = new TimeEquation
            {
                MathComponents = new List<ITimeMathComponent>
                {
                    operands[0],
                    new TimeOperator
                    {
                        Type = "x"
                    },
                    operands[1],
                    new TimeOperator
                    {
                        Type = "+"
                    },
                    operands[2]

                }
            };

            var expectedResult = TimeSpan.FromDays(43);
            equation.Calculate();

            Assert.AreEqual(expectedResult, equation.Result!.ToTimeSpan());
        }

        [TestMethod]
        public void TextComplexEquation2()
        {
            List<TimeValueGroup> operands = new List<TimeValueGroup>
            {
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "20", Type = TimeValueType.Day },
                    }
                },
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "2", Type = TimeValueType.Day }
                    }
                },
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "3" }
                    }
                },
                new TimeValueGroup
                {
                    TimeValues = new List<TimeValue>
                    {
                        new TimeValue { Number = "4", Type = TimeValueType.Min }
                    }
                }
            };

            var equation = new TimeEquation
            {
                MathComponents = new List<ITimeMathComponent>
                {
                    operands[0],
                    new TimeOperator { Type = "+" },
                    operands[1],
                    new TimeOperator { Type = "x" },
                    operands[2],
                    new TimeOperator { Type = "-" },
                    operands[3]
                }
            };

            var expectedResult = TimeSpan.FromDays(26) - TimeSpan.FromMinutes(4);
            equation.Calculate();

            Assert.AreEqual(expectedResult, equation.Result!.ToTimeSpan());
        }
    }
}