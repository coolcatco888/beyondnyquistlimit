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
    public class CharacterClassInfoWriter : ContentTypeWriter<CharacterClassInfo>
    {
        protected override void Write(ContentWriter output, CharacterClassInfo value)
        {
            // Write character class descriptive data
            output.Write(value.ClassName);
            output.Write(value.Description);

            // Write character class base attributes
            /*
            output.Write(value.Strength);
            output.Write(value.Agility);
            output.Write(value.Constitution);
            output.Write(value.Intelligence);
            output.Write(value.Wisdom);
            output.Write(value.Charisma);
            */

            // Write character class base data
            output.Write(value.BaseHealth);
            output.Write(value.BaseMana);
            output.Write(value.BaseDamage);
            output.Write(value.BaseDefense);

            // Write character class level up info
            output.Write(value.GainHealth);
            output.Write(value.GainMana);
            output.Write(value.GainDamage);
            output.Write(value.GainDefense);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return typeof(CharacterClassInfo.CharacterClassInfoReader).AssemblyQualifiedName;
        }
    }
}
