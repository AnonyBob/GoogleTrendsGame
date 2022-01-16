using System.Collections.Generic;
using System.Linq;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using UnityEngine;

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
        
        private readonly List<IReference> _options = new List<IReference>();
        
        public void Select(IReference selection)
        {
            var selected = selection.GetMutable<bool>(DetailNames.Selected);
            selected.SetValue(true);

            EntireSelection.Add(selection);
            if (_numSelectable < 2)
            {
                foreach (var option in _options)
                {
                    if (option == selection)
                    {
                        continue;
                    }

                    option.GetMutable<bool>(DetailNames.Selected).SetValue(false);
                }
            }
            else if (EntireSelection.Count > _numSelectable)
            {
                for (var i = 0; i < EntireSelection.Count; ++i)
                {
                    Deselect(EntireSelection[i--]);
                    if (_numSelectable >= EntireSelection.Count)
                    {
                        break;
                    }
                }
            }
        }

        public void Deselect(IReference reference, bool force = false)
        {
            if (_allowDeselect || force)
            {
                var selected = reference.GetMutable<bool>(DetailNames.Selected);
                selected.SetValue(false);

                EntireSelection.Remove(reference);
            }
        }

        public void DeselectAll()
        {
            foreach (var option in _options)
            {
                Deselect(option, true);
            }
        }

        public void Register(IReference option)
        {
            _options.Add(option);
        }

        public void Unregister(IReference option)
        {
            _options.Remove(option);
            EntireSelection.Remove(option);
        }
    }
}