using System.Diagnostics;
using CourseWork.Structures;

namespace CourseWork;

public static class Program
{
    public static async Task Main()
    {
        int size = 10000000;
        List<(int, int)> src = new List<(int, int)>(size);
        for (int i = size-1; i >= 0; i--)
        {
            src.Add((i, size-i));
        }

        Tree<int, int> tree = new Tree<int, int>(src);
        Stopwatch sw = Stopwatch.StartNew();
        var arr = await tree.ToSortedArrayConcurrent();
        sw.Stop();
        Console.WriteLine("Time: " + sw.ElapsedMilliseconds + "ms");
        
        tree.ClearNodesUsages();
        sw = Stopwatch.StartNew();
        arr = await tree.ToSortedArrayParallel();
        sw.Stop();
        Console.WriteLine("Time: " + sw.ElapsedMilliseconds + "ms");
        // foreach (var pair in arr)
        // {
        //     Console.WriteLine(pair.Item1 + " : " + pair.Item2);
        // }
        
    }
}