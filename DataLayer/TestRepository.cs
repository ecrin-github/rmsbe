using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;

namespace rmsbe.DataLayer;

public class TestRepository : ITestRepository
{
    private readonly string _dbConnString;
    
    
    public TestRepository(ICreds creds)
    {
        _dbConnString = creds.GetConnectionString("rms");
    }

    public async Task<int> GetMaxId(string tableName)
    {
        string sqlString = $@"select max(id) from {tableName};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
  
    }
    
    public async Task<int> StoreNewIds(string tableName, int oldMaxId)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        string sqlString = $@"insert into rms.test_data(table_name, id_in_table)
                        select '{tableName}', id from {tableName}
                        where id > {oldMaxId.ToString()};";
        return await conn.ExecuteAsync(sqlString);
    }
    
    public async Task<int> DeleteTestData(string tableName, int oldMaxId)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        string sqlString = $@"delete from rms.test_data
                              where table_name = {tableName}
                             and id > {oldMaxId.ToString()};";
        return await conn.ExecuteAsync(sqlString);
    }
    
    public async Task<int>SetNextId(string tableName)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        int next_id = await GetMaxId(tableName) + 1;
        string sqlString = $@"select pg_get_serial_sequence('{tableName}','id')";
        string seq_name = await conn.ExecuteScalarAsync<string>(sqlString);
        sqlString = $@"SELECT setval({seq_name}, {next_id.ToString()})";
        await conn.ExecuteScalarAsync<int>(sqlString);
        sqlString = $@"SELECT currval({seq_name})";
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
        
}
