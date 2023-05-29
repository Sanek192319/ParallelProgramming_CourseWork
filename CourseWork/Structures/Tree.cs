namespace CourseWork.Structures;

public partial class Tree<TKey, TValue> where TKey: IComparable<TKey>, IEquatable<TKey>
{
    public Node<TKey, TValue>? Root { get; private set; }
    public int Count { get; private set; }
    public Tree()
    {
        Count = 0;
    }

    public Tree(IEnumerable<(TKey, TValue)> source)
    {
        var sourceList = source.ToList();
        sourceList.Sort((p1, p2) => p1.Item1.CompareTo(p2.Item1));
        Count = 0;

        foreach (int index in GetBalancingIndexes(sourceList.Count))
        {
            Add(sourceList[index]);
        }
    }

    public void Add((TKey, TValue) element)
    {
        Count++;
        if (Root is null)
        {
            Root = new Node<TKey, TValue>(null, element.Item1, element.Item2);
        }
        else
        {
            Root.Add(element);
        }
    }

    public void Add(TKey key, TValue value) => Add((key, value));
    
    private static IEnumerable<int> GetBalancingIndexes(int elementsNumber)
    {
        if (elementsNumber == 0)
            yield break;
        int ownInd = elementsNumber / 2;
        yield return ownInd;
        if (elementsNumber == 1)
            yield break;
        foreach (var ind in GetBalancingIndexes(ownInd))
        {
            yield return ind;
        }
        
        foreach (var ind in GetBalancingIndexes(elementsNumber - ownInd - 1))
        {
            yield return ownInd + 1 + ind;
        }
    }

    public void ClearNodesUsages()
    {
        Root?.Clear();
    }
}