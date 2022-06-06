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
    
    // Full Study
    /*
    Task<ICollection<StudyInDb>> GetAllStudies();
    Task<StudyInDb> GetStudyById(string sd_sid);
    Task<StudyInDb> CreateStudy(StudyInDb studyInDb, string accessToken);
    Task<StudyInDb> UpdateStudy(StudyInDb studyInDb, string accessToken);
    Task<int> DeleteStudy(string sd_sid);
    */
    
    // Study data
    Task<ICollection<StudyInDb>> GetStudiesData();
    Task<StudyInDb> GetStudyData(string sd_sid);
    Task<ICollection<StudyInDb>> GetRecentStudyData(int limit);
    Task<StudyInDb> CreateStudyData(StudyInDb studyData, string accessToken);
    Task<StudyInDb> UpdateStudyData(StudyInDb studyData, string accessToken);

    // Study contributors
    Task<ICollection<StudyContributorInDb>> GetStudyContributors(string sd_sid);
    Task<StudyContributorInDb> GetStudyContributor(int? id);
    Task<StudyContributorInDb> CreateStudyContributor(StudyContributorInDb studyContributorInDb, string accessToken);
    Task<StudyContributorInDb> UpdateStudyContributor(StudyContributorInDb studyContributorInDb, string accessToken);
    Task<int> DeleteStudyContributor(int id);

    // Study features
    Task<ICollection<StudyFeatureInDb>> GetStudyFeatures(string sd_sid);
    Task<StudyFeatureInDb> GetStudyFeature(int? id);
    Task<StudyFeatureInDb> CreateStudyFeature(StudyFeatureInDb studyFeatureInDb, string accessToken);
    Task<StudyFeatureInDb> UpdateStudyFeature(StudyFeatureInDb studyFeatureInDb, string accessToken);
    Task<int> DeleteStudyFeature(int id);

    // Study identifiers
    Task<ICollection<StudyIdentifierInDb>> GetStudyIdentifiers(string sd_sid);
    Task<StudyIdentifierInDb> GetStudyIdentifier(int? id);
    Task<StudyIdentifierInDb> CreateStudyIdentifierr(StudyIdentifierInDb studyIdentifierInDb, string accessToken);
    Task<StudyIdentifierInDb> UpdateStudyIdentifier(StudyIdentifierInDb studyIdentifierInDb, string accessToken);
    Task<int> DeleteStudyIdentifier(int id);

    // Study references
    Task<IEnumerable<StudyReferenceInDb>?> GetStudyReferencesAsync(string sd_sid);
    Task<StudyReferenceInDb?> GetStudyReferenceAsync(int? id);
    Task<StudyReferenceInDb?> CreateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb, string? accessToken);
    Task<StudyReferenceInDb?> UpdateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb, string? accessToken);
    Task<int> DeleteStudyReferenceAsync(int id);

    // Study relationships
    Task<IEnumerable<StudyRelationshipInDb>?> GetStudyRelationshipsAsync(string sd_sid);
    Task<StudyRelationshipInDb?> GetStudyRelationshipAsync(int? id);
    Task<StudyRelationshipInDb?> CreateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb, string? accessToken);
    Task<StudyRelationshipInDb?> UpdateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb, string? accessToken);
    Task<int> DeleteStudyRelationshipAsync(int id);

    // Study titles
    Task<ICollection<StudyTitleInDb>?> GetStudyTitles(string sd_sid);
    Task<StudyTitleInDb?> GetStudyTitle(int? id);
    Task<StudyTitleInDb?> CreateStudyTitle(StudyTitleInDb studyTitleInDb, string accessToken);
    Task<StudyTitleInDb?> UpdateStudyTitle(StudyTitleInDb studyTitleInDb, string accessToken);
    Task<int> DeleteStudyTitle(int id);

    // Study topics
    Task<ICollection<StudyTopicInDb>?> GetStudyTopics(string sd_sid);
    Task<StudyTopicInDb?> GetStudyTopic(int? id);
    Task<StudyTopicInDb?> CreateStudyTopic(StudyTopicInDb studyTopicInDb, string accessToken);
    Task<StudyTopicInDb?> UpdateStudyTopic(StudyTopicInDb studyTopicInDb, string accessToken);
    Task<int> DeleteStudyTopic(int id);

    // Extensions
    /*
    Task<PaginationResponse<StudyInDb>> PaginateStudies(PaginationRequest paginationRequest);
    Task<PaginationResponse<StudyInDb>> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalStudies();
    */
}
