using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelDictionary : MonoBehaviour
    {
        public static LevelDictionary Instance;
        public List<LvlInfo> levels;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
      
        }

        public void Unlock(int n)
        {
            if (n < levels.Count)
            {
                LvlInfo lvl = levels[n];
                lvl.unlocked = true;
                levels[n] = lvl;
            }
        }
    }
}