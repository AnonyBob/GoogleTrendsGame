using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.Teams
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private ReferenceModule _gameReference;

        [SerializeField]
        private float _initialWait = 0.5f;

        [SerializeField]
        private float _waitBeforeShowPlace = 0.8f;
        
        [SerializeField]
        private float _waitBetweenTeams = 1.2f;

        [SerializeField]
        private float _endWait = 2f;
        
        [SerializeField]
        private Button _continueButton;

        private void OnEnable()
        {
            _continueButton.gameObject.SetActive(false);
            StartCoroutine(EndGameSequence());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private GameOverTeam[] SetupTeams()
        {
            var maxScore = GetMaxScore();
            var teams = GetComponentsInChildren<GameOverTeam>();
            foreach (var team in teams)
            {
                team.SetMaxScore(maxScore);
            }

            return teams;
        }

        private IEnumerator EndGameSequence()
        {
            yield return new WaitForSeconds(_initialWait);
            var teams = SetupTeams();

            foreach (var team in teams.OrderByDescending(t => t.Place))
            {
                team.StartPointRollUp();
                yield return new WaitForSeconds(_waitBeforeShowPlace);
                team.ShowPlace();

                yield return new WaitForSeconds(_waitBetweenTeams);
            }

            yield return new WaitForSeconds(_endWait);
            _continueButton.gameObject.SetActive(true);
        }

        private int GetMaxScore()
        {
            var maxScore = 0;
            var teams = _gameReference.Reference.GetDetail<List<IReference>>(DetailNames.Teams).GetValue();
            foreach (var team in teams)
            {
                var score = team.GetDetail<int>(DetailNames.Score).GetValue();
                if (score > maxScore)
                {
                    maxScore = score;
                }
            }

            return maxScore;
        }
    }
}