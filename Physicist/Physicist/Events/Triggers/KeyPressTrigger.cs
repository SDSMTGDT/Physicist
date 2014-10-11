namespace Physicist.Events.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;

    public class KeyPressTrigger : Trigger
    {
        public Keys TriggerKey { get; private set; }

        public Enums.KeyState KeyState { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {               
                var state = Keyboard.GetState();

                var active = this.KeyState == Enums.KeyState.Down ? state.IsKeyDown(this.TriggerKey) : state.IsKeyUp(this.TriggerKey);

                if (this.IsActive != active)
                {
                    if (active)
                    {
                        this.ActivateWithStyle();
                    }
                    else
                    {
                        this.DeactivateWithStyle();
                    }
                }

                base.Update(gameTime);
            }
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                var triggerKeyEle = element.Attribute("triggerKey");
                if (triggerKeyEle != null)
                {
                    var action = (StandardKeyAction)Enum.Parse(typeof(StandardKeyAction), triggerKeyEle.Value);
                    this.TriggerKey = Physicist.Controls.KeyboardController.KeyForAction(action);
                }

                this.KeyState = Enums.KeyState.Down;

                var keyStateEle = element.Attribute("keyState");
                if (keyStateEle != null)
                {
                    this.KeyState = (Enums.KeyState)Enum.Parse(typeof(Enums.KeyState), keyStateEle.Value);
                }

                base.XmlDeserialize(element.Element("Trigger"));
            }
        }
    }
}
