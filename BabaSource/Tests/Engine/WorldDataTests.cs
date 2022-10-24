using BabaGame.src.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Engine
{
    [TestFixture]
    internal class WorldDataTests
    {

        [Test]
        public void Serialize_ShouldNotHaveHyphens()
        {
            var wd = new WorldData();
            wd.SetState(new[]
            {
                new WorldObject{Kind = WorldObjectKind.Text, Name = 10, Color=WorldObjectColor.Yellow, Facing=Direction.Down, x=10, y=15},
            });

            Assert.AreEqual("", wd.Serialize());
        }
    }
}
