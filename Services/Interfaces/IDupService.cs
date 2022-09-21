using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IDupService
{
    /****************************************************************
    * Check functions
    ****************************************************************/
    
    Task<bool> DupExists (int id); 
    Task<bool> DupAttributeExists (int dupId, string typeName, int id);
    Task<bool> DupDuaExists(int dupId);
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
    * DUP record data
    ****************************************************************/
    
    Task<Dup?> GetDup(int dupId); 
    Task<DupOut?> GetOutDup(int dupId); 
    
    Task<Dup?> CreateDup(Dup dupContent);
    Task<Dup?> UpdateDup(int dupId, Dup dupContent);
    Task<int> DeleteDup(int dupId); 
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    Task<List<DupStudy>?> GetAllDupStudies(int dupId);
    Task<List<DupStudyOut>?> GetAllOutDupStudies(int dupId);
    
    Task<DupStudy?> GetDupStudy(int dupId); 
    Task<DupStudyOut?> GetOutDupStudy(int id); 
    
    // Update data
    Task<DupStudy?> CreateDupStudy(DupStudy dupStudyContent);
    Task<DupStudy?> UpdateDupStudy(DupStudy dupStudyContent);
    Task<int> DeleteDupStudy(int id); 

    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DupObject>?> GetAllDupObjects(int dupId);
    Task<List<DupObjectOut>?> GetAllOutDupObjects(int dupId);
    
    Task<DupObject?> GetDupObject(int dupId); 
    Task<DupObjectOut?> GetOutDupObject(int id); 
    
    // Update data
    Task<DupObject?> CreateDupObject(DupObject dupObjectContent);
    Task<DupObject?> UpdateDupObject(DupObject dupObjectContent);
    Task<int> DeleteDupObject(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<Dua?> GetDua(int dupId); 
    Task<DuaOut?> GetOutDua(int dupId); 
    
    // Update data
    Task<Dua?> CreateDua(Dua duaContent);
    Task<Dua?> UpdateDua(Dua duaContent);
    Task<int> DeleteDua(int dupId); 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
   
    // Fetch data
    Task<List<DupPrereq>?> GetAllDupPrereqs(int dupId, string sdOid);
    Task<List<DupPrereqOut>?> GetAllOutDupPrereqs(int dupId, string sdOid);
    
    Task<DupPrereq?> GetDupPrereq(int id); 
    Task<DupPrereqOut?> GetOutDupPrereq(int id); 
    
    // Update data
    Task<DupPrereq?> CreateDupPrereq(DupPrereq dupPrereqContent);
    Task<DupPrereq?> UpdateDupPrereq(DupPrereq dupPrereqContent);
    Task<int> DeleteDupPrereq(int id); 

    /****************************************************************
    * DUP Secondary use
    ****************************************************************/

    // Fetch data
    Task<List<DupSecondaryUse>?> GetAllSecUses(int dupId);
    Task<DupSecondaryUse?> GetSecUse(int dupId); 
    
    // Update data
    Task<DupSecondaryUse?> CreateSecUse(DupSecondaryUse secUseContent);
    Task<DupSecondaryUse?> UpdateSecUse(DupSecondaryUse secUseContent);
    Task<int> DeleteSecUse(int id); 

    /****************************************************************
    * DUP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DupNote>?> GetAllDupNotes(int dpId);
    Task<List<DupNoteOut>?> GetAllOutDupNotes(int dpId);
    
    Task<DupNote?> GetDupNote(int id); 
    Task<DupNoteOut?> GetOutDupNote(int id); 
    
    // Update data
    Task<DupNote?> CreateDupNote(DupNote procNoteContent);
    Task<DupNote?> UpdateDupNote(DupNote procNoteContent);
    Task<int> DeleteDupNote(int id); 

    /****************************************************************
    * DUP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DupPerson>?> GetAllDupPeople(int dpId);
    Task<List<DupPersonOut>?> GetAllOutDupPeople(int dpId);
    
    Task<DupPerson?> GetDupPerson(int id); 
    Task<DupPersonOut?> GetOutDupPerson(int id); 
    
    // Update data
    Task<DupPerson?> CreateDupPerson(DupPerson procPeopleContent);
    Task<DupPerson?> UpdateDupPerson(DupPerson procPeopleContent);
    Task<int> DeleteDupPerson(int id); 
    
}