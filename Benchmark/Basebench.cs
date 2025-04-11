using System;
using System.Diagnostics;
using System.Drawing;
using blenderShaderGraph.Nodes.TextureNodes;

namespace blenderShaderGraph.Benchmark;

public class Basebench
{
    private readonly string name;

    protected Basebench(string name)
    {
        this.name = name;
    }

    public void Run(int length)
    {
        Stopwatch sw = new();
        List<long> times = [];

        for (int i = 0; i < length; i++)
        {
            sw.Restart();
            FuncToTest();
            sw.Stop();
            times.Add(sw.ElapsedMilliseconds);
        }
        
        long[] arr = times.ToArray();
        Array.Sort(arr);
        string value = "\n-------------" + name + "-------------";
        string end = "";
        for (int i = 0; i < value.Length; i++)
        {
            end += '-';
        }
        System.Console.WriteLine(value);
        System.Console.WriteLine("Lowest Time: " + arr[0]);
        System.Console.WriteLine("Avg. Time: " + times.Average());
        Array.Reverse(arr);
        System.Console.WriteLine("Highest Time: " + arr[0]);
        System.Console.WriteLine(end + "\n");
    }

    protected virtual void FuncToTest()
    {
        return;
    }
}
