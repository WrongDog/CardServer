using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DynamicMethod;
namespace CardServer
{
   
    /// <summary>
    /// should look like
    /// 
    /// public void AttachOn(dynamic something){
    ///      something.OnSomeEvent.AddNotice(before,this.method1);
    /// }
    /// public method1(TypeA obja,TypeB objb){
    /// 
    /// }
    /// 
    /// </summary>
    public  class DynamicBehavior:IEventHandler
    {
        
        //since we are using weak reference on notice event we don't really need to detach
        //public object Owner { get; set; }
        //public NoticableEvent AttachedEvent { get; set; }
        public DynamicBehavior(Delegate addnotice, NoticableEventOccasion occasion)
        {

        }
        //add notice to some object's event
        public void Attach()
        {
            
        }
        //remove notice to some object's event
        //public void Detach()
        //{

        //}



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
