namespace Script.Apps.InternetExplorer
{
    public interface ISearchable
    {
        public string[] SearchVariations { get; set; }
        public void ActionOnSearch();
    }
}