using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Modules;

namespace GoogleTrends.Round
{
    public class GameManager : ReferenceProvider
    {
        private IReference _gameReference;

        private void CreateReference()
        {
            _gameReference = new BaseReference(CreateDetails());
        }

        private IEnumerable<IDetail> CreateDetails()
        {
            yield return new BaseDetail<int>(DetailNames.RoundNumber, 0);
            yield return new BaseDetail<string>(DetailNames.CurrentTerm, "");
            yield return new BaseDetail<IReference>(DetailNames.WinningTeam, null);
            
            yield return new BaseDetail<GameState>(DetailNames.GameState, GameState.MainMenu);
            yield return new BaseDetail<int>(DetailNames.TimerMax, 60);
            
            yield return new BaseCollectionDetail<string>(DetailNames.Terms, new List<string>(), false);
            yield return new BaseCollectionDetail<IReference>(DetailNames.Teams, new List<IReference>(), false);
        }

        public override IEnumerable<IDetail> ProvideReference()
        {
            if (_gameReference == null)
            {
                CreateReference();
            }

            return _gameReference;
        }
    }
}