using rmsbe.SysModels;

using rmsbe.DataLayer.Interfaces;
using rmsbe.DbModels;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace rmsbe.Services;

public class StudyDataService
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
    Task<bool> StudyDoesNotExistAsync(string sd_sid)
        => _studyRepository.StudyDoesNotExistAsync(sd_sid);
    
    // Check if attribute exists 
    Task<bool> AttributeDoesNotExist(string type_name, int id)
        => _studyRepository.AttributeDoesNotExist(type_name, id);

    // Check if attribute exists on this study
    Task<bool> StudyAttributeDoesNotExist (string sd_sid, string type_name, int id)
         => _studyRepository.StudyAttributeDoesNotExist(sd_sid,type_name, id); 
    
    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    /*
    // Fetch data
    
    Task<List<StudyData>?> GetStudyRecordsDataAsync(){ 
        throw new NotImplementedException();
    }
    
    Task<List<StudyData>?> GetRecentStudyRecordsAsync(int n){ 
        throw new NotImplementedException();
    }
    
    Task<StudyData?> GetStudyRecordDataAsync(string sd_sid){ 
        throw new NotImplementedException();
    }
    
    // Update data
    
    Task<StudyData?> CreateStudyRecordDataAsync(StudyData studyDataContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
    Task<StudyData?> UpdateStudyRecordDataAsync(StudyData studyDataContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
    Task<int> DeleteStudyRecordDataAsync(string sd_sid){ 
        throw new NotImplementedException();
    }
    */
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    /*
    // Fetch data
    
    Task<List<Study>?> GetFullStudyDataAsync(){ 
        throw new NotImplementedException();
    } 
    
    Task<Study?> GetFullStudyByIdAsync(string sd_sid){ 
        throw new NotImplementedException();
    }
    
    // Update data
    
    Task<Study?> CreateFullStudyAsync(Study studyContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
    Task<Study?> UpdateFullStudyAsync(Study studyContent, string? accessToken){ 
        throw new NotImplementedException();
    }
    
    Task<int> DeleteFullStudyAsync(string sd_sid){ 
        throw new NotImplementedException();
    }
    */    
    /****************************************************************
    * Study identifiers
    ****************************************************************/
    /*
    // Fetch data
    
    Task<List<StudyIdentifier>?> GetStudyIdentifiersAsync(string sd_oid){ 
        throw new NotImplementedException();
    }   
    
    Task<StudyIdentifier?> GetStudyIdentifierAsync(int id){ 
        throw new NotImplementedException();
    }        
    
    // Update data
    Task<StudyIdentifier?> CreateStudyIdentifierAsync(StudyIdentifier stIdentContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    Task<StudyIdentifier?> UpdateStudyIdentifierAsync(int id, StudyIdentifier stIdentContent, string? accessToken){ 
        throw new NotImplementedException();
    }  
    
    Task<int> DeleteStudyIdentifierAsync(int id){ 
        throw new NotImplementedException();
    }  
    */
    /****************************************************************
    * Study titles
    ****************************************************************/  
    /*
    // Fetch data
    
    Task<List<StudyTitle>?> GetStudyTitlesAsync(string sd_oid){ 
        throw new NotImplementedException();
    }  
    
    Task<StudyTitle?> GetStudyTitleAsync(int id){ 
        throw new NotImplementedException();
    }     
    
    // Update data
    
    Task<StudyTitle?> CreateStudyTitleAsync(StudyTitle stTitleContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    Task<StudyTitle?> UpdateStudyTitleAsync(int id, StudyTitle stTitleContent, string? accessToken){ 
        throw new NotImplementedException();
    }   
    
    Task<int> DeleteStudyTitleAsync(int id){ 
        throw new NotImplementedException();
    }    
    */
    /****************************************************************
    * Study contributors
    ****************************************************************/   
    /*
    // Fetch data
    
    Task<List<StudyContributor>?> GetStudyContributorsAsync(string sd_oid){ 
        throw new NotImplementedException();
    }   
    
    Task<StudyContributor?> GetStudyContributorAsync(int id){ 
        throw new NotImplementedException();
    }                
    // Update data
    
    Task<StudyContributor?> CreateStudyContributorAsync(StudyContributor stContContent, string? accessToken){  
        throw new NotImplementedException();
    } 
    
    Task<StudyContributor?> UpdateStudyContributorAsync(int id, StudyContributor stContContent, string? accessToken){ 
        throw new NotImplementedException();
    }   
    
    Task<int> DeleteStudyContributorAsync(int id){ 
        throw new NotImplementedException();
    }  
    */
    /****************************************************************
    * Study features
    ****************************************************************/
    /*
    // Fetch data
    
    Task<List<StudyFeature>?> GetStudyFeaturesAsync(string sd_oid){ 
        throw new NotImplementedException();
    }  
    
    Task<StudyFeature?> GetStudyFeatureAsync(int id){ 
        throw new NotImplementedException();
    }            
    
    // Update data
    
    Task<StudyFeature?> CreateStudyFeatureAsync(StudyFeature stFeatureContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    Task<StudyFeature?> UpdateStudyFeatureAsync(int id, StudyFeature stFeatureContent, string? accessToken){ 
        throw new NotImplementedException();
    }    
    
    Task<int> DeleteStudyFeatureAsync(int id){ 
        throw new NotImplementedException();
    }  
    */
    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    /*
    Task<List<StudyTopic>?> GetStudyTopicsAsync(string sd_oid){ 
        throw new NotImplementedException();
    }   
    
    Task<StudyTopic?> GetStudyTopicAsync(int id){ 
        throw new NotImplementedException();
    }                  
    
    // Update data
    Task<StudyTopic?> CreateStudyTopicAsync(StudyTopic stTopicContent, string? accessToken){ 
        throw new NotImplementedException();
    } 
    
    Task<StudyTopic?> UpdateStudyTopicAsync(int id, StudyTopic stTopicContent, string? accessToken){ 
        throw new NotImplementedException();
    }  
    
    Task<int> DeleteStudyTopicAsync(int id){ 
        throw new NotImplementedException();
    }  
    */
    /****************************************************************
    * Study Relationships
    ****************************************************************/
      
    // Fetch data

    async Task<List<StudyRelationship>?> GetStudyRelationshipsAsync(string sd_oid){
        var relationshipsInDb = await _studyRepository.GetStudyRelationshipsAsync(sd_oid);
        return relationshipsInDb?.Select(r => new StudyRelationship(r)).ToList();
    }    
    
    async Task<StudyRelationship?> GetStudyRelationshipAsync(int id){ 
        StudyRelationshipInDb? studyRelInDb = await _studyRepository.GetStudyRelationshipAsync(id);
        return studyRelInDb == null ? null : new StudyRelationship(studyRelInDb);
    }       
    
    // Update data
    /*
    Task<StudyRelationship?> CreateStudyRelationshipAsync(StudyRelationship stRelContent, string? accessToken){  
        throw new NotImplementedException();
    } 
    
    Task<StudyRelationship?> UpdateStudyRelationshipAsync(int id, StudyRelationship stRelContent, string? accessToken){ 
        throw new NotImplementedException();
    }   
    
    Task<int> DeleteStudyRelationshipAsync(int id){ 
        throw new NotImplementedException();
    }    
    */
    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    
    async Task<List<StudyReference>?> GetStudyReferencesAsync(string sd_oid)
    {
        var referencesInDb = await _studyRepository.GetStudyReferencesAsync(sd_oid);
        return referencesInDb?.Select(r => new StudyReference(r)).ToList();
    }     

    async Task<StudyReference?> GetStudyReferenceAsync(int id)
    { 
        StudyReferenceInDb? studyRefInDb = await _studyRepository.GetStudyReferenceAsync(id);
        return studyRefInDb == null ? null : new StudyReference(studyRefInDb);
    }                   
    
    // Update data
    
    async Task<StudyReference?> CreateStudyReferenceAsync(StudyReference stRefContent, string? accessToken)
    {
        var stRefContentInDb = new StudyReferenceInDb(stRefContent);
        stRefContentInDb.last_edited_by = _user_name;
        var res = await _studyRepository.CreateStudyReferenceAsync(stRefContentInDb, accessToken);
        return res == null ? null : new StudyReference(res);
    } 

    async Task<StudyReference?> UpdateStudyReferenceAsync(int id, StudyReference stRefContent, string? accessToken)
    {
        var stRefContentInDb = new StudyReferenceInDb(stRefContent);
        stRefContentInDb.id = id;
        stRefContentInDb.last_edited_by = _user_name;
        var res = await _studyRepository.UpdateStudyReferenceAsync(stRefContentInDb, accessToken);
        return res == null ? null : new StudyReference(res);
    } 

    async Task<int> DeleteStudyReferenceAsync(int id)
    {
        return await _studyRepository.DeleteStudyReferenceAsync(id);
    }    
}
