using System;
using System.Data;

namespace Quarks.DataAccess.Db
{
    /// <summary>
    /// An object that represents a database query.
    /// </summary>
    /// <see href="http://www.martinfowler.com/eaaCatalog/queryObject.html"/>
    public class QueryObject
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
    }
}