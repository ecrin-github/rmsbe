using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IStudyRepository
{  
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/

    Task<bool> StudyExistsAsync(string sd_sid);
    Task<bool> StudyAttributeExistsAsync(string sd_sid, string type_name, int id);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    Task<FullStudyInDb?> GetFullStudyByIdAsync(string sd_sid);
    Task<int> DeleteFullStudyAsync(string sd_sid, string user_name);
    
    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyInDb>> GetStudiesDataAsync();
    Task<IEnumerable<StudyInDb>> GetRecentStudyDataAsync(int n);
    Task<StudyInDb?> GetStudyDataAsync(string sd_sid);
    // Update data
    Task<StudyInDb?> CreateStudyDataAsync(StudyInDb studyData);
    Task<StudyInDb?> UpdateStudyDataAsync(StudyInDb studyData);
    Task<int> DeleteStudyDataAsync(string sd_sid, string user_name);
    
    /****************************************************************
    * Study contributors
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyContributorInDb>> GetStudyContributorsAsync(string sd_sid);
    Task<StudyContributorInDb?> GetStudyContributorAsync(int? id);
    // Update data
    Task<StudyContributorInDb?> CreateStudyContributorAsync(StudyContributorInDb studyContributorInDb);
    Task<StudyContributorInDb?> UpdateStudyContributorAsync(StudyContributorInDb studyContributorInDb);
    Task<int> DeleteStudyContributorAsync(int id, string user_name);

    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyFeatureInDb>> GetStudyFeaturesAsync(string sd_sid);
    Task<StudyFeatureInDb?> GetStudyFeatureAsync(int? id);
    // Update data
    Task<StudyFeatureInDb?> CreateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb);
    Task<StudyFeatureInDb?> UpdateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb);
    Task<int> DeleteStudyFeatureAsync(int id, string user_name);

    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyIdentifierInDb>> GetStudyIdentifiersAsync(string sd_sid);
    Task<StudyIdentifierInDb?> GetStudyIdentifierAsync(int? id);
    // Update data
    Task<StudyIdentifierInDb?> CreateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb);
    Task<StudyIdentifierInDb?> UpdateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb);
    Task<int> DeleteStudyIdentifierAsync(int id, string user_name);

    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyReferenceInDb>> GetStudyReferencesAsync(string sd_sid);
    Task<StudyReferenceInDb?> GetStudyReferenceAsync(int? id);
    // Update data
    Task<StudyReferenceInDb?> CreateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb);
    Task<StudyReferenceInDb?> UpdateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb);
    Task<int> DeleteStudyReferenceAsync(int id, string user_name);

    /****************************************************************
    * Study Relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyRelationshipInDb>> GetStudyRelationshipsAsync(string sd_sid);
    Task<StudyRelationshipInDb?> GetStudyRelationshipAsync(int? id);
    // Update data
    Task<StudyRelationshipInDb?> CreateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb);
    Task<StudyRelationshipInDb?> UpdateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb);
    Task<int> DeleteStudyRelationshipAsync(int id, string user_name);

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyTitleInDb>> GetStudyTitlesAsync(string sd_sid);
    Task<StudyTitleInDb?> GetStudyTitleAsync(int? id);
    // Update data
    Task<StudyTitleInDb?> CreateStudyTitleAsync(StudyTitleInDb studyTitleInDb);
    Task<StudyTitleInDb?> UpdateStudyTitleAsync(StudyTitleInDb studyTitleInDb);
    Task<int> DeleteStudyTitleAsync(int id, string user_name);

    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyTopicInDb>> GetStudyTopicsAsync(string sd_sid);
    Task<StudyTopicInDb?> GetStudyTopicAsync(int? id);
    // Update data
    Task<StudyTopicInDb?> CreateStudyTopicAsync(StudyTopicInDb studyTopicInDb);
    Task<StudyTopicInDb?> UpdateStudyTopicAsync(StudyTopicInDb studyTopicInDb);
    Task<int> DeleteStudyTopicAsync(int id, string user_name);

    // Extensions
    /*
    Task<PaginationResponse<StudyInDb>> PaginateStudies(PaginationRequest paginationRequest);
    Task<PaginationResponse<StudyInDb>> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalStudies();
    */
}
