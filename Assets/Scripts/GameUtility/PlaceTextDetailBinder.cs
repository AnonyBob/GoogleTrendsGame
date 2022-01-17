using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PlaceTextDetailBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(int))]
        private DetailObserver<int> _place;

        private TextMeshProUGUI _text;
        
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _place.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void HandleChange()
        {
            var place = _place.Value;
            var placeString = "";
            switch (place)
            {
                case 0:
                    placeString = "1st";
                    break;
                case 1:
                    placeString = "2nd";
                    break;
                case 2:
                    placeString = "3rd";
                    break;
                case 3:
                    placeString = "4th";
                    break;
            }

            _text.text = placeString;
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _place;
        }
    }
}