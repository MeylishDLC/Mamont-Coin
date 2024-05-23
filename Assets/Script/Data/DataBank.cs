namespace Script.Data
{
    public class DataBank: IDataBank
    {
        public long Clicks { get; set; }
        public long Multiplier { get; set; } = 1;
    }
}