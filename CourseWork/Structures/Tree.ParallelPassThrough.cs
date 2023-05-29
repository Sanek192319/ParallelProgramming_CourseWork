namespace CourseWork.Structures;

public partial class Tree<TKey, TValue>
{
    public async Task<(TKey, TValue)[]> ToSortedArrayParallel(Node<TKey, TValue>? subtreeRoot = null, int threads = 8)
    {
        Node<TKey, TValue> workingTreeRoot = subtreeRoot! ?? Root!;
        if (Count == 0)
        {
            return Array.Empty<(TKey, TValue)>();
        }

        return await ParallelPassThrough(workingTreeRoot, threads);
    }

    private async Task<(TKey, TValue)[]> ParallelPassThrough(Node<TKey, TValue> subtreeRoot, int threads)
    {
        if (threads < 2)
            return await ToSortedArrayConcurrent(subtreeRoot);

        if (HasTwoChildren(subtreeRoot))
        {
            var task1 = Task.Run(async () => await ParallelPassThrough(subtreeRoot.FirstChild!, threads / 2));
            var task2 = Task.Run(async () => await ParallelPassThrough(subtreeRoot.SecondChild!, threads - threads / 2));

            var res1 = await task1;
            var res2 = await task2;
            var finRes = new (TKey, TValue)[res1.Length + 1 + res2.Length];
            res1.CopyTo(finRes, 0);
            finRes[res1.Length] = (subtreeRoot.Key, subtreeRoot.Value);
            res2.CopyTo(finRes, res1.Length+1);
            return finRes;
        }
        
        if (HasOneChild(subtreeRoot))
        {
            if (subtreeRoot.FirstChild is not null)
            {
                var task1 = Task.Run(async () => await ParallelPassThrough(subtreeRoot.FirstChild!, threads));

                var res1 = await task1;
                var finalResults = new (TKey, TValue)[res1.Length + 1];
                res1.CopyTo(finalResults, 0);
                finalResults[res1.Length] = (subtreeRoot.Key, subtreeRoot.Value);
                return finalResults;
            }
            
            var task2 = Task.Run(async () => await ParallelPassThrough(subtreeRoot.SecondChild!, threads));
            var res2 = await task2;
            var finalResult = new (TKey, TValue)[1 + res2.Length];
            finalResult[0] = (subtreeRoot.Key, subtreeRoot.Value);
            res2.CopyTo(finalResult, 1);
            return finalResult;
        }
        
        return new (TKey, TValue)[] { (subtreeRoot.Key, subtreeRoot.Value) };
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