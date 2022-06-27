using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper.Contrib;
using Dapper;

namespace rmsbe.DataLayer;

public class PeopleRepository //: IPeopleRepository
{
    private readonly string _dbConnString;

    public PeopleRepository(ICreds creds)
    {
        _dbConnString = creds.GetConnectionString("people");
    }
    
    /****************************************************************
    * Check functions for people
    ****************************************************************/
    
    public async Task<bool> PersonExistsAsync(int id)
    {
        string sqlString = $@"select exists (select 1 from rms.people
                              where id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    

}
