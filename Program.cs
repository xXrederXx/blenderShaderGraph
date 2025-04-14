using blenderShaderGraph.Util;

string content = "";
string newContent = "";
string fp = "./Examples/sg/tile01.sg.json";
while (true)
{
    content = newContent;

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
