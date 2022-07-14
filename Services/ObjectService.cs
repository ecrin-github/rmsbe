using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Services.Interfaces;

namespace rmsbe.Services;

public class ObjectService : IObjectService
{
    private readonly IObjectRepository _objectRepository;
    private readonly ILookupService _lupService;
    private List<Lup> _lookups;
    private string _userName;

    public ObjectService(IObjectRepository objectRepository, ILookupService lupService)
    {
        _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        _lupService = lupService ?? throw new ArgumentNullException(nameof(lupService));
        _lookups = new List<Lup>();
        
        // for now - need a mechanism to inject this from user object,
        // either directly here or from controller;
        
        DateTime now = DateTime.Now;
        string timestring = now.Hour.ToString() + ":" + now.Minute.ToString() + ":" + now.Second.ToString(); 
        _userName = "test user" + "_" + timestring; 
    }
    
    /****************************************************************
    * Check functions 
    ****************************************************************/   
    
    // Check if data object exists 
    public async Task<bool> ObjectExists(string sdOid)
        => await _objectRepository.ObjectExists(sdOid);
    
    // Check if attribute exists on specified object
    public async Task<bool> ObjectAttributeExists(string sdOid, string typeName, int id)
        => await _objectRepository.ObjectAttributeExists(sdOid, typeName, id);
    
    /****************************************************************
    * All Data object records and object entries
    ****************************************************************/
  
    public async Task<List<DataObjectData>?> GetAllObjectRecords() {
        var objRightsInDb = await _objectRepository.GetAllObjectRecords();
        return objRightsInDb?.Select(r => new DataObjectData(r)).ToList();
    }

