using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDupRepository
{
    Task<bool> DupDoesNotExistAsync(int id);
    Task<bool> DupExistsAsync(int id);
    
    Task<bool> DupAttributeDoesNotExistAsync(int dup_id, string type_name, int id);
    Task<bool> DupAttributeExistsAsync(int dup_id, string type_name, int id);
    
    Task<bool> DupObjectDoesNotExistAsync(int dup_id, string sd_oid);
    Task<bool> DupObjectExistsAsync(int dup_id, string sd_oid);
    
    Task<bool> DupAttributePrereqDoesNotExistAsync(int dup_id, string sd_oid, int id);
    Task<bool> DupObjectPrereqExistsAsync(int dup_id, string sd_oid, int id);

    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DupInDb>> GetAllDupsAsync();
    Task<IEnumerable<DupInDb>> GetRecentDupsAsync(int n);   
    Task<DupInDb?> GetDupAsync(int dup_id); 
    // Update data
    Task<DupInDb?> CreateDupAsync(DupInDb dupContent);
    Task<DupInDb?> UpdateDupAsync(DupInDb dupContent);
    Task<int> DeleteDupAsync(int dup_id); 
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupStudyInDb>> GetAllDupStudiesAsync(int dup_id);
    Task<DupStudyInDb?> GetDupStudyAsync(int id); 
    // Update data
    Task<DupStudyInDb?> CreateDupStudyAsync(DupStudyInDb dupStudyContent);
    Task<DupStudyInDb?> UpdateDupStudyAsync(DupStudyInDb dupStudyContent);
    Task<int> DeleteDupStudyAsync(int id); 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupObjectInDb>> GetAllDupObjectsAsync(int dup_id);
    Task<DupObjectInDb?> GetDupObjectAsync(int id); 
    // Update data
    Task<DupObjectInDb?> CreateDupObjectAsync(DupObjectInDb dupObjectContent);
    Task<DupObjectInDb?> UpdateDupObjectAsync(DupObjectInDb dupObjectContent);
    Task<int> DeleteDupObjectAsync(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DuaInDb>> GetAllDuasAsync(int dup_id);
    Task<DuaInDb?> GetDuaAsync(int dup_id); 
    // Update data
    Task<DuaInDb?> CreateDuaAsync(DuaInDb dtaContent);
    Task<DuaInDb?> UpdateDuaAsync(DuaInDb dtaContent);
    Task<int> DeleteDuaAsync(int id); 
    
    
    /****************************************************************
    * DUP Access pre-requisites
    ****************************************************************/
    // Fetch data
    Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqsAsync(int dup_id, string sd_oid);
    Task<DupPrereqInDb?> GetDupPrereqAsync(int id); 
    // Update data
    Task<DupPrereqInDb?> CreateDupPrereqAsync(DupPrereqInDb dupPrereqContent);
    Task<DupPrereqInDb?> UpdateDupPrereqAsync(DupPrereqInDb dupPrereqContent);
    Task<int> DeleteDupPrereqAsync(int ide);  

    /****************************************************************
     * DUP notes
     ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupNoteInDb>> GetAllDupNotesAsync(int dp_id);
    Task<DupNoteInDb?> GetDupNoteAsync(int id); 
    // Update data
    Task<DupNoteInDb?> CreateDupNoteAsync(DupNoteInDb dupNoteContent);
    Task<DupNoteInDb?> UpdateDupNoteAsync(DupNoteInDb dupNoteContent);
    Task<int> DeleteDupNoteAsync(int id); 

    /****************************************************************
    * DUP people
    ****************************************************************/

    // Fetch data 
    Task<IEnumerable<DupPersonInDb>> GetAllDupPeopleAsync(int dp_id);
    Task<DupPersonInDb?> GetDupPersonAsync(int id); 
    // Update data
    Task<DupPersonInDb?> CreateDupPersonAsync(DupPersonInDb dupPeopleContent);
    Task<DupPersonInDb?> UpdateDupPersonAsync(DupPersonInDb dupPeopleContent);
    Task<int> DeleteDupPersonAsync(int id);  
    
    
    /****************************************************************
    *Secondary use
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<SecondaryUseInDb>> GetAllSecUsesAsync(int dup_id);
    Task<SecondaryUseInDb?> GetSecUseAsync(int dup_id); 
    // Update data
    Task<SecondaryUseInDb?> CreateSecUseAsync(SecondaryUseInDb dtaContent);
    Task<SecondaryUseInDb?> UpdateSecUseAsync(SecondaryUseInDb dtaContent);
    Task<int> DeleteSecUseAsync(int id); 


    // Statistics
    /*
    Task<PaginationResponse<DupDto>> PaginateDup(PaginationRequest paginationRequest);
    Task<PaginationResponse<DupDto>> FilterDupByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalDup();
    Task<int> GetUncompletedDup();
    */
}
