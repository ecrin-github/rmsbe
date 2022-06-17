using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;


namespace rmsbe.Services.Interfaces;

public class DupService : IDupService
{
    private readonly IDupRepository _dupRepository;

    public DupService(IDupRepository dupRepository)
    {
        _dupRepository = dupRepository ?? throw new ArgumentNullException(nameof(dupRepository));
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/
    
    // Check if DUP exists
    public async Task<bool> DupExistsAsync(int id)
        => await _dupRepository.DupExistsAsync(id);
    
    // Check if attribute exists on this DUP
    public async Task<bool> DupAttributeExistsAsync(int dup_id, string type_name, int id)
        => await _dupRepository.DupAttributeExistsAsync(dup_id, type_name, id);

    // Check if DUP / object combination exists
    public async Task<bool> DupObjectExistsAsync(int dup_id, string sd_oid)
        => await _dupRepository.DupObjectExistsAsync(dup_id, sd_oid);

    // Check if pre-req exists on this DUP / object
    public async Task<bool> DupObjectAttributeExistsAsync (int dup_id, string sd_oid, string type_name, int id)
        => await _dupRepository.DupObjectAttributeExistsAsync(dup_id, sd_oid, type_name, id);
    
    /****************************************************************
    * DUPs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dup>?> GetAllDupsAsync()
    {
        var dupsInDb = (await _dupRepository.GetAllDupsAsync()).ToList();
        return (!dupsInDb.Any()) ? null 
            : dupsInDb.Select(r => new Dup(r)).ToList();
    }

    public async Task<List<Dup>?> GetRecentDupsAsync(int n)
    {
        var dupsInDb = (await _dupRepository.GetRecentDupsAsync(n)).ToList();
        return (!dupsInDb.Any()) ? null 
            : dupsInDb.Select(r => new Dup(r)).ToList();
    }
   
    public async Task<Dup?> GetDupAsync(int dup_id)
    {
        var dupInDb = await _dupRepository.GetDupAsync(dup_id);
        return dupInDb == null ? null : new Dup(dupInDb);
    }
 
    // Update data
    public async Task<Dup?> CreateDupAsync(Dup dupContent)
    {
        var dupInDb = new DupInDb(dupContent);
        var res = await _dupRepository.CreateDupAsync(dupInDb);
        return res == null ? null : new Dup(res);
    }

    public async Task<Dup?> UpdateDupAsync(int dup_id, Dup dupContent)
    {
        var dupContentInDb = new DupInDb(dupContent) { id = dup_id };
        var res = await _dupRepository.UpdateDupAsync(dupContentInDb);
        return res == null ? null : new Dup(res);
    }

    public async Task<int> DeleteDupAsync(int dup_id)
        => await _dupRepository.DeleteDupAsync(dup_id);
 
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    public async Task<List<DupStudy>?> GetAllDupStudiesAsync(int dup_id) {
        var dupStudiesInDb = (await _dupRepository.GetAllDupStudiesAsync(dup_id)).ToList();
        return (!dupStudiesInDb.Any()) ? null 
            : dupStudiesInDb.Select(r => new DupStudy(r)).ToList();
    }

    public async Task<DupStudy?> GetDupStudyAsync(int id)
    {
        var dupStudyInDb = await _dupRepository.GetDupStudyAsync(id);
        return dupStudyInDb == null ? null : new DupStudy(dupStudyInDb);
    }
 
    // Update data
    public async Task<DupStudy?> CreateDupStudyAsync(DupStudy dupStudyContent)
    {
        var dupStudyInDb = new DupStudyInDb(dupStudyContent);
        var res = await _dupRepository.CreateDupStudyAsync(dupStudyInDb);
        return res == null ? null : new DupStudy(res);
    }

    public async Task<DupStudy?> UpdateDupStudyAsync(int aId, DupStudy dupStudyContent)
    {
        var dtpStudyContentInDb = new DupStudyInDb(dupStudyContent) { id = aId };
        var res = await _dupRepository.UpdateDupStudyAsync(dtpStudyContentInDb);
        return res == null ? null : new DupStudy(res);
    }

    public async Task<int> DeleteDupStudyAsync(int id)
        => await _dupRepository.DeleteDupStudyAsync(id);
    
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DupObject>?> GetAllDupObjectsAsync(int dup_id) {
        var dupObjectsInDb = (await _dupRepository.GetAllDupObjectsAsync(dup_id)).ToList();
        return (!dupObjectsInDb.Any()) ? null 
            : dupObjectsInDb.Select(r => new DupObject(r)).ToList();
    }

    public async Task<DupObject?> GetDupObjectAsync(int id)
    {
        var dupObjectInDb = await _dupRepository.GetDupObjectAsync(id);
        return dupObjectInDb == null ? null : new DupObject(dupObjectInDb);
    }
 
    // Update data
    public async Task<DupObject?> CreateDupObjectAsync(DupObject dupObjectContent)
    {
        var dupObjectInDb = new DupObjectInDb(dupObjectContent);
        var res = await _dupRepository.CreateDupObjectAsync(dupObjectInDb);
        return res == null ? null : new DupObject(res);
    }

    public async Task<DupObject?> UpdateDupObjectAsync(int aId, DupObject dupObjectContent)
    {
        var dupObjectInDb = new DupObjectInDb(dupObjectContent) { id = aId };
        var res = await _dupRepository.UpdateDupObjectAsync(dupObjectInDb);
        return res == null ? null : new DupObject(res);
    }

    public async Task<int> DeleteDupObjectAsync(int id)
        => await _dupRepository.DeleteDupObjectAsync(id);
 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dua>?> GetAllDuasAsync(int dup_id)
    {
        var duasInDb = (await _dupRepository.GetAllDuasAsync(dup_id)).ToList();
        return (!duasInDb.Any()) ? null 
            : duasInDb.Select(r => new Dua(r)).ToList();
    }

    public async Task<Dua?> GetDuaAsync(int id)
    {
        var duaInDb = await _dupRepository.GetDuaAsync(id);
        return duaInDb == null ? null : new Dua(duaInDb);
    }
 
    // Update data
    public async Task<Dua?> CreateDuaAsync(Dua duaContent)
    {
        var duaInDb = new DuaInDb(duaContent);
        var res = await _dupRepository.CreateDuaAsync(duaInDb);
        return res == null ? null : new Dua(res);
    }

    public async Task<Dua?> UpdateDuaAsync(int aId, Dua duaContent)
    {
        var duaInDb = new DuaInDb(duaContent) { id = aId };
        var res = await _dupRepository.UpdateDuaAsync(duaInDb);
        return res == null ? null : new Dua(res);
    }

    public async Task<int> DeleteDuaAsync(int id)
        => await _dupRepository.DeleteDuaAsync(id);
 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
    
    // Fetch data
    public async Task<List<DupPrereq>?> GetAllDupPrereqsAsync(int dup_id, string sd_oid)
    {
        var dupPrereqsInDb = (await _dupRepository.GetAllDupPrereqsAsync(dup_id, sd_oid)).ToList();
        return (!dupPrereqsInDb.Any()) ? null 
            : dupPrereqsInDb.Select(r => new DupPrereq(r)).ToList();
    }

    public async Task<DupPrereq?> GetDupPrereqAsync(int id)
    {
        var dupPrereqInDb = await _dupRepository.GetDupPrereqAsync(id);
        return dupPrereqInDb == null ? null : new DupPrereq(dupPrereqInDb);
    }
 
    // Update data
    public async Task<DupPrereq?> CreateDupPrereqAsync(DupPrereq dupPrereqContent)
    {
        var dupPrereqInDb = new DupPrereqInDb(dupPrereqContent);
        var res = await _dupRepository.CreateDupPrereqAsync(dupPrereqInDb);
        return res == null ? null : new DupPrereq(res);
    }

    public async Task<DupPrereq?> UpdateDupPrereqAsync(int aId, DupPrereq dupPrereqContent)
    {
        var dupPrereqInDb = new DupPrereqInDb(dupPrereqContent) { id = aId };
        var res = await _dupRepository.UpdateDupPrereqAsync(dupPrereqInDb);
        return res == null ? null : new DupPrereq(res);
    }

    public async Task<int> DeleteDupPrereqAsync(int id)
        => await _dupRepository.DeleteDupPrereqAsync(id);
 

    /****************************************************************
    * Secondary use
    ****************************************************************/

    // Fetch data
    public async Task<List<SecondaryUse>?> GetAllSecUsesAsync(int dup_id)
    {
        var dupSecUsesInDb = (await _dupRepository.GetAllSecUsesAsync(dup_id)).ToList();
        return (!dupSecUsesInDb.Any()) ? null 
            : dupSecUsesInDb.Select(r => new SecondaryUse(r)).ToList();
    }

    public async Task<SecondaryUse?> GetSecUseAsync(int id)
    {
        var dupSecUseInDb = await _dupRepository.GetSecUseAsync(id);
        return dupSecUseInDb == null ? null : new SecondaryUse(dupSecUseInDb);
    }

    // Update data
    public async Task<SecondaryUse?> CreateSecUseAsync(SecondaryUse secUseContent)
    {
        var secUseInDb = new SecondaryUseInDb(secUseContent);
        var res = await _dupRepository.CreateSecUseAsync(secUseInDb);
        return res == null ? null : new SecondaryUse(res);
    }

    public async Task<SecondaryUse?> UpdateSecUseAsync(int aId, SecondaryUse secUseContent)
    {
        var secUseInDb = new SecondaryUseInDb(secUseContent) { id = aId };
        var res = await _dupRepository.UpdateSecUseAsync(secUseInDb);
        return res == null ? null : new SecondaryUse(res);
    }

    public async Task<int> DeleteSecUseAsync(int id)
        => await _dupRepository.DeleteSecUseAsync(id);


    /****************************************************************
    * DUP notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DupNote>?> GetAllDupNotesAsync(int dup_id)
    {
        var dupNotesInDb = (await _dupRepository.GetAllDupNotesAsync(dup_id)).ToList();
        return (!dupNotesInDb.Any()) ? null 
            : dupNotesInDb.Select(r => new DupNote(r)).ToList();
    }

    public async Task<DupNote?> GetDupNoteAsync(int id)
    {
        var dupNoteInDb = await _dupRepository.GetDupNoteAsync(id);
        return dupNoteInDb == null ? null : new DupNote(dupNoteInDb);
    }
 
    // Update data
    public async Task<DupNote?> CreateDupNoteAsync(DupNote dupNoteContent)
    {
        var dupNoteInDb = new DupNoteInDb(dupNoteContent);
        var res = await _dupRepository.CreateDupNoteAsync(dupNoteInDb);
        return res == null ? null : new DupNote(res);
    }

    public async Task<DupNote?> UpdateDupNoteAsync(int aId, DupNote dupNoteContent)
    {
        var dupNoteInDb = new DupNoteInDb(dupNoteContent) { id = aId };
        var res = await _dupRepository.UpdateDupNoteAsync(dupNoteInDb);
        return res == null ? null : new DupNote(res);
    }

    public async Task<int> DeleteDupNoteAsync(int id)
        => await _dupRepository.DeleteDupNoteAsync(id);


    /****************************************************************
    * DUP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DupPerson>?> GetAllDupPeopleAsync(int dup_id)
    {
        var dupPeopleInDb = (await _dupRepository.GetAllDupPeopleAsync(dup_id)).ToList();
        return (!dupPeopleInDb.Any()) ? null 
            : dupPeopleInDb.Select(r => new DupPerson(r)).ToList();
    }

    public async Task<DupPerson?> GetDupPersonAsync(int id)
    {
        var dupPersonInDb = await _dupRepository.GetDupPersonAsync(id);
        return dupPersonInDb == null ? null : new DupPerson(dupPersonInDb);
    }
 
    // Update data
    public async Task<DupPerson?> CreateDupPersonAsync(DupPerson dupPersonContent)
    {
        var dupPersonInDb = new DupPersonInDb(dupPersonContent);
        var res = await _dupRepository.CreateDupPersonAsync(dupPersonInDb);
        return res == null ? null : new DupPerson(res);
    }

    public async Task<DupPerson?> UpdateDupPersonAsync(int aId, DupPerson dupPersonContent)
    {
        var dupPersonInDb = new DupPersonInDb(dupPersonContent) { id = aId };
        var res = await _dupRepository.UpdateDupPersonAsync(dupPersonInDb);
        return res == null ? null : new DupPerson(res);
    }

    public async Task<int> DeleteDupPersonAsync(int id)
        => await _dupRepository.DeleteDupPersonAsync(id);

}