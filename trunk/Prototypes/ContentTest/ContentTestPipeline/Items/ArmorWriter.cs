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
    public class ArmorWriter : ContentTypeWriter<Armor>
    {
        EquipmentWriter equipmentWriter = null;

        protected override void Write(ContentWriter output, Armor value)
        {
            output.WriteRawObject<Equipment>(value as Equipment, equipmentWriter);

            output.Write((Int32)value.ArmorSlot);
            output.Write(value.Ability);
            output.Write(value.DefenseValue);
        }

        protected override void Initialize(ContentCompiler compiler)
        {
            equipmentWriter = compiler.GetTypeWriter(typeof(Equipment))
                as EquipmentWriter;

            base.Initialize(compiler);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(Armor.ArmorContentReader).AssemblyQualifiedName;
        }
    }
}
