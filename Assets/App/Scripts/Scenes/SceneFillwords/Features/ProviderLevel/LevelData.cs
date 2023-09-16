using System;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class LevelData
    {
        public LevelData(string levelPackText)
        {
            string[] samples = levelPackText.Split(new char[] {' '});
            if (samples.Length == 0 || samples.Length % 2 != 0)
            {
                NumberOfWords = 0;
                IndexesOfWords = null;
                LengthOfWords = null;
                CoordinatesOfLetters = null;
                return;
            }

            NumberOfWords = samples.Length / 2;
            IndexesOfWords = new int[NumberOfWords];
            LengthOfWords = new int[NumberOfWords];
            CoordinatesOfLetters = new int[NumberOfWords][];
            for (int i = 0; i < NumberOfWords; i++)
            {
                if (Int32.TryParse(samples[i * 2], out int numValue1))
                    IndexesOfWords[i] = numValue1;
                else
                {
                    NumberOfWords = 0;
                    IndexesOfWords = null;
                    LengthOfWords = null;
                    CoordinatesOfLetters = null;
                    return;
                }

                string[] coordinatesOfLetters = samples[i * 2 + 1].Split(new char[] {';'});
                LengthOfWords[i] = coordinatesOfLetters.Length;
                CoordinatesOfLetters[i] = new int[LengthOfWords[i]];
                for (int j = 0; j < LengthOfWords[i]; j++)
                {
                    if (Int32.TryParse(coordinatesOfLetters[j], out int numValue2))
                        CoordinatesOfLetters[i][j] = numValue2;
                    else
                    {
                        NumberOfWords = 0;
                        IndexesOfWords = null;
                        LengthOfWords = null;
                        CoordinatesOfLetters = null;
                        return;
                    }
                }
            }
        }
        
        public int NumberOfWords { get; }
        public int[] IndexesOfWords { get; }
        public int[] LengthOfWords { get; }
        public int[][] CoordinatesOfLetters { get; }
    }
}