namespace XmlTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tasks.LabCreate();
            Console.ReadKey();
            Tasks.LabAnalyze();
            Console.ReadKey();
            Tasks.LabEdit();
        }
    }
}