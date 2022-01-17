using GoogleTrends.GameUtility;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class GameOverTeam : MonoBehaviour
    {
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