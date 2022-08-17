using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IDupService
{
    /****************************************************************
    * Check functions
    ****************************************************************/
    
    Task<bool> DupExists (int id); 
    Task<bool> DupAttributeExists (int dupId, string typeName, int id);
    Task<bool> DupObjectExists (int dupId, string sdOid); 
    Task<bool> DupObjectAttributeExists(int dupId, string sdOid, string typeName, int id);
    
    /****************************************************************
    * Fetch DUP / DUP entry data
    ****************************************************************/
    
    Task<List<Dup>?> GetAllDups();
    Task<List<DupEntry>?> GetAllDupEntries();
    
    Task<List<Dup>?> GetPaginatedDupData(PaginationRequest validFilter);
    Task<List<DupEntry>?> GetPaginatedDupEntries(PaginationRequest validFilter);
    
    Task<List<Dup>?> GetFilteredDupRecords(string titleFilter);
    Task<List<DupEntry>?> GetFilteredDupEntries(string titleFilter);
    
    Task<List<Dup>?> GetPaginatedFilteredDupRecords(string titleFilter, PaginationRequest validFilter);
    Task<List<DupEntry>?> GetPaginatedFilteredDupEntries(string titleFilter, PaginationRequest validFilter);
    
    Task<List<Dup>?> GetRecentDups(int n);   
    Task<List<DupEntry>?> GetRecentDupEntries(int n);
    
    Task<List<Dup>?> GetDupsByOrg(int orgId);   
    Task<List<DupEntry>?> GetDupEntriesByOrg(int orgId);
    
    Task<Dup?> GetDup(int dupId); 
    
    /****************************************************************
    * Update DUP data
    ****************************************************************/
    
    Task<Dup?> CreateDup(Dup dupContent);
    Task<Dup?> UpdateDup(int dupId, Dup dupContent);
    Task<int> DeleteDup(int dupId); 
    
    /****************************************************************
    * Fetch / Delete full DUP data (with attributes)
    ****************************************************************/

    Task<FullDup?> GetFullDupById(int id);
    Task<int> DeleteFullDup(int id);
    
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
    Task<List<DupStudy>?> GetAllDupStudies(int dupId);
    Task<DupStudy?> GetDupStudy(int dupId); 
    
    // Update data
    Task<DupStudy?> CreateDupStudy(DupStudy dupStudyContent);
    Task<DupStudy?> UpdateDupStudy(DupStudy dupStudyContent);
    Task<int> DeleteDupStudy(int id); 

    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DupObject>?> GetAllDupObjects(int dupId);
    Task<DupObject?> GetDupObject(int dupId); 
    
    // Update data
    Task<DupObject?> CreateDupObject(DupObject dupObjectContent);
    Task<DupObject?> UpdateDupObject(DupObject dupObjectContent);
    Task<int> DeleteDupObject(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dua>?> GetAllDuas(int dupId);
    Task<Dua?> GetDua(int dupId); 
    
    // Update data
    Task<Dua?> CreateDua(Dua duaContent);
    Task<Dua?> UpdateDua(Dua duaContent);
    Task<int> DeleteDua(int id); 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
   
    // Fetch data
    Task<List<DupPrereq>?> GetAllDupPrereqs(int dtpId, string sdOid);
    Task<DupPrereq?> GetDupPrereq(int id); 
    
    // Update data
    Task<DupPrereq?> CreateDupPrereq(DupPrereq dtpPrereqContent);
    Task<DupPrereq?> UpdateDupPrereq(DupPrereq dtpPrereqContent);
    Task<int> DeleteDupPrereq(int id); 

    /****************************************************************
    * Secondary use
    ****************************************************************/

    // Fetch data
    Task<List<SecondaryUse>?> GetAllSecUses(int dupId);
    Task<SecondaryUse?> GetSecUse(int dupId); 
    
    // Update data
    Task<SecondaryUse?> CreateSecUse(SecondaryUse secUseContent);
    Task<SecondaryUse?> UpdateSecUse(SecondaryUse secUseContent);
    Task<int> DeleteSecUse(int id); 

    /****************************************************************
    * DUP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DupNote>?> GetAllDupNotes(int dpId);
    Task<DupNote?> GetDupNote(int id); 
    
    // Update data
    Task<DupNote?> CreateDupNote(DupNote procNoteContent);
    Task<DupNote?> UpdateDupNote(DupNote procNoteContent);
    Task<int> DeleteDupNote(int id); 

    /****************************************************************
    * DUP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DupPerson>?> GetAllDupPeople(int dpId);
    Task<DupPerson?> GetDupPerson(int id); 
    // Update data
    Task<DupPerson?> CreateDupPerson(DupPerson procPeopleContent);
    Task<DupPerson?> UpdateDupPerson(DupPerson procPeopleContent);
    Task<int> DeleteDupPerson(int id); 
    
}