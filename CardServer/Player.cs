using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public class Cards
    {
        public List<Card> Hands { get; set; }
        public List<Card> Field { get; set; }
        public List<Card> GraveYard { get; set; }
        public List<Card> Bag { get; set; }
    }
    public abstract class Player:DamageableResourceHolder
    {
        
        #region properties
       
        public Cards Cards { get; set; }
        public List<Player> Enemys { get; set; }
        public List<Status> Status { get; set; }
        public Player Enemy
        {
            get
            {
                if (Enemys.Count > 0) return Enemys[0];
                return null;
            }
        }
        //needed?
        public IResource Health
        {
            get
            {
                return Resources[ResourceType.Health];               
            }
        }
        #endregion
        #region action
        public  NoticableEvent<Player,Card> Sacrifice;
        public  NoticableEvent<Player, Card> Summon;
        public  NoticableEvent<Player, Card> Cast;
        #endregion
      
        public Player()
        {
         
            this.Sacrifice = new NoticableEvent<Player, Card>(this, this.OnSacrifice);
            this.Summon = new NoticableEvent<Player, Card>(this, this.OnSummon);
            this.Cast = new NoticableEvent<Player, Card>(this, this.OnCast);
            
        }
        #region virtual/abstract methods
       
        public virtual void OnSacrifice(Card card)
        {
            this.Cards.Hands.Remove(card);
            this.Cards.GraveYard.Add(card);
            card.Sacrifice();
            
        }
        public virtual void OnSummon(Card card)
        {
            Position position = new Position();// from user input;
            this.Cards.Hands.Remove(card);
            this.Cards.Field.Add(card);
            card.SummonAt(position);
            
        }
        public virtual void OnCast(Card card)
        {
            this.Cards.Hands.Remove(card);
            this.Cards.GraveYard.Add(card);
            
        }
        #endregion

    }
}
