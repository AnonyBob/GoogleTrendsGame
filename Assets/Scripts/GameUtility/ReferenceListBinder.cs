using System;
using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Modules;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;
using UnityEngine.XR;

namespace GoogleTrends.GameUtility
{
    public class ReferenceListBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(List<IReference>))]
        private DetailObserver<List<IReference>> _listObserver;

        [SerializeField]
        private Transform _container;
        
        [SerializeField]
        private ReferenceModule _prefab;

        [SerializeField]
        private bool _destroyOnDisable;

        [SerializeField]
        private Transform[] _objectsAtBack;

        private readonly List<Transform> _existing = new List<Transform>();
        private bool _waitingForChange;
        
        private void Start()
        {
            _listObserver.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void OnEnable()
        {
            if (_waitingForChange || _destroyOnDisable)
            {
                _waitingForChange = false;
                HandleChange();
            }
        }

        private void OnDisable()
        {
            if (_destroyOnDisable)
            {
                foreach (var existing in _existing)
                {
                    Destroy(existing.gameObject);
                }

                _existing.Clear();
            }
        }

        private void HandleChange()
        {
            if (!gameObject.activeInHierarchy)
            {
                _waitingForChange = true;
            }
            
            foreach (var existing in _existing)
            {
                Destroy(existing.gameObject);
            }

            _existing.Clear();
            foreach (var element in _listObserver.Value)
            {
                var newItem = Instantiate(_prefab, _container);
                newItem.Reference = element;
                
                _existing.Add(newItem.transform);
            }

            foreach (var backObject in _objectsAtBack)
            {
                backObject.SetAsLastSibling();
            }
        }

        public void ChangeListOrder(int oldIndex, int newIndex)
        {
            var list = _listObserver.Value;
            var oldValue = list[oldIndex];
            list[oldIndex] = list[newIndex];
            list[newIndex] = oldValue;
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _listObserver;
        }
    }
}