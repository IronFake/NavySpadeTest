using UnityEngine;

namespace DefaultNamespace
{
    public class Saver
    {
        private const string MAX_SCORE_KEY = "MAX_SCORE_KEY";
        
        public int GetSavedMaxScore()
        {
            return PlayerPrefs.GetInt(MAX_SCORE_KEY, 0);
        }

        public void SaveMaxScore(int value)
        {
            PlayerPrefs.SetInt(MAX_SCORE_KEY, value);
        }
    }
}