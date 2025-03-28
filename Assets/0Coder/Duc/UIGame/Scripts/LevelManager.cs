using System.Collections.Generic;
using DesignPattern;
using UnityEngine;
namespace UIGame
{
    public class LevelManager : Singleton<LevelManager>
    {
        private const string LevelKey = "Level_";

        public static LevelManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public int GetLevelStatus(int levelId)
        {
            return PlayerPrefs.GetInt(LevelKey + levelId, levelId == 1 ? 1 : 0);
        }

        public void UnlockLevels(List<int> levelIds)
        {
            foreach (int id in levelIds)
            {
                PlayerPrefs.SetInt(LevelKey + id, 1);
            }
            PlayerPrefs.Save();
        }
        public void CompleteLevel(int levelId, TypePath pathType, List<int> nextLevelIds)
        {
            PlayerPrefs.SetInt(LevelKey + levelId, 2); 
            if (pathType == TypePath.ThreePath)
            {
                UnlockLevels(nextLevelIds);
            }
            else if (pathType == TypePath.TwoPath)
            {
                UnlockLevels(nextLevelIds);
            }
            else
            {
                UnlockLevels(new List<int> { nextLevelIds[0] });
            }
            PlayerPrefs.Save();
        }
    }
}