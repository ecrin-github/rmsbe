using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDtpRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> DtpExistsAsync(int id);
    Task<bool> DtpAttributeExistsAsync(int dtp_id, string type_name, int id);
    Task<bool> DtpObjectExistsAsync(int dtp_id, string sd_oid);
    Task<bool> DtpObjectAttributeExistsAsync(int dtp_id, string sd_oid, string type_name, int id);
    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtpInDb>> GetAllDtpsAsync();
    Task<IEnumerable<DtpInDb>> GetRecentDtpsAsync(int n);   
    Task<DtpInDb?> GetDtpAsync(int dtp_id); 
    // Update data
    Task<DtpInDb?> CreateDtpAsync(DtpInDb dtpContent);
    Task<DtpInDb?> UpdateDtpAsync(DtpInDb dtpContent);
    Task<int> DeleteDtpAsync(int dtp_id); 
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudiesAsync(int dtp_id);
    Task<DtpStudyInDb?> GetDtpStudyAsync(int id); 
    // Update data
    Task<DtpStudyInDb?> CreateDtpStudyAsync(DtpStudyInDb dtpStudyContent);
    Task<DtpStudyInDb?> UpdateDtpStudyAsync(DtpStudyInDb dtpStudyContent);
    Task<int> DeleteDtpStudyAsync(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjectsAsync(int dtp_id);
    Task<DtpObjectInDb?> GetDtpObjectAsync(int id); 
    // Update data
    Task<DtpObjectInDb?> CreateDtpObjectAsync(DtpObjectInDb dtpObjectContent);
    Task<DtpObjectInDb?> UpdateDtpObjectAsync(DtpObjectInDb dtpObjectContent);
    Task<int> DeleteDtpObjectAsync(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtaInDb>> GetAllDtasAsync(int dtp_id);
    Task<DtaInDb?> GetDtaAsync(int dtp_id); 
    // Update data
    Task<DtaInDb?> CreateDtaAsync(DtaInDb dtaContent);
    Task<DtaInDb?> UpdateDtaAsync(DtaInDb dtaContent);
    Task<int> DeleteDtaAsync(int id); 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    Task<DtpDatasetInDb?> GetDtpDatasetAsync(int id); 
    // Update data
    Task<DtpDatasetInDb?> CreateDtpDatasetAsync(DtpDatasetInDb dtpDatasetContent);
    Task<DtpDatasetInDb?> UpdateDtpDatasetAsync(DtpDatasetInDb dtpDatasetContent);
    Task<int> DeleteDtpDatasetAsync(int id);  

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    // Fetch data
    Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqsAsync(int dtp_id, string sd_oid);
    Task<DtpPrereqInDb?> GetDtpPrereqAsync(int id); 
    // Update data
    Task<DtpPrereqInDb?> CreateDtpPrereqAsync(DtpPrereqInDb dtpPrereqContent);
    Task<DtpPrereqInDb?> UpdateDtpPrereqAsync(DtpPrereqInDb dtpPrereqContent);
    Task<int> DeleteDtpPrereqAsync(int ide);  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotesAsync(int dp_id);
    Task<DtpNoteInDb?> GetDtpNoteAsync(int id); 
    // Update data
    Task<DtpNoteInDb?> CreateDtpNoteAsync(DtpNoteInDb dtpNoteContent);
    Task<DtpNoteInDb?> UpdateDtpNoteAsync(DtpNoteInDb dtpNoteContent);
    Task<int> DeleteDtpNoteAsync(int id); 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeopleAsync(int dp_id);
    Task<DtpPersonInDb?> GetDtpPersonAsync(int id); 
    // Update data
    Task<DtpPersonInDb?> CreateDtpPersonAsync(DtpPersonInDb dtpPeopleContent);
    Task<DtpPersonInDb?> UpdateDtpPersonAsync(DtpPersonInDb dtpPeopleContent);
    Task<int> DeleteDtpPersonAsync(int id);  
    
    /****************************************************************
    * Dtp statistics
    ****************************************************************/

    Task<int> GetTotalDtps();
    Task<int> GetCompletedDtps();
    Task<IEnumerable<StatisticInDb>> GetDtpsByStatus();
    
    /*
    // Statistics
    Task<PaginationResponse<DtpDto>> PaginateDtp(PaginationRequest paginationRequest);
    Task<PaginationResponse<DtpDto>> FilterDtpByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalDtp();
    Task<int> GetUncompletedDtp();
    */
    
}
