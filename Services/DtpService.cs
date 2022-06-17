using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class DtpService : IDtpService
{
    private readonly IDtpRepository _dtpRepository;

    public DtpService(IDtpRepository dtpRepository)
    {
        _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    ****************************************************************/
    
    // Check if DTP exists
    public async Task<bool> DtpDoesNotExistAsync (int id) 
           => await _dtpRepository.DtpDoesNotExistAsync(id);
    public async Task<bool> DtpExistsAsync (int id) 
           => await _dtpRepository.DtpExistsAsync(id);
    
    // Check if attribute exists on this DTP
    public async Task<bool> DtpAttributeDoesNotExistAsync(int dtp_id, string type_name, int id)
           => await _dtpRepository.DtpAttributeDoesNotExistAsync(dtp_id, type_name, id);
    public async Task<bool> DtpAttributeExistsAsync(int dtp_id, string type_name, int id)
           => await _dtpRepository.DtpAttributeExistsAsync(dtp_id, type_name, id);

    // Check if DTP / object combination exists
    public async Task<bool> DtpObjectDoesNotExistAsync(int dtp_id, string sd_oid)
           => await _dtpRepository.DtpObjectDoesNotExistAsync(dtp_id, sd_oid);
    public async Task<bool> DtpObjectExistsAsync(int dtp_id, string sd_oid)
           => await _dtpRepository.DtpObjectExistsAsync(dtp_id, sd_oid);
   
    // Check if pre-req exists on this DTP / object
    public async Task<bool> PrereqDoesNotExistAsync (int dtp_id, string sd_oid, int id)
           => await _dtpRepository.PrereqDoesNotExistAsync(dtp_id, sd_oid, id);
    public async Task<bool> DtpPrereqExistsAsync (int dtp_id, string sd_oid, int id) 
           => await _dtpRepository.DtpPrereqExistsAsync(dtp_id, sd_oid, id); 
    
    // Check if dataset exists for this object
    public async Task<bool> ObjectDatasetDoesNotExistAsync (string sd_oid, int id)
           => await _dtpRepository.ObjectDatasetDoesNotExistAsync(sd_oid, id);
    public async Task<bool> DtpObjectDatasetExistsAsync (string sd_oid, int id) 
           => await _dtpRepository.DtpObjectDatasetExistsAsync(sd_oid, id);
    
    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<Dtp>?> GetAllDtpsAsync() {
        var dtpsInDb = (await _dtpRepository.GetAllDtpsAsync()).ToList();
        return (!dtpsInDb.Any()) ? null 
            : dtpsInDb.Select(r => new Dtp(r)).ToList();
    }

    public async Task<List<Dtp>?> GetRecentDtpsAsync(int n) {
        var dtpsInDb = (await _dtpRepository.GetRecentDtpsAsync(n)).ToList();
        return (!dtpsInDb.Any()) ? null 
            : dtpsInDb.Select(r => new Dtp(r)).ToList();
    }
   
    public async Task<Dtp?> GetDtpAsync(int dtp_id) {
        var dtpInDb = await _dtpRepository.GetDtpAsync(dtp_id);
        return dtpInDb == null ? null : new Dtp(dtpInDb);
    }
 
    // Update data
    
    public async Task<Dtp?> CreateDtpAsync(Dtp dtpContent) {
        var dtpInDb = new DtpInDb(dtpContent);
        var res = await _dtpRepository.CreateDtpAsync(dtpInDb);
        return res == null ? null : new Dtp(res);
    }

    public async Task<Dtp?> UpdateDtpAsync(int aId, Dtp dtpContent) {
        var dtpInDb = new DtpInDb(dtpContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpAsync(dtpInDb);
        return res == null ? null : new Dtp(res);
    }

    public async Task<int> DeleteDtpAsync(int dtp_id)
           => await _dtpRepository.DeleteDtpAsync(dtp_id);
    

    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpStudy>?> GetAllDtpStudiesAsync(int dtp_id) {
        var dtpStudiesInDb = (await _dtpRepository.GetAllDtpStudiesAsync(dtp_id)).ToList();
        return (!dtpStudiesInDb.Any()) ? null 
            : dtpStudiesInDb.Select(r => new DtpStudy(r)).ToList();
    }

    public async Task<DtpStudy?> GetDtpStudyAsync(int id) {
        var dtpStudyInDb = await _dtpRepository.GetDtpStudyAsync(id);
        return dtpStudyInDb == null ? null : new DtpStudy(dtpStudyInDb);
    }
 
    // Update data
    public async Task<DtpStudy?> CreateDtpStudyAsync(DtpStudy dtpStudyContent) {
        var dtpStudyInDb = new DtpStudyInDb(dtpStudyContent);
        var res = await _dtpRepository.CreateDtpStudyAsync(dtpStudyInDb);
        return res == null ? null : new DtpStudy(res);
    }

    public async Task<DtpStudy?> UpdateDtpStudyAsync(int aId, DtpStudy dtpStudyContent) {
        var dtpStudyContentInDb = new DtpStudyInDb(dtpStudyContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpStudyAsync(dtpStudyContentInDb);
        return res == null ? null : new DtpStudy(res);
    }

    public async Task<int> DeleteDtpStudyAsync(int id)
           => await _dtpRepository.DeleteDtpStudyAsync(id);

    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpObject>?> GetAllDtpObjectsAsync(int dtp_id) {
        var dtpObjectsInDb = (await _dtpRepository.GetAllDtpObjectsAsync(dtp_id)).ToList();
        return (!dtpObjectsInDb.Any()) ? null 
            : dtpObjectsInDb.Select(r => new DtpObject(r)).ToList();
    }

    public async Task<DtpObject?> GetDtpObjectAsync(int id) {
        var dtpObjectInDb = await _dtpRepository.GetDtpObjectAsync(id);
        return dtpObjectInDb == null ? null : new DtpObject(dtpObjectInDb);
    }
 
    // Update data
    public async Task<DtpObject?> CreateDtpObjectAsync(DtpObject dtpObjectContent) {
        var dtpObjectInDb = new DtpObjectInDb(dtpObjectContent);
        var res = await _dtpRepository.CreateDtpObjectAsync(dtpObjectInDb);
        return res == null ? null : new DtpObject(res);
    }

    public async Task<DtpObject?> UpdateDtpObjectAsync(int aId,DtpObject dtpObjectContent)
    {
        var dtpObjectContentInDb = new DtpObjectInDb(dtpObjectContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpObjectAsync(dtpObjectContentInDb);
        return res == null ? null : new DtpObject(res);
    }

    public async Task<int> DeleteDtpObjectAsync(int id)
           => await _dtpRepository.DeleteDtpObjectAsync(id);
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dta>?> GetAllDtasAsync(int dtp_id) {
        var dtasInDb = (await _dtpRepository.GetAllDtasAsync(dtp_id)).ToList();
        return (!dtasInDb.Any()) ? null 
            : dtasInDb.Select(r => new Dta(r)).ToList();
    }

    public async Task<Dta?> GetDtaAsync(int id) {
        var dtaInDb = await _dtpRepository.GetDtaAsync(id);
        return dtaInDb == null ? null : new Dta(dtaInDb);
    }
 
    // Update data
    public async Task<Dta?> CreateDtaAsync(Dta dtaContent) {
        var dtaInDb = new DtaInDb(dtaContent);
        var res = await _dtpRepository.CreateDtaAsync(dtaInDb);
        return res == null ? null : new Dta(res);
    }

    public async Task<Dta?> UpdateDtaAsync(int aId,Dta dtaContent) {
        var dtaInDb = new DtaInDb(dtaContent) { id = aId };
        var res = await _dtpRepository.UpdateDtaAsync(dtaInDb);
        return res == null ? null : new Dta(res);
    }

    public async Task<int> DeleteDtaAsync(int id)
           => await _dtpRepository.DeleteDtaAsync(id);
    
    /***********************************************************
    * DTP datasets
    ****************************************************************/
    
    // Fetch data

    public async Task<DtpDataset?> GetDtpDatasetAsync(int id) {
        var dtpDatasetInDb = await _dtpRepository.GetDtpDatasetAsync(id);
        return dtpDatasetInDb == null ? null : new DtpDataset(dtpDatasetInDb);
    }
 
    // Update data
    public async Task<DtpDataset?> CreateDtpDatasetAsync(DtpDataset dtpDatasetContent) {
        var dtpDatasetInDb = new DtpDatasetInDb(dtpDatasetContent);
        var res = await _dtpRepository.CreateDtpDatasetAsync(dtpDatasetInDb);
        return res == null ? null : new DtpDataset(res);
    }

    public async Task<DtpDataset?> UpdateDtpDatasetAsync(int aId, DtpDataset dtpDatasetContent) {
        var dtpDatasetContentInDb = new DtpDatasetInDb(dtpDatasetContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpDatasetAsync(dtpDatasetContentInDb);
        return res == null ? null : new DtpDataset(res);
    }

    public async Task<int> DeleteDtpDatasetAsync(int id)
           => await _dtpRepository.DeleteDtpDatasetAsync(id);
    
    /****************************************************************
    * DTP pre-requisites met
    ****************************************************************/
    
    // Fetch data
    public async Task<List<DtpPrereq>?> GetAllDtpPrereqsAsync(int dtp_id, string sd_oid) {
        var dtpPrereqsInDb = (await _dtpRepository.GetAllDtpPrereqsAsync(dtp_id, sd_oid)).ToList();
        return (!dtpPrereqsInDb.Any()) ? null 
            : dtpPrereqsInDb.Select(r => new DtpPrereq(r)).ToList();
    }

    public async Task<DtpPrereq?> GetDtpPrereqAsync(int id) {
        var dtpPrereqInDb = await _dtpRepository.GetDtpPrereqAsync(id);
        return dtpPrereqInDb == null ? null : new DtpPrereq(dtpPrereqInDb);
    }
 
    // Update data
    public async Task<DtpPrereq?> CreateDtpPrereqAsync(DtpPrereq dtpPrereqContent) {
        var dtpPrereqInDb = new DtpPrereqInDb(dtpPrereqContent);
        var res = await _dtpRepository.CreateDtpPrereqAsync(dtpPrereqInDb);
        return res == null ? null : new DtpPrereq(res);
    }

    public async Task<DtpPrereq?> UpdateDtpPrereqAsync(int aId, DtpPrereq dtpPrereqContent) {
        var dtpPrereqInDb = new DtpPrereqInDb(dtpPrereqContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpPrereqAsync(dtpPrereqInDb);
        return res == null ? null : new DtpPrereq(res);
    }

    public async Task<int> DeleteDtpPrereqAsync(int id)
        => await _dtpRepository.DeleteDtpPrereqAsync(id);


    /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpNote>?> GetAllDtpNotesAsync(int dtp_id) {
        var dtpNotesInDb = (await _dtpRepository.GetAllDtpNotesAsync(dtp_id)).ToList();
        return (!dtpNotesInDb.Any()) ? null 
            : dtpNotesInDb.Select(r => new DtpNote(r)).ToList();
    }

    public async Task<DtpNote?> GetDtpNoteAsync(int id) {
        var dtpNoteInDb = await _dtpRepository.GetDtpNoteAsync(id);
        return dtpNoteInDb == null ? null : new DtpNote(dtpNoteInDb);
    }
 
    // Update data
    public async Task<DtpNote?> CreateDtpNoteAsync(DtpNote dtpNoteContent) {
        var dtpNoteInDb = new DtpNoteInDb(dtpNoteContent);
        var res = await _dtpRepository.CreateDtpNoteAsync(dtpNoteInDb);
        return res == null ? null : new DtpNote(res);
    }

    public async Task<DtpNote?> UpdateDtpNoteAsync(int aId, DtpNote dtpNoteContent) {
        var dtpNoteContentInDb = new DtpNoteInDb(dtpNoteContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpNoteAsync(dtpNoteContentInDb);
        return res == null ? null : new DtpNote(res);
    }

    public async Task<int> DeleteDtpNoteAsync(int id)
        => await _dtpRepository.DeleteDtpNoteAsync(id);


    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DtpPerson>?> GetAllDtpPeopleAsync(int dtp_id) {
        var dtpPeopleInDb = (await _dtpRepository.GetAllDtpPeopleAsync(dtp_id)).ToList();
        return (!dtpPeopleInDb.Any()) ? null 
            : dtpPeopleInDb.Select(r => new DtpPerson(r)).ToList();
    }

    public async Task<DtpPerson?> GetDtpPersonAsync(int id) {
        var dtpPersonInDb = await _dtpRepository.GetDtpPersonAsync(id);
        return dtpPersonInDb == null ? null : new DtpPerson(dtpPersonInDb);
    }
 
    // Update data
    public async Task<DtpPerson?> CreateDtpPersonAsync(DtpPerson dtpPersonContent) {
        var dtpPersonInDb = new DtpPersonInDb(dtpPersonContent);
        var res = await _dtpRepository.CreateDtpPersonAsync(dtpPersonInDb);
        return res == null ? null : new DtpPerson(res);
    }

    public async Task<DtpPerson?> UpdateDtpPersonAsync(int aId, DtpPerson dtpPersonContent) {
        var dtpPersonInDb = new DtpPersonInDb(dtpPersonContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpPersonAsync(dtpPersonInDb);
        return res == null ? null : new DtpPerson(res);
    }

    public async Task<int> DeleteDtpPersonAsync(int id)
        => await _dtpRepository.DeleteDtpPersonAsync(id);

}