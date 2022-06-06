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
        _user_name = "test user"; // for now;
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
    public async Task<bool> StudyAttributeDoesNotExist (string sd_sid, string type_name, int id)
         => await _studyRepository.StudyAttributeDoesNotExist(sd_sid,type_name, id); 
    
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
    /*
    // Update data
    
    public async Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
    public async Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
    */
    public async Task<int> DeleteStudyRecordDataAsync(string sd_sid) => await _studyRepository.DeleteStudyDataAsync(sd_sid);

    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    /*
    public async Task<List<Study>?> GetFullStudyDataAsync(){ 
        var fullStudiesInDb = await _studyRepository.GetStudyReferencesAsync(sd_oid);
        return fullStudiesInDb?.Select(r => new Study(r)).ToList();
    } 
    
    public async Task<Study?> GetFullStudyByIdAsync(string sd_sid){ 
        StudyInDb? studyRefInDb = await _studyRepository.GetStudyReferenceAsync(id);
        return studyRefInDb == null ? null : new StudyReference(studyRefInDb);
    }
    
    // Update data
    
    public async Task<Study?> CreateFullStudyAsync(Study studyContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
   public async Task<Study?> UpdateFullStudyAsync(Study studyContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    */
    public async Task<int> DeleteFullStudyAsync(string sd_sid) => await _studyRepository.DeleteFullStudy(sd_sid);
    
       
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
    
    /*
    // Update data
    public async Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    public async Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int id, StudyIdentifier stIdentContent, string? accessToken){ 
        throw new NotImplementedException();
    }  
    */
    
    public async Task<int> DeleteStudyIdentifierAsync(int id) => await _studyRepository.DeleteStudyIdentifierAsync(id);

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
    /*
    // Update data
    
    public async Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    public async Task<StudyTitle?> UpdateStudyTitleAsync(int id, StudyTitle stTitleContent, string? accessToken){ 
        throw new NotImplementedException();
    }   
   */
    
    public async Task<int> DeleteStudyTitleAsync(int id) => await _studyRepository.DeleteStudyTitleAsync(id);
 
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
    /*
    // Update data
    
    public async Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent, string? accessToken){  
        throw new NotImplementedException();
    } 
    
    public async Task<StudyContributor?> UpdateStudyContributorAsync(int id, StudyContributor stContContent, string? accessToken){ 
        throw new NotImplementedException();
    }   
    */
    public async Task<int> DeleteStudyContributorAsync(int id) => await _studyRepository.DeleteStudyContributorAsync(id);

  
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
    /*
    // Update data
    
    public Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
   public  Task<StudyFeature?> UpdateStudyFeatureAsync(int id, StudyFeature stFeatureContent, string? accessToken){ 
        throw new NotImplementedException();
    }    
      */
    
    public async Task<int> DeleteStudyFeatureAsync(int id) => await _studyRepository.DeleteStudyFeatureAsync(id);

  
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
    /*
    // Update data
    Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    Task<StudyTopic?> UpdateStudyTopicAsync(int id, StudyTopic stTopicContent, string? accessToken){ 
        throw new NotImplementedException();
    }  
      */
    
    public async Task<int> DeleteStudyTopicAsync(int id) => await _studyRepository.DeleteStudyTopicAsync(id);
  
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
    /*
    public Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent, string? accessToken){  
        throw new NotImplementedException();
    } 
    
    public Task<StudyRelationship?> UpdateStudyRelationshipAsync(int id, StudyRelationship stRelContent, string? accessToken){ 
        throw new NotImplementedException();
    }   
    */
    
    public async Task<int> DeleteStudyRelationshipAsync(int id)  => await _studyRepository.DeleteStudyRelationshipAsync(id);

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
    
    public async Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent, string? accessToken)
    {
        var stRefContentInDb = new StudyReferenceInDb(stRefContent)
        {
            last_edited_by = _user_name
        };
        var res = await _studyRepository.CreateStudyReferenceAsync(stRefContentInDb, accessToken);
        return res == null ? null : new StudyReference(res);
    } 

    public async Task<StudyReference?> UpdateStudyReferenceAsync(int id, StudyReference stRefContent, string? accessToken)
    {
        var stRefContentInDb = new StudyReferenceInDb(stRefContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _studyRepository.UpdateStudyReferenceAsync(stRefContentInDb, accessToken);
        return res == null ? null : new StudyReference(res);
    } 
    
    public async Task<int> DeleteStudyReferenceAsync(int id) => await _studyRepository.DeleteStudyReferenceAsync(id);
}
