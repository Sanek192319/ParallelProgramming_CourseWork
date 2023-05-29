namespace CourseWork.Structures;

public partial class Tree<TKey, TValue>
{
    public async Task<(TKey, TValue)[]> ToSortedArrayConcurrent(Node<TKey, TValue>? subtreeRoot = null)
    {
        Node<TKey, TValue> workingTreeRoot = subtreeRoot ?? Root!;
        (TKey, TValue)[] array = new (TKey, TValue)[Count];
        if (Count == 0)
            return array;
        
        int ctr = 0;
        Node<TKey, TValue>? current = workingTreeRoot;
        while (current != workingTreeRoot!.Parent)
        {
            if (current!.FirstChild is not null && !current.FirstChild.IsPassed)
            {
                current = current.FirstChild;
                continue;
            }
            
            if (current.SecondChild is null || !current.SecondChild.IsPassed)
                array[ctr++] = current.Pass()!.Value;

            if (current.SecondChild is not null && !current.SecondChild.IsPassed)
            {
                current = current.SecondChild;
                continue;
            }
            
            current = current.Parent;
        }

        return array;
    }
}