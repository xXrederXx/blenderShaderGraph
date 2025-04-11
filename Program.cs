using blenderShaderGraph.Util;

while (true)
{
    System.Console.WriteLine("Run");
    GraphRunner.Run("./graph.json");
    Console.ReadKey();
}
