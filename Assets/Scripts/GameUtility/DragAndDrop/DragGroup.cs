using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GoogleTrends.GameUtility.DragAndDrop
{
    public class DragGroup : MonoBehaviour
    {
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = (RectTransform)transform;
                }

                return _rectTransform;
            }
        }

        public int ChildCount => _dragElements.Count;
        
        [SerializeField]
        private ChangePositionEvent _changePositionEvent;
        
        private RectTransform _rectTransform;
        private readonly HashSet<TermDrag> _dragElements = new HashSet<TermDrag>();

        public void RegisterDragElement(TermDrag drag)
        {
            _dragElements.Add(drag);
        }

        public void UnRegisterDragElement(TermDrag drag)
        {
            _dragElements.Remove(drag);
        }

        public void OnChangePosition(int currentIndex, int previousIndex)
        {
            _changePositionEvent?.Invoke(currentIndex, previousIndex);
        }
        
        [System.Serializable]
        public class ChangePositionEvent : UnityEvent<int, int>
        {
        }
    }
}