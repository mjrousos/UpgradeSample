using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace eShopLegacy.Utilities
{
// Disabling obsolete warnings because binary formatter is used
// here only for compatibility reasons.
#pragma warning disable SYSLIB0011 // Type or member is obsolete
    public class Serializing
    {
        public Stream SerializeBinary(object input)
        {
            var stream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, input);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public object DeserializeBinary(Stream stream)
        {
            var binaryFormatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return binaryFormatter.Deserialize(stream);
        }
    }
#pragma warning restore SYSLIB0011 // Type or member is obsolete
}
