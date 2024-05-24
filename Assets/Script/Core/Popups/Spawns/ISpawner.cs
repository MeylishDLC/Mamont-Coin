namespace Script.Core.Popups.Spawns
{
    public interface ISpawner
    {
        public bool SpawnActive { get; set; }
        public int FrequencyMilliseconds { get; set; }
        public void StartSpawn();
        public void StopSpawn();
    }
}