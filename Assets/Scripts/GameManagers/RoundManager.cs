using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoogleTrends.SendScores;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Modules;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.GameManagers
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField]
        private ReferenceModule _gameReference;

        [SerializeField]
        private float _bonusTermWaitTime;
        
        private readonly ScoresProcessor _scoresProcessor = new ScoresProcessor();
        
        private DetailObserver<List<IReference>> _teams;
        private MutableDetailObserver<bool> _waitingForScores;
        private MutableDetailObserver<GameState> _gameState;
        private MutableDetailObserver<int> _roundNumber;
        private MutableDetailObserver<IReference> _currentTerm;
        private DetailObserver<List<IReference>> _gameTerms;
        private DetailObserver<bool> _setTeamNamesOnFirstRound;

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
            
            _currentTerm = new MutableDetailObserver<IReference>() { DetailName = DetailNames.CurrentTerm };
            _currentTerm.Initialize(_gameReference.Reference);
            
            _gameTerms = new DetailObserver<List<IReference>>() { DetailName = DetailNames.Terms };
            _gameTerms.Initialize(_gameReference.Reference);
            
            _roundNumber = new MutableDetailObserver<int>() { DetailName = DetailNames.RoundNumber };
            _roundNumber.Initialize(_gameReference.Reference);

            _waitingForScores = new MutableDetailObserver<bool>() { DetailName = DetailNames.WaitingForScores};
            _waitingForScores.Initialize(_gameReference.Reference);
            
            _setTeamNamesOnFirstRound = new DetailObserver<bool>() { DetailName = DetailNames.SetTeamNamesOnFirstRound };
            _setTeamNamesOnFirstRound.Initialize(_gameReference.Reference);

            _timer = new MutableDetailObserver<int>() { DetailName = DetailNames.Timer };
            _timer.Initialize(_gameReference.Reference);
            
            _waitingForTimer = new MutableDetailObserver<bool>() { DetailName = DetailNames.WaitingForTimer };
            _waitingForTimer.Initialize(_gameReference.Reference);

            _timerMax = new DetailObserver<int>() { DetailName = DetailNames.TimerMax };
            _timerMax.Initialize(_gameReference.Reference);
        }

        private void OnDestroy()
        {
            _teams?.Dispose();
            _gameState?.Dispose();
            _currentTerm?.Dispose();
            _gameTerms?.Dispose();
            _roundNumber?.Dispose();
            _waitingForScores?.Dispose();
            _setTeamNamesOnFirstRound?.Dispose();
            _timer?.Dispose();
            _waitingForTimer?.Dispose();
            _timerMax?.Dispose();
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
                if (_roundNumber.Value == 1 && _setTeamNamesOnFirstRound.Value)
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
                yield return ApplyRoundBonuses();
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
                    //Set both here because we want to initialize the final round score before multiplying it.
                    _teams.Value[i].GetMutable<int>(DetailNames.RoundScore).SetValue(roundScore);
                    _teams.Value[i].GetMutable<int>(DetailNames.FinalRoundScore).SetValue(roundScore);
                }
            }

            _gameState.Value = GameState.ShowRoundResults;
        }

        private IEnumerator ApplyRoundBonuses()
        {
            var multiplier = _currentTerm.Value.GetDetail<int>(DetailNames.Multiplier).GetValue();
            var bonusTerm = _currentTerm.Value.GetDetail<string>(DetailNames.BonusTerm).GetValue();
            var bonusTermPoints = _currentTerm.Value.GetDetail<int>(DetailNames.BonusTermPoints).GetValue();
            
            if (multiplier > 1 || !string.IsNullOrEmpty(bonusTerm))
            {
                yield return new WaitForSeconds(_bonusTermWaitTime);
            }
            
            for (var i = 0; i < _teams.Value.Count; ++i)
            {
                var roundScoreDetail = _teams.Value[i].GetMutable<int>(DetailNames.RoundScore);
                var roundScore = roundScoreDetail.GetValue();

                if (multiplier > 1)
                {
                    roundScore = Mathf.RoundToInt(roundScore * multiplier);
                }

                if (!string.IsNullOrEmpty(bonusTerm))
                {
                    var currentTermText = _teams.Value[i].GetDetail<string>(DetailNames.CurrentTerm).GetValue();
                    if (currentTermText.ToLower().Contains(bonusTerm.ToLower()))
                    {
                        roundScore += bonusTermPoints;
                        _teams.Value[i].GetMutable<bool>(DetailNames.GotBonusTerm).SetValue(true);
                    }
                }

                _teams.Value[i].GetMutable<int>(DetailNames.FinalRoundScore).SetValue(roundScore);
            }
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
        
        private void SetTeamName(IReference team, IReference currentTerm)
        {
            var currentTermText = currentTerm.GetDetail<string>(DetailNames.TermText).GetValue();
            var teamName = team.GetMutable<string>(DetailNames.Name);
            var teamTerm = team.GetDetail<string>(DetailNames.CurrentTerm);

            var newName = teamTerm.GetValue().Replace(currentTermText, "").Trim();
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
            var roundScore = team.GetMutable<int>(DetailNames.FinalRoundScore);

            score.SetValue(score.GetValue() + roundScore.GetValue());
            roundScore.SetValue(0);
        }

        private void ClearTerms(IReference team)
        {
            var previousTerms = team.GetMutableCollection<IReference>(DetailNames.Terms);
            previousTerms.Add(new BaseReference(ConstructEnteredTerm(team)));

            team.GetMutable<string>(DetailNames.CurrentTerm).SetValue("");
            team.GetMutable<bool>(DetailNames.GotBonusTerm).SetValue(false);
            team.GetMutable<int>(DetailNames.RoundScore).SetValue(0);
            team.GetMutable<int>(DetailNames.FinalRoundScore).SetValue(0);
        }

        private IEnumerable<IDetail> ConstructEnteredTerm(IReference team)
        {
            yield return new BaseDetail<string>(DetailNames.TermText,
                team.GetDetail<string>(DetailNames.CurrentTerm).GetValue());
            
            yield return new BaseDetail<int>(DetailNames.RoundScore,
                team.GetDetail<int>(DetailNames.FinalRoundScore).GetValue());
            
            yield return new BaseDetail<bool>(DetailNames.GotBonusTerm,
                team.GetDetail<bool>(DetailNames.GotBonusTerm).GetValue());
        }
    }
}