using rmsbe.DbModels;
using rmsbe.SysModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IPeopleRepository
{
    // Check if person exists
    Task<bool> PersonExistsAsync(int id);
    
    // Check if role exists on this study
    Task<bool> PersonAttributeExistsAsync (int parId, string typeName, int id); 

    /****************************************************************
    * Study Record (study data only, no attributes)
    ****************************************************************/
      
    // Fetch data
    Task<List<PersonInDb>> GetPeopleDataAsync();
    Task<List<PersonInDb>> GetRecentPeopleAsync(int n);
    Task<List<PersonInDb>> GetPaginatedPeopleDataAsync(PaginationRequest validFilter);
    Task<List<PersonInDb>> GetPaginatedFilteredPeopleAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<PersonInDb>> GetFilteredPeopleAsync(string titleFilter);
    
    Task<List<PersonEntry>> GetPeopleEntriesAsync();
    Task<List<PersonEntry>> GetRecentPeopleEntriesAsync(int n);
    Task<List<PersonEntry>> GetPaginatedPeopleEntriesAsync(PaginationRequest validFilter);
    Task<List<PersonEntry>> GetPaginatedFilteredPeopleEntriesAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<PersonEntry>> GetFilteredPeopleEntriesAsync(string titleFilter);
    
    Task<PersonInDb?>GetPersonDataAsync (int id);
    
    // Update data
    Task<PersonInDb?> CreatePersonAsync(Person personContent);
    Task<PersonInDb?> UpdatePersonAsync(Person personContent);
    Task<int> DeletePersonAsync(int id);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalPeople();  
    Task<Statistic> GetTotalFilteredPeople(string titleFilter);  
    Task<List<Statistic>> GetPeopleByRole();
    
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
    Task<List<PersonRoleInDb>> GetPersonRolesAsync(int parId);     
    Task<PersonRoleInDb?> GetPersonRoleAsync(int id);                  
    // Update data
    Task<PersonRoleInDb?> CreatePersonRoleAsync(PersonRole personRoleContent); 
    Task<PersonRoleInDb?> UpdatePersonRoleAsync(int id, PersonRole personRoleContent);    
    Task<int> DeletePersonRoleAsync(int id);  
    
}
