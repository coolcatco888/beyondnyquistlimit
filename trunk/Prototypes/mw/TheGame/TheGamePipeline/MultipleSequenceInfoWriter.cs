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
    public class MultipleSequenceInfoWriter : ContentTypeWriter<MultipleSequenceInfo>
    {
        protected override void Write(ContentWriter output, MultipleSequenceInfo value)
        {
            output.Write(value.StateKey);
            output.Write(value.OrientationKey);
            output.Write(value.Duration);
            output.WriteObject<List<SpriteSequenceInfo>>(value.Subsequences);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(MultipleSequenceInfo.MultipleSequenceInfoReader).AssemblyQualifiedName;
        }
    }
}
