using Data.Types.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class MathTests
    {
        static OperandGroup operand1 = new OperandGroup
        {
            Operands = new List<Operand>
                {
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Year,
                    },
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Day,
                    },
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Hour
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
            Assert.AreEqual(operand1.ToString(), OperandGroup.FromTimeSpan(operand1TS).ToString());
        }

        [TestMethod]
        public void TestMultiplication()
        {
            var operand2 = new OperandGroup
            {
                Operands = new List<Operand>
                {
                    new Operand
                    {
                        Number = "2",
                        Type = null,
                    }
                }
            };
            var equation = new Equation
            {
                MathComponents = new List<IMathComponent>
                {
                    operand1,
                    new Operator
                    {
                        Type = "x",
                        Operand1 = operand1,
                        Operand2 = operand2,
                    },
                    operand2
                }
            };

            equation.Calculate();

            var expectedResult = TimeSpan.FromHours(2) + TimeSpan.FromDays(366*2);

            Assert.AreEqual(expectedResult, equation.Result?.ToTimeSpan());
        }

        [TestMethod]
        public void TestDivision()
        {
            var operand2 = new OperandGroup
            {
                Operands = new List<Operand>
                {
                    new Operand
                    {
                        Number = "2",
                        Type = null,
                    }
                }
            };
            var equation = new Equation
            {
                MathComponents = new List<IMathComponent>
                {
                    operand1,
                    new Operator
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
            var operand2 = new OperandGroup
            {
                Operands = new List<Operand>
                {
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Day
                    },
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Month
                    }
                }
            };

            var equation = new Equation
            {
                MathComponents = new List<IMathComponent>
                {
                    operand1,
                    new Operator
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
            var operand2 = new OperandGroup
            {
                Operands = new List<Operand>
                {
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Day
                    },
                    new Operand
                    {
                        Number = "1",
                        Type = OperandType.Month
                    }
                }
            };

            var equation = new Equation
            {
                MathComponents = new List<IMathComponent>
                {
                    operand1,
                    new Operator
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
            List<OperandGroup> operands = new List<OperandGroup>
            {
                new OperandGroup
                {
                    Operands = new List<Operand>
                    {
                        new Operand { Number = "20", Type = OperandType.Day },
                    }
                },
                new OperandGroup
                {
                    Operands = new List<Operand>
                    {
                        new Operand { Number = "2" }
                    }
                },
                new OperandGroup
                {
                    Operands = new List<Operand>
                    {
                        new Operand { Number = "3", Type = OperandType.Day }
                    }
                }
            };

            var equation = new Equation
            {
                MathComponents = new List<IMathComponent>
                {
                    operands[0],
                    new Operator
                    {
                        Type = "x"
                    },
                    operands[1],
                    new Operator 
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
    }
}