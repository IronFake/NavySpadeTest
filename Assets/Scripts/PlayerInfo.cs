using System;

namespace DefaultNamespace
{
    public class PlayerInfo
    {
        private int _maxLives;
        
        public int Lives { get; private set; }

        public int Score { get; private set; }
        
        public int MaxScore { get; private set; }

        public event Action<int> OnLivesChanged;
        public event Action<int> OnScoreChanged;
        public event Action OnLivesIsOver;
        
        public PlayerInfo(int maxLives, int maxScore)
        {
            _maxLives = maxLives;
            Lives = maxLives;
            MaxScore = maxScore;
        }

        public void Reset()
        {
            Lives = _maxLives;
            Score = 0;
            OnLivesChanged?.Invoke(Lives);
            OnScoreChanged?.Invoke(Score);
        }

        public void AddScore(int value)
        {
            Score += value;
            OnScoreChanged?.Invoke(Score);

            if (Score > MaxScore)
            {
                MaxScore = Score;
            }
        }

        public void AddLives(int value)
        {
            int newLivesValue = Lives + value;
            if(newLivesValue > _maxLives)
                return;
            
            Lives += value;
            OnLivesChanged?.Invoke(Lives);
        }

        public void ReduceLives(int value)
        {
            Lives -= value;
            OnLivesChanged?.Invoke(Lives);

            if (Lives <= 0)
            {
                OnLivesIsOver?.Invoke();
            }
        }
    }
}