using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public enum ResourceType
    {
        Health,
        Magic,
        Rage,
        Tech
    }
    public interface IResource
    {
         ResourceType ResourceType { get; set; }
         int CurrentAmount { get; set; }
         int MaxAmount { get; set; }
   
    }
}
