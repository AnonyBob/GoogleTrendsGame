using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using GoogleTrends.SendScores;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Modules;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.Round
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField]
        private ReferenceModule _gameReference;

        private readonly ScoresProcessor _scoresProcessor = new ScoresProcessor();
        
        private DetailObserver<List<IReference>> _teams;
        private MutableDetailObserver<bool> _waitingForScores;
        private MutableDetailObserver<GameState> _gameState;
        private MutableDetailObserver<int> _roundNumber;
        private MutableDetailObserver<string> _currentTerm;
        private DetailObserver<List<string>> _gameTerms;

        private MutableDetailObserver<int> _timer;
        private MutableDetailObserver<bool> _waitingForTimer;
        private DetailObserver<int> _timerMax;

        private Coroutine _timerRoutine;

        private void Start()
        {
            _teams = new DetailObserver<List<IReference>>() { DetailName = DetailNames.Teams };
            _teams.Initialize(_gameReference.Reference);

            _gameState = new MutableDetailObserver<GameState>() { DetailName = DetailNames.GameState };
            _gameState.Initialize(_gameReference.Reference);
            
            _currentTerm = new MutableDetailObserver<string>() { DetailName = DetailNames.CurrentTerm };
            _currentTerm.Initialize(_gameReference.Reference);
            
            _gameTerms = new DetailObserver<List<string>>() { DetailName = DetailNames.Terms };
            _gameTerms.Initialize(_gameReference.Reference);
            
            _roundNumber = new MutableDetailObserver<int>() { DetailName = DetailNames.RoundNumber };
            _roundNumber.Initialize(_gameReference.Reference);

            _waitingForScores = new MutableDetailObserver<bool>() { DetailName = DetailNames.WaitingForScores};
            _waitingForScores.Initialize(_gameReference.Reference);

            _timer = new MutableDetailObserver<int>() { DetailName = DetailNames.Timer };
            _timer.Initialize(_gameReference.Reference);
            
            _waitingForTimer = new MutableDetailObserver<bool>() { DetailName = DetailNames.WaitingForTimer };
            _waitingForTimer.Initialize(_gameReference.Reference);

            _timerMax = new DetailObserver<int>() { DetailName = DetailNames.TimerMax };
            _timerMax.Initialize(_gameReference.Reference);
        }
        
        public void SubmitTerms()
        {
            StartCoroutine(DoSubmitTerms());
        }

        public void ViewResults()
        {
            Application.OpenURL(CreateURL());
        }
        
        public void NextRound()
        {
            var currentTerm = _currentTerm.Value;
            foreach (var team in _teams.Value)
            {
                
                if (_roundNumber.Value == 1)
                {
                    SetTeamName(team, currentTerm);    
                }
                
                UpdateScores(team);
                ClearTerms(team);
            }
            
            MoveToNextRoundOrEndGame();
        }

        public void StartTimer()
        {
            _timer.Value = _timerMax.Value;
            if (_timerRoutine != null)
            {
                StopCoroutine(_timerRoutine);    
            }
            
            _waitingForTimer.Value = true;
            _timerRoutine = StartCoroutine(TimerRoutine());
        }

        public void StopTimer()
        {
            _waitingForTimer.Value = false;
            _timer.Value = 0;
            if (_timerRoutine != null)
            {
                StopCoroutine(_timerRoutine);    
            }
        }

        private IEnumerator DoSubmitTerms()
        {
            var retriesRemaining = 3;
            _waitingForScores.Value = true;

            yield return _scoresProcessor.SendTerms("today 12-m", "US", GetTermsToSend());
            while (!string.IsNullOrEmpty(_scoresProcessor.Error) && retriesRemaining > 0)
            {
                retriesRemaining--;
                yield return _scoresProcessor.SendTerms("today 12-m", "US", GetTermsToSend());
            }

            if (!string.IsNullOrEmpty(_scoresProcessor.Error))
            {
                //TODO: Return to the main menu....
            }
            else
            {
                ShowResults();
            }
            
            _waitingForScores.Value = false;
        }

        private IEnumerator TimerRoutine()
        {
            while (_timer.Value > 0)
            {
                yield return new WaitForSeconds(1);
                _timer.Value--;
            }

            _timerRoutine = null;
            _waitingForTimer.Value = false;
        }

        private IEnumerable<string> GetTermsToSend()
        {
            return _teams.Value
                .Select(team => team.GetDetail<string>(DetailNames.CurrentTerm)
                    .GetValue());
        }

        private void ShowResults()
        {
            for (var i = 0; i < _scoresProcessor.Results.Count; ++i)
            {
                if (_teams.Value != null && _teams.Value.Count > i)
                {
                    var roundScore = _scoresProcessor.Results[i];
                    _teams.Value[i].GetMutable<int>(DetailNames.RoundScore).SetValue(roundScore);
                }
            }

            _gameState.Value = GameState.ShowRoundResults;
        }

        private void MoveToNextRoundOrEndGame()
        {
            var nextIndex = _roundNumber.Value;
            _roundNumber.Value++;
            if (_gameTerms.Value.Count <= nextIndex)
            {
                //End Game.
                _gameState.Value = GameState.ShowGameResults;
            }
            else
            {
                //Setup the next term.
                _currentTerm.Value = _gameTerms.Value[nextIndex];
                _timer.Value = _timerMax.Value;
                
                _gameState.Value = GameState.ShowTerm;
            }
        }
        
        private void SetTeamName(IReference team, string currentTerm)
        {
            var teamName = team.GetMutable<string>(DetailNames.Name);
            var teamTerm = team.GetDetail<string>(DetailNames.CurrentTerm);

            var newName = teamTerm.GetValue().Replace(currentTerm, "").Trim();
            teamName.SetValue($"Team {newName}");
        }

        private string CreateURL()
        {
            var baseString = "https://trends.google.com/trends/explore?geo=US&q=";
            var terms = _teams.Value
                .Select(t =>
                {
                    var value = t.GetDetail<string>(DetailNames.CurrentTerm).GetValue();
                    return value.Trim().Replace(" ", "%20");
                });

            return baseString + string.Join(",", terms);
        }

        private void UpdateScores(IReference team)
        {
            var score = team.GetMutable<int>(DetailNames.Score);
            var roundScore = team.GetMutable<int>(DetailNames.RoundScore);

            score.SetValue(score.GetValue() + roundScore.GetValue());
            roundScore.SetValue(0);
        }

        private void ClearTerms(IReference team)
        {
            var teamTerm = team.GetMutable<string>(DetailNames.CurrentTerm);
            var previousTerms = team.GetMutableCollection<string>(DetailNames.Terms);
            previousTerms.Add(teamTerm.GetValue());
            
            teamTerm.SetValue(string.Empty);
        }
    }
}