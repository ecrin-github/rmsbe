using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.Services.Interfaces;

public interface IPeopleService
{
    // Check if person exists
    Task<bool> PersonExistsAsync(int id);
    
    // Check if atttribute (currently only role) exists on this person
    Task<bool> PersonAttributeExistsAsync (int parId, string typeName, int id); 
    
    // Check that this person has no current role
    Task<bool> PersonHasCurrentRole(int id);  

    /****************************************************************
    * Study Record (study data only, no attributes)
    ****************************************************************/
      
    // Fetch data
    Task<List<Person>?> GetPeopleDataAsync();
    Task<List<Person>?> GetRecentPeopleAsync(int n);
    Task<List<Person>?> GetPaginatedPeopleDataAsync(PaginationRequest validFilter);
    Task<List<Person>?> GetPaginatedFilteredPeopleAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<Person>?> GetFilteredPeopleAsync(string titleFilter);
    
    Task<List<PersonEntry>?> GetPeopleEntriesAsync();
    Task<List<PersonEntry>?> GetRecentPeopleEntriesAsync(int n);
    Task<List<PersonEntry>?> GetPaginatedPeopleEntriesAsync(PaginationRequest validFilter);
    Task<List<PersonEntry>?> GetPaginatedFilteredPeopleEntriesAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<PersonEntry>?> GetFilteredPeopleEntriesAsync(string titleFilter);
    
    Task<Person?>GetPersonDataAsync (int id);
    
    // Update data
    Task<Person?> CreatePersonAsync(Person personContent);
    Task<Person?> UpdatePersonAsync(Person personContent);
    Task<int> DeletePersonAsync(int id);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalPeople();  
    Task<Statistic> GetTotalFilteredPeople(string titleFilter);  
    Task<List<Statistic>?> GetPeopleByRole();
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    //Task<FullPerson?> GetFullPersonByIdAsync(string sdSid);
    // Update data
    // Task<int> DeleteFullPersonAsync(string sdSid);
        
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    Task<List<PersonRole>?> GetPersonRolesAsync(int parId);
    Task<PersonRole?> GetPersonCurrentRoleAsync(int parId);
    Task<PersonRole?> GetPersonRoleAsync(int id);                  
    // Update data
    Task<PersonRole?> CreatePersonCurrentRoleAsync(PersonRole personRoleContent); 
    Task<PersonRole?> UpdatePersonCurrentRoleAsync(PersonRole personRoleContent);    
    Task<int> RevokePersonCurrentRoleAsync(int parId);  

}