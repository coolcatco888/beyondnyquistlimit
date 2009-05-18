using System;
using System.Collections.Generic;
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
    public class PointSpriteSystemInverted : PointSpriteSystem
    {
        public PointSpriteSystemInverted(GameScreen parent, PointSpriteSystemSettings settings)
            : base(parent, settings)
        { }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = GameEngine.Graphics;

            device.RenderState.BlendFunction = BlendFunction.ReverseSubtract;

            base.Draw(gameTime);
        }
    }
}
