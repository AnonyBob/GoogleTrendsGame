using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GoogleTrends.GameUtility.DragAndDrop
{
    public class TermDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField]
        private RectTransform _visualRoot;

        [SerializeField]
        private float _draggingRotation = 15;

        [SerializeField]
        private float _elementSize = 40;

        private DragGroup _dragGroup;
        private Vector2 _offset;

        private void Start()
        {
            _dragGroup = GetComponentInParent<DragGroup>();
            if (_dragGroup != null)
            {
                _dragGroup.RegisterDragElement(this);
            }
        }

        private void OnDestroy()
        {
            if (_dragGroup != null)
            {
                _dragGroup.UnRegisterDragElement(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _visualRoot.SetParent(_dragGroup.transform);
            _visualRoot.localRotation = Quaternion.Euler(0, 0, _draggingRotation);
            
            var mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragGroup.RectTransform, mousePosition,
                Camera.main, out var initialPoint);

            _offset = initialPoint - (Vector2)_visualRoot.localPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _visualRoot.SetParent(transform);
            _visualRoot.anchoredPosition = Vector2.zero;
            _visualRoot.localRotation = Quaternion.identity;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragGroup.RectTransform, mousePosition,
                Camera.main, out var point);
            _visualRoot.localPosition = point - _offset;

            var changeInPosition = _visualRoot.localPosition - transform.localPosition;
            if (Mathf.Abs(changeInPosition.y) > _elementSize)
            {
                var currentIndex = transform.GetSiblingIndex();
                var nextIndex = Mathf.Sign(changeInPosition.y) > 0 ? currentIndex - 1 : currentIndex + 1;
                if (nextIndex < 0 || nextIndex >= _dragGroup.ChildCount)
                {
                    return;
                }
                
                transform.SetSiblingIndex(nextIndex);
                _dragGroup.OnChangePosition(currentIndex, nextIndex);
            }
        }
    }
}