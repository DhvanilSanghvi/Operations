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

        // static doesnt work because, we cannot reference it by "this"
        // Documentation not in Ranking but in "Features"
        
        [FeatureCalculator(nameof(Semantics.Add), Method = CalculationMethod.FromChildrenNodes)]
        public double Add(VariableNode v, ProgramNode start, ProgramNode end) 
        {
            double score = (double) start.GetFeatureValue(this) + (double) end.GetFeatureValue(this);
            if(score == 0)    return 1;
            return 1/score;
        }

        

        // [FeatureCalculator(nameof(Semantics.Mul))]
        // public  double Mul(double v, double start, double end)
        // {
        //     if(start * end == 0)    return 1;
        //     return 1/(start * end);
        // }

        [FeatureCalculator(nameof(Semantics.Mul), Method = CalculationMethod.FromChildrenNodes)]
        public double Mul(VariableNode v, ProgramNode start, ProgramNode end) 
        {
            double score = (double) start.GetFeatureValue(this) * (double) end.GetFeatureValue(this);
            if(score == 0)    return 1;
            return 1/score;
        }

        // [FeatureCalculator(nameof(Semantics.Div))]
        // public  double Div(double v, double start, double end)
        // {
        //     if(start * end == 0)    return 1; 
        //     return 1/(start * end);
        // }

        [FeatureCalculator(nameof(Semantics.Div), Method = CalculationMethod.FromChildrenNodes)]
        public double Div(VariableNode v, ProgramNode start, ProgramNode end) 
        {
            double score = (double) start.GetFeatureValue(this) * (double) end.GetFeatureValue(this);
            if(score == 0)    return 1;
            return 1/score;
        }

        [FeatureCalculator(nameof(Semantics.Element))]
        public double Element(double v, double k)
        {
            return 1;
        }

        [FeatureCalculator("k", Method = CalculationMethod.FromLiteral)]
        public double K(int k)
        {
            if(k == 0){
                return 1;
            }
            return 1/k;
        }
    }
}