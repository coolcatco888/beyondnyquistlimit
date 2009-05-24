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
    public class MonsterInfoWriter : ContentTypeWriter<MonsterInfo>
    {
        protected override void Write(ContentWriter output, MonsterInfo value)
        {
            output.Write(value.Health);
            output.Write(value.Mana);
            output.Write(value.Damage);
            output.Write(value.Defense);
            output.Write(value.Experience);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(MonsterInfo.MonsterInfoReader).AssemblyQualifiedName;
        }
    }
}
