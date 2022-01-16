using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;

namespace GoogleTrends.Teams
{
    public class TeamDeleter : BaseDetailBinder
    {
        public void DeleteTeam()
        {
            var name = _referenceModule.Reference.GetDetail<string>(DetailNames.Name);
            var manager = GetComponentInParent<TeamManager>();
            if (manager != null)
            {
                manager.DeleteTeam(name.GetValue());
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield break;
        }
    }
}