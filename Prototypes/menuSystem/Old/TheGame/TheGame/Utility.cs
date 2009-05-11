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
using System.Reflection;

namespace TheGame
{
    public static class Utility
    {
        public static List<T> GetEnumValues<T>()
        {
            Type currentEnum = typeof(T);
            List<T> resultSet = new List<T>();

            if (currentEnum.IsEnum)
            {
                FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                    resultSet.Add((T)field.GetValue(null));
            }
            else
                throw new ArgumentException("The argument must of type Enum or of a type derived from Enum", "T");

            return resultSet;
        }
    }
}
