using System.Collections.Generic;
using System.Threading.Tasks;
using ContextService.Models.Ctx;

namespace rmsbe.Services.Interfaces;

public interface IContextService
{
    Task<ICollection<Organisation>> GetOrganisations();
    Task<Organisation> GetOrganisation(int id);
    Task<ICollection<Organisation>> GetOrganisationsByName(string name);
    
    Task<ICollection<OrgAttribute>> GetOrgAttributes(int org_id);
    Task<ICollection<OrgLink>> GetOrgLinks(int org_id);
    Task<ICollection<OrgLocation>> GetOrgLocations(int org_id);
    Task<ICollection<OrgName>> GetOrgNames(int org_id);
    Task<ICollection<OrgRelationship>> GetOrgRelationships(int org_id);
    Task<ICollection<OrgTypeMembership>> GetOrgTypeMemberships(int org_id);
    
    Task<ICollection<People>> GetPeople();
    Task<People> GetPerson(int id);
    Task<ICollection<PeopleLink>> GetPersonLinks(int person_id);
    Task<ICollection<PeopleRole>> GetPersonRoles(int person_id);
    
    Task<ICollection<GeogEntity>> GetGeogEntities();
    Task<GeogEntity> GetGeogEntity(int id);

    Task<ICollection<PublishedJournal>> GetPublishedJournals();
    Task<PublishedJournal> GetPublishedJournal(int id);
}
