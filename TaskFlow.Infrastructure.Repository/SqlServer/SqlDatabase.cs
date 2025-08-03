using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskFlow.Infrastructure.Repository.SqlServer
{
    public class SqlDatabase
    {

        private readonly string _connectionString;

        public SqlDatabase(string? connectionString)
        {
            if(connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null.");
            }

            _connectionString = connectionString;
        }

        /// <summary>
        /// Ejecuta una consulta SQL que devuelve un conjunto de resultados (SELECT)
        /// y los carga en un objeto <see cref="DataTable"/> de forma asincrónica.
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar (generalmente una instrucción SELECT).</param>
        /// <param name="parameters">Parámetros opcionales de la consulta, en formato clave-valor. Puede ser nulo.</param>
        /// <param name="cancellationToken">Token opcional para cancelar la operación asincrónica.</param>
        /// <returns>Un <see cref="DataTable"/> que contiene los datos devueltos por la consulta.</returns>
        public async Task<DataTable> ExecuteReaderAsync(string query, Dictionary<string, object?>? parameters, CancellationToken cancellationToken = default)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            AddParameters(command, parameters);
            await connection.OpenAsync(cancellationToken);
            using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        /// <summary>
        /// Ejecuta una instrucción SQL del tipo INSERT, UPDATE o DELETE de forma asincrónica,
        /// sin devolver ningún resultado, pero retornando la cantidad de filas afectadas.
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar (INSERT, UPDATE o DELETE).</param>
        /// <param name="parameters">Parámetros asociados a la consulta, en formato clave-valor. Puede ser nulo.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
        /// <returns>Un entero que representa el número de filas afectadas por la operación.</returns>

        public async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object?>? parameters, CancellationToken cancellationToken = default)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandText = query;
            AddParameters(command, parameters);
            await connection.OpenAsync(cancellationToken);
            return await command.ExecuteNonQueryAsync(cancellationToken);            
        }

        /// <summary>
        /// Ejecuta una consulta SQL de inserción que retorna un resultado (por ejemplo, mediante OUTPUT INSERTED),
        /// y lo mapea a una instancia del tipo especificado.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto al que se mapeará el resultado de la consulta.</typeparam>
        /// <param name="query">Consulta SQL de inserción que debe retornar un resultado (usualmente con cláusula OUTPUT).</param>
        /// <param name="parameters">Diccionario de parámetros para la consulta (clave: nombre del parámetro, valor: valor del parámetro).</param>
        /// <param name="map">Función que convierte un <see cref="SqlDataReader"/> en una instancia del tipo <typeparamref name="T"/>.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica si es necesario.</param>
        /// <returns>Una instancia del tipo <typeparamref name="T"/> que representa el resultado retornado por la inserción.</returns>
        /// <exception cref="Exception">Se lanza si la inserción no retorna ningún resultado.</exception>
        public async Task<T> ExecuteInsertReturningAsync<T>(
            string query,
            Dictionary<string, object?> parameters,
            Func<SqlDataReader, T> map,
            CancellationToken cancellationToken = default)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            AddParameters(command, parameters);

            await connection.OpenAsync(cancellationToken);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return map(reader);
            }

            throw new Exception("Insert no retornó ningún resultado.");
        }




        /// <summary>
        /// Ejecuta una serie de operaciones dentro de una transacción utilizando una conexión SQL abierta.
        /// Si ocurre una excepción durante la ejecución, la transacción será revertida (rollback).
        /// </summary>
        /// <param name="operations">
        /// Función asincrónica que contiene la lógica de operaciones a ejecutar, la cual recibe la conexión y la transacción como parámetros.
        /// </param>
        /// <param name="cancellationToken">
        /// Token para cancelar la operación de apertura de la conexión (no se aplica dentro de las operaciones).
        /// </param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica de ejecución de las operaciones en una transacción.
        /// </returns>

        public async Task ExecuteInTransactionAsync(Func<SqlConnection, SqlTransaction, Task> operations, CancellationToken cancellationToken)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                await operations(connection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL del tipo SELECT que retorna un único valor (escalar) y lo transforma al tipo especificado mediante una función de mapeo.
        /// Este método es útil cuando necesitas obtener un valor simple de la base de datos, como un conteo, un nombre, un estado, etc., 
        /// y deseas aplicar una transformación o conversión sobre ese valor antes de retornarlo.
        /// </summary>
        /// <typeparam name="T">Tipo de dato al que se mapeará el resultado escalar.</typeparam>
        /// <param name="sql">Consulta SQL que debe retornar un único valor.</param>
        /// <param name="parameters">Diccionario de parámetros a utilizar en la consulta (clave: nombre del parámetro, valor: valor del parámetro).</param>
        /// <param name="map">Función que convierte el resultado escalar (object) en una instancia del tipo T.</param>
        /// <returns>Una instancia del tipo T resultante de aplicar la función de mapeo al valor retornado por la consulta.</returns>

        public async Task<T> ExecuteScalarMapAsync<T>(string sql, Dictionary<string, object> parameters, Func<object, T> map)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            foreach (var param in parameters)
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            return map(result);
        }


        /// <summary>
        /// Ejecuta una consulta SQL que retorna un solo registro (fila), y lo mapea a un objeto del tipo especificado.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto al que se mapeará el resultado de la consulta.</typeparam>
        /// <param name="query">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Diccionario con los parámetros de la consulta (clave: nombre del parámetro, valor: valor del parámetro).</param>
        /// <param name="map">Función que convierte un SqlDataReader en una instancia del tipo T.</param>
        /// <returns>Una instancia del tipo T si se encuentra un registro; de lo contrario, null.</returns>
        public async Task<T?> ExecuteSingleAsync<T>(string query, Dictionary<string, object?>? parameters, Func<SqlDataReader, T> map)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            AddParameters(command, parameters);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return map(reader);
            }

            return default;
        }

        /// <summary>
        /// Encargado del registro de parametros en el comando. Centralizamos la logica de registro en este unico método.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        private static void AddParameters(SqlCommand command, Dictionary<string, object?>? parameters)
        {
            if (parameters == null) return;

            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }


    }
}
