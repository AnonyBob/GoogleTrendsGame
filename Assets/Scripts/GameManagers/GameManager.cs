using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;

namespace GoogleTrends.GameManagers
{
    public static class GameManager
    {
        public static void ResetGame(IReference game, IEnumerable<IReference> defaultTeams)
        {
            game.GetMutable<int>(DetailNames.RoundNumber).SetValue(0);
            game.GetMutable<IReference>(DetailNames.CurrentTerm).SetValue(null);
            game.GetMutable<IReference>(DetailNames.WinningTeam).SetValue(null);
            game.GetMutable<GameState>(DetailNames.GameState).SetValue(GameState.MainMenu);
            
            game.GetMutableCollection<IReference>(DetailNames.Terms).Clear();
            var teams = game.GetMutableCollection<IReference>(DetailNames.Teams);
            teams.Clear();

            foreach (var team in defaultTeams)
            {
                teams.Add(team);    
            }
        }
    }
}