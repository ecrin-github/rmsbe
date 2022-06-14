using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IObjectRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> ObjectDoesNotExistAsync(string sd_oid);
    Task<bool> ObjectAttributeDoesNotExistAsync(string sd_oid, string type_name, int id);
    
    /****************************************************************
    * Full Data Object data (including attributes in other tables)
    ****************************************************************/
  
    // Fetch data
    Task<FullObjectInDb?> GetFullObjectByIdAsync(string sd_oid);
    // Update data
    Task<int> DeleteFullObjectAsync(string sd_oid, string user_name);
    
    /****************************************************************
    * Data Object data (without attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DataObjectInDb>?> GetDataObjectsDataAsync();
    Task<IEnumerable<DataObjectInDb>?> GetRecentObjectDataAsync(int limit);
    Task<DataObjectInDb?> GetDataObjectDataAsync(string sd_oid);
    // Update data
    Task<DataObjectInDb?> CreateDataObjectDataAsync(DataObjectInDb dataObjectData);
    Task<DataObjectInDb?> UpdateDataObjectDataAsync(DataObjectInDb dataObjectData);
    Task<int> DeleteDataObjectDataAsync(string sd_oid, string user_name);
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectContributorInDb>?> GetObjectContributorsAsync(string sd_oid);
    Task<ObjectContributorInDb?> GetObjectContributorAsync(int? id);
    // Update data
    Task<ObjectContributorInDb?> CreateObjectContributorAsync(ObjectContributorInDb objectContributorInDb);
    Task<ObjectContributorInDb?> UpdateObjectContributorAsync(ObjectContributorInDb objectContributorInDb);
    Task<int> DeleteObjectContributorAsync(int id, string user_name);

    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDatasetInDb>?> GetObjectDatasetsAsync(string sd_oid);
    Task<ObjectDatasetInDb?> GetObjectDatasetAsync(int? id);
    // Update data
    Task<ObjectDatasetInDb?> CreateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb);
    Task<ObjectDatasetInDb?> UpdateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb);
    Task<int> DeleteObjectDatasetAsync(int id, string user_name);

    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDateInDb>?> GetObjectDatesAsync(string sd_oid);
    Task<ObjectDateInDb?> GetObjectDateAsync(int? id);
    // Update data
    Task<ObjectDateInDb?> CreateObjectDateAsync(ObjectDateInDb objectDateInDb);
    Task<ObjectDateInDb?> UpdateObjectDateAsync(ObjectDateInDb objectDateInDb);
    Task<int> DeleteObjectDateAsync(int id, string user_name);

    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDescriptionInDb>?> GetObjectDescriptionsAsync(string sd_oid);
    Task<ObjectDescriptionInDb?> GetObjectDescriptionAsync(int? id);
    // Update data
    Task<ObjectDescriptionInDb?> CreateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb);
    Task<ObjectDescriptionInDb?> UpdateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb);
    Task<int> DeleteObjectDescriptionAsync(int id, string user_name);

    /****************************************************************
    * Object identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectIdentifierInDb>?> GetObjectIdentifiersAsync(string sd_oid);
    Task<ObjectIdentifierInDb?> GetObjectIdentifierAsync(int? id);
    // Update data
    Task<ObjectIdentifierInDb?> CreateObjectIdentifierAsync(ObjectIdentifierInDb object_identifierInDb);
    Task<ObjectIdentifierInDb?> UpdateObjectIdentifierAsync(ObjectIdentifierInDb object_identifierInDb);
    Task<int> DeleteObjectIdentifierAsync(int id, string user_name);

    /****************************************************************
    * Object instances
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectInstanceInDb>?> GetObjectInstancesAsync(string sd_oid);
    Task<ObjectInstanceInDb?> GetObjectInstanceAsync(int? id);
    // Update data
    Task<ObjectInstanceInDb?> CreateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb);
    Task<ObjectInstanceInDb?> UpdateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb);
    Task<int> DeleteObjectInstanceAsync(int id, string user_name);

    /****************************************************************
    * Object relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectRelationshipInDb>?> GetObjectRelationshipsAsync(string sd_oid);
    Task<ObjectRelationshipInDb?> GetObjectRelationshipAsync(int? id);
    // Update data
    Task<ObjectRelationshipInDb?> CreateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb);
    Task<ObjectRelationshipInDb?> UpdateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb);
    Task<int> DeleteObjectRelationshipAsync(int id, string user_name);

    /****************************************************************
    * Object rights
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectRightInDb>?> GetObjectRightsAsync(string sd_oid);
    Task<ObjectRightInDb?> GetObjectRightAsync(int? id);
    // Update data
    Task<ObjectRightInDb?> CreateObjectRightAsync(ObjectRightInDb objectRightInDb);
    Task<ObjectRightInDb?> UpdateObjectRightAsync(ObjectRightInDb objectRightInDb);
    Task<int> DeleteObjectRightAsync(int id, string user_name);
   
    /****************************************************************
    * Object titles
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<ObjectTitleInDb>?> GetObjectTitlesAsync(string sd_oid);
    Task<ObjectTitleInDb?> GetObjectTitleAsync(int? id);
    // Update data
    Task<ObjectTitleInDb?> CreateObjectTitleAsync(ObjectTitleInDb objectTitleInDb);
    Task<ObjectTitleInDb?> UpdateObjectTitleAsync(ObjectTitleInDb objectTitleInDb);
    Task<int> DeleteObjectTitleAsync(int id, string user_name);
  
    /****************************************************************
    * Object topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectTopicInDb>?> GetObjectTopicsAsync(string sd_oid);
    Task<ObjectTopicInDb?> GetObjectTopicAsync(int? id);
    // Update data
    Task<ObjectTopicInDb?> CreateObjectTopicAsync(ObjectTopicInDb objectTopicInDb);
    Task<ObjectTopicInDb?> UpdateObjectTopicAsync(ObjectTopicInDb objectTopicInDb);
    Task<int> DeleteObjectTopicAsync(int id, string user_name);

    // Extensions
    /*
    Task<PaginationResponse<DataObjectInDb>?> PaginateDataObjects(PaginationRequest paginationRequest);
    Task<PaginationResponse<DataObjectInDb>?> FilterDataObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalDataObjects();
    */
}