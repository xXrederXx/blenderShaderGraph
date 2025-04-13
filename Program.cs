using blenderShaderGraph.Util;

string content = "";
string newContent = "";
string fp = "./graph.sg.json";
while (true)
{
    content = newContent;

    System.Console.WriteLine("Run");
    try
    {
        GraphRunner.Run(fp);
    }
    catch (Exception err)
    {
        System.Console.WriteLine(err.Message + err.StackTrace);
    }
    while (content == newContent)
    {
        Thread.Sleep(500); // Ajust as needed
        StreamReader x = File.OpenText(fp);
        newContent = x.ReadToEnd();
        x.Close();
    }
}
