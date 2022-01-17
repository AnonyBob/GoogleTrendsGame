using UnityEngine;

namespace GoogleTrends.UI
{
    public class MainMenuOpener : MonoBehaviour
    {
        public void OpenMainMenu()
        {
            MainMenuManager.ReturnToMain();
        }
    }
}