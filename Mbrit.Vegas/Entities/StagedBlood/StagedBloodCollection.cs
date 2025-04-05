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
    /// Defines the collection for entities of type <see cref="StagedBlood"/>.
    /// </summary>
    [Serializable()]
    public class StagedBloodCollection : StagedBloodCollectionBase
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public StagedBloodCollection()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected StagedBloodCollection(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
    }
}
