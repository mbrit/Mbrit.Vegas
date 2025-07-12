namespace Mbrit.Vegas
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Data;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using BootFX.Common;
    using BootFX.Common.Dto;
    using BootFX.Common.Data;
    using BootFX.Common.Entities;
    using BootFX.Common.Entities.Attributes;
    
    
    /// <summary>
    /// Defines the base entity type for 'GameRunHands'.
    /// </summary>
    [Serializable()]
    public abstract class GameRunHandBase : BootFX.Common.Entities.Entity
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        protected GameRunHandBase()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected GameRunHandBase(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// <summary>
        /// Gets or sets the value for 'GameRunHandId'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'GameRunHandId' column.
        /// </remarks>
        [EntityField("GameRunHandId", System.Data.DbType.Int32, ((BootFX.Common.Entities.EntityFieldFlags.Key | BootFX.Common.Entities.EntityFieldFlags.Common) 
                    | BootFX.Common.Entities.EntityFieldFlags.AutoIncrement))]
        public int GameRunHandId
        {
            get
            {
                return ((int)(this["GameRunHandId"]));
            }
            set
            {
                this["GameRunHandId"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'GameRunId'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'GameRunId' column.
        /// </remarks>
        [EntityField("GameRunId", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        [DBNullEquivalent(0)]
        public int GameRunId
        {
            get
            {
                return ((int)(this["GameRunId"]));
            }
            set
            {
                this["GameRunId"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Hand'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Hand' column.
        /// </remarks>
        [EntityField("Hand", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int Hand
        {
            get
            {
                return ((int)(this["Hand"]));
            }
            set
            {
                this["Hand"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Outcome'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Outcome' column.
        /// </remarks>
        [EntityField("Outcome", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public Mbrit.Vegas.Simulator.WinLoseDrawType Outcome
        {
            get
            {
                return ((Mbrit.Vegas.Simulator.WinLoseDrawType)(this["Outcome"]));
            }
            set
            {
                this["Outcome"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'DateTimeUtc'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'DateTimeUtc' column.
        /// </remarks>
        [EntityField("DateTimeUtc", System.Data.DbType.DateTime, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public System.DateTime DateTimeUtc
        {
            get
            {
                return ((System.DateTime)(this["DateTimeUtc"]));
            }
            set
            {
                this["DateTimeUtc"] = value;
            }
        }
        
        /// <summary>
        /// Gets the link to 'GameRun'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'FK_GameRunHands_GameRuns' constraint.
        /// </remarks>
        [EntityLinkToParent("GameRun", "FK_GameRunHands_GameRuns", typeof(GameRun), new string[] {
                "GameRunId"})]
        public GameRun GameRun
        {
            get
            {
                return ((GameRun)(this.GetParent("GameRun")));
            }
            set
            {
                this.SetParent("GameRun", value);
            }
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'GameRunHand'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter CreateFilterBase()
        {
            return new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'GameRunHand'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter<GameRunHand> CreateFilter()
        {
            return new BootFX.Common.Data.SqlFilter<GameRunHand>();
        }
        
        /// <summary>
        /// Get all <see cref="GameRunHand"/> entities.
        /// </summary>
        public static GameRunHandCollection GetAll()
        {
            BootFX.Common.Data.SqlFilter filter = BootFX.Common.Data.SqlFilter.CreateGetAllFilter(typeof(GameRunHand));
            return ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets the <see cref="GameRunHand"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRunHand
        /// </bootfx>
        public static GameRunHand GetById(int gameRunHandId)
        {
            return Mbrit.Vegas.GameRunHand.GetById(gameRunHandId, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets the <see cref="GameRunHand"/> entity where the ID matches the given specification.
        /// </summary>
        public static GameRunHand GetById(int gameRunHandId, BootFX.Common.Data.SqlOperator gameRunHandIdOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("GameRunHandId", gameRunHandIdOperator, gameRunHandId);
            return ((GameRunHand)(filter.ExecuteEntity()));
        }
        
        /// <summary>
        /// Gets the <see cref="GameRunHand"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRunHand
        /// </bootfx>
        public static GameRunHand GetById(int gameRunHandId, BootFX.Common.OnNotFound onNotFound)
        {
            return Mbrit.Vegas.GameRunHand.GetById(gameRunHandId, BootFX.Common.Data.SqlOperator.EqualTo, onNotFound);
        }
        
        /// <summary>
        /// Gets the <see cref="GameRunHand"/> entity where the ID matches the given specification.
        /// </summary>
        public static GameRunHand GetById(int gameRunHandId, BootFX.Common.Data.SqlOperator gameRunHandIdOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("GameRunHandId", gameRunHandIdOperator, gameRunHandId);
            GameRunHand results = ((GameRunHand)(filter.ExecuteEntity()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where GameRunId is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRunHand
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>GameRunId</c>
        /// </remarks>
        public static GameRunHandCollection GetByGameRunId(int gameRunId)
        {
            return Mbrit.Vegas.GameRunHand.GetByGameRunId(gameRunId, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where GameRunId matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>GameRunId</c>
        /// </remarks>
        public static GameRunHandCollection GetByGameRunId(int gameRunId, BootFX.Common.Data.SqlOperator gameRunIdOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("GameRunId", gameRunIdOperator, gameRunId);
            return ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where GameRunId matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>GameRunId</c>
        /// </remarks>
        public static GameRunHandCollection GetByGameRunId(int gameRunId, BootFX.Common.Data.SqlOperator gameRunIdOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("GameRunId", gameRunIdOperator, gameRunId);
            GameRunHandCollection results = ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Hand is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRunHand
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Hand</c>
        /// </remarks>
        public static GameRunHandCollection GetByHand(int hand)
        {
            return Mbrit.Vegas.GameRunHand.GetByHand(hand, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Hand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Hand</c>
        /// </remarks>
        public static GameRunHandCollection GetByHand(int hand, BootFX.Common.Data.SqlOperator handOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("Hand", handOperator, hand);
            return ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Hand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Hand</c>
        /// </remarks>
        public static GameRunHandCollection GetByHand(int hand, BootFX.Common.Data.SqlOperator handOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("Hand", handOperator, hand);
            GameRunHandCollection results = ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Outcome is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRunHand
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Outcome</c>
        /// </remarks>
        public static GameRunHandCollection GetByOutcome(Mbrit.Vegas.Simulator.WinLoseDrawType outcome)
        {
            return Mbrit.Vegas.GameRunHand.GetByOutcome(outcome, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Outcome matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Outcome</c>
        /// </remarks>
        public static GameRunHandCollection GetByOutcome(Mbrit.Vegas.Simulator.WinLoseDrawType outcome, BootFX.Common.Data.SqlOperator outcomeOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("Outcome", outcomeOperator, outcome);
            return ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Outcome matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Outcome</c>
        /// </remarks>
        public static GameRunHandCollection GetByOutcome(Mbrit.Vegas.Simulator.WinLoseDrawType outcome, BootFX.Common.Data.SqlOperator outcomeOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("Outcome", outcomeOperator, outcome);
            GameRunHandCollection results = ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where DateTimeUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRunHand
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>DateTimeUtc</c>
        /// </remarks>
        public static GameRunHandCollection GetByDateTimeUtc(System.DateTime dateTimeUtc)
        {
            return Mbrit.Vegas.GameRunHand.GetByDateTimeUtc(dateTimeUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where DateTimeUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>DateTimeUtc</c>
        /// </remarks>
        public static GameRunHandCollection GetByDateTimeUtc(System.DateTime dateTimeUtc, BootFX.Common.Data.SqlOperator dateTimeUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("DateTimeUtc", dateTimeUtcOperator, dateTimeUtc);
            return ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where DateTimeUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>DateTimeUtc</c>
        /// </remarks>
        public static GameRunHandCollection GetByDateTimeUtc(System.DateTime dateTimeUtc, BootFX.Common.Data.SqlOperator dateTimeUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("DateTimeUtc", dateTimeUtcOperator, dateTimeUtc);
            GameRunHandCollection results = ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
    }
}


/*
***** SqlServerDialect *****
CREATE TABLE [GameRunHands] (
	[GameRunHandId] int NOT NULL IDENTITY(1,1), 
	[GameRunId] int NOT NULL, 
	[Hand] int NOT NULL, 
	[Outcome] int NOT NULL, 
	[DateTimeUtc] datetime NOT NULL
	);
ALTER TABLE [GameRunHands] ADD CONSTRAINT [PK_GameRunHands] PRIMARY KEY ([GameRunHandId]);

***** PostgresDialect *****
CREATE TABLE "GameRunHands" (
	"GameRunHandId" serial, 
	"GameRunId" int NOT NULL, 
	"Hand" int NOT NULL, 
	"Outcome" int NOT NULL, 
	"DateTimeUtc" timestamp NOT NULL
	);
ALTER TABLE "GameRunHands" ADD CONSTRAINT "PK_GameRunHands" PRIMARY KEY ("GameRunHandId");

***** MySqlDialect *****
CREATE TABLE `GameRunHands` (
	`GameRunHandId` int NOT NULL AUTO_INCREMENT, 
	`GameRunId` int NOT NULL, 
	`Hand` int NOT NULL, 
	`Outcome` int NOT NULL, 
	`DateTimeUtc` datetime NOT NULL
	, PRIMARY KEY (`GameRunHandId`));


*/
