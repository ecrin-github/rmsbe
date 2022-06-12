// using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ContextService 
    {
        /*
        private readonly ContextDbConnection _dbConnection;

        public ContextService(ContextDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<ICollection<Organisation>> GetOrganisations()
        {
            var organisations = await _dbConnection.Organisations.AnyAsync();
            if (!organisations) return null;
            return await _dbConnection.Organisations.ToArrayAsync();
        }

        public async Task<Organisation> GetOrganisation(int id)
        {
            var organisations = await _dbConnection.Organisations.AnyAsync();
            if (!organisations) return null;
            return await _dbConnection.Organisations
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ICollection<Organisation>> GetOrganisationsByName(string name)
        {
            if (!_dbConnection.Organisations.Any()) return null;
            return await _dbConnection.Organisations
                .AsNoTracking()
                .Where(p => p.DefaultName.ToLower().Contains(name.ToLower())).ToArrayAsync();
        }
           */
    }
 
}