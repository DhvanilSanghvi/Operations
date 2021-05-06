using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Features;

namespace ProseTutorial
{
    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score")
        {
        }

        protected override double GetFeatureValueForVariable(VariableNode variable)
        {
            return 0;
        }

        [FeatureCalculator(nameof(Semantics.Add))]
        public static double Add(double v, double start, double end)
        {
            if(start + end == 0)    return 1;
            return 1/(start + end);
        }

        

        [FeatureCalculator(nameof(Semantics.Mul))]
        public static double Mul(double v, double start, double end)
        {
            if(start * end == 0)    return 1;
            return 0.1/(start * end);
        }

        [FeatureCalculator(nameof(Semantics.Div))]
        public static double Div(double v, double start, double end)
        {
            if(start * end == 0)    return 1;
            return 0.1/(start * end);
        }

        [FeatureCalculator(nameof(Semantics.Element))]
        public static double Element(double v, double k)
        {
            return 1;
        }

        [FeatureCalculator("k", Method = CalculationMethod.FromLiteral)]
        public static double K(int k)
        {
            return k;
        }
    }
}