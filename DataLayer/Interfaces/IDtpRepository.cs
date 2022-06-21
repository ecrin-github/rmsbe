using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDtpRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> DtpExistsAsync(int id);
    Task<bool> DtpAttributeExistsAsync(int dtpId, string typeName, int id);
    Task<bool> DtpObjectExistsAsync(int dtpId, string sdOid);
    Task<bool> DtpObjectAttributeExistsAsync(int dtpId, string sdOid, string typeName, int id);
    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtpInDb>> GetAllDtpsAsync();
    Task<IEnumerable<DtpInDb>> GetRecentDtpsAsync(int n);  
    Task<IEnumerable<DtpInDb>> GetPaginatedDtpDataAsync(int pNum, int pSize);
    Task<IEnumerable<DtpInDb>> GetPaginatedFilteredDtpDataAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DtpInDb>> GetFilteredDtpDataAsync(string titleFilter);
    
    Task<IEnumerable<DtpEntryInDb>> GetDtpEntriesAsync();
    Task<IEnumerable<DtpEntryInDb>> GetRecentDtpEntriesAsync(int n);
    Task<IEnumerable<DtpEntryInDb>> GetPaginatedDtpEntriesAsync(int pNum, int pSize);
    Task<IEnumerable<DtpEntryInDb>> GetPaginatedFilteredDtpEntriesAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DtpEntryInDb>> GetFilteredDtpEntriesAsync(string titleFilter);
    
    Task<DtpInDb?> GetDtpAsync(int dtpId); 
    
    // Update data
    Task<DtpInDb?> CreateDtpAsync(DtpInDb dtpContent);
    Task<DtpInDb?> UpdateDtpAsync(DtpInDb dtpContent);
    Task<int> DeleteDtpAsync(int dtpId); 
    
    /****************************************************************
    * Dtp statistics
    ****************************************************************/

    Task<int> GetTotalDtps();
    Task<int> GetTotalFilteredDtps(string titleFilter);
    Task<int> GetCompletedDtps();
    Task<IEnumerable<StatisticInDb>> GetDtpsByStatus();
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudiesAsync(int dtpId);
    Task<DtpStudyInDb?> GetDtpStudyAsync(int id); 
    // Update data
    Task<DtpStudyInDb?> CreateDtpStudyAsync(DtpStudyInDb dtpStudyContent);
    Task<DtpStudyInDb?> UpdateDtpStudyAsync(DtpStudyInDb dtpStudyContent);
    Task<int> DeleteDtpStudyAsync(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjectsAsync(int dtpId);
    Task<DtpObjectInDb?> GetDtpObjectAsync(int id); 
    // Update data
    Task<DtpObjectInDb?> CreateDtpObjectAsync(DtpObjectInDb dtpObjectContent);
    Task<DtpObjectInDb?> UpdateDtpObjectAsync(DtpObjectInDb dtpObjectContent);
    Task<int> DeleteDtpObjectAsync(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtaInDb>> GetAllDtasAsync(int dtpId);
    Task<DtaInDb?> GetDtaAsync(int dtpId); 
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
    Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqsAsync(int dtpId, string sdOid);
    Task<DtpPrereqInDb?> GetDtpPrereqAsync(int id); 
    // Update data
    Task<DtpPrereqInDb?> CreateDtpPrereqAsync(DtpPrereqInDb dtpPrereqContent);
    Task<DtpPrereqInDb?> UpdateDtpPrereqAsync(DtpPrereqInDb dtpPrereqContent);
    Task<int> DeleteDtpPrereqAsync(int ide);  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotesAsync(int dpId);
    Task<DtpNoteInDb?> GetDtpNoteAsync(int id); 
    // Update data
    Task<DtpNoteInDb?> CreateDtpNoteAsync(DtpNoteInDb dtpNoteContent);
    Task<DtpNoteInDb?> UpdateDtpNoteAsync(DtpNoteInDb dtpNoteContent);
    Task<int> DeleteDtpNoteAsync(int id); 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeopleAsync(int dpId);
    Task<DtpPersonInDb?> GetDtpPersonAsync(int id); 
    // Update data
    Task<DtpPersonInDb?> CreateDtpPersonAsync(DtpPersonInDb dtpPeopleContent);
    Task<DtpPersonInDb?> UpdateDtpPersonAsync(DtpPersonInDb dtpPeopleContent);
    Task<int> DeleteDtpPersonAsync(int id);  
    
}
