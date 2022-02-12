using GoogleTrends.GameUtility.Selection;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class CloseTeamPopup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private SelectionGroup _selectionGroup;
        
        public void OnSelectionChanged()
        {
            if (_selectionGroup.FirstSelection != null)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
        }
    }
}