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

    /****************************************************************
    * Obtains the number of records in the table
    ****************************************************************/
    
    public async Task<int> GetTotal(string tableName)
    {
        string sqlString = $@"select count(*) from {tableName};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    /****************************************************************
    * First obtains the max value of the id field in the DB table
    * Stores that value in a record in the test_data table, so it can
    * be used later to mark the beginning of the test data (or rather
    * the last non-test data record,'old_max'), but note that any
    * pre-existing 'old max' values must first be deleted.
    * Empty tables return 0.
    *
    * The assumption is that this call will be made JUST BEFORE the
    * addition / generation of new test data
    ****************************************************************/
    
    public async Task<int> GetMaxId(string tableName)
    {
        string sqlString = $@"select max(id) from {tableName};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        int res = await conn.ExecuteScalarAsync<int?>(sqlString) ?? 0;
        sqlString = $@"delete from rms.test_data
                       where table_name = '{tableName}' 
                       and comments = 'old_max';
                       insert into rms.test_data(table_name, id_in_table, comments)
                       values('{tableName}', {res.ToString()}, 'old_max')";
        await conn.ExecuteAsync(sqlString);
        return res;
    }
    
    /****************************************************************
    * Retrieves the old_max value for this table.
    * Stores the ids of test data records in the test data table.
    * These are presumed to be those records with ids greater than
    * the old_max value.
    *
    * The assumption is that this call will be made JUST AFTER the
    * addition / generation of new test data
    ****************************************************************/
   
    public async Task<int> StoreNewIds(string tableName)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        string sqlString = $@"select id_in_table from rms.test_data
                              where table_name = '{tableName}' 
                              and comments = 'old_max';";
        var oldMaxId = await conn.ExecuteScalarAsync<int>(sqlString);
        sqlString = $@"insert into rms.test_data(table_name, id_in_table)
                        select '{tableName}', t.id from {tableName} t
                        where t.id > {oldMaxId.ToString()};";
        return await conn.ExecuteAsync(sqlString);
    }
    
    /****************************************************************
    * This call is designed to delete both the designated test data
    * and the records related to that data in the test_data table.
    *
    * The first is done by deleting the records matching those in
    * the test_data table (with the exception of the old_max record),
    * while the second is done by a simple delete of all linked
    * records in the test_data table.
    *
    * The assumption is that this call will be made JUST AFTER the
    * set of tests have been carried out,
    * OR,
    * if the test data has been added to populate the database,
    * when real data starts to be entered and the test data can be dropped.
    ****************************************************************/
    
    public async Task<int> DeleteTestData(string tableName)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        string sqlString = $@"delete from {tableName}
                              where id in 
                                    (Select id_in_table from rms.test_data
                                     where table_name = '{tableName}'
                                     and (comments is null or comments <> 'old_max'));";
        var numDeleted = await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.test_data
                              where table_name = '{tableName}';";
        await conn.ExecuteAsync(sqlString);
        return numDeleted;
    }

    
    /****************************************************************
    * This call is designed to reset the identity sequence, after
    * deletion of test data would otherwise leave a gap.
    *
    * It gets the current max id value, and resets the identity
    * sequence to that value. It then checks the process by
    * reading and returning the sequence value. This is the current
    * value of the sequence - the next Id, that is returned, is
    * obtained by incrementing that value.
    *  
    * The assumption is that this call will be made JUST AFTER a
    * call to the delete test records function above, when no other
    * (non test) data has been added. It effectively puts the table
    * back into its pre-test state.
    ****************************************************************/
    
    public async Task<int>ResetIdentitySequence(string tableName)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
      
        // get sequence name
        string sqlString = $@"select pg_get_serial_sequence('{tableName}','id')";
        string seqName = await conn.ExecuteScalarAsync<string>(sqlString);
        
        // get max id
        sqlString = $@"select max(id) from {tableName};";
        var currentMax = await conn.ExecuteScalarAsync<int?>(sqlString);

        int nextId = 1;          // default value
        if (currentMax == null) // if no records
        {
            // retrieve the start value of the sequence

            string schemaName = seqName.Substring(0, seqName.IndexOf(".", StringComparison.Ordinal));
            string shortTableName = seqName.Substring(seqName.IndexOf(".", StringComparison.Ordinal) + 1);
            sqlString = $@"select start_value from pg_sequences 
                        where schemaname = '{schemaName}' and sequencename = '{shortTableName}'";
            var startValue = await conn.ExecuteScalarAsync<int>(sqlString);
            if (startValue > 1)    // usually the case
            {
                nextId = startValue;
            }
        }
        else
        {
            nextId = ((int)currentMax) + 1;
        }
        
        // set the next value of the sequence. The false parameter means that the value
        // is not incremented before it is set. Then return the new 'next' id value
        
        sqlString = $@"SELECT setVal('{seqName}', {nextId.ToString()}, false )";
        await conn.ExecuteScalarAsync<int>(sqlString);
        return nextId;
    }
}
