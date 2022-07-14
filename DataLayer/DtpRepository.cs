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

    public async Task<bool> DtpObjectExists(int dtpId, string sdOid)
    {
        var sqlString = $@"select exists (select 1 from rms.dtp_objects
                              where dtp_id = {dtpId.ToString()} and sd_oid = {sdOid})";
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
        var sqlString = $"select * from rms.dtps";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpEntryInDb>> GetAllDtpEntries()
    {
        var sqlString = $"select id, org_id, display_name from rms.dtps";
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
        var sqlString = $@"select id, org_id, display_name from rms.dtps
                              order by created_on DESC
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
                            where display_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }    
    
    public async Task<IEnumerable<DtpEntryInDb>> GetFilteredDtpEntries(string titleFilter)
    {
        var sqlString = $@"select id, org_id, display_name from rms.dtps
                            where display_name ilike '%{titleFilter}%'";
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
        var sqlString = $@"select id, org_id, display_name from rms.dtps
                              where display_name ilike '%{titleFilter}%'
                              order by created_on DESC
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
        var sqlString = $@"select id, org_id, display_name from rms.dtps
                              order by created_on DESC
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
        var sqlString = $@"select id, org_id, display_name from rms.dtps
                              where org_id = {orgId.ToString()} 
                              order by created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpEntryInDb>(sqlString);
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
    * DTP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudies(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_studies where dtp_id = '{dtpId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpStudyInDb>(sqlString);
    }

    public async Task<DtpStudyInDb?> GetDtpStudy(int id)
    {
        var sqlString = $"select * from rms.dtp_studies where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpStudyInDb>(sqlString);
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
        return (await conn.UpdateAsync(dtpStudyContent)) ? dtpStudyContent : null;
    }

    public async Task<int> DeleteDtpStudy(int id)
    {
        var sqlString = $@"delete from mdr.dtp_studies where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjects(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_objects where dtp_id = '{dtpId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpObjectInDb>(sqlString);
    }

    public async Task<DtpObjectInDb?> GetDtpObject(int id)
    {
        var sqlString = $"select * from rms.dtp_objects where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpObjectInDb>(sqlString);
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
        return (await conn.UpdateAsync(dtpObjectContent)) ? dtpObjectContent : null;
    }

    public async Task<int> DeleteDtpObject(int id)
    {
        var sqlString = $@"delete from mdr.dtp_objects where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DtaInDb>> GetAllDtas(int dtpId)
    {
        var sqlString = $"select * from rms.dtas where dtp_id = '{dtpId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtaInDb>(sqlString);
    }

    public async Task<DtaInDb?> GetDta(int id)
    {
        var sqlString = $"select * from rms.dtas where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtaInDb>(sqlString);
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
        return (await conn.UpdateAsync(dtaContent)) ? dtaContent : null;
    }

    public async Task<int> DeleteDta(int id)
    {
        var sqlString = $@"delete from mdr.dtas where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    public async Task<DtpDatasetInDb?> GetDtpDataset(int id)
    {
        var sqlString = $"select * from rms.dtp_datasets where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpDatasetInDb>(sqlString);
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
        return (await conn.UpdateAsync(dtpDatasetContent)) ? dtpDatasetContent : null;
    }

    public async Task<int> DeleteDtpDataset(int id)
    {
        var sqlString = $@"delete from mdr.dtp_datasets where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    // Fetch data
    public async Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqs(int dtpId, string sdOid)
    {
        var sqlString = $"select * from rms.dtp_prereqs where dtp_id = '{dtpId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPrereqInDb>(sqlString);
    }

    public async Task<DtpPrereqInDb?> GetDtpPrereq(int id)
    {
        var sqlString = $"select * from rms.dtp_prereqs where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPrereqInDb>(sqlString);
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
        var sqlString = $@"delete from mdr.dtp_prereqs where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotes(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_notes where dtp_id = '{dtpId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpNoteInDb>(sqlString);
    }

    public async Task<DtpNoteInDb?> GetDtpNote(int id)
    {
        var sqlString = $"select * from rms.dtp_notes where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpNoteInDb>(sqlString);
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
        return (await conn.UpdateAsync(dtpNoteContent)) ? dtpNoteContent : null;
    }

    public async Task<int> DeleteDtpNote(int id)
    {
        var sqlString = $@"delete from mdr.dtp_notes where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeople(int dtpId)
    {
        var sqlString = $"select * from rms.dtp_people where dtp_id = '{dtpId.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPersonInDb>(sqlString);
    }

    public async Task<DtpPersonInDb?> GetDtpPerson(int id)
    {
        var sqlString = $"select * from rms.dtp_people where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPersonInDb>(sqlString);
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
        var sqlString = $@"delete from mdr.dtp_people where id = {id.ToString()};";
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
