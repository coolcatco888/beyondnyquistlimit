using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame
{
    public class Monster : Actor
    {
        #region Fields

        protected MonsterInfo monsterStats;
        public MonsterInfo MonsterStats
        {
            get { return monsterStats; }
            set { monsterStats = value; }
        }

        public bool hasBeenHit = false;
        public string monsterName;

        #endregion // Fields

        #region Constructor

        public Monster(GameScreen parent, SpriteInfo spriteInfo, Vector3 position, string monsterName)
            : base(parent, spriteInfo, position)
        {
            behaviors = new Dictionary<ObjectType, Behaviors>();
            type = ObjectType.Monster;
            this.monsterName = monsterName;
        }

        #endregion // Constructor

        #region Initialization

        public override void Initialize()
        {
            InitializeMonsterStats();

            // Initalize AI behaviors
            InitializeBehaviours();

            InitializeSpriteSequences();

            InitializeBoundingShapes();

            base.Initialize();
        }

        #endregion // Initialization

        #region Update

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            previousState = state;

            UpdatePosition(gameTime);

            UpdateAI(gameTime);

            HandleStates();

            base.Update(gameTime);
        }

        #endregion // Update

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            boundingShapesSelf[state.ToString() + orientation.ToString()].Draw(primitiveBatch);
            primitiveShape.Draw(primitiveBatch);
            base.Draw(gameTime);
        }

        #endregion // Draw

        #region Initialization Methods

        private void InitializeMonsterStats()
        {
            string monsterInfo = monsterName + "Info";
            monsterStats = GameEngine.Content.Load<MonsterInfo>(@monsterInfo);
            this.actorStats.PopulateFields(monsterStats);
        }

        private void InitializeBehaviours()
        {
            Behavior wander = new WanderBehavior(Parent, this);
            Behaviors baseBehavior = new Behaviors();
            baseBehavior.Add(wander);

            behaviors.Add(ObjectType.None, baseBehavior);
        }

        private void InitializeSpriteSequences()
        {
            string monsterSprites = monsterName + "SpriteSequences";

            List<SpriteSequenceInfo> sequenceInfo = GameEngine.Content.Load<List<SpriteSequenceInfo>>(@monsterSprites);
            SpriteSequence sequence;

            foreach (SpriteSequenceInfo info in sequenceInfo)
            {
                Orientation o = Utility.GetOrientationFromString(info.OrientationKey);

                if (!info.ChangeScale)
                {
                    sequence = new SpriteSequence(info.StateKey, o, info.IsLoop, info.SequenceVelocity,
                        info.NumBuffers);
                }
                else
                {
                    sequence = new SpriteSequence(info.StateKey, o, info.IsLoop, info.SequenceVelocity,
                        info.NumBuffers, info.ScaleX, info.ScaleY);
                }

                if (!info.IsARowOrColumn)
                {
                    sequence.AddFrame(info.IndexX, info.IndexY);
                }
                else
                {
                    sequence.AddRow(info.RowOrColumn, info.IndexX, info.IndexY);
                }
                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            // default starting sequence
            currentSequence = sequences["IdleSouth"];
        }

        /// <summary>
        /// Initialize the idle, walking, and running bounding shapes. These ones are all triangles...
        /// </summary>
        /// <param name="x">Y texture coordinate offset</param>
        private void InitializeBoundingShapes()
        {
            string monsterBounds = monsterName + "BoundingShapes";
            List<BoundingShapeInfo> shapesInfo = GameEngine.Content.Load<List<BoundingShapeInfo>>(@monsterBounds);

            foreach (BoundingShapeInfo info in shapesInfo)
            {
                if (info.StateKey == "Idle")
                {
                    boundingShapesSelf[info.StateKey + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf[info.StateKey + info.OrientationKey].ShapeColor = Color.Aqua;

                    boundingShapesSelf["Walking" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Walking" + info.OrientationKey].ShapeColor = Color.Magenta;

                    boundingShapesSelf["Running" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Running" + info.OrientationKey].ShapeColor = Color.Ivory;

                    boundingShapesSelf["Hit" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Hit" + info.OrientationKey].ShapeColor = Color.Red;

                    boundingShapesSelf["Dying" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Dying" + info.OrientationKey].ShapeColor = Color.Green;

                    boundingShapesSelf["Dead" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Dead" + info.OrientationKey].ShapeColor = Color.Black;


                }
                else if (info.StateKey == "Others")
                {
                    primitiveShape = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                }
                else
                {
                    boundingShapesSelf[info.StateKey + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf[info.StateKey + info.OrientationKey].ShapeColor = Color.Black;
                }
            }
        }

        #endregion // Initialization Methods

        #region Input Methods

        public void HandleStates()
        {
            switch (state)
            {
                case ActorState.Hit:
                    HitStateInput();
                    break;
                case ActorState.Dead:
                    DeadStateInput();
                    break;
                case ActorState.Dying:
                    DyingStateInput();
                    break;
            }
        }

        private void DeadStateInput()
        {
            foreach (Monster m in ((Level)Parent).MonsterList)
            {
                if (this.Equals((Monster)m))
                {
                    ((Level)Parent).MonsterList.Remove(this);
                    this.Dispose();
                    break;
                }
            }
        }

        private void DyingStateInput()
        {
            speed = 0.0f;
            if (currentSequence.IsComplete)
                state = ActorState.Dead;
        }

        private void HitStateInput()
        {
            speed = 0.0f;
            if (currentSequence.IsComplete)
                state = ActorState.Idle;
        }

        #endregion // Input methods

        #region Update Methods

        public void UpdateAI(GameTime gameTime)
        {
            foreach (Behaviors reactions in behaviors.Values)
            {
                foreach (Behavior reaction in reactions)
                {
                    reaction.Update(gameTime);
                    if (reaction.IsReacted)
                    {
                        reaction.Reset();
                    }
                }
            }
        }

        protected override void CheckBillboardBoundingBoxes(Vector3 oldPosition)
        {
            boundingShapesSelf[state.ToString() + orientation.ToString()].Update(position);

            // Check for collision against each players bounding shape
            foreach (Player p in ((Level)Parent).PlayerList)
            {
                if (IsHit(p.PrimitiveShape))
                {
                    position = oldPosition;
                }
            }
        }

        #endregion // Update Methods

        #region IAi Members

        protected Dictionary<ObjectType, Behaviors> behaviors;
        public Dictionary<ObjectType, Behaviors> Behaviors
        {
            get { return behaviors; }
        }

        public void React(Billboard otherObject, GameTime gameTime)
        {
            Behaviors reactions = Behaviors[otherObject.Type];

            foreach (Behavior reaction in reactions)
            {
                reaction.React(otherObject, gameTime);
            }
        }

        #endregion

        #region Damage Helper Methods

        public void ApplyDamage(int value)
        {
            actorStats.CurrentHealth -= value;
            if (actorStats.CurrentHealth <= 0)
                state = ActorState.Dying;
        }

        public void GetHit(int damage, Orientation o, float delta)
        {
            this.state = ActorState.Hit;
            this.orientation = Utility.GetOppositeOrientation(o);
            this.position = position + Utility.PositionChangeBasedOnOrientation(o, delta);
            currentSequence = sequences["Hit" + orientation.ToString()];
            this.ApplyDamage(damage);
        }

        #endregion // Damage Helper Methods
    }
}
