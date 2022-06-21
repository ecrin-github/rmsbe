using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDupRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> DupExistsAsync(int id);
    Task<bool> DupAttributeExistsAsync(int dupId, string typeName, int id);
    Task<bool> DupObjectExistsAsync(int dupId, string sdOid);
    Task<bool> DupObjectAttributeExistsAsync(int dtpId, string sdOid, string typeName, int id);
    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DupInDb>> GetAllDupsAsync();
    Task<IEnumerable<DupInDb>> GetRecentDupsAsync(int n);   
    Task<IEnumerable<DupInDb>> GetPaginatedDupDataAsync(int pNum, int pSize);
    Task<IEnumerable<DupInDb>> GetPaginatedFilteredDupDataAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DupInDb>> GetFilteredDupDataAsync(string titleFilter);
    
    Task<IEnumerable<DupEntryInDb>> GetDupEntriesAsync();
    Task<IEnumerable<DupEntryInDb>> GetRecentDupEntriesAsync(int n);
    Task<IEnumerable<DupEntryInDb>> GetPaginatedDupEntriesAsync(int pNum, int pSize);
    Task<IEnumerable<DupEntryInDb>> GetPaginatedFilteredDupEntriesAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DupEntryInDb>> GetFilteredDupEntriesAsync(string titleFilter);
    
    Task<DupInDb?> GetDupAsync(int dupId); 
    
    // Update data
    Task<DupInDb?> CreateDupAsync(DupInDb dupContent);
    Task<DupInDb?> UpdateDupAsync(DupInDb dupContent);
    Task<int> DeleteDupAsync(int dupId); 
    
    /****************************************************************
    * Dup statistics
    ****************************************************************/

    Task<int> GetTotalDups();
    Task<int> GetTotalFilteredDups(string titleFilter);
    Task<int> GetCompletedDups();
    Task<IEnumerable<StatisticInDb>> GetDupsByStatus();

    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupStudyInDb>> GetAllDupStudiesAsync(int dupId);
    Task<DupStudyInDb?> GetDupStudyAsync(int id); 
    // Update data
    Task<DupStudyInDb?> CreateDupStudyAsync(DupStudyInDb dupStudyContent);
    Task<DupStudyInDb?> UpdateDupStudyAsync(DupStudyInDb dupStudyContent);
    Task<int> DeleteDupStudyAsync(int id); 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupObjectInDb>> GetAllDupObjectsAsync(int dupId);
    Task<DupObjectInDb?> GetDupObjectAsync(int id); 
    // Update data
    Task<DupObjectInDb?> CreateDupObjectAsync(DupObjectInDb dupObjectContent);
    Task<DupObjectInDb?> UpdateDupObjectAsync(DupObjectInDb dupObjectContent);
    Task<int> DeleteDupObjectAsync(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DuaInDb>> GetAllDuasAsync(int dupId);
    Task<DuaInDb?> GetDuaAsync(int dupId); 
    // Update data
    Task<DuaInDb?> CreateDuaAsync(DuaInDb dtaContent);
    Task<DuaInDb?> UpdateDuaAsync(DuaInDb dtaContent);
    Task<int> DeleteDuaAsync(int id); 
    
    
    /****************************************************************
    * DUP Access pre-requisites
    ****************************************************************/
    // Fetch data
    Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqsAsync(int dupId, string sdOid);
    Task<DupPrereqInDb?> GetDupPrereqAsync(int id); 
    // Update data
    Task<DupPrereqInDb?> CreateDupPrereqAsync(DupPrereqInDb dupPrereqContent);
    Task<DupPrereqInDb?> UpdateDupPrereqAsync(DupPrereqInDb dupPrereqContent);
    Task<int> DeleteDupPrereqAsync(int ide);  

    /****************************************************************
     * DUP notes
     ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupNoteInDb>> GetAllDupNotesAsync(int dpId);
    Task<DupNoteInDb?> GetDupNoteAsync(int id); 
    // Update data
    Task<DupNoteInDb?> CreateDupNoteAsync(DupNoteInDb dupNoteContent);
    Task<DupNoteInDb?> UpdateDupNoteAsync(DupNoteInDb dupNoteContent);
    Task<int> DeleteDupNoteAsync(int id); 

    /****************************************************************
    * DUP people
    ****************************************************************/

    // Fetch data 
    Task<IEnumerable<DupPersonInDb>> GetAllDupPeopleAsync(int dpId);
    Task<DupPersonInDb?> GetDupPersonAsync(int id); 
    // Update data
    Task<DupPersonInDb?> CreateDupPersonAsync(DupPersonInDb dupPeopleContent);
    Task<DupPersonInDb?> UpdateDupPersonAsync(DupPersonInDb dupPeopleContent);
    Task<int> DeleteDupPersonAsync(int id);  
    
    
    /****************************************************************
    * Secondary use
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<SecondaryUseInDb>> GetAllSecUsesAsync(int dupId);
    Task<SecondaryUseInDb?> GetSecUseAsync(int dupId); 
    // Update data
    Task<SecondaryUseInDb?> CreateSecUseAsync(SecondaryUseInDb dtaContent);
    Task<SecondaryUseInDb?> UpdateSecUseAsync(SecondaryUseInDb dtaContent);
    Task<int> DeleteSecUseAsync(int id); 
    
}
