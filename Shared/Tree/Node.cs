namespace Shared.Tree
{
    public class Node<T>
    {
        public T Data { get; }
        public int Depth { get; }

        public Node(T data, int depth)
        {
            Data = data;
            Depth = depth;
        }
    }
}