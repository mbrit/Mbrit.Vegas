using BootFX.Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    internal class AsStringConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert) => true;

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var asInt64 = 0L;
                if (reader.TryGetInt64(out asInt64))
                    return asInt64.ToString();
                else
                {
                    var asDecimal = 0M;
                    if (reader.TryGetDecimal(out asDecimal))
                        return asDecimal.ToString();
                    else
                        throw new NotImplementedException("This operation has not been implemented.");
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
                return reader.GetString();
            else
                throw new NotSupportedException($"Cannot handle '{reader.TokenType}'.");
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) => 
            writer.WriteStringValue(ConversionHelper.ToString(value));
    }
}
