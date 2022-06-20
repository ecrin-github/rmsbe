using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IContextRepository
{
    Task<IEnumerable<OrganisationName>> GetOrganisations();
    Task<IEnumerable<OrganisationName>> GetOrganisationsIncString(string search_par);
    
    Task<OrganisationInDb> GetOrganisation(int id);
}
