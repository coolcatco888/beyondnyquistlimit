
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    public class BillboardSlash : Billboard
    {
        #region Fields
        
        #endregion

        #region Constructors

        public BillboardSlash(GameScreen parent, Texture2D texture2D, Vector3 position, float rotation)
            : base(parent, texture2D)
        {
            this.position = position;
            this.rotation = rotation;
        }

        #endregion
    }
}
