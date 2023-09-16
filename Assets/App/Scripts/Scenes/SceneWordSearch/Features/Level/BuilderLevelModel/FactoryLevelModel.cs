using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            List<char> listChars = new List<char>();
            Dictionary<char, int> generalWord = toDictionary(listChars);
            
            foreach (var word in words)
            {
                List<char> letters = word.ToList();
                Dictionary<char, int> nextWord = toDictionary(letters);
                Concat(generalWord, nextWord);
            }

            listChars = ToListChar(generalWord);
            return listChars;
        }
        
        private Dictionary<char, int> toDictionary(List<char> word)
        {
            Dictionary<char, int> result = new Dictionary<char, int>();
            foreach(var letter in word)
            {
                if (result.TryGetValue(letter, out var number))
                    result[letter] = ++number;
                else
                    result.Add(letter, 1);
            }
            return result;
        }
        
        private void Concat(Dictionary<char, int> main, Dictionary<char, int> adit)
        {
            foreach(var pair in adit)
            {
                char letter = pair.Key;
                if(main.TryGetValue(letter, out var number))
                {
                    int max = Math.Max(number, pair.Value);
                    main[letter] = max;
                }
                else
                    main.Add(letter, pair.Value);
            }
        }

        private List<char> ToListChar(Dictionary<char, int> word)
        {
            List<char> result = new List<char>();
            foreach (var pair in word)
                for (int i = 0; i < pair.Value; i++)
                    result.Add(pair.Key);

            return result;
        }
    }
}