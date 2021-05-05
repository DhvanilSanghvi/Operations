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

        [FeatureCalculator(nameof(Semantics.AddFinal))]
        public static double AddFinal(double v, double start, double end)
        {
            return start + end;
        }

        [FeatureCalculator(nameof(Semantics.Add))]
        public static double Add(double v, double start, double end)
        {
            return start + end;
        }

        
        [FeatureCalculator(nameof(Semantics.MulFinal))]
        public static double MulFinal(double v, double start, double end)
        {
            return start * end;
        }

        [FeatureCalculator(nameof(Semantics.Mul))]
        public static double Mul(double v, double start, double end)
        {
            return start * end;
        }

        [FeatureCalculator(nameof(Semantics.DivFinal))]
        public static double DivFinal(double v, double start, double end)
        {
            return start * end;
        }

        [FeatureCalculator(nameof(Semantics.Div))]
        public static double Div(double v, double start, double end)
        {
            return start * end;
        }

        [FeatureCalculator(nameof(Semantics.Element))]
        public static double Element(double v, double k)
        {
            return 0;
        }

        [FeatureCalculator(nameof(Semantics.ElementFinal))]
        public static double ElementFinal(double v, double k)
        {
            return 0;
        }
    }
}