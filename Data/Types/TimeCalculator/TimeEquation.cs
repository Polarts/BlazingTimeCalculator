namespace Data.Types.TimeCalculator
{
    public class TimeEquation
    {
        #region Properties

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public List<ITimeMathComponent> MathComponents { get; set; } = new List<ITimeMathComponent>();

        public TimeValueGroup? Result { get; private set; }

        #endregion

        #region Methods

        public void Calculate()
        {
            List<TimeOperator> loadedOperators = new List<TimeOperator>();

            for (int i = 0; i < MathComponents.Count; i++)
            {
                var component = MathComponents[i];

                if (component is TimeOperator _operator)
                {
                    _operator.Operand1 = MathComponents[i - 1] as TimeValueGroup;
                    _operator.Operand2 = MathComponents[i + 1] as TimeValueGroup;
                    loadedOperators.Add(_operator);
                }

                if (component is TimeValueGroup _operand)
                {
                    _operand.EquationIndex = i;
                }
            }

            // clear off priority operators
            for (int i = 1; i < loadedOperators.Count - 1; i++)
            {
                TimeOperator _operator = loadedOperators[i];
                TimeOperator prevOp = loadedOperators[i - 1];
                TimeOperator nextOp = loadedOperators[i + 1];
                if (_operator.Type == "x" || _operator.Type == "/")
                {
                    if (prevOp.Operand2!.EquationIndex == _operator.Operand1!.EquationIndex)
                    {
                        prevOp.Operand2 = TimeValueGroup.FromTimeSpan(_operator.GetResult());
                        nextOp.Operand1 = prevOp.Operand2;
                    }
                    if (nextOp.Operand1!.EquationIndex == _operator.Operand2!.EquationIndex)
                    {
                        nextOp.Operand1 = TimeValueGroup.FromTimeSpan(_operator.GetResult());
                        prevOp.Operand2 = nextOp.Operand1;
                    }
                    loadedOperators.RemoveAt(i);
                    i--;
                }
            }

            TimeOperator firstLoadedOp = loadedOperators.First();

            if (firstLoadedOp.Type == "x" || firstLoadedOp.Type == "/")
            {
                firstLoadedOp.Operand1 = TimeValueGroup.FromTimeSpan(firstLoadedOp.GetResult());
                if (loadedOperators.Count > 1)
                {
                    firstLoadedOp.Operand2 = loadedOperators[1].Operand2;
                    firstLoadedOp.Type = loadedOperators[1].Type;
                    loadedOperators.RemoveAt(1);
                }
                else
                {
                    Result = firstLoadedOp.Operand1;
                    return;
                }
            }

            if (loadedOperators.Count > 1)
            {
                for (int i = 1; i < loadedOperators.Count; i++)
                {
                    firstLoadedOp.Operand2 = TimeValueGroup.FromTimeSpan(loadedOperators[i].GetResult());
                    if (i < loadedOperators.Count - 1)
                        loadedOperators[i + 1].Operand1 = firstLoadedOp.Operand2;
                }
            }

            Result = TimeValueGroup.FromTimeSpan(firstLoadedOp.GetResult());
        }

        public void Clear()
        {
            MathComponents.Clear();
            Result = null;
        }

        #endregion

    }
}