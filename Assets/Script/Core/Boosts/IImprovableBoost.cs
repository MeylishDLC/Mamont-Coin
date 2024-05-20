namespace Script.Core.Boosts
{
    public interface IImprovableBoost
    {
        public string ImproveText { get; set; }
        public void Improve();
    }
}