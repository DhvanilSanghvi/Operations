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

        [WitnessFunction(nameof(Semantics.Add), 1 , Verify=true)]
        public DisjunctiveExamplesSpec WitnessStartPositionAdd(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<uint?>) inputState[rule.Body[0]];
                var output = (uint?)example.Value;
                var occurrences = new List<uint?>();
                // for (int i=0; i<input.Count; i++) {
                //     if(input[i] <= output){
                //         occurrences.Add(input[i]);
                //     }
                // }
                for(uint i=1; i<output; i++){
                    occurrences.Add(i);
                }
                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        
        [WitnessFunction(nameof(Semantics.Add), 2, Verify=true, DependsOnParameters = new[] {1})]
        public ExampleSpec WitnessEndPositionAdd(GrammarRule rule, ExampleSpec spec, ExampleSpec startSpec)
        {
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<uint?>) inputState[rule.Body[0]];
                var output = (uint?)example.Value;
                var firstNum = (uint?)startSpec.Examples[inputState];

                // for (int i=0; i<input.Count; i++) {
                //     if(input[i] + firstNum == output){
                //         result[inputState] = input[i];
                //         break;
                //     }
                // }
                if(output < firstNum)   return null;
                result[inputState] = output-firstNum;
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Mul), 1 , Verify=true)]
        public DisjunctiveExamplesSpec WitnessStartPositionMul(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<uint?>) inputState[rule.Body[0]];
                var output = (uint?)example.Value;
                var occurrences = new List<uint?>();
                // for (int i=0; i<input.Count; i++) {
                //     if(input[i] <= output){
                //         if(input[i]!=1)
                //             occurrences.Add(input[i]);
                //     }
                // }
                for(uint i=1+1; i<output; i++){
                    occurrences.Add(i);
                }
                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        
        [WitnessFunction(nameof(Semantics.Mul), 2, Verify=true, DependsOnParameters = new[] {1})]
        public ExampleSpec WitnessEndPositionMul(GrammarRule rule, ExampleSpec spec, ExampleSpec startSpec)
        {
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<uint?>) inputState[rule.Body[0]];
                var output = (uint?)example.Value;
                var firstNum = (uint?)startSpec.Examples[inputState];

                // for (int i=0; i<input.Count; i++) {
                //     if(input[i]*firstNum == output){
                //         if(input[i] != 1){
                //             result[inputState] = input[i];
                //             break;
                //         }
                //     }
                // }
                if(output < firstNum)   return null;
                if(output%firstNum == 0){
                    result[inputState] = output/firstNum;
                }
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Div), 1, Verify=true )]
        public DisjunctiveExamplesSpec WitnessStartPositionDiv(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<uint?>) inputState[rule.Body[0]];
                var output = (uint?)example.Value;
                var occurrences = new List<uint?>();
                // for (int i=0; i<input.Count; i++) {
                //     if(input[i] >= output){
                //         if(input[i]!=1)
                //             occurrences.Add(input[i]);
                //     }
                // }
                for(uint? i=output+1; i<10*output; i++){
                    occurrences.Add((uint)i);
                }

                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);
        }

        
        [WitnessFunction(nameof(Semantics.Div), 2, Verify=true, DependsOnParameters = new[] {1})]
        public ExampleSpec WitnessEndPositionDiv(GrammarRule rule, ExampleSpec spec, ExampleSpec startSpec)
        {
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                State inputState = example.Key;
                var input = (List<uint?>) inputState[rule.Body[0]];
                var output = (uint?)example.Value;
                var firstNum = (uint?)startSpec.Examples[inputState];
                int flag = 0;
                for (int i=0; i<input.Count; i++) {
                    if(input[i] == 0)   continue;
                    if(firstNum < input[i] || firstNum % input[i] != 0)    continue;
                    if(firstNum/input[i] == output){
                        if(input[i]!=1 && input[i] != output){ 
                            result[inputState] = input[i];
                            flag = 1;
                            break;
                        }
                    }
                }
                if(flag == 0)   return null;
            }
            return new ExampleSpec(result);
        }

        // [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        // public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, DisjunctiveExamplesSpec spec)
        // {
        //     var kExamples = new Dictionary<State, IEnumerable<object>>();
        //     foreach (KeyValuePair<State, IEnumerable<object>> example in spec.DisjunctiveExamples)
        //     {
        //         State inputState = example.Key;
        //         var v = inputState[rule.Body[0]] as string;

        //         var positions = new List<int>();
        //         foreach (int pos in example.Value)
        //         {
        //             positions.Add(pos + 1);
        //             positions.Add(pos - v.Length - 1);
        //         }
        //         if (positions.Count == 0) return null;
        //         kExamples[inputState] = positions.Cast<object>();
        //     }
        //     return DisjunctiveExamplesSpec.From(kExamples);
        // }

        [WitnessFunction(nameof(Semantics.Element), 1)]
        public DisjunctiveExamplesSpec WitnessElement(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, IEnumerable<object>> example in spec.DisjunctiveExamples)
            {
                State inputState = example.Key;
                List<uint?> v = (List<uint?>)inputState[rule.Body[0]];

                var positions = new List<int>();
                foreach (uint? num in example.Value)
                {
                    for(int i=0; i<v.Count; i++){
                        if(v[i] == num){
                            positions.Add(i);
                        }
                    }
                }
                if (positions.Count == 0) return null;
                kExamples[inputState] = positions.Cast<object>();
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }
    }
}