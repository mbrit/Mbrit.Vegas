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
    /// Defines the base entity type for 'Casinos'.
    /// </summary>
    [Serializable()]
    public abstract class CasinoBase : BootFX.Common.Entities.Entity
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        protected CasinoBase()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected CasinoBase(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// <summary>
        /// Gets or sets the value for 'CasinoId'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'CasinoId' column.
        /// </remarks>
        [EntityField("CasinoId", System.Data.DbType.Int32, ((BootFX.Common.Entities.EntityFieldFlags.Key | BootFX.Common.Entities.EntityFieldFlags.Common) 
                    | BootFX.Common.Entities.EntityFieldFlags.AutoIncrement))]
        public int CasinoId
        {
            get
            {
                return ((int)(this["CasinoId"]));
            }
            set
            {
                this["CasinoId"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'Name'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'Name' column.
        /// </remarks>
        [EntityField("Name", System.Data.DbType.String, BootFX.Common.Entities.EntityFieldFlags.Common, 64)]
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
        /// Gets or sets the value for 'LocationId'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'LocationId' column.
        /// </remarks>
        [EntityField("LocationId", System.Data.DbType.Int32, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public int LocationId
        {
            get
            {
                return ((int)(this["LocationId"]));
            }
            set
            {
                this["LocationId"] = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the value for 'IsActive'.
        /// </summary>
        /// <remarks>
        /// This property maps to the 'IsActive' column.
        /// </remarks>
        [EntityField("IsActive", System.Data.DbType.Boolean, BootFX.Common.Entities.EntityFieldFlags.Common)]
        public bool IsActive
        {
            get
            {
                return ((bool)(this["IsActive"]));
            }
            set
            {
                this["IsActive"] = value;
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
        /// Creates a SqlFilter for an instance of 'Casino'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter CreateFilterBase()
        {
            return new BootFX.Common.Data.SqlFilter(typeof(Casino));
        }
        
        /// <summary>
        /// Creates a SqlFilter for an instance of 'Casino'.
        /// </summary>
        public static BootFX.Common.Data.SqlFilter<Casino> CreateFilter()
        {
            return new BootFX.Common.Data.SqlFilter<Casino>();
        }
        
        /// <summary>
        /// Get all <see cref="Casino"/> entities.
        /// </summary>
        public static CasinoCollection GetAll()
        {
            BootFX.Common.Data.SqlFilter filter = BootFX.Common.Data.SqlFilter.CreateGetAllFilter(typeof(Casino));
            return ((CasinoCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets the <see cref="Casino"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Casino
        /// </bootfx>
        public static Casino GetById(int casinoId)
        {
            return Mbrit.Vegas.Casino.GetById(casinoId, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets the <see cref="Casino"/> entity where the ID matches the given specification.
        /// </summary>
        public static Casino GetById(int casinoId, BootFX.Common.Data.SqlOperator casinoIdOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("CasinoId", casinoIdOperator, casinoId);
            return ((Casino)(filter.ExecuteEntity()));
        }
        
        /// <summary>
        /// Gets the <see cref="Casino"/> entity with the given ID.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Casino
        /// </bootfx>
        public static Casino GetById(int casinoId, BootFX.Common.OnNotFound onNotFound)
        {
            return Mbrit.Vegas.Casino.GetById(casinoId, BootFX.Common.Data.SqlOperator.EqualTo, onNotFound);
        }
        
        /// <summary>
        /// Gets the <see cref="Casino"/> entity where the ID matches the given specification.
        /// </summary>
        public static Casino GetById(int casinoId, BootFX.Common.Data.SqlOperator casinoIdOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("CasinoId", casinoIdOperator, casinoId);
            Casino results = ((Casino)(filter.ExecuteEntity()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Name is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Casino
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Name</c>
        /// </remarks>
        public static CasinoCollection GetByName(string name)
        {
            return Mbrit.Vegas.Casino.GetByName(name, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Name matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Name</c>
        /// </remarks>
        public static CasinoCollection GetByName(string name, BootFX.Common.Data.SqlOperator nameOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("Name", nameOperator, name);
            return ((CasinoCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Name matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Name</c>
        /// </remarks>
        public static CasinoCollection GetByName(string name, BootFX.Common.Data.SqlOperator nameOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("Name", nameOperator, name);
            CasinoCollection results = ((CasinoCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where LocationId is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Casino
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>LocationId</c>
        /// </remarks>
        public static CasinoCollection GetByLocationId(int locationId)
        {
            return Mbrit.Vegas.Casino.GetByLocationId(locationId, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where LocationId matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>LocationId</c>
        /// </remarks>
        public static CasinoCollection GetByLocationId(int locationId, BootFX.Common.Data.SqlOperator locationIdOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("LocationId", locationIdOperator, locationId);
            return ((CasinoCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where LocationId matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>LocationId</c>
        /// </remarks>
        public static CasinoCollection GetByLocationId(int locationId, BootFX.Common.Data.SqlOperator locationIdOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("LocationId", locationIdOperator, locationId);
            CasinoCollection results = ((CasinoCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where IsActive is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Casino
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>IsActive</c>
        /// </remarks>
        public static CasinoCollection GetByIsActive(bool isActive)
        {
            return Mbrit.Vegas.Casino.GetByIsActive(isActive, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where IsActive matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>IsActive</c>
        /// </remarks>
        public static CasinoCollection GetByIsActive(bool isActive, BootFX.Common.Data.SqlOperator isActiveOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("IsActive", isActiveOperator, isActive);
            return ((CasinoCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where IsActive matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>IsActive</c>
        /// </remarks>
        public static CasinoCollection GetByIsActive(bool isActive, BootFX.Common.Data.SqlOperator isActiveOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("IsActive", isActiveOperator, isActive);
            CasinoCollection results = ((CasinoCollection)(filter.ExecuteEntityCollection()));
            return results;
        }
        
        /// <summary>
        /// Gets entities where Token is equal to the given value.
        /// </summary>
        /// <bootfx>
        /// CreateEntityFilterEqualToMethod - Casino
        /// </bootfx>
        /// <remarks>
        /// Created for column <c>Token</c>
        /// </remarks>
        public static CasinoCollection GetByToken(string token)
        {
            return Mbrit.Vegas.Casino.GetByToken(token, BootFX.Common.Data.SqlOperator.EqualTo);
        }
        
        /// <summary>
        /// Gets entities where Token matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Token</c>
        /// </remarks>
        public static CasinoCollection GetByToken(string token, BootFX.Common.Data.SqlOperator tokenOperator)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("Token", tokenOperator, token);
            return ((CasinoCollection)(filter.ExecuteEntityCollection()));
        }
        
        /// <summary>
        /// Gets entities where Token matches the given specification.
        /// </summary>
        /// <remarks>
        /// Created for column <c>Token</c>
        /// </remarks>
        public static CasinoCollection GetByToken(string token, BootFX.Common.Data.SqlOperator tokenOperator, BootFX.Common.OnNotFound onNotFound)
        {
            BootFX.Common.Data.SqlFilter filter = new BootFX.Common.Data.SqlFilter(typeof(Casino));
            filter.Constraints.Add("Token", tokenOperator, token);
            CasinoCollection results = ((CasinoCollection)(filter.ExecuteEntityCollection()));
            return results;
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
CREATE TABLE [Casinos] (
	[CasinoId] int NOT NULL IDENTITY(1,1), 
	[Name] nvarchar(64) NOT NULL, 
	[LocationId] int NOT NULL, 
	[IsActive] bit NOT NULL, 
	[Token] nvarchar(64) NOT NULL
	);
ALTER TABLE [Casinos] ADD CONSTRAINT [PK_Casinos] PRIMARY KEY ([CasinoId]);

***** PostgresDialect *****
CREATE TABLE "Casinos" (
	"CasinoId" serial, 
	"Name" varchar(64) collate english_ci NOT NULL, 
	"LocationId" int NOT NULL, 
	"IsActive" boolean NOT NULL, 
	"Token" varchar(64) collate english_ci NOT NULL
	);
ALTER TABLE "Casinos" ADD CONSTRAINT "PK_Casinos" PRIMARY KEY ("CasinoId");

***** MySqlDialect *****
CREATE TABLE `Casinos` (
	`CasinoId` int NOT NULL AUTO_INCREMENT, 
	`Name` nvarchar(64) NOT NULL, 
	`LocationId` int NOT NULL, 
	`IsActive` bit NOT NULL, 
	`Token` nvarchar(64) NOT NULL
	, PRIMARY KEY (`CasinoId`));


*/
