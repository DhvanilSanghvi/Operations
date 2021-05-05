using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;

namespace ProseTutorial
{
    public class WitnessFunctions : DomainLearningLogic
    {
        public WitnessFunctions(Grammar grammar) : base(grammar)
        {
        }

        [WitnessFunction(nameof(Semantics.Add), 1, Verify=true)]
        public ExampleSpec WitnessStartPositionAdd(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<int>) inputState[rule.Body[0]];
                var output = (int)example.Value;
                var occurrences = new List<int>();
                for (int i=0; i<input.Length; i++) {
                    if(input[i] <= output){
                        occurrences.Add(input[i]);
                    }
                }
                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        
        [WitnessFunction(nameof(Semantics.Add), 2, verify = true, DependsOnParameters = new[] {1})]
        public ExampleSpec WitnessEndPositionAdd(GrammarRule rule, ExampleSpec spec, ExampleSpec startSpec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<int>) inputState[rule.Body[0]];
                var output = (int)example.Value;
                var firstNum = (int)startSpec.Examples[inputState];
                var occurrences = new List<int>();
                for (int i=0; i<input.Length; i++) {
                    if(input[i] + firstNum == output){
                        occurrences.Add(i);
                    }
                }
                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences.Cast<object>();
            }
            return new ExampleSpec(result);
        }
    }
}