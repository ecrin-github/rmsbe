using rmsbe.Services.Interfaces;
using rmsbe.DbModels;    
using rmsbe.SysModels;    
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class PeopleService : IPeopleService
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly ILookupService _lupService;
    private List<Lup> _lookups;

    public PeopleService(IPeopleRepository peopleRepository, ILookupService lupService)
    {
        _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
        _lupService = lupService ?? throw new ArgumentNullException(nameof(lupService));
        _lookups = new List<Lup>();
    }
    
    /****************************************************************
    * Check functions
    ****************************************************************/
    
    // Check if person exists
    public async Task<bool> PersonExists(int id)
           => await _peopleRepository.PersonExists(id);
    
    // Check if attribute (currently only role) exists on this person
    public async Task<bool> PersonAttributeExists(int parId, string typeName, int id)
           => await _peopleRepository.PersonAttributeExists(parId, typeName, id);

    // Check if person has a current role in the system
    public async Task<bool> PersonHasCurrentRole(int id)
        => await _peopleRepository.PersonHasCurrentRole(id);

    /****************************************************************
    * All People / People entries
    ****************************************************************/
      
    public async Task<List<Person>?> GetAllPeopleData()
    {
        var peopleInDb = (await _peopleRepository.GetAllPeopleData()).ToList();
        return !peopleInDb.Any() ? null 
            : peopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetAllPeopleEntries()
    {
        var peopleEntriesInDb = (await _peopleRepository.GetAllPeopleEntries()).ToList();
        return !peopleEntriesInDb.Any() ? null 
            : peopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated People / People entries
    ****************************************************************/
    
    public async Task<List<Person>?> GetPaginatedPeople(PaginationRequest validFilter)
    {
        var pagedPeopleInDb = (await _peopleRepository
            .GetPaginatedPeople(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedPeopleInDb.Any() ? null 
            : pagedPeopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetPaginatedPeopleEntries(PaginationRequest validFilter)
    {
        var pagedPeopleEntriesInDb = (await _peopleRepository
            .GetPaginatedPeopleEntries(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedPeopleEntriesInDb.Any() ? null 
            : pagedPeopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    /****************************************************************
    * Filtered People / People entries
    ****************************************************************/
    
    public async Task<List<Person>?> GetFilteredPeople(string titleFilter)
    {
        var filteredPeopleInDb = (await _peopleRepository
            .GetFilteredPeople(titleFilter)).ToList();
        return !filteredPeopleInDb.Any() ? null 
            : filteredPeopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetFilteredPeopleEntries(string titleFilter)
    {
        var filteredPeopleEntriesInDb = (await _peopleRepository
                    .GetFilteredPeopleEntries(titleFilter)).ToList();
                return !filteredPeopleEntriesInDb.Any() ? null 
                    : filteredPeopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated and filtered People / People entries
    ****************************************************************/
    
    public async Task<List<Person>?> GetPaginatedFilteredPeople(string titleFilter, PaginationRequest validFilter)
    {
        var pagedFilteredPeopleInDb = (await _peopleRepository
            .GetPaginatedFilteredPeople(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredPeopleInDb.Any() ? null 
            : pagedFilteredPeopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetPaginatedFilteredPeopleEntries(string titleFilter, PaginationRequest validFilter)
    {
        var pagedFilteredPeopleEntriesInDb = (await _peopleRepository
            .GetPaginatedFilteredPeopleEntries(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredPeopleEntriesInDb.Any() ? null 
            : pagedFilteredPeopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    /****************************************************************
    * Recent People / People entries
    ****************************************************************/   
    
    public async Task<List<Person>?> GetRecentPeople(int n)
    {
        var peopleInDb = (await _peopleRepository.GetRecentPeople(n)).ToList();
        return !peopleInDb.Any() ? null 
            : peopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetRecentPeopleEntries(int n)
    {
        var peopleEntriesInDb = (await _peopleRepository.GetRecentPeopleEntries(n)).ToList();
        return !peopleEntriesInDb.Any() ? null 
            : peopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    /****************************************************************
    * People / People entries by Org
    ****************************************************************/ 
    
    public async Task<List<Person>?> GetPeopleByOrg(int orgId)
    {
        var peopleByOrgInDb = (await _peopleRepository.GetPeopleByOrg(orgId)).ToList();
        return !peopleByOrgInDb.Any() ? null 
            : peopleByOrgInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetPeopleEntriesByOrg(int orgId)
    {
        var peopleEntriesByOrgInDb = (await _peopleRepository.GetPeopleEntriesByOrg(orgId)).ToList();
        return !peopleEntriesByOrgInDb.Any() ? null 
            : peopleEntriesByOrgInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    /****************************************************************
    * Get single Person record
    ****************************************************************/     
    
    public async Task<Person?>GetPersonData(int id)
    {
        var personInDb = await _peopleRepository.GetPersonData(id);
        return personInDb == null ? null : new Person(personInDb);
    }
    
    /****************************************************************
    * Update People Records
    ****************************************************************/ 
    
    public async Task<Person?> CreatePerson(Person personContent)
    {
        var personInDb = new PersonInDb(personContent);
        var res = await _peopleRepository.CreatePerson(personInDb);
        return res == null ? null : new Person(res);
    }
    
    public async Task<Person?> UpdatePerson(Person personContent)
    {
        var personInDb = new PersonInDb(personContent);
        var res = await _peopleRepository.UpdatePerson(personInDb);
        return res == null ? null : new Person(res);
    }
    
    public async Task<int> DeletePerson(int id)
           => await _peopleRepository.DeletePerson(id);
    
    
    /****************************************************************
    * Full People data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    
    public async Task<FullPerson?> GetFullPersonById(int id){ 
        FullPersonInDb? fullPersonInDb = await _peopleRepository.GetFullPersonById(id);
        return fullPersonInDb == null ? null : new FullPerson(fullPersonInDb);
    }
    
    // Delete data
    public async Task<int> DeleteFullPerson(int id) 
        => await _peopleRepository.DeleteFullPerson(id);
    
        
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    public async Task<Statistic> GetTotalPeople()
    {
        int res = await _peopleRepository.GetTotalPeople();
        return new Statistic("Total", res);
    }
    
    public async Task<Statistic> GetTotalFilteredPeople(string titleFilter)
    {
        int res = await _peopleRepository.GetTotalFilteredPeople(titleFilter);
        return new Statistic("TotalFiltered", res);
    }
    
    public async Task<List<Statistic>> GetPersonInvolvement(int id)
    {
        var stats = new List<Statistic>(); 
        int dtpRes = await _peopleRepository.GetPersonDtpInvolvement(id);
        int dupRes = await _peopleRepository.GetPersonDupInvolvement(id);
        stats.Add(new("DtpTotal", dtpRes));
        stats.Add(new("DupTotal", dupRes));
        return stats;
    }

    public async Task<List<Statistic>?> GetPeopleByRole()
    {
        var res = (await _peopleRepository.GetPeopleByRole()).ToList();
        if (await ResetLookupsAsync("rms-user-types"))
        {
            return !res.Any()
                ? null
                : res.Select(r => new Statistic(LuTypeName(r.stat_type), r.stat_value)).ToList();
        }

        return null;
    }

    private string LuTypeName(int n)
    {
        foreach (var p in _lookups.Where(p => n == p.Id))
        {
            return p.Name ?? "null name in matching lookup!";
        }
        return "not known";
    }

    private async Task<bool> ResetLookupsAsync(string typeName)
    {
        _lookups = new List<Lup>();  // reset to empty list
        _lookups = await _lupService.GetLookUpValues(typeName);
        return _lookups.Count > 0 ;
    }
    
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    public async Task<List<PersonRole>?> GetPersonRoles(int parId)
    {
        var personRolesInDb = (await _peopleRepository.GetPersonRoles(parId)).ToList();
        return (!personRolesInDb.Any()) ? null 
            : personRolesInDb.Select(r => new PersonRole(r)).ToList();
    }   
    
    public async Task<PersonRole?> GetPersonCurrentRole(int parId)
    {
        var personRoleInDb = (await _peopleRepository.GetPersonCurrentRole(parId));
        return personRoleInDb == null ? null : new PersonRole(personRoleInDb);
    }   
    
    public async Task<PersonRole?> GetPersonRole(int id)
    {
        var personRoleInDb = await _peopleRepository.GetPersonRole(id);
        return personRoleInDb == null ? null : new PersonRole(personRoleInDb);
    }   
    
    // Update data
    public async Task<PersonRole?> CreatePersonCurrentRole(PersonRole personRoleContent)
    {
        personRoleContent.IsCurrent = true;
        personRoleContent.Granted = DateTime.Now;
        personRoleContent.Revoked = null;
        var personRoleContentInDb = new PersonRoleInDb(personRoleContent);
        var res = await _peopleRepository.CreatePersonCurrentRole(personRoleContentInDb);
        return res == null ? null : new PersonRole(res);
    } 
    
    public async Task<PersonRole?> UpdatePersonCurrentRole(PersonRole personRoleContent)
    {
        var personRoleContentInDb = new PersonRoleInDb(personRoleContent);
        var res = await _peopleRepository.UpdatePersonCurrentRole(personRoleContentInDb);
        return res == null ? null : new PersonRole(res);
    }    
    
    public async Task<int> RevokePersonCurrentRole(int parId)
        => await _peopleRepository.RevokePersonCurrentRole(parId);
    

}

