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
    /// Defines the base entity type for 'Permutations'.
    /// </summary>
    [Serializable()]
    public abstract class PermutationBase : BootFX.Common.Entities.Entity
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        protected PermutationBase()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected PermutationBase(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// <summary>
        /// Gets or sets the value for 'PermutationId'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'PermutationId' column.
        /// </remarks>
        [EntityField("PermutationId", System.Data.DbType.Int32, ((BootFX.Common.Entities.EntityFieldFlags.Key | BootFX.Common.Entities.EntityFieldFlags.Common) 
                    | BootFX.Common.Entities.EntityFieldFlags.AutoIncrement))]
        public int PermutationId
        {
            get
            {
                return ((int)(this["PermutationId"]));
            }
            set
            {
                this["PermutationId"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Key'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Key' column.
        /// </remarks>
        [EntityField("Key", System.Data.DbType.String, BootFX.Common.Entities.EntityFieldFlags.Common, 32)]
        public string Key
        {
            get
            {
                return ((string)(this["Key"]));
            }
            set
            {
                this["Key"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'HailMaryMap'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'HailMaryMap' column.
        /// </remarks>
        [EntityField("HailMaryMap", System.Data.DbType.String, (BootFX.Common.Entities.EntityFieldFlags.Nullable | BootFX.Common.Entities.EntityFieldFlags.Common), 32)]
        public string HailMaryMap
        {
            get
            {
                return ((string)(this["HailMaryMap"]));
            }
            set
            {
                this["HailMaryMap"] = value;
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
        /// Gets or sets the value for 'UnitSize'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'UnitSize' column.
        /// </remarks>
        [EntityField("UnitSize", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int UnitSize
        {
            get
            {
                return ((int)(this["UnitSize"]));
            }
            set
            {
                this["UnitSize"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MajorBustSeen'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MajorBustSeen' column.
        /// </remarks>
        [EntityField("MajorBustSeen", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public bool MajorBustSeen
        {
            get
            {
                return ((bool)(this["MajorBustSeen"]));
            }
            set
            {
                this["MajorBustSeen"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MajorBustHand'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MajorBustHand' column.
        /// </remarks>
        [EntityField("MajorBustHand", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int MajorBustHand
        {
            get
            {
                return ((int)(this["MajorBustHand"]));
            }
            set
            {
                this["MajorBustHand"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MajorBustProfit'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MajorBustProfit' column.
        /// </remarks>
        [EntityField("MajorBustProfit", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal MajorBustProfit
        {
            get
            {
                return ((decimal)(this["MajorBustProfit"]));
            }
            set
            {
                this["MajorBustProfit"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MajorBustWagered'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MajorBustWagered' column.
        /// </remarks>
        [EntityField("MajorBustWagered", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal MajorBustWagered
        {
            get
            {
                return ((decimal)(this["MajorBustWagered"]));
            }
            set
            {
                this["MajorBustWagered"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MajorBustEv'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MajorBustEv' column.
        /// </remarks>
        [EntityField("MajorBustEv", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal MajorBustEv
        {
            get
            {
                return ((decimal)(this["MajorBustEv"]));
            }
            set
            {
                this["MajorBustEv"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MinorBustSeen'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MinorBustSeen' column.
        /// </remarks>
        [EntityField("MinorBustSeen", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public bool MinorBustSeen
        {
            get
            {
                return ((bool)(this["MinorBustSeen"]));
            }
            set
            {
                this["MinorBustSeen"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MinorBustHand'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MinorBustHand' column.
        /// </remarks>
        [EntityField("MinorBustHand", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int MinorBustHand
        {
            get
            {
                return ((int)(this["MinorBustHand"]));
            }
            set
            {
                this["MinorBustHand"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MinorBustProfit'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MinorBustProfit' column.
        /// </remarks>
        [EntityField("MinorBustProfit", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal MinorBustProfit
        {
            get
            {
                return ((decimal)(this["MinorBustProfit"]));
            }
            set
            {
                this["MinorBustProfit"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MinorBustWagered'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MinorBustWagered' column.
        /// </remarks>
        [EntityField("MinorBustWagered", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal MinorBustWagered
        {
            get
            {
                return ((decimal)(this["MinorBustWagered"]));
            }
            set
            {
                this["MinorBustWagered"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'MinorBustEv'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'MinorBustEv' column.
        /// </remarks>
        [EntityField("MinorBustEv", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal MinorBustEv
        {
            get
            {
                return ((decimal)(this["MinorBustEv"]));
            }
            set
            {
                this["MinorBustEv"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike0p5Seen'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike0p5Seen' column.
        /// </remarks>
        [EntityField("Spike0p5Seen", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public bool Spike0p5Seen
        {
            get
            {
                return ((bool)(this["Spike0p5Seen"]));
            }
            set
            {
                this["Spike0p5Seen"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike0p5Hand'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike0p5Hand' column.
        /// </remarks>
        [EntityField("Spike0p5Hand", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int Spike0p5Hand
        {
            get
            {
                return ((int)(this["Spike0p5Hand"]));
            }
            set
            {
                this["Spike0p5Hand"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike0p5Profit'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike0p5Profit' column.
        /// </remarks>
        [EntityField("Spike0p5Profit", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Spike0p5Profit
        {
            get
            {
                return ((decimal)(this["Spike0p5Profit"]));
            }
            set
            {
                this["Spike0p5Profit"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike0p5Wagered'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike0p5Wagered' column.
        /// </remarks>
        [EntityField("Spike0p5Wagered", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Spike0p5Wagered
        {
            get
            {
                return ((decimal)(this["Spike0p5Wagered"]));
            }
            set
            {
                this["Spike0p5Wagered"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike0p5Ev'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike0p5Ev' column.
        /// </remarks>
        [EntityField("Spike0p5Ev", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Spike0p5Ev
        {
            get
            {
                return ((decimal)(this["Spike0p5Ev"]));
            }
            set
            {
                this["Spike0p5Ev"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike1Seen'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike1Seen' column.
        /// </remarks>
        [EntityField("Spike1Seen", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public bool Spike1Seen
        {
            get
            {
                return ((bool)(this["Spike1Seen"]));
            }
            set
            {
                this["Spike1Seen"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike1Hand'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike1Hand' column.
        /// </remarks>
        [EntityField("Spike1Hand", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int Spike1Hand
        {
            get
            {
                return ((int)(this["Spike1Hand"]));
            }
            set
            {
                this["Spike1Hand"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike1Profit'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike1Profit' column.
        /// </remarks>
        [EntityField("Spike1Profit", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Spike1Profit
        {
            get
            {
                return ((decimal)(this["Spike1Profit"]));
            }
            set
            {
                this["Spike1Profit"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike1Wagered'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike1Wagered' column.
        /// </remarks>
        [EntityField("Spike1Wagered", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Spike1Wagered
        {
            get
            {
                return ((decimal)(this["Spike1Wagered"]));
            }
            set
            {
                this["Spike1Wagered"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Spike1Ev'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Spike1Ev' column.
        /// </remarks>
        [EntityField("Spike1Ev", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Spike1Ev
        {
            get
            {
                return ((decimal)(this["Spike1Ev"]));
            }
            set
            {
                this["Spike1Ev"] = value;
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
        /// Gets or sets the value for 'Profit'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Profit' column.
        /// </remarks>
        [EntityField("Profit", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Profit
        {
            get
            {
                return ((decimal)(this["Profit"]));
            }
            set
            {
                this["Profit"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Invested'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Invested' column.
        /// </remarks>
        [EntityField("Invested", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Invested
        {
            get
            {
                return ((decimal)(this["Invested"]));
            }
            set
            {
                this["Invested"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Wagered'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Wagered' column.
        /// </remarks>
        [EntityField("Wagered", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Wagered
        {
            get
            {
                return ((decimal)(this["Wagered"]));
            }
            set
            {
                this["Wagered"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Ev'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Ev' column.
        /// </remarks>
        [EntityField("Ev", System.Data.DbType.Decimal, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public decimal Ev
        {
            get
            {
                return ((decimal)(this["Ev"]));
            }
            set
            {
                this["Ev"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Outcome'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Outcome' column.
        /// </remarks>
        [EntityField("Outcome", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public Mbrit.Vegas.Simulator.WalkGameOutcome Outcome
        {
            get
            {
                return ((Mbrit.Vegas.Simulator.WalkGameOutcome)(this["Outcome"]));
            }
            set
            {
                this["Outcome"] = value;
            }
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'Permutation'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter CreateFilterBase()
        {
            return new BootFX.Common.Data.SqlFilter(typeof(Permutation));
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'Permutation'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter<Permutation> CreateFilter()
        {
            return new BootFX.Common.Data.SqlFilter<Permutation>();
        }
        
        /// <summary>
        /// Get all <see cref="Permutation"/> entities.
        /// </summary>
        public static PermutationCollection GetAll()
        {
            BootFX.Common.Data.SqlFilter filter = BootFX.Common.Data.SqlFilter.CreateGetAllFilter(typeof(Permutation));
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets the <see cref="Permutation"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        public static Permutation GetById(int permutationId)
        {
            return Mbrit.Vegas.Permutation.GetById(permutationId, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets the <see cref="Permutation"/> entity where the ID matches the given specification.
        /// </summary>
        public static Permutation GetById(int permutationId, BootFX.Common.Data.SqlOperator permutationIdOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("PermutationId", permutationIdOperator, permutationId);
            return ((Permutation)(filter.ExecuteEntity()));
        }
        
        /// <summary>
        /// Gets the <see cref="Permutation"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        public static Permutation GetById(int permutationId, BootFX.Common.OnNotFound onNotFound)
        {
            return Mbrit.Vegas.Permutation.GetById(permutationId, BootFX.Common.Data.SqlOperator.EqualTo, onNotFound);
        }
        
        /// <summary>
        /// Gets the <see cref="Permutation"/> entity where the ID matches the given specification.
        /// </summary>
        public static Permutation GetById(int permutationId, BootFX.Common.Data.SqlOperator permutationIdOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("PermutationId", permutationIdOperator, permutationId);
            Permutation results = ((Permutation)(filter.ExecuteEntity()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Key is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Key</c>
        /// </remarks>
        public static PermutationCollection GetByKey(string key)
        {
            return Mbrit.Vegas.Permutation.GetByKey(key, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Key matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Key</c>
        /// </remarks>
        public static PermutationCollection GetByKey(string key, BootFX.Common.Data.SqlOperator keyOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Key", keyOperator, key);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Key matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Key</c>
        /// </remarks>
        public static PermutationCollection GetByKey(string key, BootFX.Common.Data.SqlOperator keyOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Key", keyOperator, key);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where HailMaryMap is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>HailMaryMap</c>
        /// </remarks>
        public static PermutationCollection GetByHailMaryMap(string hailMaryMap)
        {
            return Mbrit.Vegas.Permutation.GetByHailMaryMap(hailMaryMap, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where HailMaryMap matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>HailMaryMap</c>
        /// </remarks>
        public static PermutationCollection GetByHailMaryMap(string hailMaryMap, BootFX.Common.Data.SqlOperator hailMaryMapOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("HailMaryMap", hailMaryMapOperator, hailMaryMap);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where HailMaryMap matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>HailMaryMap</c>
        /// </remarks>
        public static PermutationCollection GetByHailMaryMap(string hailMaryMap, BootFX.Common.Data.SqlOperator hailMaryMapOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("HailMaryMap", hailMaryMapOperator, hailMaryMap);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Hands is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Hands</c>
        /// </remarks>
        public static PermutationCollection GetByHands(int hands)
        {
            return Mbrit.Vegas.Permutation.GetByHands(hands, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Hands matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Hands</c>
        /// </remarks>
        public static PermutationCollection GetByHands(int hands, BootFX.Common.Data.SqlOperator handsOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Hands", handsOperator, hands);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Hands matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Hands</c>
        /// </remarks>
        public static PermutationCollection GetByHands(int hands, BootFX.Common.Data.SqlOperator handsOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Hands", handsOperator, hands);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Investables is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Investables</c>
        /// </remarks>
        public static PermutationCollection GetByInvestables(int investables)
        {
            return Mbrit.Vegas.Permutation.GetByInvestables(investables, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Investables matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Investables</c>
        /// </remarks>
        public static PermutationCollection GetByInvestables(int investables, BootFX.Common.Data.SqlOperator investablesOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Investables", investablesOperator, investables);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Investables matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Investables</c>
        /// </remarks>
        public static PermutationCollection GetByInvestables(int investables, BootFX.Common.Data.SqlOperator investablesOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Investables", investablesOperator, investables);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Mode is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Mode</c>
        /// </remarks>
        public static PermutationCollection GetByMode(Mbrit.Vegas.Simulator.WalkGameMode mode)
        {
            return Mbrit.Vegas.Permutation.GetByMode(mode, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Mode matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Mode</c>
        /// </remarks>
        public static PermutationCollection GetByMode(Mbrit.Vegas.Simulator.WalkGameMode mode, BootFX.Common.Data.SqlOperator modeOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Mode", modeOperator, mode);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Mode matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Mode</c>
        /// </remarks>
        public static PermutationCollection GetByMode(Mbrit.Vegas.Simulator.WalkGameMode mode, BootFX.Common.Data.SqlOperator modeOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Mode", modeOperator, mode);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where UnitSize is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>UnitSize</c>
        /// </remarks>
        public static PermutationCollection GetByUnitSize(int unitSize)
        {
            return Mbrit.Vegas.Permutation.GetByUnitSize(unitSize, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where UnitSize matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>UnitSize</c>
        /// </remarks>
        public static PermutationCollection GetByUnitSize(int unitSize, BootFX.Common.Data.SqlOperator unitSizeOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("UnitSize", unitSizeOperator, unitSize);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where UnitSize matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>UnitSize</c>
        /// </remarks>
        public static PermutationCollection GetByUnitSize(int unitSize, BootFX.Common.Data.SqlOperator unitSizeOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("UnitSize", unitSizeOperator, unitSize);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MajorBustSeen is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MajorBustSeen</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustSeen(bool majorBustSeen)
        {
            return Mbrit.Vegas.Permutation.GetByMajorBustSeen(majorBustSeen, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MajorBustSeen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustSeen</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustSeen(bool majorBustSeen, BootFX.Common.Data.SqlOperator majorBustSeenOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustSeen", majorBustSeenOperator, majorBustSeen);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MajorBustSeen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustSeen</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustSeen(bool majorBustSeen, BootFX.Common.Data.SqlOperator majorBustSeenOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustSeen", majorBustSeenOperator, majorBustSeen);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MajorBustHand is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MajorBustHand</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustHand(int majorBustHand)
        {
            return Mbrit.Vegas.Permutation.GetByMajorBustHand(majorBustHand, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MajorBustHand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustHand</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustHand(int majorBustHand, BootFX.Common.Data.SqlOperator majorBustHandOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustHand", majorBustHandOperator, majorBustHand);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MajorBustHand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustHand</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustHand(int majorBustHand, BootFX.Common.Data.SqlOperator majorBustHandOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustHand", majorBustHandOperator, majorBustHand);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MajorBustProfit is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MajorBustProfit</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustProfit(decimal majorBustProfit)
        {
            return Mbrit.Vegas.Permutation.GetByMajorBustProfit(majorBustProfit, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MajorBustProfit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustProfit</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustProfit(decimal majorBustProfit, BootFX.Common.Data.SqlOperator majorBustProfitOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustProfit", majorBustProfitOperator, majorBustProfit);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MajorBustProfit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustProfit</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustProfit(decimal majorBustProfit, BootFX.Common.Data.SqlOperator majorBustProfitOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustProfit", majorBustProfitOperator, majorBustProfit);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MajorBustWagered is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MajorBustWagered</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustWagered(decimal majorBustWagered)
        {
            return Mbrit.Vegas.Permutation.GetByMajorBustWagered(majorBustWagered, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MajorBustWagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustWagered</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustWagered(decimal majorBustWagered, BootFX.Common.Data.SqlOperator majorBustWageredOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustWagered", majorBustWageredOperator, majorBustWagered);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MajorBustWagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustWagered</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustWagered(decimal majorBustWagered, BootFX.Common.Data.SqlOperator majorBustWageredOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustWagered", majorBustWageredOperator, majorBustWagered);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MajorBustEv is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MajorBustEv</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustEv(decimal majorBustEv)
        {
            return Mbrit.Vegas.Permutation.GetByMajorBustEv(majorBustEv, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MajorBustEv matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustEv</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustEv(decimal majorBustEv, BootFX.Common.Data.SqlOperator majorBustEvOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustEv", majorBustEvOperator, majorBustEv);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MajorBustEv matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MajorBustEv</c>
        /// </remarks>
        public static PermutationCollection GetByMajorBustEv(decimal majorBustEv, BootFX.Common.Data.SqlOperator majorBustEvOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MajorBustEv", majorBustEvOperator, majorBustEv);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MinorBustSeen is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MinorBustSeen</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustSeen(bool minorBustSeen)
        {
            return Mbrit.Vegas.Permutation.GetByMinorBustSeen(minorBustSeen, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MinorBustSeen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustSeen</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustSeen(bool minorBustSeen, BootFX.Common.Data.SqlOperator minorBustSeenOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustSeen", minorBustSeenOperator, minorBustSeen);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MinorBustSeen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustSeen</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustSeen(bool minorBustSeen, BootFX.Common.Data.SqlOperator minorBustSeenOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustSeen", minorBustSeenOperator, minorBustSeen);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MinorBustHand is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MinorBustHand</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustHand(int minorBustHand)
        {
            return Mbrit.Vegas.Permutation.GetByMinorBustHand(minorBustHand, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MinorBustHand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustHand</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustHand(int minorBustHand, BootFX.Common.Data.SqlOperator minorBustHandOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustHand", minorBustHandOperator, minorBustHand);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MinorBustHand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustHand</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustHand(int minorBustHand, BootFX.Common.Data.SqlOperator minorBustHandOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustHand", minorBustHandOperator, minorBustHand);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MinorBustProfit is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MinorBustProfit</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustProfit(decimal minorBustProfit)
        {
            return Mbrit.Vegas.Permutation.GetByMinorBustProfit(minorBustProfit, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MinorBustProfit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustProfit</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustProfit(decimal minorBustProfit, BootFX.Common.Data.SqlOperator minorBustProfitOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustProfit", minorBustProfitOperator, minorBustProfit);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MinorBustProfit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustProfit</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustProfit(decimal minorBustProfit, BootFX.Common.Data.SqlOperator minorBustProfitOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustProfit", minorBustProfitOperator, minorBustProfit);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MinorBustWagered is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MinorBustWagered</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustWagered(decimal minorBustWagered)
        {
            return Mbrit.Vegas.Permutation.GetByMinorBustWagered(minorBustWagered, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MinorBustWagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustWagered</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustWagered(decimal minorBustWagered, BootFX.Common.Data.SqlOperator minorBustWageredOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustWagered", minorBustWageredOperator, minorBustWagered);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MinorBustWagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustWagered</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustWagered(decimal minorBustWagered, BootFX.Common.Data.SqlOperator minorBustWageredOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustWagered", minorBustWageredOperator, minorBustWagered);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where MinorBustEv is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>MinorBustEv</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustEv(decimal minorBustEv)
        {
            return Mbrit.Vegas.Permutation.GetByMinorBustEv(minorBustEv, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where MinorBustEv matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustEv</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustEv(decimal minorBustEv, BootFX.Common.Data.SqlOperator minorBustEvOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustEv", minorBustEvOperator, minorBustEv);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where MinorBustEv matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>MinorBustEv</c>
        /// </remarks>
        public static PermutationCollection GetByMinorBustEv(decimal minorBustEv, BootFX.Common.Data.SqlOperator minorBustEvOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("MinorBustEv", minorBustEvOperator, minorBustEv);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Seen is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike0p5Seen</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Seen(bool spike0p5Seen)
        {
            return Mbrit.Vegas.Permutation.GetBySpike0p5Seen(spike0p5Seen, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Seen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Seen</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Seen(bool spike0p5Seen, BootFX.Common.Data.SqlOperator spike0p5SeenOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Seen", spike0p5SeenOperator, spike0p5Seen);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Seen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Seen</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Seen(bool spike0p5Seen, BootFX.Common.Data.SqlOperator spike0p5SeenOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Seen", spike0p5SeenOperator, spike0p5Seen);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Hand is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike0p5Hand</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Hand(int spike0p5Hand)
        {
            return Mbrit.Vegas.Permutation.GetBySpike0p5Hand(spike0p5Hand, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Hand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Hand</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Hand(int spike0p5Hand, BootFX.Common.Data.SqlOperator spike0p5HandOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Hand", spike0p5HandOperator, spike0p5Hand);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Hand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Hand</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Hand(int spike0p5Hand, BootFX.Common.Data.SqlOperator spike0p5HandOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Hand", spike0p5HandOperator, spike0p5Hand);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Profit is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike0p5Profit</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Profit(decimal spike0p5Profit)
        {
            return Mbrit.Vegas.Permutation.GetBySpike0p5Profit(spike0p5Profit, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Profit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Profit</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Profit(decimal spike0p5Profit, BootFX.Common.Data.SqlOperator spike0p5ProfitOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Profit", spike0p5ProfitOperator, spike0p5Profit);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Profit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Profit</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Profit(decimal spike0p5Profit, BootFX.Common.Data.SqlOperator spike0p5ProfitOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Profit", spike0p5ProfitOperator, spike0p5Profit);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Wagered is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike0p5Wagered</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Wagered(decimal spike0p5Wagered)
        {
            return Mbrit.Vegas.Permutation.GetBySpike0p5Wagered(spike0p5Wagered, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Wagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Wagered</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Wagered(decimal spike0p5Wagered, BootFX.Common.Data.SqlOperator spike0p5WageredOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Wagered", spike0p5WageredOperator, spike0p5Wagered);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Wagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Wagered</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Wagered(decimal spike0p5Wagered, BootFX.Common.Data.SqlOperator spike0p5WageredOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Wagered", spike0p5WageredOperator, spike0p5Wagered);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Ev is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike0p5Ev</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Ev(decimal spike0p5Ev)
        {
            return Mbrit.Vegas.Permutation.GetBySpike0p5Ev(spike0p5Ev, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Ev matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Ev</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Ev(decimal spike0p5Ev, BootFX.Common.Data.SqlOperator spike0p5EvOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Ev", spike0p5EvOperator, spike0p5Ev);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike0p5Ev matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike0p5Ev</c>
        /// </remarks>
        public static PermutationCollection GetBySpike0p5Ev(decimal spike0p5Ev, BootFX.Common.Data.SqlOperator spike0p5EvOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike0p5Ev", spike0p5EvOperator, spike0p5Ev);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike1Seen is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike1Seen</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Seen(bool spike1Seen)
        {
            return Mbrit.Vegas.Permutation.GetBySpike1Seen(spike1Seen, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike1Seen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Seen</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Seen(bool spike1Seen, BootFX.Common.Data.SqlOperator spike1SeenOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Seen", spike1SeenOperator, spike1Seen);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike1Seen matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Seen</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Seen(bool spike1Seen, BootFX.Common.Data.SqlOperator spike1SeenOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Seen", spike1SeenOperator, spike1Seen);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike1Hand is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike1Hand</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Hand(int spike1Hand)
        {
            return Mbrit.Vegas.Permutation.GetBySpike1Hand(spike1Hand, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike1Hand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Hand</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Hand(int spike1Hand, BootFX.Common.Data.SqlOperator spike1HandOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Hand", spike1HandOperator, spike1Hand);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike1Hand matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Hand</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Hand(int spike1Hand, BootFX.Common.Data.SqlOperator spike1HandOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Hand", spike1HandOperator, spike1Hand);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike1Profit is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike1Profit</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Profit(decimal spike1Profit)
        {
            return Mbrit.Vegas.Permutation.GetBySpike1Profit(spike1Profit, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike1Profit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Profit</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Profit(decimal spike1Profit, BootFX.Common.Data.SqlOperator spike1ProfitOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Profit", spike1ProfitOperator, spike1Profit);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike1Profit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Profit</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Profit(decimal spike1Profit, BootFX.Common.Data.SqlOperator spike1ProfitOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Profit", spike1ProfitOperator, spike1Profit);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike1Wagered is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike1Wagered</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Wagered(decimal spike1Wagered)
        {
            return Mbrit.Vegas.Permutation.GetBySpike1Wagered(spike1Wagered, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike1Wagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Wagered</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Wagered(decimal spike1Wagered, BootFX.Common.Data.SqlOperator spike1WageredOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Wagered", spike1WageredOperator, spike1Wagered);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike1Wagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Wagered</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Wagered(decimal spike1Wagered, BootFX.Common.Data.SqlOperator spike1WageredOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Wagered", spike1WageredOperator, spike1Wagered);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Spike1Ev is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Spike1Ev</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Ev(decimal spike1Ev)
        {
            return Mbrit.Vegas.Permutation.GetBySpike1Ev(spike1Ev, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Spike1Ev matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Ev</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Ev(decimal spike1Ev, BootFX.Common.Data.SqlOperator spike1EvOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Ev", spike1EvOperator, spike1Ev);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Spike1Ev matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Spike1Ev</c>
        /// </remarks>
        public static PermutationCollection GetBySpike1Ev(decimal spike1Ev, BootFX.Common.Data.SqlOperator spike1EvOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Spike1Ev", spike1EvOperator, spike1Ev);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where CreatedUtc is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>CreatedUtc</c>
        /// </remarks>
        public static PermutationCollection GetByCreatedUtc(System.DateTime createdUtc)
        {
            return Mbrit.Vegas.Permutation.GetByCreatedUtc(createdUtc, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where CreatedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>CreatedUtc</c>
        /// </remarks>
        public static PermutationCollection GetByCreatedUtc(System.DateTime createdUtc, BootFX.Common.Data.SqlOperator createdUtcOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("CreatedUtc", createdUtcOperator, createdUtc);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where CreatedUtc matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>CreatedUtc</c>
        /// </remarks>
        public static PermutationCollection GetByCreatedUtc(System.DateTime createdUtc, BootFX.Common.Data.SqlOperator createdUtcOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("CreatedUtc", createdUtcOperator, createdUtc);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Profit is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Profit</c>
        /// </remarks>
        public static PermutationCollection GetByProfit(decimal profit)
        {
            return Mbrit.Vegas.Permutation.GetByProfit(profit, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Profit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Profit</c>
        /// </remarks>
        public static PermutationCollection GetByProfit(decimal profit, BootFX.Common.Data.SqlOperator profitOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Profit", profitOperator, profit);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Profit matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Profit</c>
        /// </remarks>
        public static PermutationCollection GetByProfit(decimal profit, BootFX.Common.Data.SqlOperator profitOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Profit", profitOperator, profit);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Invested is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Invested</c>
        /// </remarks>
        public static PermutationCollection GetByInvested(decimal invested)
        {
            return Mbrit.Vegas.Permutation.GetByInvested(invested, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Invested matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Invested</c>
        /// </remarks>
        public static PermutationCollection GetByInvested(decimal invested, BootFX.Common.Data.SqlOperator investedOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Invested", investedOperator, invested);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Invested matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Invested</c>
        /// </remarks>
        public static PermutationCollection GetByInvested(decimal invested, BootFX.Common.Data.SqlOperator investedOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Invested", investedOperator, invested);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Wagered is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Wagered</c>
        /// </remarks>
        public static PermutationCollection GetByWagered(decimal wagered)
        {
            return Mbrit.Vegas.Permutation.GetByWagered(wagered, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Wagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Wagered</c>
        /// </remarks>
        public static PermutationCollection GetByWagered(decimal wagered, BootFX.Common.Data.SqlOperator wageredOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Wagered", wageredOperator, wagered);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Wagered matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Wagered</c>
        /// </remarks>
        public static PermutationCollection GetByWagered(decimal wagered, BootFX.Common.Data.SqlOperator wageredOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Wagered", wageredOperator, wagered);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Ev is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Ev</c>
        /// </remarks>
        public static PermutationCollection GetByEv(decimal ev)
        {
            return Mbrit.Vegas.Permutation.GetByEv(ev, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Ev matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Ev</c>
        /// </remarks>
        public static PermutationCollection GetByEv(decimal ev, BootFX.Common.Data.SqlOperator evOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Ev", evOperator, ev);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Ev matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Ev</c>
        /// </remarks>
        public static PermutationCollection GetByEv(decimal ev, BootFX.Common.Data.SqlOperator evOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Ev", evOperator, ev);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Outcome is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Permutation
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Outcome</c>
        /// </remarks>
        public static PermutationCollection GetByOutcome(Mbrit.Vegas.Simulator.WalkGameOutcome outcome)
        {
            return Mbrit.Vegas.Permutation.GetByOutcome(outcome, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Outcome matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Outcome</c>
        /// </remarks>
        public static PermutationCollection GetByOutcome(Mbrit.Vegas.Simulator.WalkGameOutcome outcome, BootFX.Common.Data.SqlOperator outcomeOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Outcome", outcomeOperator, outcome);
            return ((PermutationCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Outcome matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Outcome</c>
        /// </remarks>
        public static PermutationCollection GetByOutcome(Mbrit.Vegas.Simulator.WalkGameOutcome outcome, BootFX.Common.Data.SqlOperator outcomeOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Permutation));
            filter.Constraints.Add("Outcome", outcomeOperator, outcome);
            PermutationCollection results = ((PermutationCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
    }
}


/*
***** SqlServerDialect *****
CREATE TABLE [Permutations] (
	[PermutationId] int NOT NULL IDENTITY(1,1), 
	[Key] nvarchar(32) NOT NULL, 
	[HailMaryMap] nvarchar(32) NULL, 
	[Hands] int NOT NULL, 
	[Investables] int NOT NULL, 
	[Mode] int NOT NULL, 
	[UnitSize] int NOT NULL, 
	[MajorBustSeen] bit NOT NULL, 
	[MajorBustHand] int NOT NULL, 
	[MajorBustProfit] decimal(18, 5) NOT NULL, 
	[MajorBustWagered] decimal(18, 5) NOT NULL, 
	[MajorBustEv] decimal(18, 5) NOT NULL, 
	[MinorBustSeen] bit NOT NULL, 
	[MinorBustHand] int NOT NULL, 
	[MinorBustProfit] decimal(18, 5) NOT NULL, 
	[MinorBustWagered] decimal(18, 5) NOT NULL, 
	[MinorBustEv] decimal(18, 5) NOT NULL, 
	[Spike0p5Seen] bit NOT NULL, 
	[Spike0p5Hand] int NOT NULL, 
	[Spike0p5Profit] decimal(18, 5) NOT NULL, 
	[Spike0p5Wagered] decimal(18, 5) NOT NULL, 
	[Spike0p5Ev] decimal(18, 5) NOT NULL, 
	[Spike1Seen] bit NOT NULL, 
	[Spike1Hand] int NOT NULL, 
	[Spike1Profit] decimal(18, 5) NOT NULL, 
	[Spike1Wagered] decimal(18, 5) NOT NULL, 
	[Spike1Ev] decimal(18, 5) NOT NULL, 
	[CreatedUtc] datetime NOT NULL, 
	[Profit] decimal(18, 5) NOT NULL, 
	[Invested] decimal(18, 5) NOT NULL, 
	[Wagered] decimal(18, 5) NOT NULL, 
	[Ev] decimal(18, 5) NOT NULL, 
	[Outcome] int NOT NULL
	);
ALTER TABLE [Permutations] ADD CONSTRAINT [PK_Permutations] PRIMARY KEY ([PermutationId]);

***** PostgresDialect *****
CREATE TABLE "Permutations" (
	"PermutationId" serial, 
	"Key" varchar(32) collate english_ci NOT NULL, 
	"HailMaryMap" varchar(32) collate english_ci NULL, 
	"Hands" int NOT NULL, 
	"Investables" int NOT NULL, 
	"Mode" int NOT NULL, 
	"UnitSize" int NOT NULL, 
	"MajorBustSeen" boolean NOT NULL, 
	"MajorBustHand" int NOT NULL, 
	"MajorBustProfit" decimal(18, 5) NOT NULL, 
	"MajorBustWagered" decimal(18, 5) NOT NULL, 
	"MajorBustEv" decimal(18, 5) NOT NULL, 
	"MinorBustSeen" boolean NOT NULL, 
	"MinorBustHand" int NOT NULL, 
	"MinorBustProfit" decimal(18, 5) NOT NULL, 
	"MinorBustWagered" decimal(18, 5) NOT NULL, 
	"MinorBustEv" decimal(18, 5) NOT NULL, 
	"Spike0p5Seen" boolean NOT NULL, 
	"Spike0p5Hand" int NOT NULL, 
	"Spike0p5Profit" decimal(18, 5) NOT NULL, 
	"Spike0p5Wagered" decimal(18, 5) NOT NULL, 
	"Spike0p5Ev" decimal(18, 5) NOT NULL, 
	"Spike1Seen" boolean NOT NULL, 
	"Spike1Hand" int NOT NULL, 
	"Spike1Profit" decimal(18, 5) NOT NULL, 
	"Spike1Wagered" decimal(18, 5) NOT NULL, 
	"Spike1Ev" decimal(18, 5) NOT NULL, 
	"CreatedUtc" timestamp NOT NULL, 
	"Profit" decimal(18, 5) NOT NULL, 
	"Invested" decimal(18, 5) NOT NULL, 
	"Wagered" decimal(18, 5) NOT NULL, 
	"Ev" decimal(18, 5) NOT NULL, 
	"Outcome" int NOT NULL
	);
ALTER TABLE "Permutations" ADD CONSTRAINT "PK_Permutations" PRIMARY KEY ("PermutationId");

***** MySqlDialect *****
CREATE TABLE `Permutations` (
	`PermutationId` int NOT NULL AUTO_INCREMENT, 
	`Key` nvarchar(32) NOT NULL, 
	`HailMaryMap` nvarchar(32) NULL, 
	`Hands` int NOT NULL, 
	`Investables` int NOT NULL, 
	`Mode` int NOT NULL, 
	`UnitSize` int NOT NULL, 
	`MajorBustSeen` bit NOT NULL, 
	`MajorBustHand` int NOT NULL, 
	`MajorBustProfit` decimal(18, 5) NOT NULL, 
	`MajorBustWagered` decimal(18, 5) NOT NULL, 
	`MajorBustEv` decimal(18, 5) NOT NULL, 
	`MinorBustSeen` bit NOT NULL, 
	`MinorBustHand` int NOT NULL, 
	`MinorBustProfit` decimal(18, 5) NOT NULL, 
	`MinorBustWagered` decimal(18, 5) NOT NULL, 
	`MinorBustEv` decimal(18, 5) NOT NULL, 
	`Spike0p5Seen` bit NOT NULL, 
	`Spike0p5Hand` int NOT NULL, 
	`Spike0p5Profit` decimal(18, 5) NOT NULL, 
	`Spike0p5Wagered` decimal(18, 5) NOT NULL, 
	`Spike0p5Ev` decimal(18, 5) NOT NULL, 
	`Spike1Seen` bit NOT NULL, 
	`Spike1Hand` int NOT NULL, 
	`Spike1Profit` decimal(18, 5) NOT NULL, 
	`Spike1Wagered` decimal(18, 5) NOT NULL, 
	`Spike1Ev` decimal(18, 5) NOT NULL, 
	`CreatedUtc` datetime NOT NULL, 
	`Profit` decimal(18, 5) NOT NULL, 
	`Invested` decimal(18, 5) NOT NULL, 
	`Wagered` decimal(18, 5) NOT NULL, 
	`Ev` decimal(18, 5) NOT NULL, 
	`Outcome` int NOT NULL
	, PRIMARY KEY (`PermutationId`));


*/
