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
    Task<bool> DtpDoesNotExistAsync (int id); 
    // Check if attribute exists on this DTP
    Task<bool> DtpAttributeDoesNotExistAsync (int dtp_id, string type_name, int id); 
    // Check if DTP / object combination exists
    Task<bool> DtpObjectDoesNotExistAsync (int dtp_id, string sd_oid); 
    // Check if DTP pre-requisite on this DTP / object
    Task<bool> PrereqDoesNotExistAsync (int dtp_id, string sd_oid, int id); 
    
    // Check if dataset exists for this object
    Task<bool> ObjectDatasetDoesNotExistAsync (string sd_oid, int id); 
    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dtp>?> GetAllDtpsAsync();
    Task<List<Dtp>?> GetRecentDtpsAsync(int n);   
    Task<Dtp?> GetDtpAsync(int dtp_id); 
    // Update data
    Task<Dtp?> CreateDtpAsync(Dtp dtpContent);
    Task<Dtp?> UpdateDtpAsync(int dtp_id,Dtp dtpContent);
    Task<int> DeleteDtpAsync(int dtp_id); 
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    Task<List<DtpStudy>?> GetAllDtpStudiesAsync(int dtp_id);
    Task<DtpStudy?> GetDtpStudyAsync(int dtp_id); 
    // Update data
    Task<DtpStudy?> CreateDtpStudyAsync(DtpStudy dtpStudyContent);
    Task<DtpStudy?> UpdateDtpStudyAsync(int id,DtpStudy dtpStudyContent);
    Task<int> DeleteDtpStudyAsync(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DtpObject>?> GetAllDtpObjectsAsync(int dtp_id);
    Task<DtpObject?> GetDtpObjectAsync(int dtp_id); 
    // Update data
    Task<DtpObject?> CreateDtpObjectAsync(DtpObject dtpObjectContent);
    Task<DtpObject?> UpdateDtpObjectAsync(int id,DtpObject dtpObjectContent);
    Task<int> DeleteDtpObjectAsync(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dta>?> GetAllDtasAsync(int dtp_id);
    Task<Dta?> GetDtaAsync(int dtp_id); 
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
    Task<List<DtpPrereq>?> GetAllDtpPrereqsAsync(int dtp_id, string sd_oid);
    Task<DtpPrereq?> GetDtpPrereqAsync(int id); 
    // Update data
    Task<DtpPrereq?> CreateDtpPrereqAsync(DtpPrereq dtpPrereqContent);
    Task<DtpPrereq?> UpdateDtpPrereqAsync(int id, DtpPrereq dtpPrereqContent);
    Task<int> DeleteDtpPrereqAsync(int id); 
    
   /****************************************************************
    * DTP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DtpNote>?> GetAllDtpNotesAsync(int dp_id);
    Task<DtpNote?> GetDtpNoteAsync(int id); 
    // Update data
    Task<DtpNote?> CreateDtpNoteAsync(DtpNote procNoteContent);
    Task<DtpNote?> UpdateDtpNoteAsync(int id, DtpNote procNoteContent);
    Task<int> DeleteDtpNoteAsync(int id); 

    /****************************************************************
    * DTP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DtpPerson>?> GetAllDtpPeopleAsync(int dp_id);
    Task<DtpPerson?> GetDtpPersonAsync(int id); 
    // Update data
    Task<DtpPerson?> CreateDtpPersonAsync(DtpPerson procPeopleContent);
    Task<DtpPerson?> UpdateDtpPersonAsync(int id, DtpPerson procPeopleContent);
    Task<int> DeleteDtpPersonAsync(int id); 
    
}