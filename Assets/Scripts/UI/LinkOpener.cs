using UnityEngine;

namespace GoogleTrends.UI
{
    public class LinkOpener : MonoBehaviour
    {
        [SerializeField]
        private string _link;
        
        public void OpenLink()
        {
            Application.OpenURL(_link);
        }
    }
}