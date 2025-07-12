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
    /// Defines the collection for entities of type <see cref="GameRun"/>.
    /// </summary>
    [Serializable()]
    public class GameRunCollection : GameRunCollectionBase
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameRunCollection()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected GameRunCollection(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
    }
}
