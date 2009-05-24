
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    class BillboardEffect : Billboard, IAudioEmitter
    {
        #region Fields

        SpriteSequence spriteSequence;
        AudioManager audioManager;
        SpriteInfo spriteInfo;

        #endregion  // Fields

        #region Accessors
        #endregion  // Accessors

        #region Constructors

        public BillboardEffect(GameScreen parent, SpriteInfo spriteInfo, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(parent, spriteInfo.SpriteSheet)
        {
            //this is done in billboard is it not?
            //vertices[0].Position = new Vector3(1, 1, 0);
            //vertices[1].Position = new Vector3(-1, 1, 0);
            //vertices[2].Position = new Vector3(-1, -1, 0);
            //vertices[3].Position = new Vector3(1, -1, 0);

            this.spriteInfo = spriteInfo;
            spriteSequence = new SpriteSequence(true, 0);
            spriteSequence.AddRow(0, 0, (int)(this.spriteInfo.SpriteSheet.Width / this.spriteInfo.Width) - 1);

            audioManager = (AudioManager)GameEngine.Services.GetService(typeof(AudioManager));
        }

        public BillboardEffect(GameScreen parent, SpriteInfo spriteInfo, Vector3 position, Vector3 rotation)
            : this(parent, spriteInfo, position, rotation, Vector3.One)
        {
        }

        public BillboardEffect(GameScreen parent, SpriteInfo spriteInfo, Vector3 position)
            : this(parent, spriteInfo, position, Vector3.Zero, Vector3.One)
        {
        }

        public BillboardEffect(GameScreen parent, SpriteInfo spriteInfo)
            : this(parent, spriteInfo, Vector3.Zero, Vector3.Zero, Vector3.One)
        {
        }

        #endregion  // Constructors

        public override void Update(GameTime gameTime)
        {
            if (spriteSequence.IsComplete)
            {
                this.Dispose();
                return;
            }

            spriteSequence.Update(gameTime);
            UpdateVertices();

            if (spriteSequence.CurrentFrameColumn == 0)
            {
                audioManager.Play3DCue("asplosion", this);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void UpdateVertices()
        {
            vertices[0].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y);

            vertices[1].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y);

            vertices[2].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

            vertices[3].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);
        }

        #region IAudioEmitter Members


        public Vector3 Forward
        {
            get { return Vector3.Forward; }
        }

        public Vector3 Up
        {
            get { return Vector3.Up; }
        }

        public Vector3 Velocity
        {
            get { return Vector3.Zero; }
        }

        #endregion
    }
}
