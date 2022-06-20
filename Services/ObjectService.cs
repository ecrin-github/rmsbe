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
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/   
    
    // Check if data object exists 
    public async Task<bool> ObjectExistsAsync(string sdOid)
        => await _objectRepository.ObjectExistsAsync(sdOid);
    
    // Check if attribute exists on specified object
    public async Task<bool> ObjectAttributeExistsAsync(string sdOid, string typeName, int id)
        => await _objectRepository.ObjectAttributeExistsAsync(sdOid, typeName, id);
    
    /****************************************************************
    * Data object... (data object data only, no attributes)
    ****************************************************************/
  
    // Fetch data 
    public async Task<List<DataObjectData>?> GetAllObjectsDataAsync() {
        var objRightsInDb = await _objectRepository.GetDataObjectsDataAsync();
        return objRightsInDb?.Select(r => new DataObjectData(r)).ToList();
    }
     
    public async Task<List<DataObjectData>?> GetRecentObjectsDataAsync(int n) {
        var objsInDb = await _objectRepository.GetRecentObjectDataAsync(n);
        return objsInDb?.Select(r => new DataObjectData(r)).ToList();
    }
 
    public async Task<DataObjectData?> GetObjectDataAsync(string sdOid) {
        var objInDb = await _objectRepository.GetDataObjectDataAsync(sdOid);
        return objInDb == null ? null : new DataObjectData(objInDb);
    }    

    // Update data
    public async Task<DataObjectData?> CreateDataObjectDataAsync(DataObjectData dataObjectContent) {
        var dataObjectContentInDb = new DataObjectInDb(dataObjectContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateDataObjectDataAsync(dataObjectContentInDb);
        return res == null ? null : new DataObjectData(res);
    } 

    public async Task<DataObjectData?> UpdateDataObjectDataAsync(DataObjectData dataObjectContent) {
        var dataObjectContentInDb = new DataObjectInDb(dataObjectContent) { last_edited_by = _userName };
        var res = await _objectRepository.UpdateDataObjectDataAsync(dataObjectContentInDb);
        return res == null ? null : new DataObjectData(res);
    } 
    
    public async Task<int> DeleteDataObjectAsync(string sdOid)
           => await _objectRepository.DeleteDataObjectDataAsync(sdOid, _userName);
    
    
    /****************************************************************
    * Full Data object...(with attribute data)
    ****************************************************************/
    
    // Fetch data 
    
    public async Task<FullDataObject?> GetFullObjectByIdAsync(string sdOid) {
        var fullObjectInDb = await _objectRepository.GetFullObjectByIdAsync(sdOid);
        return fullObjectInDb == null ? null : new FullDataObject(fullObjectInDb);
    }    
    
    // Update data
    public async Task<int> DeleteFullObjectAsync(string sdOid)
           => await _objectRepository.DeleteFullObjectAsync( sdOid, _userName);
    
    
    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDataset>?> GetObjectDatasetsAsync(string sdOid) {
        var objDatasetsInDb = await _objectRepository.GetObjectDatasetsAsync(sdOid);
        return objDatasetsInDb?.Select(r => new ObjectDataset(r)).ToList();
    }
    
    public async Task<ObjectDataset?> GetObjectDatasetAsync(int id) {
        var objDatasetInDb = await _objectRepository.GetObjectDatasetAsync(id);
        return objDatasetInDb == null ? null : new ObjectDataset(objDatasetInDb);
    }    

    // Update data
    public async Task<ObjectDataset?> CreateObjectDatasetAsync(ObjectDataset objDatasetContent) {
        var objDatasetContentInDb = new ObjectDatasetInDb(objDatasetContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectDatasetAsync(objDatasetContentInDb);
        return res == null ? null : new ObjectDataset(res);
    } 

    public async Task<ObjectDataset?> UpdateObjectDatasetAsync(int aId, ObjectDataset objDatasetContent) {
        var objDatasetContentInDb = new ObjectDatasetInDb(objDatasetContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectDatasetAsync(objDatasetContentInDb);
        return res == null ? null : new ObjectDataset(res);
    } 
    
    public async Task<int> DeleteObjectDatasetAsync(int id)
           => await _objectRepository.DeleteObjectDatasetAsync(id, _userName);
    
    
    /****************************************************************
    * Object titles
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectTitle>?> GetObjectTitlesAsync(string sdOid) {
        var objTitlesInDb = await _objectRepository.GetObjectTitlesAsync(sdOid);
        return objTitlesInDb?.Select(r => new ObjectTitle(r)).ToList();
    }
    
    public async Task<ObjectTitle?> GetObjectTitleAsync(int id) {
        var objTitleInDb = await _objectRepository.GetObjectTitleAsync(id);
        return objTitleInDb == null ? null : new ObjectTitle(objTitleInDb);
    }    

    // Update data
    public async Task<ObjectTitle?> CreateObjectTitleAsync(ObjectTitle objTitleContent) {
        var objTitleContentInDb = new ObjectTitleInDb(objTitleContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectTitleAsync(objTitleContentInDb);
        return res == null ? null : new ObjectTitle(res);
    } 

    public async Task<ObjectTitle?> UpdateObjectTitleAsync(int aId, ObjectTitle objTitleContent) {
        var objTitleContentInDb = new ObjectTitleInDb(objTitleContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectTitleAsync(objTitleContentInDb);
        return res == null ? null : new ObjectTitle(res);
    } 
    
    public async Task<int> DeleteObjectTitleAsync(int id)
           => await _objectRepository.DeleteObjectTitleAsync(id, _userName);
    
    
    /****************************************************************
    * Object instances
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectInstance>?> GetObjectInstancesAsync(string sdOid) {
        var objInstancesInDb = await _objectRepository.GetObjectInstancesAsync(sdOid);
        return objInstancesInDb?.Select(r => new ObjectInstance(r)).ToList();
    }
   
    public async Task<ObjectInstance?> GetObjectInstanceAsync(int id) {
        var objInstanceInDb = await _objectRepository.GetObjectInstanceAsync(id);
        return objInstanceInDb == null ? null : new ObjectInstance(objInstanceInDb);
    }    

    // Update data
    public async Task<ObjectInstance?> CreateObjectInstanceAsync(ObjectInstance objInstContent) {
        var objInstContentInDb = new ObjectInstanceInDb(objInstContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectInstanceAsync(objInstContentInDb);
        return res == null ? null : new ObjectInstance(res);
    } 

    public async Task<ObjectInstance?> UpdateObjectInstanceAsync(int aId, ObjectInstance objInstContent) {
        var objInstContentInDb = new ObjectInstanceInDb(objInstContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectInstanceAsync(objInstContentInDb);
        return res == null ? null : new ObjectInstance(res);
    } 
    
    public async Task<int> DeleteObjectInstanceAsync(int id)
           => await _objectRepository.DeleteObjectInstanceAsync(id, _userName);
    
    
    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDate>?> GetObjectDatesAsync(string sdOid) {
        var objDatesInDb = await _objectRepository.GetObjectDatesAsync(sdOid);
        return objDatesInDb?.Select(r => new ObjectDate(r)).ToList();
    }
  
    public async Task<ObjectDate?> GetObjectDateAsync(int id) {
        var objDateInDb = await _objectRepository.GetObjectDateAsync(id);
        return objDateInDb == null ? null : new ObjectDate(objDateInDb);
    }    

    // Update data
    public async Task<ObjectDate?> CreateObjectDateAsync(ObjectDate objDateContent) {
        var objDateContentInDb = new ObjectDateInDb(objDateContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectDateAsync(objDateContentInDb);
        return res == null ? null : new ObjectDate(res);
    } 

    public async Task<ObjectDate?> UpdateObjectDateAsync(int aId, ObjectDate objDateContent) {
        var objDateContentInDb = new ObjectDateInDb(objDateContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectDateAsync(objDateContentInDb);
        return res == null ? null : new ObjectDate(res);
    } 
    
    public async Task<int> DeleteObjectDateAsync(int id)
           => await _objectRepository.DeleteObjectDateAsync(id, _userName);
    
    
    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDescription>?> GetObjectDescriptionsAsync(string sdOid) {
        var objDescsInDb = await _objectRepository.GetObjectDescriptionsAsync(sdOid);
        return objDescsInDb?.Select(r => new ObjectDescription(r)).ToList();
    }
   
    public async Task<ObjectDescription?> GetObjectDescriptionAsync(int id) {
        var objDescInDb = await _objectRepository.GetObjectDescriptionAsync(id);
        return objDescInDb == null ? null : new ObjectDescription(objDescInDb);
    }    

    // Update data
    public async Task<ObjectDescription?> CreateObjectDescriptionAsync(ObjectDescription objDescContent) {
        var objDescContentInDb = new ObjectDescriptionInDb(objDescContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectDescriptionAsync(objDescContentInDb);
        return res == null ? null : new ObjectDescription(res);
    } 

    public async Task<ObjectDescription?> UpdateObjectDescriptionAsync(int aId, ObjectDescription objDescContent) {
        var objDescContentInDb = new ObjectDescriptionInDb(objDescContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectDescriptionAsync(objDescContentInDb);
        return res == null ? null : new ObjectDescription(res);
    } 
    
    public async Task<int> DeleteObjectDescriptionAsync(int id)
           => await _objectRepository.DeleteObjectDescriptionAsync(id, _userName);
    
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectContributor>?> GetObjectContributorsAsync(string sdOid) {
        var objContsInDb = await _objectRepository.GetObjectContributorsAsync(sdOid);
        return objContsInDb?.Select(r => new ObjectContributor(r)).ToList();
    }
  
    public async Task<ObjectContributor?> GetObjectContributorAsync(int id) {
        var objContInDb = await _objectRepository.GetObjectContributorAsync(id);
        return objContInDb == null ? null : new ObjectContributor(objContInDb);
    }    

    // Update data
    public async Task<ObjectContributor?> CreateObjectContributorAsync(ObjectContributor objContContent) {
        var objContContentInDb = new ObjectContributorInDb(objContContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectContributorAsync(objContContentInDb);
        return res == null ? null : new ObjectContributor(res);
    } 

    public async Task<ObjectContributor?> UpdateObjectContributorAsync(int aId, ObjectContributor objContContent) {
        var objRightContentInDb = new ObjectContributorInDb(objContContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectContributorAsync(objRightContentInDb);
        return res == null ? null : new ObjectContributor(res);
    } 
    
    public async Task<int> DeleteObjectContributorAsync(int id)
           => await _objectRepository.DeleteObjectContributorAsync(id, _userName);
    
    
    /****************************************************************
    * Object topics
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectTopic>?> GetObjectTopicsAsync(string sdOid) {
        var objTopicsInDb = await _objectRepository.GetObjectTopicsAsync(sdOid);
        return objTopicsInDb?.Select(r => new ObjectTopic(r)).ToList();
    }
   
    public async Task<ObjectTopic?> GetObjectTopicAsync(int id) {
        var objTopicInDb = await _objectRepository.GetObjectTopicAsync(id);
        return objTopicInDb == null ? null : new ObjectTopic(objTopicInDb);
    }    

    // Update data
    public async Task<ObjectTopic?> CreateObjectTopicAsync(ObjectTopic objTopicContent) {
        var objTopicContentInDb = new ObjectTopicInDb(objTopicContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectTopicAsync(objTopicContentInDb);
        return res == null ? null : new ObjectTopic(res);
    } 

    public async Task<ObjectTopic?> UpdateObjectTopicAsync(int aId, ObjectTopic objTopicContent) {
        var objTopicContentInDb = new ObjectTopicInDb(objTopicContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectTopicAsync(objTopicContentInDb);
        return res == null ? null : new ObjectTopic(res);
    } 
    
    public async Task<int> DeleteObjectTopicAsync(int id)
           => await _objectRepository.DeleteObjectTopicAsync(id, _userName);
    
    
    /****************************************************************
    * Object identifiers
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectIdentifier>?> GetObjectIdentifiersAsync(string sdOid) {
        var aIdentsInDb = await _objectRepository.GetObjectIdentifiersAsync(sdOid);
        return aIdentsInDb?.Select(r => new ObjectIdentifier(r)).ToList();
    }
     
    public async Task<ObjectIdentifier?> GetObjectIdentifierAsync(int id) { 
        var aIdentInDb = await _objectRepository.GetObjectIdentifierAsync(id);
        return aIdentInDb == null ? null : new ObjectIdentifier(aIdentInDb);
    }    

    // Update data
    public async Task<ObjectIdentifier?> CreateObjectIdentifierAsync(ObjectIdentifier aIdentContent) {
        var aIdentContentInDb = new ObjectIdentifierInDb(aIdentContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectIdentifierAsync(aIdentContentInDb);
        return res == null ? null : new ObjectIdentifier(res);
    } 

    public async Task<ObjectIdentifier?> UpdateObjectIdentifierAsync(int aId, ObjectIdentifier aIdentContent) {
        var aIdentContentInDb = new ObjectIdentifierInDb(aIdentContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectIdentifierAsync(aIdentContentInDb);
        return res == null ? null : new ObjectIdentifier(res);
    } 
    
    public async Task<int> DeleteObjectIdentifierAsync(int id)
           => await _objectRepository.DeleteObjectIdentifierAsync(id, _userName);
    
    
    /****************************************************************
    * Object relationships
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectRelationship>?> GetObjectRelationshipsAsync(string sdOid) {
        var objRelsInDb = await _objectRepository.GetObjectRelationshipsAsync(sdOid);
        return objRelsInDb?.Select(r => new ObjectRelationship(r)).ToList();
    }
   
    public async Task<ObjectRelationship?> GetObjectRelationshipAsync(int id) {
        var objRelInDb = await _objectRepository.GetObjectRelationshipAsync(id);
        return objRelInDb == null ? null : new ObjectRelationship(objRelInDb);
    }    

    // Update data
    public async Task<ObjectRelationship?> CreateObjectRelationshipAsync(ObjectRelationship objRelContent) {
        var objRelContentInDb = new ObjectRelationshipInDb(objRelContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectRelationshipAsync(objRelContentInDb);
        return res == null ? null : new ObjectRelationship(res);
    } 
    
    public async Task<ObjectRelationship?> UpdateObjectRelationshipAsync(int aId, ObjectRelationship objRelContent) {
        var objRelContentInDb = new ObjectRelationshipInDb(objRelContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectRelationshipAsync(objRelContentInDb);
        return res == null ? null : new ObjectRelationship(res);
    } 
    
    public async Task<int> DeleteObjectRelationshipAsync(int id)
           => await _objectRepository.DeleteObjectRelationshipAsync(id, _userName);
    
    
    /****************************************************************
    * Object rights
    ****************************************************************/
  
    // Fetch data

    public async Task<List<ObjectRight>?> GetObjectRightsAsync(string sdOid) {
        var objRightsInDb = await _objectRepository.GetObjectRightsAsync(sdOid);
        return objRightsInDb?.Select(r => new ObjectRight(r)).ToList();
    }

    public async Task<ObjectRight?> GetObjectRightAsync(int id) {
        var objRightInDb = await _objectRepository.GetObjectRightAsync(id);
        return objRightInDb == null ? null : new ObjectRight(objRightInDb);
    }    
    
    // Update data
    
    public async Task<ObjectRight?> CreateObjectRightAsync(ObjectRight objRightContent) {
        var objRightContentInDb = new ObjectRightInDb(objRightContent) { last_edited_by = _userName };
        var res = await _objectRepository.CreateObjectRightAsync(objRightContentInDb);
        return res == null ? null : new ObjectRight(res);
    } 
    
    public async Task<ObjectRight?> UpdateObjectRightAsync(int aId, ObjectRight objRightContent) {
        var objRightContentInDb = new ObjectRightInDb(objRightContent) { id = aId, last_edited_by = _userName };
        var res = await _objectRepository.UpdateObjectRightAsync(objRightContentInDb);
        return res == null ? null : new ObjectRight(res);
    } 
    
    public async Task<int> DeleteObjectRightAsync(int id)
           => await _objectRepository.DeleteObjectRightAsync(id, _userName);
    
    
    /****************************************************************
    * Statistics
    ****************************************************************/

    public async Task<Statistic> GetTotalObjects()
    {
        int res = await _objectRepository.GetTotalObjects();
        return new Statistic("Total", res);
    }

    public async Task<List<Statistic>?> GetObjectsByType()
    {
        var res = (await _objectRepository.GetObjectsByType()).ToList();
        if (await ResetLookupsAsync("object-types"))
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
    
    private async Task<bool> ResetLookupsAsync(string typeName)
    {
        _lookups = new List<Lup>();  // reset to empty list
        _lookups = await _lupService.GetLookUpValuesAsync(typeName);
        return _lookups.Count > 0 ;
    }
}