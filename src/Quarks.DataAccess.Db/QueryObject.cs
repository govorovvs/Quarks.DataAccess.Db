using System;
using System.Data;

namespace Quarks.DataAccess.Db
{
    /// <summary>
    /// An object that represents a database query.
    /// </summary>
    /// <see href="http://www.martinfowler.com/eaaCatalog/queryObject.html"/>
    public class QueryObject : IEquatable<QueryObject>
    {
        public QueryObject(string text, object parameters = null, CommandType? commandType = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            Text = text;
            Parameters = parameters;
            CommandType = commandType;
        }

        public QueryObject(string text, CommandType commandType) : this(text, null, commandType)
        {
        }

        public string Text { get; }

        public object Parameters { get; }

        public CommandType? CommandType { get; }

        public bool Equals(QueryObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Text, other.Text) && 
                Equals(Parameters, other.Parameters) && 
                CommandType == other.CommandType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((QueryObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Text?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Parameters?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ CommandType.GetHashCode();
                return hashCode;
            }
        }
    }
}