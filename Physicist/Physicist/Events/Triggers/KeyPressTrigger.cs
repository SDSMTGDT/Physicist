namespace Physicist.Events.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;
    using Physicist.Extensions;

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

        public override XElement XmlSerialize()
        {
            return new XElement(
                "KeyPressTrigger",
                new XAttribute("triggerKey", this.TriggerKey.ToString()),
                new XAttribute("keyState", this.KeyState.ToString()),
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.TriggerKey = Physicist.Controls.KeyboardController.KeyForAction(element.GetAttribute("triggerKey", StandardKeyAction.Jump));
                this.KeyState = element.GetAttribute("keyState", Enums.KeyState.Down);
                base.XmlDeserialize(element.Element("Trigger"));
            }
        }
    }
}
