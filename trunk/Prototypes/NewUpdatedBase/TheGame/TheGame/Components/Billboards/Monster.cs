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

        public override Actor.ActorState State
        {
            get { return base.State; }
            set
            {
                SetState(value);
            }
        }

        private void SetState(ActorState value)
        {
            switch (value)
            {
                case ActorState.Attacking:
                    if (state != ActorState.Hit && state != ActorState.Casting &&
                        state != ActorState.Chanting)
                        state = value;
                    break;
                case ActorState.Walking:
                    if (state != ActorState.Hit)
                        state = value;
                    break;
                case ActorState.Idle:
                    if (state != ActorState.Hit)
                        state = value;
                    break;
                case ActorState.Override:
                    if (state == ActorState.Stun)
                    {
                        state = ActorState.Idle;
                    }
                    break;
                default:
                    state = value;
                    break;
            }
        }

        public bool isDead = false;
        public float behaviorTimer = 0.0f;
        public float attackTimer = 2000.0f;
        public float stunTimer = 0.0f;
        public float stunDuration = 0.0f;
        public string monsterName;
        public bool isStunned = false;

        #endregion // Fields

        #region Constructor

        public Monster(GameScreen parent, SpriteInfo spriteInfo, Vector3 position, string monsterName)
            : base(parent, spriteInfo, position)
        {
            behaviors = new List<Behavior>();
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

            HandleStates(gameTime);

            base.Update(gameTime);
        }

        #endregion // Update

        #region Draw

        //public override void Draw(GameTime gameTime)
        //{
        //    boundingShapesSelf[state.ToString() + orientation.ToString()].Draw(primitiveBatch);
        //    primitiveShape.Draw(primitiveBatch);
        //    base.Draw(gameTime);
        //}

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
            behaviors.Add(wander);
            Behavior idle = new IdleBehavior(Parent, this);
            behaviors.Add(idle);
            Behavior seek = new SeekBehavior(Parent, this, 15.0f);
            behaviors.Add(seek);
            Behavior attack = new AttackBehavior(Parent, this, 3.2f);
            Behaviors.Add(attack);
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
            string monsterBounds = this.monsterName + "BoundingShapes";

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

                    boundingShapesSelf["Attacking" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Attacking" + info.OrientationKey].ShapeColor = Color.Black;

                    boundingShapesSelf["Hit" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Hit" + info.OrientationKey].ShapeColor = Color.Red;

                    boundingShapesSelf["Dying" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Dying" + info.OrientationKey].ShapeColor = Color.Green;

                    boundingShapesSelf["Dead" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Dead" + info.OrientationKey].ShapeColor = Color.Black;

                    boundingShapesSelf["Stun" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Stun" + info.OrientationKey].ShapeColor = Color.Black;
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

        public void HandleStates(GameTime gameTime)
        {
            if (hasBeenHit)
                state = ActorState.Hit;
            if (isStunned)
                state = ActorState.Stun;
            if (isDying)
                state = ActorState.Dying;
            if (isDead)
                state = ActorState.Dead;
            switch (state)
            {
                case ActorState.Stun:
                    StunStateInput(gameTime);
                    break;
                case ActorState.Attacking:
                    AttackingStateInput(gameTime);
                    break;
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

        private void StunStateInput(GameTime gameTime)
        {
            speed = 0.0f;
            stunTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (stunTimer >= stunDuration)
            {
                State = ActorState.Override;
                stunTimer = 0.0f;
                isStunned = false;
            }
        }

        private void AttackingStateInput(GameTime gameTime)
        {
            attackTimer -= gameTime.ElapsedGameTime.Milliseconds;
            speed = 0.0f;
            if (attackTimer <= 0.0f)
            {
                if (currentSequence.Title == "Attacking" && ((currentSequence.CurrentFrame.X == 4
                 || currentSequence.CurrentFrame.X == 12)) && !hasAttacked)
                {
                    foreach (Player p in ((Level)Parent).PlayerList)
                    {
                        if (IsHit(p.PrimitiveShape))
                        {
                            playerTarget = p;
                            playerTarget.PrimitiveShape.ShapeColor = Color.Blue;
                        }
                    }
                    if (playerTarget != null)
                    {
                        playerTarget.PhysicalHit(this.monsterStats.Damage, orientation);
                        this.hasAttacked = true;
                        attackTimer = 500.0f;
                    }
                }
            }

            if (attackTimer >= 500.0f)
            {
                if (playerTarget != null)
                {
                    hasAttacked = false;
                    playerTarget.PrimitiveShape.ShapeColor = Color.White;
                    playerTarget = null;
                    state = ActorState.Idle;
                }
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
            {
                isDead = true;
                isDying = false;
            }
        }

        private void HitStateInput()
        {
            speed = 0.0f;
            if (currentSequence.IsComplete)
            {
                hasBeenHit = false;
                state = ActorState.Idle;
                Color = Color.White;
            }
        }

        #endregion // Input methods

        #region Update Methods

        public void UpdateAI(GameTime gameTime)
        {
            behaviorTimer += (float)gameTime.ElapsedGameTime.Milliseconds;
            int desireTotal = 0;
            int runningTotal = 0;
            int prevTotal = 0;

            reactions = new List<Behavior>();

            foreach (Behavior behavior in behaviors)
            {
                if (behaviorTimer >= behavior.CurrentTimeInterval || behavior.UpdateTimeInterval == 0.0f)
                {
                    behavior.React(gameTime);
                    behavior.CurrentTimeInterval += behavior.UpdateTimeInterval;
                    if (behavior.IsReacted)
                    {
                        desireTotal += behavior.DesireLevel;
                        reactions.Add(behavior);
                        behavior.Reset();
                    }
                }
            }

            int num = GameEngine.Random.Next(desireTotal) + 1;

            foreach (Behavior reaction in reactions)
            {
                prevTotal = runningTotal;
                runningTotal += reaction.DesireLevel;
                if (num > prevTotal && num <= runningTotal)
                {
                    reaction.Update(gameTime);
                    break;
                }
            }

            if (behaviorTimer == 10000.0f)
            {
                foreach (Behavior behavior in behaviors)
                {
                    behavior.ResetTimeInterval();
                }
                behaviorTimer = 0.0f;
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

        protected List<Behavior> behaviors;
        public List<Behavior> Behaviors
        {
            get { return behaviors; }
        }

        protected List<Behavior> reactions;
        #endregion

        public override void ApplyDamage(int damage)
        {
            actorStats.CurrentHealth -= damage;
            if (actorStats.CurrentHealth <= 0)
                this.isDying = true;
        }

        public void PhysicalHit(int damage, Orientation o)
        {
            hasBeenHit = true;
            this.Color = Color.Red;
            this.orientation = Utility.GetOppositeOrientation(o);
            this.position = position + Utility.PositionChangeBasedOnOrientation(o, 0.5f);
            this.ApplyDamage(damage);
        }
    }
}
