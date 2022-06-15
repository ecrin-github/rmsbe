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
    
    public DtpRepository(ICredentials creds)
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
    }

    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    public async Task<bool> DtpDoesNotExistAsync(int id)
    {
        string sqlString = $@"select not exists (select 1 from rms.dtps where id = '{id}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpExistsAsync(int id)
    {
        string sqlString = $@"select exists (select 1 from rms.dtps where id = '{id}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DtpAttributeDoesNotExistAsync(int dtp_id, string type_name, int id)
    {
        string sqlString = $@"select not exists (select 1 from {_typeList[type_name]}
                              where id = {id.ToString()} and dtp_id = {dtp_id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpAttributeExistsAsync(int dtp_id, string type_name, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[type_name]}
                              where id = {id.ToString()} and dtp_id = {dtp_id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> DtpObjectDoesNotExistAsync(int dtp_id, string sd_oid)
    {
        string sqlString = $@"select not exists (select 1 from rms.dtp_objects
                              where dtp_id = {dtp_id.ToString()} and sd_oid = '{sd_oid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpObjectExistsAsync(int dtp_id, string sd_oid)
    {
        string sqlString = $@"select exists (select 1 from rms.dtp_objects
                              where dtp_id = {dtp_id.ToString()} and sd_oid = '{sd_oid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> PrereqDoesNotExistAsync(int dtp_id, string sd_oid, int id)
    {
        string sqlString = $@"select not exists (select 1 from rms.dtp_prereqs
                              where dtp_id = {dtp_id.ToString()} and sd_oid = '{sd_oid}'
                              and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> PrereqExistsAsync(int dtp_id, string sd_oid, int id)
    {
        string sqlString = $@"select exists (select 1 from rms.dtp_prereqs
                              where dtp_id = {dtp_id.ToString()} and sd_oid = '{sd_oid}'
                              and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> ObjectDatasetDoesNotExistAsync(string sd_oid, int id)
    {
        string sqlString = $@"select not exists (select 1 from rms.dtp_datasets
                              where sd_oid = '{sd_oid}' and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> DtpObjectDatasetExistsAsync(string sd_oid, int id)
    {
        string sqlString = $@"select exists (select 1 from rms.dtp_datasets
                              where sd_oid = '{sd_oid}' and id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DtpInDb>> GetAllDtpsAsync()
    {
        string sqlString = $"select * from rms.dtps";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }

    public async Task<IEnumerable<DtpInDb>> GetRecentDtpsAsync(int n)
    {
        string sqlString = $@"select * from rms.dtps
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpInDb>(sqlString);
    }
   
    public async Task<DtpInDb?> GetDtpAsync(int dtp_id)
    {
        string sqlString = $"select * from rms.dtps where dtp_id = {dtp_id}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpInDb?> CreateDtpAsync(DtpInDb dtpContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpContent);
        string sqlString = $"select * from rms.dtps where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpInDb>(sqlString);
    }

    public async Task<DtpInDb?> UpdateDtpAsync(DtpInDb dtpContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpContent)) ? dtpContent : null;
    }

    public async Task<int> DeleteDtpAsync(int dtp_id)
    {
        string sqlString = $"delete from rms.dtps where dtp_id = {dtp_id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudiesAsync(int dtp_id)
    {
        string sqlString = $"select * from rms.dtp_studies where dtp_id = '{dtp_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpStudyInDb>(sqlString);
    }

    public async Task<DtpStudyInDb?> GetDtpStudyAsync(int id)
    {
        string sqlString = $"select * from rms.dtp_studies where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpStudyInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpStudyInDb?> CreateDtpStudyAsync(DtpStudyInDb dtpStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpStudyContent);
        string sqlString = $"select * from rms.dtp_studies where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpStudyInDb>(sqlString);
    }

    public async Task<DtpStudyInDb?> UpdateDtpStudyAsync(DtpStudyInDb dtpStudyContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpStudyContent)) ? dtpStudyContent : null;
    }

    public async Task<int> DeleteDtpStudyAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtp_studies where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjectsAsync(int dtp_id)
    {
        string sqlString = $"select * from rms.dtp_objects where dtp_id = '{dtp_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpObjectInDb>(sqlString);
    }

    public async Task<DtpObjectInDb?> GetDtpObjectAsync(int id)
    {
        string sqlString = $"select * from rms.dtp_objects where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpObjectInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpObjectInDb?> CreateDtpObjectAsync(DtpObjectInDb dtpObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpObjectContent);
        string sqlString = $"select * from rms.dtp_objects where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpObjectInDb>(sqlString);
    }

    public async Task<DtpObjectInDb?> UpdateDtpObjectAsync(DtpObjectInDb dtpObjectContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpObjectContent)) ? dtpObjectContent : null;
    }

    public async Task<int> DeleteDtpObjectAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtp_objects where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DtaInDb>> GetAllDtasAsync(int dtp_id)
    {
        string sqlString = $"select * from rms.dtas where dtp_id = '{dtp_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtaInDb>(sqlString);
    }

    public async Task<DtaInDb?> GetDtaAsync(int id)
    {
        string sqlString = $"select * from rms.dtas where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtaInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtaInDb?> CreateDtaAsync(DtaInDb dtaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtaContent);
        string sqlString = $"select * from rms.dtas where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtaInDb>(sqlString);
    }

    public async Task<DtaInDb?> UpdateDtaAsync(DtaInDb dtaContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtaContent)) ? dtaContent : null;
    }

    public async Task<int> DeleteDtaAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtas where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    public async Task<DtpDatasetInDb?> GetDtpDatasetAsync(int id)
    {
        string sqlString = $"select * from rms.dtp_datasets where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpDatasetInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpDatasetInDb?> CreateDtpDatasetAsync(DtpDatasetInDb dtpDatasetContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpDatasetContent);
        string sqlString = $"select * from rms.dtp_datasets where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpDatasetInDb>(sqlString);
    }

    public async Task<DtpDatasetInDb?> UpdateDtpDatasetAsync(DtpDatasetInDb dtpDatasetContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpDatasetContent)) ? dtpDatasetContent : null;
    }

    public async Task<int> DeleteDtpDatasetAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtp_datasets where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    // Fetch data
    public async Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqsAsync(int dtp_id, string sd_oid)
    {
        string sqlString = $"select * from rms.dtp_prereqs where dtp_id = '{dtp_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPrereqInDb>(sqlString);
    }

    public async Task<DtpPrereqInDb?> GetDtpPrereqAsync(int id)
    {
        string sqlString = $"select * from rms.dtp_prereqs where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPrereqInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpPrereqInDb?> CreateDtpPrereqAsync(DtpPrereqInDb dtpPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpPrereqContent);
        string sqlString = $"select * from rms.dtp_prereqs where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpPrereqInDb>(sqlString);
    }

    public async Task<DtpPrereqInDb?> UpdateDtpPrereqAsync(DtpPrereqInDb dtpPrereqContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpPrereqContent)) ? dtpPrereqContent : null;
    }

    public async Task<int> DeleteDtpPrereqAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtp_prereqs where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotesAsync(int dtp_id)
    {
        string sqlString = $"select * from rms.dtp_notes where dtp_id = '{dtp_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpNoteInDb>(sqlString);
    }

    public async Task<DtpNoteInDb?> GetDtpNoteAsync(int id)
    {
        string sqlString = $"select * from rms.dtp_notes where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpNoteInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpNoteInDb?> CreateDtpNoteAsync(DtpNoteInDb dtpNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpNoteContent);
        string sqlString = $"select * from rms.dtp_notes where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpNoteInDb>(sqlString);
    }

    public async Task<DtpNoteInDb?> UpdateDtpNoteAsync(DtpNoteInDb dtpNoteContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpNoteContent)) ? dtpNoteContent : null;
    }

    public async Task<int> DeleteDtpNoteAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtp_notes where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeopleAsync(int dtp_id)
    {
        string sqlString = $"select * from rms.dtp_people where dtp_id = '{dtp_id.ToString()}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DtpPersonInDb>(sqlString);
    }

    public async Task<DtpPersonInDb?> GetDtpPersonAsync(int id)
    {
        string sqlString = $"select * from rms.dtp_people where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DtpPersonInDb>(sqlString);
    }
 
    // Update data
    public async Task<DtpPersonInDb?> CreateDtpPersonAsync(DtpPersonInDb dtpPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dtpPersonContent);
        string sqlString = $"select * from rms.dtp_people where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DtpPersonInDb>(sqlString);
    }

    public async Task<DtpPersonInDb?> UpdateDtpPersonAsync(DtpPersonInDb dtpPersonContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dtpPersonContent)) ? dtpPersonContent : null;
    }

    public async Task<int> DeleteDtpPersonAsync(int id)
    {
        string sqlString = $@"delete from mdr.dtp_people where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    
    /*
    // Statistics
    public async Task<PaginationResponse<DtpDto>> PaginateDtp(PaginationRequest paginationRequest)
    public async Task<PaginationResponse<DtpDto>> FilterDtpByTitle(FilteringByTitleRequest filteringByTitleRequest);
    public async Task<int> GetTotalDtp();
    public async Task<int> GetUncompletedDtp();
    */

}
