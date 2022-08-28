using rmsbe.SysModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Services;

public class StudyService : IStudyService
{
    private readonly IStudyRepository _studyRepository;
    private readonly ILookupService _lupService;
    private List<Lup> _lookups;
    private string _userName;

    public StudyService(IStudyRepository studyRepository, ILookupService lupService)
    {
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        _lupService = lupService ?? throw new ArgumentNullException(nameof(lupService));
        _lookups = new List<Lup>();

        // for now - need a mechanism to inject this from user object,
        // either directly here or from controller;
        
        DateTime now = DateTime.Now;
        string timestring = now.Hour.ToString() + ":" + now.Minute.ToString() + ":" + now.Second.ToString(); 
        _userName = "test user" + "_" + timestring; 
    }
    
    /****************************************************************
    * Check functions
    ****************************************************************/
    
    // Check if study exists 
    public async Task<bool> StudyExists(string sdSid)
           => await _studyRepository.StudyExists(sdSid);

    // Check if attribute exists on this study
    public async Task<bool> StudyAttributeExists(string sdSid, string typeName, int id)
        => await _studyRepository.StudyAttributeExists(sdSid,typeName, id); 
    
    /****************************************************************
    * All Study Records and study entries
    ****************************************************************/
    
    public async Task<List<StudyData>?> GetAllStudyRecords(){ 
        var studiesInDb = (await _studyRepository.GetAllStudyRecords()).ToList();
        return !studiesInDb.Any() ? null 
            : studiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyEntry>?> GetAllStudyEntries(){ 
        var studiesInDb = (await _studyRepository.GetAllStudyEntries()).ToList();
        return !studiesInDb.Any() ? null 
            : studiesInDb.Select(r => new StudyEntry(r)).ToList();
    }    
    
    /****************************************************************
    * Paginated Study Records and study entries
    ****************************************************************/
   
    public async Task<List<StudyData>?> GetPaginatedStudyRecords(PaginationRequest validFilter)
    {
        var pagedStudiesInDb = (await _studyRepository
            .GetPaginatedStudyRecords(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedStudiesInDb.Any() ? null 
            : pagedStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
   
    public async Task<List<StudyEntry>?> GetPaginatedStudyEntries(PaginationRequest validFilter)
    {
        var pagedStudiesInDb = (await _studyRepository
            .GetPaginatedStudyEntries(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedStudiesInDb.Any() ? null 
            : pagedStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }

    /****************************************************************
    * Filtered Study Records and study entries
    ****************************************************************/    
    
    public async Task<List<StudyData>?> GetFilteredStudyRecords(string titleFilter)
    {
        var filteredStudiesInDb = (await _studyRepository
            .GetFilteredStudyRecords(titleFilter)).ToList();
        return !filteredStudiesInDb.Any() ? null 
            : filteredStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyEntry>?> GetFilteredStudyEntries(string titleFilter)
    {
        var filteredStudiesInDb = (await _studyRepository
            .GetFilteredStudyEntries(titleFilter)).ToList();
        return !filteredStudiesInDb.Any() ? null 
            : filteredStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated and filtered Study Records and study entries
    ****************************************************************/    
    
    public async Task<List<StudyData>?> GetPaginatedFilteredStudyRecords(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredStudiesInDb = (await _studyRepository
            .GetPaginatedFilteredStudyRecords(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredStudiesInDb.Any() ? null 
            : pagedFilteredStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyEntry>?> GetPaginatedFilteredStudyEntries(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredStudiesInDb = (await _studyRepository
            .GetPaginatedFilteredStudyEntries(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredStudiesInDb.Any() ? null 
            : pagedFilteredStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }
   
    /****************************************************************
    * Recent Study Records and study entries
    ****************************************************************/
    
    public async Task<List<StudyData>?> GetRecentStudyRecords(int n){ 
        var recentStudiesInDb = (await _studyRepository.GetRecentStudyRecords(n)).ToList();
        return !recentStudiesInDb.Any() ? null 
            : recentStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
     
    public async Task<List<StudyEntry>?> GetRecentStudyEntries(int n){ 
        var recentStudiesInDb = (await _studyRepository.GetRecentStudyEntries(n)).ToList();
        return !recentStudiesInDb.Any() ? null 
            : recentStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }
 
    /****************************************************************
    * Study Records and study entries by Organisation
    ****************************************************************/
    
    public async Task<List<StudyData>?> GetStudyRecordsByOrg(int orgId){ 
        var studiesByOrgInDb = (await _studyRepository.GetStudyRecordsByOrg(orgId)).ToList();
        return !studiesByOrgInDb.Any() ? null 
            : studiesByOrgInDb.Select(r => new StudyData(r)).ToList();
    }
     
    public async Task<List<StudyEntry>?> GetStudyEntriesByOrg(int orgId){ 
        var studyEntriesByOrgInDb = (await _studyRepository.GetStudyEntriesByOrg(orgId)).ToList();
        return !studyEntriesByOrgInDb.Any() ? null 
            : studyEntriesByOrgInDb.Select(r => new StudyEntry(r)).ToList();
    }
    
    /****************************************************************
    * Fetch a single Study record 
    ****************************************************************/
    
    public async Task<StudyData?> GetStudyRecordData(string sdSid){ 
        var studyInDb = await _studyRepository.GetStudyData(sdSid);
        return studyInDb == null ? null : new StudyData(studyInDb);
    }
    
    /****************************************************************
    * Update Study record data
    ****************************************************************/
    
    public async Task<StudyData?> CreateStudyRecordData(StudyData studyDataContent){ 
        var stInDb = new StudyInDb(studyDataContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyData(stInDb);
        return res == null ? null : new StudyData(res);
    }
    
    public async Task<StudyData?> UpdateStudyRecordData(StudyData studyDataContent){ 
        var stInDb = new StudyInDb(studyDataContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyData(stInDb);
        return res == null ? null : new StudyData(res);
    }
    
    public async Task<int> DeleteStudyRecordData(string sdSid) 
           => await _studyRepository.DeleteStudyData(sdSid, _userName);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<FullStudy?> GetFullStudyById(string sdSid){ 
        FullStudyInDb? fullStudyInDb = await _studyRepository.GetFullStudyById(sdSid);
        return fullStudyInDb == null ? null : new FullStudy(fullStudyInDb);
    }
    
    // Delete data
    public async Task<int> DeleteFullStudy(string sdSid) 
           => await _studyRepository.DeleteFullStudy(sdSid, _userName);
    
    // List of linked data objects 
    public async Task<List<DataObjectEntry>?> GetStudyObjectList(string sdSid)
    {
        var studyObjectsInDb = (await _studyRepository.GetStudyObjectList(sdSid)).ToList();
        return (!studyObjectsInDb.Any()) ? null 
            : studyObjectsInDb.Select(r => new DataObjectEntry(r)).ToList();
    }
    
    /****************************************************************
    * Fetch study data (only) from the MDR and store it in the RMS
    * Not terribly useful except as a rehearsal for the 'full' 
    * transfer of data...
    ****************************************************************/

    public async Task<StudyData?> GetStudyFromMdr(int regId, string sdSid)
    {
        // get the preferred registry id - may not be the same as that provided
        
        StudyMdrDetails? mdrDets = await _studyRepository.GetStudyDetailsFromMdr(regId, sdSid);
        if (mdrDets == null) // MDR details not found
        {
            return null;
        }

        // first check if this study has not already been added
        
        var newStudyRmsSdSid =  "RMS-" + mdrDets.sd_sid;
        if (await _studyRepository.StudyExists(newStudyRmsSdSid))
        {
            return new StudyData() { DisplayTitle = "EXISTING RMS STUDY" };
        }
        
        // if genuinely new get the details from the MDR
        
        StudyInDb? studyInDb = null;
        var studyInMdr = await _studyRepository.GetStudyDataFromMdr(mdrDets.study_id);
        if (studyInMdr != null)
        {
            var newStudyInDb = new StudyInDb(studyInMdr, mdrDets);
            newStudyInDb.last_edited_by = _userName;
            studyInDb = await _studyRepository.CreateStudyData(newStudyInDb);
        }
        return studyInDb == null ? null : new StudyData(studyInDb);

    }
        

    /****************************************************************
     * Fetch study data and all related data - study attributes
     * and a list of linked objects - from the MDR
     * The objects are listed to allow them to be selected for
     * possible inclusion as well...
     ****************************************************************/
    
    public async Task<FullStudyFromMdr?> GetFullStudyFromMdr(int regId, string sdSid)
    {
        // get the preferred registry id - may not be the same as that provided
        
        StudyMdrDetails? mdrDets = await _studyRepository.GetStudyDetailsFromMdr(regId, sdSid);
        if (mdrDets == null) // MDR details not found
        {
            return null;
        }
        
        // check if this study has not already been added
        
        var newStudyRmsSdSid =  "RMS-" + mdrDets.sd_sid;
        if (await _studyRepository.StudyExists(newStudyRmsSdSid))
        {
            return new FullStudyFromMdr() { CoreStudy = 
                new StudyData() { DisplayTitle = "EXISTING RMS STUDY" }};
        }
        
        // if genuinely new, retrieve the 'core' study data from the MDR studies table

        var studyInMdr = await _studyRepository.GetStudyDataFromMdr(mdrDets.study_id);
        if (studyInMdr == null)
        {
            return null;
        }
        
        // Create a new new study record in the format expected by the RMS
        // add in the user details and store in the RMS studies table
        
        var newStudyInDb = new StudyInDb(studyInMdr, mdrDets)
        {
            last_edited_by = _userName
        };
        var studyInRmsDb = await _studyRepository.CreateStudyData(newStudyInDb);
        if (studyInRmsDb == null)
        {
            return null;
        }
        
        // Assuming new study record creation was successful, fetch and 
        // store study attributes, transferring from Mdr to Rms format and Ids
        // (The int study id must be replaced by the string sd_sid)
        // Also appends a list of linked objects, for inspection
        
        FullStudyFromMdrInDb? fullStudyFromMdrInDb = await _studyRepository.
                        GetFullStudyDataFromMdr(studyInRmsDb, mdrDets.study_id);
        return fullStudyFromMdrInDb == null ? null : new FullStudyFromMdr(fullStudyFromMdrInDb);
    }
    
    
    /****************************************************************
    * Statistics
    ****************************************************************/

    public async Task<Statistic> GetTotalStudies()
    {
        int res = await _studyRepository.GetTotalStudies();
        return new Statistic("Total", res);
    }

    public async Task<Statistic> GetTotalFilteredStudies(string titleFilter)
    {
        int res = await _studyRepository.GetTotalFilteredStudies(titleFilter);
        return new Statistic("TotalFiltered", res);
    }
    
    public async Task<List<Statistic>?> GetStudiesByType()
    {
        var res = (await _studyRepository.GetStudiesByType()).ToList();
        if (await ResetLookups("study-types"))
        {
            return !res.Any()
                ? null
                : res.Select(r => new Statistic(LuTypeName(r.stat_type), r.stat_value)).ToList();
        }
        return null;
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
    
    public async Task<List<Statistic>> GetStudyInvolvement(string sdSid)
    {
        var stats = new List<Statistic>(); 
        int dtpRes = await _studyRepository.GetStudyDtpInvolvement(sdSid);
        int dupRes = await _studyRepository.GetStudyDupInvolvement(sdSid);
        stats.Add(new("DtpTotal", dtpRes));
        stats.Add(new("DupTotal", dupRes));
        return stats;
    }

       
    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyIdentifier>?> GetStudyIdentifiers(string sdSid){ 
        var identifiersInDb = (await _studyRepository.GetStudyIdentifiers(sdSid)).ToList();
        return (!identifiersInDb.Any()) ? null 
            : identifiersInDb.Select(r => new StudyIdentifier(r)).ToList();
    }   
    
    public async Task<StudyIdentifier?> GetStudyIdentifier(int id){ 
        var studyIdentInDb = await _studyRepository.GetStudyIdentifier(id);
        return studyIdentInDb == null ? null : new StudyIdentifier(studyIdentInDb);
    }   
    
    // Update data
    
    public async Task<StudyIdentifier?> CreateStudyIdentifier(StudyIdentifier stIdentContent){ 
        var stIdentContentInDb = new StudyIdentifierInDb(stIdentContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyIdentifier(stIdentContentInDb);
        return res == null ? null : new StudyIdentifier(res);
    } 
    
    public async Task<StudyIdentifier?> UpdateStudyIdentifier(StudyIdentifier stIdentContent){ 
        var stIdentContentInDb = new StudyIdentifierInDb(stIdentContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyIdentifier(stIdentContentInDb);
        return res == null ? null : new StudyIdentifier(res);
    }  
    
    public async Task<int> DeleteStudyIdentifier(int id) 
           => await _studyRepository.DeleteStudyIdentifier(id, _userName);
    

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    
    public  async Task<List<StudyTitle>?> GetStudyTitles(string sdSid){ 
        var titlesInDb = (await _studyRepository.GetStudyTitles(sdSid)).ToList();
        return (!titlesInDb.Any()) ? null 
            : titlesInDb.Select(r => new StudyTitle(r)).ToList();
    }  
    
    public async Task<StudyTitle?> GetStudyTitle(int id){ 
        var studyTitleInDb = await _studyRepository.GetStudyTitle(id);
        return studyTitleInDb == null ? null : new StudyTitle(studyTitleInDb);
    }     

    // Update data
    
    public async Task<StudyTitle?> CreateStudyTitle(StudyTitle stTitleContent){ 
        var stTitleContentInDb = new StudyTitleInDb(stTitleContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyTitle(stTitleContentInDb);
        return res == null ? null : new StudyTitle(res);
    } 
    
    public async Task<StudyTitle?> UpdateStudyTitle(StudyTitle stTitleContent){ 
        var stTitleContentInDb = new StudyTitleInDb(stTitleContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyTitle(stTitleContentInDb);
        return res == null ? null : new StudyTitle(res);
    }   
    
    public async Task<int> DeleteStudyTitle(int id) 
           => await _studyRepository.DeleteStudyTitle(id, _userName);
    
 
    /****************************************************************
    * Study contributors
    ****************************************************************/   
    
    // Fetch data
    
    public async Task<List<StudyContributor>?> GetStudyContributors(string sdSid){ 
        var contributorsInDb = (await _studyRepository.GetStudyContributors(sdSid)).ToList();
        return (!contributorsInDb.Any()) ? null 
                       : contributorsInDb.Select(r => new StudyContributor(r)).ToList();
    }   
    
    public async Task<StudyContributor?> GetStudyContributor(int id){ 
        var studyContInDb = await _studyRepository.GetStudyContributor(id);
        return studyContInDb == null ? null : new StudyContributor(studyContInDb);
    }  
    
    // Update data
    
    public async Task<StudyContributor?> CreateStudyContributor(StudyContributor stContContent){  
        var stContContentInDb = new StudyContributorInDb(stContContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyContributor(stContContentInDb);
        return res == null ? null : new StudyContributor(res);
    } 
    
    public async Task<StudyContributor?> UpdateStudyContributor(StudyContributor stContContent){ 
        var stContContentInDb = new StudyContributorInDb(stContContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyContributor(stContContentInDb);
        return res == null ? null : new StudyContributor(res);
    }   

    public async Task<int> DeleteStudyContributor(int id) 
           => await _studyRepository.DeleteStudyContributor(id, _userName);

  
    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyFeature>?> GetStudyFeatures(string sdSid){ 
        var featuresInDb = (await _studyRepository.GetStudyFeatures(sdSid)).ToList();
        return (!featuresInDb.Any()) ? null 
            : featuresInDb.Select(r => new StudyFeature(r)).ToList();
    }  
    
    public async Task<StudyFeature?> GetStudyFeature(int id){ 
        var studyFeatInDb = await _studyRepository.GetStudyFeature(id);
        return studyFeatInDb == null ? null : new StudyFeature(studyFeatInDb);
    }            
    
    // Update data
    
    public async Task<StudyFeature?> CreateStudyFeature(StudyFeature stFeatureContent){ 
        var stFeatureContentInDb = new StudyFeatureInDb(stFeatureContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyFeature(stFeatureContentInDb);
        return res == null ? null : new StudyFeature(res);
    } 
    
   public async Task<StudyFeature?> UpdateStudyFeature(StudyFeature stFeatureContent){ 
       var stFeatureContentInDb = new StudyFeatureInDb(stFeatureContent) { last_edited_by = _userName };
       var res = await _studyRepository.UpdateStudyFeature(stFeatureContentInDb);
       return res == null ? null : new StudyFeature(res);
    }    
  
    public async Task<int> DeleteStudyFeature(int id) 
           => await _studyRepository.DeleteStudyFeature(id, _userName);

  
    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyTopic>?> GetStudyTopics(string sdSid){ 
        var topicsInDb = (await _studyRepository.GetStudyTopics(sdSid)).ToList();
        return (!topicsInDb.Any()) ? null 
            : topicsInDb.Select(r => new StudyTopic(r)).ToList();
    }   
    
    public async Task<StudyTopic?> GetStudyTopic(int id){ 
        var studyTopInDb = await _studyRepository.GetStudyTopic(id);
        return studyTopInDb == null ? null : new StudyTopic(studyTopInDb);
        
    }                  

    // Update data
    public async Task<StudyTopic?> CreateStudyTopic(StudyTopic stTopicContent){ 
        var stTopicContentInDb = new StudyTopicInDb(stTopicContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyTopic(stTopicContentInDb);
        return res == null ? null : new StudyTopic(res);
    } 
    
    public async  Task<StudyTopic?> UpdateStudyTopic(StudyTopic stTopicContent){ 
        var stTopicContentInDb = new StudyTopicInDb(stTopicContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyTopic(stTopicContentInDb);
        return res == null ? null : new StudyTopic(res);
    }  
    
    public async Task<int> DeleteStudyTopic(int id) 
           => await _studyRepository.DeleteStudyTopic(id, _userName);
    
  
    /****************************************************************
    * Study Relationships
    ****************************************************************/
      
    // Fetch data

    public async Task<List<StudyRelationship>?> GetStudyRelationships(string sdSid){
        var relationshipsInDb = (await _studyRepository.GetStudyRelationships(sdSid)).ToList();
        return !relationshipsInDb.Any() ? null 
            : relationshipsInDb.Select(r => new StudyRelationship(r)).ToList();
    }    
    
    public async Task<StudyRelationship?> GetStudyRelationship(int id){ 
        var studyRelInDb = await _studyRepository.GetStudyRelationship(id);
        return studyRelInDb == null ? null : new StudyRelationship(studyRelInDb);
    }       
    
    // Update data
    
    public async Task<StudyRelationship?> CreateStudyRelationship(StudyRelationship stRelContent){  
        var stRelContentInDb = new StudyRelationshipInDb(stRelContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyRelationship(stRelContentInDb);
        return res == null ? null : new StudyRelationship(res);
    } 
    
    public async Task<StudyRelationship?> UpdateStudyRelationship(StudyRelationship stRelContent){ 
        var stRelContentInDb = new StudyRelationshipInDb(stRelContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyRelationship(stRelContentInDb);
        return res == null ? null : new StudyRelationship(res);
    }   
    
    public async Task<int> DeleteStudyRelationship(int id)  
           => await _studyRepository.DeleteStudyRelationship(id, _userName);

}
