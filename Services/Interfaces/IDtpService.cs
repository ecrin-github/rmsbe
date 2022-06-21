using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IDtpService
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/

    // Check if DTP exists
    Task<bool> DtpExistsAsync (int id); 
    
    // Check if attribute exists on this DTP
    Task<bool> DtpAttributeExistsAsync (int dtpId, string typeName, int id); 
    
    // Check if DTP / object combination exists
    Task<bool> DtpObjectExistsAsync (int dtpId, string sdOid); 
    
    // Check if DTP pre-requisite or dataset on this DTP / object
    Task<bool> DtpObjectAttributeExistsAsync (int dtpId, string sdOid, string typeName, int id); 
    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dtp>?> GetAllDtpsAsync();
    Task<List<Dtp>?> GetRecentDtpsAsync(int n);   
    Task<List<Dtp>?> GetPaginatedDtpDataAsync(PaginationRequest validFilter);
    Task<List<Dtp>?> GetPaginatedFilteredDtpRecordsAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<Dtp>?> GetFilteredDtpRecordsAsync(string titleFilter);
    
    Task<List<DtpEntry>?> GetDtpEntriesAsync();
    Task<List<DtpEntry>?> GetRecentDtpEntriesAsync(int n);
    Task<List<DtpEntry>?> GetPaginatedDtpEntriesAsync(PaginationRequest validFilter);
    Task<List<DtpEntry>?> GetPaginatedFilteredDtpEntriesAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<DtpEntry>?> GetFilteredDtpEntriesAsync(string titleFilter);
    
    Task<Dtp?> GetDtpAsync(int dtpId); 
    // Update data
    Task<Dtp?> CreateDtpAsync(Dtp dtpContent);
    Task<Dtp?> UpdateDtpAsync(int dtpId,Dtp dtpContent);
    Task<int> DeleteDtpAsync(int dtpId); 
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalDtps();  
    Task<Statistic> GetTotalFilteredDtps(string titleFilter);  
    Task<List<Statistic>> GetDtpsByCompletion();
    Task<List<Statistic>?> GetDtpsByStatus();
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    Task<List<DtpStudy>?> GetAllDtpStudiesAsync(int dtpId);
    Task<DtpStudy?> GetDtpStudyAsync(int dtpId); 
    // Update data
    Task<DtpStudy?> CreateDtpStudyAsync(DtpStudy dtpStudyContent);
    Task<DtpStudy?> UpdateDtpStudyAsync(int id,DtpStudy dtpStudyContent);
    Task<int> DeleteDtpStudyAsync(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DtpObject>?> GetAllDtpObjectsAsync(int dtpId);
    Task<DtpObject?> GetDtpObjectAsync(int dtpId); 
    // Update data
    Task<DtpObject?> CreateDtpObjectAsync(DtpObject dtpObjectContent);
    Task<DtpObject?> UpdateDtpObjectAsync(int id,DtpObject dtpObjectContent);
    Task<int> DeleteDtpObjectAsync(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dta>?> GetAllDtasAsync(int dtpId);
    Task<Dta?> GetDtaAsync(int dtpId); 
    // Update data
    Task<Dta?> CreateDtaAsync(Dta dtaContent);
    Task<Dta?> UpdateDtaAsync(int id,Dta dtaContent);
    Task<int> DeleteDtaAsync(int id); 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    Task<DtpDataset?> GetDtpDatasetAsync(int id); 
    // Update data
    Task<DtpDataset?> CreateDtpDatasetAsync(DtpDataset dtpDatasetContent);
    Task<DtpDataset?> UpdateDtpDatasetAsync(int id,DtpDataset dtpDatasetContent);
    Task<int> DeleteDtpDatasetAsync(int id); 

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    // Fetch data
    Task<List<DtpPrereq>?> GetAllDtpPrereqsAsync(int dtpId, string sdOid);
    Task<DtpPrereq?> GetDtpPrereqAsync(int id); 
    // Update data
    Task<DtpPrereq?> CreateDtpPrereqAsync(DtpPrereq dtpPrereqContent);
    Task<DtpPrereq?> UpdateDtpPrereqAsync(int id, DtpPrereq dtpPrereqContent);
    Task<int> DeleteDtpPrereqAsync(int id); 
    
   /****************************************************************
    * DTP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DtpNote>?> GetAllDtpNotesAsync(int dpId);
    Task<DtpNote?> GetDtpNoteAsync(int id); 
    // Update data
    Task<DtpNote?> CreateDtpNoteAsync(DtpNote procNoteContent);
    Task<DtpNote?> UpdateDtpNoteAsync(int id, DtpNote procNoteContent);
    Task<int> DeleteDtpNoteAsync(int id); 

    /****************************************************************
    * DTP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DtpPerson>?> GetAllDtpPeopleAsync(int dpId);
    Task<DtpPerson?> GetDtpPersonAsync(int id); 
    // Update data
    Task<DtpPerson?> CreateDtpPersonAsync(DtpPerson procPeopleContent);
    Task<DtpPerson?> UpdateDtpPersonAsync(int id, DtpPerson procPeopleContent);
    Task<int> DeleteDtpPersonAsync(int id); 
    
}