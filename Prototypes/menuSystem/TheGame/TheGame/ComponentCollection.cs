using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TheGame
{
    public class ComponentCollection : Collection<Component>
    {
        GameScreen owner;

        public ComponentCollection(GameScreen owner)
        {
            this.owner = owner;
        }

        // Override InsertItem so we can set the parent of the
        // component to the owner
        protected override void InsertItem(int index, Component item)
        {
            if (item.Parent != null && item.Parent != owner)
                item.Parent.Components.Remove(item);

            item.Parent = owner;

            base.InsertItem(index, item);
        }

        // Override RemoveItem so we can set the paren of
        // the component to null (no parent)
        protected override void RemoveItem(int index)
        {
            Items[index].Parent = null;

            base.RemoveItem(index);
        }
    }
}
