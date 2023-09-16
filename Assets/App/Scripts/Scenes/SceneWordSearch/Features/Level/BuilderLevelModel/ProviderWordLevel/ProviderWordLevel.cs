using System;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private string _levelInfoPath = "WordSearch/Levels/";
        
        public LevelInfo LoadLevelData(int levelIndex)
        {
            TextAsset levelInfoTextAsset = (TextAsset)Resources.Load(_levelInfoPath + levelIndex);
            string levelInfoText = levelInfoTextAsset.text;
            LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(levelInfoText);
            return levelInfo;
        }
    }
}