using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day08 : ASolution
    {
        private List<Instruction> _program;
        public Day08() : base(08, 2020, "")
        {
            _program = Input.SplitByNewline().Select(Instruction.FromLine).ToList();
        }

        protected override string SolvePartOne()
        {
            var execution = RunProgram(_program);
            return execution.AccumulatorValue.ToString();
        }

        private (bool RanToCompletion, long AccumulatorValue) RunProgram(List<Instruction> instructions)
        {
            var accumulator = 0;
            var executedInstructions = new HashSet<int>();
            var programCounter = 0;
            while (!executedInstructions.Contains(programCounter))
            {
                if (programCounter >= instructions.Count)
                {
                    break;
                }
                executedInstructions.Add(programCounter);
                var instructionToExecute = instructions[programCounter];
                switch (instructionToExecute.Operation)
                {
                    case "nop":
                        programCounter += 1;
                        break;
                    case "acc":
                        accumulator += instructionToExecute.Operand;
                        programCounter += 1;
                        break;
                    case "jmp":
                        programCounter += instructionToExecute.Operand;
                        break;
                }
            }

            if (executedInstructions.Contains(programCounter))
            {
                return (RanToCompletion: false, accumulator);
            }
            
            return (RanToCompletion: true, accumulator);
        }
        
        protected override string SolvePartTwo()
        {
            for (var i = 0; i < _program.Count; i++)
            {
                var instruction = _program[i];
                // try to run with changed instruction
                var newInstruction = instruction.Operation switch
                {
                    "nop" => new Instruction("jmp", instruction.Operand),
                    "jmp" => new Instruction("nop", instruction.Operand),
                    _ => instruction
                };

                _program[i] = newInstruction;

                var result = RunProgram(_program);
                if (result.RanToCompletion) // ran to completion 
                {
                    return result.AccumulatorValue.ToString();
                } 
                _program[i] = instruction;
            }

            return "Oops";
        }
        
        
        private class Instruction
        {
            public string Operation { get; set; }
            public int Operand { get; set; }

            public Instruction()
            { }

            public Instruction(string op, int operand)
            {
                Operation = op;
                Operand = operand;
            }
            
            public static Instruction FromLine(string line)
            {
                var split = line.Split();
                var operand = int.Parse(split[1]);
                return new Instruction
                {
                    Operation = split[0],
                    Operand = operand
                };
            }

            public override string ToString()
            {
                return $"{Operation} {Operand}";
            }
        }
        
    }
}
