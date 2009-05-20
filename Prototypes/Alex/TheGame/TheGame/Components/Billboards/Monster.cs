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
        protected int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        protected int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        private bool hasBeenHit;
        public bool HasBeenHit
        {
            get { return hasBeenHit; }
            set { hasBeenHit = value; }
        }

        #region Constructor

        public Monster(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo, Vector3.UnitY)
        {
            behaviors = new Dictionary<ObjectType, Behaviors>();
            type = ObjectType.Monster;
            maxHealth = 20;
            currentHealth = maxHealth;
        }

        #endregion // Constructor

        #region Initialization

        public override void Initialize()
        {
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
            
            UpdateAI(gameTime);

            if (currentSequence.Title == "Hit" && currentSequence.IsComplete)
            {
                hasBeenHit = false;
            }

            base.Update(gameTime);

            HandleStates();

            if (currentHealth <= 0)
            {
                if (state != ActorState.Dying)
                    state = ActorState.Dying;
                else
                {
                    if (currentSequence.IsComplete == true)
                        state = ActorState.Dead;
                }
            }
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

        private void InitializeBehaviours()
        {
            Behavior wander = new WanderBehavior(Parent, this);
            Behaviors baseBehavior = new Behaviors();
            baseBehavior.Add(wander);

            behaviors.Add(ObjectType.None, baseBehavior);
        }

        private void InitializeSpriteSequences()
        {
            List<SpriteSequenceInfo> sequenceInfo = GameEngine.Content.Load<List<SpriteSequenceInfo>>(@"PoringSpriteSequences");
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
            List<BoundingShapeInfo> shapesInfo = GameEngine.Content.Load<List<BoundingShapeInfo>>(@"PlayerBoundingShapes");

            foreach (BoundingShapeInfo info in shapesInfo)
            {
                if (info.StateKey == "Idle")
                {
                    boundingShapesSelf[info.StateKey + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf[info.StateKey + info.OrientationKey].ShapeColor = Color.Aqua;

                    boundingShapesSelf["Walking" + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf["Walking" + info.OrientationKey].ShapeColor = Color.Magenta;

                    boundingShapesSelf["Running" + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf["Running" + info.OrientationKey].ShapeColor = Color.Ivory;

                    boundingShapesSelf["Hit" + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf["Hit" + info.OrientationKey].ShapeColor = Color.Red;

                    boundingShapesSelf["Dying" + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf["Dying" + info.OrientationKey].ShapeColor = Color.Green;

                    boundingShapesSelf["Dead" + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf["Dead" + info.OrientationKey].ShapeColor = Color.Black;


                }
                else if (info.StateKey == "Others")
                {
                    primitiveShape = new PrimitiveShape(position, scale, info.Verts);
                }
                else
                {
                    boundingShapesSelf[info.StateKey + info.OrientationKey] = new PrimitiveShape(position, scale, info.Verts);
                    boundingShapesSelf[info.StateKey + info.OrientationKey].ShapeColor = Color.Black;
                }
            }
        }

        #endregion // Initialization Methods

        #region Update Methods

        public void UpdateOrientation()
        {
            int num = GameEngine.Random.Next(8);

            switch (num)
            {
                case 0:
                    orientation = Orientation.South;
                    velocity = new Vector3(0.0f, 0.0f, -1.0f);
                    break;
                case 1:
                    orientation = Orientation.Southwest;
                    velocity = Vector3.Normalize(new Vector3(-1.0f, 0.0f, -1.0f));
                    break;
                case 2:
                    orientation = Orientation.West;
                    velocity = new Vector3(-1.0f, 0.0f, 0.0f);
                    break;
                case 3:
                    orientation = Orientation.Northwest;
                    velocity = Vector3.Normalize(new Vector3(-1.0f, 0.0f, 1.0f));
                    break;
                case 4:
                    orientation = Orientation.North;
                    velocity = new Vector3(0.0f, 0.0f, 1.0f);
                    break;
                case 5:
                    orientation = Orientation.Northeast;
                    velocity = Vector3.Normalize(new Vector3(1.0f, 0.0f, 1.0f));
                    break;
                case 6:
                    orientation = Orientation.East;
                    velocity = new Vector3(1.0f, 0.0f, 0.0f);
                    break;
                case 7:
                    orientation = Orientation.Southeast;
                    velocity = Vector3.Normalize(new Vector3(1.0f, 0.0f, -1.0f));
                    break;
            }
        }

        public void HandleStates()
        {
            if (state != previousState)
            {
                switch (state)
                {
                    case ActorState.Hit:
                        if(hasBeenHit == true)
                        {
                            currentSequence = sequences["Hit" + orientation.ToString()];
                            primitiveShape.ShapeColor = Color.Red;
                        }
                        break;
                    case ActorState.Dead:
                        foreach (Monster m in ((Level)Parent).MonsterList)
                        {
                            if (this.Equals((Monster)m))
                            {
                                ((Level)Parent).MonsterList.Remove(this);
                                this.Dispose();
                                break;
                            }
                        }
                        break;
                    case ActorState.Dying:
                        state = ActorState.Dead;
                        currentSequence = sequences["Dying" + orientation.ToString()];
                        
                        break;
                }
            }
        }

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

        public void ApplyDamage(int value)
        {
            currentHealth -= value;
        }

        public void GetHit(int damage, Orientation o, float delta)
        {
            hasBeenHit = true;
            this.primitiveShape.ShapeColor = Color.Red;
            this.state = ActorState.Hit;
            this.orientation = Utility.GetOppositeOrientation(o);
            this.position = position + Utility.PositionChangeBasedOnOrientation(o, delta);
            this.ApplyDamage(damage);
        }
    }
}
