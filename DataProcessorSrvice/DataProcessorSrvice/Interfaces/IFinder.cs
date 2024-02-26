namespace DataProcessorService.Interfaces
{
    public interface IFinder
    {
        public List<string> FindAllData(string message, string dataToFind);
        public List<string> FindAllModuleCategoryIds(string message);
    }
}
