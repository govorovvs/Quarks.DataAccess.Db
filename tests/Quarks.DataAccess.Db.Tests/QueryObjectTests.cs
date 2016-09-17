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
    }
}