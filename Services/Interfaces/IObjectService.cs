using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IObjectService
{
    /****************************************************************
    * Check functions
    ****************************************************************/

    // Check if data object exists 
    Task<bool> ObjectExists(string sdOid);

    // Check if attribute exists on specified object
    Task<bool> ObjectAttributeExists(string sdOid, string typeName, int id);

    /****************************************************************
    * Fetch Object / Object entry data 
    ****************************************************************/

    Task<List<DataObjectData>?> GetAllObjectsData();
    Task<List<DataObjectEntry>?> GetAllObjectEntries();
    
    Task<List<DataObjectData>?> GetPaginatedObjectData(PaginationRequest validFilter);
    Task<List<DataObjectEntry>?> GetPaginatedObjectEntries(PaginationRequest validFilter);
    
    Task<List<DataObjectData>?> GetFilteredObjectRecords(string titleFilter);
    Task<List<DataObjectEntry>?> GetFilteredObjectEntries(string titleFilter);
    
    Task<List<DataObjectData>?> GetPaginatedFilteredObjectRecords(string titleFilter, PaginationRequest validFilter);
    Task<List<DataObjectEntry>?> GetPaginatedFilteredObjectEntries(string titleFilter, PaginationRequest validFilter);
  
    Task<List<DataObjectData>?> GetRecentObjectsData(int n);
    Task<List<DataObjectEntry>?> GetRecentObjectEntries(int n);
    
    Task<List<DataObjectData>?> GetObjectsByOrg(int orgId);
    Task<List<DataObjectEntry>?> GetObjectEntriesByOrg(int orgId);
    
    Task<DataObjectData?> GetObjectData(string sdOid);

    /****************************************************************
   * Update Object data 
   ****************************************************************/
    
    Task<DataObjectData?> CreateDataObjectData(DataObjectData dataObjectContent);
    Task<DataObjectData?> UpdateDataObjectData(DataObjectData dataObjectContent);
    Task<int> DeleteDataObject(string sdOid);

    /****************************************************************
    * Full Data object...(with attribute data)
    ****************************************************************/

    Task<FullDataObject?> GetFullObjectById(string sdOid);
    Task<int> DeleteFullObject(string sdOid);
    
    /****************************************************************
    * Full object data (including attributes in other tables)
    * fetch from the MDR and store in the RMS DB 
    ****************************************************************/
    
    Task<FullDataObject?> GetFullObjectFromMdr(string sdSid, int mdrId);

    /****************************************************************
    * Statistics
    ****************************************************************/

    Task<Statistic> GetTotalObjects();
    Task<Statistic> GetTotalFilteredObjects(string titleFilter);
    Task<List<Statistic>?> GetObjectsByType();
    
    /****************************************************************
    * Object datasets
    ****************************************************************/

    // Fetch data
    Task<List<ObjectDataset>?> GetObjectDatasets(string sdOid);
    Task<ObjectDataset?> GetObjectDataset(int id);

    // Update data
    Task<ObjectDataset?> CreateObjectDataset(ObjectDataset objDatasetContent);
    Task<ObjectDataset?> UpdateObjectDataset(int id, ObjectDataset objDatasetContent);
    Task<int> DeleteObjectDataset(int id);

    /****************************************************************
    * Object titles
    ****************************************************************/

    // Fetch data
    Task<List<ObjectTitle>?> GetObjectTitles(string sdOid);
    Task<ObjectTitle?> GetObjectTitle(int id);

    // Update data
    Task<ObjectTitle?> CreateObjectTitle(ObjectTitle objTitleContent);
    Task<ObjectTitle?> UpdateObjectTitle(int id, ObjectTitle objTitleContent);
    Task<int> DeleteObjectTitle(int id);

    /****************************************************************
    * Object instances
    ****************************************************************/

    // Fetch data
    Task<List<ObjectInstance>?> GetObjectInstances(string sdOid);
    Task<ObjectInstance?> GetObjectInstance(int id);

    // Update data
    Task<ObjectInstance?> CreateObjectInstance(ObjectInstance objInstanceContent);
    Task<ObjectInstance?> UpdateObjectInstance(int id, ObjectInstance objInstanceContent);
    Task<int> DeleteObjectInstance(int id);

    /****************************************************************
    * Object dates 
    ****************************************************************/

    // Fetch data
    Task<List<ObjectDate>?> GetObjectDates(string sdOid);
    Task<ObjectDate?> GetObjectDate(int id);

    // Update data
    Task<ObjectDate?> CreateObjectDate(ObjectDate objDateContent);
    Task<ObjectDate?> UpdateObjectDate(int id, ObjectDate objDateContent);
    Task<int> DeleteObjectDate(int id);

    /****************************************************************
    * Object descriptions
    ****************************************************************/

    // Fetch data
    Task<List<ObjectDescription>?> GetObjectDescriptions(string sdOid);
    Task<ObjectDescription?> GetObjectDescription(int id);

    // Update data
    Task<ObjectDescription?> CreateObjectDescription(ObjectDescription objDescContent);
    Task<ObjectDescription?> UpdateObjectDescription(int id, ObjectDescription objDescContent);
    Task<int> DeleteObjectDescription(int id);

    /****************************************************************
    * Object contributors
    ****************************************************************/

    // Fetch data
    Task<List<ObjectContributor>?> GetObjectContributors(string sdOid);
    Task<ObjectContributor?> GetObjectContributor(int id);

    // Update data
    Task<ObjectContributor?> CreateObjectContributor(ObjectContributor objContContent);
    Task<ObjectContributor?> UpdateObjectContributor(int id, ObjectContributor objContContent);
    Task<int> DeleteObjectContributor(int id);

    /****************************************************************
    * Object topics
    ****************************************************************/

    // Fetch data
    Task<List<ObjectTopic>?> GetObjectTopics(string sdOid);
    Task<ObjectTopic?> GetObjectTopic(int id);

    // Update data
    Task<ObjectTopic?> CreateObjectTopic(ObjectTopic objTopicContent);
    Task<ObjectTopic?> UpdateObjectTopic(int id, ObjectTopic objTopicContent);
    Task<int> DeleteObjectTopic(int id);

    /****************************************************************
    * Object identifiers
    ****************************************************************/

    // Fetch data
    Task<List<ObjectIdentifier>?> GetObjectIdentifiers(string sdOid);
    Task<ObjectIdentifier?> GetObjectIdentifier(int id);

    // Update data
    Task<ObjectIdentifier?> CreateObjectIdentifier(ObjectIdentifier objIdentContent);
    Task<ObjectIdentifier?> UpdateObjectIdentifier(int id, ObjectIdentifier objIdentContent);
    Task<int> DeleteObjectIdentifier(int id);

    /****************************************************************
    * Object relationships
    ****************************************************************/

    // Fetch data
    Task<List<ObjectRelationship>?> GetObjectRelationships(string sdOid);
    Task<ObjectRelationship?> GetObjectRelationship(int id);

    // Update data
    Task<ObjectRelationship?> CreateObjectRelationship(ObjectRelationship objRelContent);
    Task<ObjectRelationship?> UpdateObjectRelationship(int id, ObjectRelationship objRelContent);
    Task<int> DeleteObjectRelationship(int id);

    /****************************************************************
    * Object rights
    ****************************************************************/

    // Fetch data
    Task<List<ObjectRight>?> GetObjectRights(string sdOid);
    Task<ObjectRight?> GetObjectRight(int id);

    // Update data
    Task<ObjectRight?> CreateObjectRight(ObjectRight objRightContent);
    Task<ObjectRight?> UpdateObjectRight(int id, ObjectRight objRightContent);
    Task<int> DeleteObjectRight(int id);
}

