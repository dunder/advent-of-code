using System.Collections.Generic;
using System.Linq;

namespace Solutions.Event2018.Day08
{
    public class Problem : ProblemBase
    {
        public override Event Event => Event.Event2018;
        public override Day Day => Day.Day08;

        public override string FirstStar()
        {
            var input = ReadInput();
            var sumOfMetadataEntries = CalculateSumOfMetadata(input);
            return sumOfMetadataEntries.ToString();
        }

        public override string SecondStar()
        {
            var input = ReadInput();
            var result = "Not implemented";
            return result.ToString();
        }
        public static int CalculateSumOfMetadata(string input)
        {
            var data = input.Split(' ').Select(int.Parse).ToList();
            var stack = new Stack<int>(data);
            var allMetadata = new List<int>();

            while (stack.Any())
            {
                var nrChildren = stack.Pop();
                var nrMetadata = stack.Pop();

                for (int _ = 0; _ < nrChildren; _++)
                {
                    stack.Pop();
                }

                for (int _ = 0; _ < nrMetadata; _++)
                {
                    var metadata = stack.Pop();
                    allMetadata.Add(metadata);
                }
            }

            return allMetadata.Sum();
        }

        public static (int nrChildren, int nrMetadata) ReadHeader(Stack<int> stack)
        {
            return (stack.Pop(), stack.Pop());
        }

        public static Node ReadNode(Stack<int> stack)
        {
            var header = ReadHeader(stack);
            var children = ReadChildren(stack, header.nrChildren);
            var metadata = ReadMetadata(stack, header.nrMetadata);
            return new Node
            {

            };
        }

        private static List<int> ReadMetadata(Stack<int> stack, int nrMetadata)
        {
            var metadatas = new List<int>();
            for (int _ = 0; _ < nrMetadata; _++)
            {
                var metaData = stack.Pop();
                metadatas.Add(metaData);
            }

            return metadatas;
        }

        public static List<Node> ReadChildren(Stack<int> stack, int nrChildren)
        {
            var children = new List<Node>();
            for (int _ = 0; _ < nrChildren; _++)
            {
                var child = ReadNode(stack);
                children.Add(child);
            }

            return children;
        }

        public class Node
        {
            private static int CurrentId = (int)'A';

            public int NrChildren { get; set; }
            public int NrMetadata { get; set; }
            public List<int> Metadata { get; set; }
            public List<Node> Children { get; set; }
        }
    }
}