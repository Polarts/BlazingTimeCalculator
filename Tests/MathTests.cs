using Data.Types.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void TestMultiplication()
        {
            var operand1 = new OperandGroup
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
    }
}