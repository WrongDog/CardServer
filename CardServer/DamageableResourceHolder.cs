using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public abstract class DamageableResourceHolder : WorldObject
    {
        public Resources Resources { get; set; }
        #region events
        public NoticableEvent<DamageableResourceHolder, Damage> OnDamage;
        public NoticableEvent<DamageableResourceHolder, MagicalDamage> OnMagicalDamage;
        public NoticableEvent<DamageableResourceHolder, PhysicalDamage> OnPhysicalDamage;
        #endregion
        public DamageableResourceHolder()
        {
            this.OnDamage = new NoticableEvent<DamageableResourceHolder, Damage>(this, this.OnDamegeFireCore);
            this.OnMagicalDamage = new NoticableEvent<DamageableResourceHolder, MagicalDamage>(this, this.OnMagicalDamageFireCore);
            this.OnPhysicalDamage = new NoticableEvent<DamageableResourceHolder, PhysicalDamage>(this, this.OnPhysicalDamageFireCore);
        }
        #region virtual
        public virtual void OnDamegeFireCore(Damage damage)
        {
            if (damage.GetType() == typeof(MagicalDamage))
            {
                this.OnMagicalDamage.Fire((MagicalDamage)damage);
            }
            else if (damage.GetType() == typeof(PhysicalDamage))
            {
                this.OnPhysicalDamage.Fire((PhysicalDamage)damage);
            }
        }
        public virtual void OnMagicalDamageFireCore(MagicalDamage damage)
        {
            this.Resources.ResourceChange(ResourceType.Health, damage.Amount);
        }
        public virtual void OnPhysicalDamageFireCore(PhysicalDamage damage)
        {
            this.Resources.ResourceChange(ResourceType.Health, damage.Amount);
        }
        #endregion
        
    }
}
