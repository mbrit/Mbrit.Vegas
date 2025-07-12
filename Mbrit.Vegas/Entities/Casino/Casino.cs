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
    /// Defines the entity type for 'Casinos'.
    /// </summary>
    [Serializable()]
    [Entity(typeof(CasinoCollection), "Casinos")]
    [SortSpecification(new string[] {
            "Name"}, new BootFX.Common.Data.SortDirection[] {
            BootFX.Common.Data.SortDirection.Ascending})]
    public class Casino : CasinoBase
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public Casino()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected Casino(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
    }
}
