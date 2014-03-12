using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public class PositiveStatus : Status
    {
        public override StatusType StatusType
        {
            get { return StatusType.Positive; }
        }
    }
    public class NegativeStatus : Status
    {
        public override StatusType StatusType
        {
            get { return StatusType.Negative; }
        }
    }

    public enum StatusType
    {
        Positive,
        Negative
    }
    public abstract class Status : IEventHandler
    {
        public dynamic Attached { get; set; }
        public abstract StatusType StatusType { get; }

        public NoticableEvent<DamageableResourceHolder, DamageableResourceHolder> Attach;
        public NoticableEvent<DamageableResourceHolder, DamageableResourceHolder> Dettach;

        public void Handle(object eventowner, object eventparams)
        {
            throw new NotImplementedException();
        }

        public bool DestroyAfterAction
        {
            get;
            set;
        }
    }
}
