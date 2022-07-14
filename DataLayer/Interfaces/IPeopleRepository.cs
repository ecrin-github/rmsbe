using rmsbe.DbModels;
using rmsbe.SysModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IPeopleRepository
{
    /****************************************************************
    * Check functions
    ****************************************************************/
    
    Task<bool> PersonExists(int id);
    Task<bool> PersonAttributeExists (int parId, string typeName, int id); 
    Task<bool> PersonHasCurrentRole(int id);

    /****************************************************************
    * Fetch data
    ****************************************************************/
      
    Task<IEnumerable<PersonInDb>> GetAllPeopleData();
    Task<IEnumerable<PersonEntryInDb>> GetAllPeopleEntries();
    
    Task<IEnumerable<PersonInDb>> GetPaginatedPeople(int pageNum, int pageSize);
    Task<IEnumerable<PersonEntryInDb>> GetPaginatedPeopleEntries(int pageNum, int pageSize);
    
    Task<IEnumerable<PersonInDb>> GetFilteredPeople(string titleFilter);
    Task<IEnumerable<PersonEntryInDb>> GetFilteredPeopleEntries(string titleFilter);
    
    Task<IEnumerable<PersonInDb>> GetPaginatedFilteredPeople(string titleFilter, int pageNum, int pageSize);
    Task<IEnumerable<PersonEntryInDb>> GetPaginatedFilteredPeopleEntries(string titleFilter, int pageNum, int pageSize);
    
    Task<IEnumerable<PersonInDb>> GetRecentPeople(int n);
    Task<IEnumerable<PersonEntryInDb>> GetRecentPeopleEntries(int n);
    
    Task<IEnumerable<PersonInDb>> GetPeopleByOrg(int orgId);
    Task<IEnumerable<PersonEntryInDb>> GetPeopleEntriesByOrg(int orgId);
    
    Task<PersonInDb?>GetPersonData (int id);
    
    /****************************************************************
    * Update data
    ****************************************************************/
    
    Task<PersonInDb?> CreatePerson(PersonInDb personContent);
    Task<PersonInDb?> UpdatePerson(PersonInDb personContent);
    Task<int> DeletePerson(int id);
    
    /****************************************************************
    * Full Person data (including attributes in other tables)
    * For now only attribute is a single person role
    ****************************************************************/
  
    Task<FullPersonInDb?> GetFullPersonById(int id);
    Task<int> DeleteFullPerson(int id);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<int> GetTotalPeople();  
    Task<int> GetTotalFilteredPeople(string titleFilter);  
    Task<IEnumerable<StatisticInDb>> GetPeopleByRole();
    Task<int> GetPersonDtpInvolvement(int id);
    Task<int> GetPersonDupInvolvement(int id);
    
    /****************************************************************
    * Full People data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    //Task<FullPerson?> GetFullPersonById(string sdSid);
    // Update data
    // Task<int> DeleteFullPerson(string sdSid);
        
    /****************************************************************
    * People Roles
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<PersonRoleInDb>> GetPersonRoles(int parId);
    Task<PersonRoleInDb?> GetPersonCurrentRole(int parId);
    Task<PersonRoleInDb?> GetPersonRole(int id);                  
    // Update data
    Task<PersonRoleInDb?> CreatePersonCurrentRole(PersonRoleInDb personRoleContent); 
    Task<PersonRoleInDb?> UpdatePersonCurrentRole(PersonRoleInDb personRoleContent);    
    Task<int> RevokePersonCurrentRole(int id);  

}
