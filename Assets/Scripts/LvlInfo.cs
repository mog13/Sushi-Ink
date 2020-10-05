using System;

namespace DefaultNamespace
{
    [Serializable]
    public struct LvlInfo
    {
        public string sceneName;
        public string levelName;
        public int levelNo;
        public int AllowedLeaves;
        public float[] goals;
        public float timeToComplete;
        public Recipe[] Recipes;
        public bool unlocked;
    }
}