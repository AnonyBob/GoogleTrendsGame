using System.Collections.Generic;
using System.Linq;
using GoogleTrends.UI;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Data.Serialized;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.GameManagers
{
    public class GameSettingsManager : BaseDetailBinder
    {
        private IMutableCollectionDetail<IReference> Terms =>
            _referenceModule.Reference.GetMutableCollection<IReference>(DetailNames.Terms);

        private void Start()
        {
            MainMenuManager.ReturnToMain();
        }
        
        public void OpenGameSettings()
        {
            var gameState = _referenceModule.Reference.GetMutable<GameState>(DetailNames.GameState);
            gameState.SetValue(GameState.GameSetup);
        }

        public void AddTerm()
        {
            Terms.Add(new BaseReference(
                new BaseDetail<string>(DetailNames.TermText, ""),
                new BaseDetail<string>(DetailNames.BonusTerm, ""),
                new BaseDetail<int>(DetailNames.BonusTermPoints, 0),
                new BaseDetail<int>(DetailNames.Multiplier, 0)));
        }

        public void MoveTermUp()
        {
            var terms = Terms;
        }
        
        public void MoveTermDown()
        {
            var terms = Terms;
        }
        
        public void RemoveTerm(int index)
        {
            Terms.RemoveAt(index);
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield break;
        }
    }
}