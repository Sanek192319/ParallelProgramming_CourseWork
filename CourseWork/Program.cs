using System.Diagnostics;
using CourseWork.Structures;

namespace CourseWork;

public static class Program
{
    public static async Task Main()
    {
        int size = 100000000;
        List<(int, int)> src = new List<(int, int)>(size);
        for (int i = size-1; i >= 0; i--)
        {
            src.Add((i, size-i));
        }

        Tree<int, int> tree = new Tree<int, int>(src);
        Stopwatch sw = Stopwatch.StartNew();
        var arr1 = await tree.ToSortedArrayConcurrent();
        sw.Stop();
        Console.WriteLine("Time: " + sw.ElapsedMilliseconds + "ms");
        
        tree.ClearNodesUsages();
        sw = Stopwatch.StartNew();
        var arr2 = await tree.ToSortedArrayParallel();
        sw.Stop();
        Console.WriteLine("Time: " + sw.ElapsedMilliseconds + "ms");

        Console.WriteLine("Correctness: "+Check(arr1, arr2));
    }

    private static bool Check((int, int)[] arr1, (int, int)[] arr2)
    {
        for (int i = 0; i < arr1.Length; i++)
        {
            var el1 = arr1[i];
            var el2 = arr2[i];
            if (arr1[i] != arr2[i])
                return false;
        }
        
        return true;
    }
}