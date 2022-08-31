using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IObjectRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> ObjectExists(string sdOid);
    Task<bool> ObjectAttributeExists(string sdOid, string typeName, int id);
    
    /****************************************************************
    * Fetch Object / Object entry data 
    ****************************************************************/

    Task<IEnumerable<DataObjectInDb>> GetAllObjectRecords();
    Task<IEnumerable<DataObjectEntryInDb>> GetAllObjectEntries();
    
    Task<IEnumerable<DataObjectInDb>> GetPaginatedObjectRecords(int pNum, int pSize);
    Task<IEnumerable<DataObjectEntryInDb>> GetPaginatedObjectEntries(int pNum, int pSize);
    
    Task<IEnumerable<DataObjectInDb>> GetFilteredObjectRecords(string titleFilter);
    Task<IEnumerable<DataObjectEntryInDb>> GetFilteredObjectEntries(string titleFilter);
    
    Task<IEnumerable<DataObjectInDb>> GetPaginatedFilteredObjectRecords(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DataObjectEntryInDb>> GetPaginatedFilteredObjectEntries(string titleFilter, int pNum, int pSize);
     
    Task<IEnumerable<DataObjectInDb>> GetRecentObjectRecords(int n);
    Task<IEnumerable<DataObjectEntryInDb>> GetRecentObjectEntries(int n);
    
    Task<IEnumerable<DataObjectInDb>> GetObjectRecordsByOrg(int orgId);
    Task<IEnumerable<DataObjectEntryInDb>> GetObjectEntriesByOrg(int orgId);
    
    Task<DataObjectInDb?> GetDataObjectData(string sdOid);
    
    /****************************************************************
    * Update Object data 
    ****************************************************************/
    
    Task<DataObjectInDb?> CreateDataObjectData(DataObjectInDb dataObjectData, bool addTitle);
    Task<DataObjectInDb?> UpdateDataObjectData(DataObjectInDb dataObjectData);
    Task<int> DeleteDataObjectData(string sdOid, string userName);
    
    /****************************************************************
    * Full Data Object data (including attributes in other tables)
    ****************************************************************/
  
    Task<FullObjectInDb?> GetFullObjectById(string sdOid);
    Task<int> DeleteFullObject(string sdOid, string userName);
    
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

    Task<int> GetObjectDtpInvolvement(string sdOid);
    Task<int> GetObjectDupInvolvement(string sdOid);
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectContributorInDb>?> GetObjectContributors(string sdOid);
    Task<ObjectContributorInDb?> GetObjectContributor(int? id);
    
    // Update data
    Task<ObjectContributorInDb?> CreateObjectContributor(ObjectContributorInDb objectContributorInDb);
    Task<ObjectContributorInDb?> UpdateObjectContributor(ObjectContributorInDb objectContributorInDb);
    Task<int> DeleteObjectContributor(int id, string userName);

    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDatasetInDb>?> GetObjectDatasets(string sdOid);
    Task<ObjectDatasetInDb?> GetObjectDataset(int? id);
    
    // Update data
    Task<ObjectDatasetInDb?> CreateObjectDataset(ObjectDatasetInDb objectDatasetInDb);
    Task<ObjectDatasetInDb?> UpdateObjectDataset(ObjectDatasetInDb objectDatasetInDb);
    Task<int> DeleteObjectDataset(int id, string userName);

    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDateInDb>?> GetObjectDates(string sdOid);
    Task<ObjectDateInDb?> GetObjectDate(int? id);
    
    // Update data
    Task<ObjectDateInDb?> CreateObjectDate(ObjectDateInDb objectDateInDb);
    Task<ObjectDateInDb?> UpdateObjectDate(ObjectDateInDb objectDateInDb);
    Task<int> DeleteObjectDate(int id, string userName);

    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectDescriptionInDb>?> GetObjectDescriptions(string sdOid);
    Task<ObjectDescriptionInDb?> GetObjectDescription(int? id);
    
    // Update data
    Task<ObjectDescriptionInDb?> CreateObjectDescription(ObjectDescriptionInDb objectDescriptionInDb);
    Task<ObjectDescriptionInDb?> UpdateObjectDescription(ObjectDescriptionInDb objectDescriptionInDb);
    Task<int> DeleteObjectDescription(int id, string userName);

    /****************************************************************
    * Object identifiers
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectIdentifierInDb>?> GetObjectIdentifiers(string sdOid);
    Task<ObjectIdentifierInDb?> GetObjectIdentifier(int? id);
    
    // Update data
    Task<ObjectIdentifierInDb?> CreateObjectIdentifier(ObjectIdentifierInDb objectIdentifierInDb);
    Task<ObjectIdentifierInDb?> UpdateObjectIdentifier(ObjectIdentifierInDb objectIdentifierInDb);
    Task<int> DeleteObjectIdentifier(int id, string userName);

    /****************************************************************
    * Object instances
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectInstanceInDb>?> GetObjectInstances(string sdOid);
    Task<ObjectInstanceInDb?> GetObjectInstance(int? id);
    
    // Update data
    Task<ObjectInstanceInDb?> CreateObjectInstance(ObjectInstanceInDb objectInstanceInDb);
    Task<ObjectInstanceInDb?> UpdateObjectInstance(ObjectInstanceInDb objectInstanceInDb);
    Task<int> DeleteObjectInstance(int id, string userName);

    /****************************************************************
    * Object relationships
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectRelationshipInDb>?> GetObjectRelationships(string sdOid);
    Task<ObjectRelationshipInDb?> GetObjectRelationship(int? id);
    
    // Update data
    Task<ObjectRelationshipInDb?> CreateObjectRelationship(ObjectRelationshipInDb objectRelationshipInDb);
    Task<ObjectRelationshipInDb?> UpdateObjectRelationship(ObjectRelationshipInDb objectRelationshipInDb);
    Task<int> DeleteObjectRelationship(int id, string userName);

    /****************************************************************
    * Object rights
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectRightInDb>?> GetObjectRights(string sdOid);
    Task<ObjectRightInDb?> GetObjectRight(int? id);
    
    // Update data
    Task<ObjectRightInDb?> CreateObjectRight(ObjectRightInDb objectRightInDb);
    Task<ObjectRightInDb?> UpdateObjectRight(ObjectRightInDb objectRightInDb);
    Task<int> DeleteObjectRight(int id, string userName);
   
    /****************************************************************
    * Object titles
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<ObjectTitleInDb>?> GetObjectTitles(string sdOid);
    Task<ObjectTitleInDb?> GetObjectTitle(int? id);
    
    // Update data
    Task<ObjectTitleInDb?> CreateObjectTitle(ObjectTitleInDb objectTitleInDb);
    Task<ObjectTitleInDb?> UpdateObjectTitle(ObjectTitleInDb objectTitleInDb);
    Task<int> DeleteObjectTitle(int id, string userName);
  
    /****************************************************************
    * Object topics
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<ObjectTopicInDb>?> GetObjectTopics(string sdOid);
    Task<ObjectTopicInDb?> GetObjectTopic(int? id);
    
    // Update data
    Task<ObjectTopicInDb?> CreateObjectTopic(ObjectTopicInDb objectTopicInDb);
    Task<ObjectTopicInDb?> UpdateObjectTopic(ObjectTopicInDb objectTopicInDb);
    Task<int> DeleteObjectTopic(int id, string userName);
   
}