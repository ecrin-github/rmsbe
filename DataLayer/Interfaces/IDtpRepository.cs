using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDtpRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> DtpExists(int id);
    Task<bool> DtpAttributeExists(int dtpId, string typeName, int id);
    Task<bool> DtpObjectExists(int dtpId, string sdOid);
    Task<bool> DtpObjectAttributeExists(int dtpId, string sdOid, string typeName, int id);
    
    /****************************************************************
    * Fetch DTP / DTP entry data
    ****************************************************************/

    Task<IEnumerable<DtpInDb>> GetAllDtps();
    Task<IEnumerable<DtpEntryInDb>> GetAllDtpEntries();
    
    Task<IEnumerable<DtpInDb>> GetPaginatedDtpData(int pNum, int pSize);
    Task<IEnumerable<DtpEntryInDb>> GetPaginatedDtpEntries(int pNum, int pSize);
    
    Task<IEnumerable<DtpInDb>> GetFilteredDtpData(string titleFilter);
    Task<IEnumerable<DtpEntryInDb>> GetFilteredDtpEntries(string titleFilter);
    
    Task<IEnumerable<DtpInDb>> GetPaginatedFilteredDtpData(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DtpEntryInDb>> GetPaginatedFilteredDtpEntries(string titleFilter, int pNum, int pSize);
    
    Task<IEnumerable<DtpInDb>> GetRecentDtps(int n);  
    Task<IEnumerable<DtpEntryInDb>> GetRecentDtpEntries(int n);
    
    Task<IEnumerable<DtpInDb>> GetDtpsByOrg(int orgId);  
    Task<IEnumerable<DtpEntryInDb>> GetDtpEntriesByOrg(int orgId);
    
    Task<DtpInDb?> GetDtp(int dtpId); 
    
    /****************************************************************
    * Update DTP data
    ****************************************************************/
    
    Task<DtpInDb?> CreateDtp(DtpInDb dtpContent);
    Task<DtpInDb?> UpdateDtp(DtpInDb dtpContent);
    Task<int> DeleteDtp(int dtpId); 
    
    /****************************************************************
    * Full DTP data (including attributes in other tables)
    ****************************************************************/
  
    Task<FullDtpInDb?> GetFullDtpById(int id);
    Task<int> DeleteFullDtp(int id);
    
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
    Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudies(int dtpId);
    Task<DtpStudyInDb?> GetDtpStudy(int id); 
    
    // Update data
    Task<DtpStudyInDb?> CreateDtpStudy(DtpStudyInDb dtpStudyContent);
    Task<DtpStudyInDb?> UpdateDtpStudy(DtpStudyInDb dtpStudyContent);
    Task<int> DeleteDtpStudy(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjects(int dtpId);
    Task<DtpObjectInDb?> GetDtpObject(int id); 
    
    // Update data
    Task<DtpObjectInDb?> CreateDtpObject(DtpObjectInDb dtpObjectContent);
    Task<DtpObjectInDb?> UpdateDtpObject(DtpObjectInDb dtpObjectContent);
    Task<int> DeleteDtpObject(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtaInDb>> GetAllDtas(int dtpId);
    Task<DtaInDb?> GetDta(int dtpId); 
    
    // Update data
    Task<DtaInDb?> CreateDta(DtaInDb dtaContent);
    Task<DtaInDb?> UpdateDta(DtaInDb dtaContent);
    Task<int> DeleteDta(int id); 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    Task<DtpDatasetInDb?> GetDtpDataset(int id); 
    
    // Update data
    Task<DtpDatasetInDb?> CreateDtpDataset(DtpDatasetInDb dtpDatasetContent);
    Task<DtpDatasetInDb?> UpdateDtpDataset(DtpDatasetInDb dtpDatasetContent);
    Task<int> DeleteDtpDataset(int id);  

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqs(int dtpId, string sdOid);
    Task<DtpPrereqInDb?> GetDtpPrereq(int id); 
    
    // Update data
    Task<DtpPrereqInDb?> CreateDtpPrereq(DtpPrereqInDb dtpPrereqContent);
    Task<DtpPrereqInDb?> UpdateDtpPrereq(DtpPrereqInDb dtpPrereqContent);
    Task<int> DeleteDtpPrereq(int ide);  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotes(int dpId);
    Task<DtpNoteInDb?> GetDtpNote(int id); 
    
    // Update data
    Task<DtpNoteInDb?> CreateDtpNote(DtpNoteInDb dtpNoteContent);
    Task<DtpNoteInDb?> UpdateDtpNote(DtpNoteInDb dtpNoteContent);
    Task<int> DeleteDtpNote(int id); 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeople(int dpId);
    Task<DtpPersonInDb?> GetDtpPerson(int id); 
    
    // Update data
    Task<DtpPersonInDb?> CreateDtpPerson(DtpPersonInDb dtpPeopleContent);
    Task<DtpPersonInDb?> UpdateDtpPerson(DtpPersonInDb dtpPeopleContent);
    Task<int> DeleteDtpPerson(int id);  
    
}
