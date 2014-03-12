using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public abstract class WorldObject
    {
        #region events
        public NoticableEvent<WorldObject, bool> RoundStart;
        public NoticableEvent<WorldObject, bool> RoundEnd;
        #endregion
        public WorldObject()
        {
            this.RoundStart = new NoticableEvent<WorldObject, bool>(this, null);
            this.RoundEnd = new NoticableEvent<WorldObject, bool>(this,null);
        }
    }
}
