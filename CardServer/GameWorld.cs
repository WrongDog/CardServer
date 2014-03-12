using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMethod;

namespace CardServer
{
    public class GameWorld
    {
        List<Player> players;
        BattleField battleField;
        public static readonly EvaluatorPool BehaviorPool = new EvaluatorPool();
        protected BattleField CreateBattleField(string fieldsetting)
        {
            return new BattleField();
        }
        protected Player CreatePlayer(string playerID)
        {
            return null;
        }
        protected Card CreateCard(Player owner, string cardID)
        {
            return null;
        }
    }
}