    public async Task<List<DataObjectEntry>?> GetAllObjectEntries(){ 
        var objectsInDb = (await _objectRepository.GetAllObjectEntries()).ToList();
        return !objectsInDb.Any() ? null 
            : objectsInDb.Select(r => new DataObjectEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated Data object records and object entries
    ****************************************************************/
    
     public async Task<List<DataObjectData>?> GetPaginatedObjectRecords(PaginationRequest validFilter)
    {
        var pagedObjectsInDb = (await _objectRepository
            .GetPaginatedObjectRecords(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedObjectsInDb.Any() ? null 
            : pagedObjectsInDb.Select(r => new DataObjectData(r)).ToList();
    }
    
    public async Task<List<DataObjectEntry>?> GetPaginatedObjectEntries(PaginationRequest validFilter)
    {
        var pagedObjectsInDb = (await _objectRepository
            .GetPaginatedObjectEntries(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedObjectsInDb.Any() ? null 
            : pagedObjectsInDb.Select(r => new DataObjectEntry(r)).ToList();
    } 
    
    /****************************************************************
    * Filtered Data object records and object entries
    ****************************************************************/        
    
    public async Task<List<DataObjectData>?> GetFilteredObjectRecords(string titleFilter)
    {
        var filteredObjectsInDb = (await _objectRepository
            .GetFilteredObjectRecords(titleFilter)).ToList();
        return !filteredObjectsInDb.Any() ? null 
            : filteredObjectsInDb.Select(r => new DataObjectData(r)).ToList();
    }
     
    
    public async Task<List<DataObjectEntry>?> GetFilteredObjectEntries(string titleFilter)
    {
        var filteredObjectsInDb = (await _objectRepository
            .GetFilteredObjectEntries(titleFilter)).ToList();
        return !filteredObjectsInDb.Any() ? null 
            : filteredObjectsInDb.Select(r => new DataObjectEntry(r)).ToList();
    }

    /****************************************************************
    * Paginated and filtered Data object records and object entries
    ****************************************************************/        
    
    public async Task<List<DataObjectData>?> GetPaginatedFilteredObjectRecords(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredObjectsInDb = (await _objectRepository
            .GetPaginatedFilteredObjectRecords(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredObjectsInDb.Any() ? null 
            : pagedFilteredObjectsInDb.Select(r => new DataObjectData(r)).ToList();
    }
    
    public async Task<List<DataObjectEntry>?> GetPaginatedFilteredObjectEntries(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredObjectsInDb = (await _objectRepository
            .GetPaginatedFilteredObjectEntries(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredObjectsInDb.Any() ? null 
            : pagedFilteredObjectsInDb.Select(r => new DataObjectEntry(r)).ToList();
    }
    
    /****************************************************************
    * Recent Data object records and object entries
    ****************************************************************/
    
    public async Task<List<DataObjectData>?> GetRecentObjectRecords(int n) {
        var objsInDb = (await _objectRepository.GetRecentObjectRecords(n)).ToList();
        return !objsInDb.Any() ? null 
            : objsInDb.Select(r => new DataObjectData(r)).ToList();
    }

    public async Task<List<DataObjectEntry>?> GetRecentObjectEntries(int n){ 
        var recentObjectsInDb = (await _objectRepository.GetRecentObjectEntries(n)).ToList();
        return !recentObjectsInDb.Any() ? null 
            : recentObjectsInDb.Select(r => new DataObjectEntry(r)).ToList();
    }

    /****************************************************************
    * Data object records and object entries by Organisation
    ****************************************************************/
    
    public async Task<List<DataObjectData>?> GetObjectRecordsByOrg(int orgId) {
        var objsByOrgInDb = (await _objectRepository.GetObjectRecordsByOrg(orgId)).ToList();;
        return !objsByOrgInDb.Any() ? null 
            : objsByOrgInDb.Select(r => new DataObjectData(r)).ToList();
    }

    public async Task<List<DataObjectEntry>?> GetObjectEntriesByOrg(int orgId){ 
        var objectEntriesByOrgInDb = (await _objectRepository.GetObjectEntriesByOrg(orgId)).ToList();
        return !objectEntriesByOrgInDb.Any() ? null 
            : objectEntriesByOrgInDb.Select(r => new DataObjectEntry(r)).ToList();
    }    
     
    
    /****************************************************************
    * Fetch a single Data Object record 
    ****************************************************************/
    
    public async Task<DataObjectData?> GetObjectData(string sdOid) {
        var objInDb = await _objectRepository.GetDataObjectData(sdOid);
        return objInDb == null ? null : new DataObjectData(objInDb);
    }    

    /****************************************************************
    * Update Data Object data 
    ****************************************************************/
    
    public async Task<DataObjectData?> CreateDataObjectData(DataObjectData dataObjectContent) {
        var dataObjectContentInDb = new DataObjectInDb(dataObjectContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateDataObjectData(dataObjectContentInDb);
        return res == null ? null : new DataObjectData(res);
    } 

    public async Task<DataObjectData?> UpdateDataObjectData(DataObjectData dataObjectContent) {
        var dataObjectContentInDb = new DataObjectInDb(dataObjectContent) { last_edited_by = _userName };
        var res = await _objectRepository.UpdateDataObjectData(dataObjectContentInDb);
        return res == null ? null : new DataObjectData(res);
    } 
    
    public async Task<int> DeleteDataObject(string sdOid)
           => await _objectRepository.DeleteDataObjectData(sdOid, _userName);
    

    /****************************************************************
    * Full Data object...(with attribute data)
    ****************************************************************/
    
    // Fetch data 
    
    public async Task<FullDataObject?> GetFullObjectById(string sdOid) {
        var fullObjectInDb = await _objectRepository.GetFullObjectById(sdOid);
        return fullObjectInDb == null ? null : new FullDataObject(fullObjectInDb);
    }    
    
    // Update data
    public async Task<int> DeleteFullObject(string sdOid)
           => await _objectRepository.DeleteFullObject( sdOid, _userName);


    public async Task<FullDataObject?> GetFullObjectFromMdr(string sdSid, int mdrId)
    {
       FullObjectInDb? fullObjectFromMdrInDb = null;       
       
       // First obtain the sdOid for this object
       string? sdOid = await _objectRepository.GetSdOidFromMdr(sdSid, mdrId);
       if (sdOid != null)
       {
           
           var objectInMdr = await _objectRepository.GetObjectDataFromMdr(mdrId);
           if (objectInMdr != null)
           {
               // Create a new new object record in the format expected by the RMS
               // add in the user details and store in the RMS objects table

               var newObjectInDb = new DataObjectInDb(objectInMdr, sdSid, sdOid)
               {
                   last_edited_by = _userName
               };
               var objectInRmsDb = await _objectRepository.CreateDataObjectData(newObjectInDb);

               // Assuming new object record creation was successful, fetch and 
               // store object attributes, transferring from Mdr to Rms format and Ids
               // (The int object id must be replaced by the string sd_oid)

               if (objectInRmsDb != null)
               {
                   fullObjectFromMdrInDb = await _objectRepository.GetFullObjectDataFromMdr(objectInRmsDb, mdrId);
               }
           }
       }
       
       return fullObjectFromMdrInDb == null ? null : new FullDataObject(fullObjectFromMdrInDb);
    }
    
    
    /****************************************************************
    * Statistics
    ****************************************************************/

    public async Task<Statistic> GetTotalObjects()
    {
        int res = await _objectRepository.GetTotalObjects();
        return new Statistic("Total", res);
    }

    public async Task<Statistic> GetTotalFilteredObjects(string titleFilter)
    {
        int res = await _objectRepository.GetTotalFilteredObjects(titleFilter);
        return new Statistic("TotalFiltered", res);
    }
    
    public async Task<List<Statistic>?> GetObjectsByType()
    {
        var res = (await _objectRepository.GetObjectsByType()).ToList();
        if (await ResetLookups("object-types"))
        {
            return !res.Any()
                ? null
                : res.Select(r => new Statistic(LuTypeName(r.stat_type), r.stat_value)).ToList();
        }
        return null;
    }
    
    private string LuTypeName(int n)
    {
        foreach (var p in _lookups.Where(p => n == p.Id))
        {
            return p.Name ?? "null name in matching lookup!";
        }
        return "not known";
    }
    
    private async Task<bool> ResetLookups(string typeName)
    {
        _lookups = new List<Lup>();  // reset to empty list
        _lookups = await _lupService.GetLookUpValues(typeName);
        return _lookups.Count > 0 ;
    }
    
    
    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDataset>?> GetObjectDatasets(string sdOid) {
        var objDatasetsInDb = await _objectRepository.GetObjectDatasets(sdOid);
        return objDatasetsInDb?.Select(r => new ObjectDataset(r)).ToList();
    }
    
    public async Task<ObjectDataset?> GetObjectDataset(int id) {
        var objDatasetInDb = await _objectRepository.GetObjectDataset(id);
        return objDatasetInDb == null ? null : new ObjectDataset(objDatasetInDb);
    }    

    // Update data
    public async Task<ObjectDataset?> CreateObjectDataset(ObjectDataset objDatasetContent) {
        var objDatasetContentInDb = new ObjectDatasetInDb(objDatasetContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectDataset(objDatasetContentInDb);
        return res == null ? null : new ObjectDataset(res);
    } 

    public async Task<ObjectDataset?> UpdateObjectDataset(int aId, ObjectDataset objDatasetContent) {
        var objDatasetContentInDb = new ObjectDatasetInDb(objDatasetContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectDataset(objDatasetContentInDb);
        return res == null ? null : new ObjectDataset(res);
    } 
    
    public async Task<int> DeleteObjectDataset(int id)
           => await _objectRepository.DeleteObjectDataset(id, _userName);
    
    
    /****************************************************************
    * Object titles
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectTitle>?> GetObjectTitles(string sdOid) {
        var objTitlesInDb = await _objectRepository.GetObjectTitles(sdOid);
        return objTitlesInDb?.Select(r => new ObjectTitle(r)).ToList();
    }
    
    public async Task<ObjectTitle?> GetObjectTitle(int id) {
        var objTitleInDb = await _objectRepository.GetObjectTitle(id);
        return objTitleInDb == null ? null : new ObjectTitle(objTitleInDb);
    }    

    // Update data
    public async Task<ObjectTitle?> CreateObjectTitle(ObjectTitle objTitleContent) {
        var objTitleContentInDb = new ObjectTitleInDb(objTitleContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectTitle(objTitleContentInDb);
        return res == null ? null : new ObjectTitle(res);
    } 

    public async Task<ObjectTitle?> UpdateObjectTitle(int aId, ObjectTitle objTitleContent) {
        var objTitleContentInDb = new ObjectTitleInDb(objTitleContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectTitle(objTitleContentInDb);
        return res == null ? null : new ObjectTitle(res);
    } 
    
    public async Task<int> DeleteObjectTitle(int id)
           => await _objectRepository.DeleteObjectTitle(id, _userName);
    
    
    /****************************************************************
    * Object instances
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectInstance>?> GetObjectInstances(string sdOid) {
        var objInstancesInDb = await _objectRepository.GetObjectInstances(sdOid);
        return objInstancesInDb?.Select(r => new ObjectInstance(r)).ToList();
    }
   
    public async Task<ObjectInstance?> GetObjectInstance(int id) {
        var objInstanceInDb = await _objectRepository.GetObjectInstance(id);
        return objInstanceInDb == null ? null : new ObjectInstance(objInstanceInDb);
    }    

    // Update data
    public async Task<ObjectInstance?> CreateObjectInstance(ObjectInstance objInstContent) {
        var objInstContentInDb = new ObjectInstanceInDb(objInstContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectInstance(objInstContentInDb);
        return res == null ? null : new ObjectInstance(res);
    } 

    public async Task<ObjectInstance?> UpdateObjectInstance(int aId, ObjectInstance objInstContent) {
        var objInstContentInDb = new ObjectInstanceInDb(objInstContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectInstance(objInstContentInDb);
        return res == null ? null : new ObjectInstance(res);
    } 
    
    public async Task<int> DeleteObjectInstance(int id)
           => await _objectRepository.DeleteObjectInstance(id, _userName);
    
    
    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDate>?> GetObjectDates(string sdOid) {
        var objDatesInDb = await _objectRepository.GetObjectDates(sdOid);
        return objDatesInDb?.Select(r => new ObjectDate(r)).ToList();
    }
  
    public async Task<ObjectDate?> GetObjectDate(int id) {
        var objDateInDb = await _objectRepository.GetObjectDate(id);
        return objDateInDb == null ? null : new ObjectDate(objDateInDb);
    }    

    // Update data
    public async Task<ObjectDate?> CreateObjectDate(ObjectDate objDateContent) {
        var objDateContentInDb = new ObjectDateInDb(objDateContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectDate(objDateContentInDb);
        return res == null ? null : new ObjectDate(res);
    } 

    public async Task<ObjectDate?> UpdateObjectDate(int aId, ObjectDate objDateContent) {
        var objDateContentInDb = new ObjectDateInDb(objDateContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectDate(objDateContentInDb);
        return res == null ? null : new ObjectDate(res);
    } 
    
    public async Task<int> DeleteObjectDate(int id)
           => await _objectRepository.DeleteObjectDate(id, _userName);
    
    
    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDescription>?> GetObjectDescriptions(string sdOid) {
        var objDescsInDb = await _objectRepository.GetObjectDescriptions(sdOid);
        return objDescsInDb?.Select(r => new ObjectDescription(r)).ToList();
    }
   
    public async Task<ObjectDescription?> GetObjectDescription(int id) {
        var objDescInDb = await _objectRepository.GetObjectDescription(id);
        return objDescInDb == null ? null : new ObjectDescription(objDescInDb);
    }    

    // Update data
    public async Task<ObjectDescription?> CreateObjectDescription(ObjectDescription objDescContent) {
        var objDescContentInDb = new ObjectDescriptionInDb(objDescContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectDescription(objDescContentInDb);
        return res == null ? null : new ObjectDescription(res);
    } 

    public async Task<ObjectDescription?> UpdateObjectDescription(int aId, ObjectDescription objDescContent) {
        var objDescContentInDb = new ObjectDescriptionInDb(objDescContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectDescription(objDescContentInDb);
        return res == null ? null : new ObjectDescription(res);
    } 
    
    public async Task<int> DeleteObjectDescription(int id)
           => await _objectRepository.DeleteObjectDescription(id, _userName);
    
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectContributor>?> GetObjectContributors(string sdOid) {
        var objContsInDb = await _objectRepository.GetObjectContributors(sdOid);
        return objContsInDb?.Select(r => new ObjectContributor(r)).ToList();
    }
  
    public async Task<ObjectContributor?> GetObjectContributor(int id) {
        var objContInDb = await _objectRepository.GetObjectContributor(id);
        return objContInDb == null ? null : new ObjectContributor(objContInDb);
    }    

    // Update data
    public async Task<ObjectContributor?> CreateObjectContributor(ObjectContributor objContContent) {
        var objContContentInDb = new ObjectContributorInDb(objContContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectContributor(objContContentInDb);
        return res == null ? null : new ObjectContributor(res);
    } 

    public async Task<ObjectContributor?> UpdateObjectContributor(int aId, ObjectContributor objContContent) {
        var objRightContentInDb = new ObjectContributorInDb(objContContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectContributor(objRightContentInDb);
        return res == null ? null : new ObjectContributor(res);
    } 
    
    public async Task<int> DeleteObjectContributor(int id)
           => await _objectRepository.DeleteObjectContributor(id, _userName);
    
    
    /****************************************************************
    * Object topics
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectTopic>?> GetObjectTopics(string sdOid) {
        var objTopicsInDb = await _objectRepository.GetObjectTopics(sdOid);
        return objTopicsInDb?.Select(r => new ObjectTopic(r)).ToList();
    }
   
    public async Task<ObjectTopic?> GetObjectTopic(int id) {
        var objTopicInDb = await _objectRepository.GetObjectTopic(id);
        return objTopicInDb == null ? null : new ObjectTopic(objTopicInDb);
    }    

    // Update data
    public async Task<ObjectTopic?> CreateObjectTopic(ObjectTopic objTopicContent) {
        var objTopicContentInDb = new ObjectTopicInDb(objTopicContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectTopic(objTopicContentInDb);
        return res == null ? null : new ObjectTopic(res);
    } 

    public async Task<ObjectTopic?> UpdateObjectTopic(int aId, ObjectTopic objTopicContent) {
        var objTopicContentInDb = new ObjectTopicInDb(objTopicContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectTopic(objTopicContentInDb);
        return res == null ? null : new ObjectTopic(res);
    } 
    
    public async Task<int> DeleteObjectTopic(int id)
           => await _objectRepository.DeleteObjectTopic(id, _userName);
    
    
    /****************************************************************
    * Object identifiers
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectIdentifier>?> GetObjectIdentifiers(string sdOid) {
        var aIdentsInDb = await _objectRepository.GetObjectIdentifiers(sdOid);
        return aIdentsInDb?.Select(r => new ObjectIdentifier(r)).ToList();
    }
     
    public async Task<ObjectIdentifier?> GetObjectIdentifier(int id) { 
        var aIdentInDb = await _objectRepository.GetObjectIdentifier(id);
        return aIdentInDb == null ? null : new ObjectIdentifier(aIdentInDb);
    }    

    // Update data
    public async Task<ObjectIdentifier?> CreateObjectIdentifier(ObjectIdentifier aIdentContent) {
        var aIdentContentInDb = new ObjectIdentifierInDb(aIdentContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectIdentifier(aIdentContentInDb);
        return res == null ? null : new ObjectIdentifier(res);
    } 

    public async Task<ObjectIdentifier?> UpdateObjectIdentifier(int aId, ObjectIdentifier aIdentContent) {
        var aIdentContentInDb = new ObjectIdentifierInDb(aIdentContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectIdentifier(aIdentContentInDb);
        return res == null ? null : new ObjectIdentifier(res);
    } 
    
    public async Task<int> DeleteObjectIdentifier(int id)
           => await _objectRepository.DeleteObjectIdentifier(id, _userName);
    
    
    /****************************************************************
    * Object relationships
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectRelationship>?> GetObjectRelationships(string sdOid) {
        var objRelsInDb = await _objectRepository.GetObjectRelationships(sdOid);
        return objRelsInDb?.Select(r => new ObjectRelationship(r)).ToList();
    }
   
    public async Task<ObjectRelationship?> GetObjectRelationship(int id) {
        var objRelInDb = await _objectRepository.GetObjectRelationship(id);
        return objRelInDb == null ? null : new ObjectRelationship(objRelInDb);
    }    

    // Update data
    public async Task<ObjectRelationship?> CreateObjectRelationship(ObjectRelationship objRelContent) {
        var objRelContentInDb = new ObjectRelationshipInDb(objRelContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectRelationship(objRelContentInDb);
        return res == null ? null : new ObjectRelationship(res);
    } 
    
    public async Task<ObjectRelationship?> UpdateObjectRelationship(int aId, ObjectRelationship objRelContent) {
        var objRelContentInDb = new ObjectRelationshipInDb(objRelContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectRelationship(objRelContentInDb);
        return res == null ? null : new ObjectRelationship(res);
    } 
    
    public async Task<int> DeleteObjectRelationship(int id)
           => await _objectRepository.DeleteObjectRelationship(id, _userName);
    
    
    /****************************************************************
    * Object rights
    ****************************************************************/
  
    // Fetch data

    public async Task<List<ObjectRight>?> GetObjectRights(string sdOid) {
        var objRightsInDb = await _objectRepository.GetObjectRights(sdOid);
        return objRightsInDb?.Select(r => new ObjectRight(r)).ToList();
    }

    public async Task<ObjectRight?> GetObjectRight(int id) {
        var objRightInDb = await _objectRepository.GetObjectRight(id);
        return objRightInDb == null ? null : new ObjectRight(objRightInDb);
    }    
    
    // Update data
    
    public async Task<ObjectRight?> CreateObjectRight(ObjectRight objRightContent) {
        var objRightContentInDb = new ObjectRightInDb(objRightContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectRight(objRightContentInDb);
        return res == null ? null : new ObjectRight(res);
    } 
    
    public async Task<ObjectRight?> UpdateObjectRight(int aId, ObjectRight objRightContent) {
        var objRightContentInDb = new ObjectRightInDb(objRightContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectRight(objRightContentInDb);
        return res == null ? null : new ObjectRight(res);
    } 
    
    public async Task<int> DeleteObjectRight(int id)
           => await _objectRepository.DeleteObjectRight(id, _userName);
    
}