using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ContentTestLibrary;

namespace ContentTestPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class SpriteContentWriter : ContentTypeWriter<Sprite>
    {
        protected override void Write(
                ContentWriter output,
                Sprite value)
        {
            output.Write(value.Position);
            output.Write(value.Rotation);
            output.Write(value.Scale);
            output.Write(value.TextureAsset);
        }

        public override string GetRuntimeReader(
                TargetPlatform targetPlatform)
        {
            return typeof(SpriteContentReader).AssemblyQualifiedName;
        }
    }

}
