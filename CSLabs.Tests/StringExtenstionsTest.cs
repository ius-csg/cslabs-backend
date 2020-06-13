using CSLabs.Api.Util;
using NUnit.Framework;

namespace CSLabs.Tests
{
    public class StringExtenstionsTest
    {
        [Test]
        public void TestToSafeId()
        {
            Assert.AreEqual("Test3", "Test 3".ToSafeId());
        }
    }
}