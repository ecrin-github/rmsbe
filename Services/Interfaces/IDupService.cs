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
    Task<bool> DupDoesNotExistAsync (int id); 
    // Check if attribute exists on this DUP
    Task<bool> DupAttributeDoesNotExistAsync (int dup_id, string type_name, int id); 
    // Check if DUP / object combination exists
    Task<bool> DupObjectDoesNotExistAsync (int dup_id, string sd_oid); 
    // Check if pre-req exists on this DUP / object
    Task<bool> PrereqDoesNotExistAsync (int dup_id, string sd_oid, int id); 
    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dup>?> GetAllDupsAsync();
    Task<List<Dup>?> GetRecentDupsAsync(int n);   
    Task<Dup?> GetDupAsync(int dup_id); 
    // Update data
    Task<Dup?> CreateDupAsync(Dup dupContent);
    Task<Dup?> UpdateDupAsync(int dup_id, Dup dupContent);
    Task<int> DeleteDupAsync(int dup_id); 
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    Task<List<DupStudy>?> GetAllDupStudiesAsync(int dup_id);
    Task<DupStudy?> GetDupStudyAsync(int dup_id); 
    // Update data
    Task<DupStudy?> CreateDupStudyAsync(DupStudy dupStudyContent);
    Task<DupStudy?> UpdateDupStudyAsync(int id, DupStudy dupStudyContent);
    Task<int> DeleteDupStudyAsync(int id); 

    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DupObject>?> GetAllDupObjectsAsync(int dup_id);
    Task<DupObject?> GetDupObjectAsync(int dup_id); 
    // Update data
    Task<DupObject?> CreateDupObjectAsync(DupObject dupObjectContent);
    Task<DupObject?> UpdateDupObjectAsync(int id, DupObject dupObjectContent);
    Task<int> DeleteDupObjectAsync(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dua>?> GetAllDuasAsync(int dup_id);
    Task<Dua?> GetDuaAsync(int dup_id); 
    // Update data
    Task<Dua?> CreateDuaAsync(Dua duaContent);
    Task<Dua?> UpdateDuaAsync(int id, Dua duaContent);
    Task<int> DeleteDuaAsync(int id); 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
    // Fetch data
    Task<List<DupPrereq>?> GetAllDupPrereqsAsync(int dtp_id, string sd_oid);
    Task<DupPrereq?> GetDupPrereqAsync(int id); 
    // Update data
    Task<DupPrereq?> CreateDupPrereqAsync(DupPrereq dtpPrereqContent);
    Task<DupPrereq?> UpdateDupPrereqAsync(int id, DupPrereq dtpPrereqContent);
    Task<int> DeleteDupPrereqAsync(int id); 

    /****************************************************************
    * Secondary use
    ****************************************************************/

    // Fetch data
    Task<List<SecondaryUse>?> GetAllSecUsesAsync(int dup_id);
    Task<SecondaryUse?> GetSecUseAsync(int dup_id); 
    // Update data
    Task<SecondaryUse?> CreateSecUseAsync(SecondaryUse secUseContent);
    Task<SecondaryUse?> UpdateSecUseAsync(int id, SecondaryUse secUseContent);
    Task<int> DeleteSecUseAsync(int id); 

    /****************************************************************
    * DUP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DupNote>?> GetAllDupNotesAsync(int dp_id);
    Task<DupNote?> GetDupNoteAsync(int id); 
    // Update data
    Task<DupNote?> CreateDupNoteAsync(DupNote procNoteContent);
    Task<DupNote?> UpdateDupNoteAsync(int id, DupNote procNoteContent);
    Task<int> DeleteDupNoteAsync(int id); 

    /****************************************************************
    * DUP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DupPerson>?> GetAllDupPeopleAsync(int dp_id);
    Task<DupPerson?> GetDupPersonAsync(int id); 
    // Update data
    Task<DupPerson?> CreateDupPersonAsync(DupPerson procPeopleContent);
    Task<DupPerson?> UpdateDupPersonAsync(int id, DupPerson procPeopleContent);
    Task<int> DeleteDupPersonAsync(int id); 
}