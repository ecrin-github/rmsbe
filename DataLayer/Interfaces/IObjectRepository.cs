using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IObjectRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> ObjectExistsAsync(string sdOid);
    Task<bool> ObjectAttributeExistsAsync(string sdOid, string typeName, int id);
    
    /****************************************************************
    * Data Object data (without attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DataObjectInDb>> GetDataObjectsDataAsync();
    Task<IEnumerable<DataObjectInDb>> GetRecentObjectDataAsync(int limit);
    Task<IEnumerable<DataObjectInDb>> GetPaginatedObjectDataAsync(int pNum, int pSize);
    Task<IEnumerable<DataObjectInDb>> GetPaginatedFilteredObjectDataAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DataObjectInDb>> GetFilteredObjectDataAsync(string titleFilter);
    
    Task<IEnumerable<DataObjectEntryInDb>> GetObjectEntriesAsync();
    Task<IEnumerable<DataObjectEntryInDb>> GetRecentObjectEntriesAsync(int n);
    Task<IEnumerable<DataObjectEntryInDb>> GetPaginatedObjectEntriesAsync(int pNum, int pSize);
    Task<IEnumerable<DataObjectEntryInDb>> GetPaginatedFilteredObjectEntriesAsync(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DataObjectEntryInDb>> GetFilteredObjectEntriesAsync(string titleFilter);
    
    Task<DataObjectInDb?> GetDataObjectDataAsync(string sdOid);
    
    // Update data
    Task<DataObjectInDb?> CreateDataObjectDataAsync(DataObjectInDb dataObjectData);
    Task<DataObjectInDb?> UpdateDataObjectDataAsync(DataObjectInDb dataObjectData);
    Task<int> DeleteDataObjectDataAsync(string sdOid, string userName);
    
    /****************************************************************
    * Full Data Object data (including attributes in other tables)
    ****************************************************************/
  
    Task<FullObjectInDb?> GetFullObjectByIdAsync(string sdOid);
    Task<int> DeleteFullObjectAsync(string sdOid, string userName);
    
    /****************************************************************
    * Full Data Object data from the MDR
    ****************************************************************/
    
    Task<string?> GetSdOidFromMdr(string sdSid, int mdrId);
    Task<DataObjectInMdr?> GetObjectDataFromMdr(int mdrId);
    Task<FullObjectInDb?> GetFullObjectDataFromMdr(DataObjectInDb studyInRmsDb, int mdrId);
    
    /****************************************************************
    * Object statistics
    ****************************************************************/
    
    Task<int> GetTotalObjects();
    Task<int> GetTotalFilteredObjects(string titleFilter);
    Task<IEnumerable<StatisticInDb>> GetObjectsByType();
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectContributorInDb>?> GetObjectContributorsAsync(string sdOid);
    Task<ObjectContributorInDb?> GetObjectContributorAsync(int? id);
    // Update data
    Task<ObjectContributorInDb?> CreateObjectContributorAsync(ObjectContributorInDb objectContributorInDb);
    Task<ObjectContributorInDb?> UpdateObjectContributorAsync(ObjectContributorInDb objectContributorInDb);
    Task<int> DeleteObjectContributorAsync(int id, string userName);

    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDatasetInDb>?> GetObjectDatasetsAsync(string sdOid);
    Task<ObjectDatasetInDb?> GetObjectDatasetAsync(int? id);
    // Update data
    Task<ObjectDatasetInDb?> CreateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb);
    Task<ObjectDatasetInDb?> UpdateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb);
    Task<int> DeleteObjectDatasetAsync(int id, string userName);

    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDateInDb>?> GetObjectDatesAsync(string sdOid);
    Task<ObjectDateInDb?> GetObjectDateAsync(int? id);
    // Update data
    Task<ObjectDateInDb?> CreateObjectDateAsync(ObjectDateInDb objectDateInDb);
    Task<ObjectDateInDb?> UpdateObjectDateAsync(ObjectDateInDb objectDateInDb);
    Task<int> DeleteObjectDateAsync(int id, string userName);

    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDescriptionInDb>?> GetObjectDescriptionsAsync(string sdOid);
    Task<ObjectDescriptionInDb?> GetObjectDescriptionAsync(int? id);
    // Update data
    Task<ObjectDescriptionInDb?> CreateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb);
    Task<ObjectDescriptionInDb?> UpdateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb);
    Task<int> DeleteObjectDescriptionAsync(int id, string userName);

    /****************************************************************
    * Object identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectIdentifierInDb>?> GetObjectIdentifiersAsync(string sdOid);
    Task<ObjectIdentifierInDb?> GetObjectIdentifierAsync(int? id);
    // Update data
    Task<ObjectIdentifierInDb?> CreateObjectIdentifierAsync(ObjectIdentifierInDb objectIdentifierInDb);
    Task<ObjectIdentifierInDb?> UpdateObjectIdentifierAsync(ObjectIdentifierInDb objectIdentifierInDb);
    Task<int> DeleteObjectIdentifierAsync(int id, string userName);

    /****************************************************************
    * Object instances
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectInstanceInDb>?> GetObjectInstancesAsync(string sdOid);
    Task<ObjectInstanceInDb?> GetObjectInstanceAsync(int? id);
    // Update data
    Task<ObjectInstanceInDb?> CreateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb);
    Task<ObjectInstanceInDb?> UpdateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb);
    Task<int> DeleteObjectInstanceAsync(int id, string userName);

    /****************************************************************
    * Object relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectRelationshipInDb>?> GetObjectRelationshipsAsync(string sdOid);
    Task<ObjectRelationshipInDb?> GetObjectRelationshipAsync(int? id);
    // Update data
    Task<ObjectRelationshipInDb?> CreateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb);
    Task<ObjectRelationshipInDb?> UpdateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb);
    Task<int> DeleteObjectRelationshipAsync(int id, string userName);

    /****************************************************************
    * Object rights
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectRightInDb>?> GetObjectRightsAsync(string sdOid);
    Task<ObjectRightInDb?> GetObjectRightAsync(int? id);
    // Update data
    Task<ObjectRightInDb?> CreateObjectRightAsync(ObjectRightInDb objectRightInDb);
    Task<ObjectRightInDb?> UpdateObjectRightAsync(ObjectRightInDb objectRightInDb);
    Task<int> DeleteObjectRightAsync(int id, string userName);
   
    /****************************************************************
    * Object titles
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<ObjectTitleInDb>?> GetObjectTitlesAsync(string sdOid);
    Task<ObjectTitleInDb?> GetObjectTitleAsync(int? id);
    // Update data
    Task<ObjectTitleInDb?> CreateObjectTitleAsync(ObjectTitleInDb objectTitleInDb);
    Task<ObjectTitleInDb?> UpdateObjectTitleAsync(ObjectTitleInDb objectTitleInDb);
    Task<int> DeleteObjectTitleAsync(int id, string userName);
  
    /****************************************************************
    * Object topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectTopicInDb>?> GetObjectTopicsAsync(string sdOid);
    Task<ObjectTopicInDb?> GetObjectTopicAsync(int? id);
    // Update data
    Task<ObjectTopicInDb?> CreateObjectTopicAsync(ObjectTopicInDb objectTopicInDb);
    Task<ObjectTopicInDb?> UpdateObjectTopicAsync(ObjectTopicInDb objectTopicInDb);
    Task<int> DeleteObjectTopicAsync(int id, string userName);
   
}