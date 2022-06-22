using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using Npgsql;
using Dapper.Contrib;
using Dapper;

namespace rmsbe.DataLayer;

public class ContextRepository : IContextRepository
{
    /*
    private string _dbconnstring;
    public ContextRepository(string dbconnstring)
    {
        _dbconnstring = dbconnstring ?? throw new ArgumentNullException(nameof(dbconnstring));
    }
    
    public async Task<IEnumerable<OrganisationName>> GetOrganisations()
    {
        string sqlstring = "select id as Id, name as Name from ctx.org_names";
        await using var conn = new NpgsqlConnection(_dbconnstring);
        conn.Open();
        return await conn.QueryAsync<OrganisationName>(sqlstring);
    }
    
    public async Task<IEnumerable<OrganisationName>> GetOrganisationsIncString(string search_par)
    {
        string sqlstring = $"select id as Id, name as Name from ctx.org_names where name ilike '%{search_par}%'";
        await using var conn = new NpgsqlConnection(_dbconnstring);
        conn.Open();
        return await conn.QueryAsync<OrganisationName>(sqlstring);
     }
    
    public async Task<OrganisationInDb> GetOrganisation(int id)
    {
        string sqlstring = "select * from ctx.organisations where id = " + id.ToString();
        await using var conn = new NpgsqlConnection(_dbconnstring);
        conn.Open();
        return await conn.QueryFirstAsync<OrganisationInDb>(sqlstring);
    }
    */
}
