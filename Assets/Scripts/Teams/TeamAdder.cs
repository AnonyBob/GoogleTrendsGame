using System;
using System.Collections.Generic;
using System.Linq;
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

        private static readonly Type[] _colorType = new Type[] { typeof(Color) };
        
        public void AddTeam()
        {
            var teams = _gameReference.Reference.GetMutableCollection<IReference>(DetailNames.Teams);
            if (teams.Count < _maxTeams)
            {
                var index = GetTeamsByColor().FindIndex(c => NotIncluded(c, teams));
                var nextTeam = teams.Count % _teamTemplates.Length;
                teams.Add(_teamTemplates[nextTeam].Reference.ConvertToReference());
            }
        }

        private bool NotIncluded(Color color, IMutableCollectionDetail<IReference> teams)
        {
            var included = false;
            foreach (var team in teams)
            {
                var teamColor = team.GetDetail<Color>(DetailNames.Color).GetValue();
                if (teamColor == color)
                {
                    included = true;
                }
            }

            return !included;
        }

        private List<Color> GetTeamsByColor()
        {
            return _teamTemplates.Select(t =>
            {
                var details = t.Reference.GetDetails(_colorType);
                var detail = (BaseSerializedDetail)details.FirstOrDefault(d => d.Name == DetailNames.Color);
                return detail.GetColor();
            }).ToList();
        }
    }
}