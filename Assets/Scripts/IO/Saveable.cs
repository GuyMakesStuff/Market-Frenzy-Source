using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.IO
{
    [System.Serializable]
    public class Saveable
    {
        public string FileName;

        public void Save()
        {
            SaveSystem.SaveObject(this, FileName, Application.persistentDataPath);
        }

        public void Clear()
        {
            SaveSystem.DeleteObject(FileName, Application.persistentDataPath);
        }
    }
}