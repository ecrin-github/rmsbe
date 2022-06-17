using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;
using Dapper.Contrib.Extensions;

namespace rmsbe.DataLayer;

public class DupRepository : IDupRepository
{
    private readonly string _dbConnString;
    private readonly Dictionary<string, string> _typeList;
    
    public DupRepository(ICredentials creds)
    {
        _dbConnString = creds.GetConnectionString("rms");
        
        // set up dictionary of table name equivalents for type parameter
        _typeList = new Dictionary<string, string>
        {
            { "DupStudy", "rms.dup_studies" },  
            { "DupObject", "rms.dup_objects" },           
            { "Dua", "rms.dup_duas" },
            { "DupPrereq", "rms.dup_prereqs" },
            { "DupNote", "rms.dup_notes" },           
            { "DupPerson", "rms.dup_people" },
            { "SecondaryUse", "rms.dup_sec_uses" }
        };
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    public async Task<bool> DupExistsAsync(int id)
    {
        string sqlString = $@"select exists (select 1 from rms.dups where id = '{id}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DupAttributeExistsAsync(int dup_id, string type_name, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[type_name]}
                              where id = {id.ToString()} and dup_id = {dup_id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DupObjectExistsAsync(int dup_id, string sd_oid)
    {
        string sqlString = $@"select exists (select 1 from rms.dup_objects
                              where dup_id = {dup_id.ToString()} and sd_oid = '{sd_oid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DupObjectAttributeExistsAsync(int dtp_id, string sd_oid, string type_name, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[type_name]} 
                              where dup_id = {dtp_id.ToString()} 
                              and sd_oid = '{sd_oid}'
                              and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DupInDb>> GetAllDupsAsync()
    {
        string sqlString = $"select * from rms.dups";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    public async Task<IEnumerable<DupInDb>> GetRecentDupsAsync(int n)
    {
        string sqlString = $@"select * from rms.dups
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }
   
    public async Task<DupInDb?> GetDupAsync(int dup_id)
    {
        string sqlString = $"select * from rms.dups where dup_id = {dup_id}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupInDb?> CreateDupAsync(DupInDb dupContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dupContent);
        string sqlString = $"select * from rms.dups where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupInDb>(sqlString);
    }

    public async Task<DupInDb?> UpdateDupAsync(DupInDb dupContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupContent)) ? dupContent : null;
    }

    public async Task<int> DeleteDupAsync(int dup_id)
    {
        string sqlString = $"delete from rms.dups where dtp_id = {dup_id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupStudyInDb>> GetAllDupStudiesAsync(int dup_id)
    {
        string sqlString = $"select * from rms.dup_studies where dup_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupStudyInDb>(sqlString);
    }

    public async Task<DupStudyInDb?> GetDupStudyAsync(int id)
    {
        string sqlString = $"select * from rms.dup_studies where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupStudyInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupStudyInDb?> CreateDupStudyAsync(DupStudyInDb dupStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dupStudyContent);
        string sqlString = $"select * from rms.dup_studies where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupStudyInDb>(sqlString);
    }

    public async Task<DupStudyInDb?> UpdateDupStudyAsync(DupStudyInDb dupStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupStudyContent)) ? dupStudyContent : null;
    }

    public async Task<int> DeleteDupStudyAsync(int id)
    {
        string sqlString = $@"delete from mdr.dup_studies where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupObjectInDb>> GetAllDupObjectsAsync(int dup_id)
    {
        string sqlString = $"select * from rms.dup_objects where dup_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupObjectInDb>(sqlString);
    }

    public async Task<DupObjectInDb?> GetDupObjectAsync(int id)
    {
        string sqlString = $"select * from rms.dup_objects where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupObjectInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupObjectInDb?> CreateDupObjectAsync(DupObjectInDb dupObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dupObjectContent);
        string sqlString = $"select * from rms.dup_objects where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupObjectInDb>(sqlString);
    }

    public async Task<DupObjectInDb?> UpdateDupObjectAsync(DupObjectInDb dupObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupObjectContent)) ? dupObjectContent : null;
    }

    public async Task<int> DeleteDupObjectAsync(int id)
    {
        string sqlString = $@"delete from mdr.dup_objects where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DuaInDb>> GetAllDuasAsync(int dup_id)
    {
        string sqlString = $"select * from rms.duas where dup_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DuaInDb>(sqlString);
    }

    public async Task<DuaInDb?> GetDuaAsync(int id)
    {
        string sqlString = $"select * from rms.duas where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DuaInDb>(sqlString);
    }
 
    // Update data
    public async Task<DuaInDb?> CreateDuaAsync(DuaInDb duaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(duaContent);
        string sqlString = $"select * from rms.duas where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DuaInDb>(sqlString);
    }

    public async Task<DuaInDb?> UpdateDuaAsync(DuaInDb duaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(duaContent)) ? duaContent : null;
    }

    public async Task<int> DeleteDuaAsync(int id)
    {
        string sqlString = $@"delete from mdr.duas where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Access pre-requisites
    ****************************************************************/
    // Fetch data
    public async Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqsAsync(int dup_id, string sd_oid)
    {
        string sqlString = $"select * from rms.dup_prereqs where dup_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPrereqInDb>(sqlString);
    }

    public async Task<DupPrereqInDb?> GetDupPrereqAsync(int id)
    {
        string sqlString = $"select * from rms.dup_prereqs where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPrereqInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupPrereqInDb?> CreateDupPrereqAsync(DupPrereqInDb dupPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dupPrereqContent);
        string sqlString = $"select * from rms.dup_prereqs where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupPrereqInDb>(sqlString);
    }

    public async Task<DupPrereqInDb?> UpdateDupPrereqAsync(DupPrereqInDb dupPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupPrereqContent)) ? dupPrereqContent : null;
    }

