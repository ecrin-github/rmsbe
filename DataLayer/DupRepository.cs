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
    
    public DupRepository(ICreds creds)
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
        
        SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
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

    public async Task<bool> DupAttributeExistsAsync(int dupId, string typeName, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[typeName]}
                              where id = {id.ToString()} and dup_id = {dupId.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DupObjectExistsAsync(int dupId, string sdOid)
    {
        string sqlString = $@"select exists (select 1 from rms.dup_objects
                              where dup_id = {dupId.ToString()} and sd_oid = '{sdOid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DupObjectAttributeExistsAsync(int dtpId, string sdOid, string typeName, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[typeName]} 
                              where dup_id = {dtpId.ToString()} 
                              and sd_oid = '{sdOid}'
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
   
    public async Task<IEnumerable<DupInDb>> GetPaginatedDupDataAsync(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from rms.dups
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    
    public async Task<IEnumerable<DupInDb>> GetPaginatedFilteredDupDataAsync(string titleFilter, int pNum,
        int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from rms.dups
                              where display_name ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    
    public async Task<IEnumerable<DupInDb>> GetFilteredDupDataAsync(string titleFilter)
    {
        string sqlString = $@"select * from rms.dups
                            where display_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    
    public async Task<DupInDb?> GetDupAsync(int dupId)
    {
        string sqlString = $"select * from rms.dups where id = {dupId}";
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

    public async Task<int> DeleteDupAsync(int dupId)
    {
        string sqlString = $"delete from rms.dups where id = {dupId.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    /****************************************************************
    * Study Entries (fetching lists of id, sd_sid, display name only)
    ****************************************************************/
    
    public async Task<IEnumerable<DupEntryInDb>> GetDupEntriesAsync()
    {
        string sqlString = $"select id, sd_sid, display_title from rms.dups";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }

    
    public async Task<IEnumerable<DupEntryInDb>> GetRecentDupEntriesAsync(int n)
    {
        string sqlString = $@"select id, sd_sid, display_title from rms.dups
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }

    
    public async Task<IEnumerable<DupEntryInDb>> GetPaginatedDupEntriesAsync(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select id, sd_sid, display_title from rms.dups
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }

    
    public async Task<IEnumerable<DupEntryInDb>> GetPaginatedFilteredDupEntriesAsync(string titleFilter, int pNum,
        int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select id, sd_sid, display_title from rms.dups
                              where display_title ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }

    
    public async Task<IEnumerable<DupEntryInDb>> GetFilteredDupEntriesAsync(string titleFilter)
    {
        string sqlString = $@"select id, sd_sid, display_title from rms.dups
                            where display_title ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }
    
    
    /****************************************************************
    * DUP statistics
    ****************************************************************/
    
    public async Task<int> GetTotalDups()
    {
        string sqlString = $@"select count(*) from rms.dups;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetTotalFilteredDups(string titleFilter)
    {
        string sqlString = $@"select count(*) from rms.dups
                             where display_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetCompletedDups()
    {
        // completed status id = 16
        string sqlString = $@"select count(*) from rms.dups
                           where status_id = 16;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<IEnumerable<StatisticInDb>> GetDupsByStatus()
    {
        string sqlString = $@"select status_id as stat_type, 
                             count(id) as stat_value 
                             from rms.dups group by status_id;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StatisticInDb>(sqlString);
    }

    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupStudyInDb>> GetAllDupStudiesAsync(int dupId)
    {
        string sqlString = $"select * from rms.dup_studies where dup_id = '{dupId.ToString()}'";
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
    public async Task<IEnumerable<DupObjectInDb>> GetAllDupObjectsAsync(int dupId)
    {
        string sqlString = $"select * from rms.dup_objects where dup_id = '{dupId.ToString()}'";
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
    public async Task<IEnumerable<DuaInDb>> GetAllDuasAsync(int dupId)
    {
        string sqlString = $"select * from rms.duas where dup_id = '{dupId.ToString()}'";
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
    public async Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqsAsync(int dupId, string sdOid)
    {
        string sqlString = $"select * from rms.dup_prereqs where dup_id = '{dupId.ToString()}'";
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
    public async Task<IEnumerable<DupNoteInDb>> GetAllDupNotesAsync(int dupId)
    {
        string sqlString = $"select * from rms.dup_notes where dup_id = '{dupId.ToString()}'";
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
    public async Task<IEnumerable<DupPersonInDb>> GetAllDupPeopleAsync(int dupId)
    {
        string sqlString = $"select * from rms.dup_people where dtp_id = '{dupId.ToString()}'";
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
    public async Task<IEnumerable<SecondaryUseInDb>> GetAllSecUsesAsync(int dupId)
    {
        string sqlString = $"select * from rms.dup_sec_uses where dup_id = '{dupId.ToString()}'";
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
 
}
