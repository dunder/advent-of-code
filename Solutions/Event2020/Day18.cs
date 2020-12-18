using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using static Solutions.InputReader;


namespace Solutions.Event2020
{
    // --- Day 18: Operation Order ---

    public class Day18
    {
        private readonly ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        public List<string> Split(string part)
        {
            var parts = new List<string>();

            if (part.Contains('(') || part.Contains(')'))
            {
                while (part.Length > 0 && part[0] == '(')
                {
                    parts.Add("(");
                    part = part.Length == 1 ? part : part.Substring(1);
                }

                var endParan = part.IndexOf(')');

                if (endParan == -1)
                {
                    parts.Add(part);
                    part = "";
                }
                else
                {
                    var number = part.Substring(0, endParan);
                    parts.Add(number);
                    part = part.Substring(endParan);
                }

                while (part.Length > 0)
                {
                    parts.Add(")");
                    part = part.Length == 1 ? "" : part.Substring(1);
                }
            }
            else
            {
                parts.Add(part);
            }

            return parts;
        }

        private bool IsAddition(string candidate) => candidate == "+";
        private bool IsMultiplication(string candidate) => candidate == "*";
        private bool IsNumber(string candidate) => int.TryParse(candidate, out int _);
        

        public Queue<string> ToQueue(string line)
        {
            return new Queue<string>(line.Split(" ").SelectMany(p => Split(p)));
        }

        public long SumLines(List<string> lines, bool plusHasPrecedence)
        {
            return lines.Aggregate(0L, (sum, x) => sum + EvaluateLine(x, plusHasPrecedence));
        }

        public long EvaluateLine(string line, bool plusHasPrecedence)
        {
            var result = EvaluateExpression(ToQueue(line), plusHasPrecedence);
            return result;
        }

        public long Evaluate(string operation, Stack<long> calculationStack)
        {
            if (IsAddition(operation))
            {
                return calculationStack.Pop() + calculationStack.Pop();
            }
            else
            {
                return calculationStack.Pop() * calculationStack.Pop();
            }
        }

        public long EvaluateExpression(Queue<string> input, bool plusHasPrecedence)
        {
            var operations = new Stack<string>();
            var calculationStack = new Stack<long>();

            while (input.Any())
            {
                string next = input.Dequeue();
                if (IsNumber(next))
                {
                    calculationStack.Push(int.Parse(next));
                }
                else if (next == "(")
                {
                    var result = EvaluateExpression(input, plusHasPrecedence);
                    calculationStack.Push(result);
                }
                else if (next == ")")
                {
                    break;
                }
                else
                {
                    if (!operations.Any() || (plusHasPrecedence && IsMultiplication(operations.Peek())))
                    {
                        operations.Push(next);
                    }
                    else
                    {
                        var result = Evaluate(operations.Pop(), calculationStack);
                        calculationStack.Push(result);
                        operations.Push(next);
                    }
                }
            }

            while (operations.Any())
            {
                var result = Evaluate(operations.Pop(), calculationStack);
                calculationStack.Push(result);
            }

            return calculationStack.Peek();
        }

        public long FirstStar()
        {
            var input = ReadLineInput().ToList();
            var result = SumLines(input, plusHasPrecedence: false);
            return result;
        }

        public long SecondStar()
        {
            var input = ReadLineInput().ToList();
            var result = SumLines(input, plusHasPrecedence: true);
            return result;
        }

        [Fact]
        public void FirstStarTest()
        {
            Assert.Equal(12956356593940, FirstStar());
        }

        [Fact]
        public void SecondStarTest()
        {
            Assert.Equal(94240043727614, SecondStar());
        }

        [Theory]
        [InlineData("2 * 3 + (4 * 5)", 26)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void FirstStarExample(string line, long expected)
        {
            var expression = EvaluateLine(line, plusHasPrecedence: false);
            Assert.Equal(expected, expression);
        }

        [Theory]
        [InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [InlineData("2 * 3 + (4 * 5)", 46)]
        [InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void SecondStarExample(string line, long expected)
        {
            var expression = EvaluateLine(line, plusHasPrecedence: true);
            Assert.Equal(expected, expression);
        }
    }
}
