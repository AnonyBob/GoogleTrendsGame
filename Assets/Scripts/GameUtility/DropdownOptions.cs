using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [System.Serializable]
    public struct DropdownOption
    {
        [SerializeField]
        public string DisplayValue;
        
        [SerializeField]
        public string Value;
    }
    
    [CreateAssetMenu(menuName = "GoogleTrends/Dropdown Options")]
    public class DropdownOptions : ScriptableObject
    {
        [SerializeField]
        public List<DropdownOption> Options = new List<DropdownOption>();

        public IEnumerable<TMP_Dropdown.OptionData> GetOptions()
        {
            return Options.Select(x => new TMP_Dropdown.OptionData(x.DisplayValue));
        }

        public string GetValueFromIndex(int index)
        {
            return Options[index].Value;
        }
        
        public string GetDisplayValueFromIndex(int index)
        {
            return Options[index].DisplayValue;
        }
        
        public int GetIndex(string value)
        {
            return Options.FindIndex(o => o.Value == value);
        }
    }
}