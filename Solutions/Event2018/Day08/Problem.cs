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
            var valueOfRoot = CalcualateValueOfRootNode(input);
            return valueOfRoot.ToString();
        }

        private static List<int> _allMetadata = new List<int>();

        public static int CalculateSumOfMetadata(string input)
        {
            var data = input.Split(' ').Select(int.Parse);

            var stack = new Stack<int>(data.Reverse());
            

            var topNode = ReadNode(stack);

            return _allMetadata.Sum();
        }

        public static int CalcualateValueOfRootNode(string input)
        {
            var data = input.Split(' ').Select(int.Parse);

            var stack = new Stack<int>(data.Reverse());


            var topNode = ReadNode(stack);

            return topNode.Value;
        }

        public static (int nrChildren, int nrMetadata) ReadHeader(Stack<int> stack)
        {
            return (stack.Pop(), stack.Pop());
        }

        public static Node ReadNode(Stack<int> stack)
        {
            var node = new Node();
            var header = ReadHeader(stack);
            var children = ReadChildren(stack, header.nrChildren);
            var metadata = ReadMetadata(stack, header.nrMetadata);

            node.Children = children;
            node.Metadata = metadata;
            return node;
        }

        private static List<int> ReadMetadata(Stack<int> stack, int nrMetadata)
        {
            var metadatas = new List<int>();
            for (int _ = 0; _ < nrMetadata; _++)
            {
                var metaData = stack.Pop();
                metadatas.Add(metaData);
                _allMetadata.Add(metaData);
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
            private static int _currentId = 'A';
            private int _id;

            public Node()
            {
                _id = _currentId++;
            }

            public char Id => (char)_id;
            public List<int> Metadata { get; set; }
            public List<Node> Children { get; set; }

            public int Value
            {
                get
                {
                    if (!Children.Any())
                    {
                        return Metadata.Sum();
                    }

                    var value = 0;

                    foreach (var i in Metadata)
                    {
                        if (i == 0)
                        {
                            continue;
                        }

                        var childIndex = i - 1;

                        if (childIndex < Children.Count)
                        {
                            var child = Children[childIndex];
                            value += child.Value;
                        }
                    }

                    return value;
                }
            }
        }
    }
}