    public async Task<int> DeleteDupPrereqAsync(int id)
    {
        string sqlString = $@"delete from mdr.dup_prereqs where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  

    /****************************************************************
     * DUP notes
     ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupNoteInDb>> GetAllDupNotesAsync(int dup_id)
    {
        string sqlString = $"select * from rms.dup_notes where dup_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupNoteInDb>(sqlString);
    }

    public async Task<DupNoteInDb?> GetDupNoteAsync(int id)
    {
        string sqlString = $"select * from rms.dup_notes where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupNoteInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupNoteInDb?> CreateDupNoteAsync(DupNoteInDb dupNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dupNoteContent);
        string sqlString = $"select * from rms.dup_notes where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupNoteInDb>(sqlString);
    }

    public async Task<DupNoteInDb?> UpdateDupNoteAsync(DupNoteInDb dupNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupNoteContent)) ? dupNoteContent : null;
    }

    public async Task<int> DeleteDupNoteAsync(int id)
    {
        string sqlString = $@"delete from mdr.dup_notes where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 

    /****************************************************************
    * DUP people
    ****************************************************************/

    // Fetch data 
    public async Task<IEnumerable<DupPersonInDb>> GetAllDupPeopleAsync(int dup_id)
    {
        string sqlString = $"select * from rms.dup_people where dtp_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPersonInDb>(sqlString);
    }

    public async Task<DupPersonInDb?> GetDupPersonAsync(int id)
    {
        string sqlString = $"select * from rms.dup_people where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPersonInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupPersonInDb?> CreateDupPersonAsync(DupPersonInDb dupPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dupPersonContent);
        string sqlString = $"select * from rms.dup_people where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupPersonInDb>(sqlString);
    }

    public async Task<DupPersonInDb?> UpdateDupPersonAsync(DupPersonInDb dupPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupPersonContent)) ? dupPersonContent : null;
    }

    public async Task<int> DeleteDupPersonAsync(int id)
    {
        string sqlString = $@"delete from mdr.dup_people where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
    
    /****************************************************************
    *Secondary use
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<SecondaryUseInDb>> GetAllSecUsesAsync(int dup_id)
    {
        string sqlString = $"select * from rms.dup_sec_uses where dup_id = '{dup_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<SecondaryUseInDb>(sqlString);
    }

    public async Task<SecondaryUseInDb?> GetSecUseAsync(int id)
    {
        string sqlString = $"select * from rms.dup_sec_uses where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<SecondaryUseInDb>(sqlString);
    }
 
    // Update data
    public async Task<SecondaryUseInDb?> CreateSecUseAsync(SecondaryUseInDb secUseContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(secUseContent);
        string sqlString = $"select * from rms.dup_sec_uses where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<SecondaryUseInDb>(sqlString);
    }

    public async Task<SecondaryUseInDb?> UpdateSecUseAsync(SecondaryUseInDb secUseContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(secUseContent)) ? secUseContent : null;
    }

    public async Task<int> DeleteSecUseAsync(int id)
    {
        string sqlString = $@"delete from mdr.dup_sec_uses where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    // Statistics
    /*
    public async Task<PaginationResponse<DupDto>> PaginateDup(PaginationRequest paginationRequest);
    public async Task<PaginationResponse<DupDto>> FilterDupByTitle(FilteringByTitleRequest filteringByTitleRequest);
    public async Task<int> GetTotalDup();
    public async Task<int> GetUncompletedDup();
    */
    
    
}
