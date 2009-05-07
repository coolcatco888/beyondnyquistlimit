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
    public class EquipmentWriter : ContentTypeWriter<Equipment>
    {
        protected override void Write(ContentWriter output, Equipment value)
        {
            // TODO: write the specified value to the output ContentWriter.
            output.Write(value.Title);
            output.Write(value.Description);
            output.Write(value.GoldValue);
            output.WriteObject(value.RestrictedClasses);
            output.Write(value.MinimumLevel);
            output.Write(value.TextureFileName);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(Equipment.EquipmentContentReader).AssemblyQualifiedName;
        }
    }
}
