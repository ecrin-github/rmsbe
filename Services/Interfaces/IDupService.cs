using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IDupService
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/
    
    // Check if DUP exists
    Task<bool> DupExistsAsync (int id); 
    
    // Check if attribute exists on this DUP
    Task<bool> DupAttributeExistsAsync (int dupId, string typeName, int id);
    
    // Check if DUP / object combination exists
    Task<bool> DupObjectExistsAsync (int dupId, string sdOid); 
    
    // Check if pre-req exists on this DUP / object
    Task<bool> DupObjectAttributeExistsAsync(int dupId, string sdOid, string typeName, int id);
    
    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dup>?> GetAllDupsAsync();
    Task<List<Dup>?> GetRecentDupsAsync(int n);   
    Task<List<Dup>?> GetPaginatedDupDataAsync(PaginationRequest validFilter);
    Task<List<Dup>?> GetPaginatedFilteredDupRecordsAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<Dup>?> GetFilteredDupRecordsAsync(string titleFilter);
    
    Task<List<DupEntry>?> GetDupEntriesAsync();
    Task<List<DupEntry>?> GetRecentDupEntriesAsync(int n);
    Task<List<DupEntry>?> GetPaginatedDupEntriesAsync(PaginationRequest validFilter);
    Task<List<DupEntry>?> GetPaginatedFilteredDupEntriesAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<DupEntry>?> GetFilteredDupEntriesAsync(string titleFilter);
    
    Task<Dup?> GetDupAsync(int dupId); 
    // Update data
    Task<Dup?> CreateDupAsync(Dup dupContent);
    Task<Dup?> UpdateDupAsync(int dupId, Dup dupContent);
    Task<int> DeleteDupAsync(int dupId); 
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalDups();  
    Task<Statistic> GetTotalFilteredDups(string titleFilter);  
    Task<List<Statistic>?> GetDupsByStatus();
    Task<List<Statistic>> GetDupsByCompletion();
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    Task<List<DupStudy>?> GetAllDupStudiesAsync(int dupId);
    Task<DupStudy?> GetDupStudyAsync(int dupId); 
    // Update data
    Task<DupStudy?> CreateDupStudyAsync(DupStudy dupStudyContent);
    Task<DupStudy?> UpdateDupStudyAsync(int id, DupStudy dupStudyContent);
    Task<int> DeleteDupStudyAsync(int id); 

    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DupObject>?> GetAllDupObjectsAsync(int dupId);
    Task<DupObject?> GetDupObjectAsync(int dupId); 
    // Update data
    Task<DupObject?> CreateDupObjectAsync(DupObject dupObjectContent);
    Task<DupObject?> UpdateDupObjectAsync(int id, DupObject dupObjectContent);
    Task<int> DeleteDupObjectAsync(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dua>?> GetAllDuasAsync(int dupId);
    Task<Dua?> GetDuaAsync(int dupId); 
    // Update data
    Task<Dua?> CreateDuaAsync(Dua duaContent);
    Task<Dua?> UpdateDuaAsync(int id, Dua duaContent);
    Task<int> DeleteDuaAsync(int id); 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
    // Fetch data
    Task<List<DupPrereq>?> GetAllDupPrereqsAsync(int dtpId, string sdOid);
    Task<DupPrereq?> GetDupPrereqAsync(int id); 
    // Update data
    Task<DupPrereq?> CreateDupPrereqAsync(DupPrereq dtpPrereqContent);
    Task<DupPrereq?> UpdateDupPrereqAsync(int id, DupPrereq dtpPrereqContent);
    Task<int> DeleteDupPrereqAsync(int id); 

    /****************************************************************
    * Secondary use
    ****************************************************************/

    // Fetch data
    Task<List<SecondaryUse>?> GetAllSecUsesAsync(int dupId);
    Task<SecondaryUse?> GetSecUseAsync(int dupId); 
    // Update data
    Task<SecondaryUse?> CreateSecUseAsync(SecondaryUse secUseContent);
    Task<SecondaryUse?> UpdateSecUseAsync(int id, SecondaryUse secUseContent);
    Task<int> DeleteSecUseAsync(int id); 

    /****************************************************************
    * DUP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DupNote>?> GetAllDupNotesAsync(int dpId);
    Task<DupNote?> GetDupNoteAsync(int id); 
    // Update data
    Task<DupNote?> CreateDupNoteAsync(DupNote procNoteContent);
    Task<DupNote?> UpdateDupNoteAsync(int id, DupNote procNoteContent);
    Task<int> DeleteDupNoteAsync(int id); 

    /****************************************************************
    * DUP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DupPerson>?> GetAllDupPeopleAsync(int dpId);
    Task<DupPerson?> GetDupPersonAsync(int id); 
    // Update data
    Task<DupPerson?> CreateDupPersonAsync(DupPerson procPeopleContent);
    Task<DupPerson?> UpdateDupPersonAsync(int id, DupPerson procPeopleContent);
    Task<int> DeleteDupPersonAsync(int id); 
    
}