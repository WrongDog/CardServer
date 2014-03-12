using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public enum NoticableEventOccasion
    {
        Before,
        After
    }
    public class NoticableEvent<T,T2>
    {
        public delegate void firedele(T2 param);
        protected firedele FireCore;
        protected T Owner;
        protected Dictionary<NoticableEventOccasion, List<WeakReference>> Observers = new Dictionary<NoticableEventOccasion,List<WeakReference>>();
        ///// <summary>
        ///// we propably can't add delegate here because it's shared between hosts and it can not be actually destroied
        ///// </summary>
        ///// <param name="occasion"></param>
        ///// <param name="action"></param>
        //public void AddNotice(NoticableEventOccasion occasion,Action<T,T2> action)
        //{
        //    List<WeakReference> listObserver = new List<WeakReference>();
        //    Observers.TryGetValue(occasion, out listObserver);
        //    listObserver.Add(new WeakReference(action));       
        //}
        public void AddNotice(NoticableEventOccasion occasion, IEventHandler handler)
        {
            List<WeakReference> listObserver = new List<WeakReference>();
            Observers.TryGetValue(occasion, out listObserver);
            listObserver.Add(new WeakReference(handler));
            
        }
        
        public NoticableEvent(T owner,firedele fireCore)
        {
            this.Owner = owner;
            this.FireCore = fireCore;
            foreach (NoticableEventOccasion occasion in Enum.GetValues(typeof(NoticableEventOccasion)))
            {
                Observers.Add(occasion, new List<WeakReference>());
            }
        }
        private void TriggerEventHandlers(T2 param,NoticableEventOccasion occasion)
        {
            //no need to remove notice since we are using weak but that reqires gabage collect before invoke
            GC.Collect();
            List<WeakReference> listObserver = new List<WeakReference>();
            Observers.TryGetValue(occasion, out listObserver);
            foreach (WeakReference weakaction in listObserver)
            {
                if (weakaction.IsAlive && weakaction.Target != null)
                {
                    //Action<T, T2> action = (Action<T, T2>)weakaction.Target;
                    //action.Invoke(this.Owner, param);
                    IEventHandler handler = (IEventHandler)weakaction.Target;
                    handler.Handle(this.Owner, param);
                    if (handler.DestroyAfterAction) weakaction.Target = null;
                }
            }
            // listObserver = listObserver.AsQueryable().Where((w) => w.IsAlive).ToList<WeakReference>();
            listObserver.RemoveAll((w) => !w.IsAlive || w.Target==null);
        }
        public void Fire(T2 param)
        {
            TriggerEventHandlers(param, NoticableEventOccasion.Before);

            if(FireCore != null)FireCore(param);

            TriggerEventHandlers(param, NoticableEventOccasion.After);
        }
    }
}
