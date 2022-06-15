using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IStudyService
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/
 
    // Check if study exists
    Task<bool> StudyDoesNotExistAsync (string sd_sid);
    Task<bool> StudyExistsAsync(string sd_sid);
    
    // Check if attribute exists on this study
    Task<bool> StudyAttributeDoesNotExistAsync (string sd_sid, string type_name, int id); 
    Task<bool> StudyAttributeExistsAsync (string sd_sid, string type_name, int id); 

    /****************************************************************
    * Study Record (study data only, no attributes)
    ****************************************************************/
      
    // Fetch data
    Task<List<StudyData>?> GetStudyRecordsDataAsync();
    Task<List<StudyData>?> GetRecentStudyRecordsAsync(int n);
    Task<StudyData?> GetStudyRecordDataAsync(string sd_sid);
    // Update data
    Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent);
    Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent);
    Task<int> DeleteStudyRecordDataAsync(string sd_sid);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    Task<FullStudy?> GetFullStudyByIdAsync(string sd_sid);
    // Update data
    Task<int> DeleteFullStudyAsync(string sd_sid);
        
    /****************************************************************
    * Study identifiers
    ****************************************************************/

    // Fetch data
    Task<List<StudyIdentifier>?> GetStudyIdentifiersAsync(string sd_oid);     
    Task<StudyIdentifier?> GetStudyIdentifierAsync(int id);                  
    // Update data
    Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent); 
    Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int id, StudyIdentifier stIdentContent);    
    Task<int> DeleteStudyIdentifierAsync(int id);  
    
    /****************************************************************
    * Study titles
    ****************************************************************/  
 
    // Fetch data
    Task<List<StudyTitle>?> GetStudyTitlesAsync(string sd_oid);    
    Task<StudyTitle?> GetStudyTitleAsync(int id);                 
    // Update data
    Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent); 
    Task<StudyTitle?> UpdateStudyTitleAsync(int id, StudyTitle stTitleContent);    
    Task<int> DeleteStudyTitleAsync(int id);   
    
    /****************************************************************
    * Study contributors
    ****************************************************************/   

    // Fetch data
    Task<List<StudyContributor>?> GetStudyContributorsAsync(string sd_oid);  
    Task<StudyContributor?> GetStudyContributorAsync(int id);                 
    // Update data
    Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent); 
    Task<StudyContributor?> UpdateStudyContributorAsync(int id, StudyContributor stContContent);    
    Task<int> DeleteStudyContributorAsync(int id);  
        
    /****************************************************************
    * Study features
    ****************************************************************/

    // Fetch data
    Task<List<StudyFeature>?> GetStudyFeaturesAsync(string sd_oid);    
    Task<StudyFeature?> GetStudyFeatureAsync(int id);                
    // Update data
    Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent); 
    Task<StudyFeature?> UpdateStudyFeatureAsync(int id, StudyFeature stFeatureContent);    
    Task<int> DeleteStudyFeatureAsync(int id);  

    /****************************************************************
    * Study topics
    ****************************************************************/

    // Fetch data
    Task<List<StudyTopic>?> GetStudyTopicsAsync(string sd_oid);   
    Task<StudyTopic?> GetStudyTopicAsync(int id);                 
    // Update data
    Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent); 
    Task<StudyTopic?> UpdateStudyTopicAsync(int id, StudyTopic stTopicContent);    
    Task<int> DeleteStudyTopicAsync(int id);  

    /****************************************************************
    * Study Relationships
    ****************************************************************/

    // Fetch data
    Task<List<StudyRelationship>?> GetStudyRelationshipsAsync(string sd_oid);    
    Task<StudyRelationship?> GetStudyRelationshipAsync(int id);               
    // Update data
    Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent); 
    Task<StudyRelationship?> UpdateStudyRelationshipAsync(int id, StudyRelationship stRelContent);    
    Task<int> DeleteStudyRelationshipAsync(int id);   

    /****************************************************************
    * Study References
    ****************************************************************/

    // Fetch data
    Task<List<StudyReference>?> GetStudyReferencesAsync(string sd_oid);  
    Task<StudyReference?> GetStudyReferenceAsync(int id);             
    // Update data
    Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent); 
    Task<StudyReference?> UpdateStudyReferenceAsync(int id, StudyReference stRefContent);    
    Task<int> DeleteStudyReferenceAsync(int id);    
}