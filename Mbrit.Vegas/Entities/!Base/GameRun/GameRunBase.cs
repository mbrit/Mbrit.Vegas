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
    /// Defines the base entity type for 'GameRuns'.
    /// </summary>
    [Serializable()]
    [Index("GameRuns_Token", "GameRuns_Token", true, "Token")]
    public abstract class GameRunBase : BootFX.Common.Entities.Entity
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        protected GameRunBase()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected GameRunBase(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// <summary>
        /// Gets or sets the value for 'GameRunId'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'GameRunId' column.
        /// </remarks>
        [EntityField("GameRunId", System.Data.DbType.Int32, ((BootFX.Common.Entities.EntityFieldFlags.Key | BootFX.Common.Entities.EntityFieldFlags.Common) 
                    | BootFX.Common.Entities.EntityFieldFlags.AutoIncrement))]
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
        /// Gets or sets the value for 'CreatedUtc'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'CreatedUtc' column.
        /// </remarks>
        [EntityField("CreatedUtc", System.Data.DbType.DateTime, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public System.DateTime CreatedUtc
        {
            get
            {
                return ((System.DateTime)(this["CreatedUtc"]));
            }
            set
            {
                this["CreatedUtc"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Token'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Token' column.
        /// </remarks>
        [EntityField("Token", System.Data.DbType.String, BootFX.Common.Entities.EntityFieldFlags.Common, 64)]
        public string Token
        {
            get
            {
                return ((string)(this["Token"]));
            }
            set
            {
                this["Token"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Unit'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Unit' column.
        /// </remarks>
        [EntityField("Unit", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int Unit
        {
            get
            {
                return ((int)(this["Unit"]));
            }
            set
            {
                this["Unit"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Mode'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Mode' column.
        /// </remarks>
        [EntityField("Mode", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public Mbrit.Vegas.Simulator.WalkGameMode Mode
        {
            get
            {
                return ((Mbrit.Vegas.Simulator.WalkGameMode)(this["Mode"]));
            }
            set
            {
                this["Mode"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'HailMaryCount'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'HailMaryCount' column.
        /// </remarks>
        [EntityField("HailMaryCount", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int HailMaryCount
        {
            get
            {
                return ((int)(this["HailMaryCount"]));
            }
            set
            {
                this["HailMaryCount"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Name'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Name' column.
        /// </remarks>
        [EntityField("Name", System.Data.DbType.String, BootFX.Common.Entities.EntityFieldFlags.Common, 128)]
        public string Name
        {
            get
            {
                return ((string)(this["Name"]));
            }
            set
            {
                this["Name"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Hands'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Hands' column.
        /// </remarks>
        [EntityField("Hands", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int Hands
        {
            get
            {
                return ((int)(this["Hands"]));
            }
            set
            {
                this["Hands"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Investables'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Investables' column.
        /// </remarks>
        [EntityField("Investables", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int Investables
        {
            get
            {
                return ((int)(this["Investables"]));
            }
            set
            {
                this["Investables"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'HouseEdge'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'HouseEdge' column.
        /// </remarks>
        [EntityField("HouseEdge", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal HouseEdge
        {
            get
            {
                return ((decimal)(this["HouseEdge"]));
            }
            set
            {
                this["HouseEdge"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'IsWaitingOnDecision'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'IsWaitingOnDecision' column.
        /// </remarks>
        [EntityField("IsWaitingOnDecision", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        [DatabaseDefault(BootFX.Common.Data.Schema.SqlDatabaseDefaultType.Primitive, 0)]
        public bool IsWaitingOnDecision
        {
            get
            {
                return ((bool)(this["IsWaitingOnDecision"]));
            }
            set
            {
                this["IsWaitingOnDecision"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'WaitingOnDecisionUtc'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'WaitingOnDecisionUtc' column.
        /// </remarks>
        [EntityField("WaitingOnDecisionUtc", System.Data.DbType.DateTime, (BootFX.Common.Entities.EntityFieldFlags.Nullable | BootFX.Common.Entities.EntityFieldFlags.Common))]
        public System.DateTime WaitingOnDecisionUtc
        {
            get
            {
                return ((System.DateTime)(this["WaitingOnDecisionUtc"]));
            }
            set
            {
                this["WaitingOnDecisionUtc"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'LastPlayedUtc'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'LastPlayedUtc' column.
        /// </remarks>
        [EntityField("LastPlayedUtc", System.Data.DbType.DateTime, (BootFX.Common.Entities.EntityFieldFlags.Nullable | BootFX.Common.Entities.EntityFieldFlags.Common))]
        public System.DateTime LastPlayedUtc
        {
            get
            {
                return ((System.DateTime)(this["LastPlayedUtc"]));
            }
            set
            {
                this["LastPlayedUtc"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'IsAbandoned'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'IsAbandoned' column.
        /// </remarks>
        [EntityField("IsAbandoned", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        [DatabaseDefault(BootFX.Common.Data.Schema.SqlDatabaseDefaultType.Primitive, 0)]
        public bool IsAbandoned
        {
            get
            {
                return ((bool)(this["IsAbandoned"]));
            }
            set
            {
                this["IsAbandoned"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'AbandonedUtc'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'AbandonedUtc' column.
        /// </remarks>
        [EntityField("AbandonedUtc", System.Data.DbType.DateTime, (BootFX.Common.Entities.EntityFieldFlags.Nullable | BootFX.Common.Entities.EntityFieldFlags.Common))]
        public System.DateTime AbandonedUtc
        {
            get
            {
                return ((System.DateTime)(this["AbandonedUtc"]));
            }
            set
            {
                this["AbandonedUtc"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'ActionUnits'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'ActionUnits' column.
        /// </remarks>
        [EntityField("ActionUnits", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        [DatabaseDefault(BootFX.Common.Data.Schema.SqlDatabaseDefaultType.Primitive, 0)]
        public int ActionUnits
        {
            get
            {
                return ((int)(this["ActionUnits"]));
            }
            set
            {
                this["ActionUnits"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Action'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Action' column.
        /// </remarks>
        [EntityField("Action", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        [DatabaseDefault(BootFX.Common.Data.Schema.SqlDatabaseDefaultType.Primitive, 0)]
        public Mbrit.Vegas.Simulator.WalkGameAction Action
        {
            get
            {
                return ((Mbrit.Vegas.Simulator.WalkGameAction)(this["Action"]));
            }
            set
            {
                this["Action"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'ActionUtc'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'ActionUtc' column.
        /// </remarks>
        [EntityField("ActionUtc", System.Data.DbType.DateTime, (BootFX.Common.Entities.EntityFieldFlags.Nullable | BootFX.Common.Entities.EntityFieldFlags.Common))]
        public System.DateTime ActionUtc
        {
            get
            {
                return ((System.DateTime)(this["ActionUtc"]));
            }
            set
            {
                this["ActionUtc"] = value;
            }
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'GameRun'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter CreateFilterBase()
        {
            return new BootFX.Common.Data.SqlFilter(typeof(GameRun));
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'GameRun'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter<GameRun> CreateFilter()
        {
            return new BootFX.Common.Data.SqlFilter<GameRun>();
        }
        
        /// <summary>
        /// Get all <see cref="GameRun"/> entities.
        /// </summary>
        public static GameRunCollection GetAll()
        {
            BootFX.Common.Data.SqlFilter filter = BootFX.Common.Data.SqlFilter.CreateGetAllFilter(typeof(GameRun));
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets the <see cref="GameRun"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        public static GameRun GetById(int gameRunId)
        {
            return Mbrit.Vegas.GameRun.GetById(gameRunId, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets the <see cref="GameRun"/> entity where the ID matches the given specification.
        /// </summary>
        public static GameRun GetById(int gameRunId, BootFX.Common.Data.SqlOperator gameRunIdOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("GameRunId", gameRunIdOperator, gameRunId);
            return ((GameRun)(filter.ExecuteEntity()));
        }
        
        /// <summary>
        /// Gets the <see cref="GameRun"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        public static GameRun GetById(int gameRunId, BootFX.Common.OnNotFound onNotFound)
        {
            return Mbrit.Vegas.GameRun.GetById(gameRunId, BootFX.Common.Data.SqlOperator.EqualTo, onNotFound);
        }
        
        /// <summary>
        /// Gets the <see cref="GameRun"/> entity where the ID matches the given specification.
        /// </summary>
        public static GameRun GetById(int gameRunId, BootFX.Common.Data.SqlOperator gameRunIdOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("GameRunId", gameRunIdOperator, gameRunId);
            GameRun results = ((GameRun)(filter.ExecuteEntity()));
            return results;
        }
        
        /// <summary>
        /// Gets an entity where Token is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for index <c>GameRuns_Token</c>.
        /// </remarks>
        public static GameRun GetByToken(string token)
        {
            return Mbrit.Vegas.GameRun.GetByToken(token, BootFX.Common.OnNotFound.ReturnNull);
        }
        
        /// <summary>
        /// Gets an entity where Token is equal to the given value.
        /// </summary>
        /// <remarks>
        /// Created for index <c>GameRuns_Token</c>.
        /// </remarks>
        public static GameRun GetByToken(string token, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Token", BootFX.Common.Data.SqlOperator.EqualTo, token);
            GameRun results = ((GameRun)(filter.ExecuteEntity()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where CreatedUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>CreatedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByCreatedUtc(System.DateTime createdUtc)
        {
            return Mbrit.Vegas.GameRun.GetByCreatedUtc(createdUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where CreatedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>CreatedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByCreatedUtc(System.DateTime createdUtc, BootFX.Common.Data.SqlOperator createdUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("CreatedUtc", createdUtcOperator, createdUtc);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where CreatedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>CreatedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByCreatedUtc(System.DateTime createdUtc, BootFX.Common.Data.SqlOperator createdUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("CreatedUtc", createdUtcOperator, createdUtc);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Unit is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Unit</c>
        /// </remarks>
        public static GameRunCollection GetByUnit(int unit)
        {
            return Mbrit.Vegas.GameRun.GetByUnit(unit, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Unit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Unit</c>
        /// </remarks>
        public static GameRunCollection GetByUnit(int unit, BootFX.Common.Data.SqlOperator unitOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Unit", unitOperator, unit);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Unit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Unit</c>
        /// </remarks>
        public static GameRunCollection GetByUnit(int unit, BootFX.Common.Data.SqlOperator unitOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Unit", unitOperator, unit);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Mode is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Mode</c>
        /// </remarks>
        public static GameRunCollection GetByMode(Mbrit.Vegas.Simulator.WalkGameMode mode)
        {
            return Mbrit.Vegas.GameRun.GetByMode(mode, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Mode matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Mode</c>
        /// </remarks>
        public static GameRunCollection GetByMode(Mbrit.Vegas.Simulator.WalkGameMode mode, BootFX.Common.Data.SqlOperator modeOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Mode", modeOperator, mode);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Mode matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Mode</c>
        /// </remarks>
        public static GameRunCollection GetByMode(Mbrit.Vegas.Simulator.WalkGameMode mode, BootFX.Common.Data.SqlOperator modeOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Mode", modeOperator, mode);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where HailMaryCount is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>HailMaryCount</c>
        /// </remarks>
        public static GameRunCollection GetByHailMaryCount(int hailMaryCount)
        {
            return Mbrit.Vegas.GameRun.GetByHailMaryCount(hailMaryCount, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where HailMaryCount matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>HailMaryCount</c>
        /// </remarks>
        public static GameRunCollection GetByHailMaryCount(int hailMaryCount, BootFX.Common.Data.SqlOperator hailMaryCountOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("HailMaryCount", hailMaryCountOperator, hailMaryCount);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where HailMaryCount matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>HailMaryCount</c>
        /// </remarks>
        public static GameRunCollection GetByHailMaryCount(int hailMaryCount, BootFX.Common.Data.SqlOperator hailMaryCountOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("HailMaryCount", hailMaryCountOperator, hailMaryCount);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Name is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Name</c>
        /// </remarks>
        public static GameRunCollection GetByName(string name)
        {
            return Mbrit.Vegas.GameRun.GetByName(name, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Name matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Name</c>
        /// </remarks>
        public static GameRunCollection GetByName(string name, BootFX.Common.Data.SqlOperator nameOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Name", nameOperator, name);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Name matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Name</c>
        /// </remarks>
        public static GameRunCollection GetByName(string name, BootFX.Common.Data.SqlOperator nameOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Name", nameOperator, name);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Hands is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Hands</c>
        /// </remarks>
        public static GameRunCollection GetByHands(int hands)
        {
            return Mbrit.Vegas.GameRun.GetByHands(hands, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Hands matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Hands</c>
        /// </remarks>
        public static GameRunCollection GetByHands(int hands, BootFX.Common.Data.SqlOperator handsOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Hands", handsOperator, hands);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Hands matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Hands</c>
        /// </remarks>
        public static GameRunCollection GetByHands(int hands, BootFX.Common.Data.SqlOperator handsOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Hands", handsOperator, hands);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Investables is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Investables</c>
        /// </remarks>
        public static GameRunCollection GetByInvestables(int investables)
        {
            return Mbrit.Vegas.GameRun.GetByInvestables(investables, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Investables matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Investables</c>
        /// </remarks>
        public static GameRunCollection GetByInvestables(int investables, BootFX.Common.Data.SqlOperator investablesOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Investables", investablesOperator, investables);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Investables matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Investables</c>
        /// </remarks>
        public static GameRunCollection GetByInvestables(int investables, BootFX.Common.Data.SqlOperator investablesOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Investables", investablesOperator, investables);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where HouseEdge is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>HouseEdge</c>
        /// </remarks>
        public static GameRunCollection GetByHouseEdge(decimal houseEdge)
        {
            return Mbrit.Vegas.GameRun.GetByHouseEdge(houseEdge, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where HouseEdge matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>HouseEdge</c>
        /// </remarks>
        public static GameRunCollection GetByHouseEdge(decimal houseEdge, BootFX.Common.Data.SqlOperator houseEdgeOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("HouseEdge", houseEdgeOperator, houseEdge);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where HouseEdge matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>HouseEdge</c>
        /// </remarks>
        public static GameRunCollection GetByHouseEdge(decimal houseEdge, BootFX.Common.Data.SqlOperator houseEdgeOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("HouseEdge", houseEdgeOperator, houseEdge);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where IsWaitingOnDecision is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>IsWaitingOnDecision</c>
        /// </remarks>
        public static GameRunCollection GetByIsWaitingOnDecision(bool isWaitingOnDecision)
        {
            return Mbrit.Vegas.GameRun.GetByIsWaitingOnDecision(isWaitingOnDecision, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where IsWaitingOnDecision matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>IsWaitingOnDecision</c>
        /// </remarks>
        public static GameRunCollection GetByIsWaitingOnDecision(bool isWaitingOnDecision, BootFX.Common.Data.SqlOperator isWaitingOnDecisionOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("IsWaitingOnDecision", isWaitingOnDecisionOperator, isWaitingOnDecision);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where IsWaitingOnDecision matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>IsWaitingOnDecision</c>
        /// </remarks>
        public static GameRunCollection GetByIsWaitingOnDecision(bool isWaitingOnDecision, BootFX.Common.Data.SqlOperator isWaitingOnDecisionOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("IsWaitingOnDecision", isWaitingOnDecisionOperator, isWaitingOnDecision);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where WaitingOnDecisionUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>WaitingOnDecisionUtc</c>
        /// </remarks>
        public static GameRunCollection GetByWaitingOnDecisionUtc(System.DateTime waitingOnDecisionUtc)
        {
            return Mbrit.Vegas.GameRun.GetByWaitingOnDecisionUtc(waitingOnDecisionUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where WaitingOnDecisionUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>WaitingOnDecisionUtc</c>
        /// </remarks>
        public static GameRunCollection GetByWaitingOnDecisionUtc(System.DateTime waitingOnDecisionUtc, BootFX.Common.Data.SqlOperator waitingOnDecisionUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("WaitingOnDecisionUtc", waitingOnDecisionUtcOperator, waitingOnDecisionUtc);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where WaitingOnDecisionUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>WaitingOnDecisionUtc</c>
        /// </remarks>
        public static GameRunCollection GetByWaitingOnDecisionUtc(System.DateTime waitingOnDecisionUtc, BootFX.Common.Data.SqlOperator waitingOnDecisionUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("WaitingOnDecisionUtc", waitingOnDecisionUtcOperator, waitingOnDecisionUtc);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where LastPlayedUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>LastPlayedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByLastPlayedUtc(System.DateTime lastPlayedUtc)
        {
            return Mbrit.Vegas.GameRun.GetByLastPlayedUtc(lastPlayedUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where LastPlayedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>LastPlayedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByLastPlayedUtc(System.DateTime lastPlayedUtc, BootFX.Common.Data.SqlOperator lastPlayedUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("LastPlayedUtc", lastPlayedUtcOperator, lastPlayedUtc);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where LastPlayedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>LastPlayedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByLastPlayedUtc(System.DateTime lastPlayedUtc, BootFX.Common.Data.SqlOperator lastPlayedUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("LastPlayedUtc", lastPlayedUtcOperator, lastPlayedUtc);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where IsAbandoned is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>IsAbandoned</c>
        /// </remarks>
        public static GameRunCollection GetByIsAbandoned(bool isAbandoned)
        {
            return Mbrit.Vegas.GameRun.GetByIsAbandoned(isAbandoned, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where IsAbandoned matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>IsAbandoned</c>
        /// </remarks>
        public static GameRunCollection GetByIsAbandoned(bool isAbandoned, BootFX.Common.Data.SqlOperator isAbandonedOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("IsAbandoned", isAbandonedOperator, isAbandoned);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where IsAbandoned matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>IsAbandoned</c>
        /// </remarks>
        public static GameRunCollection GetByIsAbandoned(bool isAbandoned, BootFX.Common.Data.SqlOperator isAbandonedOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("IsAbandoned", isAbandonedOperator, isAbandoned);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where AbandonedUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>AbandonedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByAbandonedUtc(System.DateTime abandonedUtc)
        {
            return Mbrit.Vegas.GameRun.GetByAbandonedUtc(abandonedUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where AbandonedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>AbandonedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByAbandonedUtc(System.DateTime abandonedUtc, BootFX.Common.Data.SqlOperator abandonedUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("AbandonedUtc", abandonedUtcOperator, abandonedUtc);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where AbandonedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>AbandonedUtc</c>
        /// </remarks>
        public static GameRunCollection GetByAbandonedUtc(System.DateTime abandonedUtc, BootFX.Common.Data.SqlOperator abandonedUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("AbandonedUtc", abandonedUtcOperator, abandonedUtc);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where ActionUnits is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>ActionUnits</c>
        /// </remarks>
        public static GameRunCollection GetByActionUnits(int actionUnits)
        {
            return Mbrit.Vegas.GameRun.GetByActionUnits(actionUnits, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where ActionUnits matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>ActionUnits</c>
        /// </remarks>
        public static GameRunCollection GetByActionUnits(int actionUnits, BootFX.Common.Data.SqlOperator actionUnitsOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("ActionUnits", actionUnitsOperator, actionUnits);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where ActionUnits matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>ActionUnits</c>
        /// </remarks>
        public static GameRunCollection GetByActionUnits(int actionUnits, BootFX.Common.Data.SqlOperator actionUnitsOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("ActionUnits", actionUnitsOperator, actionUnits);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Action is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Action</c>
        /// </remarks>
        public static GameRunCollection GetByAction(Mbrit.Vegas.Simulator.WalkGameAction action)
        {
            return Mbrit.Vegas.GameRun.GetByAction(action, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Action matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Action</c>
        /// </remarks>
        public static GameRunCollection GetByAction(Mbrit.Vegas.Simulator.WalkGameAction action, BootFX.Common.Data.SqlOperator actionOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Action", actionOperator, action);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Action matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Action</c>
        /// </remarks>
        public static GameRunCollection GetByAction(Mbrit.Vegas.Simulator.WalkGameAction action, BootFX.Common.Data.SqlOperator actionOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("Action", actionOperator, action);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where ActionUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - GameRun
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>ActionUtc</c>
        /// </remarks>
        public static GameRunCollection GetByActionUtc(System.DateTime actionUtc)
        {
            return Mbrit.Vegas.GameRun.GetByActionUtc(actionUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where ActionUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>ActionUtc</c>
        /// </remarks>
        public static GameRunCollection GetByActionUtc(System.DateTime actionUtc, BootFX.Common.Data.SqlOperator actionUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("ActionUtc", actionUtcOperator, actionUtc);
            return ((GameRunCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where ActionUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>ActionUtc</c>
        /// </remarks>
        public static GameRunCollection GetByActionUtc(System.DateTime actionUtc, BootFX.Common.Data.SqlOperator actionUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRun));
            filter.Constraints.Add("ActionUtc", actionUtcOperator, actionUtc);
            GameRunCollection results = ((GameRunCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Get all of the child 'GameRunHand' entities.
        /// </summary>
        /// <remarks>
        /// Created for link <c>GameRun</c>.  (Stub method.)
        /// </remarks>
        public GameRunHandCollection GetGameRunHandItems()
        {
            // defer...
            return GameRun.GetGameRunHandItems(this.GameRunId);
        }
        
        /// <summary>
        /// Get all of the child 'GameRunHand' entities.
        /// </summary>
        /// <remarks>
        /// Created for link <c>GameRun</c>.  (Concrete method.)
        /// </remarks>
        public static GameRunHandCollection GetGameRunHandItems(int gameRunId)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(GameRunHand));
            filter.Constraints.Add("GameRunId", BootFX.Common.Data.SqlOperator.EqualTo, gameRunId);
            return ((GameRunHandCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Returns the value in the 'Name' property.
        /// </summary>
        public override string ToString()
        {
            return this.Name;
        }
    }
}


/*
***** SqlServerDialect *****
CREATE TABLE [GameRuns] (
	[GameRunId] int NOT NULL IDENTITY(1,1), 
	[CreatedUtc] datetime NOT NULL, 
	[Token] nvarchar(64) NOT NULL, 
	[Unit] int NOT NULL, 
	[Mode] int NOT NULL, 
	[HailMaryCount] int NOT NULL, 
	[Name] nvarchar(128) NOT NULL, 
	[Hands] int NOT NULL, 
	[Investables] int NOT NULL, 
	[HouseEdge] decimal(18, 5) NOT NULL, 
	[IsWaitingOnDecision] bit NOT NULL, 
	[WaitingOnDecisionUtc] datetime NULL, 
	[LastPlayedUtc] datetime NULL, 
	[IsAbandoned] bit NOT NULL, 
	[AbandonedUtc] datetime NULL, 
	[ActionUnits] int NOT NULL, 
	[Action] int NOT NULL, 
	[ActionUtc] datetime NULL
	);
ALTER TABLE [GameRuns] ADD CONSTRAINT [PK_GameRuns] PRIMARY KEY ([GameRunId]);
ALTER TABLE [GameRuns]  ADD  DEFAULT (0) FOR [IsWaitingOnDecision]
ALTER TABLE [GameRuns]  ADD  DEFAULT (0) FOR [IsAbandoned]
ALTER TABLE [GameRuns]  ADD  DEFAULT (0) FOR [ActionUnits]
ALTER TABLE [GameRuns]  ADD  DEFAULT (0) FOR [Action]
CREATE UNIQUE INDEX [GameRuns_Token] ON [GameRuns] (
	[Token]
	) WITH (FILLFACTOR=90)

***** PostgresDialect *****
CREATE TABLE "GameRuns" (
	"GameRunId" serial, 
	"CreatedUtc" timestamp NOT NULL, 
	"Token" varchar(64) collate english_ci NOT NULL, 
	"Unit" int NOT NULL, 
	"Mode" int NOT NULL, 
	"HailMaryCount" int NOT NULL, 
	"Name" varchar(128) collate english_ci NOT NULL, 
	"Hands" int NOT NULL, 
	"Investables" int NOT NULL, 
	"HouseEdge" decimal(18, 5) NOT NULL, 
	"IsWaitingOnDecision" boolean NOT NULL, 
	"WaitingOnDecisionUtc" timestamp NULL, 
	"LastPlayedUtc" timestamp NULL, 
	"IsAbandoned" boolean NOT NULL, 
	"AbandonedUtc" timestamp NULL, 
	"ActionUnits" int NOT NULL, 
	"Action" int NOT NULL, 
	"ActionUtc" timestamp NULL
	);
ALTER TABLE "GameRuns" ADD CONSTRAINT "PK_GameRuns" PRIMARY KEY ("GameRunId");
ALTER TABLE "GameRuns" ALTER COLUMN "IsWaitingOnDecision" SET DEFAULT (False);
ALTER TABLE "GameRuns" ALTER COLUMN "IsAbandoned" SET DEFAULT (False);
ALTER TABLE "GameRuns" ALTER COLUMN "ActionUnits" SET DEFAULT (0);
ALTER TABLE "GameRuns" ALTER COLUMN "Action" SET DEFAULT (0);
CREATE UNIQUE INDEX "GameRuns_Token" ON "GameRuns" (
	"Token"
	) WITH (FILLFACTOR=90)

***** MySqlDialect *****
CREATE TABLE `GameRuns` (
	`GameRunId` int NOT NULL AUTO_INCREMENT, 
	`CreatedUtc` datetime NOT NULL, 
	`Token` nvarchar(64) NOT NULL, 
	`Unit` int NOT NULL, 
	`Mode` int NOT NULL, 
	`HailMaryCount` int NOT NULL, 
	`Name` nvarchar(128) NOT NULL, 
	`Hands` int NOT NULL, 
	`Investables` int NOT NULL, 
	`HouseEdge` decimal(18, 5) NOT NULL, 
	`IsWaitingOnDecision` bit NOT NULL, 
	`WaitingOnDecisionUtc` datetime NULL, 
	`LastPlayedUtc` datetime NULL, 
	`IsAbandoned` bit NOT NULL, 
	`AbandonedUtc` datetime NULL, 
	`ActionUnits` int NOT NULL, 
	`Action` int NOT NULL, 
	`ActionUtc` datetime NULL
	, PRIMARY KEY (`GameRunId`));

ALTER TABLE `GameRuns` MODIFY COLUMN `IsWaitingOnDecision` bit  DEFAULT 0
ALTER TABLE `GameRuns` MODIFY COLUMN `IsAbandoned` bit  DEFAULT 0
ALTER TABLE `GameRuns` MODIFY COLUMN `ActionUnits` int  DEFAULT 0
ALTER TABLE `GameRuns` MODIFY COLUMN `Action` int  DEFAULT 0
CREATE UNIQUE INDEX `GameRuns_Token` ON `GameRuns` (
	`Token`
	)

*/
