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

        public void SetMaxScore(int maxScore)
        {
            _barRollUp.maxNumberSize = maxScore;
        }
        
        public void StartPointRollUp()
        {
            _barRollUp.pauseFill = false;
            _rollUp.pauseFill = false;
        }

        public void ShowPlace()
        {
            _placeAnimation.Play();
        }
    }
}