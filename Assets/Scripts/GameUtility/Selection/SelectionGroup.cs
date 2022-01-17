using System.Collections.Generic;
using System.Linq;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace GoogleTrends.GameUtility.Selection
{
    public class SelectionGroup : MonoBehaviour
    {
        public IReference FirstSelection => EntireSelection.FirstOrDefault();

        public List<IReference> EntireSelection { get; } = new List<IReference>();
        
        [SerializeField]
        private int _numSelectable = 1;

        [SerializeField]
        private bool _allowDeselect = true;

        [SerializeField]
        private UnityEvent _selectionChanged;
        
        private readonly List<IReference> _options = new List<IReference>();
        
        public void Select(IReference selection)
        {
            var selected = selection.GetMutable<bool>(DetailNames.Selected);
            selected.SetValue(true);

            EntireSelection.Add(selection);
            if (EntireSelection.Count > _numSelectable)
            {
                for (var i = 0; i < EntireSelection.Count; ++i)
                {
                    InternalDeselect(EntireSelection[i--]);
                    if (_numSelectable >= EntireSelection.Count)
                    {
                        break;
                    }
                }
            }
            
            _selectionChanged?.Invoke();
        }

        public void Deselect(IReference reference, bool force = false)
        {
            if (_allowDeselect || force)
            {
                InternalDeselect(reference);
                _selectionChanged?.Invoke();
            }
        }

        private void InternalDeselect(IReference reference)
        {
            var selected = reference.GetMutable<bool>(DetailNames.Selected);
            selected.SetValue(false);
            
            EntireSelection.Remove(reference);
        }

        public void DeselectAll()
        {
            foreach (var option in _options)
            {
                InternalDeselect(option);
            }
            
            _selectionChanged?.Invoke();
        }

        public void Register(IReference option)
        {
            _options.Add(option);
        }

        public void Unregister(IReference option)
        {
            _options.Remove(option);
            if (EntireSelection.Remove(option))
            {
                _selectionChanged?.Invoke();
            }
        }
    }
}