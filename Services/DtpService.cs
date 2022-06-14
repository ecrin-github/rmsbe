using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class DtpService : IDtpService
{
    private readonly IDtpRepository _dtpRepository;
    private string _user_name;

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
    
    // Check if attribute exists on this DTP
    public async Task<bool> DtpAttributeDoesNotExistAsync(int dtp_id, string type_name, int id)
        => await _dtpRepository.DtpAttributeDoesNotExistAsync(dtp_id, type_name, id);

    // Check if DTP / object combination exists
    public async Task<bool> DtpObjectDoesNotExistAsync(int dtp_id, string sd_oid)
        => await _dtpRepository.DtpObjectDoesNotExistAsync(dtp_id, sd_oid);

    // Check if pre-req exists on this DTP / object
    public async Task<bool> ObjectDtpPrereqDoesNotExistAsync (int dtp_id, string sd_oid, int id)
        => await _dtpRepository.ObjectDtpPrereqDoesNotExistAsync(dtp_id, sd_oid, id);

    // Check if dataset exists for this object
    public async Task<bool> ObjectDatasetDoesNotExistAsync (string sd_oid, int id)
        => await _dtpRepository.ObjectDatasetDoesNotExistAsync(sd_oid, id);
    
    
    /****************************************************************
    * DTPs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dtp>?> GetAllDtpsAsync()
    {
        var dtpsInDb = (await _dtpRepository.GetAllDtpsAsync()).ToList();
        return (!dtpsInDb.Any()) ? null 
            : dtpsInDb.Select(r => new Dtp(r)).ToList();
    }

    public async Task<List<Dtp>?> GetRecentDtpsAsync(int n)
    {
        var dtpsInDb = (await _dtpRepository.GetRecentDtpsAsync(n)).ToList();
        return (!dtpsInDb.Any()) ? null 
            : dtpsInDb.Select(r => new Dtp(r)).ToList();
    }
   
    public async Task<Dtp?> GetDtpAsync(int dtp_id)
    {
        var dtpInDb = await _dtpRepository.GetDtpAsync(dtp_id);
        return dtpInDb == null ? null : new Dtp(dtpInDb);
    }
 
    // Update data
    public async Task<Dtp?> CreateDtpAsync(Dtp dtpContent)
    {
        var dtpInDb = new DtpInDb(dtpContent);
        var res = await _dtpRepository.CreateDtpAsync(dtpInDb);
        return res == null ? null : new Dtp(res);
    }

    public async Task<Dtp?> UpdateDtpAsync(int dtp_id,Dtp dtpContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpAsync(int dtp_id)
        => await _dtpRepository.DeleteDtpAsync(dtp_id);

    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpStudy>?> GetAllDtpStudiesAsync(int dtp_id)
    {
        var dtpStudiesInDb = (await _dtpRepository.GetAllDtpStudiesAsync(dtp_id)).ToList();
        return (!dtpStudiesInDb.Any()) ? null 
            : dtpStudiesInDb.Select(r => new DtpStudy(r)).ToList();
    }

    public async Task<DtpStudy?> GetDtpStudyAsync(int id)
    {
        var dtpStudyInDb = await _dtpRepository.GetDtpStudyAsync(id);
        return dtpStudyInDb == null ? null : new DtpStudy(dtpStudyInDb);
    }
 
    // Update data
    public async Task<DtpStudy?> CreateDtpStudyAsync(DtpStudy dtpStudyContent)
    {
        var dtpStudyInDb = new DtpStudyInDb(dtpStudyContent);
        var res = await _dtpRepository.CreateDtpStudyAsync(dtpStudyInDb);
        return res == null ? null : new DtpStudy(res);
    }

    public async Task<DtpStudy?> UpdateDtpStudyAsync(int id,DtpStudy dtpObjectContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpStudyAsync(int id)
        => await _dtpRepository.DeleteDtpStudyAsync(id);

    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpObject>?> GetAllDtpObjectsAsync(int dtp_id)
    {
        var dtpObjectsInDb = (await _dtpRepository.GetAllDtpObjectsAsync(dtp_id)).ToList();
        return (!dtpObjectsInDb.Any()) ? null 
            : dtpObjectsInDb.Select(r => new DtpObject(r)).ToList();
    }

    public async Task<DtpObject?> GetDtpObjectAsync(int id)
    {
        var dtpObjectInDb = await _dtpRepository.GetDtpObjectAsync(id);
        return dtpObjectInDb == null ? null : new DtpObject(dtpObjectInDb);
    }
 
    // Update data
    public async Task<DtpObject?> CreateDtpObjectAsync(DtpObject dtpObjectContent)
    {
        var dtpObjectInDb = new DtpObjectInDb(dtpObjectContent);
        var res = await _dtpRepository.CreateDtpObjectAsync(dtpObjectInDb);
        return res == null ? null : new DtpObject(res);
    }

    public async Task<DtpObject?> UpdateDtpObjectAsync(int id,DtpObject dtpObjectContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpObjectAsync(int id)
        => await _dtpRepository.DeleteDtpObjectAsync(id);
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dta>?> GetAllDtasAsync(int dtp_id)
    {
        var dtasInDb = (await _dtpRepository.GetAllDtasAsync(dtp_id)).ToList();
        return (!dtasInDb.Any()) ? null 
            : dtasInDb.Select(r => new Dta(r)).ToList();
    }

    public async Task<Dta?> GetDtaAsync(int id)
    {
        var dtaInDb = await _dtpRepository.GetDtaAsync(id);
        return dtaInDb == null ? null : new Dta(dtaInDb);
    }
 
    // Update data
    public async Task<Dta?> CreateDtaAsync(Dta dtaContent)
    {
        var dtaInDb = new DtaInDb(dtaContent);
        var res = await _dtpRepository.CreateDtaAsync(dtaInDb);
        return res == null ? null : new Dta(res);
    }

    public async Task<Dta?> UpdateDtaAsync(int id,Dta duaContent)
    {
        return null;
    }

    public async Task<int> DeleteDtaAsync(int id)
        => await _dtpRepository.DeleteDtaAsync(id);
    
    /***********************************************************
    * DTP datasets
    ****************************************************************/
    
    // Fetch data

    public async Task<DtpDataset?> GetDtpDatasetAsync(int id)
    {
        var dtpDatasetInDb = await _dtpRepository.GetDtpDatasetAsync(id);
        return dtpDatasetInDb == null ? null : new DtpDataset(dtpDatasetInDb);
    }
 
    // Update data
    public async Task<DtpDataset?> CreateDtpDatasetAsync(DtpDataset dtpDatasetContent)
    {
        var dtpDatasetInDb = new DtpDatasetInDb(dtpDatasetContent);
        var res = await _dtpRepository.CreateDtpDatasetAsync(dtpDatasetInDb);
        return res == null ? null : new DtpDataset(res);
    }

    public async Task<DtpDataset?> UpdateDtpDatasetAsync(int id, DtpDataset dtpPrereqContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpDatasetAsync(int id)
        => await _dtpRepository.DeleteDtpDatasetAsync(id);
    
    /****************************************************************
    * DTP pre-requisites met
    ****************************************************************/
    
    // Fetch data
    public async Task<List<AccessPrereq>?> GetAllDtpAccessPrereqsAsync(int dtp_id, string sd_oid)
    {
        var accessPrereqsInDb = (await _dtpRepository.GetAllDtpAccessPrereqsAsync(dtp_id, sd_oid)).ToList();
        return (!accessPrereqsInDb.Any()) ? null 
            : accessPrereqsInDb.Select(r => new AccessPrereq(r)).ToList();
    }

    public async Task<AccessPrereq?> GetAccessPrereqAsync(int id)
    {
        var accessPrereqInDb = await _dtpRepository.GetAccessPrereqAsync(id);
        return accessPrereqInDb == null ? null : new AccessPrereq(accessPrereqInDb);
    }
 
    // Update data
    public async Task<AccessPrereq?> CreateAccessPrereqAsync(AccessPrereq dtpPrereqContent)
    {
        var accessPrereqInDb = new AccessPrereqInDb(dtpPrereqContent);
        var res = await _dtpRepository.CreateAccessPrereqAsync(accessPrereqInDb);
        return res == null ? null : new AccessPrereq(res);
    }

    public async Task<AccessPrereq?> UpdateAccessPrereqAsync(int id, AccessPrereq dtpPrereqContent)
    {
        return null;
    }

    public async Task<int> DeleteAccessPrereqAsync(int id)
        => await _dtpRepository.DeleteAccessPrereqAsync(id);


    /****************************************************************
    * DTP Process notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpNote>?> GetAllDtpNotesAsync(int dtp_id)
    {
        var dtpNotesInDb = (await _dtpRepository.GetAllDtpNotesAsync(dtp_id)).ToList();
        return (!dtpNotesInDb.Any()) ? null 
            : dtpNotesInDb.Select(r => new DtpNote(r)).ToList();
    }

    public async Task<DtpNote?> GetDtpNoteAsync(int id)
    {
        var dtpNoteInDb = await _dtpRepository.GetDtpNoteAsync(id);
        return dtpNoteInDb == null ? null : new DtpNote(dtpNoteInDb);
    }
 
    // Update data
    public async Task<DtpNote?> CreateDtpNoteAsync(DtpNote dtpNoteContent)
    {
        var dtpNoteInDb = new DtpNoteInDb(dtpNoteContent);
        var res = await _dtpRepository.CreateDtpNoteAsync(dtpNoteInDb);
        return res == null ? null : new DtpNote(res);
    }

    public async Task<DtpNote?> UpdateDtpNoteAsync(int id, DtpNote procNoteContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpNoteAsync(int id)
        => await _dtpRepository.DeleteDtpNoteAsync(id);


    /****************************************************************
    * DTP Process people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DtpPerson>?> GetAllDtpPeopleAsync(int dtp_id)
    {
        var dtpPeopleInDb = (await _dtpRepository.GetAllDtpPeopleAsync(dtp_id)).ToList();
        return (!dtpPeopleInDb.Any()) ? null 
            : dtpPeopleInDb.Select(r => new DtpPerson(r)).ToList();
    }

    public async Task<DtpPerson?> GetDtpPersonAsync(int id)
    {
        var dtpPersonInDb = await _dtpRepository.GetDtpPersonAsync(id);
        return dtpPersonInDb == null ? null : new DtpPerson(dtpPersonInDb);
    }
 
    // Update data
    public async Task<DtpPerson?> CreateDtpPersonAsync(DtpPerson dtpPersonContent)
    {
        var dtpPersonInDb = new DtpPersonInDb(dtpPersonContent);
        var res = await _dtpRepository.CreateDtpPersonAsync(dtpPersonInDb);
        return res == null ? null : new DtpPerson(res);
    }

    public async Task<DtpPerson?> UpdateDtpPersonAsync(int id, DtpPerson procPeopleContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpPersonAsync(int id)
        => await _dtpRepository.DeleteDtpPersonAsync(id);

}