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

        public static Orientation GetOrientationFromString(string value)
        {
            Orientation orientation = Orientation.South;
            foreach (Orientation o in Enum.GetValues(typeof(Orientation)))
            {
                if (o.ToString() == value)
                    orientation = o;
            }

            return orientation;
        }

        public static Orientation GetOppositeOrientation(Orientation o)
        {
            Orientation orientation = Orientation.South;

            switch (o)
            {
                case Orientation.South:
                    orientation = Orientation.North;
                    break;
                case Orientation.North:
                    orientation = Orientation.South;
                    break;
                case Orientation.Southwest:
                    orientation = Orientation.Northeast;
                    break;
                case Orientation.Southeast:
                    orientation = Orientation.Northwest;
                    break;
                case Orientation.Northeast:
                    orientation = Orientation.Southwest;
                    break;
                case Orientation.Northwest:
                    orientation = Orientation.Southeast;
                    break;
                case Orientation.East:
                    orientation = Orientation.West;
                    break;
                case Orientation.West:
                    orientation = Orientation.East;
                    break;
            }

            return orientation;
        }

        public static Vector3 PositionChangeBasedOnOrientation(Orientation o, float delta)
        {
            Vector3 hitBack = new Vector3();

            switch (o)
            {
                case Orientation.South:
                    hitBack = new Vector3(0.0f, 0.0f, delta);
                    break;
                case Orientation.North:
                    hitBack = new Vector3(0.0f, 0.0f, -delta);
                    break;
                case Orientation.Southwest:
                    hitBack = new Vector3(-delta, 0.0f, delta);
                    break;
                case Orientation.Southeast:
                    hitBack = new Vector3(delta, 0.0f, delta);
                    break;
                case Orientation.Northeast:
                    hitBack = new Vector3(delta, 0.0f, -delta);
                    break;
                case Orientation.Northwest:
                    hitBack = new Vector3(-delta, 0.0f, -delta);
                    break;
                case Orientation.East:
                    hitBack = new Vector3(delta, 0.0f, 0.0f);
                    break;
                case Orientation.West:
                    hitBack = new Vector3(-delta, 0.0f, 0.0f);
                    break;
            }

            return hitBack;
        }
    }
}
