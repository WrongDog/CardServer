using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public class Resources:TypeListDictionary<ResourceType,IResource>
    {
        
        public void ResourceChange(ResourceType resourceType,int amount,bool increaseMax = false,bool canExceedMax = false)
        {
            IResource resource = this[resourceType];
            if (resource != null)
            {
                if (increaseMax) resource.MaxAmount += amount;
                resource.CurrentAmount += amount;
                if (resource.CurrentAmount > resource.MaxAmount && !canExceedMax)
                {
                    resource.CurrentAmount = resource.MaxAmount;
                }
                
            }
        }
       
    }
}
