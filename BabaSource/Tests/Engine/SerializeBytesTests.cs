using Core.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine
{
    [TestFixture]
    public class SerializeBytesTests
    {

        public class TestClass
        {
            public int IntProp { get; set; }
            public int IntField;
            public short ShortProp { get; set; }
            public short ShortField;

            public string? StringProp { get; set; }
            public string? StringField;

            public override bool Equals(object? obj)
            {
                if (obj is TestClass c)
                    return c.IntField== IntField && c.StringField==StringField && c.IntProp == IntProp && c.ShortProp == ShortProp && c.ShortField == ShortField && c.StringProp == StringProp;
                return base.Equals(obj);    
            }

            public override string ToString() => $"{IntProp} {IntField} {ShortProp} {ShortField} {StringProp} {StringField}";
        }

        [Test]
        public void Should_De_SerializeItems()
        {
            var input = new List<TestClass>()
            {
                new TestClass { IntProp = 1, IntField=2, ShortField=3, ShortProp=4, StringField="hi", StringProp="there" },
                new TestClass { IntProp = 10, IntField=2, ShortField=3, ShortProp=4, StringField="33333", StringProp="there2" },
                new TestClass { IntProp = 10, IntField=2, ShortField=3, ShortProp=4, StringField=null, StringProp="there2" },
                new TestClass { IntProp = 10, IntField=2, ShortField=3, ShortProp=4, StringField=null, StringProp=null },
                new TestClass { IntProp = 10, IntField=2, ShortField=3, ShortProp=4, StringField="AAAA===/ds/fsdf", StringProp="there2" },
            };
            var serialized = SerializeBytes.SerializeObjects(input);
            var deserialized = SerializeBytes.DeserializeObjects<TestClass>(serialized);
            Assert.AreEqual(input, deserialized);
        }

        [Test]
        public void JoinSerialized()
        {
            var joined = SerializeBytes.JoinSerializedStrings("hi", "thereblahfffffffffffffffffffffffffffffffffffffnewline");

            Assert.AreEqual("AgAAAAhiNQAAAAthereblahfffffffffffffffffffffffffffffffffffffnewl\nine", joined);
        }

        [Test]
        public void SplitJoinedSerialized()
        {
            var joined = "AgAAAAhiNQAAAAthereblahfffffffffffffffffffffffffffffffffffff\nnewline";
            var split = SerializeBytes.SplitSerializedStrings(joined);
            Assert.AreEqual(new string[] { "hi", "thereblahfffffffffffffffffffffffffffffffffffffnewline" }, split);
        }
    }
}
