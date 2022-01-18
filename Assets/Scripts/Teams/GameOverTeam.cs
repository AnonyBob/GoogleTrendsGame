using GoogleTrends.GameManagers;
using GoogleTrends.GameUtility;
using OwlAndJackalope.UX.Runtime.Modules;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class GameOverTeam : MonoBehaviour
    {
        public int Place => GetComponent<ReferenceModule>().Reference.GetDetail<int>(DetailNames.Place).GetValue();
        
        [SerializeField]
        private BarSizeRollUpBinder _barRollUp;
        
        [SerializeField]
        private IntegerRollUpBinder _rollUp;

        [SerializeField]
        private Animation _placeAnimation;

        [SerializeField]
        private AudioClip _firstPlace;

        [SerializeField]
        private AudioClip _secondPlace;

        [SerializeField]
        private AudioClip _otherPlace;

        public void SetMaxScore(int maxScore)
        {
            _barRollUp.maxNumberSize = maxScore;
        }
        
        public void StartPointRollUp()
        {
            _barRollUp.pauseFill = false;
            _rollUp.pauseFill = false;

            var place = Place;
            if (place == 0)
            {
                SoundManager.PlaySound(_firstPlace);
            }
            else if (place <= 2)
            {
                SoundManager.PlaySound(_secondPlace);
            }
            else
            {
                
            }
        }

        public void ShowPlace()
        {
            _placeAnimation.Play();
        }
    }
}