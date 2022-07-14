using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IStudyRepository
{  
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/

    Task<bool> StudyExists(string sdSid);
    Task<bool> StudyAttributeExists(string sdSid, string typeName, int id);
    
    /****************************************************************
    * Fetch Study / Study entry data
    ****************************************************************/
    
    Task<IEnumerable<StudyInDb>> GetAllStudyRecords();
    Task<IEnumerable<StudyEntryInDb>> GetAllStudyEntries();
    
    Task<IEnumerable<StudyInDb>> GetPaginatedStudyRecords(int pNum, int pSize);
    Task<IEnumerable<StudyEntryInDb>> GetPaginatedStudyEntries(int pNum, int pSize);
    
    Task<IEnumerable<StudyInDb>> GetFilteredStudyRecords(string titleFilter);
    Task<IEnumerable<StudyEntryInDb>> GetFilteredStudyEntries(string titleFilter);
    
    Task<IEnumerable<StudyInDb>> GetPaginatedFilteredStudyRecords(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<StudyEntryInDb>> GetPaginatedFilteredStudyEntries(string titleFilter, int pNum, int pSize);
    
    Task<IEnumerable<StudyInDb>> GetRecentStudyRecords(int n);
    Task<IEnumerable<StudyEntryInDb>> GetRecentStudyEntries(int n);
    
    Task<IEnumerable<StudyInDb>> GetStudyRecordsByOrg(int orgId);
    Task<IEnumerable<StudyEntryInDb>> GetStudyEntriesByOrg(int orgId);
    
    Task<StudyInDb?> GetStudyData(string sdSid);
    
    /****************************************************************
    * Update Study data
    ****************************************************************/
    
    Task<StudyInDb?> CreateStudyData(StudyInDb studyData);
    Task<StudyInDb?> UpdateStudyData(StudyInDb studyData);
    Task<int> DeleteStudyData(string sdSid, string userName);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    Task<FullStudyInDb?> GetFullStudyById(string sdSid);
    Task<int> DeleteFullStudy(string sdSid, string userName);

    /****************************************************************
    * Obtain and store Study data from the MDR
    ****************************************************************/
    
    Task<StudyMdrDetails?> GetStudyDetailsFromMdr(int regId, string sdSid);
    Task<StudyInMdr?> GetStudyDataFromMdr(int mdrId);
    Task<FullStudyFromMdrInDb?> GetFullStudyDataFromMdr(StudyInDb importedStudy, int mdrId);
    
    /****************************************************************
    * Study statistics
    ****************************************************************/

    Task<int> GetTotalStudies();
    Task<int> GetTotalFilteredStudies(string titleFilter);
    Task<IEnumerable<StatisticInDb>> GetStudiesByType();
    
    /****************************************************************
    * Study contributors
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyContributorInDb>> GetStudyContributors(string sdSid);
    Task<StudyContributorInDb?> GetStudyContributor(int? id);
    
    // Update data
    Task<StudyContributorInDb?> CreateStudyContributor(StudyContributorInDb studyContributorInDb);
    Task<StudyContributorInDb?> UpdateStudyContributor(StudyContributorInDb studyContributorInDb);
    Task<int> DeleteStudyContributor(int id, string userName);

    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyFeatureInDb>> GetStudyFeatures(string sdSid);
    Task<StudyFeatureInDb?> GetStudyFeature(int? id);
    
    // Update data
    Task<StudyFeatureInDb?> CreateStudyFeature(StudyFeatureInDb studyFeatureInDb);
    Task<StudyFeatureInDb?> UpdateStudyFeature(StudyFeatureInDb studyFeatureInDb);
    Task<int> DeleteStudyFeature(int id, string userName);

    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyIdentifierInDb>> GetStudyIdentifiers(string sdSid);
    Task<StudyIdentifierInDb?> GetStudyIdentifier(int? id);
    
    // Update data
    Task<StudyIdentifierInDb?> CreateStudyIdentifier(StudyIdentifierInDb studyIdentifierInDb);
    Task<StudyIdentifierInDb?> UpdateStudyIdentifier(StudyIdentifierInDb studyIdentifierInDb);
    Task<int> DeleteStudyIdentifier(int id, string userName);

    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyReferenceInDb>> GetStudyReferences(string sdSid);
    Task<StudyReferenceInDb?> GetStudyReference(int? id);
    
    // Update data
    Task<StudyReferenceInDb?> CreateStudyReference(StudyReferenceInDb studyReferenceInDb);
    Task<StudyReferenceInDb?> UpdateStudyReference(StudyReferenceInDb studyReferenceInDb);
    Task<int> DeleteStudyReference(int id, string userName);

    /****************************************************************
    * Study Relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyRelationshipInDb>> GetStudyRelationships(string sdSid);
    Task<StudyRelationshipInDb?> GetStudyRelationship(int? id);
    
    // Update data
    Task<StudyRelationshipInDb?> CreateStudyRelationship(StudyRelationshipInDb studyRelationshipInDb);
    Task<StudyRelationshipInDb?> UpdateStudyRelationship(StudyRelationshipInDb studyRelationshipInDb);
    Task<int> DeleteStudyRelationship(int id, string userName);

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyTitleInDb>> GetStudyTitles(string sdSid);
    Task<StudyTitleInDb?> GetStudyTitle(int? id);
    
    // Update data
    Task<StudyTitleInDb?> CreateStudyTitle(StudyTitleInDb studyTitleInDb);
    Task<StudyTitleInDb?> UpdateStudyTitle(StudyTitleInDb studyTitleInDb);
    Task<int> DeleteStudyTitle(int id, string userName);

    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyTopicInDb>> GetStudyTopics(string sdSid);
    Task<StudyTopicInDb?> GetStudyTopic(int? id);
    
    // Update data
    Task<StudyTopicInDb?> CreateStudyTopic(StudyTopicInDb studyTopicInDb);
    Task<StudyTopicInDb?> UpdateStudyTopic(StudyTopicInDb studyTopicInDb);
    Task<int> DeleteStudyTopic(int id, string userName);

}
