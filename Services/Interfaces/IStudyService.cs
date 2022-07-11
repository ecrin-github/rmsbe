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
    Task<bool> StudyExistsAsync(string sdSid);
    
    // Check if attribute exists on this study
    Task<bool> StudyAttributeExistsAsync (string sdSid, string typeName, int id); 

    /****************************************************************
    * Study Record (study data only, no attributes)
    ****************************************************************/
      
    // Fetch data
    Task<List<StudyData>?> GetStudyRecordsDataAsync();
    Task<List<StudyData>?> GetRecentStudyRecordsAsync(int n);
    Task<List<StudyData>?> GetPaginatedStudyDataAsync(PaginationRequest validFilter);
    Task<List<StudyData>?> GetPaginatedFilteredStudyRecordsAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<StudyData>?> GetFilteredStudyRecordsAsync(string titleFilter);
    
    Task<List<StudyEntry>?> GetStudyEntriesAsync();
    Task<List<StudyEntry>?> GetRecentStudyEntriesAsync(int n);
    Task<List<StudyEntry>?> GetPaginatedStudyEntriesAsync(PaginationRequest validFilter);
    Task<List<StudyEntry>?> GetPaginatedFilteredStudyEntriesAsync(string titleFilter, PaginationRequest validFilter);
    Task<List<StudyEntry>?> GetFilteredStudyEntriesAsync(string titleFilter);
    
    Task<StudyData?>GetStudyRecordDataAsync (string sdSid);
    
    // Update data
    Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent);
    Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent);
    Task<int> DeleteStudyRecordDataAsync(string sdSid);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalStudies();  
    Task<Statistic> GetTotalFilteredStudies(string titleFilter);  
    Task<List<Statistic>?> GetStudiesByType();
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    Task<FullStudy?> GetFullStudyByIdAsync(string sdSid);
    // Update data
    Task<int> DeleteFullStudyAsync(string sdSid);
    
    /****************************************************************
    * Full Study data (including, optionally, attributes in other
    * tables) from the MDR
    ****************************************************************/
    
    // Fetch and store full data
    Task<FullStudyFromMdr?> GetFullStudyFromMdr(int regId, string sdSid);
    // Fetch and store study data
    Task<StudyData?> GetStudyFromMdr(int regId, string sdSid);
    
    /****************************************************************
    * Study identifiers
    ****************************************************************/

    // Fetch data
    Task<List<StudyIdentifier>?> GetStudyIdentifiersAsync(string sdOid);     
    Task<StudyIdentifier?> GetStudyIdentifierAsync(int id);                  
    // Update data
    Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent); 
    Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int id, StudyIdentifier stIdentContent);    
    Task<int> DeleteStudyIdentifierAsync(int id);  
    
    /****************************************************************
    * Study titles
    ****************************************************************/  
 
    // Fetch data
    Task<List<StudyTitle>?> GetStudyTitlesAsync(string sdOid);    
    Task<StudyTitle?> GetStudyTitleAsync(int id);                 
    // Update data
    Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent); 
    Task<StudyTitle?> UpdateStudyTitleAsync(int id, StudyTitle stTitleContent);    
    Task<int> DeleteStudyTitleAsync(int id);   
    
    /****************************************************************
    * Study contributors
    ****************************************************************/   

    // Fetch data
    Task<List<StudyContributor>?> GetStudyContributorsAsync(string sdOid);  
    Task<StudyContributor?> GetStudyContributorAsync(int id);                 
    // Update data
    Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent); 
    Task<StudyContributor?> UpdateStudyContributorAsync(int id, StudyContributor stContContent);    
    Task<int> DeleteStudyContributorAsync(int id);  
        
    /****************************************************************
    * Study features
    ****************************************************************/

    // Fetch data
    Task<List<StudyFeature>?> GetStudyFeaturesAsync(string sdOid);    
    Task<StudyFeature?> GetStudyFeatureAsync(int id);                
    // Update data
    Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent); 
    Task<StudyFeature?> UpdateStudyFeatureAsync(int id, StudyFeature stFeatureContent);    
    Task<int> DeleteStudyFeatureAsync(int id);  

    /****************************************************************
    * Study topics
    ****************************************************************/

    // Fetch data
    Task<List<StudyTopic>?> GetStudyTopicsAsync(string sdOid);   
    Task<StudyTopic?> GetStudyTopicAsync(int id);                 
    // Update data
    Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent); 
    Task<StudyTopic?> UpdateStudyTopicAsync(int id, StudyTopic stTopicContent);    
    Task<int> DeleteStudyTopicAsync(int id);  

    /****************************************************************
    * Study Relationships
    ****************************************************************/

    // Fetch data
    Task<List<StudyRelationship>?> GetStudyRelationshipsAsync(string sdOid);    
    Task<StudyRelationship?> GetStudyRelationshipAsync(int id);               
    // Update data
    Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent); 
    Task<StudyRelationship?> UpdateStudyRelationshipAsync(int id, StudyRelationship stRelContent);    
    Task<int> DeleteStudyRelationshipAsync(int id);   

    /****************************************************************
    * Study References
    ****************************************************************/

    // Fetch data
    Task<List<StudyReference>?> GetStudyReferencesAsync(string sdOid);  
    Task<StudyReference?> GetStudyReferenceAsync(int id);             
    // Update data
    Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent); 
    Task<StudyReference?> UpdateStudyReferenceAsync(int id, StudyReference stRefContent);    
    Task<int> DeleteStudyReferenceAsync(int id);    
    
}