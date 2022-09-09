using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IStudyService
{
    /****************************************************************
    * Check functions 
    ****************************************************************/
 
    Task<bool> StudyExists(string sdSid);
    Task<bool> StudyAttributeExists (string sdSid, string typeName, int id); 

    /****************************************************************
    * Fetch Study / Study entry data
    ****************************************************************/
      
    Task<List<StudyData>?> GetAllStudyRecords();
    Task<List<StudyEntry>?> GetAllStudyEntries();
     
    Task<List<StudyData>?> GetPaginatedStudyRecords(PaginationRequest validFilter);
    Task<List<StudyEntry>?> GetPaginatedStudyEntries(PaginationRequest validFilter);
    
    Task<List<StudyData>?> GetFilteredStudyRecords(string titleFilter);
    Task<List<StudyEntry>?> GetFilteredStudyEntries(string titleFilter);
    
    Task<List<StudyData>?> GetPaginatedFilteredStudyRecords(string titleFilter, PaginationRequest validFilter);
    Task<List<StudyEntry>?> GetPaginatedFilteredStudyEntries(string titleFilter, PaginationRequest validFilter);
    
    Task<List<StudyData>?> GetRecentStudyRecords(int n);
    Task<List<StudyEntry>?> GetRecentStudyEntries(int n);
    
    Task<List<StudyData>?> GetStudyRecordsByOrg(int orgId);
    Task<List<StudyEntry>?> GetStudyEntriesByOrg(int orgId);
    
    Task<StudyData?>GetStudyRecordData (string sdSid);
    
    /****************************************************************
    * Update Study data
    ****************************************************************/
    
    Task<StudyData?> CreateStudyRecordData(StudyData studyDataContent);
    Task<StudyData?> UpdateStudyRecordData(StudyData studyDataContent);
    Task<int> DeleteStudyRecordData(string sdSid);
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    Task<FullStudy?> GetFullStudyById(string sdSid);
    // Update data
    Task<int> DeleteFullStudy(string sdSid);
    
    // List of linked data objects 
    Task<List<DataObjectEntry>?> GetStudyObjectList(string sdSid);
    // List of linked data objects from multiple studies
    Task<List<DataObjectEntry>?> GetMultiStudyObjectList(string[] studyList);
    
    /****************************************************************
    * Full Study data (including, optionally, attributes in other
    * tables) retrieved from the MDR and stored in the RMS database
    ****************************************************************/
    
    // Fetch and store full data
    Task<FullStudyFromMdr?> GetFullStudyFromMdr(int regId, string sdSid);
    // Fetch and store study data
    Task<StudyData?> GetStudyFromMdr(int regId, string sdSid);
    
    /****************************************************************
    * Statistics
    ****************************************************************/
    
    Task<Statistic> GetTotalStudies();  
    Task<Statistic> GetTotalFilteredStudies(string titleFilter);  
    Task<List<Statistic>?> GetStudiesByType();
    
    Task<List<Statistic>> GetStudyInvolvement(string sdSid);
    
    /****************************************************************
    * Study identifiers
    ****************************************************************/

    // Fetch data
    Task<List<StudyIdentifier>?> GetStudyIdentifiers(string sdOid);     
    Task<StudyIdentifier?> GetStudyIdentifier(int id);      
    
    // Update data
    Task<StudyIdentifier?> CreateStudyIdentifier(StudyIdentifier stIdentContent); 
    Task<StudyIdentifier?> UpdateStudyIdentifier(StudyIdentifier stIdentContent);    
    Task<int> DeleteStudyIdentifier(int id);  
    
    /****************************************************************
    * Study titles
    ****************************************************************/  
 
    // Fetch data
    Task<List<StudyTitle>?> GetStudyTitles(string sdOid);    
    Task<StudyTitle?> GetStudyTitle(int id);         
    
    // Update data
    Task<StudyTitle?> CreateStudyTitle(StudyTitle stTitleContent); 
    Task<StudyTitle?> UpdateStudyTitle(StudyTitle stTitleContent);    
    Task<int> DeleteStudyTitle(int id);   
    
    /****************************************************************
    * Study contributors
    ****************************************************************/   

    // Fetch data
    Task<List<StudyContributor>?> GetStudyContributors(string sdOid);  
    Task<StudyContributor?> GetStudyContributor(int id);      
    
    // Update data
    Task<StudyContributor?> CreateStudyContributor(StudyContributor stContContent); 
    Task<StudyContributor?> UpdateStudyContributor(StudyContributor stContContent);    
    Task<int> DeleteStudyContributor(int id);  
        
    /****************************************************************
    * Study features
    ****************************************************************/

    // Fetch data
    Task<List<StudyFeature>?> GetStudyFeatures(string sdOid);    
    Task<StudyFeature?> GetStudyFeature(int id);  
    
    // Update data
    Task<StudyFeature?> CreateStudyFeature(StudyFeature stFeatureContent); 
    Task<StudyFeature?> UpdateStudyFeature(StudyFeature stFeatureContent);    
    Task<int> DeleteStudyFeature(int id);  

    /****************************************************************
    * Study topics
    ****************************************************************/

    // Fetch data
    Task<List<StudyTopic>?> GetStudyTopics(string sdOid);   
    Task<StudyTopic?> GetStudyTopic(int id);     
    
    // Update data
    Task<StudyTopic?> CreateStudyTopic(StudyTopic stTopicContent); 
    Task<StudyTopic?> UpdateStudyTopic(StudyTopic stTopicContent);    
    Task<int> DeleteStudyTopic(int id);  

    /****************************************************************
    * Study Relationships
    ****************************************************************/

    // Fetch data
    Task<List<StudyRelationship>?> GetStudyRelationships(string sdOid);    
    Task<StudyRelationship?> GetStudyRelationship(int id);   
    
    // Update data
    Task<StudyRelationship?> CreateStudyRelationship(StudyRelationship stRelContent); 
    Task<StudyRelationship?> UpdateStudyRelationship(StudyRelationship stRelContent);    
    Task<int> DeleteStudyRelationship(int id);   

}