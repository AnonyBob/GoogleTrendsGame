using System.Linq;
using GoogleTrends.GameManagers;
using OwlAndJackalope.UX.Runtime.Data.Serialized;
using OwlAndJackalope.UX.Runtime.Modules;
using UnityEngine;

namespace GoogleTrends.UI
{
    public class MainMenuManager : MonoSingleton<MainMenuManager>
    {
        [SerializeField]
        private ReferenceModule _referenceModule;
        
        [SerializeField]
        private ReferenceTemplate[] _defaultTeams;

        public static void ReturnToMain()
        {
            Instance.ReturnToMainInstance();
        }

        public void ReturnToMainInstance()
        {
            GameManager.ResetGame(_referenceModule.Reference, _defaultTeams.Select(t => t.Reference.ConvertToReference()));
        }
    }
}