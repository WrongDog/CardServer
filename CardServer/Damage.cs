using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public class PhysicalDamage : Damage
    {
        public override DamageType DamageType { get { return DamageType.Physical; } }
    }
    public class MagicalDamage:Damage
    {
        public override DamageType DamageType { get { return DamageType.Magical; } }
    }
    public abstract class Damage
    {
        public abstract DamageType DamageType { get; }
        public int Amount { get; set; }
        
    }
    public enum DamageType
    {
        Physical,
        Magical
    }
}
