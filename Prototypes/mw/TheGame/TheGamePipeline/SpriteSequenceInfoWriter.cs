using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Library;

namespace TheGamePipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class SpriteSequenceInfoWriter : ContentTypeWriter<SpriteSequenceInfo>
    {
        protected override void Write(ContentWriter output, SpriteSequenceInfo value)
        {
            output.Write(value.StateKey);
            output.Write(value.OrientationKey);
            output.Write(value.IsLoop);
            output.Write(value.SequenceVelocity);
            output.Write(value.NumBuffers);
            output.Write(value.ScaleX);
            output.Write(value.ScaleY);
            output.Write(value.ChangeScale);
            output.Write(value.IsARowOrColumn);
            output.Write(value.IndexX);
            output.Write(value.IndexY);
            output.Write(value.RowOrColumn);
            output.Write(value.AttackKey);
            output.Write(value.AttackValue);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteSequenceInfo.SpriteSequenceInfoReader).AssemblyQualifiedName;
        }
    }
}
