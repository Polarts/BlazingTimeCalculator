
using Data.Types.Math;

namespace Data.Repositories
{

    public class TimeCalculatorRepository
    {
        public List<Equation> EquationsHistory { get; } = new List<Equation>();

        public Equation CurrentEquation { get; set; } = new Equation();
    }

}
