using System.Data;
using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;
using Dapper.Contrib.Extensions;
 
namespace rmsbe.DataLayer;

public class DtpRepository : IDtpRepository
{
    private readonly string _dbConnString;
    private readonly Dictionary<string, string> _typeList;
    
    public DtpRepository(ICreds creds)
    {
        _dbConnString = creds.GetConnectionString("rms");
        
        // set up dictionary of table name equivalents for type parameter
        _typeList = new Dictionary<string, string>
        {
            { "DtpStudy", "rms.dtp_studies" },  
            { "DtpObject", "rms.dtp_objects" },           
            { "Dta", "rms.dtp_dtas" },
            { "DtpPrereq", "rms.dtp_prereqs" },
            { "DtpNote", "rms.dtp_notes" },           
            { "DtpPerson", "rms.dtp_people" },
            { "DtpDataset", "rms.dtp_datasets" }
        };
        
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    public async Task<bool> DtpExists(int id)
    {
        var sqlString = $@"select exists (select 1 from rms.dtps where id = '{id}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DtpAttributeExists(int dtpId, string typeName, int id)
    {
        var sqlString = $@"select exists (select 1 from {_typeList[typeName]}
                              where id = {id.ToString()} and dtp_id = {dtpId.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpDtaExists(int dtpId)
    {
        var sqlString = $@"select exists (select 1 from rms.dtas
                              where dtp_id = {dtpId.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DtpObjectExists(int dtpId, string sdOid)
    {
        var sqlString = $@"select exists (select 1 from rms.dtp_objects
                              where dtp_id = {dtpId.ToString()} and sd_oid = '{sdOid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpObjectDatasetExists(int dtpId, string sdOid)
    {
        var sqlString = $@"select exists (select 1 from rms.dtp_datasets
                              where dtp_id = {dtpId.ToString()} and sd_oid = '{sdOid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpObjectAttributeExists(int dtpId, string sdOid, string typeName, int id)
    {
        var sqlString = $@"select exists (select 1 from {_typeList[typeName]} 
                              where dtp_id = {dtpId.ToString()} 
                              and sd_oid = '{sdOid}'
                              and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    /****************************************************************
    * All DTPs / DTP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DtpInDb>> GetAllDtps()
    {
        var sqlString = $"select * from rms.dtps order by created_on desc";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpEntryInDb>> GetAllDtpEntries()
    {
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dtps d
                           left join lup.dtp_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           order by d.created_on desc";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
    }

    /****************************************************************
    * Paginated DTPs / DTP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DtpInDb>> GetPaginatedDtpData(int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select * from rms.dtps
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DtpEntryInDb>> GetPaginatedDtpEntries(int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dtps d
                           left join lup.dtp_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           order by d.created_on desc
                           offset {offset.ToString()}
                           limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Filtered DTPs / DTP entries
    ****************************************************************/   
    
    public async Task<IEnumerable<DtpInDb>> GetFilteredDtpData(string titleFilter)
    {
        var sqlString = $@"select * from rms.dtps
                            where display_name ilike '%{titleFilter}%'
                            order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }    
    
    public async Task<IEnumerable<DtpEntryInDb>> GetFilteredDtpEntries(string titleFilter)
    {
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dtps d
                           left join lup.dtp_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           where display_name ilike '%{titleFilter}%'
                           order by d.created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Paginated and filtered DTPs / DTP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DtpInDb>> GetPaginatedFilteredDtpData(string titleFilter, int pNum,
        int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select * from rms.dtps 
                              where display_name ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DtpEntryInDb>> GetPaginatedFilteredDtpEntries(string titleFilter, int pNum,
        int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dtps d
                           left join lup.dtp_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           where display_name ilike '%{titleFilter}%'
                           order by d.created_on DESC
                           offset {offset.ToString()}
                           limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
    }   
    
    /****************************************************************
    * Recent DTPs / DTP entries
    ****************************************************************/
    
    public async Task<IEnumerable<DtpInDb>> GetRecentDtps(int n)
    {
        var sqlString = $@"select * from rms.dtps
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }    
    
    public async Task<IEnumerable<DtpEntryInDb>> GetRecentDtpEntries(int n)
    {
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dtps d
                           left join lup.dtp_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           order by d.created_on DESC
                           limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
    }
   
    /****************************************************************
    * DTPs / DTP entries by Organisation
    ****************************************************************/

    public async Task<IEnumerable<DtpInDb>> GetDtpsByOrg(int orgId)
    {
        var sqlString = $@"select * from rms.dtps
                              where org_id = {orgId.ToString()} 
                              order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }    
    
    public async Task<IEnumerable<DtpEntryInDb>> GetDtpEntriesByOrg(int orgId)
    {
        var sqlString = $@"select d.id, d.display_name, 
                           g.default_name as org_name,
                           s.name as status_name
                           from rms.dtps d
                           left join lup.dtp_status_types s
                           on d.status_id = s.id
                           left join lup.organisations g
                           on d.org_id = g.id
                           where org_id = {orgId.ToString()} 
                           order by d.created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
    }

    
    /****************************************************************
    * Fetch / delete Full DTP records, with attributes
    ****************************************************************/   
    
    public async Task<FullDtpInDb?> GetFullDtpById(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $"select * from rms.dtps where id = {id.ToString()}";   
        DtpInDb? coreDtp = await conn.QueryFirstOrDefaultAsync<DtpInDb>(sqlString);     
        sqlString = $"select * from rms.dtas where dtp_id = {id.ToString()}";
        var dtasInDb = (await conn.QueryAsync<DtaInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dtp_studies where dtp_id = {id.ToString()}";
        var dtpStudiesInDb = (await conn.QueryAsync<DtpStudyInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dtp_objects where dtp_id = {id.ToString()}";
        var dtpObjectsInDb = (await conn.QueryAsync<DtpObjectInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dtp_prereqs where dtp_id = {id.ToString()}";
        var dtpPrereqsInDb = (await conn.QueryAsync<DtpPrereqInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dtp_datasets where dtp_id = {id.ToString()}";
        var dtpDatasetsInDb = (await conn.QueryAsync<DtpDatasetInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dtp_notes where dtp_id = {id.ToString()}";
        var dtpNotesInDb = (await conn.QueryAsync<DtpNoteInDb>(sqlString)).ToList();
        sqlString = $"select * from rms.dtp_people where dtp_id = {id.ToString()}";
        var dtpPeopleInDb = (await conn.QueryAsync<DtpPersonInDb>(sqlString)).ToList();

        return new FullDtpInDb(coreDtp, dtasInDb, dtpStudiesInDb, dtpObjectsInDb,
            dtpPrereqsInDb, dtpDatasetsInDb, dtpNotesInDb, dtpPeopleInDb);
    } 
    
    // delete data
    public async Task<int> DeleteFullDtp(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $@"delete from rms.dtp_people where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtp_notes where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtp_datasets where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtp_prereqs where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtp_objects where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtp_studies where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtas where dtp_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.dtps where id = {id.ToString()};";
        return await conn.ExecuteAsync(sqlString);
    }
    
    /****************************************************************
    * DTP statistics
    ****************************************************************/
    
    public async Task<int> GetTotalDtps()
    {
        var sqlString = $@"select count(*) from rms.dtps;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetTotalFilteredDtps(string titleFilter)
    {
        var sqlString = $@"select count(*) from rms.dtps
                             where display_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetCompletedDtps()
    {
        // completed status id = 16
        var sqlString = $@"select count(*) from rms.dtps
                           where status_id = 16;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<IEnumerable<StatisticInDb>> GetDtpsByStatus()
    {
        var sqlString = $@"select status_id as stat_type, 
                             count(id) as stat_value 
                             from rms.dtps group by status_id;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StatisticInDb>(sqlString);
    }
    
    /****************************************************************
    * Get single DTP record
    ****************************************************************/   
   
    public async Task<DtpInDb?> GetDtp(int dtpId)
    {
        var sqlString = $"select * from rms.dtps where id = {dtpId}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpInDb>(sqlString);
    }
    
    public async Task<DtpOutInDb?> GetOutDtp(int dtpId)
    {
        var sqlString = $@"select d.id, d.org_id, 
                           g.default_name as org_name,
                           d.display_name, d.status_id, 
                           ds.name as status_name, 
                           d.initial_contact_date, d.set_up_completed,
                           d.md_access_granted, d.md_complete_date,
                           d.dta_agreed_date, d.upload_access_requested,
                           d.upload_access_confirmed, d.uploads_complete,
                           d.qc_checks_completed, d.md_integrated_with_mdr,
                           d.availability_requested, d.availability_confirmed
                           from rms.dtps d
                           left join lup.organisations g
                           on d.org_id = g.id
                           left join lup.dtp_status_types ds
                           on d.status_id = ds.id
                           where d.id = {dtpId}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpOutInDb>(sqlString);
    }
    
    
    /****************************************************************
    * Update DTP records
    ****************************************************************/   
    
    public async Task<DtpInDb?> CreateDtp(DtpInDb dtpContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpContent);
        var sqlString = $"select * from rms.dtps where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpInDb>(sqlString);
    }

    public async Task<DtpInDb?> UpdateDtp(DtpInDb dtpContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpContent)) ? dtpContent : null;
    }

    public async Task<int> DeleteDtp(int dtpId)
    {
        var sqlString = $"delete from rms.dtps where id = {dtpId.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
    
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudies(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_studies where dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpStudyInDb>(sqlString);
    }
    
    public async Task<IEnumerable<DtpStudyOutInDb>> GetAllOutDtpStudies(int dtpId)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.sd_sid,
                        s.display_title as study_name,
                        d.md_check_status_id,
                        cs.name as md_check_status_name,
                        d.md_check_date, d.md_check_by
                        from rms.dtp_studies d
                        left join mdr.studies s 
                        on d.sd_sid = s.sd_sid
                        left join lup.check_status_types cs
                        on d.md_check_status_id = cs.id
                        where d.dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpStudyOutInDb>(sqlString);
    }
    
    public async Task<DtpStudyInDb?> GetDtpStudy(int id)
    {
        var sqlString = $"select * from rms.dtp_studies where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpStudyInDb>(sqlString);
    }
    
    public async Task<DtpStudyOutInDb?> GetOutDtpStudy(int id)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.sd_sid,
                        s.display_title as study_name,
                        d.md_check_status_id,
                        cs.name as md_check_status_name,
                        d.md_check_date, d.md_check_by
                        from rms.dtp_studies d
                        left join mdr.studies s 
                        on d.sd_sid = s.sd_sid
                        left join lup.check_status_types cs
                        on d.md_check_status_id = cs.id
                        where d.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpStudyOutInDb>(sqlString);
    }
    
 
    // Update data
    public async Task<DtpStudyInDb?> CreateDtpStudy(DtpStudyInDb dtpStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpStudyContent);
        var sqlString = $"select * from rms.dtp_studies where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpStudyInDb>(sqlString);
    }

    public async Task<DtpStudyInDb?> UpdateDtpStudy(DtpStudyInDb dtpStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        // ensure the study sd_sid is present
        if (dtpStudyContent.sd_sid == null || dtpStudyContent.sd_sid.Trim() == "")
        {
            var sqlString = $@"select sd_sid from rms.dtp_studies 
                               where id = {dtpStudyContent.id.ToString()}";
            string? res = await conn.QueryFirstOrDefaultAsync<string?>(sqlString);
            if (res != null)
            {
                dtpStudyContent.sd_sid = res;
            }
        }
        return (await conn.UpdateAsync(dtpStudyContent)) ? dtpStudyContent : null;
    }

    public async Task<int> DeleteDtpStudy(int id)
    {
        var sqlString = $@"delete from rms.dtp_studies where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjects(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_objects where dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpObjectInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpObjectOutInDb>> GetAllOutDtpObjects(int dtpId)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.sd_oid,
                           b.display_title as object_name,
                           d.is_dataset, d.access_type_id,
                           at.name as access_type_name,
                           d.download_allowed, d.access_details, d.embargo_requested,
                           d.embargo_regime, d.embargo_still_applies, d.access_check_status_id,
                           cs1.name as access_check_status_name,
                           d.access_check_date, d.access_check_by, d.md_check_status_id,
                           cs2.name as md_check_status_name,
                           d.md_check_date, d.md_check_by, notes
                           from rms.dtp_objects d
                           left join mdr.data_objects b 
                           on d.sd_oid = b.sd_oid
                           left join lup.repo_access_types at
                           on d.access_type_id = at.id
                           left join lup.check_status_types cs1
                           on d.access_check_status_id = cs1.id
                           left join lup.check_status_types cs2
                           on d.md_check_status_id = cs2.id
                           where d.dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpObjectOutInDb>(sqlString);
    }
    
    public async Task<DtpObjectInDb?> GetDtpObject(int id)
    {
        var sqlString = $"select * from rms.dtp_objects where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpObjectInDb>(sqlString);
    }
 
    public async Task<DtpObjectOutInDb?> GetOutDtpObject(int id)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.sd_oid,
                           b.display_title as object_name,
                           d.is_dataset, d.access_type_id,
                           at.name as access_type_name,
                           d.download_allowed, d.access_details, d.embargo_requested,
                           d.embargo_regime, d.embargo_still_applies, d.access_check_status_id,
                           cs1.name as access_check_status_name,
                           d.access_check_date, d.access_check_by, d.md_check_status_id,
                           cs2.name as md_check_status_name,
                           d.md_check_date, d.md_check_by, notes
                           from rms.dtp_objects d
                           left join mdr.data_objects b 
                           on d.sd_oid = b.sd_oid
                           left join lup.repo_access_types at
                           on d.access_type_id = at.id
                           left join lup.check_status_types cs1
                           on d.access_check_status_id = cs1.id
                           left join lup.check_status_types cs2
                           on d.md_check_status_id = cs2.id 
                           where d.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpObjectOutInDb>(sqlString);
    }
    
    // Update data
    public async Task<DtpObjectInDb?> CreateDtpObject(DtpObjectInDb dtpObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpObjectContent);
        var sqlString = $"select * from rms.dtp_objects where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpObjectInDb>(sqlString);
    }

    public async Task<DtpObjectInDb?> UpdateDtpObject(DtpObjectInDb dtpObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        // ensure the object sd_oid is present
        if (dtpObjectContent.sd_oid == null || dtpObjectContent.sd_oid.Trim() == "")
        {
            var sqlString = $"select sd_oid from rms.dtp_objects where id = {dtpObjectContent.id.ToString()}";
            string? res = await conn.QueryFirstOrDefaultAsync<string?>(sqlString);
            if (res != null)
            {
                dtpObjectContent.sd_oid = res;
            }
        }
        return (await conn.UpdateAsync(dtpObjectContent)) ? dtpObjectContent : null;
    }

    public async Task<int> DeleteDtpObject(int id)
    {
        var sqlString = $@"delete from rms.dtp_objects where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<DtaInDb?> GetDta(int dtpId)
    {
        var sqlString = $"select * from rms.dtas where dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtaInDb>(sqlString);
    }
    
    public async Task<DtaOutInDb?> GetOutDta(int dtpId)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.conforms_to_default,
                           d.variations, dta_file_path, 
                           d.repo_signatory_1,
                           LTRIM(COALESCE(p1.given_name, '')||' '||p1.family_name) as repo_signatory_1_name,
                           d.repo_signatory_2,
                           LTRIM(COALESCE(p2.given_name, '')||' '||p2.family_name) as repo_signatory_2_name,
                           d.provider_signatory_1,
                           LTRIM(COALESCE(p3.given_name, '')||' '||p3.family_name) as provider_signatory_1_name,
                           d.provider_signatory_2,
                           LTRIM(COALESCE(p4.given_name, '')||' '||p4.family_name) as provider_signatory_2_name,
                           d.notes
                           from rms.dtas d
                           left join rms.people p1
                           on d.repo_signatory_1 = p1.id
                           left join rms.people p2
                           on d.repo_signatory_2 = p2.id
                           left join rms.people p3
                           on d.provider_signatory_1 = p3.id
                           left join rms.people p4
                           on d.provider_signatory_2 = p4.id
                           where d.dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtaOutInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtaInDb?> CreateDta(DtaInDb dtaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtaContent);
        var sqlString = $"select * from rms.dtas where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtaInDb>(sqlString);
    }

    public async Task<DtaInDb?> UpdateDta(DtaInDb dtaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var sqlString = $@"select id from rms.dtas 
                           where dtp_id = {dtaContent.dtp_id.ToString()}";
        dtaContent.id = await conn.QueryFirstOrDefaultAsync<int>(sqlString);
        return (await conn.UpdateAsync(dtaContent)) ? dtaContent : null;  
    }

    public async Task<int> DeleteDta(int dtpId)
    {
        var sqlString = $@"delete from rms.dtas where dtp_id = {dtpId.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    public async Task<DtpDatasetInDb?> GetDtpDataset(int dtpId, string sdOid)
    {
        var sqlString = $@"select * from rms.dtp_datasets 
                           where dtp_id = {dtpId.ToString()}
                           and sd_oid = '{sdOid}';";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpDatasetInDb>(sqlString);
    }
 
    public async Task<DtpDatasetOutInDb?> GetOutDtpDataset(int dtpId, string sdOid)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.sd_oid,
                           b.display_title as object_name,
                           d.legal_status_id,
                           ls.name as legal_status_name,
                           d.legal_status_text, d.legal_status_path, d.desc_md_check_status_id,
                           cs1.name as desc_md_check_status_name,
                           d.desc_md_check_date, d.desc_md_check_by, d.deident_check_status_id,
                           cs2.name as deident_check_status_name,
                           d.deident_check_date, d.deident_check_by, d.notes
                           from rms.dtp_datasets d
                           left join mdr.data_objects b 
                           on d.sd_oid = b.sd_oid
                           left join lup.legal_status_types ls
                           on d.legal_status_id = ls.id
                           left join lup.check_status_types cs1
                           on d.desc_md_check_status_id = cs1.id
                           left join lup.check_status_types cs2
                           on d.deident_check_status_id = cs2.id
                           where d.dtp_id = {dtpId.ToString()}
                           and d.sd_oid = '{sdOid}';";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpDatasetOutInDb>(sqlString);
    }
    
    
    // Update data
    public async Task<DtpDatasetInDb?> CreateDtpDataset(DtpDatasetInDb dtpDatasetContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpDatasetContent);
        var sqlString = $"select * from rms.dtp_datasets where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpDatasetInDb>(sqlString);
    }

    public async Task<DtpDatasetInDb?> UpdateDtpDataset(DtpDatasetInDb dtpDatasetContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var sqlString = $@"select id from rms.dtp_datasets
                           where dtp_id = {dtpDatasetContent.dtp_id.ToString()}
                           and sd_oid = '{dtpDatasetContent.sd_oid}'";
        dtpDatasetContent.id = await conn.QueryFirstOrDefaultAsync<int>(sqlString);
        return (await conn.UpdateAsync(dtpDatasetContent)) ? dtpDatasetContent : null;
    }

    public async Task<int> DeleteDtpDataset(int dtpId, string sdOid)
    {
        var sqlString = $@"delete from rms.dtp_datasets 
                           where dtp_id = {dtpId.ToString()}
                           and sd_oid = '{sdOid}';";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqs(int dtpId, string sdOid)
    {
        var sqlString = $"select * from rms.dtp_prereqs where dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPrereqInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpPrereqOutInDb>> GetAllOutDtpPrereqs(int dtpId, string sdOid)
    {
        var sqlString = $@"select p.id, p.dtp_id, p.sd_oid,
                           b.display_title as object_name,
                           p.pre_requisite_type_id,
                           t.name as pre_requisite_type_name,
                           p.pre_requisite_notes 
                           from rms.dtp_prereqs p 
                           left join mdr.data_objects b 
                           on p.sd_oid = b.sd_oid
                           left join lup.prereq_types t
                           on p.pre_requisite_type_id = t.id
                           where p.dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPrereqOutInDb>(sqlString);
    }
    
    public async Task<DtpPrereqInDb?> GetDtpPrereq(int id)
    {
        var sqlString = $"select * from rms.dtp_prereqs where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPrereqInDb>(sqlString);
    }
 
    public async Task<DtpPrereqOutInDb?> GetOutDtpPrereq(int id)
    {
        var sqlString = $@"select p.id, p.dtp_id, p.sd_oid,
                           b.display_title as object_name,
                           p.pre_requisite_type_id,
                           t.name as pre_requisite_type_name,
                           p.pre_requisite_notes
                           from rms.dtp_prereqs p 
                           left join mdr.data_objects b 
                           on p.sd_oid = b.sd_oid
                           left join lup.prereq_types t
                           on p.pre_requisite_type_id = t.id 
                           where p.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPrereqOutInDb>(sqlString);
    }

    
    // Update data
    public async Task<DtpPrereqInDb?> CreateDtpPrereq(DtpPrereqInDb dtpPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpPrereqContent);
        var sqlString = $"select * from rms.dtp_prereqs where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpPrereqInDb>(sqlString);
    }

    public async Task<DtpPrereqInDb?> UpdateDtpPrereq(DtpPrereqInDb dtpPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpPrereqContent)) ? dtpPrereqContent : null;
    }

    public async Task<int> DeleteDtpPrereq(int id)
    {
        var sqlString = $@"delete from rms.dtp_prereqs where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotes(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_notes where dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpNoteInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpNoteOutInDb>> GetAllOutDtpNotes(int dtpId)
    {
        var sqlString = $@"select n.id, n.dtp_id, n.text, n.author, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as author_name, 
                           n.created_on
                           from rms.dtp_notes n
                           left join rms.people p
                           on n.author = p.id
                           where n.dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpNoteOutInDb>(sqlString);
    }

    public async Task<DtpNoteInDb?> GetDtpNote(int id)
    {
        var sqlString = $"select * from rms.dtp_notes where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpNoteInDb>(sqlString);
    }
 
    public async Task<DtpNoteOutInDb?> GetOutDtpNote(int id)
    {
        var sqlString = $@"select n.id, n.dtp_id, n.text, n.author, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as author_name, 
                           n.created_on
                           from rms.dtp_notes n
                           left join rms.people p
                           on n.author = p.id
                           where n.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpNoteOutInDb>(sqlString);
    }
    
    // Update data
    public async Task<DtpNoteInDb?> CreateDtpNote(DtpNoteInDb dtpNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpNoteContent);
        var sqlString = $"select * from rms.dtp_notes where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpNoteInDb>(sqlString);
    }

    public async Task<DtpNoteInDb?> UpdateDtpNote(DtpNoteInDb dtpNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        // ensure the author person id is present
        if (dtpNoteContent.author == null)
        {
            var sqlString = $"select author from rms.dtp_notes where id = {dtpNoteContent.id.ToString()}";
            int? res = await conn.QueryFirstOrDefaultAsync<int?>(sqlString);
            if (res != null)
            {
                dtpNoteContent.author = res;
            }
        }
        return (await conn.UpdateAsync(dtpNoteContent)) ? dtpNoteContent : null;
    }

    public async Task<int> DeleteDtpNote(int id)
    {
        var sqlString = $@"delete from rms.dtp_notes where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeople(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_people where dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPersonInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpPersonOutInDb>> GetAllOutDtpPeople(int dtpId)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.person_id, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as person_name, 
                           d.notes
                           from rms.dtp_people d
                           left join rms.people p
                           on d.person_id = p.id
                           where d.dtp_id = {dtpId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPersonOutInDb>(sqlString);
    }
    
    public async Task<DtpPersonInDb?> GetDtpPerson(int id)
    {
        var sqlString = $"select * from rms.dtp_people where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPersonInDb>(sqlString);
    }
 
    public async Task<DtpPersonOutInDb?> GetOutDtpPerson(int id)
    {
        var sqlString = $@"select d.id, d.dtp_id, d.person_id, 
                           LTRIM(COALESCE(p.given_name, '')||' '||p.family_name) as person_name, 
                           d.notes
                           from rms.dtp_people d
                           left join rms.people p
                           on d.person_id = p.id 
                           where d.id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPersonOutInDb>(sqlString);
    }
    
    // Update data
    public async Task<DtpPersonInDb?> CreateDtpPerson(DtpPersonInDb dtpPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(dtpPersonContent);
        var sqlString = $"select * from rms.dtp_people where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpPersonInDb>(sqlString);
    }

    public async Task<DtpPersonInDb?> UpdateDtpPerson(DtpPersonInDb dtpPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpPersonContent)) ? dtpPersonContent : null;
    }

    public async Task<int> DeleteDtpPerson(int id)
    {
        var sqlString = $@"delete from rms.dtp_people where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

}

// A small helper class that extends the type mapping functionality
// in Dapper. Needed to include the new DateOnly type in the CRUD operations.

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }
}
