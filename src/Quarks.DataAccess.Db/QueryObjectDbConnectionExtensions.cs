using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;

namespace Quarks.DataAccess.Db
{
    public static class QueryObjectDbConnectionExtensions
    {
        public static int Execute(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null, TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.Execute(command);
        }

        public static IEnumerable<dynamic> Query(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null, TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.Query(command.CommandText, command.Parameters, command.Transaction, true,
                command.CommandTimeout, command.CommandType);
        }

        public static IEnumerable<T> Query<T>(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null, TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.Query<T>(command);
        }

        public static dynamic QuerySingle(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QuerySingle(command.CommandText, command.Parameters, command.Transaction,
                command.CommandTimeout, command.CommandType);
        }

        public static T QuerySingle<T>(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QuerySingle<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static dynamic QuerySingleOrDefault(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QuerySingleOrDefault(command.CommandText, command.Parameters, command.Transaction,
                command.CommandTimeout, command.CommandType);
        }

        public static T QuerySingleOrDefault<T>(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QuerySingleOrDefault<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static dynamic QueryFirst(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QueryFirst(command.CommandText, command.Parameters, command.Transaction,
                command.CommandTimeout, command.CommandType);
        }

        public static T QueryFirst<T>(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QueryFirst<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static dynamic QueryFirstOrDefault(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QueryFirstOrDefault(command.CommandText, command.Parameters, command.Transaction,
                command.CommandTimeout, command.CommandType);
        }

        public static T QueryFirstOrDefault<T>(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QueryFirstOrDefault<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static SqlMapper.GridReader QueryMultiple(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, commandTimeout);
            return connection.QueryMultiple(command);
        }

        private static CommandDefinition CreateCommandDefinition(
            QueryObject queryObject, DbTransaction transaction, TimeSpan? commandTimeout)
        {
            CommandDefinition command =
                new CommandDefinition(queryObject.Text, queryObject.Parameters, transaction, (int?)commandTimeout?.TotalSeconds,
                    queryObject.CommandType, CommandFlags.None);
            return command;
        }
    }
}