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
    
    // Check if person exists
    public async Task<bool> PersonExistsAsync(int id)
           => await _peopleRepository.PersonExistsAsync(id);
    
    // Check if attribute (currently only role) exists on this person
    public async Task<bool> PersonAttributeExistsAsync(int parId, string typeName, int id)
           => await _peopleRepository.PersonAttributeExistsAsync(parId, typeName, id);

    // Check if person has a current role in the system
    public async Task<bool> PersonHasCurrentRole(int id)
        => await _peopleRepository.PersonHasCurrentRole(id);

    /****************************************************************
    * Study Record (study data only, no attributes)
    ****************************************************************/
      
    // Fetch data
    public async Task<List<Person>?> GetPeopleDataAsync()
    {
        var peopleInDb = (await _peopleRepository.GetPeopleDataAsync()).ToList();
        return !peopleInDb.Any() ? null 
            : peopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<Person>?> GetRecentPeopleAsync(int n)
    {
        var peopleInDb = (await _peopleRepository.GetRecentPeopleAsync(n)).ToList();
        return !peopleInDb.Any() ? null 
            : peopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<Person>?> GetPaginatedPeopleDataAsync(PaginationRequest validFilter)
    {
        var pagedPeopleInDb = (await _peopleRepository
            .GetPaginatedPeopleDataAsync(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedPeopleInDb.Any() ? null 
            : pagedPeopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<Person>?> GetPaginatedFilteredPeopleAsync(string titleFilter, PaginationRequest validFilter)
    {
        var pagedFilteredPeopleInDb = (await _peopleRepository
            .GetPaginatedFilteredPeopleAsync(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredPeopleInDb.Any() ? null 
            : pagedFilteredPeopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<Person>?> GetFilteredPeopleAsync(string titleFilter)
    {
        var filteredPeopleInDb = (await _peopleRepository
            .GetFilteredPeopleAsync(titleFilter)).ToList();
        return !filteredPeopleInDb.Any() ? null 
            : filteredPeopleInDb.Select(r => new Person(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetPeopleEntriesAsync()
    {
        var peopleEntriesInDb = (await _peopleRepository.GetPeopleEntriesAsync()).ToList();
        return !peopleEntriesInDb.Any() ? null 
            : peopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetRecentPeopleEntriesAsync(int n)
    {
        var peopleEntriesInDb = (await _peopleRepository.GetRecentPeopleEntriesAsync(n)).ToList();
        return !peopleEntriesInDb.Any() ? null 
            : peopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetPaginatedPeopleEntriesAsync(PaginationRequest validFilter)
    {
        var pagedPeopleEntriesInDb = (await _peopleRepository
            .GetPaginatedPeopleEntriesAsync(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedPeopleEntriesInDb.Any() ? null 
            : pagedPeopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetPaginatedFilteredPeopleEntriesAsync(string titleFilter, PaginationRequest validFilter)
    {
        var pagedFilteredPeopleEntriesInDb = (await _peopleRepository
            .GetPaginatedFilteredPeopleEntriesAsync(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredPeopleEntriesInDb.Any() ? null 
            : pagedFilteredPeopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    public async Task<List<PersonEntry>?> GetFilteredPeopleEntriesAsync(string titleFilter)
    {
        var filteredPeopleEntriesInDb = (await _peopleRepository
                    .GetFilteredPeopleEntriesAsync(titleFilter)).ToList();
                return !filteredPeopleEntriesInDb.Any() ? null 
                    : filteredPeopleEntriesInDb.Select(r => new PersonEntry(r)).ToList();
    }
    
    public async Task<Person?>GetPersonDataAsync (int id)
    {
        var personInDb = await _peopleRepository.GetPersonDataAsync(id);
        return personInDb == null ? null : new Person(personInDb);
    }
    
    // Update data
    public async Task<Person?> CreatePersonAsync(Person personContent)
    {
        var personInDb = new PersonInDb(personContent);
        var res = await _peopleRepository.CreatePersonAsync(personInDb);
        return res == null ? null : new Person(res);
    }
    
    public async Task<Person?> UpdatePersonAsync(Person personContent)
    {
        var personInDb = new PersonInDb(personContent);
        var res = await _peopleRepository.UpdatePersonAsync(personInDb);
        return res == null ? null : new Person(res);
    }
    
    public async Task<int> DeletePersonAsync(int id)
           => await _peopleRepository.DeletePersonAsync(id);
    
    
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
        _lookups = await _lupService.GetLookUpValuesAsync(typeName);
        return _lookups.Count > 0 ;
    }
    
    /****************************************************************
    * Full People data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    //Task<FullPerson?> GetFullPersonByIdAsync(string sdSid);
    // Update data
    // Task<int> DeleteFullPersonAsync(string sdSid);
        
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    public async Task<List<PersonRole>?> GetPersonRolesAsync(int parId)
    {
        var personRolesInDb = (await _peopleRepository.GetPersonRolesAsync(parId)).ToList();
        return (!personRolesInDb.Any()) ? null 
            : personRolesInDb.Select(r => new PersonRole(r)).ToList();
    }   
    
    public async Task<PersonRole?> GetPersonCurrentRoleAsync(int parId)
    {
        var personRoleInDb = (await _peopleRepository.GetPersonCurrentRoleAsync(parId));
        return personRoleInDb == null ? null : new PersonRole(personRoleInDb);
    }   
    
    public async Task<PersonRole?> GetPersonRoleAsync(int id)
    {
        var personRoleInDb = await _peopleRepository.GetPersonRoleAsync(id);
        return personRoleInDb == null ? null : new PersonRole(personRoleInDb);
    }   
    
    // Update data
    public async Task<PersonRole?> CreatePersonCurrentRoleAsync(PersonRole personRoleContent)
    {
        personRoleContent.IsCurrent = true;
        personRoleContent.Granted = DateTime.Now;
        personRoleContent.Revoked = null;
        var personRoleContentInDb = new PersonRoleInDb(personRoleContent);
        var res = await _peopleRepository.CreatePersonCurrentRoleAsync(personRoleContentInDb);
        return res == null ? null : new PersonRole(res);
    } 
    
    public async Task<PersonRole?> UpdatePersonCurrentRoleAsync(PersonRole personRoleContent)
    {
        var personRoleContentInDb = new PersonRoleInDb(personRoleContent);
        var res = await _peopleRepository.UpdatePersonCurrentRoleAsync(personRoleContentInDb);
        return res == null ? null : new PersonRole(res);
    }    
    
    public async Task<int> RevokePersonCurrentRoleAsync(int parId)
        => await _peopleRepository.RevokePersonCurrentRoleAsync(parId);
    

}

