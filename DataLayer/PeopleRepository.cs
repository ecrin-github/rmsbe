using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper.Contrib;
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
    public async Task<bool> PersonExistsAsync(int id)
    {
        string sqlString = $@"select exists (select 1 from rms.people
                              where id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    // Check if attribute (usually role) exists on this person
    public async Task<bool> PersonAttributeExistsAsync(int parId, string typeName, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[typeName]}
                              where id = {id.ToString()} and person_id = {parId.ToString()})";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    // Check a person has no current role in the system
    public async Task<bool> PersonHasNoCurrentRole(int id)
    {
        string sqlString = $@"select count(*) from rms.people_roles
                                   where person_id = {id.ToString()} 
                                   and is_current = true";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString) == 0;
    }

    
    /****************************************************************
    * People data (recorddata only, no attributes)
    ****************************************************************/
      
    // Fetch data
    public async Task<IEnumerable<PersonInDb>> GetPeopleDataAsync()
    {
        string sqlString = $@"select * from rms.people";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }
    
    public async Task<IEnumerable<PersonInDb>> GetRecentPeopleAsync(int n)
    {
        string sqlString = $@"select * from rms.people
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonInDb>> GetPaginatedPeopleDataAsync(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from rms.people 
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonInDb>> GetPaginatedFilteredPeopleAsync(string titleFilter, int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from rms.people 
                              where given_name ilike '%{titleFilter}%'
                              or family_name ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonInDb>> GetFilteredPeopleAsync(string titleFilter)
    {
        string sqlString = $@"select * from rms.people 
                              where given_name ilike '%{titleFilter}%'
                              or family_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonInDb>(sqlString);
    }

    /****************************************************************
    * People entry data (id, name, org and role
    ****************************************************************/
    
    public async Task<IEnumerable<PersonEntryInDb>> GetPeopleEntriesAsync()
    {
        string sqlString = $@"select p.id, p.title, p.given_name, p.family_name,
                              p.org_id, p.org_name, cpr.role_id, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.Id = cpr.person_id";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetRecentPeopleEntriesAsync(int n)
    {
        string sqlString = $@"select p.id, p.title, p.given_name, p.family_name,
                              p.org_id, p.org_name, cpr.role_id, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.Id = cpr.person_id
                              order by p.created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetPaginatedPeopleEntriesAsync(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select p.id, p.title, p.given_name, p.family_name,
                              p.org_id, p.org_name, cpr.role_id, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.Id = cpr.person_id                 
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetPaginatedFilteredPeopleEntriesAsync(string titleFilter, int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select p.id, p.title, p.given_name, p.family_name,
                              p.org_id, p.org_name, cpr.role_id, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.Id = cpr.person_id
                              where p.given_name ilike '%{titleFilter}%'
                              or p.family_name ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }

    public async Task<IEnumerable<PersonEntryInDb>> GetFilteredPeopleEntriesAsync(string titleFilter)
    {
        string sqlString = $@"select p.id, p.title, p.given_name, p.family_name,
                              p.org_id, p.org_name, cpr.role_id, cpr.role_name
                              from rms.people p
                              left join 
                                  (select person_id, role_id, role_name
                                   from rms.people_roles
                                   where is_current = true) cpr
                              on p.Id = cpr.person_id
                              where p.given_name ilike '%{titleFilter}%'
                              or p.family_name ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonEntryInDb>(sqlString);
    }

    
    public async Task<PersonInDb?>GetPersonDataAsync (int id)
    {
        string sqlString = $@"select * from rms.people
                              where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<PersonInDb>(sqlString);
    }

    
    // Update data
    public async Task<PersonInDb?> CreatePersonAsync(PersonInDb personContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(personContent);
        string sqlString = $@"select * from rms.people
                              where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<PersonInDb>(sqlString);
    }

    public async Task<PersonInDb?> UpdatePersonAsync(PersonInDb personContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync<PersonInDb>(personContent)) ? personContent : null;
    }

    public async Task<int> DeletePersonAsync(int id)
    {
        string sqlString = $@"delete from rms.people
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    public async Task<int> GetTotalPeople()
    {
        string sqlString = $@"select count(*) from rms.people;";
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
        string sqlString = $@"select role_type_id as stat_type, 
                             count(id) as stat_value 
                             from rms.people group by role_type_id;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StatisticInDb>(sqlString);
    }

    
    /****************************************************************
    * Full People data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    //public async Task<FullPerson?> GetFullPersonByIdAsync(string sdSid);
    // Update data
    // public async Task<int> DeleteFullPersonAsync(string sdSid);
    
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<PersonRoleInDb>> GetPersonRolesAsync(int parId)
    {
        string sqlString = $@"select * from rms.people_roles 
                              where person_id = {parId.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<PersonRoleInDb>(sqlString);
    }
    
    public async Task<PersonRoleInDb?> GetPersonCurrentRoleAsync(int parId)
    {
        string sqlString = $@"select * from rms.people_roles 
                              where person_id = {parId.ToString()}
                              and is_current = true";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }
    
    public async Task<PersonRoleInDb?> GetPersonRoleAsync(int id)
    {
        string sqlString = $@"select * from rms.people_roles 
                              where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }

    // Update data
    public async Task<PersonRoleInDb?> CreatePersonRoleAsync(PersonRoleInDb personRoleContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(personRoleContent);
        string sqlString = $@"select * from rms.people_roles 
                              where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<PersonRoleInDb>(sqlString);
    }

    public async Task<PersonRoleInDb?> UpdatePersonRoleAsync(PersonRoleInDb personRoleContent)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(personRoleContent)) ? personRoleContent : null;
    }
    
    public async Task<int> RevokePersonRoleAsync(int id)
    {
        string sqlString = $@"Update mdr.people_roles 
                              set is_current = false,
                              revoked = Current_timestamp(0)
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
    
}
