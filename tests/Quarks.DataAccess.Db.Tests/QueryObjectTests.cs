using System.Data;
using NUnit.Framework;

namespace Quarks.DataAccess.Db.Tests
{
    [TestFixture]
    public class QueryObjectTests
    {
        private const string Text = "SELECT * FROM TABLE";
        private static readonly object Parameters = new {Id = 10, Name = "name"};
        private const CommandType CommandType = System.Data.CommandType.Text;

        [Test]
        public void Can_Be_Constructed_With_Text_And_Parameters_And_CommandType()
        {
            QueryObject queryObject = new QueryObject(Text, Parameters, CommandType);

            Assert.That(queryObject.Text, Is.EqualTo(Text));
            Assert.That(queryObject.Parameters, Is.EqualTo(Parameters));
            Assert.That(queryObject.CommandType, Is.EqualTo(CommandType));
        }

        [Test]
        public void Can_Be_Constructed_With_Text_And_Parameters()
        {
            QueryObject queryObject = new QueryObject(Text, Parameters);

            Assert.That(queryObject.Text, Is.EqualTo(Text));
            Assert.That(queryObject.Parameters, Is.EqualTo(Parameters));
            Assert.That(queryObject.CommandType, Is.Null);
        }

        [Test]
        public void Can_Be_Constructed_With_Text()
        {
            QueryObject queryObject = new QueryObject(Text);

            Assert.That(queryObject.Text, Is.EqualTo(Text));
            Assert.That(queryObject.Parameters, Is.Null);
            Assert.That(queryObject.CommandType, Is.Null);
        }

        [Test]
        public void Can_Be_Constructed_With_Text_And_CommandType()
        {
            QueryObject queryObject = new QueryObject(Text, CommandType);

            Assert.That(queryObject.Text, Is.EqualTo(Text));
            Assert.That(queryObject.Parameters, Is.Null);
            Assert.That(queryObject.CommandType, Is.EqualTo(CommandType));
        }

        [Test]
        public void Same_Objects_Are_Equal()
        {
            var parameters = new {Id = 10};

            QueryObject queryObject = new QueryObject(Text, parameters, CommandType);
            QueryObject queryObject2 = new QueryObject(Text, parameters, CommandType);

            Assert.That(queryObject.Equals(queryObject), Is.True);
            Assert.That(queryObject.Equals(queryObject2), Is.True);
            Assert.That(queryObject.GetHashCode() == queryObject2.GetHashCode(), Is.True);
        }

        [Test]
        public void Different_Objects_Are_Not_Equal()
        {
            var parameters = new { Id = 10 };

            QueryObject queryObject = new QueryObject(Text, parameters, CommandType);
            QueryObject queryObject2 = new QueryObject("text", parameters, CommandType);
            QueryObject queryObject3 = new QueryObject(Text, null, CommandType);
            QueryObject queryObject4 = new QueryObject(Text, parameters, null);

            Assert.That(queryObject.Equals(queryObject2), Is.False);
            Assert.That(queryObject.Equals(queryObject3), Is.False);
            Assert.That(queryObject.Equals(queryObject4), Is.False);
            Assert.That(queryObject.GetHashCode() == queryObject2.GetHashCode(), Is.False);
            Assert.That(queryObject.GetHashCode() == queryObject3.GetHashCode(), Is.False);
            Assert.That(queryObject.GetHashCode() == queryObject4.GetHashCode(), Is.False);
        }

        [Test]
        public void Not_Equal_To_Null()
        {
            QueryObject queryObject = new QueryObject(Text, CommandType);

            Assert.That(queryObject.Equals(null), Is.False);
        }

        [Test]
        public void Not_Equal_To_OtherType_Object()
        {
            QueryObject queryObject = new QueryObject(Text, CommandType);
            object other = new object(); 

            Assert.That(queryObject.Equals(other), Is.False);
        }
    }
}