namespace CourseWork.Structures;

public partial class Tree<TKey, TValue>
{
    private (TKey, TValue)[] finalResult;
    private List<Task> activeTasks;
    public async Task<(TKey, TValue)[]> ToSortedArrayParallel(Node<TKey, TValue>? subtreeRoot = null, int threads = 8)
    {
        Node<TKey, TValue> workingTreeRoot = subtreeRoot! ?? Root!;
        if (Count == 0)
        {
            return Array.Empty<(TKey, TValue)>();
        }

        activeTasks = new List<Task>();
        finalResult = new (TKey, TValue)[workingTreeRoot.SubtreeSize];
        
        await ParallelPassThrough(workingTreeRoot, threads, 0);
        await Task.WhenAll(activeTasks);
        return finalResult;
    }

    private async Task ParallelPassThrough(Node<TKey, TValue> subtreeRoot, int threads, int arrayStartingIndex)
    {
        if (threads < 2)
        {
            (await ToSortedArrayConcurrent(subtreeRoot)).CopyTo(finalResult, arrayStartingIndex);
            return;
        }

        if (HasTwoChildren(subtreeRoot))
        {
            activeTasks.Add(Task.Run(async () => await ParallelPassThrough(subtreeRoot.FirstChild!, threads / 2, arrayStartingIndex)));
            activeTasks.Add(Task.Run(async () => await ParallelPassThrough(subtreeRoot.SecondChild!,
                threads - threads / 2, arrayStartingIndex + subtreeRoot.FirstChild!.SubtreeSize + 1)));
            finalResult[arrayStartingIndex + subtreeRoot.FirstChild!.SubtreeSize] =
                (subtreeRoot.Key, subtreeRoot.Value);
            return;
        }
        
        finalResult[arrayStartingIndex] = (subtreeRoot.Key, subtreeRoot.Value);
        if (HasOneChild(subtreeRoot))
        {
            if (subtreeRoot.FirstChild is not null)
            {
                activeTasks.Add(Task.Run(async () => await ParallelPassThrough(subtreeRoot.FirstChild!, threads, arrayStartingIndex)));

                finalResult[arrayStartingIndex + subtreeRoot.FirstChild!.SubtreeSize] =
                    (subtreeRoot.Key, subtreeRoot.Value);
                return;
            }
            
            activeTasks.Add(Task.Run(async () => await ParallelPassThrough(subtreeRoot.SecondChild!, threads, arrayStartingIndex+1)));
        }
    }

    private bool HasTwoChildren(Node<TKey, TValue> node)
    {
        return (node.FirstChild is not null && node.SecondChild is not null);
    }
    
    private bool HasOneChild(Node<TKey, TValue> node)
    {
        return node.SubtreeSize > 1;
    }
}