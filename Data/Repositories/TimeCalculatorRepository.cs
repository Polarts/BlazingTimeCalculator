
using Data.Types.TimeCalculator;

namespace Data.Repositories
{

    public class TimeCalculatorRepository
    {
        public List<TimeEquation> EquationsHistory { get; } = new List<TimeEquation>();

        public TimeEquation CurrentEquation { get; set; } = new TimeEquation();
    }

}
