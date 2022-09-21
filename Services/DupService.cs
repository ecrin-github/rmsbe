using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;


namespace rmsbe.Services;

public class DupService : IDupService
{
    private readonly IDupRepository _dupRepository;
    private readonly ILookupService _lupService;
    private List<Lup> _lookups;

    public DupService(IDupRepository dupRepository, ILookupService lupService)
    {
        _dupRepository = dupRepository ?? throw new ArgumentNullException(nameof(dupRepository));
        _lupService = lupService ?? throw new ArgumentNullException(nameof(lupService));
        _lookups = new List<Lup>();
    }
    
    /****************************************************************
    * Check functions 
    ****************************************************************/
    
    // Check if DUP exists
    public async Task<bool> DupExists(int id)
        => await _dupRepository.DupExists(id);
    
    // Check if attribute exists on this DUP
    public async Task<bool> DupAttributeExists(int dupId, string typeName, int id)
        => await _dupRepository.DupAttributeExists(dupId, typeName, id);
    
    // Check if DUA exists on this DUP
    public async Task<bool> DupDuaExists(int dupId)
        => await _dupRepository.DupDuaExists(dupId);

    // Check if DUP / object combination exists
    public async Task<bool> DupObjectExists(int dupId, string sdOid)
        => await _dupRepository.DupObjectExists(dupId, sdOid);

    // Check if pre-req exists on this DUP / object
    public async Task<bool> DupObjectAttributeExists(int dupId, string sdOid, string typeName, int id)
        => await _dupRepository.DupObjectAttributeExists(dupId, sdOid, typeName, id);
    
    /****************************************************************
    * All DUPs / DUP entries
    ****************************************************************/
    
    public async Task<List<Dup>?> GetAllDups()
    {
        var dupsInDb = (await _dupRepository.GetAllDups()).ToList();
        return (!dupsInDb.Any()) ? null 
            : dupsInDb.Select(r => new Dup(r)).ToList();
    }
    
