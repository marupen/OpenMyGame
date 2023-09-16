using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private const string WordsListFileName = "Fillwords/words_list";
        private const string LevelsPackFileName = "Fillwords/pack_0";

        private LevelData[] _levelDatas;
        private string[] _wordPack;

        public ProviderFillwordLevel()
        {
            _levelDatas = GetLevelDatas();
            _wordPack = GetWordPack();
        }
        
        public GridFillWords LoadModel(int index)
        {
            index--;
            
            if (!_levelDatas.TryGetElement(index, out var levelData))
                return null;
            
            int size = LevelDatasValidateAndCalculateSize(levelData);
            if (size > 0)
            {
                GridFillWords gridFillWords = new GridFillWords(new Vector2Int(size, size));
                for (int i = 0; i < _levelDatas[index].NumberOfWords; i++)
                {
                    if (!_wordPack.TryGetElement(_levelDatas[index].IndexesOfWords[i], out var word))
                        return null;
                    
                    char[] letters = word.ToCharArray();
                    for (int j = 0; j < letters.Length - 1; j++)
                    {
                        gridFillWords.Set(GetX(_levelDatas[index].CoordinatesOfLetters[i][j], size),
                                          GetY(_levelDatas[index].CoordinatesOfLetters[i][j], size),
                                          new CharGridModel(letters[j]));
                    }
                }
                return gridFillWords;
            }

            return null;
        }

        private LevelData[] GetLevelDatas()
        {
            TextAsset wordsListTextAsset = Resources.Load<TextAsset>(LevelsPackFileName);
            string wordListText = wordsListTextAsset.text;
            string[] wordListStrings = wordListText.Split(new char[] {'\n'});
            
            LevelData[] levelDatas = new LevelData[wordListStrings.Length];
            for (int i = 0; i < wordListStrings.Length; i++)
            {
                levelDatas[i] = new LevelData(wordListStrings[i]);
            }
            return levelDatas;
        }

        private string[] GetWordPack()
        {
            TextAsset levelsPackTextAsset = (TextAsset)Resources.Load(WordsListFileName);
            string levelsPackText = levelsPackTextAsset.text;
            string[] words = levelsPackText.Split(new char[] {'\n'});
            return words;
        }

        // Метод проверки корректности данных уровня
        private int LevelDatasValidateAndCalculateSize(LevelData levelData)
        {
            // Проверка, есть ли на уровне хоть одно слово
            if (levelData.NumberOfWords == 0)
                return 0;
            
            // Выгрузка данных в список
            List<int> coordinates = new List<int>();
            foreach (int[] wordPosition in levelData.CoordinatesOfLetters)
            {
                foreach (int letterPosition in wordPosition)
                {
                    coordinates.Add(letterPosition);
                }
            }

            // Проверка, является ли таблица квадратной
            int size = (int) Math.Sqrt(coordinates.Count);
            if (size * size != coordinates.Count)
                return 0;
            
            // Проверка, являются ли номера порядковыми
            coordinates.Sort();
            for (int i = 0; i < coordinates.Count; i++)
            {
                if (coordinates[i] != i)
                    return 0;
            }

            // Проверка, корректно ли указана длина слов на уровне
            for (int i = 0; i < levelData.NumberOfWords; i++)
            {
                if (_wordPack[levelData.IndexesOfWords[i]].Length - 1 != levelData.LengthOfWords[i])
                    return 0;
            }

            return size;
        }

        private int GetY(int coordinate, int size)
        {
            return coordinate % size;
        }
        
        private int GetX(int coordinate, int size)
        {
            return coordinate / size;
        }
    }
    
    public static class ArraySafe
    {
        public static bool TryGetElement<T>(this T[] array, int index, out T element)
        {
            if(array == null || index < 0 || index >= array.Length)
            {
                element = default(T);
                return false;
            }

            element = array[index];
            return true;
        }
    }
}