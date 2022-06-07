using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.Services.Interfaces;

public interface IContextService
{
    // TEST commit - !!!
    // and again !!! and again!!!  and again  !!!
    
    Task<ICollection<Organisation>> GetOrganisations();
    Task<Organisation> GetOrganisation(int id);
    Task<ICollection<Organisation>> GetOrganisationsByName(string name);

}
