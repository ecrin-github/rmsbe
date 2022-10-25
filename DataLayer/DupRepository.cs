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
            { "SecondaryUse", "rms.dup_sec_use" }
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

    public async Task<bool> DupDuaExists(int dupId)
    {
        var sqlString = $@"select exists (select 1 from rms.duas
                              where dup_id = {dupId.ToString()})";
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
    
    public async Task<bool> DupObjectAttributeExists(int dupId, string sdOid, string typeName, int id)
    {
        var sqlString = $@"select exists (select 1 from {_typeList[typeName]} 
                              where dup_id = {dupId.ToString()} 
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
        var sqlString = $@"select * from rms.dups 
                           order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    public async Task<IEnumerable<DupEntryInDb>> GetAllDupEntries()
    {
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dups d
                           left join lup.dup_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           order by d.created_on desc";
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
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dups d
                           left join lup.dup_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           order by d.created_on desc
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
                            where display_name ilike '%{titleFilter}%'
                            order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupInDb>(sqlString);
    }

    public async Task<IEnumerable<DupEntryInDb>> GetFilteredDupEntries(string titleFilter)
    {
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dups d
                           left join lup.dup_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           where display_name ilike '%{titleFilter}%'
                           order by d.created_on DESC";
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
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dups d
                           left join lup.dup_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           where display_name ilike '%{titleFilter}%'
                           order by d.created_on DESC
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
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dups d
                           left join lup.dup_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           order by d.created_on DESC
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
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dups d
                           left join lup.dup_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           where org_id = {orgId.ToString()} 
                           order by d.created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupEntryInDb>(sqlString);
    }
    
    
    /****************************************************************
    * Fetch / delete Full DUP records, with attributes
    ****************************************************************/   
    
    public async Task<FullDupInDb?> GetFullDupById(int id)
    {
        // fetch data
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $"select * from rms.dups where id = {id.ToString()}";   
        DupInDb? coreDup = await conn.QueryFirstOrDefaultAsync<DupInDb>(sqlString);     
        sqlString = $"select * from rms.duas where dup_id = {id.ToString()}";
        var duasInDb = (await conn.QueryAsync<DuaInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dup_studies where dup_id = {id.ToString()}";
        var dupStudiesInDb = (await conn.QueryAsync<DupStudyInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dup_objects where dup_id = {id.ToString()}";
        var dupObjectsInDb = (await conn.QueryAsync<DupObjectInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dup_prereqs where dup_id = {id.ToString()}";
        var dupPrereqsInDb = (await conn.QueryAsync<DupPrereqInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dup_sec_use where dup_id = {id.ToString()}";
        var dupSecUseInDb = (await conn.QueryAsync<DupSecondaryUseInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dup_notes where dup_id = {id.ToString()}";
        var dupNotesInDb = (await conn.QueryAsync<DupNoteInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dup_people where dup_id = {id.ToString()}";
        var dupPeopleInDb = (await conn.QueryAsync<DupPersonInDb>(sqlString)).ToList();

        return new FullDupInDb(coreDup, duasInDb, dupStudiesInDb, dupObjectsInDb,
            dupPrereqsInDb, dupSecUseInDb, dupNotesInDb, dupPeopleInDb);
    } 
    
    // delete data
    public async Task<int> DeleteFullDup(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $@"delete from rms.dup_people where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dup_notes where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dup_sec_use where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dup_prereqs where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dup_objects where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dup_studies where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.duas where dup_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);     
        sqlString = $@"delete from rms.dups where id = {id.ToString()};";
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
    * Get single DUP record
    ****************************************************************/        
    
    public async Task<DupInDb?> GetDup(int dupId)
    {
        var sqlString = $"select * from rms.dups where id = {dupId}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupInDb>(sqlString);
    }
    
    public async Task<DupOutInDb?> GetOutDup(int dupId)
    {
        var sqlString = $@"select d.id, d.org_id, 
                           g.default_name as org_name,
                           d.display_name, d.status_id, 
                           ds.name as status_name, 
                           d.initial_contact_date, d.set_up_completed,
                           d.prereqs_met, d.dua_agreed_date,
                           d.availability_requested, 
                           d.availability_confirmed, d.access_confirmed
                           from rms.dups d
                           left join lup.organisations g
                           on d.org_id = g.id
                           left join lup.dup_status_types ds
                           on d.status_id = ds.id
                           where d.id = {dupId}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupOutInDb>(sqlString);
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
    * DUP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupStudyInDb>> GetAllDupStudies(int dupId)
    {
        var sqlString = $"select * from rms.dup_studies where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupStudyInDb>(sqlString);
    }

    public async Task<IEnumerable<DupStudyOutInDb>> GetAllOutDupStudies(int dupId)
    {
        var sqlString = $@"select d.id, d.dup_id, d.sd_sid,
                        s.display_title as study_name
                        from rms.dup_studies d
                        left join mdr.studies s 
                        on d.sd_sid = s.sd_sid
                        where d.dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupStudyOutInDb>(sqlString);
    }
    
    public async Task<DupStudyInDb?> GetDupStudy(int id)
    {
        var sqlString = $"select * from rms.dup_studies where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupStudyInDb>(sqlString);
    }
    
    public async Task<DupStudyOutInDb?> GetOutDupStudy(int id)
    {
        var sqlString = $@"select d.id, d.dup_id, d.sd_sid,
                        s.display_title as study_name
                        from rms.dup_studies d
                        left join mdr.studies s 
                        on d.sd_sid = s.sd_sid
                        where d.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupStudyOutInDb>(sqlString);
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
        
        // ensure the study sd_sid is present
        if (dupStudyContent.sd_sid == null || dupStudyContent.sd_sid.Trim() == "")
        {
            var sqlString = $"select sd_sid from rms.dup_studies where id = {dupStudyContent.id.ToString()}";
            string? res = await conn.QueryFirstOrDefaultAsync<string?>(sqlString);
            if (res != null)
            {
                dupStudyContent.sd_sid = res;
            }
        }
        return (await conn.UpdateAsync(dupStudyContent)) ? dupStudyContent : null;
    }

    public async Task<int> DeleteDupStudy(int id)
    {
        var sqlString = $@"delete from rms.dup_studies where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupObjectInDb>> GetAllDupObjects(int dupId)
    {
        var sqlString = $"select * from rms.dup_objects where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupObjectInDb>(sqlString);
    }

    public async Task<IEnumerable<DupObjectOutInDb>> GetAllOutDupObjects(int dupId)
    {
        var sqlString = $@"select d.id, d.dup_id, d.sd_oid,
                           b.display_title as object_name,
                           d.access_type_id, 
                           at.name as access_type_name,
                           d.access_details, d.notes
                           from rms.dup_objects d
                           left join mdr.data_objects b 
                           on d.sd_oid = b.sd_oid
                           left join lup.repo_access_types at
                           on d.access_type_id = at.id
                           where d.dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupObjectOutInDb>(sqlString);
    }
    
    public async Task<DupObjectInDb?> GetDupObject(int id)
    {
        var sqlString = $"select * from rms.dup_objects where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupObjectInDb>(sqlString);
    }
 
    public async Task<DupObjectOutInDb?> GetOutDupObject(int id)
    {
        var sqlString = $@"select d.id, d.dup_id, d.sd_oid,
                           b.display_title as object_name,
                           d.access_type_id, 
                           at.name as access_type_name,
                           d.access_details, d.notes
                           from rms.dup_objects d
                           left join mdr.data_objects b 
                           on d.sd_oid = b.sd_oid
                           left join lup.repo_access_types at
                           on d.access_type_id = at.id
                           where d.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupObjectOutInDb>(sqlString);
    }

    // Update data
    public async Task<DupObjectInDb?> CreateDupObject(DupObjectInDb dupObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dupObjectContent);
        var sqlString = $"select * from rms.dup_objects where id = {id.ToString()}";
        var res = await conn.QueryFirstOrDefaultAsync<DupObjectInDb>(sqlString);
        
        // If addition has been successful then any set of pre-requisites linked 
        // to this object need to be represented as a set of DUP pre-requisites
        if (res != null)
        {
            string? thisObject = dupObjectContent.sd_oid;
            int thisDup = dupObjectContent.dup_id;
            sqlString = $@"insert into rms.dup_prereqs (dup_id, sd_oid, pre_requisite_id, pre_requisite_notes)
                           select {thisDup.ToString()}, '{thisObject}', pre_requisite_type_id, pre_requisite_notes
                           from rms.dtp_prereqs p where p.sd_oid = '{thisObject}'";
            await conn.ExecuteAsync(sqlString);
        }
        return res;
    }
    
    public async Task<DupObjectInDb?> UpdateDupObject(DupObjectInDb dupObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        // ensure the object sd_oid is present
        if (dupObjectContent.sd_oid == null || dupObjectContent.sd_oid.Trim() == "")
        {
            var sqlString = $"select sd_oid from rms.dup_objects where id = {dupObjectContent.id.ToString()}";
            string? res = await conn.QueryFirstOrDefaultAsync<string?>(sqlString);
            if (res != null)
            {
                dupObjectContent.sd_oid = res;
            }
        }
        return (await conn.UpdateAsync(dupObjectContent)) ? dupObjectContent : null;
    }

    public async Task<int> DeleteDupObject(int id)
    {
        var sqlString = $@"delete from rms.dup_objects where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    public async Task<DuaInDb?> GetDua(int dupId)
    {
        var sqlString = $"select * from rms.duas where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DuaInDb>(sqlString);
    }
    
    public async Task<DuaOutInDb?> GetOutDua(int dupId)
    {
        var sqlString = $@"select d.id, d.dup_id, d.conforms_to_default,
                           d.variations, dua_file_path, 
                           d.repo_signatory_1,
                           LTRIM(COALESCE(p1.given_name, '')||' '||p1.family_name) as repo_signatory_1_name,
                           d.repo_signatory_2,
                           LTRIM(COALESCE(p2.given_name, '')||' '||p2.family_name) as repo_signatory_2_name,
                           d.provider_signatory_1,
                           LTRIM(COALESCE(p3.given_name, '')||' '||p3.family_name) as provider_signatory_1_name,
                           d.provider_signatory_2,
                           LTRIM(COALESCE(p4.given_name, '')||' '||p4.family_name) as provider_signatory_2_name,
                           d.requester_signatory_1,
                           LTRIM(COALESCE(p5.given_name, '')||' '||p5.family_name) as requester_signatory_1_name,
                           d.requester_signatory_2,
                           LTRIM(COALESCE(p6.given_name, '')||' '||p6.family_name) as requester_signatory_2_name,
                           d.notes
                           from rms.duas d
                           left join rms.people p1
                           on d.repo_signatory_1 = p1.id
                           left join rms.people p2
                           on d.repo_signatory_2 = p2.id
                           left join rms.people p3
                           on d.provider_signatory_1 = p3.id
                           left join rms.people p4
                           on d.provider_signatory_2 = p4.id
                           left join rms.people p5
                           on d.requester_signatory_1 = p5.id
                           left join rms.people p6
                           on d.requester_signatory_2 = p6.id
                           where d.dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DuaOutInDb>(sqlString);
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
        var sqlString = $@"select id from rms.duas 
                           where dup_id = {duaContent.dup_id.ToString()}";
        duaContent.id = await conn.QueryFirstOrDefaultAsync<int>(sqlString);
        return (await conn.UpdateAsync(duaContent)) ? duaContent : null;
    }

    public async Task<int> DeleteDua(int dupId)
    {
        var sqlString = $@"delete from rms.duas where dup_id = {dupId.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DUP Access pre-requisites
    ****************************************************************/
    // Fetch data
    public async Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqs(int dupId, string sdOid)
    {
        var sqlString = $"select * from rms.dup_prereqs where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPrereqInDb>(sqlString);
    }

    public async Task<IEnumerable<DupPrereqOutInDb>> GetAllOutDupPrereqs(int dupId, string sdOid)
    {
        var sqlString = $@"select p.id, p.dup_id, p.sd_oid,
                           b.display_title as object_name,
                           p.pre_requisite_id,
                           t.name as pre_requisite_name,
                           p.pre_requisite_notes,
                           p.pre_requisite_met, p.met_notes
                           from rms.dup_prereqs p 
                           left join mdr.data_objects b 
                           on p.sd_oid = b.sd_oid
                           left join lup.prereq_types t
                           on p.pre_requisite_id = t.id
                           where p.dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPrereqOutInDb>(sqlString);
    }
    
    public async Task<DupPrereqInDb?> GetDupPrereq(int id)
    {
        var sqlString = $"select * from rms.dup_prereqs where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPrereqInDb>(sqlString);
    }
    
    public async Task<DupPrereqOutInDb?> GetOutDupPrereq(int id)
    {
        var sqlString = $@"select p.id, p.dup_id, p.sd_oid,
                           b.display_title as object_name,
                           p.pre_requisite_id,
                           t.name as pre_requisite_name,
                           p.pre_requisite_notes,
                           p.pre_requisite_met, p.met_notes
                           from rms.dup_prereqs p 
                           left join mdr.data_objects b 
                           on p.sd_oid = b.sd_oid
                           left join lup.prereq_types t
                           on p.pre_requisite_id = t.id
                           where p.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPrereqOutInDb>(sqlString);
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
        var sqlString = $@"delete from rms.dup_prereqs where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  

    /****************************************************************
     * DUP notes
     ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DupNoteInDb>> GetAllDupNotes(int dupId)
    {
        var sqlString = $"select * from rms.dup_notes where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupNoteInDb>(sqlString);
    }

    public async Task<IEnumerable<DupNoteOutInDb>> GetAllOutDupNotes(int dupId)
    {
        var sqlString = $@"select n.id, n.dup_id, n.text, n.author, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as author_name, 
                           n.created_on
                           from rms.dup_notes n
                           left join rms.people p
                           on n.author = p.id
                           where n.dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupNoteOutInDb>(sqlString);
    }
    
    public async Task<DupNoteInDb?> GetDupNote(int id)
    {
        var sqlString = $"select * from rms.dup_notes where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupNoteInDb>(sqlString);
    }
    
    public async Task<DupNoteOutInDb?> GetOutDupNote(int id)
    {
        var sqlString = $@"select n.id, n.dup_id, n.text, n.author, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as author_name, 
                           n.created_on
                           from rms.dup_notes n
                           left join rms.people p
                           on n.author = p.id
                           where n.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupNoteOutInDb>(sqlString);
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
        // ensure the author person id is present
        if (dupNoteContent.author == null)
        {
            var sqlString = $"select author from rms.dup_notes where id = {dupNoteContent.id.ToString()}";
            int? res = await conn.QueryFirstOrDefaultAsync<int?>(sqlString);
            if (res != null)
            {
                dupNoteContent.author = res;
            }
        }
        return (await conn.UpdateAsync(dupNoteContent)) ? dupNoteContent : null;
    }

    public async Task<int> DeleteDupNote(int id)
    {
        var sqlString = $@"delete from rms.dup_notes where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 

    /****************************************************************
    * DUP people
    ****************************************************************/

    // Fetch data 
    public async Task<IEnumerable<DupPersonInDb>> GetAllDupPeople(int dupId)
    {
        var sqlString = $"select * from rms.dup_people where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPersonInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DupPersonOutInDb>> GetAllOutDupPeople(int dupId)
    {
        var sqlString = $@"select d.id, d.dup_id, d.person_id, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as person_name, 
                           d.notes
                           from rms.dup_people d
                           left join rms.people p
                           on d.person_id = p.id
                           where d.dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupPersonOutInDb>(sqlString);
    }
    
    public async Task<DupPersonOutInDb?> GetOutDupPerson(int id)
    {
        var sqlString = $@"select d.id, d.dup_id, d.person_id, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as person_name, 
                           d.notes
                           from rms.dup_people d
                           left join rms.people p
                           on d.person_id = p.id 
                           where d.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupPersonOutInDb>(sqlString);
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
        var sqlString = $@"delete from rms.dup_people where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
    /****************************************************************
    *Secondary use
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DupSecondaryUseInDb>> GetAllSecUses(int dupId)
    {
        var sqlString = $"select * from rms.dup_sec_use where dup_id = {dupId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DupSecondaryUseInDb>(sqlString);
    }

    public async Task<DupSecondaryUseInDb?> GetSecUse(int id)
    {
        var sqlString = $"select * from rms.dup_sec_use where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DupSecondaryUseInDb>(sqlString);
    }
 
    // Update data
    public async Task<DupSecondaryUseInDb?> CreateSecUse(DupSecondaryUseInDb secUseContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(secUseContent);
        var sqlString = $"select * from rms.dup_sec_use where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DupSecondaryUseInDb>(sqlString);
    }

    public async Task<DupSecondaryUseInDb?> UpdateSecUse(DupSecondaryUseInDb secUseContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(secUseContent)) ? secUseContent : null;
    }

    public async Task<int> DeleteSecUse(int id)
    {
        var sqlString = $@"delete from rms.dup_sec_use where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
}
