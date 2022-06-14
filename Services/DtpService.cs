using rmsbe.SysModels;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class DtpService
{
    private readonly IDtpRepository _dtpRepository;
    private string _user_name;

    public DtpService(IDtpRepository dtpRepository)
    {
        _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));

        // for now - need a mechanism to inject this from user object,
        // either directly here or from controller;
        
        DateTime now = DateTime.Now;
        string timestring = now.Hour.ToString() + ":" + now.Minute.ToString() + ":" + now.Second.ToString(); 
        _user_name = "test user" + "_" + timestring; 
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
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
    public async Task<bool> DtpAttributePrereqDoesNotExistAsync (int dtp_id, string sd_oid, int id)
        => await _dtpRepository.DtpAttributePrereqDoesNotExistAsync(dtp_id, sd_oid, id);

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
        return null;
    }
   
    public async Task<Dtp?> GetDtpAsync(int dtp_id)
    {
        return null;
    }
 
    // Update data
    public async Task<Dtp?> CreateDtpAsync(Dtp dtpContent)
    {
        return null;
    }

    public async Task<Dtp?> UpdateDtpAsync(int dtp_id,Dtp dtpContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpAsync(int dtp_id)
    {
        return 0;
    }

    /****************************************************************
   * DTP Studies
   ****************************************************************/

    // Fetch data
    public async Task<List<DtpObject>?> GetAllDtpStudiesAsync(int dtp_id)
    {
        return null;
    }

    public async Task<DtpObject?> GetDtpStudyAsync(int dtp_id)
    {
        return null;
    }
 
    // Update data
    public async Task<DtpObject?> CreateDtpStudyAsync(DtpObject dtpObjectContent)
    {
        return null;
    }

    public async Task<DtpObject?> UpdateDtpStudyAsync(int id,DtpObject dtpObjectContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpStudyAsync(int id)
    {
        return 0;
    }

    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpObject>?> GetAllDtpObjectsAsync(int dtp_id)
    {
        return null;
    }

    public async Task<DtpObject?> GetDtpObjectAsync(int dtp_id)
    {
        return null;
    }
 
    // Update data
    public async Task<DtpObject?> CreateDtpObjectAsync(DtpObject dtpObjectContent)
    {
        return null;
    }

    public async Task<DtpObject?> UpdateDtpObjectAsync(int id,DtpObject dtpObjectContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpObjectAsync(int id)
    {
        return 0;
    }

    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dta>?> GetAllDtasAsync(int dtp_id)
    {
        return null;
    }

    public async Task<Dta?> GetDtaAsync(int dtp_id)
    {
        return null;
    }
 
    // Update data
    public async Task<Dta?> CreateDuaAsync(Dta duaContent)
    {
        return null;
    }

    public async Task<Dta?> UpdateDuaAsync(int id,Dta duaContent)
    {
        return null;
    }

    public async Task<int> DeleteDtaAsync(int id)
    {
        return 0;
    }
    
    /***********************************************************
    * DTP datasets
    ****************************************************************/
    
    // Fetch data

    public async Task<DtpDataset?> GetDtpDatasetAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<DtpDataset?> CreateDtpDatasetAsync(DtpDataset dtpPrereqContent)
    {
        return null;
    }

    public async Task<DtpDataset?> UpdateDtpDatasetAsync(int id, DtpDataset dtpPrereqContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpDatasetAsync(int id)
    {
        return 0;
    }
    
    /****************************************************************
    * DTP pre-requisites met
    ****************************************************************/
    
    // Fetch data
    public async Task<List<AccessPrereq>?> GetAllDtpPrereqsAsync(int dtp_id, string sd_oid)
    {
        return null;
    }

    public async Task<AccessPrereq?> GetDtpPrereqAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<AccessPrereq?> CreateDtpPrereqAsync(AccessPrereq dtpPrereqContent)
    {
        return null;
    }

    public async Task<AccessPrereq?> UpdateDtpPrereqAsync(int id, AccessPrereq dtpPrereqContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpPrereqAsync(int id)
    {
        return 0;
    }


    /****************************************************************
    * DTP Process notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpNote>?> GetAllDtpNotesAsync(int dp_id)
    {
        return null;
    }

    public async Task<DtpNote?> GetDtpNoteAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<DtpNote?> CreateDtpNoteAsync(DtpNote procNoteContent)
    {
        return null;
    }

    public async Task<DtpNote?> UpdateDtpNoteAsync(int id, DtpNote procNoteContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpNoteAsync(int id)
    {
        return 0;
    }


    /****************************************************************
    * DTP Process people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DtpPerson>?> GetAllDtpPeopleAsync(int dp_id)
    {
        return null;
    }

    public async Task<DtpPerson?> GetDtpPersonAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<DtpPerson?> CreateDtpPersonAsync(DtpPerson procPeopleContent)
    {
        return null;
    }

    public async Task<DtpPerson?> UpdateDtpPersonAsync(int id, DtpPerson procPeopleContent)
    {
        return null;
    }

    public async Task<int> DeleteDtpPersonAsync(int id)
    {
        return 0;
    }

}