using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using GroBuf;
using GroBuf.DataMembersExtracters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ImplicitEither.Tests
{
    [TestFixture]
    public class Serialization_Tests : TestBase
    {
        private static IEnumerable<TestCaseData> Serializers
        {
            get
            {
                yield return new TestCaseData(new GroBufSerializer()).SetName("GroBuf");
                yield return new TestCaseData(new MyJsonSerializer()).SetName("Json.NET");
                yield return new TestCaseData(new MyDataContractSerializer()).SetName("DataContract");
            }
        }

        [TestCaseSource(nameof(Serializers))]
        public void TestSerializer(ISerializer serializer)
        {
            Assert.Multiple(() =>
            {
                TestSerialization(EitherInt, serializer);
                TestSerialization(EitherString, serializer);

                Either<string, ComplexSerializableStuff> either = new ComplexSerializableStuff
                {
                    X = 1,
                    Y = "2",
                    Stuff = new ComplexSerializableStuff
                    {
                        X = 3,
                        Y = "4",
                        Stuff = new ComplexSerializableStuff {X = 5}
                    }
                };
                TestSerialization(either, serializer);
            });
        }

        private void TestSerialization<T>(T obj, ISerializer serializer)
        {
            var data = serializer.Serialize(obj);
            Console.Out.WriteLine("Serialized {0} ->\n{1}\n", obj, data);
            var deserialized = serializer.Deserialize<T>(data);
            Assert.That(deserialized, Is.EqualTo(obj));
        }

        public interface ISerializer
        {
            string Serialize<T>(T obj);
            T Deserialize<T>(string data);
        }

        private class MyDataContractSerializer : ISerializer
        {
            private static Type[] KnownTypes { get; } = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()).ToArray();

            public string Serialize<T>(T obj)
            {
                var serializer = new DataContractSerializer(typeof(T), KnownTypes);
                using (var writer = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings {Indent = true}))
                        serializer.WriteObject(xmlWriter, obj);
                    return writer.ToString();
                }
            }

            public T Deserialize<T>(string data)
            {
                var serializer = new DataContractSerializer(typeof(T), KnownTypes);
                using (var reader = new StringReader(data))
                using (var xmlReader = XmlReader.Create(reader))
                {
                    return (T) serializer.ReadObject(xmlReader);
                }
            }
        }

        private class GroBufSerializer : ISerializer
        {
            private GroBuf.ISerializer Serializer { get; } = new Serializer(new AllPropertiesExtractor(), null, GroBufOptions.MergeOnRead);

            public string Serialize<T>(T obj) => Convert.ToBase64String(Serializer.Serialize(obj));
            public T Deserialize<T>(string data) => Serializer.Deserialize<T>(Convert.FromBase64String(data));
        }

        private class MyJsonSerializer : ISerializer
        {
            public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);
            public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data);
        }

        public class ComplexSerializableStuff : IEquatable<ComplexSerializableStuff>
        {
            public int X { get; set; }
            public string Y { get; set; }
            public ComplexSerializableStuff Stuff { get; set; }

            public override string ToString() => $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Stuff)}: {Stuff}";

            public bool Equals(ComplexSerializableStuff other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && string.Equals(Y, other.Y) && Equals(Stuff, other.Stuff);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((ComplexSerializableStuff) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = X;
                    hashCode = (hashCode * 397) ^ (Y?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ (Stuff?.GetHashCode() ?? 0);
                    return hashCode;
                }
            }
        }
    }
}