namespace Data.Types.Math
{
    public class Equation
    {
        #region Properties

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public List<IMathComponent> MathComponents { get; set; } = new List<IMathComponent>();

        public OperandGroup? Result { get; private set; }

        #endregion

        #region Methods

        public void Calculate()
        {
            List<Operator> loadedOperators = new List<Operator>();

            for (int i = 0; i < MathComponents.Count; i++)
            {
                var component = MathComponents[i];

                if (component is Operator _operator)
                {
                    _operator.Operand1 = MathComponents[i - 1] as OperandGroup;
                    _operator.Operand2 = MathComponents[i + 1] as OperandGroup;
                    loadedOperators.Add(_operator);
                }

                if (component is OperandGroup _operand)
                {
                    _operand.EquationIndex = i;
                }
            }

            Console.WriteLine(loadedOperators.Select(op => $"{op.Operand1} {op.Type} {op.Operand2} | ").Aggregate("", (accum, curr) => accum + curr));

            // clear off priority operators
            for (int i = 1; i < loadedOperators.Count - 1; i++)
            {
                Operator _operator = loadedOperators[i];
                Operator prevOp = loadedOperators[i - 1];
                Operator nextOp = loadedOperators[i + 1];
                if (_operator.Type == "x" || _operator.Type == "/")
                {
                    if (prevOp.Operand2!.EquationIndex == _operator.Operand1!.EquationIndex)
                    {
                        prevOp.Operand2 = OperandGroup.FromTimeSpan(_operator.GetResult());
                        nextOp.Operand1 = prevOp.Operand2;
                    }
                    if (nextOp.Operand1!.EquationIndex == _operator.Operand2!.EquationIndex)
                    {
                        nextOp.Operand1 = OperandGroup.FromTimeSpan(_operator.GetResult());
                        prevOp.Operand2 = nextOp.Operand1;
                    }
                }
            }

            Console.WriteLine(loadedOperators.Select(op => $"{op.Operand1} {op.Type} {op.Operand2} | ").Aggregate("", (accum, curr) => accum + curr));

            Operator firstLoadedOp = loadedOperators.First();

            if (loadedOperators.Count > 1)
            {
                for (int i = 1; i < loadedOperators.Count; i++)
                {
                    firstLoadedOp.Operand2 = OperandGroup.FromTimeSpan(loadedOperators[i].GetResult());
                    if (i < loadedOperators.Count - 1)
                        loadedOperators[i + 1].Operand1 = firstLoadedOp.Operand2;
                }
            }

            Console.WriteLine($"{firstLoadedOp.Operand1} {firstLoadedOp.Type} {firstLoadedOp.Operand2}");

            Result = OperandGroup.FromTimeSpan(firstLoadedOp.GetResult());
        }

        #endregion

    }
}