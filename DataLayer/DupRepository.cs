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
        
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    public async Task<bool> DupExists(int id)
    {
        var sqlString = $@"select exists (select 1 from rms.dups where id = '{id}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DupAttributeExists(int dupId, string typeName, int id)
    {
        var sqlString = $@"select exists (select 1 from {_typeList[typeName]}
                              where id = {id.ToString()} and dup_id = {dupId.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DupObjectExists(int dupId, string sdOid)
    {
        var sqlString = $@"select exists (select 1 from rms.dup_objects
                              where dup_id = {dupId.ToString()} and sd_oid = '{sdOid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DupObjectAttributeExists(int dtpId, string sdOid, string typeName, int id)
    {
        var sqlString = $@"select exists (select 1 from {_typeList[typeName]} 
                              where dup_id = {dtpId.ToString()} 
                              and sd_oid = '{sdOid}'
                              and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    /****************************************************************
    * All DUPs / DUP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DupInDb>> GetAllDups()
    {
        var sqlString = $"select * from rms.dups";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    public async Task<IEnumerable<DupEntryInDb>> GetAllDupEntries()
    {
        var sqlString = $"select id, sd_sid, display_title from rms.dups";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Paginated DUPs / DUP entries
    ****************************************************************/

    public async Task<IEnumerable<DupInDb>> GetPaginatedDupData(int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select * from rms.dups
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    public async Task<IEnumerable<DupEntryInDb>> GetPaginatedDupEntries(int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select id, sd_sid, display_title from rms.dups
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }

    /****************************************************************
    * Filtered DUPs / DUP entries
    ****************************************************************/   
    
    public async Task<IEnumerable<DupInDb>> GetFilteredDupData(string titleFilter)
    {
        var sqlString = $@"select * from rms.dups
                            where display_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    public async Task<IEnumerable<DupEntryInDb>> GetFilteredDupEntries(string titleFilter)
    {
        var sqlString = $@"select id, sd_sid, display_title from rms.dups
                            where display_title ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }    
    
    /****************************************************************
    * Paginated and filtered DUPs / DUP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DupInDb>> GetPaginatedFilteredDupData(string titleFilter, int pNum,
        int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select * from rms.dups
                              where display_name ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DupEntryInDb>> GetPaginatedFilteredDupEntries(string titleFilter, int pNum,
        int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select id, sd_sid, display_title from rms.dups
                              where display_title ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Recent DUPs / DUP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DupInDb>> GetRecentDups(int n)
    {
        var sqlString = $@"select * from rms.dups
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DupEntryInDb>> GetRecentDupEntries(int n)
    {
        var sqlString = $@"select id, sd_sid, display_title from rms.dups
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * DUPs / DUP entries by Organisation
    ****************************************************************/   
    
    public async Task<IEnumerable<DupInDb>> GetDupsByOrg(int orgId)
    {
        var sqlString = $@"select * from rms.dups
                           where org_id = {orgId.ToString()} 
                           order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DupEntryInDb>> GetDupEntriesByOrg(int orgId)
    {
        var sqlString = $@"select id, sd_sid, display_title from rms.dups
                           where org_id = {orgId.ToString()} 
                           order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Get single DUP record
    ****************************************************************/        
    
    public async Task<DupInDb?> GetDup(int dupId)
    {
        var sqlString = $"select * from rms.dups where id = {dupId}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupInDb>(sqlString);
    }
 
    /****************************************************************
    * Update DUP records
    ****************************************************************/ 
    
    public async Task<DupInDb?> CreateDup(DupInDb dupContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupContent);
        var sqlString = $"select * from rms.dups where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupInDb>(sqlString);
    }

    public async Task<DupInDb?> UpdateDup(DupInDb dupContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupContent)) ? dupContent : null;
    }

    public async Task<int> DeleteDup(int dupId)
    {
        var sqlString = $"delete from rms.dups where id = {dupId.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    /****************************************************************
    * DUP statistics
    ****************************************************************/
    
    public async Task<int> GetTotalDups()
    {
        var sqlString = $@"select count(*) from rms.dups;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetTotalFilteredDups(string titleFilter)
    {
        var sqlString = $@"select count(*) from rms.dups
                             where display_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetCompletedDups()
    {
        // completed status id = 16
        var sqlString = $@"select count(*) from rms.dups
                           where status_id = 16;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<IEnumerable<StatisticInDb>> GetDupsByStatus()
    {
        var sqlString = $@"select status_id as stat_type, 
                             count(id) as stat_value 
                             from rms.dups group by status_id;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StatisticInDb>(sqlString);
    }
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupStudyInDb>> GetAllDupStudies(int dupId)
    {
        var sqlString = $"select * from rms.dup_studies where dup_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupStudyInDb>(sqlString);
    }

    public async Task<DupStudyInDb?> GetDupStudy(int id)
    {
        var sqlString = $"select * from rms.dup_studies where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupStudyInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupStudyInDb?> CreateDupStudy(DupStudyInDb dupStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupStudyContent);
        var sqlString = $"select * from rms.dup_studies where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupStudyInDb>(sqlString);
    }

    public async Task<DupStudyInDb?> UpdateDupStudy(DupStudyInDb dupStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupStudyContent)) ? dupStudyContent : null;
    }

    public async Task<int> DeleteDupStudy(int id)
    {
        var sqlString = $@"delete from mdr.dup_studies where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupObjectInDb>> GetAllDupObjects(int dupId)
    {
        var sqlString = $"select * from rms.dup_objects where dup_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupObjectInDb>(sqlString);
    }

    public async Task<DupObjectInDb?> GetDupObject(int id)
    {
        var sqlString = $"select * from rms.dup_objects where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupObjectInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupObjectInDb?> CreateDupObject(DupObjectInDb dupObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupObjectContent);
        var sqlString = $"select * from rms.dup_objects where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupObjectInDb>(sqlString);
    }

    public async Task<DupObjectInDb?> UpdateDupObject(DupObjectInDb dupObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupObjectContent)) ? dupObjectContent : null;
    }

    public async Task<int> DeleteDupObject(int id)
    {
        var sqlString = $@"delete from mdr.dup_objects where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DuaInDb>> GetAllDuas(int dupId)
    {
        var sqlString = $"select * from rms.duas where dup_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DuaInDb>(sqlString);
    }

    public async Task<DuaInDb?> GetDua(int id)
    {
        var sqlString = $"select * from rms.duas where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DuaInDb>(sqlString);
    }
 
    // Update data
    public async Task<DuaInDb?> CreateDua(DuaInDb duaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(duaContent);
        var sqlString = $"select * from rms.duas where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DuaInDb>(sqlString);
    }

    public async Task<DuaInDb?> UpdateDua(DuaInDb duaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(duaContent)) ? duaContent : null;
    }

    public async Task<int> DeleteDua(int id)
    {
        var sqlString = $@"delete from mdr.duas where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Access pre-requisites
    ****************************************************************/
    // Fetch data
    public async Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqs(int dupId, string sdOid)
    {
        var sqlString = $"select * from rms.dup_prereqs where dup_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPrereqInDb>(sqlString);
    }

    public async Task<DupPrereqInDb?> GetDupPrereq(int id)
    {
        var sqlString = $"select * from rms.dup_prereqs where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPrereqInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupPrereqInDb?> CreateDupPrereq(DupPrereqInDb dupPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupPrereqContent);
        var sqlString = $"select * from rms.dup_prereqs where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupPrereqInDb>(sqlString);
    }

    public async Task<DupPrereqInDb?> UpdateDupPrereq(DupPrereqInDb dupPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupPrereqContent)) ? dupPrereqContent : null;
    }

    public async Task<int> DeleteDupPrereq(int id)
    {
        var sqlString = $@"delete from mdr.dup_prereqs where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  

    /****************************************************************
     * DUP notes
     ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupNoteInDb>> GetAllDupNotes(int dupId)
    {
        var sqlString = $"select * from rms.dup_notes where dup_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupNoteInDb>(sqlString);
    }

    public async Task<DupNoteInDb?> GetDupNote(int id)
    {
        var sqlString = $"select * from rms.dup_notes where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupNoteInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupNoteInDb?> CreateDupNote(DupNoteInDb dupNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupNoteContent);
        var sqlString = $"select * from rms.dup_notes where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupNoteInDb>(sqlString);
    }

    public async Task<DupNoteInDb?> UpdateDupNote(DupNoteInDb dupNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupNoteContent)) ? dupNoteContent : null;
    }

    public async Task<int> DeleteDupNote(int id)
    {
        var sqlString = $@"delete from mdr.dup_notes where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 

    /****************************************************************
    * DUP people
    ****************************************************************/

    // Fetch data 
    public async Task<IEnumerable<DupPersonInDb>> GetAllDupPeople(int dupId)
    {
        var sqlString = $"select * from rms.dup_people where dtp_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPersonInDb>(sqlString);
    }

    public async Task<DupPersonInDb?> GetDupPerson(int id)
    {
        var sqlString = $"select * from rms.dup_people where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPersonInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupPersonInDb?> CreateDupPerson(DupPersonInDb dupPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupPersonContent);
        var sqlString = $"select * from rms.dup_people where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupPersonInDb>(sqlString);
    }

    public async Task<DupPersonInDb?> UpdateDupPerson(DupPersonInDb dupPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dupPersonContent)) ? dupPersonContent : null;
    }

    public async Task<int> DeleteDupPerson(int id)
    {
        var sqlString = $@"delete from mdr.dup_people where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
    /****************************************************************
    *Secondary use
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<SecondaryUseInDb>> GetAllSecUses(int dupId)
    {
        var sqlString = $"select * from rms.dup_sec_uses where dup_id = '{dupId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<SecondaryUseInDb>(sqlString);
    }

    public async Task<SecondaryUseInDb?> GetSecUse(int id)
    {
        var sqlString = $"select * from rms.dup_sec_uses where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<SecondaryUseInDb>(sqlString);
    }
 
    // Update data
    public async Task<SecondaryUseInDb?> CreateSecUse(SecondaryUseInDb secUseContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(secUseContent);
        var sqlString = $"select * from rms.dup_sec_uses where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<SecondaryUseInDb>(sqlString);
    }

    public async Task<SecondaryUseInDb?> UpdateSecUse(SecondaryUseInDb secUseContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(secUseContent)) ? secUseContent : null;
    }

    public async Task<int> DeleteSecUse(int id)
    {
        var sqlString = $@"delete from mdr.dup_sec_uses where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
}
