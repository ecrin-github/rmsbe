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
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/
    
    // Check if study exists 
    public async Task<bool> StudyExistsAsync(string sdSid)
           => await _studyRepository.StudyExistsAsync(sdSid);

    // Check if attribute exists on this study
    public async Task<bool> StudyAttributeExistsAsync (string sdSid, string typeName, int id)
        => await _studyRepository.StudyAttributeExistsAsync(sdSid,typeName, id); 
    
    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyData>?> GetStudyRecordsDataAsync(){ 
        var studiesInDb = (await _studyRepository.GetStudiesDataAsync()).ToList();
        return !studiesInDb.Any() ? null 
            : studiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyData>?> GetRecentStudyRecordsAsync(int n){ 
        var recentStudiesInDb = (await _studyRepository.GetRecentStudyDataAsync(n)).ToList();
        return !recentStudiesInDb.Any() ? null 
            : recentStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyData>?> GetPaginatedStudyDataAsync(PaginationRequest validFilter)
    {
        var pagedStudiesInDb = (await _studyRepository
            .GetPaginatedStudyDataAsync(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedStudiesInDb.Any() ? null 
            : pagedStudiesInDb.Select(r => new StudyData(r)).ToList();
    }

    public async Task<List<StudyData>?> GetPaginatedFilteredStudyRecordsAsync(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredStudiesInDb = (await _studyRepository
            .GetPaginatedFilteredStudyDataAsync(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredStudiesInDb.Any() ? null 
            : pagedFilteredStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyData>?> GetFilteredStudyRecordsAsync(string titleFilter)
    {
        var filteredStudiesInDb = (await _studyRepository
            .GetFilteredStudyDataAsync(titleFilter)).ToList();
        return !filteredStudiesInDb.Any() ? null 
            : filteredStudiesInDb.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<StudyData?> GetStudyRecordDataAsync(string sdSid){ 
        var studyInDb = await _studyRepository.GetStudyDataAsync(sdSid);
        return studyInDb == null ? null : new StudyData(studyInDb);
    }

    
    // Update data
    
    public async Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent){ 
        var stInDb = new StudyInDb(studyDataContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyDataAsync(stInDb);
        return res == null ? null : new StudyData(res);
    }
    
    public async Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent){ 
        var stInDb = new StudyInDb(studyDataContent) { last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyDataAsync(stInDb);
        return res == null ? null : new StudyData(res);
    }
    
    public async Task<int> DeleteStudyRecordDataAsync(string sdSid) 
           => await _studyRepository.DeleteStudyDataAsync(sdSid, _userName);
    
    
    /****************************************************************
    * Study Entries (fetching lists of id, sd_sid, display name only)
    ****************************************************************/
    
    public async Task<List<StudyEntry>?> GetStudyEntriesAsync(){ 
        var studiesInDb = (await _studyRepository.GetStudyEntriesAsync()).ToList();
        return !studiesInDb.Any() ? null 
            : studiesInDb.Select(r => new StudyEntry(r)).ToList();
    }
    
    public async Task<List<StudyEntry>?> GetRecentStudyEntriesAsync(int n){ 
        var recentStudiesInDb = (await _studyRepository.GetRecentStudyEntriesAsync(n)).ToList();
        return !recentStudiesInDb.Any() ? null 
            : recentStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }
    
    public async Task<List<StudyEntry>?> GetPaginatedStudyEntriesAsync(PaginationRequest validFilter)
    {
        var pagedStudiesInDb = (await _studyRepository
            .GetPaginatedStudyEntriesAsync(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedStudiesInDb.Any() ? null 
            : pagedStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }

    public async Task<List<StudyEntry>?> GetPaginatedFilteredStudyEntriesAsync(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredStudiesInDb = (await _studyRepository
            .GetPaginatedFilteredStudyEntriesAsync(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredStudiesInDb.Any() ? null 
            : pagedFilteredStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }
    
    public async Task<List<StudyEntry>?> GetFilteredStudyEntriesAsync(string titleFilter)
    {
        var filteredStudiesInDb = (await _studyRepository
            .GetFilteredStudyEntriesAsync(titleFilter)).ToList();
        return !filteredStudiesInDb.Any() ? null 
            : filteredStudiesInDb.Select(r => new StudyEntry(r)).ToList();
    }

    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    
    public async Task<FullStudy?> GetFullStudyByIdAsync(string sdSid){ 
        FullStudyInDb? fullStudyInDb = await _studyRepository.GetFullStudyByIdAsync(sdSid);
        return fullStudyInDb == null ? null : new FullStudy(fullStudyInDb);
    }
    
    // Update data
    
    public async Task<int> DeleteFullStudyAsync(string sdSid) 
           => await _studyRepository.DeleteFullStudyAsync(sdSid, _userName);
    
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
        if (await ResetLookupsAsync("study-types"))
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

    private async Task<bool> ResetLookupsAsync(string typeName)
    {
        _lookups = new List<Lup>();  // reset to empty list
        _lookups = await _lupService.GetLookUpValuesAsync(typeName);
        return _lookups.Count > 0 ;
    }

       
    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyIdentifier>?> GetStudyIdentifiersAsync(string sdOid){ 
        var identifiersInDb = (await _studyRepository.GetStudyIdentifiersAsync(sdOid)).ToList();
        return (!identifiersInDb.Any()) ? null 
            : identifiersInDb.Select(r => new StudyIdentifier(r)).ToList();
    }   
    
    public async Task<StudyIdentifier?> GetStudyIdentifierAsync(int id){ 
        var studyIdentInDb = await _studyRepository.GetStudyIdentifierAsync(id);
        return studyIdentInDb == null ? null : new StudyIdentifier(studyIdentInDb);
    }   
    
    // Update data
    
    public async Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent){ 
        var stIdentContentInDb = new StudyIdentifierInDb(stIdentContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyIdentifierAsync(stIdentContentInDb);
        return res == null ? null : new StudyIdentifier(res);
    } 
    
    public async Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int aId, StudyIdentifier stIdentContent){ 
        var stIdentContentInDb = new StudyIdentifierInDb(stIdentContent) { id = aId, last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyIdentifierAsync(stIdentContentInDb);
        return res == null ? null : new StudyIdentifier(res);
    }  
    
    public async Task<int> DeleteStudyIdentifierAsync(int id) 
           => await _studyRepository.DeleteStudyIdentifierAsync(id, _userName);
    

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    
    public  async Task<List<StudyTitle>?> GetStudyTitlesAsync(string sdOid){ 
        var titlesInDb = (await _studyRepository.GetStudyTitlesAsync(sdOid)).ToList();
        return (!titlesInDb.Any()) ? null 
            : titlesInDb.Select(r => new StudyTitle(r)).ToList();
    }  
    
    public async Task<StudyTitle?> GetStudyTitleAsync(int id){ 
        var studyTitleInDb = await _studyRepository.GetStudyTitleAsync(id);
        return studyTitleInDb == null ? null : new StudyTitle(studyTitleInDb);
    }     

    // Update data
    
    public async Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent){ 
        var stTitleContentInDb = new StudyTitleInDb(stTitleContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyTitleAsync(stTitleContentInDb);
        return res == null ? null : new StudyTitle(res);
    } 
    
    public async Task<StudyTitle?> UpdateStudyTitleAsync(int aId, StudyTitle stTitleContent){ 
        var stTitleContentInDb = new StudyTitleInDb(stTitleContent) { id = aId, last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyTitleAsync(stTitleContentInDb);
        return res == null ? null : new StudyTitle(res);
    }   
    
    public async Task<int> DeleteStudyTitleAsync(int id) 
           => await _studyRepository.DeleteStudyTitleAsync(id, _userName);
    
 
    /****************************************************************
    * Study contributors
    ****************************************************************/   
    
    // Fetch data
    
    public async Task<List<StudyContributor>?> GetStudyContributorsAsync(string sdOid){ 
        var contributorsInDb = (await _studyRepository.GetStudyContributorsAsync(sdOid)).ToList();
        return (!contributorsInDb.Any()) ? null 
                       : contributorsInDb.Select(r => new StudyContributor(r)).ToList();
    }   
    
    public async Task<StudyContributor?> GetStudyContributorAsync(int id){ 
        var studyContInDb = await _studyRepository.GetStudyContributorAsync(id);
        return studyContInDb == null ? null : new StudyContributor(studyContInDb);
    }  
    
    // Update data
    
    public async Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent){  
        var stContContentInDb = new StudyContributorInDb(stContContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyContributorAsync(stContContentInDb);
        return res == null ? null : new StudyContributor(res);
    } 
    
    public async Task<StudyContributor?> UpdateStudyContributorAsync(int aId, StudyContributor stContContent){ 
        var stContContentInDb = new StudyContributorInDb(stContContent) { id = aId, last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyContributorAsync(stContContentInDb);
        return res == null ? null : new StudyContributor(res);
    }   

    public async Task<int> DeleteStudyContributorAsync(int id) 
           => await _studyRepository.DeleteStudyContributorAsync(id, _userName);

  
    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyFeature>?> GetStudyFeaturesAsync(string sdOid){ 
        var featuresInDb = (await _studyRepository.GetStudyFeaturesAsync(sdOid)).ToList();
        return (!featuresInDb.Any()) ? null 
            : featuresInDb.Select(r => new StudyFeature(r)).ToList();
    }  
    
    public async Task<StudyFeature?> GetStudyFeatureAsync(int id){ 
        var studyFeatInDb = await _studyRepository.GetStudyFeatureAsync(id);
        return studyFeatInDb == null ? null : new StudyFeature(studyFeatInDb);
    }            
    
    // Update data
    
    public async Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent){ 
        var stFeatureContentInDb = new StudyFeatureInDb(stFeatureContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyFeatureAsync(stFeatureContentInDb);
        return res == null ? null : new StudyFeature(res);
    } 
    
   public async Task<StudyFeature?> UpdateStudyFeatureAsync(int aId, StudyFeature stFeatureContent){ 
       var stFeatureContentInDb = new StudyFeatureInDb(stFeatureContent) { id = aId, last_edited_by = _userName };
       var res = await _studyRepository.UpdateStudyFeatureAsync(stFeatureContentInDb);
       return res == null ? null : new StudyFeature(res);
    }    
  
    public async Task<int> DeleteStudyFeatureAsync(int id) 
           => await _studyRepository.DeleteStudyFeatureAsync(id, _userName);

  
    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyTopic>?> GetStudyTopicsAsync(string sdOid){ 
        var topicsInDb = (await _studyRepository.GetStudyTopicsAsync(sdOid)).ToList();
        return (!topicsInDb.Any()) ? null 
            : topicsInDb.Select(r => new StudyTopic(r)).ToList();
    }   
    
    public async Task<StudyTopic?> GetStudyTopicAsync(int id){ 
        var studyTopInDb = await _studyRepository.GetStudyTopicAsync(id);
        return studyTopInDb == null ? null : new StudyTopic(studyTopInDb);
        
    }                  

    // Update data
    public async Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent){ 
        var stTopicContentInDb = new StudyTopicInDb(stTopicContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyTopicAsync(stTopicContentInDb);
        return res == null ? null : new StudyTopic(res);
    } 
    
    public async  Task<StudyTopic?> UpdateStudyTopicAsync(int aId, StudyTopic stTopicContent){ 
        var stTopicContentInDb = new StudyTopicInDb(stTopicContent) { id = aId, last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyTopicAsync(stTopicContentInDb);
        return res == null ? null : new StudyTopic(res);
    }  
    
    public async Task<int> DeleteStudyTopicAsync(int id) 
           => await _studyRepository.DeleteStudyTopicAsync(id, _userName);
    
  
    /****************************************************************
    * Study Relationships
    ****************************************************************/
      
    // Fetch data

    public async Task<List<StudyRelationship>?> GetStudyRelationshipsAsync(string sdOid){
        var relationshipsInDb = (await _studyRepository.GetStudyRelationshipsAsync(sdOid)).ToList();
        return !relationshipsInDb.Any() ? null 
            : relationshipsInDb.Select(r => new StudyRelationship(r)).ToList();
    }    
    
    public async Task<StudyRelationship?> GetStudyRelationshipAsync(int id){ 
        var studyRelInDb = await _studyRepository.GetStudyRelationshipAsync(id);
        return studyRelInDb == null ? null : new StudyRelationship(studyRelInDb);
    }       
    
    // Update data
    
    public async Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent){  
        var stRelContentInDb = new StudyRelationshipInDb(stRelContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyRelationshipAsync(stRelContentInDb);
        return res == null ? null : new StudyRelationship(res);
    } 
    
    public async Task<StudyRelationship?> UpdateStudyRelationshipAsync(int aId, StudyRelationship stRelContent){ 
        var stRelContentInDb = new StudyRelationshipInDb(stRelContent) { id = aId, last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyRelationshipAsync(stRelContentInDb);
        return res == null ? null : new StudyRelationship(res);
    }   
    
    public async Task<int> DeleteStudyRelationshipAsync(int id)  
           => await _studyRepository.DeleteStudyRelationshipAsync(id, _userName);

    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyReference>?> GetStudyReferencesAsync(string sdOid){
        var referencesInDb = (await _studyRepository.GetStudyReferencesAsync(sdOid)).ToList();
        return !referencesInDb.Any() ? null 
            : referencesInDb.Select(r => new StudyReference(r)).ToList();
    }     

    public async Task<StudyReference?> GetStudyReferenceAsync(int id){ 
        var studyRefInDb = await _studyRepository.GetStudyReferenceAsync(id);
        return studyRefInDb == null ? null : new StudyReference(studyRefInDb);
    }                   
    
    // Update data
    
    public async Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent){
        var stRefContentInDb = new StudyReferenceInDb(stRefContent) { last_edited_by = _userName };
        var res = await _studyRepository.CreateStudyReferenceAsync(stRefContentInDb);
        return res == null ? null : new StudyReference(res);
    } 

    public async Task<StudyReference?> UpdateStudyReferenceAsync(int aId, StudyReference stRefContent){
        var stRefContentInDb = new StudyReferenceInDb(stRefContent) { id = aId, last_edited_by = _userName };
        var res = await _studyRepository.UpdateStudyReferenceAsync(stRefContentInDb);
        return res == null ? null : new StudyReference(res);
    } 
    
    public async Task<int> DeleteStudyReferenceAsync(int id) 
           => await _studyRepository.DeleteStudyReferenceAsync(id, _userName);
 
}
