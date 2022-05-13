namespace AdventOfCode
{
    public static class LinkedListExtensions
    {
        public static LinkedListNode<T> MoveCircular<T>(this LinkedListNode<T> node, int move)
        {
            int dir = Math.Sign(move);

            while (move != 0)
            {
                if (dir == 1)
                {
                    if (node.Next != null)
                        node = node.Next;
                    else
                        node = node.List.First;
                }
                else
                {
                    if (node.Previous != null)
                        node = node.Previous;
                    else
                        node = node.List.Last;
                }

                move -= dir;
            }

            return node;
        }
    }

    public class CircularList<T> : IEnumerable<T>
    {
        LinkedList<T> list;

        public LinkedListNode<T> First { get; private set; }
        public int Count { get { return list.Count; } }

        public CircularList()
        {
            SetList(new LinkedList<T>());
        }

        public CircularList(IEnumerable<T> items)
        {
            SetList(new LinkedList<T>(items));
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = First;

            if (node != null)
            {
                do
                {
                    yield return node.Value;

                    node = node.MoveCircular(1);
                }
                while (node != First);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void SetList(LinkedList<T> list)
        {
            this.list = list;

            First = list.First;
        }

        public virtual T this[int pos]
        {
            get
            {
                return Position(pos).Value;
            }

            set
            {
                Position(pos).Value = value;
            }
        }

        public LinkedListNode<T> Position(int pos)
        {
            if (pos > (Count - 1))
            {
                throw new IndexOutOfRangeException();
            }

            var node = First;

            for (int i = 0; i < pos; i++)
            {
                node = node.MoveCircular(1);
            }

            return node;
        }

        public LinkedListNode<T> Find(T value)
        {
            var node = First;

            do
            {
                if (node.Value.Equals(value))
                    return node;

                node = node.MoveCircular(1);
            }
            while (node != First);

            return null;
        }

        public int IndexOf(T value)
        {
            int pos = 0;

            var node = First;

            if (node == null)
                return -1;

            do
            {
                if (node.Value.Equals(value))
                    return pos;

                node = node.MoveCircular(1);

                pos++;
            }
            while (node != First);

            return -1;
        }

        public void Remove(LinkedListNode<T> node)
        {
            if (First == node)
                First = node.Next;

            list.Remove(node);
        }

        public T RemoveAt(int position)
        {
            var node = Position(position);

            T value = node.Value;

            Remove(node);

            return value;
        }

        public void InsertAt(int position, T value)
        {
            if (position == Count)
                list.AddBefore(First, value);
            else
            {
                var node = Position(position);

                list.AddBefore(node, value);

                if (node == First)
                {
                    First = node.Previous;
                }
            }
        }

        public void InsertAt(int position, LinkedListNode<T> node)
        {
            if (position == Count)
                list.AddBefore(First, node);
            else
            {
                var posNode = Position(position);

                list.AddBefore(posNode, node);

                if (posNode == First)
                {
                    First = posNode.Previous;
                }
            }
        }

        public void InsertBefore(LinkedListNode<T> node, T value)
        {
            list.AddBefore(node, value);
        }

        public void InsertBefore(LinkedListNode<T> beforeNode, LinkedListNode<T> node)
        {
            list.AddBefore(beforeNode, node);
        }

        public void InsertAfter(LinkedListNode<T> node, T value)
        {
            list.AddAfter(node, value);
        }

        public void InsertAfter(LinkedListNode<T> afterNode, LinkedListNode<T> node)
        {
            list.AddAfter(afterNode, node);
        }

        public void Rotate(int offset)
        {
            First = First.MoveCircular(-offset);
        }
    }
}
