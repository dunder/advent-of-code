using System;
using System.Collections.Generic;
using System.Linq;

namespace Y2017.Day09 {
    public class GroupParser {
        public class ParserState {
            public int GroupScore { get; set; }
            public int GroupLevel { get; set; }
            public int CanceldGarbage { get; set; }
        }

        public static ParserState CountGroupScore(string input) {

            Queue<char> sequence = new Queue<char>(input);
            Stack<char> stack = new Stack<char>();

            ParserState parserState = new ParserState();

            ReadGroup(sequence, stack, parserState);

            return parserState;
        }

        private static void ReadGroup(Queue<char> sequence, Stack<char> stack, ParserState parserState) {
            var next = sequence.Peek();
            if (next != '{') {
                throw new InvalidOperationException($"Unexpected character when parsing group: {next}");
            }
            stack.Push(sequence.Dequeue());

            parserState.GroupLevel += 1;

            next = sequence.Peek();
            switch (next) {
                case '<':
                    ReadGarbage(sequence, stack, parserState);
                    break;
                case '{':
                    ReadGroup(sequence, stack, parserState);
                    break;
                case '}':
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected character when parsing content of group: {next}");
            }

            while (sequence.Any() && sequence.Peek() == ',') {
                sequence.Dequeue();
                if (sequence.Peek() == '{') {
                    ReadGroup(sequence, stack, parserState);
                } else {
                    ReadGarbage(sequence, stack, parserState);
                }
            }

            next = sequence.Peek();
            if (next != '}') {
                throw new InvalidOperationException("Expected '}' but found: " + next);
            }
            var expectedStartingBracket = stack.Pop();
            if (expectedStartingBracket != '{') {
                throw new InvalidOperationException("Syntax error");
            }

            parserState.GroupScore += parserState.GroupLevel;
            parserState.GroupLevel -= 1;
            sequence.Dequeue();
        }

        private static void ReadGarbage(Queue<char> sequence, Stack<char> stack, ParserState parserState) {
            var next = sequence.Peek();
            if (next != '<') {
                throw new InvalidOperationException($"Unexpected character when parsing garbage: {next}");
            }
            next = sequence.Dequeue();
            stack.Push(next);
            bool stop = false;
            ;
            while (!stop) {
                next = sequence.Dequeue();
                switch (next) {
                    case '!':
                        sequence.Dequeue();
                        break;
                    case '>':
                        stop = true;
                        break;
                    default:
                        parserState.CanceldGarbage += 1;
                        break;
                }
            }

            var expectedStartingBracket = stack.Pop();

            if (expectedStartingBracket != '<') {
                throw new InvalidOperationException("Syntax error. Unmatched '<' character. Found: " + expectedStartingBracket);
            }
        }
    }
}