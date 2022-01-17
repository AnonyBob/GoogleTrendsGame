using System.Linq;
using OwlAndJackalope.UX.Runtime.Data;

namespace GoogleTrends.Terms
{
    public class TeamTermsAreReadyChecker : TermsAreReadyChecker
    {
        protected override string DetailName => DetailNames.CurrentTerm;

        protected override bool DetermineIfReady()
        {
            var currentTerm = _referenceModule.Reference.GetDetail<IReference>(DetailNames.CurrentTerm);
            if (currentTerm == null || currentTerm.GetValue() == null)
            {
                return false;
            }

            var text = currentTerm.GetValue().GetDetail<string>(DetailNames.TermText);
            if (text == null || text.GetValue() == null)
            {
                return false;
            }
            
            var currentTermText = text.GetValue().ToLower();
            var count = _termTexts.Select(t => t.Value)
                .Distinct()
                .Count(t => !string.IsNullOrEmpty(t) && t.ToLower().Contains(currentTermText));
            
            return  count == _termTexts.Count && _termTexts.Count > 0;
        }
    }
}