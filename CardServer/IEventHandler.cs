using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public interface IEventHandler
    {
        void Handle(object eventowner, object eventparams);
        bool DestroyAfterAction { get; set; }
    }
}
