using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleTrends.Teams;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using UnityEngine;

namespace GoogleTrends.GameManagers
{
    public class RoundBonusesApplier : MonoBehaviour
    {
        public float initialWait = 2f;

        public float multiplierAnimationWait = 1.3f;

        public float waitBetweenBonuses = 1.5f;
        
        public float waitAfterBonusReveal = 2f;

        public float bonusAnimationWait = 1.3f;

        public Animation bonuseRevealObject;

        public async Task StartApplyRoundBonuses(List<IReference> teams, IReference currentTerm)
        {
            await Task.Delay(TimeSpan.FromSeconds(initialWait));

            var multiplier = currentTerm.GetValue<int>(DetailNames.Multiplier);
            if (multiplier > 1)
            {
                await PlayMultiplierAnimations(multiplier);
                foreach (var team in teams)
                {
                    var roundScore = team.GetValue<int>(DetailNames.RoundScore);
                    roundScore *= multiplier;
                    
                    team.SetValue(DetailNames.FinalRoundScore, roundScore);
                }

                await Task.Delay(TimeSpan.FromSeconds(waitBetweenBonuses));
            }

            var bonusTerm = currentTerm.GetValue<string>(DetailNames.BonusTerm);
            var bonusPoints = currentTerm.GetValue<int>(DetailNames.BonusTermPoints);

            if (!string.IsNullOrEmpty(bonusTerm) && bonusPoints > 0)
            {
                bonusTerm = bonusTerm.ToLower();
                
                bonuseRevealObject.Play();
                await Task.Delay(TimeSpan.FromSeconds(waitAfterBonusReveal));
                
                foreach (var team in teams)
                {
                    var teamTerm = team.GetValue<string>(DetailNames.CurrentTerm);
                    if (teamTerm.ToLower().Contains(bonusTerm))
                    {
                        await PlayBonusAnimations(team, bonusPoints);
                        var roundScore = team.GetMutable<int>(DetailNames.FinalRoundScore);
                        roundScore.SetValue(roundScore.GetValue() + bonusPoints);
                        team.SetValue(DetailNames.GotBonusTerm, true);

                        //Increase the bar size to the max size.
                        var baseRoundScore = team.GetMutable<int>(DetailNames.RoundScore);
                        if (baseRoundScore.GetValue() < 100)
                        {
                            baseRoundScore.SetValue(100);
                        }
                    }
                }
            }
        }

        public void ResetBonusReveal()
        {
            bonuseRevealObject.GetComponent<CanvasGroup>().alpha = 0;
        }

        private async Task PlayMultiplierAnimations(int multiplier)
        {
            foreach (var roundScoreTeam in GetComponentsInChildren<RoundScoreTeam>())
            {
                roundScoreTeam.PlayMultiplier(multiplier);
            }

            await Task.Delay(TimeSpan.FromSeconds(multiplierAnimationWait));
        }
        
        private async Task PlayBonusAnimations(IReference team, int bonus)
        {
            foreach (var roundScoreTeam in GetComponentsInChildren<RoundScoreTeam>())
            {
                if (roundScoreTeam.IsTeam(team.GetDetail<string>(DetailNames.Name)))
                {
                    roundScoreTeam.PlayBonus(bonus);    
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(bonusAnimationWait));
        }
    }
}