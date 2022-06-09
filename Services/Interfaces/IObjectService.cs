using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IObjectService
{
    // Check functions - return a boolean that indicates if a record
    // with the provided id does NOT exists in the database, 
    // i.e. it is true if there is no matching record.
    // Allows controller functions to avoid this error and return a
    // request body with suitable status code
   
    // Check if data object exists 
    Task<bool> ObjectDoesNotExistAsync (string sd_oid);
    // Check if attribute exists on specified object
    Task<bool> ObjectAttributeDoesNotExistAsync (string sd_oid, string type_name, int id); 
    
    /****************************************************************
    * Data object... (data object data only, no attributes)
    ****************************************************************/
  
    // Fetch data 
    Task<List<DataObjectData>?> GetAllObjectsDataAsync(); 
    Task<List<DataObjectData>?> GetRecentObjectsDataAsync(int n); 
    Task<DataObjectData?> GetObjectDataAsync(string sd_oid);   
    // Update data
    Task<DataObjectData?> CreateDataObjectDataAsync(DataObjectData dataObjectContent);
    Task<DataObjectData?> UpdateDataObjectDataAsync(DataObjectData dataObjectContent);
    Task<int> DeleteDataObjectAsync(string sd_oid);   
    
    /****************************************************************
    * Full Data object...(with attribute data)
    ****************************************************************/
    
    // Fetch data 
    Task<List<FullDataObject>?> GetAllFullObjectsAsync();    
    Task<FullDataObject?> GetFullObjectByIdAsync(string sd_oid);   
    // Update data
    Task<int> DeleteFullObjectAsync(string sd_oid);   
    
    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    Task<List<ObjectDataset>?> GetObjectDatasetsAsync(string sd_oid);    
    Task<ObjectDataset?> GetObjectDatasetAsync(int id);                  
    // Update data
    Task<ObjectDataset?> CreateObjectDatasetAsync(ObjectDataset objDatasetContent); 
    Task<ObjectDataset?> UpdateObjectDatasetAsync(int id, ObjectDataset objDatasetContent);    
    Task<int> DeleteObjectDatasetAsync(int id);    
    
    /****************************************************************
    * Object titles
    ****************************************************************/
  
    // Fetch data
    Task<List<ObjectTitle>?> GetObjectTitlesAsync(string sd_oid);    
    Task<ObjectTitle?> GetObjectTitleAsync(int id);                  
    // Update data
    Task<ObjectTitle?> CreateObjectTitleAsync(ObjectTitle objTitleContent); 
    Task<ObjectTitle?> UpdateObjectTitleAsync(int id, ObjectTitle objTitleContent);    
    Task<int> DeleteObjectTitleAsync(int id);    
    
    /****************************************************************
    * Object instances
    ****************************************************************/
   
    // Fetch data
    Task<List<ObjectInstance>?> GetObjectInstancesAsync(string sd_oid);   
    Task<ObjectInstance?> GetObjectInstanceAsync(int id);                
    // Update data
    Task<ObjectInstance?> CreateObjectInstanceAsync(ObjectInstance objInstanceContent); 
    Task<ObjectInstance?> UpdateObjectInstanceAsync(int id, ObjectInstance objInstanceContent);    
    Task<int> DeleteObjectInstanceAsync(int id);    
    
    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    Task<List<ObjectDate>?> GetObjectDatesAsync(string sd_oid);   
    Task<ObjectDate?> GetObjectDateAsync(int id);               
    // Update data
    Task<ObjectDate?> CreateObjectDateAsync(ObjectDate objDateContent); 
    Task<ObjectDate?> UpdateObjectDateAsync(int id, ObjectDate objDateContent);    
    Task<int> DeleteObjectDateAsync(int id);    
    
    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    Task<List<ObjectDescription>?> GetObjectDescriptionsAsync(string sd_oid);    
    Task<ObjectDescription?> GetObjectDescriptionAsync(int id);                  
    // Update data
    Task<ObjectDescription?> CreateObjectDescriptionAsync(ObjectDescription objDescContent); 
    Task<ObjectDescription?> UpdateObjectDescriptionAsync(int id, ObjectDescription objDescContent);    
    Task<int> DeleteObjectDescriptionAsync(int id);    
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
  
    // Fetch data
    Task<List<ObjectContributor>?> GetObjectContributorsAsync(string sd_oid);   
    Task<ObjectContributor?> GetObjectContributorAsync(int id);                 
    // Update data
    Task<ObjectContributor?> CreateObjectContributorAsync(ObjectContributor objContContent); 
    Task<ObjectContributor?> UpdateObjectContributorAsync(int id, ObjectContributor objContContent);    
    Task<int> DeleteObjectContributorAsync(int id);    
    
    /****************************************************************
    * Object topics
    ****************************************************************/
   
    // Fetch data
    Task<List<ObjectTopic>?> GetObjectTopicsAsync(string sd_oid);    
    Task<ObjectTopic?> GetObjectTopicAsync(int id);                
    // Update data
    Task<ObjectTopic?> CreateObjectTopicAsync(ObjectTopic objTopicContent); 
    Task<ObjectTopic?> UpdateObjectTopicAsync(int id, ObjectTopic objTopicContent);    
    Task<int> DeleteObjectTopicAsync(int id);    
    
    /****************************************************************
    * Object identifiers
    ****************************************************************/
   
    // Fetch data
    Task<List<ObjectIdentifier>?> GetObjectIdentifiersAsync(string sd_oid);     
    Task<ObjectIdentifier?> GetObjectIdentifierAsync(int id);                 
    // Update data
    Task<ObjectIdentifier?> CreateObjectIdentifierAsync(ObjectIdentifier objIdentContent); 
    Task<ObjectIdentifier?> UpdateObjectIdentifierAsync(int id, ObjectIdentifier objIdentContent);    
    Task<int> DeleteObjectIdentifierAsync(int id);   
    
    /****************************************************************
    * Object relationships
    ****************************************************************/
  
    // Fetch data
    Task<List<ObjectRelationship>?> GetObjectRelationshipsAsync(string sd_oid);    
    Task<ObjectRelationship?> GetObjectRelationshipAsync(int id);                  
    // Update data
    Task<ObjectRelationship?> CreateObjectRelationshipAsync(ObjectRelationship objRelContent); 
    Task<ObjectRelationship?> UpdateObjectRelationshipAsync(int id, ObjectRelationship objRelContent);    
    Task<int> DeleteObjectRelationshipAsync(int id);    
    
    /****************************************************************
    * Object rights
    ****************************************************************/
  
    // Fetch data
    Task<List<ObjectRight>?> GetObjectRightsAsync(string sd_oid);     
    Task<ObjectRight?> GetObjectRightAsync(int id);                
    // Update data
    Task<ObjectRight?> CreateObjectRightAsync(ObjectRight objRightContent); 
    Task<ObjectRight?> UpdateObjectRightAsync(int id, ObjectRight objRightContent);    
    Task<int> DeleteObjectRightAsync(int id);    
    
}

