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
    /// <summary>
    /// Custom vertex structure for drawing point sprite particles.
    /// </summary>
    struct ParticleVertex
    {
        // Stores the starting position of the particle.
        public Vector3 Position;

        // Stores the constant velocity of the particle.
        public Vector3 Velocity;

        // color added to the particle
        public Color Color;

        //Size of the partical
        public float Size;

        // x: The time (in seconds) at which this particle was created.
        // y: The lifespan in seconds of the particle
        public Vector2 Time;

        //Misc data such as random values
        public Vector2 Data;

        // Describe the layout of this vertex structure.
        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, 0, VertexElementFormat.Vector3,
                                    VertexElementMethod.Default,
                                    VertexElementUsage.Position, 0),

            new VertexElement(0, 12, VertexElementFormat.Vector3,
                                     VertexElementMethod.Default,
                                     VertexElementUsage.Normal, 0),

            new VertexElement(0, 24, VertexElementFormat.Color,
                                     VertexElementMethod.Default,
                                     VertexElementUsage.Color, 0),

            new VertexElement(0, 28, VertexElementFormat.Single,
                                     VertexElementMethod.Default,
                                     VertexElementUsage.PointSize, 0),

            new VertexElement(0, 32, VertexElementFormat.Vector2,
                                     VertexElementMethod.Default,
                                     VertexElementUsage.TextureCoordinate, 0),

            new VertexElement(0, 40, VertexElementFormat.Vector2,
                                     VertexElementMethod.Default,
                                     VertexElementUsage.TextureCoordinate, 1)  
        };


        // Describe the size of this vertex structure.
        public const int SizeInBytes = 48;
    }
}
