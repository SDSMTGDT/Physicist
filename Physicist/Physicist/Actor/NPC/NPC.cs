namespace Physicist.Actors.NPCs
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class NPC : Actor
    {
        private string behavior = "Idle";

        protected NPC()
        {
        }
        
        protected NPC(string name) :
            base(name)
        {
            this.Player = null;
            this.UpdateMethods = new Dictionary<string, UpdateDel>();
            this.UpdateMethods.Add(StandardBehavior.Aggressive.ToString(), new UpdateDel(this.AggressiveUpdate));
            this.UpdateMethods.Add(StandardBehavior.Crazy.ToString(), new UpdateDel(this.CrazyUpdate));
            this.UpdateMethods.Add(StandardBehavior.Fearful.ToString(), new UpdateDel(this.FearfulUpdate));
            this.UpdateMethods.Add(StandardBehavior.Funny.ToString(), new UpdateDel(this.FunnyUpdate));
            this.UpdateMethods.Add(StandardBehavior.Idle.ToString(), new UpdateDel(this.IdleUpdate));
            this.UpdateMethods.Add(StandardBehavior.Normal.ToString(), new UpdateDel(this.NormalUpdate));
            this.UpdateMethods.Add(StandardBehavior.Submissive.ToString(), new UpdateDel(this.SubmissiveUpdate));
        }

        protected delegate void UpdateDel(GameTime gameTime);

        public string Behavior 
        {
            get
            {
                return this.behavior;
            }

            set
            {
                if (this.UpdateMethods[value] != null)
                {
                    this.behavior = value;
                }
            }
        }

        protected Player Player { get; set; }

        protected Dictionary<string, UpdateDel> UpdateMethods { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                if (this.Player == null)
                {
                    var players = this.Map.Players as List<Player>;
                    if (players != null && players.Count > 0)
                    {
                        this.Player = players[0];
                    }
                }

                this.UpdateMethods[this.Behavior].Invoke(gameTime);

                base.Update(gameTime);
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                                "NPC",
                                new XAttribute("behavior", this.Behavior),
                                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Player = null;
                this.UpdateMethods = new Dictionary<string, UpdateDel>();
                this.UpdateMethods.Add(StandardBehavior.Aggressive.ToString(), new UpdateDel(this.AggressiveUpdate));
                this.UpdateMethods.Add(StandardBehavior.Crazy.ToString(), new UpdateDel(this.CrazyUpdate));
                this.UpdateMethods.Add(StandardBehavior.Fearful.ToString(), new UpdateDel(this.FearfulUpdate));
                this.UpdateMethods.Add(StandardBehavior.Funny.ToString(), new UpdateDel(this.FunnyUpdate));
                this.UpdateMethods.Add(StandardBehavior.Idle.ToString(), new UpdateDel(this.IdleUpdate));
                this.UpdateMethods.Add(StandardBehavior.Normal.ToString(), new UpdateDel(this.NormalUpdate));
                this.UpdateMethods.Add(StandardBehavior.Submissive.ToString(), new UpdateDel(this.SubmissiveUpdate));

                var behav = element.GetAttribute("behavior", StandardBehavior.Normal.ToString());

                if (this.UpdateMethods.ContainsKey(behav))
                {
                    this.Behavior = behav;
                }
                else
                {
                    this.UpdateMethods.Add(behav, null);
                }

                base.XmlDeserialize(element.Element("Actor"));
            }
        }

        protected virtual void NormalUpdate(GameTime gameTime)
        {
        }

        protected virtual void IdleUpdate(GameTime gameTime)
        {
        }

        protected virtual void AggressiveUpdate(GameTime gameTime)
        {
        }

        protected virtual void SubmissiveUpdate(GameTime gameTime)
        {
        }

        protected virtual void FearfulUpdate(GameTime gameTime)
        {
        }

        protected virtual void FunnyUpdate(GameTime gameTime)
        {
        }

        protected virtual void CrazyUpdate(GameTime gameTime)
        {
        }

        protected virtual void VolatileUpdate(GameTime gameTime)
        {
        }
    }
}
