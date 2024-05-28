namespace Script.Apps.SmallStuff.AppsOnWorkspace.InternetExplorer
{
    public interface ISearchable
    {
        public string[] SearchVariations { get; set; }
        public void ActionOnSearch();
    }
}