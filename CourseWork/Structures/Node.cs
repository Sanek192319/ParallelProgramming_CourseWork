namespace CourseWork.Structures;

public class Node<TKey, TVal> where TKey: IEquatable<TKey>, IComparable<TKey>
{
    public TKey Key { get; }
    public TVal Value { get; set; }
    
    public Node<TKey, TVal>? FirstChild { get; set; }
    public Node<TKey, TVal>? SecondChild { get; set; }
    public int SubtreeSize { get; private set; }
    public Node<TKey, TVal>? Parent { get; set; }
    public bool IsPassed { get; protected set; }

    public (TKey, TVal)? Pass()
    {
        lock (this)
        {
            if (IsPassed)
                return null;
            IsPassed = true;
        }
        return (Key, Value);
    }

    public Node(Node<TKey, TVal>? parent, TKey key, TVal value)
    {
        Key = key;
        Value = value;
        Parent = parent;
        IsPassed = false;
        SubtreeSize = 1;
    }

    public void Add((TKey Key, TVal Value) child)
    {
        SubtreeSize++;
        if (child.Key.CompareTo(this.Key) == 1)
        {
            if (SecondChild is null)
                SecondChild = new Node<TKey, TVal>(this, child.Key, child.Value);
            else SecondChild.Add(child);
        }
        else
        {
            if (FirstChild is null)
                FirstChild = new Node<TKey, TVal>(this, child.Key, child.Value);
            else FirstChild.Add(child);
        }
    }

    public void Clear()
    {
        IsPassed = false;
        if (FirstChild is not null)
            FirstChild.Clear();
        if (SecondChild is not null)
            SecondChild.Clear();
    }
}