using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IObjectDataService
{
    // Check functions - return a boolean that indicates if a record
    // with the provided id does NOT exists in the database, 
    // i.e. it is true if there is no matching record.
    // Allows controller functions to avoid this error and return a
    // request body with suitable status code
    
    Task<bool> ObjectDoesNotExist (string sd_oid); // Check if data object exists 
    Task<bool> AttributeDoesNotExist (string type_name, int id); // Check if attribute exists 
    // Check if attribute exists on this object
    Task<bool> ObjectAttributeDoesNotExist (string sd_oid, string type_name, int id); 
    
    // Data object...
    
    // Object datasets
    // Fetch data
    Task<List<ObjectDataset>?> GetObjectDatasetsAsync(string sd_oid);     // all datasets of a particular data object
    Task<ObjectDataset?> GetObjectDatasetAsync(int id);                   // a specific object dataset
    // Update data
    Task<ObjectDataset?> CreateObjectDatasetAsync(ObjectDataset objDatasetContent, string? accessToken); 
    Task<ObjectDataset?> UpdateObjectDatasetAsync(int id, ObjectDataset objDatasetContent, string? accessToken);    
    Task<int> DeleteObjectDatasetAsync(int id);    
    
    // Object titles
    // Fetch data
    Task<List<ObjectTitle>?> GetObjectTitlesAsync(string sd_oid);     // all titles of a particular data object
    Task<ObjectTitle?> GetObjectTitleAsync(int id);                   // a specific object date
    // Update data
    Task<ObjectTitle?> CreateObjectTitleAsync(ObjectTitle objTitleContent, string? accessToken); 
    Task<ObjectTitle?> UpdateObjectTitleAsync(int id, ObjectTitle objTitleContent, string? accessToken);    
    Task<int> DeleteObjectTitleAsync(int id);    
    
    // Object instances
    // Fetch data
    Task<List<ObjectInstance>?> GetObjectInstancesAsync(string sd_oid);     // all instances of a particular data object
    Task<ObjectInstance?> GetObjectInstanceAsync(int id);                   // a specific object instance
    // Update data
    Task<ObjectInstance?> CreateObjectInstanceAsync(ObjectInstance objInstanceContent, string? accessToken); 
    Task<ObjectInstance?> UpdateObjectInstanceAsync(int id, ObjectInstance objInstanceContent, string? accessToken);    
    Task<int> DeleteObjectInstanceAsync(int id);    
    
    // Object dates 
    // Fetch data
    Task<List<ObjectDate>?> GetObjectDatesAsync(string sd_oid);     // all dates of a particular data object
    Task<ObjectDate?> GetObjectDateAsync(int id);                   // a specific object date
    // Update data
    Task<ObjectDate?> CreateObjectDateAsync(ObjectDate objDateContent, string? accessToken); 
    Task<ObjectDate?> UpdateObjectDateAsync(int id, ObjectDate objDateContent, string? accessToken);    
    Task<int> DeleteObjectDateAsync(int id);    
    
    // Object descriptions
    // Fetch data
    Task<List<ObjectDescription>?> GetObjectDescriptionsAsync(string sd_oid);     // all descriptions of a particular data object
    Task<ObjectDescription?> GetObjectDescriptionAsync(int id);                   // a specific object description
    // Update data
    Task<ObjectDescription?> CreateObjectDescriptionAsync(ObjectDescription objDescContent, string? accessToken); 
    Task<ObjectDescription?> UpdateObjectDescriptionAsync(int id, ObjectDescription objDescContent, string? accessToken);    
    Task<int> DeleteObjectDescriptionAsync(int id);    
    
    // Object contributors
    // Fetch data
    Task<List<ObjectContributor>?> GetObjectContributorsAsync(string sd_oid);     // all contributors of a particular data object
    Task<ObjectContributor?> GetObjectContributorAsync(int id);                   // a specific object contributor
    // Update data
    Task<ObjectContributor?> CreateObjectContributorAsync(ObjectContributor objContContent, string? accessToken); 
    Task<ObjectContributor?> UpdateObjectContributorAsync(int id, ObjectContributor objContContent, string? accessToken);    
    Task<int> DeleteObjectContributorAsync(int id);    
    
    // Object topics
    // Fetch data
    Task<List<ObjectTopic>?> GetObjectTopicsAsync(string sd_oid);     // all topics of a particular data object
    Task<ObjectTopic?> GetObjectTopicAsync(int id);                   // a specific object topic
    // Update data
    Task<ObjectTopic?> CreateObjectTopicAsync(ObjectTopic objTopicContent, string? accessToken); 
    Task<ObjectTopic?> UpdateObjectTopicAsync(int id, ObjectTopic objTopicContent, string? accessToken);    
    Task<int> DeleteObjectTopicAsync(int id);    
    
    // Object identifiers
    // Fetch data
    Task<List<ObjectIdentifier>?> GetObjectIdentifiersAsync(string sd_oid);     // all identifiers of a particular data object
    Task<ObjectIdentifier?> GetObjectIdentifierAsync(int id);                   // a specific object identifier
    // Update data
    Task<ObjectIdentifier?> CreateObjectIdentifierAsync(ObjectIdentifier objIdentContent, string? accessToken); 
    Task<ObjectIdentifier?> UpdateObjectIdentifierAsync(int id, ObjectIdentifier objIdentContent, string? accessToken);    
    Task<int> DeleteObjectIdentifierAsync(int id);   
    
    // Object relationships
    // Fetch data
    Task<List<ObjectRelationship>?> GetObjectRelationshipsAsync(string sd_oid);     // all relationships of a particular data object
    Task<ObjectRelationship?> GetObjectRelationshipAsync(int id);                   // a specific object relationship
    // Update data
    Task<ObjectRelationship?> CreateObjectRelationshipAsync(ObjectRelationship objRelContent, string? accessToken); 
    Task<ObjectRelationship?> UpdateObjectRelationshipAsync(int id, ObjectRelationship objRelContent, string? accessToken);    
    Task<int> DeleteObjectRelationshipAsync(int id);    
    
    // Object rights
    // Fetch data
    Task<List<ObjectRight>?> GetObjectRightsAsync(string sd_oid);     // all rights of a particular data object
    Task<ObjectRight?> GetObjectRightAsync(int id);                   // a specific object right
    // Update data
    Task<ObjectRight?> CreateObjectRightAsync(ObjectRight objRightContent, string? accessToken); 
    Task<ObjectRight?> UpdateObjectRightAsync(int id, ObjectRight objRightContent, string? accessToken);    
    Task<int> DeleteObjectRightAsync(int id);    
    
}

