using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Data.Serialized;
using OwlAndJackalope.UX.Runtime.Modules;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class TeamAdder : MonoBehaviour
    {
        [SerializeField]
        private ReferenceModule _gameReference;

        [SerializeField]
        private ReferenceTemplate[] _teamTemplates;

        [SerializeField]
        private int _maxTeams = 4;
        
        public void AddTeam()
        {
            var teams = _gameReference.Reference.GetMutableCollection<IReference>(DetailNames.Teams);
            if (teams.Count < _maxTeams)
            {
                var nextTeam = teams.Count % _teamTemplates.Length;
                teams.Add(_teamTemplates[nextTeam].Reference.ConvertToReference());
            }
        }
    }
}