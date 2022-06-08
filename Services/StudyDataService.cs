using rmsbe.SysModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Services;

public class StudyDataService : IStudyDataService
{
    private readonly IStudyRepository _studyRepository;
    private string _user_name;

    public StudyDataService(IStudyRepository studyRepository)
    {
        _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        _user_name = "test user"; // for now - need a mechanism to inject this from user object;
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/
    
    // Check if study exists 
    public async Task<bool> StudyDoesNotExistAsync(string sd_sid)
        => await _studyRepository.StudyDoesNotExistAsync(sd_sid);

    // Check if attribute exists on this study
    public async Task<bool> StudyAttributeDoesNotExistAsync (string sd_sid, string type_name, int id)
         => await _studyRepository.StudyAttributeDoesNotExistAsync(sd_sid,type_name, id); 
    
    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyData>?> GetStudyRecordsDataAsync(){ 
        var studiesInDb = await _studyRepository.GetStudiesDataAsync();
        return studiesInDb?.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<List<StudyData>?> GetRecentStudyRecordsAsync(int n){ 
        var recentStudiesInDb = await _studyRepository.GetRecentStudyDataAsync(n);
        return recentStudiesInDb?.Select(r => new StudyData(r)).ToList();
    }
    
    public async Task<StudyData?> GetStudyRecordDataAsync(string sd_sid){ 
        StudyInDb? studyInDb = await _studyRepository.GetStudyDataAsync(sd_sid);
        return studyInDb == null ? null : new StudyData(studyInDb);
    }
    
    // Update data
    
    public async Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent){ 
        var stInDb = new StudyInDb(studyDataContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyDataAsync(stInDb);
        return res == null ? null : new StudyData(res);
    }
    
    public async Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent){ 
        var stInDb = new StudyInDb(studyDataContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyDataAsync(stInDb);
        return res == null ? null : new StudyData(res);
    }
    
   
    public async Task<int> DeleteStudyRecordDataAsync(string sd_sid) 
        => await _studyRepository.DeleteStudyDataAsync(sd_sid, _user_name);

    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<FullStudy>?> GetAllFullStudiesAsync(){ 
        var fullStudiesInDb = await _studyRepository.GetAllFullStudiesAsync();
        return fullStudiesInDb?.Select(r => new FullStudy(r)).ToList();
    } 
    
    public async Task<FullStudy?> GetFullStudyByIdAsync(string sd_sid){ 
        FullStudyInDb? fullStudyInDb = await _studyRepository.GetFullStudyByIdAsync(sd_sid);
        return fullStudyInDb == null ? null : new FullStudy(fullStudyInDb);
    }
    
    // Update data
    
    public async Task<int> DeleteFullStudyAsync(string sd_sid) 
        => await _studyRepository.DeleteFullStudyAsync(sd_sid, _user_name);
    
       
    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyIdentifier>?> GetStudyIdentifiersAsync(string sd_oid){ 
        var identifiersInDb = await _studyRepository.GetStudyIdentifiersAsync(sd_oid);
        return identifiersInDb?.Select(r => new StudyIdentifier(r)).ToList();
    }   
    
    public async Task<StudyIdentifier?> GetStudyIdentifierAsync(int id){ 
        StudyIdentifierInDb? studyIdentInDb = await _studyRepository.GetStudyIdentifierAsync(id);
        return studyIdentInDb == null ? null : new StudyIdentifier(studyIdentInDb);
    }   
    
    // Update data
    
    public async Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent){ 
        var stIdentContentInDb = new StudyIdentifierInDb(stIdentContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyIdentifierAsync(stIdentContentInDb);
        return res == null ? null : new StudyIdentifier(res);
    } 
    
    public async Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int id, StudyIdentifier stIdentContent){ 
        var stIdentContentInDb = new StudyIdentifierInDb(stIdentContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyIdentifierAsync(stIdentContentInDb);
        return res == null ? null : new StudyIdentifier(res);
    }  
    
    public async Task<int> DeleteStudyIdentifierAsync(int id) 
        => await _studyRepository.DeleteStudyIdentifierAsync(id, _user_name);

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    
    public  async Task<List<StudyTitle>?> GetStudyTitlesAsync(string sd_oid){ 
        var titlesInDb = await _studyRepository.GetStudyTitlesAsync(sd_oid);
        return titlesInDb?.Select(r => new StudyTitle(r)).ToList();
    }  
    
    public async Task<StudyTitle?> GetStudyTitleAsync(int id){ 
        StudyTitleInDb? studyTitleInDb = await _studyRepository.GetStudyTitleAsync(id);
        return studyTitleInDb == null ? null : new StudyTitle(studyTitleInDb);
    }     

    // Update data
    
    public async Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent){ 
        var stTitleContentInDb = new StudyTitleInDb(stTitleContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyTitleAsync(stTitleContentInDb);
        return res == null ? null : new StudyTitle(res);
    } 
    
    public async Task<StudyTitle?> UpdateStudyTitleAsync(int id, StudyTitle stTitleContent){ 
        var stTitleContentInDb = new StudyTitleInDb(stTitleContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyTitleAsync(stTitleContentInDb);
        return res == null ? null : new StudyTitle(res);
    }   

    
    public async Task<int> DeleteStudyTitleAsync(int id) 
        => await _studyRepository.DeleteStudyTitleAsync(id, _user_name);
 
    /****************************************************************
    * Study contributors
    ****************************************************************/   
    
    // Fetch data
    
    public async Task<List<StudyContributor>?> GetStudyContributorsAsync(string sd_oid){ 
        var contributorsInDb = await _studyRepository.GetStudyContributorsAsync(sd_oid);
        return contributorsInDb?.Select(r => new StudyContributor(r)).ToList();
    }   
    
    public async Task<StudyContributor?> GetStudyContributorAsync(int id){ 
        StudyContributorInDb? studyContInDb = await _studyRepository.GetStudyContributorAsync(id);
        return studyContInDb == null ? null : new StudyContributor(studyContInDb);
    }  
    
    // Update data
    
    public async Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent){  
        var stContContentInDb = new StudyContributorInDb(stContContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyContributorAsync(stContContentInDb);
        return res == null ? null : new StudyContributor(res);
    } 
    
    public async Task<StudyContributor?> UpdateStudyContributorAsync(int id, StudyContributor stContContent){ 
        var stContContentInDb = new StudyContributorInDb(stContContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyContributorAsync(stContContentInDb);
        return res == null ? null : new StudyContributor(res);
    }   

    public async Task<int> DeleteStudyContributorAsync(int id) 
        => await _studyRepository.DeleteStudyContributorAsync(id, _user_name);

  
    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyFeature>?> GetStudyFeaturesAsync(string sd_oid){ 
        var featuresInDb = await _studyRepository.GetStudyFeaturesAsync(sd_oid);
        return featuresInDb?.Select(r => new StudyFeature(r)).ToList();
    }  
    
    public async Task<StudyFeature?> GetStudyFeatureAsync(int id){ 
        StudyFeatureInDb? studyFeatInDb = await _studyRepository.GetStudyFeatureAsync(id);
        return studyFeatInDb == null ? null : new StudyFeature(studyFeatInDb);
    }            
    
    // Update data
    
    public async Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent){ 
        var stFeatureContentInDb = new StudyFeatureInDb(stFeatureContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyFeatureAsync(stFeatureContentInDb);
        return res == null ? null : new StudyFeature(res);
    } 
    
   public async Task<StudyFeature?> UpdateStudyFeatureAsync(int id, StudyFeature stFeatureContent){ 
       var stFeatureContentInDb = new StudyFeatureInDb(stFeatureContent)
       {
           id = id,
           last_edited_by = _user_name
       };
       var res = await _studyRepository.UpdateStudyFeatureAsync(stFeatureContentInDb);
       return res == null ? null : new StudyFeature(res);
    }    
  
    
    public async Task<int> DeleteStudyFeatureAsync(int id) 
        => await _studyRepository.DeleteStudyFeatureAsync(id, _user_name);

  
    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyTopic>?> GetStudyTopicsAsync(string sd_oid){ 
        var topicsInDb = await _studyRepository.GetStudyTopicsAsync(sd_oid);
        return topicsInDb?.Select(r => new StudyTopic(r)).ToList();
    }   
    
    public async Task<StudyTopic?> GetStudyTopicAsync(int id){ 
        StudyTopicInDb? studyTopInDb = await _studyRepository.GetStudyTopicAsync(id);
        return studyTopInDb == null ? null : new StudyTopic(studyTopInDb);
        
    }                  

    // Update data
    public async Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent){ 
        var stTopicContentInDb = new StudyTopicInDb(stTopicContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyTopicAsync(stTopicContentInDb);
        return res == null ? null : new StudyTopic(res);
    } 
    
    public async  Task<StudyTopic?> UpdateStudyTopicAsync(int id, StudyTopic stTopicContent){ 
        var stTopicContentInDb = new StudyTopicInDb(stTopicContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyTopicAsync(stTopicContentInDb);
        return res == null ? null : new StudyTopic(res);
    }  

    
    public async Task<int> DeleteStudyTopicAsync(int id) 
        => await _studyRepository.DeleteStudyTopicAsync(id, _user_name);
  
    /****************************************************************
    * Study Relationships
    ****************************************************************/
      
    // Fetch data

    public async Task<List<StudyRelationship>?> GetStudyRelationshipsAsync(string sd_oid){
        var relationshipsInDb = await _studyRepository.GetStudyRelationshipsAsync(sd_oid);
        return relationshipsInDb?.Select(r => new StudyRelationship(r)).ToList();
    }    
    
    public async Task<StudyRelationship?> GetStudyRelationshipAsync(int id){ 
        StudyRelationshipInDb? studyRelInDb = await _studyRepository.GetStudyRelationshipAsync(id);
        return studyRelInDb == null ? null : new StudyRelationship(studyRelInDb);
    }       
    
    // Update data
    
    public async Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent){  
        var stRelContentInDb = new StudyRelationshipInDb(stRelContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyRelationshipAsync(stRelContentInDb);
        return res == null ? null : new StudyRelationship(res);
    } 
    
    public async Task<StudyRelationship?> UpdateStudyRelationshipAsync(int id, StudyRelationship stRelContent){ 
        var stRelContentInDb = new StudyRelationshipInDb(stRelContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyRelationshipAsync(stRelContentInDb);
        return res == null ? null : new StudyRelationship(res);
    }   
    
    public async Task<int> DeleteStudyRelationshipAsync(int id)  
        => await _studyRepository.DeleteStudyRelationshipAsync(id, _user_name);

    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    
    public async Task<List<StudyReference>?> GetStudyReferencesAsync(string sd_oid)
    {
        var referencesInDb = await _studyRepository.GetStudyReferencesAsync(sd_oid);
        return referencesInDb?.Select(r => new StudyReference(r)).ToList();
    }     

    public async Task<StudyReference?> GetStudyReferenceAsync(int id)
    { 
        StudyReferenceInDb? studyRefInDb = await _studyRepository.GetStudyReferenceAsync(id);
        return studyRefInDb == null ? null : new StudyReference(studyRefInDb);
    }                   
    
    // Update data
    
    public async Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent)
    {
        var stRefContentInDb = new StudyReferenceInDb(stRefContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyReferenceAsync(stRefContentInDb);
        return res == null ? null : new StudyReference(res);
    } 

    public async Task<StudyReference?> UpdateStudyReferenceAsync(int id, StudyReference stRefContent)
    {
        var stRefContentInDb = new StudyReferenceInDb(stRefContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyReferenceAsync(stRefContentInDb);
        return res == null ? null : new StudyReference(res);
    } 
    
    public async Task<int> DeleteStudyReferenceAsync(int id) 
        => await _studyRepository.DeleteStudyReferenceAsync(id, _user_name);
}
