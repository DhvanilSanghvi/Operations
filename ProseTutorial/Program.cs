using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.VersionSpace; 

namespace ProseTutorial
{
    internal class Program
    {
        private static readonly Grammar Grammar = DSLCompiler.Compile(new CompilerOptions
        {
            InputGrammarText = File.ReadAllText("synthesis/grammar/substring.grammar"),
            References = CompilerReference.FromAssemblyFiles(typeof(Program).GetTypeInfo().Assembly)
        }).Value;

        private static SynthesisEngine _prose;

        private static readonly Dictionary<State, object> Examples = new Dictionary<State, object>();
        private static ProgramNode _topProgram;

        private static void Main(string[] args)
        {
            _prose = ConfigureSynthesis();
            var menu = @"Select one of the options: 
1 - provide new example
2 - run top synthesized program on a new input
3 - exit";
            var option = 0;
            while (option != 3)
            {
                Console.Out.WriteLine(menu);
                try
                {
                    option = short.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Out.WriteLine("Invalid option. Try again.");
                    continue;
                }

                try
                {
                    RunOption(option);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Something went wrong...");
                    Console.Error.WriteLine("Exception message: {0}", e.Message);
                }
            }
        }

        private static void RunOption(int option)
        {
            switch (option)
            {
                case 1:
                    LearnFromNewExample();
                    break;
                case 2:
                    RunOnNewInput();
                    break;
                default:
                    Console.Out.WriteLine("Invalid option. Try again.");
                    break;
            }
        }

        private static void LearnFromNewExample()
        {
            Console.Out.Write("Provide a new input-output example (e.g., [1,2,3,4],7 ) : ");
            try
            {
                string input = Console.ReadLine();
                if (input != null)
                {
                    List<uint?> inputList = new List<uint?>();
                    string temp = string.Empty;
                    int startList = input.IndexOf('[');
                    int endList = input.IndexOf(']');
                    string listString = input.Substring(startList, endList-startList+1);
                    // Console.Out.Write("here");

                    for(int i=1; i<listString.Length; i++){
                        // Console.Out.Write("here\n");
                        if(listString[i].Equals(',') || listString[i].Equals(']')){
                            // Save temp string as number
                            inputList.Add(UInt32.Parse(temp));
                            // Make temp string as empty
                            temp = string.Empty;
                            // Console.Out.Write("here1\n");
                        }
                        else{
                            // Append to the temp string
                            temp = temp + listString[i];
                            // Console.Out.Write("here2\n");
                        }
                    }
                    // Console.Out.Write("here");

                    uint? expectedOutput = UInt32.Parse(input.Substring(endList+2));
                    State inputState = State.CreateForExecution(Grammar.InputSymbol, inputList);
                    // Console.Out.Write("here");
                    Examples.Add(inputState, expectedOutput);

                    // if (startFirstExample >= endFirstExample || startSecondExample >= endSecondExample)
                    //     throw new Exception(
                    //         "Invalid example format. Please try again. input and out should be between quotes");

                    // string inputExample = input.Substring(startFirstExample, endFirstExample - startFirstExample - 1);
                    // string outputExample =
                    //     input.Substring(startSecondExample, endSecondExample - startSecondExample - 1);

                    // State inputState = State.CreateForExecution(Grammar.InputSymbol, inputExample);
                    // Examples.Add(inputState, outputExample);
                }
            }
            catch (Exception)
            {
                throw new Exception("Invalid example format. Please try again. input and out should be between quotes");
            }

            var spec = new ExampleSpec(Examples);
            Console.Out.WriteLine("Learning a program for examples:");
            foreach (KeyValuePair<State, object> example in Examples){
                List<uint?> l = (List<uint?>) example.Key.Bindings.First().Value;
                Console.Write("[");
                int len = l.Count;
                // foreach(uint a in l){
                //     Console.Write("{0},", a);
                // }
                for(int j=0; j<len-1; j++){
                    Console.Write("{0},", l[j]);
                }
                Console.Write("{0}", l[len-1]);
                Console.Write("]");
                Console.Write(" -> {0}\n", example.Value);
            }

            var scoreFeature = new RankingScore(Grammar);
            ProgramSet topPrograms = _prose.LearnGrammarTopK(spec, scoreFeature, 4, null);
            if (topPrograms.IsEmpty) throw new Exception("No program was found for this specification.");

            _topProgram = topPrograms.RealizedPrograms.First();
            Console.Out.WriteLine("Top 4 learned programs:");
            var counter = 1;
            foreach (ProgramNode program in topPrograms.RealizedPrograms)
            {
                if (counter > 4) break;
                Console.Out.WriteLine("==========================");
                Console.Out.WriteLine("Program {0}: ", counter);
                Console.Out.WriteLine(program.PrintAST(ASTSerializationFormat.HumanReadable));
                counter++;
            }
        }

        private static void RunOnNewInput()
        {
            if (_topProgram == null)
                throw new Exception("No program was synthesized. Try to provide new examples first.");
            Console.Out.WriteLine("Top program: {0}", _topProgram);

            try
            {
                Console.Out.Write("Insert a new input: ");
                string newInput = Console.ReadLine();
                if (newInput != null)
                {
                    int startFirstExample = newInput.IndexOf("\"", StringComparison.Ordinal) + 1;
                    int endFirstExample = newInput.IndexOf("\"", startFirstExample + 1, StringComparison.Ordinal) + 1;
                    newInput = newInput.Substring(startFirstExample, endFirstExample - startFirstExample - 1);
                    State newInputState = State.CreateForExecution(Grammar.InputSymbol, newInput);
                    Console.Out.WriteLine("RESULT: \"{0}\" -> \"{1}\"", newInput, _topProgram.Invoke(newInputState));
                }
            }
            catch (Exception)
            {
                throw new Exception("The execution of the program on this input thrown an exception");
            }
        }

        public static SynthesisEngine ConfigureSynthesis()
        {
            var witnessFunctions = new WitnessFunctions(Grammar);
            var deductiveSynthesis = new DeductiveSynthesis(witnessFunctions);
            var synthesisExtrategies = new ISynthesisStrategy[] {deductiveSynthesis};
            var synthesisConfig = new SynthesisEngine.Config {Strategies = synthesisExtrategies};
            var prose = new SynthesisEngine(Grammar, synthesisConfig);
            return prose;
        }
    }
}