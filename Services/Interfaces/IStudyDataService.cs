using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IStudyDataService
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/
 
    // Check if study exists
    Task<bool> StudyDoesNotExist (string sd_sid); 
    // Check if attribute exists on this study
    Task<bool> StudyAttributeDoesNotExist (string sd_sid, string type_name, int id); 

    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
      
    // Fetch data
    Task<List<StudyData>?> GetStudyRecordsDataAsync();
    Task<List<StudyData>?> GetRecentStudyRecordsAsync(int n);
    Task<StudyData?> GetStudyRecordDataAsync(string sd_sid);
    // Update data
    Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent, string? accessToken);
    Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent, string? accessToken);
    Task<int> DeleteStudyRecordDataAsync(string sd_sid);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    Task<List<Study>?> GetFullStudyDataAsync(); 
    Task<Study?> GetFullStudyByIdAsync(string sd_sid);
    // Update data
    Task<Study?> CreateFullStudyAsync(Study studyContent, string? accessToken);
    Task<Study?> UpdateFullStudyAsync(Study studyContent, string? accessToken);
    Task<int> DeleteFullStudyAsync(string sd_sid);
        
    /****************************************************************
    * Study identifiers
    ****************************************************************/

    // Fetch data
    Task<List<StudyIdentifier>?> GetStudyIdentifiersAsync(string sd_oid);     
    Task<StudyIdentifier?> GetStudyIdentifierAsync(int id);                  
    // Update data
    Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent, string? accessToken); 
    Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int id, StudyIdentifier stIdentContent, string? accessToken);    
    Task<int> DeleteStudyIdentifierAsync(int id);  
    
    /****************************************************************
    * Study titles
    ****************************************************************/  
 
    // Fetch data
    Task<List<StudyTitle>?> GetStudyTitlesAsync(string sd_oid);    
    Task<StudyTitle?> GetStudyTitleAsync(int id);                 
    // Update data
    Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent, string? accessToken); 
    Task<StudyTitle?> UpdateStudyTitleAsync(int id, StudyTitle stTitleContent, string? accessToken);    
    Task<int> DeleteStudyTitleAsync(int id);   
    
    /****************************************************************
    * Study contributors
    ****************************************************************/   

    // Fetch data
    Task<List<StudyContributor>?> GetStudyContributorsAsync(string sd_oid);  
    Task<StudyContributor?> GetStudyContributorAsync(int id);                 
    // Update data
    Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent, string? accessToken); 
    Task<StudyContributor?> UpdateStudyContributorAsync(int id, StudyContributor stContContent, string? accessToken);    
    Task<int> DeleteStudyContributorAsync(int id);  
        
    /****************************************************************
    * Study features
    ****************************************************************/

    // Fetch data
    Task<List<StudyFeature>?> GetStudyFeaturesAsync(string sd_oid);    
    Task<StudyFeature?> GetStudyFeatureAsync(int id);                
    // Update data
    Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent, string? accessToken); 
    Task<StudyFeature?> UpdateStudyFeatureAsync(int id, StudyFeature stFeatureContent, string? accessToken);    
    Task<int> DeleteStudyFeatureAsync(int id);  

    /****************************************************************
    * Study topics
    ****************************************************************/

    // Fetch data
    Task<List<StudyTopic>?> GetStudyTopicsAsync(string sd_oid);   
    Task<StudyTopic?> GetStudyTopicAsync(int id);                 
    // Update data
    Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent, string? accessToken); 
    Task<StudyTopic?> UpdateStudyTopicAsync(int id, StudyTopic stTopicContent, string? accessToken);    
    Task<int> DeleteStudyTopicAsync(int id);  

    /****************************************************************
    * Study Relationships
    ****************************************************************/

    // Fetch data
    Task<List<StudyRelationship>?> GetStudyRelationshipsAsync(string sd_oid);    
    Task<StudyRelationship?> GetStudyRelationshipAsync(int id);               
    // Update data
    Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent, string? accessToken); 
    Task<StudyRelationship?> UpdateStudyRelationshipAsync(int id, StudyRelationship stRelContent, string? accessToken);    
    Task<int> DeleteStudyRelationshipAsync(int id);   

    /****************************************************************
    * Study References
    ****************************************************************/

    // Fetch data
    Task<List<StudyReference>?> GetStudyReferencesAsync(string sd_oid);  
    Task<StudyReference?> GetStudyReferenceAsync(int id);             
    // Update data
    Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent, string? accessToken); 
    Task<StudyReference?> UpdateStudyReferenceAsync(int id, StudyReference stRefContent, string? accessToken);    
    Task<int> DeleteStudyReferenceAsync(int id);    
}