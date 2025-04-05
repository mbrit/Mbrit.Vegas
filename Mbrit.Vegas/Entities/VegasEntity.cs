using BootFX.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public abstract class VegasEntity : Entity
    {
        public VegasEntity()
        {
        }

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected VegasEntity(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
                base(info, context)
        {
        }
    }
}
