using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IStudyRepository
{  
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/

    Task<bool> StudyExistsAsync(string sdSid);
    Task<bool> StudyAttributeExistsAsync(string sdSid, string typeName, int id);
    
   /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyInDb>> GetStudiesDataAsync();
    Task<IEnumerable<StudyInDb>> GetRecentStudyDataAsync(int n);
    Task<IEnumerable<StudyInDb>> GetPaginatedStudyDataAsync(int pNum, int pSize);
    Task<IEnumerable<StudyInDb>> GetPaginatedFilteredStudyDataAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<StudyInDb>> GetFilteredStudyDataAsync(string titleFilter);
    
    Task<IEnumerable<StudyEntryInDb>> GetStudyEntriesAsync();
    Task<IEnumerable<StudyEntryInDb>> GetRecentStudyEntriesAsync(int n);
    Task<IEnumerable<StudyEntryInDb>> GetPaginatedStudyEntriesAsync(int pNum, int pSize);
    Task<IEnumerable<StudyEntryInDb>> GetPaginatedFilteredStudyEntriesAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<StudyEntryInDb>> GetFilteredStudyEntriesAsync(string titleFilter);
    
    Task<StudyInDb?> GetStudyDataAsync(string sdSid);
    
    // Update data
    Task<StudyInDb?> CreateStudyDataAsync(StudyInDb studyData);
    Task<StudyInDb?> UpdateStudyDataAsync(StudyInDb studyData);
    Task<int> DeleteStudyDataAsync(string sdSid, string userName);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    Task<FullStudyInDb?> GetFullStudyByIdAsync(string sdSid);
    Task<int> DeleteFullStudyAsync(string sdSid, string userName);

    /****************************************************************
    * Obtain and store Study data from the MDR
    ****************************************************************/
    
    Task<StudyMdrDetails?> GetStudyDetailsFromMdr(int regId, string sdSid);
    Task<StudyInMdr?> GetStudyDataFromMdr(int mdrId);
    Task<FullStudyInDb?> GetFullStudyDataFromMdr(StudyInDb importedStudy, int mdrId, string sdSid, string userName);
    
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
    Task<IEnumerable<StudyContributorInDb>> GetStudyContributorsAsync(string sdSid);
    Task<StudyContributorInDb?> GetStudyContributorAsync(int? id);
    // Update data
    Task<StudyContributorInDb?> CreateStudyContributorAsync(StudyContributorInDb studyContributorInDb);
    Task<StudyContributorInDb?> UpdateStudyContributorAsync(StudyContributorInDb studyContributorInDb);
    Task<int> DeleteStudyContributorAsync(int id, string userName);

    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyFeatureInDb>> GetStudyFeaturesAsync(string sdSid);
    Task<StudyFeatureInDb?> GetStudyFeatureAsync(int? id);
    // Update data
    Task<StudyFeatureInDb?> CreateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb);
    Task<StudyFeatureInDb?> UpdateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb);
    Task<int> DeleteStudyFeatureAsync(int id, string userName);

    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyIdentifierInDb>> GetStudyIdentifiersAsync(string sdSid);
    Task<StudyIdentifierInDb?> GetStudyIdentifierAsync(int? id);
    // Update data
    Task<StudyIdentifierInDb?> CreateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb);
    Task<StudyIdentifierInDb?> UpdateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb);
    Task<int> DeleteStudyIdentifierAsync(int id, string userName);

    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyReferenceInDb>> GetStudyReferencesAsync(string sdSid);
    Task<StudyReferenceInDb?> GetStudyReferenceAsync(int? id);
    // Update data
    Task<StudyReferenceInDb?> CreateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb);
    Task<StudyReferenceInDb?> UpdateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb);
    Task<int> DeleteStudyReferenceAsync(int id, string userName);

    /****************************************************************
    * Study Relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyRelationshipInDb>> GetStudyRelationshipsAsync(string sdSid);
    Task<StudyRelationshipInDb?> GetStudyRelationshipAsync(int? id);
    // Update data
    Task<StudyRelationshipInDb?> CreateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb);
    Task<StudyRelationshipInDb?> UpdateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb);
    Task<int> DeleteStudyRelationshipAsync(int id, string userName);

    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    Task<IEnumerable<StudyTitleInDb>> GetStudyTitlesAsync(string sdSid);
    Task<StudyTitleInDb?> GetStudyTitleAsync(int? id);
    // Update data
    Task<StudyTitleInDb?> CreateStudyTitleAsync(StudyTitleInDb studyTitleInDb);
    Task<StudyTitleInDb?> UpdateStudyTitleAsync(StudyTitleInDb studyTitleInDb);
    Task<int> DeleteStudyTitleAsync(int id, string userName);

    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<StudyTopicInDb>> GetStudyTopicsAsync(string sdSid);
    Task<StudyTopicInDb?> GetStudyTopicAsync(int? id);
    // Update data
    Task<StudyTopicInDb?> CreateStudyTopicAsync(StudyTopicInDb studyTopicInDb);
    Task<StudyTopicInDb?> UpdateStudyTopicAsync(StudyTopicInDb studyTopicInDb);
    Task<int> DeleteStudyTopicAsync(int id, string userName);

}
