using Data.Utils;

namespace Data.Types.TimeCalculator
{

    public class TimeOperator : ITimeMathComponent
    {
        #region Properties

        public string? Type { get; set; }

        public TimeValueGroup? Operand1 { get; set; }

        public TimeValueGroup? Operand2 { get; set; }

        #endregion

        #region Methods

        public TimeValueGroup? GetResult()
        {
            if (Operand1 is null || Operand2 is null)
                return null;

            var op1ValueDict = Operand1.ToValuesDict();
            var op2ValueDict = Operand2.ToValuesDict();

            Dictionary<TimeValueType, double> resultsDict = new Dictionary<TimeValueType, double>();

            if (op2ValueDict is null)
            {
                var multiplier = double.Parse(Operand2.TimeValues[0].Number);
                List<TimeValueGroup> optimizedResults = new List<TimeValueGroup>();
                switch (Type)
                {
                    case "x": 
                        foreach(var key in op1ValueDict!.Keys)
                        {
                            resultsDict[key] = op2ValueDict![key] * multiplier;
                            var timeValue = new TimeValue
                            {
                                Type = key,
                                Number = resultsDict[key].ToString()
                            };
                            optimizedResults.Add(TimeConverter.OptimizeUpwards(timeValue));
                        }
                    break;
                }
            }
        }

        //public TimeValueGroup? GetResult()
        //{
        //    if (Operand1 is null || Operand2 is null)
        //        return null;

        //    var op1SingleVal = Operand1.ToSingleValue();
        //    var op2SingleVal = Operand2.ToSingleValue();

        //    if (op2SingleVal.Type != null)
        //    {
        //        if (op1SingleVal.Type > op2SingleVal.Type)
        //        {
        //            op2SingleVal = TimeConverter.Convert(op2SingleVal, op1SingleVal.Type.Value);
        //        }
        //        else
        //        {
        //            op1SingleVal = TimeConverter.Convert(op1SingleVal, op2SingleVal.Type.Value);
        //        }
        //    }

        //    TimeValue result = new TimeValue { Type = op1SingleVal!.Type };

        //    switch (Type)
        //    {
        //        case "+":
        //            result.Number = (double.Parse(op1SingleVal!.Number) + double.Parse(op2SingleVal!.Number)).ToString();
        //            break;

        //        case "-":
        //            result.Number = (double.Parse(op1SingleVal!.Number) - double.Parse(op2SingleVal!.Number)).ToString();
        //            break;

        //        case "x":
        //            result.Number = (double.Parse(op1SingleVal.Number) * double.Parse(op2SingleVal!.Number)).ToString();
        //            break;

        //        case "/":
        //            result.Number = (double.Parse(op1SingleVal!.Number) / double.Parse(op2SingleVal!.Number)).ToString();
        //            break;

        //        default: return null;

        //    }

        //    TimeValueGroup resultGroup = new TimeValueGroup();


        //}

        #endregion
    }

}