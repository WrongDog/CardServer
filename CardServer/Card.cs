using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public abstract class Card : DamageableResourceHolder
    {
        
        public Player Owner { get; set; }
        public Card OppositeCard { get; set; }
        public abstract bool CanBeSummon(Position position);
        public List<Status> Status { get; set; }
        //public Behaviors Behaviors { get; set; }
        public Position CardPosition { get; set; }
        public Card(Player owner)
        {
            this.Owner = owner;
            //Behaviors = new Behaviors(this);
        }
        public void Sacrifice()
        {
        }
        public void SummonAt(Position position)
        {
            this.CardPosition = position;
            //remove resources from owner

            //add status to target

            //need to attach behaviors to target
        }
        public override void Detach()
        {
            //Behaviors.Detach();
        }
      
    }
}
