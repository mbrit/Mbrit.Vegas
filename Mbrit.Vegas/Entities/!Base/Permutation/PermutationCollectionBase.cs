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
    /// Defines the base collection for entities of type <see cref="Permutation"/>.
    /// </summary>
    [Serializable()]
    public abstract class PermutationCollectionBase : EntityCollection<Permutation>
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        protected PermutationCollectionBase()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected PermutationCollectionBase(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
    }
}
