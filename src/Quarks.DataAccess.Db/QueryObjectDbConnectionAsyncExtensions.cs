using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Quarks.DataAccess.Db
{
    public static class QueryObjectDbConnectionAsyncExtensions
    {
        public static Task<int> ExecuteAsync(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
            CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
                CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.ExecuteAsync(command);
        }

        public static Task<IEnumerable<dynamic>> QueryAsync(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
            CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryAsync(command);
        }

        public static Task<IEnumerable<T>> QueryAsync<T>(
            this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
            CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryAsync<T>(command);
        }

        public static Task<dynamic> QuerySingleAsync(
           this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QuerySingleAsync(command);
        }

        public static Task<T> QuerySingleAsync<T>(
          this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
          CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QuerySingleAsync<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static Task<dynamic> QuerySingleOrDefaultAsync(
           this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
           CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QuerySingleOrDefaultAsync(command);
        }

        public static Task<T> QuerySingleOrDefaultAsync<T>(
          this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
          CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QuerySingleOrDefaultAsync<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static Task<dynamic> QueryFirstAsync(
          this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
          CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryFirstAsync(command);
        }

        public static Task<T> QueryFirstAsync<T>(
         this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
         CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryFirstAsync<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static Task<dynamic> QueryFirstOrDefaultAsync(
          this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
          CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryFirstOrDefaultAsync(command);
        }

        public static Task<T> QueryFirstOrDefaultAsync<T>(
         this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
         CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryFirstOrDefaultAsync<T>(command.CommandText, command.Parameters,
                command.Transaction, command.CommandTimeout, command.CommandType);
        }

        public static Task<SqlMapper.GridReader> QueryMultipleAsync(
          this DbConnection connection, QueryObject queryObject, DbTransaction transaction = null,
          CancellationToken cancellationToken = default(CancellationToken), TimeSpan? commandTimeout = null)
        {
            CommandDefinition command =
               CreateCommandDefinition(queryObject, transaction, cancellationToken, commandTimeout);
            return connection.QueryMultipleAsync(command);
        }

        private static CommandDefinition CreateCommandDefinition(
            QueryObject queryObject, DbTransaction transaction,
            CancellationToken cancellationToken, TimeSpan? commandTimeout)
        {
            CommandDefinition command =
                new CommandDefinition(queryObject.Text, queryObject.Parameters, transaction, (int?) commandTimeout?.TotalSeconds,
                    queryObject.CommandType, CommandFlags.None, cancellationToken);
            return command;
        }
    }
}