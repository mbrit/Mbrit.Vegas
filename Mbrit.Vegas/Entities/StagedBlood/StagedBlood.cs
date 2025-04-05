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
    /// Defines the entity type for 'StagedBloods'.
    /// </summary>
    [Serializable()]
    [Entity(typeof(StagedBloodCollection), "stagedbloods")]
    public class StagedBlood : StagedBloodBase
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public StagedBlood()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected StagedBlood(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
    }
}
