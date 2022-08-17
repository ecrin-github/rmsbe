using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IDtpService
{
    /****************************************************************
    * Check functions
    ****************************************************************/
    
    Task<bool> DtpExists (int id); 
    Task<bool> DtpAttributeExists (int dtpId, string typeName, int id); 
    Task<bool> DtpObjectExists (int dtpId, string sdOid); 
    Task<bool> DtpObjectAttributeExists (int dtpId, string sdOid, string typeName, int id); 
    
    /****************************************************************
    * Fetch DTP / DTP entry data
    ****************************************************************/
    
    Task<List<Dtp>?> GetAllDtps();
    Task<List<DtpEntry>?> GetAllDtpEntries();
    
    Task<List<Dtp>?> GetPaginatedDtpData(PaginationRequest validFilter);
    Task<List<DtpEntry>?> GetPaginatedDtpEntries(PaginationRequest validFilter);
    
    Task<List<Dtp>?> GetFilteredDtpRecords(string titleFilter);
    Task<List<DtpEntry>?> GetFilteredDtpEntries(string titleFilter);
    
    Task<List<Dtp>?> GetPaginatedFilteredDtpRecords(string titleFilter, PaginationRequest validFilter);
    Task<List<DtpEntry>?> GetPaginatedFilteredDtpEntries(string titleFilter, PaginationRequest validFilter);
    
    Task<List<Dtp>?> GetRecentDtps(int n);   
    Task<List<DtpEntry>?> GetRecentDtpEntries(int n);
    
    Task<List<Dtp>?> GetDtpsByOrg(int orgId);   
    Task<List<DtpEntry>?> GetDtpEntriesByOrg(int orgId);    
    
    Task<Dtp?> GetDtp(int dtpId); 
    
    /****************************************************************
    * Update DTP data
    ****************************************************************/
   
    Task<Dtp?> CreateDtp(Dtp dtpContent);
    Task<Dtp?> UpdateDtp(int dtpId,Dtp dtpContent);
    Task<int> DeleteDtp(int dtpId); 
    
    /****************************************************************
    * Fetch / Delete full DTP data (with attributes)
    ****************************************************************/

    Task<FullDtp?> GetFullDtpById(int id);
    Task<int> DeleteFullDtp(int id);
    
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
    Task<List<DtpStudy>?> GetAllDtpStudies(int dtpId);
    Task<DtpStudy?> GetDtpStudy(int dtpId); 
    
    // Update data
    Task<DtpStudy?> CreateDtpStudy(DtpStudy dtpStudyContent);
    Task<DtpStudy?> UpdateDtpStudy(DtpStudy dtpStudyContent);
    Task<int> DeleteDtpStudy(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<List<DtpObject>?> GetAllDtpObjects(int dtpId);
    Task<DtpObject?> GetDtpObject(int dtpId); 
    
    // Update data
    Task<DtpObject?> CreateDtpObject(DtpObject dtpObjectContent);
    Task<DtpObject?> UpdateDtpObject(DtpObject dtpObjectContent);
    Task<int> DeleteDtpObject(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<List<Dta>?> GetAllDtas(int dtpId);
    Task<Dta?> GetDta(int dtpId); 
    
    // Update data
    Task<Dta?> CreateDta(Dta dtaContent);
    Task<Dta?> UpdateDta(Dta dtaContent);
    Task<int> DeleteDta(int id); 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    Task<DtpDataset?> GetDtpDataset(int id); 
    
    // Update data
    Task<DtpDataset?> CreateDtpDataset(DtpDataset dtpDatasetContent);
    Task<DtpDataset?> UpdateDtpDataset(DtpDataset dtpDatasetContent);
    Task<int> DeleteDtpDataset(int id); 

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
   
    // Fetch data
    Task<List<DtpPrereq>?> GetAllDtpPrereqs(int dtpId, string sdOid);
    Task<DtpPrereq?> GetDtpPrereq(int id); 
    
    // Update data
    Task<DtpPrereq?> CreateDtpPrereq(DtpPrereq dtpPrereqContent);
    Task<DtpPrereq?> UpdateDtpPrereq(DtpPrereq dtpPrereqContent);
    Task<int> DeleteDtpPrereq(int id); 
    
   /****************************************************************
    * DTP Process notes
    ****************************************************************/

    // Fetch data
    Task<List<DtpNote>?> GetAllDtpNotes(int dpId);
    Task<DtpNote?> GetDtpNote(int id); 
    
    // Update data
    Task<DtpNote?> CreateDtpNote(DtpNote procNoteContent);
    Task<DtpNote?> UpdateDtpNote(DtpNote procNoteContent);
    Task<int> DeleteDtpNote(int id); 

    /****************************************************************
    * DTP Process people
    ****************************************************************/
    
    // Fetch data 
    Task<List<DtpPerson>?> GetAllDtpPeople(int dpId);
    Task<DtpPerson?> GetDtpPerson(int id); 
    
    // Update data
    Task<DtpPerson?> CreateDtpPerson(DtpPerson procPeopleContent);
    Task<DtpPerson?> UpdateDtpPerson(DtpPerson procPeopleContent);
    Task<int> DeleteDtpPerson(int id); 
    
}