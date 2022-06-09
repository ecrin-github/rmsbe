using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IRmsService
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
    // Check if dataset exists for this object
    Task<bool> ObjectDatasetDoesNotExistAsync (string sd_oid, int id); 
    // Check if DUP exists
    Task<bool> DupDoesNotExistAsync (int id); 
    // Check if attribute exists on this DUP
    Task<bool> DupAttributeDoesNotExistAsync (int dup_id, string type_name, int id); 
    // Check if DUP / object combination exists
    Task<bool> DupObjectDoesNotExistAsync (int dup_id, string sd_oid); 
    // Check if pre-req exists on this DUP / object
    Task<bool> DupAttributePrereqDoesNotExistAsync (int dup_id, string sd_oid, int id); 
    
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
    * Access pre-requisites
    ****************************************************************/
    // Fetch data
    Task<List<AccessPrereq>?> GetAllDtpAccessPrereqsAsync(int dtp_id);
    Task<AccessPrereq?> GetAccessPrereqAsync(int id); 
    // Update data
    Task<AccessPrereq?> CreateAccessPrereqAsync(AccessPrereq dtpPrereqContent);
    Task<AccessPrereq?> UpdateAccessPrereqAsync(int id, AccessPrereq dtpPrereqContent);
    Task<int> DeleteAccessPrereqAsync(int id); 

    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dup>?> GetAllDupsAsync();
    Task<List<Dup>?> GetRecentDupsAsync(int n);   
    Task<Dup?> GetDupAsync(int dup_id); 
    // Update data
    Task<Dup?> CreateDupAsync(Dup dupContent);
    Task<Dup?> UpdateDupAsync(int dup_id,Dup dupContent);
    Task<int> DeleteDupAsync(int dup_id); 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DupObject>?> GetAllDupObjectsAsync(int dup_id);
    Task<DupObject?> GetDupObjectAsync(int dup_id); 
    // Update data
    Task<DupObject?> CreateDupObjectAsync(DupObject dupObjectContent);
    Task<DupObject?> UpdateDupObjectAsync(int id,DupObject dupObjectContent);
    Task<int> DeleteDupObjectAsync(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dua>?> GetAllDuasAsync(int dup_id);
    Task<Dua?> GetDuaAsync(int dup_id); 
    // Update data
    Task<Dua?> CreateDuaAsync(Dua duaContent);
    Task<Dua?> UpdateDuaAsync(int id,Dua duaContent);
    Task<int> DeleteDuaAsync(int id); 
    
    /****************************************************************
    * Dup pre-requisites met
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
    Task<List<SecondaryUse>?> GetAllSecondaryUsesAsync(int dup_id);
    Task<SecondaryUse?> GetSecondaryUseAsync(int dup_id); 
    // Update data
    Task<SecondaryUse?> CreateSecondaryUseAsync(SecondaryUse secUseContent);
    Task<SecondaryUse?> UpdateSecondaryUseAsync(int id, SecondaryUse secUseContent);
    Task<int> DeleteSecondaryUseAsync(int id); 

   /****************************************************************
    * Process notes
    ****************************************************************/

    // Fetch data
    Task<List<ProcessNote>?> GetAllProcessNotesAsync(int dp_id);
    Task<ProcessNote?> GetProcessNoteAsync(int id); 
    // Update data
    Task<ProcessNote?> CreateProcessNoteAsync(ProcessNote procNoteContent);
    Task<ProcessNote?> UpdateProcessNoteAsync(int id, ProcessNote procNoteContent);
    Task<int> DeleteProcessNoteAsync(int id); 

    /****************************************************************
    * Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<ProcessPeople>?> GetAllProcessPeoplesAsync(int dp_id);
    Task<ProcessPeople?> GetProcessPeopleAsync(int id); 
    // Update data
    Task<ProcessPeople?> CreateDtpObjectAsync(ProcessPeople procPeopleContent);
    Task<ProcessPeople?> UpdateProcessPeopleAsync(int id, ProcessPeople procPeopleContent);
    Task<int> DeleteProcessPeopleAsync(int id); 
}