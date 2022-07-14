using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.Services.Interfaces;

public interface IPeopleService
{
    // Check if person exists
    Task<bool> PersonExists(int id);
    
    // Check if attribute (currently only role) exists on this person
    Task<bool> PersonAttributeExists (int parId, string typeName, int id); 
    
    // Check that this person has a current role
    Task<bool> PersonHasCurrentRole(int id);  

    /****************************************************************
    * Fetch data
    ****************************************************************/
      
    Task<List<Person>?> GetAllPeopleData();
    Task<List<PersonEntry>?> GetAllPeopleEntries();
    
    Task<List<Person>?> GetPaginatedPeopleData(PaginationRequest validFilter);
    Task<List<PersonEntry>?> GetPaginatedPeopleEntries(PaginationRequest validFilter);
    
    Task<List<Person>?> GetFilteredPeople(string titleFilter);
    Task<List<PersonEntry>?> GetFilteredPeopleEntries(string titleFilter);
    
    Task<List<Person>?> GetPaginatedFilteredPeople(string titleFilter, PaginationRequest validFilter);
    Task<List<PersonEntry>?> GetPaginatedFilteredPeopleEntries(string titleFilter, PaginationRequest validFilter);
    
    Task<List<Person>?> GetPeopleByOrg(int n);
    Task<List<PersonEntry>?> GetRecentPeopleEntries(int n);
    
    Task<List<Person>?> GetRecentPeople(int orgId);
    Task<List<PersonEntry>?> GetPeopleEntriesByOrg(int orgId);
    
    Task<Person?>GetPersonData (int id);
    
    // Update data
    Task<Person?> CreatePerson(Person personContent);
    Task<Person?> UpdatePerson(Person personContent);
    Task<int> DeletePerson(int id);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalPeople();  
    Task<Statistic> GetTotalFilteredPeople(string titleFilter);  
    Task<List<Statistic>?> GetPeopleByRole();
    Task<List<Statistic>> GetPersonInvolvement(int id);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    //Task<FullPerson?> GetFullPersonById(string sdSid);
    // Update data
    // Task<int> DeleteFullPerson(string sdSid);
        
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    Task<List<PersonRole>?> GetPersonRoles(int parId);
    Task<PersonRole?> GetPersonCurrentRole(int parId);
    Task<PersonRole?> GetPersonRole(int id);        
    
    // Update data
    Task<PersonRole?> CreatePersonCurrentRole(PersonRole personRoleContent); 
    Task<PersonRole?> UpdatePersonCurrentRole(PersonRole personRoleContent);    
    Task<int> RevokePersonCurrentRole(int parId);  

}