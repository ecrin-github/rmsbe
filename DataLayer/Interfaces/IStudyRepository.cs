using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IStudyRepository
{  
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    ****************************************************************/

    Task<bool> StudyDoesNotExistAsync(string sd_sid);
    Task<bool> AttributeDoesNotExist(string type_name, int id);
    Task<bool> StudyAttributeDoesNotExist(string sd_sid, string type_name, int id);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    Task<IEnumerable<StudyInDb>?> GetAllStudies();
    Task<StudyInDb?> GetStudyById(string sd_sid);
    Task<int> DeleteFullStudy(string sd_sid);
    
    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyInDb>?> GetStudiesDataAsync();
    Task<IEnumerable<StudyInDb>?> GetRecentStudyDataAsync(int n);
    Task<StudyInDb?> GetStudyDataAsync(string sd_sid);
    // Update data
    Task<StudyInDb?> CreateStudyDataAsync(StudyInDb studyData, string accessToken);
    Task<StudyInDb?> UpdateStudyDataAsync(StudyInDb studyData, string accessToken);
    Task<int> DeleteStudyDataAsync(string sd_sid);
    
    /****************************************************************
    * Study contributors
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyContributorInDb>?> GetStudyContributorsAsync(string sd_sid);
    Task<StudyContributorInDb?> GetStudyContributorAsync(int? id);
    // Update data
    Task<StudyContributorInDb?> CreateStudyContributorAsync(StudyContributorInDb studyContributorInDb, string accessToken);
    Task<StudyContributorInDb?> UpdateStudyContributorAsync(StudyContributorInDb studyContributorInDb, string accessToken);
    Task<int> DeleteStudyContributorAsync(int id);

    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyFeatureInDb>?> GetStudyFeaturesAsync(string sd_sid);
    Task<StudyFeatureInDb?> GetStudyFeatureAsync(int? id);
    // Update data
    Task<StudyFeatureInDb?> CreateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb, string accessToken);
    Task<StudyFeatureInDb?> UpdateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb, string accessToken);
    Task<int> DeleteStudyFeatureAsync(int id);

    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyIdentifierInDb>?> GetStudyIdentifiersAsync(string sd_sid);
    Task<StudyIdentifierInDb?> GetStudyIdentifierAsync(int? id);
    // Update data
    Task<StudyIdentifierInDb?> CreateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb, string accessToken);
    Task<StudyIdentifierInDb?> UpdateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb, string accessToken);
    Task<int> DeleteStudyIdentifierAsync(int id);

    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyReferenceInDb>?> GetStudyReferencesAsync(string sd_sid);
    Task<StudyReferenceInDb?> GetStudyReferenceAsync(int? id);
    // Update data
    Task<StudyReferenceInDb?> CreateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb, string? accessToken);
    Task<StudyReferenceInDb?> UpdateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb, string? accessToken);
    Task<int> DeleteStudyReferenceAsync(int id);

    /****************************************************************
    * Study Relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyRelationshipInDb>?> GetStudyRelationshipsAsync(string sd_sid);
    Task<StudyRelationshipInDb?> GetStudyRelationshipAsync(int? id);
    // Update data
    Task<StudyRelationshipInDb?> CreateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb, string? accessToken);
    Task<StudyRelationshipInDb?> UpdateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb, string? accessToken);
    Task<int> DeleteStudyRelationshipAsync(int id);

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyTitleInDb>?> GetStudyTitlesAsync(string sd_sid);
    Task<StudyTitleInDb?> GetStudyTitleAsync(int? id);
    // Update data
    Task<StudyTitleInDb?> CreateStudyTitleAsync(StudyTitleInDb studyTitleInDb, string accessToken);
    Task<StudyTitleInDb?> UpdateStudyTitleAsync(StudyTitleInDb studyTitleInDb, string accessToken);
    Task<int> DeleteStudyTitleAsync(int id);

    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyTopicInDb>?> GetStudyTopicsAsync(string sd_sid);
    Task<StudyTopicInDb?> GetStudyTopicAsync(int? id);
    // Update data
    Task<StudyTopicInDb?> CreateStudyTopicAsync(StudyTopicInDb studyTopicInDb, string accessToken);
    Task<StudyTopicInDb?> UpdateStudyTopiAsyncc(StudyTopicInDb studyTopicInDb, string accessToken);
    Task<int> DeleteStudyTopicAsync(int id);

    // Extensions
    /*
    Task<PaginationResponse<StudyInDb>> PaginateStudies(PaginationRequest paginationRequest);
    Task<PaginationResponse<StudyInDb>> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalStudies();
    */
}
