using FileParserService.Services;
using FileParserService.Services.Interfaces;

public class Program
{
    static void Main(string[] args)
    {
        IProcessor fileProcessor = new Processor();
        HashSet<string> processedFiles = new HashSet<string>();
        string folderPath = "Xml"; 

        Thread fileProcessingThread = new Thread(() =>
        {
            while (true)
            {
                string[] filePaths = Directory.GetFiles(folderPath, "*.xml"); 

                foreach (var filePath in filePaths)
                {
                    if (!processedFiles.Contains(filePath)) 
                    {
                        Console.WriteLine($"Processing file: {filePath}");
                        fileProcessor.ProcessFile(filePath);
                        processedFiles.Add(filePath); 
                    }
                }
                Thread.Sleep(1000); 
            }
        });

        fileProcessingThread.Start();
        fileProcessingThread.Join();

    }
}
