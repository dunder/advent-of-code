using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Solutions.Event2016.Day21
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2016;
        public override Day Day => Day.Day21;

        private const string Input1 = "abcdefgh";
        private const string Input2 = "fbgdceah";

        public override string FirstStar()
        {
            var instructions = ReadLineInput();
            var result = Scramble(Input1, instructions);
            return result;
        }

        public override string SecondStar()
        {
            var instructions = ReadLineInput();
            var result = ReverseScramble(Input2, instructions.Reverse().ToList());
            return result;
        }

        public static string Scramble(string input, IList<string> instructions)
        {
            var swapIndexExpression = new Regex(@"swap position (\d) with position (\d)");
            var swapLetterExpression = new Regex(@"swap letter (\w) with letter (\w)");
            var rotateRightExpression = new Regex(@"rotate right (\d) steps?");
            var rotateLeftExpression = new Regex(@"rotate left (\d) steps?");
            var rotateLetterExpression = new Regex(@"rotate based on position of letter (\w)");
            var reverseExpression = new Regex(@"reverse positions (\d) through (\d)");
            var moveExpression = new Regex(@"move position (\d) to position (\d)");

            var scrambled = input;
            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case var swapInstruction when swapIndexExpression.IsMatch(swapInstruction):
                        var swapMatch = swapIndexExpression.Match(swapInstruction);
                        var swapX = int.Parse(swapMatch.Groups[1].Value);
                        var swapY = int.Parse(swapMatch.Groups[2].Value);
                        scrambled = SwapIndex(scrambled, swapX, swapY);
                        break;
                    case var swapLetterInstruction when swapLetterExpression.IsMatch(swapLetterInstruction):
                        var swapLetterMatch = swapLetterExpression.Match(swapLetterInstruction);
                        var swapLetter1 = swapLetterMatch.Groups[1].Value.First();
                        var swapLetter2 = swapLetterMatch.Groups[2].Value.First();
                        scrambled = SwapLetter(scrambled, swapLetter1, swapLetter2);
                        break;
                    case var rotateRightInstruction when rotateRightExpression.IsMatch(rotateRightInstruction):
                        var rotateRightMatch = rotateRightExpression.Match(rotateRightInstruction);
                        var rightSteps = int.Parse(rotateRightMatch.Groups[1].Value);
                        scrambled = RotateRight(scrambled, rightSteps);
                        break;
                    case var rotateLeftInstruction when rotateLeftExpression.IsMatch(rotateLeftInstruction):
                        var rotateLeftMatch = rotateLeftExpression.Match(rotateLeftInstruction);
                        var leftSteps = int.Parse(rotateLeftMatch.Groups[1].Value);
                        scrambled = RotateLeft(scrambled, leftSteps);
                        break;
                    case var rotateLetterInstruction when rotateLetterExpression.IsMatch(rotateLetterInstruction):
                        var rotateLetterMatch = rotateLetterExpression.Match(rotateLetterInstruction);
                        var rotateLetter = rotateLetterMatch.Groups[1].Value.First();
                        scrambled = RotateLetter(scrambled, rotateLetter);
                        break;
                    case var reverseInstruction when reverseExpression.IsMatch(reverseInstruction):
                        var reverseMatch = reverseExpression.Match(reverseInstruction);
                        var reverseIndex1 = int.Parse(reverseMatch.Groups[1].Value);
                        var reverseIndex2 = int.Parse(reverseMatch .Groups[2].Value);
                        scrambled = ReverseBetween(scrambled, reverseIndex1, reverseIndex2);
                        break;
                    case var moveInstruction when moveExpression.IsMatch(moveInstruction):
                        var moveMatch = moveExpression.Match(moveInstruction);
                        var moveX = int.Parse(moveMatch.Groups[1].Value);
                        var moveY = int.Parse(moveMatch.Groups[2].Value);
                        scrambled = Move(scrambled, moveX, moveY);
                        break;
                    default:
                        throw new InvalidOperationException($"Cannot match instruction: {instruction}");
                }
            }
            return scrambled;
        }

        public static string ReverseScramble(string input, IList<string> instructions)
        {
            var swapIndexExpression = new Regex(@"swap position (\d) with position (\d)");
            var swapLetterExpression = new Regex(@"swap letter (\w) with letter (\w)");
            var rotateRightExpression = new Regex(@"rotate right (\d) steps?");
            var rotateLeftExpression = new Regex(@"rotate left (\d) steps?");
            var rotateLetterExpression = new Regex(@"rotate based on position of letter (\w)");
            var reverseExpression = new Regex(@"reverse positions (\d) through (\d)");
            var moveExpression = new Regex(@"move position (\d) to position (\d)");

            var scrambled = input;
            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case var swapInstruction when swapIndexExpression.IsMatch(swapInstruction):
                        var swapMatch = swapIndexExpression.Match(swapInstruction);
                        var swapX = int.Parse(swapMatch.Groups[1].Value);
                        var swapY = int.Parse(swapMatch.Groups[2].Value);
                        scrambled = SwapIndex(scrambled, swapX, swapY);
                        break;
                    case var swapLetterInstruction when swapLetterExpression.IsMatch(swapLetterInstruction):
                        var swapLetterMatch = swapLetterExpression.Match(swapLetterInstruction);
                        var swapLetter1 = swapLetterMatch.Groups[1].Value.First();
                        var swapLetter2 = swapLetterMatch.Groups[2].Value.First();
                        scrambled = SwapLetter(scrambled, swapLetter1, swapLetter2);
                        break;
                    case var rotateRightInstruction when rotateRightExpression.IsMatch(rotateRightInstruction):
                        var rotateRightMatch = rotateRightExpression.Match(rotateRightInstruction);
                        var leftSteps = int.Parse(rotateRightMatch.Groups[1].Value);
                        scrambled = RotateLeft(scrambled, leftSteps);
                        break;
                    case var rotateLeftInstruction when rotateLeftExpression.IsMatch(rotateLeftInstruction):
                        var rotateLeftMatch = rotateLeftExpression.Match(rotateLeftInstruction);
                        var rightSteps = int.Parse(rotateLeftMatch.Groups[1].Value);
                        scrambled = RotateRight(scrambled, rightSteps);
                        break;
                    case var rotateLetterInstruction when rotateLetterExpression.IsMatch(rotateLetterInstruction):
                        var rotateLetterMatch = rotateLetterExpression.Match(rotateLetterInstruction);
                        var rotateLetter = rotateLetterMatch.Groups[1].Value.First();
                        scrambled = RotateLetterReversed(scrambled, rotateLetter);
                        break;
                    case var reverseInstruction when reverseExpression.IsMatch(reverseInstruction):
                        var reverseMatch = reverseExpression.Match(reverseInstruction);
                        var reverseIndex1 = int.Parse(reverseMatch.Groups[1].Value);
                        var reverseIndex2 = int.Parse(reverseMatch.Groups[2].Value);
                        scrambled = ReverseBetween(scrambled, reverseIndex1, reverseIndex2);
                        break;
                    case var moveInstruction when moveExpression.IsMatch(moveInstruction):
                        var moveMatch = moveExpression.Match(moveInstruction);
                        var moveX = int.Parse(moveMatch.Groups[1].Value);
                        var moveY = int.Parse(moveMatch.Groups[2].Value);
                        scrambled = Move(scrambled, moveY, moveX);
                        break;
                    default:
                        throw new InvalidOperationException($"Cannot match instruction: {instruction}");
                }
            }
            return scrambled;
        }


        public static string SwapIndex(string input, int x, int y)
        {
            var outputBuilder = new StringBuilder(input)
            {
                [x] = input[y],
                [y] = input[x]
            };

            return outputBuilder.ToString();
        }

        public static string SwapLetter(string input, char c1, char c2)
        {
            var outputBuilder = new StringBuilder(input);

            outputBuilder.Replace(c1, '#');
            outputBuilder.Replace(c2, c1);
            outputBuilder.Replace('#', c2);

            return outputBuilder.ToString();
        }

        public static string RotateLeft(string input, int steps)
        {
            return input.Substring(steps, input.Length - steps) + input.Substring(0, steps);
        }

        public static string RotateRight(string input, int steps)
        {
            steps = steps % input.Length;
            return input.Substring(input.Length - steps, steps) + input.Substring(0, input.Length - steps);
        }

        public static string RotateLetter(string input, char letter)
        {
            int steps = 1;

            steps += input.IndexOf(letter);

            if (input.IndexOf(letter) >= 4)
            {
                steps++;
            }
            
            return RotateRight(input, steps);
        }

        public static string RotateLetterReversed(string input, char letter)
        {
            var output = input;
            do
            {
                output = RotateLeft(output, 1);
            }
            while (RotateLetter(output, letter) != input);

            return output;
        }

        public static string ReverseBetween(string input, int x, int y)
        {
            var outputBuilder = new StringBuilder(input);

            var reverse = input.Substring(x, y - x + 1);
            var reversed = reverse.Reverse().ToList();
            for (int i = x; i <= y; i++)
            {
                outputBuilder[i] = reversed[i - x];
            }

            return outputBuilder.ToString();
        }

        public static string Move(string input, int x, int y)
        {
            var outputBuilder = new StringBuilder(input);
            var toMove = input[x];
            outputBuilder.Remove(x, 1);
            outputBuilder.Insert(y, toMove);

            return outputBuilder.ToString();
        }
    }
}