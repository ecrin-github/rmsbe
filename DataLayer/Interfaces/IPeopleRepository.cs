using rmsbe.DbModels;
using rmsbe.SysModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IPeopleRepository
{
    // Check if person exists
    Task<bool> PersonExistsAsync(int id);
    
    // Check if role exists on this study
    Task<bool> PersonAttributeExistsAsync (int parId, string typeName, int id); 
    
    // Check a person has no current role in the system
    Task<bool> PersonHasNoCurrentRole(int id);

    /****************************************************************
    * People Records (study data only, no attributes)
    ****************************************************************/
      
    // Fetch data
    Task<IEnumerable<PersonInDb>> GetPeopleDataAsync();
    Task<IEnumerable<PersonInDb>> GetRecentPeopleAsync(int n);
    Task<IEnumerable<PersonInDb>> GetPaginatedPeopleDataAsync(int pageNum, int pageSize);
    Task<IEnumerable<PersonInDb>> GetPaginatedFilteredPeopleAsync(string titleFilter, int pageNum, int pageSize);
    Task<IEnumerable<PersonInDb>> GetFilteredPeopleAsync(string titleFilter);
    
    Task<IEnumerable<PersonEntryInDb>> GetPeopleEntriesAsync();
    Task<IEnumerable<PersonEntryInDb>> GetRecentPeopleEntriesAsync(int n);
    Task<IEnumerable<PersonEntryInDb>> GetPaginatedPeopleEntriesAsync(int pageNum, int pageSize);
    Task<IEnumerable<PersonEntryInDb>> GetPaginatedFilteredPeopleEntriesAsync(string titleFilter, int pageNum, int pageSize);
    Task<IEnumerable<PersonEntryInDb>> GetFilteredPeopleEntriesAsync(string titleFilter);
    
    Task<PersonInDb?>GetPersonDataAsync (int id);
    
    // Update data
    Task<PersonInDb?> CreatePersonAsync(PersonInDb personContent);
    Task<PersonInDb?> UpdatePersonAsync(PersonInDb personContent);
    Task<int> DeletePersonAsync(int id);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<int> GetTotalPeople();  
    Task<int> GetTotalFilteredPeople(string titleFilter);  
    Task<IEnumerable<StatisticInDb>> GetPeopleByRole();
    
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
    Task<IEnumerable<PersonRoleInDb>> GetPersonRolesAsync(int parId);
    Task<PersonRoleInDb?> GetPersonCurrentRoleAsync(int parId);
    Task<PersonRoleInDb?> GetPersonRoleAsync(int id);                  
    // Update data
    Task<PersonRoleInDb?> CreatePersonRoleAsync(PersonRoleInDb personRoleContent); 
    Task<PersonRoleInDb?> UpdatePersonRoleAsync(PersonRoleInDb personRoleContent);    
    Task<int> RevokePersonRoleAsync(int id);  
    
}
