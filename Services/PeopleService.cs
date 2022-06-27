using rmsbe.Services.Interfaces;
using rmsbe.SysModels;    
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class PeopleService : IPeopleService
{
    private readonly IPeopleRepository _peopleRepository;

    public PeopleService(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
    }
    
    // Check if person exists
    public async Task<bool> PersonExistsAsync(int id)
           => await _peopleRepository.PersonExistsAsync(id);
    
    // Check if role exists on this study
    public async Task<bool> PersonAttributeExistsAsync(int parId, string typeName, int id)
           => await _peopleRepository.PersonAttributeExistsAsync(parId, typeName, id);

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
        
    }
    
    public async Task<List<Person>?> GetFilteredPeopleAsync(string titleFilter)
    {
        
    }
    
    public async Task<List<PersonEntry>?> GetPeopleEntriesAsync()
    {
        
    }
    
    public async Task<List<PersonEntry>?> GetRecentPeopleEntriesAsync(int n)
    {
        
    }
    
    public async Task<List<PersonEntry>?> GetPaginatedPeopleEntriesAsync(PaginationRequest validFilter)
    {
        
    }
    
    public async <List<PersonEntry>?> GetPaginatedFilteredPeopleEntriesAsync(string titleFilter, PaginationRequest validFilter)
    {
        
    }
    
    public async Task<List<PersonEntry>?> GetFilteredPeopleEntriesAsync(string titleFilter)
    {
        
    }
    
    public async Task<Person?>GetPersonDataAsync (int id)
    {
        
    }
    
    // Update data
    public async Task<Person?> CreatePersonAsync(Person personContent)
    {
        
    }
    
    public async Task<Person?> UpdatePersonAsync(Person personContent)
    {
        
    }
    
    public async Task<int> DeletePersonAsync(int id)
    {
        
    }
    
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    public async Task<Statistic> GetTotalPeople()
    {
        
    }
    
    public async Task<Statistic> GetTotalFilteredPeople(string titleFilter)
    {
        
    }  
    
    public async Task<List<Statistic>?> GetPeopleByRole()
    {
        
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
        
    }   
    
    public async Task<PersonRole?> GetPersonRoleAsync(int id)
    {
        
    }   
    
    // Update data
    public async Task<PersonRole?> CreatePersonRoleAsync(PersonRole personRoleContent)
    {
        
    } 
    
    public async Task<PersonRole?> UpdatePersonRoleAsync(int id, PersonRole personRoleContent)
    {
        
    }    
    
    public async Task<int> DeletePersonRoleAsync(int id)
    {
        
    } 
    
}

