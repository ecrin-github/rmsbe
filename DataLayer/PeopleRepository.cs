using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;
using Dapper.Contrib.Extensions;

namespace rmsbe.DataLayer;

public class PeopleRepository : IPeopleRepository
{
    private readonly string _dbConnString;
    private readonly Dictionary<string, string> _typeList;

    public PeopleRepository(ICreds creds)
    {
        _dbConnString = creds.GetConnectionString("people");
        
        // set up dictionary of table name equivalents for type parameter
        _typeList = new Dictionary<string, string>
        {
            { "PersonRole", "rms.people_roles" },  
            { "PersonLink", "rms.people_links" }        
        };
    }
    
    /****************************************************************
    * Check functions for people
    ****************************************************************/
    
    // Check if person exists
    public async Task<bool> PersonExists(int id)
    {
        var sqlString = $@"select exists (select 1 from rms.people
                              where id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    // Check if attribute (usually role) exists on this person
    public async Task<bool> PersonAttributeExists(int parId, string typeName, int id)
    {
        var sqlString = $@"select exists (select 1 from {_typeList[typeName]}
                              where id = {id.ToString()} and person_id = {parId.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    // Check a person has one current role in the system
    public async Task<bool> PersonHasCurrentRole(int id)
    {
        var sqlString = $@"select count(*) from rms.people_roles
                                   where person_id = {id.ToString()} 
                                   and is_current = true";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString) == 1;
    }
    
    // Get times person is a signatory in a DTA
    public async Task<int> PersonDtaSignatures(int id)
    {
        var sqlString = $@"select count(*) from rms.dtas
                                   where repo_signatory_1 = {id.ToString()} 
                                   or repo_signatory_2 = {id.ToString()} 
                                   or provider_signatory_1 = {id.ToString()} 
                                   or provider_signatory_2 = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }

    // Get times person is a signatory in a DUA
    public async Task<int> PersonDuaSignatures(int id)
    {
        var sqlString = $@"select count(*) from rms.duas
                                   where repo_signatory_1 = {id.ToString()} 
                                   or repo_signatory_2 = {id.ToString()} 
                                   or provider_signatory_1 = {id.ToString()} 
                                   or provider_signatory_2 = {id.ToString()}
                                   or requester_signatory_1 = {id.ToString()} 
                                   or requester_signatory_2 = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    /****************************************************************
    * All People / People entries
    ****************************************************************/
      
    public async Task<IEnumerable<PersonInDb>> GetAllPeopleData()
    {
        var sqlString = $@"select * from rms.people
                              order by family_name, given_name";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }
    
    
    public async Task<IEnumerable<PersonEntryInDb>> GetAllPeopleEntries()
    {
        var sqlString = $@"select p.id, p.given_name, p.family_name,
                              p.email, p.org_name, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.id = cpr.person_id
                              order by family_name, given_name";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Paginated People / People entries
    ****************************************************************/    

    public async Task<IEnumerable<PersonInDb>> GetPaginatedPeople(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from rms.people 
                              order by family_name, given_name
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }
    
    public async Task<IEnumerable<PersonEntryInDb>> GetPaginatedPeopleEntries(int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select p.id, p.given_name, p.family_name,
                              p.email, p.org_name, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.id = cpr.person_id                
                              order by family_name, given_name
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Filtered People / People entries
    ****************************************************************/    
    
    public async Task<IEnumerable<PersonInDb>> GetFilteredPeople(string titleFilter)
    {
        var sqlString = $@"select * from rms.people 
                              where given_name ilike '%{titleFilter}%'
                              or family_name ilike '%{titleFilter}%'
                              order by family_name, given_name";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }


    public async Task<IEnumerable<PersonEntryInDb>> GetFilteredPeopleEntries(string titleFilter)
    {
        var sqlString = $@"select p.id, p.given_name, p.family_name,
                              p.email, p.org_name, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.id = cpr.person_id
                              where p.given_name ilike '%{titleFilter}%'
                              or p.family_name ilike '%{titleFilter}%'
                              order by family_name, given_name";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Paginated and filtered People / People entries
    ****************************************************************/    
    
    public async Task<IEnumerable<PersonInDb>> GetPaginatedFilteredPeople(string titleFilter, int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select * from rms.people 
                              where given_name ilike '%{titleFilter}%'
                              or family_name ilike '%{titleFilter}%'
                              order by family_name, given_name
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetPaginatedFilteredPeopleEntries(string titleFilter, int pNum, int pSize)
    {
        var offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        var sqlString = $@"select p.id, p.given_name, p.family_name,
                              p.email, p.org_name, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.id = cpr.person_id
                              where p.given_name ilike '%{titleFilter}%'
                              or p.family_name ilike '%{titleFilter}%'
                              order by family_name, given_name
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Recent People / People entries
    ****************************************************************/      

    public async Task<IEnumerable<PersonInDb>> GetRecentPeople(int n)
    {
        var sqlString = $@"select * from rms.people
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetRecentPeopleEntries(int n)
    {
        var sqlString = $@"select p.id, p.given_name, p.family_name,
                              p.email, p.org_name, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.id = cpr.person_id
                              order by p.created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }

    /****************************************************************
    * People / People entries by Org
    ****************************************************************/ 
    
    public async Task<IEnumerable<PersonInDb>> GetPeopleByOrg(int orgId)
    {
        var sqlString = $@"select * from rms.people
                              where org_id = {orgId.ToString()}
                              order by family_name, given_name";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetPeopleEntriesByOrg(int orgId)
    {
        var sqlString = $@"select p.id, p.title, p.given_name, p.family_name,
                              p.email, p.org_name, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.Id = cpr.person_id
                              where org_id = {orgId.ToString()}
                              order by family_name, given_name";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }
    
    
    /****************************************************************
    * Get single Person record
    ****************************************************************/        
    
    public async Task<PersonInDb?>GetPersonData (int id)
    {
        var sqlString = $@"select * from rms.people
                              where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<PersonInDb>(sqlString);
    }

    /****************************************************************
    * Update People Records
    ****************************************************************/ 
    
    public async Task<PersonInDb?> CreatePerson(PersonInDb personContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var id = conn.Insert(personContent);
        var sqlString = $@"select * from rms.people
                              where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<PersonInDb>(sqlString);
    }

    public async Task<PersonInDb?> UpdatePerson(PersonInDb personContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(personContent)) ? personContent : null;
    }

    /****************************************************************
    * This is a true delete that also removes any related
    * person role records. It should not normally be allowed, and
    * would not be allowed if the person was involved in any
    * DTP or DUP.
    ****************************************************************/
    
    public async Task<int> DeletePerson(int id)
    {
        var sqlString = $@"delete from rms.people_roles
                              where person_id = {id.ToString()};
                              delete from rms.people
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    public async Task<int> GetTotalPeople()
    {
        var sqlString = $@"select count(*) from rms.people;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }

    public async Task<int> GetTotalFilteredPeople(string titleFilter)
    {
        string sqlString = $@"select count(*) from rms.people
                              where given_name ilike '%{titleFilter}%'
                              or family_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }

    public async Task<IEnumerable<StatisticInDb>> GetPeopleByRole()
    {
        var sqlString = $@"select role_id as stat_type, 
                             count(id) as stat_value 
                             from rms.people_roles group by role_id;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StatisticInDb>(sqlString);
    }

    public async Task<int> GetPersonDtpInvolvement(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var sqlString = $@"select count(*) from rms.dtp_people
                              where person_id = {id.ToString()}";
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }

    public async Task<int> GetPersonDupInvolvement(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        var sqlString = $@"select count(*) from rms.dup_people
                              where person_id = {id.ToString()}";
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    /************************************************************
    * Full People data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<FullPersonInDb?> GetFullPersonById(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $@"select * from rms.people where id = {id.ToString()}";   
        PersonInDb? personInDb = await conn.QueryFirstOrDefaultAsync<PersonInDb>(sqlString);     
        sqlString = $@"select * from rms.people_roles 
                       where person_id = {id.ToString()} 
                       and is_current = true";
        var personRoleInDb = await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
        
        return new FullPersonInDb(personInDb, personRoleInDb);
    }
    
    // Delete data
    public async Task<int> DeleteFullPerson(int id)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);

        var sqlString = $@"delete from rms.people_roles where person_id = {id.ToString()};";
        await conn.ExecuteAsync(sqlString);
        sqlString = $@"delete from rms.people where id = {id.ToString()};";
        return  await conn.ExecuteAsync(sqlString);
    }
    
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<PersonRoleInDb>> GetPersonRoles(int parId)
    {
        var sqlString = $@"select * from rms.people_roles 
                              where person_id = {parId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonRoleInDb>(sqlString);
    }
    
    public async Task<PersonRoleInDb?> GetPersonCurrentRole(int parId)
    {
        var sqlString = $@"select * from rms.people_roles 
                              where person_id = {parId.ToString()}
                              and is_current = true";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }
    
    public async Task<PersonRoleInDb?> GetPersonRole(int id)
    {
        var sqlString = $@"select * from rms.people_roles 
                              where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }

    // Update data
    public async Task<PersonRoleInDb?> CreatePersonCurrentRole(PersonRoleInDb personRoleContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = await conn.InsertAsync(personRoleContent);
        var sqlString = $@"select * from rms.people_roles 
                              where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }

    public async Task<PersonRoleInDb?> UpdatePersonCurrentRole(PersonRoleInDb personRoleContent)
    {
         var pId = personRoleContent.person_id.ToString();
         await using var conn = new NpgsqlConnection(_dbConnString);
         var sqlString = $@"Update rms.people_roles set
                                      role_id = {personRoleContent.role_id.ToString()},
                                      role_name = '{personRoleContent.role_name}',
                                      is_current = true,
                                      granted = Current_timestamp(0),   
                                      revoked = null
                                      where person_id = {pId};";
         await conn.ExecuteAsync(sqlString);
         sqlString = $@"select * from rms.people_roles where person_id = {pId};";
         return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }
    
    public async Task<int> RevokePersonCurrentRole(int parId)
    {
        var sqlString = $@"Update rms.people_roles 
                              set is_current = false,
                              revoked = Current_timestamp(0)
                              where person_id = {parId.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

}