    public async Task<List<DupEntry>?> GetAllDupEntries(){ 
        var dupEntriesInDb = (await _dupRepository.GetAllDupEntries()).ToList();
        return !dupEntriesInDb.Any() ? null 
            : dupEntriesInDb.Select(r => new DupEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated DUPs / DUP entries
    ****************************************************************/
   
    public async Task<List<Dup>?> GetPaginatedDupData(PaginationRequest validFilter)
    {
        var pagedDupsInDb = (await _dupRepository
            .GetPaginatedDupData(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedDupsInDb.Any() ? null 
            : pagedDupsInDb.Select(r => new Dup(r)).ToList();
    }

    public async Task<List<DupEntry>?> GetPaginatedDupEntries(PaginationRequest validFilter)
    {
        var pagedDupEntriesInDb = (await _dupRepository
            .GetPaginatedDupEntries(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedDupEntriesInDb.Any() ? null 
            : pagedDupEntriesInDb.Select(r => new DupEntry(r)).ToList();
    }
     
    /****************************************************************
    * Filtered DUPs / DUP entries
    ****************************************************************/        
    
    public async Task<List<Dup>?> GetFilteredDupRecords(string titleFilter)
    {
        var filteredDupsInDb = (await _dupRepository
            .GetFilteredDupData(titleFilter)).ToList();
        return !filteredDupsInDb.Any() ? null 
            : filteredDupsInDb.Select(r => new Dup(r)).ToList();
    }
    
    public async Task<List<DupEntry>?> GetFilteredDupEntries(string titleFilter)
    {
        var filteredDupEntriesInDb = (await _dupRepository
            .GetFilteredDupEntries(titleFilter)).ToList();
        return !filteredDupEntriesInDb.Any() ? null 
            : filteredDupEntriesInDb.Select(r => new DupEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated and filtered DUPs / DUP entries
    ****************************************************************/
    
    public async Task<List<Dup>?> GetPaginatedFilteredDupRecords(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredDupsInDb = (await _dupRepository
            .GetPaginatedFilteredDupData(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredDupsInDb.Any() ? null 
            : pagedFilteredDupsInDb.Select(r => new Dup(r)).ToList();
    }

    public async Task<List<DupEntry>?> GetPaginatedFilteredDupEntries(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredDupEntriesInDb = (await _dupRepository
            .GetPaginatedFilteredDupEntries(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredDupEntriesInDb.Any() ? null 
            : pagedFilteredDupEntriesInDb.Select(r => new DupEntry(r)).ToList();
    }
    
    /****************************************************************
    * Recent DUPs / DUP entries
    ****************************************************************/
    
    public async Task<List<Dup>?> GetRecentDups(int n)
    {
        var recentDupsInDb = (await _dupRepository.GetRecentDups(n)).ToList();
        return !recentDupsInDb.Any() ? null 
            : recentDupsInDb.Select(r => new Dup(r)).ToList();
    }
    
    public async Task<List<DupEntry>?> GetRecentDupEntries(int n){ 
        var recentDupEntriesInDb = (await _dupRepository.GetRecentDupEntries(n)).ToList();
        return !recentDupEntriesInDb.Any() ? null 
            : recentDupEntriesInDb.Select(r => new DupEntry(r)).ToList();
    }
    
    /****************************************************************
    * DUPs / DUP entries by Organisation
    ****************************************************************/
    
    public async Task<List<Dup>?> GetDupsByOrg(int orgId)
    {
        var dupsByOrgInDb = (await _dupRepository.GetDupsByOrg(orgId)).ToList();
        return !dupsByOrgInDb.Any() ? null 
            : dupsByOrgInDb.Select(r => new Dup(r)).ToList();
    }
    
    public async Task<List<DupEntry>?> GetDupEntriesByOrg(int orgId){ 
        var dupEntriesByOrgInDb = (await _dupRepository.GetDupEntriesByOrg(orgId)).ToList();
        return !dupEntriesByOrgInDb.Any() ? null 
            : dupEntriesByOrgInDb.Select(r => new DupEntry(r)).ToList();
    }

    
    /****************************************************************
    * Full DUP data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<FullDup?> GetFullDupById(int id){ 
        FullDupInDb? fullDupInDb = await _dupRepository.GetFullDupById(id);
        return fullDupInDb == null ? null : new FullDup(fullDupInDb);
    }
    
    // Delete data
    public async Task<int> DeleteFullDup(int id) 
        => await _dupRepository.DeleteFullDup(id);

    /****************************************************************
    * Statistics
    ****************************************************************/

    public async Task<Statistic> GetTotalDups()
    {
        int res = await _dupRepository.GetTotalDups();
        return new Statistic("Total", res);
    }
    
    public async Task<Statistic> GetTotalFilteredDups(string titleFilter)
    {
        int res = await _dupRepository.GetTotalFilteredDups(titleFilter);
        return new Statistic("TotalFiltered", res);
    }
    
    public async Task<List<Statistic>?> GetDupsByStatus()
    {
        var res = (await _dupRepository.GetDupsByStatus()).ToList();
        if (await ResetLookups("dup-status-types"))
        {
            return !res.Any()
                ? null
                : res.Select(r => new Statistic(LuTypeName(r.stat_type), r.stat_value)).ToList();
        }
        return null;
    }
    
    public async Task<List<Statistic>> GetDupsByCompletion()
    {
        int total = await _dupRepository.GetTotalDups();
        int completed = await _dupRepository.GetCompletedDups();
        return new List<Statistic>()
        {
            new Statistic("Total", total),
            new Statistic("Incomplete", total - completed)
        };
    }

    private string LuTypeName(int n)
    {
        foreach (var p in _lookups.Where(p => n == p.Id))
        {
            return p.Name ?? "null name in matching lookup!";
        }
        return "not known";
    }

    private async Task<bool> ResetLookups(string typeName)
    {
        _lookups = new List<Lup>();  // reset to empty list
        _lookups = await _lupService.GetLookUpValues(typeName);
        return _lookups.Count > 0 ;
    }
    
    /****************************************************************
    * Get single DUP record
    ****************************************************************/      
    
    public async Task<Dup?> GetDup(int dupId)
    {
        var dupInDb = await _dupRepository.GetDup(dupId);
        return dupInDb == null ? null : new Dup(dupInDb);
    }
    
    public async Task<DupOut?> GetOutDup(int dupId)
    {
        var dupOutInDb = await _dupRepository.GetOutDup(dupId);
        return dupOutInDb == null ? null : new DupOut(dupOutInDb);
    }
 
    /****************************************************************
    * Update DUP records
    ****************************************************************/ 
    
    public async Task<Dup?> CreateDup(Dup dupContent)
    {
        var dupInDb = new DupInDb(dupContent);
        var res = await _dupRepository.CreateDup(dupInDb);
        return res == null ? null : new Dup(res);
    }

    public async Task<Dup?> UpdateDup(int dupId, Dup dupContent)
    {
        var dupInDb = new DupInDb(dupContent) { id = dupId };
        var res = await _dupRepository.UpdateDup(dupInDb);
        return res == null ? null : new Dup(res);
    }

    public async Task<int> DeleteDup(int dupId)
        => await _dupRepository.DeleteDup(dupId);
 
    
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    public async Task<List<DupStudy>?> GetAllDupStudies(int dupId) {
        var dupStudiesInDb = (await _dupRepository.GetAllDupStudies(dupId)).ToList();
        return (!dupStudiesInDb.Any()) ? null 
            : dupStudiesInDb.Select(r => new DupStudy(r)).ToList();
    }

    public async Task<List<DupStudyOut>?> GetAllOutDupStudies(int dupId){
        var dupStudiesOutInDb = (await _dupRepository.GetAllOutDupStudies(dupId)).ToList();
        return (!dupStudiesOutInDb.Any()) ? null 
            : dupStudiesOutInDb.Select(r => new DupStudyOut(r)).ToList();
    }
    
    public async Task<DupStudy?> GetDupStudy(int id)
    {
        var dupStudyInDb = await _dupRepository.GetDupStudy(id);
        return dupStudyInDb == null ? null : new DupStudy(dupStudyInDb);
    }
    
    public async Task<DupStudyOut?> GetOutDupStudy(int id) {
        var dupStudyOutInDb = await _dupRepository.GetOutDupStudy(id);
        return dupStudyOutInDb == null ? null : new DupStudyOut(dupStudyOutInDb);
    }
 
    // Update data
    public async Task<DupStudy?> CreateDupStudy(DupStudy dupStudyContent)
    {
        var dupStudyInDb = new DupStudyInDb(dupStudyContent);
        var res = await _dupRepository.CreateDupStudy(dupStudyInDb);
        return res == null ? null : new DupStudy(res);
    }

    public async Task<DupStudy?> UpdateDupStudy(DupStudy dupStudyContent)
    {
        var dupDupContentInDb = new DupStudyInDb(dupStudyContent);
        var res = await _dupRepository.UpdateDupStudy(dupDupContentInDb);
        return res == null ? null : new DupStudy(res);
    }

    public async Task<int> DeleteDupStudy(int id)
        => await _dupRepository.DeleteDupStudy(id);
    
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DupObject>?> GetAllDupObjects(int dupId) {
        var dupObjectsInDb = (await _dupRepository.GetAllDupObjects(dupId)).ToList();
        return (!dupObjectsInDb.Any()) ? null 
            : dupObjectsInDb.Select(r => new DupObject(r)).ToList();
    }

    public async Task<List<DupObjectOut>?> GetAllOutDupObjects(int dupId){
        var dupObjectsOutInDb = (await _dupRepository.GetAllOutDupObjects(dupId)).ToList();
        return (!dupObjectsOutInDb.Any()) ? null 
            : dupObjectsOutInDb.Select(r => new DupObjectOut(r)).ToList();
    }
    
    public async Task<DupObject?> GetDupObject(int id)
    {
        var dupObjectInDb = await _dupRepository.GetDupObject(id);
        return dupObjectInDb == null ? null : new DupObject(dupObjectInDb);
    }
    
    public async Task<DupObjectOut?> GetOutDupObject(int id){
        var dupObjectOutInDb = await _dupRepository.GetOutDupObject(id);
        return dupObjectOutInDb == null ? null : new DupObjectOut(dupObjectOutInDb);
    }
    
    // Update data
    public async Task<DupObject?> CreateDupObject(DupObject dupObjectContent)
    {
        var dupObjectInDb = new DupObjectInDb(dupObjectContent);
        var res = await _dupRepository.CreateDupObject(dupObjectInDb);
        return res == null ? null : new DupObject(res);
    }

    public async Task<DupObject?> UpdateDupObject(DupObject dupObjectContent)
    {
        var dupObjectInDb = new DupObjectInDb(dupObjectContent);
        var res = await _dupRepository.UpdateDupObject(dupObjectInDb);
        return res == null ? null : new DupObject(res);
    }

    public async Task<int> DeleteDupObject(int id)
        => await _dupRepository.DeleteDupObject(id);
 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    public async Task<Dua?> GetDua(int dupId)
    {
        var duaInDb = await _dupRepository.GetDua(dupId);
        return duaInDb == null ? null : new Dua(duaInDb);
    }
 
    public async Task<DuaOut?> GetOutDua(int dupId) {
        var duaOutInDb = await _dupRepository.GetOutDua(dupId);
        return duaOutInDb == null ? null : new DuaOut(duaOutInDb);
    }
    
    // Update data
    public async Task<Dua?> CreateDua(Dua duaContent)
    {
        var duaInDb = new DuaInDb(duaContent);
        var res = await _dupRepository.CreateDua(duaInDb);
        return res == null ? null : new Dua(res);
    }

    public async Task<Dua?> UpdateDua(Dua duaContent)
    {
        var duaInDb = new DuaInDb(duaContent);
        var res = await _dupRepository.UpdateDua(duaInDb);
        return res == null ? null : new Dua(res);
    }

    public async Task<int> DeleteDua(int dupId)
        => await _dupRepository.DeleteDua(dupId);
 
    
    /****************************************************************
    * DUP pre-requisites met
    ****************************************************************/
    
    // Fetch data
    public async Task<List<DupPrereq>?> GetAllDupPrereqs(int dupId, string sdOid)
    {
        var dupPrereqsInDb = (await _dupRepository.GetAllDupPrereqs(dupId, sdOid)).ToList();
        return (!dupPrereqsInDb.Any()) ? null 
            : dupPrereqsInDb.Select(r => new DupPrereq(r)).ToList();
    }

    public async Task<List<DupPrereqOut>?> GetAllOutDupPrereqs(int dupId, string sdOid){
        var dupPrereqsOutInDb = (await _dupRepository.GetAllOutDupPrereqs(dupId, sdOid)).ToList();
        return (!dupPrereqsOutInDb.Any()) ? null 
            : dupPrereqsOutInDb.Select(r => new DupPrereqOut(r)).ToList();
    }
    
    public async Task<DupPrereq?> GetDupPrereq(int id)
    {
        var dupPrereqInDb = await _dupRepository.GetDupPrereq(id);
        return dupPrereqInDb == null ? null : new DupPrereq(dupPrereqInDb);
    }
    
    public async Task<DupPrereqOut?> GetOutDupPrereq(int id){
        var dupPrereqOutInDb = await _dupRepository.GetOutDupPrereq(id);
        return dupPrereqOutInDb == null ? null : new DupPrereqOut(dupPrereqOutInDb);
    }
 
    // Update data
    public async Task<DupPrereq?> CreateDupPrereq(DupPrereq dupPrereqContent)
    {
        var dupPrereqInDb = new DupPrereqInDb(dupPrereqContent);
        var res = await _dupRepository.CreateDupPrereq(dupPrereqInDb);
        return res == null ? null : new DupPrereq(res);
    }

    public async Task<DupPrereq?> UpdateDupPrereq(DupPrereq dupPrereqContent)
    {
        var dupPrereqInDb = new DupPrereqInDb(dupPrereqContent);
        var res = await _dupRepository.UpdateDupPrereq(dupPrereqInDb);
        return res == null ? null : new DupPrereq(res);
    }

    public async Task<int> DeleteDupPrereq(int id)
        => await _dupRepository.DeleteDupPrereq(id);
 

    /****************************************************************
    * Secondary use
    ****************************************************************/

    // Fetch data
    public async Task<List<DupSecondaryUse>?> GetAllSecUses(int dupId)
    {
        var dupSecUsesInDb = (await _dupRepository.GetAllSecUses(dupId)).ToList();
        return (!dupSecUsesInDb.Any()) ? null 
            : dupSecUsesInDb.Select(r => new DupSecondaryUse(r)).ToList();
    }

    public async Task<DupSecondaryUse?> GetSecUse(int id)
    {
        var dupSecUseInDb = await _dupRepository.GetSecUse(id);
        return dupSecUseInDb == null ? null : new DupSecondaryUse(dupSecUseInDb);
    }

    // Update data
    public async Task<DupSecondaryUse?> CreateSecUse(DupSecondaryUse secUseContent)
    {
        var secUseInDb = new DupSecondaryUseInDb(secUseContent);
        var res = await _dupRepository.CreateSecUse(secUseInDb);
        return res == null ? null : new DupSecondaryUse(res);
    }

    public async Task<DupSecondaryUse?> UpdateSecUse(DupSecondaryUse secUseContent)
    {
        var secUseInDb = new DupSecondaryUseInDb(secUseContent);
        var res = await _dupRepository.UpdateSecUse(secUseInDb);
        return res == null ? null : new DupSecondaryUse(res);
    }

    public async Task<int> DeleteSecUse(int id)
        => await _dupRepository.DeleteSecUse(id);


    /****************************************************************
    * DUP notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DupNote>?> GetAllDupNotes(int dupId)
    {
        var dupNotesInDb = (await _dupRepository.GetAllDupNotes(dupId)).ToList();
        return (!dupNotesInDb.Any()) ? null 
            : dupNotesInDb.Select(r => new DupNote(r)).ToList();
    }
    
    public async Task<List<DupNoteOut>?> GetAllOutDupNotes(int dupId){
        var dupNotesOutInDb = (await _dupRepository.GetAllOutDupNotes(dupId)).ToList();
        return (!dupNotesOutInDb.Any()) ? null 
            : dupNotesOutInDb.Select(r => new DupNoteOut(r)).ToList();
    }

    public async Task<DupNote?> GetDupNote(int id)
    {
        var dupNoteInDb = await _dupRepository.GetDupNote(id);
        return dupNoteInDb == null ? null : new DupNote(dupNoteInDb);
    }
    
    public async Task<DupNoteOut?> GetOutDupNote(int id){
        var dupNoteOutInDb = await _dupRepository.GetOutDupNote(id);
        return dupNoteOutInDb == null ? null : new DupNoteOut(dupNoteOutInDb);
    }
 
    // Update data
    public async Task<DupNote?> CreateDupNote(DupNote dupNoteContent)
    {
        var dupNoteInDb = new DupNoteInDb(dupNoteContent);
        var res = await _dupRepository.CreateDupNote(dupNoteInDb);
        return res == null ? null : new DupNote(res);
    }

    public async Task<DupNote?> UpdateDupNote(DupNote dupNoteContent)
    {
        var dupNoteInDb = new DupNoteInDb(dupNoteContent);
        var res = await _dupRepository.UpdateDupNote(dupNoteInDb);
        return res == null ? null : new DupNote(res);
    }

    public async Task<int> DeleteDupNote(int id)
        => await _dupRepository.DeleteDupNote(id);


    /****************************************************************
    * DUP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DupPerson>?> GetAllDupPeople(int dupId)
    {
        var dupPeopleInDb = (await _dupRepository.GetAllDupPeople(dupId)).ToList();
        return (!dupPeopleInDb.Any()) ? null 
            : dupPeopleInDb.Select(r => new DupPerson(r)).ToList();
    }
    
    public async Task<List<DupPersonOut>?> GetAllOutDupPeople(int dupId) {
        var dupPeopleOutInDb = (await _dupRepository.GetAllOutDupPeople(dupId)).ToList();
        return (!dupPeopleOutInDb.Any()) ? null 
            : dupPeopleOutInDb.Select(r => new DupPersonOut(r)).ToList();
    }

    public async Task<DupPerson?> GetDupPerson(int id)
    {
        var dupPersonInDb = await _dupRepository.GetDupPerson(id);
        return dupPersonInDb == null ? null : new DupPerson(dupPersonInDb);
    }
    
    public async Task<DupPersonOut?> GetOutDupPerson(int id){
        var dupPersonOutInDb = await _dupRepository.GetOutDupPerson(id);
        return dupPersonOutInDb == null ? null : new DupPersonOut(dupPersonOutInDb);
    }
 
    // Update data
    public async Task<DupPerson?> CreateDupPerson(DupPerson dupPersonContent)
    {
        var dupPersonInDb = new DupPersonInDb(dupPersonContent);
        var res = await _dupRepository.CreateDupPerson(dupPersonInDb);
        return res == null ? null : new DupPerson(res);
    }

    public async Task<DupPerson?> UpdateDupPerson(DupPerson dupPersonContent)
    {
        var dupPersonInDb = new DupPersonInDb(dupPersonContent);
        var res = await _dupRepository.UpdateDupPerson(dupPersonInDb);
        return res == null ? null : new DupPerson(res);
    }

    public async Task<int> DeleteDupPerson(int id)
        => await _dupRepository.DeleteDupPerson(id);
    
}