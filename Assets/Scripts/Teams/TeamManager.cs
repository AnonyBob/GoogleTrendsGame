using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Modules;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class TeamManager : MonoBehaviour
    {
        [SerializeField]
        private ReferenceModule _gameReference;
        
        public void DeleteTeam(string teamName)
        {
            var teams = _gameReference.Reference.GetMutableCollection<IReference>(DetailNames.Teams);
            for (var i = 0; i < teams.Count; ++i)
            {
                if (teams[i].GetDetail<string>(DetailNames.Name).GetValue() == teamName)
                {
                    teams.RemoveAt(i);
                    break;
                }
            }
        }
    }
}