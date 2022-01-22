using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Modules;
using TMPro;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class RoundScoreTeam : MonoBehaviour
    {
        private ReferenceModule _referenceModule;
        
        public Animation extraAnimation;
        public CanvasGroup bonusGroup;
        public CanvasGroup multiplierGroup;

        private void Start()
        {
            _referenceModule = GetComponent<ReferenceModule>();
        }
        
        public bool IsTeam(IDetail<string> teamName)
        {
            var localName = _referenceModule.Reference.GetDetail<string>(DetailNames.Name);
            return localName == teamName;
        }
        
        public void PlayMultiplier(int multiplier)
        {
            _referenceModule.Reference.SetValue(DetailNames.Multiplier, multiplier);
            
            bonusGroup.alpha = 0;
            multiplierGroup.alpha = 1;
            extraAnimation.Play();
        }

        public void PlayBonus(int bonus)
        {
            _referenceModule.Reference.SetValue(DetailNames.BonusTermPoints, bonus);
            
            bonusGroup.alpha = 1;
            multiplierGroup.alpha = 0;
            extraAnimation.Play();
        }
    }
}