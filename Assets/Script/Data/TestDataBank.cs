using System.IO;
using UnityEngine;

namespace Script.Data
{
    public class TestDataBank: IDataBank
    {
        private long clicks;
        public long Clicks
        {
            get => clicks;
            set
            {
                clicks = value;
                if (clicks < 0)
                {
                    clicks = 0;
                }
            }
        }

        private long multiplier = 1;
        public long Multiplier
        {
            get => multiplier;
            set
            {
                multiplier = value;
                if (multiplier < 0)
                {
                    multiplier = 0;
                }
            }
        }
        private string path => Path.Combine(Application.dataPath, "save.json");
            
        public void SaveData()
        {
            var json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        public bool LoadData()
        {
            if (!File.Exists(path))
            {
                return false;
            }
            var json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
            return true;
        }
    }
}