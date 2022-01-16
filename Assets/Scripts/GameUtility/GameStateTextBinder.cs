using System;
using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameStateTextBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(GameState))]
        private DetailObserver<GameState> _gameStateDetail;

        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _gameStateDetail.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void HandleChange()
        {
            _text.text = GetGameStateAsString(_gameStateDetail.Value);
        }

        private string GetGameStateAsString(GameState value)
        {
            switch (value)
            {
                case GameState.MainMenu:
                    return "Main Menu";
                case GameState.GameSetup:
                    return "Setup";
                case GameState.ShowTerm:
                    return "Reveal Term";
                case GameState.ShowRoundResults:
                    return "Compare";
                case GameState.ShowGameResults:
                    return "Game Over";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _gameStateDetail;
        }
    }
}