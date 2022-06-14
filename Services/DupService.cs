using rmsbe.SysModels;
using rmsbe.DataLayer.Interfaces;


namespace rmsbe.Services.Interfaces;

public class DupService
{
    private readonly IDupRepository _dupRepository;
    private string _user_name;

    public DupService(IDupRepository dupRepository)
    {
        _dupRepository = dupRepository ?? throw new ArgumentNullException(nameof(dupRepository));

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
    
    // Check if DUP exists
    public async Task<bool> DupDoesNotExistAsync(int id)
        => await _dupRepository.DupDoesNotExistAsync(id);
    
    // Check if attribute exists on this DUP
    public async Task<bool> DupAttributeDoesNotExistAsync(int dup_id, string type_name, int id)
        => await _dupRepository.DupAttributeDoesNotExistAsync(dup_id, type_name, id);

    // Check if DUP / object combination exists
    public async Task<bool> DupObjectDoesNotExistAsync(int dup_id, string sd_oid)
        => await _dupRepository.DupObjectDoesNotExistAsync(dup_id, sd_oid);

    // Check if pre-req exists on this DUP / object
    public async Task<bool> DupAttributePrereqDoesNotExistAsync (int dup_id, string sd_oid, int id)
        => await _dupRepository.DupAttributePrereqDoesNotExistAsync(dup_id, sd_oid, id);
    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dup>?> GetAllDupsAsync()
    {
        return null;
    }

    public async Task<List<Dup>?> GetRecentDupsAsync(int n)
    {
        return null;
    }
   
    public async Task<Dup?> GetDupAsync(int dup_id)
    {
        return null;
    }
 
    // Update data
    public async Task<Dup?> CreateDupAsync(Dup dupContent)
    {
        return null;
    }

    public async Task<Dup?> UpdateDupAsync(int dup_id,Dup dupContent)
    {
        return null;
    }

    public async Task<int> DeleteDupAsync(int dup_id)
    {
        return 0;
    }
 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DupObject>?> GetAllDupObjectsAsync(int dup_id)
    {
        return null;
    }

    public async Task<DupObject?> GetDupObjectAsync(int dup_id)
    {
        return null;
    }
 
    // Update data
    public async Task<DupObject?> CreateDupObjectAsync(DupObject dupObjectContent)
    {
        return null;
    }

    public async Task<DupObject?> UpdateDupObjectAsync(int id,DupObject dupObjectContent)
    {
        return null;
    }

    public async Task<int> DeleteDupObjectAsync(int id)
    {
        return 0;
    }
 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dua>?> GetAllDuasAsync(int dup_id)
    {
        return null;
    }

    public async Task<Dua?> GetDuaAsync(int dup_id)
    {
        return null;
    }
 
    // Update data
    public async Task<Dua?> CreateDuaAsync(Dua duaContent)
    {
        return null;
    }

    public async Task<Dua?> UpdateDuaAsync(int id,Dua duaContent)
    {
        return null;
    }

    public async Task<int> DeleteDuaAsync(int id)
    {
        return 0;
    }
 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
    // Fetch data
    public async Task<List<DupPrereq>?> GetAllDupPrereqsAsync(int dtp_id, string sd_oid)
    {
        return null;
    }

    public async Task<DupPrereq?> GetDupPrereqAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<DupPrereq?> CreateDupPrereqAsync(DupPrereq dtpPrereqContent)
    {
        return null;
    }

    public async Task<DupPrereq?> UpdateDupPrereqAsync(int id, DupPrereq dtpPrereqContent)
    {
        return null;
    }

    public async Task<int> DeleteDupPrereqAsync(int id)
    {
        return 0;
    }
 

    /****************************************************************
    * Secondary use
    ****************************************************************/

    // Fetch data
    public async Task<List<SecondaryUse>?> GetAllSecondaryUsesAsync(int dup_id)
    {
        return null;
    }

    public async Task<SecondaryUse?> GetSecondaryUseAsync(int dup_id)
    {
        return null;
    }

    // Update data
    public async Task<SecondaryUse?> CreateSecondaryUseAsync(SecondaryUse secUseContent)
    {
        return null;
    }

    public async Task<SecondaryUse?> UpdateSecondaryUseAsync(int id, SecondaryUse secUseContent)
    {
        return null;
    }

    public async Task<int> DeleteSecondaryUseAsync(int id)
    {
        return 0;
    }


    /****************************************************************
    * DUP Process notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DupNote>?> GetAllDupNotesAsync(int dp_id)
    {
        return null;
    }

    public async Task<DupNote?> GetDupNoteAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<DupNote?> CreateDupNoteAsync(DupNote procNoteContent)
    {
        return null;
    }

    public async Task<DupNote?> UpdateDupNoteAsync(int id, DupNote procNoteContent)
    {
        return null;
    }

    public async Task<int> DeleteDupNoteAsync(int id)
    {
        return 0;
    }


    /****************************************************************
    * DUP Process people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DupPerson>?> GetAllDupPeopleAsync(int dp_id)
    {
        return null;
    }

    public async Task<DupPerson?> GetDupPersonAsync(int id)
    {
        return null;
    }
 
    // Update data
    public async Task<DupPerson?> CreateDupPersonAsync(DupPerson procPeopleContent)
    {
        return null;
    }

    public async Task<DupPerson?> UpdateDupPersonAsync(int id, DupPerson procPeopleContent)
    {
        return null;
    }

    public async Task<int> DeleteDupPersonAsync(int id)
    {
        return 0;
    }

}