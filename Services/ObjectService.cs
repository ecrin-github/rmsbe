using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Services.Interfaces;

namespace rmsbe.Services;

public class ObjectService : IObjectService
{
    private readonly IObjectRepository _objectRepository;
    private string _user_name;

    public ObjectService(IObjectRepository objectRepository)
    {
        _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        _user_name = "test user"; // for now - need a mechanism to inject this from user object;
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record
    * with the provided id does NOT exists in the database, 
    * i.e. it is true if there is no matching record.
    * Allows controller functions to avoid this error and return a
    * request body with suitable status code
    ****************************************************************/   
    
    // Check if data object exists 
    public async Task<bool> ObjectDoesNotExistAsync(string sd_oid)
        => await _objectRepository.ObjectDoesNotExistAsync(sd_oid);
    
    // Check if attribute exists on specified object
    public async Task<bool> ObjectAttributeDoesNotExistAsync(string sd_oid, string type_name, int id)
        => await _objectRepository.ObjectAttributeDoesNotExistAsync(sd_oid, type_name, id);
    
    /****************************************************************
    * Data object... (data object data only, no attributes)
    ****************************************************************/
  
    // Fetch data 
    public async Task<List<DataObjectData>?> GetAllObjectsDataAsync()
    {
        var objRightsInDb = await _objectRepository.GetDataObjectsDataAsync();
        return objRightsInDb?.Select(r => new DataObjectData(r)).ToList();
    }
     
    public async Task<List<DataObjectData>?> GetRecentObjectsDataAsync(int n)
    {
        var objsInDb = await _objectRepository.GetRecentObjectDataAsync(n);
        return objsInDb?.Select(r => new DataObjectData(r)).ToList();
    }
 
    public async Task<DataObjectData?> GetObjectDataAsync(string sd_oid)
    {
        DataObjectInDb? objInDb = await _objectRepository.GetDataObjectDataAsync(sd_oid);
        return objInDb == null ? null : new DataObjectData(objInDb);
    }    

    // Update data
    public async Task<DataObjectData?> CreateDataObjectDataAsync(DataObjectData dataObjectContent)
    {
        var dataObjectContentInDb = new DataObjectInDb(dataObjectContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateDataObjectDataAsync(dataObjectContentInDb);
        return res == null ? null : new DataObjectData(res);
    } 

    public async Task<DataObjectData?> UpdateDataObjectDataAsync(DataObjectData dataObjectContent)
    {
        var dataObjectContentInDb = new DataObjectInDb(dataObjectContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateDataObjectDataAsync(dataObjectContentInDb);
        return res == null ? null : new DataObjectData(res);
    } 
    
    public async Task<int> DeleteDataObjectAsync(string sd_oid)
        => await _objectRepository.DeleteDataObjectAsync(sd_oid, _user_name);
    
    /****************************************************************
    * Full Data object...(with attribute data)
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<FullDataObject>?> GetAllFullObjectsAsync()
    {
        var fullObjectsInDb = await _objectRepository.GetAllFullDataObjectsAsync();
        return fullObjectsInDb?.Select(r => new FullDataObject(r)).ToList();
    }
    
    public async Task<FullDataObject?> GetFullObjectByIdAsync(string sd_oid)
    {
        FullObjectInDb? fullObjectInDb = await _objectRepository.GetFullObjectByIdAsync(sd_oid);
        return fullObjectInDb == null ? null : new FullDataObject(fullObjectInDb);
    }    
    
    // Update data
    public async Task<int> DeleteFullObjectAsync(string sd_oid)
        => await _objectRepository.DeleteDataObjectAsync( sd_oid, _user_name);
    
    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDataset>?> GetObjectDatasetsAsync(string sd_oid)
    {
        var objDatasetsInDb = await _objectRepository.GetObjectDatasetsAsync(sd_oid);
        return objDatasetsInDb?.Select(r => new ObjectDataset(r)).ToList();
    }
    
    public async Task<ObjectDataset?> GetObjectDatasetAsync(int id)
    {
        ObjectDatasetInDb? objDatasetInDb = await _objectRepository.GetObjectDatasetAsync(id);
        return objDatasetInDb == null ? null : new ObjectDataset(objDatasetInDb);
    }    

    // Update data
    public async Task<ObjectDataset?> CreateObjectDatasetAsync(ObjectDataset objDatasetContent)
    {
        var objDatasetContentInDb = new ObjectDatasetInDb(objDatasetContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectDatasetAsync(objDatasetContentInDb);
        return res == null ? null : new ObjectDataset(res);
    } 

    public async Task<ObjectDataset?> UpdateObjectDatasetAsync(int id, ObjectDataset objDatasetContent)
    {
        var objDatasetContentInDb = new ObjectDatasetInDb(objDatasetContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectDatasetAsync(objDatasetContentInDb);
        return res == null ? null : new ObjectDataset(res);
    } 

    
    public async Task<int> DeleteObjectDatasetAsync(int id)
        => await _objectRepository.DeleteObjectDatasetAsync(id, _user_name);
    
    /****************************************************************
    * Object titles
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectTitle>?> GetObjectTitlesAsync(string sd_oid)
    {
        var objTitlesInDb = await _objectRepository.GetObjectTitlesAsync(sd_oid);
        return objTitlesInDb?.Select(r => new ObjectTitle(r)).ToList();
    }
    
    public async Task<ObjectTitle?> GetObjectTitleAsync(int id)
    {
        ObjectTitleInDb? objTitleInDb = await _objectRepository.GetObjectTitleAsync(id);
        return objTitleInDb == null ? null : new ObjectTitle(objTitleInDb);
    }    

    // Update data
    public async Task<ObjectTitle?> CreateObjectTitleAsync(ObjectTitle objTitleContent)
    {
        var objTitleContentInDb = new ObjectTitleInDb(objTitleContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectTitleAsync(objTitleContentInDb);
        return res == null ? null : new ObjectTitle(res);
    } 

    public async Task<ObjectTitle?> UpdateObjectTitleAsync(int id, ObjectTitle objTitleContent)
    {
        var objTitleContentInDb = new ObjectTitleInDb(objTitleContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectTitleAsync(objTitleContentInDb);
        return res == null ? null : new ObjectTitle(res);
    } 

    
    public async Task<int> DeleteObjectTitleAsync(int id)
        => await _objectRepository.DeleteObjectTitleAsync(id, _user_name);
    
    /****************************************************************
    * Object instances
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectInstance>?> GetObjectInstancesAsync(string sd_oid)
    {
        var objInstancesInDb = await _objectRepository.GetObjectInstancesAsync(sd_oid);
        return objInstancesInDb?.Select(r => new ObjectInstance(r)).ToList();
    }
   
    public async Task<ObjectInstance?> GetObjectInstanceAsync(int id)
    {
        ObjectInstanceInDb? objInstanceInDb = await _objectRepository.GetObjectInstanceAsync(id);
        return objInstanceInDb == null ? null : new ObjectInstance(objInstanceInDb);
    }    

    // Update data
    public async Task<ObjectInstance?> CreateObjectInstanceAsync(ObjectInstance objInstanceContent)
    {
        var objInstanceContentInDb = new ObjectInstanceInDb(objInstanceContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectInstanceAsync(objInstanceContentInDb);
        return res == null ? null : new ObjectInstance(res);
    } 

    public async Task<ObjectInstance?> UpdateObjectInstanceAsync(int id, ObjectInstance objInstanceContent)
    {
        var objInstanceContentInDb = new ObjectInstanceInDb(objInstanceContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectInstanceAsync(objInstanceContentInDb);
        return res == null ? null : new ObjectInstance(res);
    } 

    
    public async Task<int> DeleteObjectInstanceAsync(int id)
        => await _objectRepository.DeleteObjectInstanceAsync(id, _user_name);
    
    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDate>?> GetObjectDatesAsync(string sd_oid)
    {
        var objDatesInDb = await _objectRepository.GetObjectDatesAsync(sd_oid);
        return objDatesInDb?.Select(r => new ObjectDate(r)).ToList();
    }
  
    public async Task<ObjectDate?> GetObjectDateAsync(int id)
    {
        ObjectDateInDb? objDateInDb = await _objectRepository.GetObjectDateAsync(id);
        return objDateInDb == null ? null : new ObjectDate(objDateInDb);
    }    

    // Update data
    public async Task<ObjectDate?> CreateObjectDateAsync(ObjectDate objDateContent)
    {
        var objDateContentInDb = new ObjectDateInDb(objDateContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectDateAsync(objDateContentInDb);
        return res == null ? null : new ObjectDate(res);
    } 

    public async Task<ObjectDate?> UpdateObjectDateAsync(int id, ObjectDate objDateContent)
    {
        var objDateContentInDb = new ObjectDateInDb(objDateContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectDateAsync(objDateContentInDb);
        return res == null ? null : new ObjectDate(res);
    } 
    
    public async Task<int> DeleteObjectDateAsync(int id)
        => await _objectRepository.DeleteObjectDateAsync(id, _user_name);
    
    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    public async Task<List<ObjectDescription>?> GetObjectDescriptionsAsync(string sd_oid)
    {
        var objDescsInDb = await _objectRepository.GetObjectDescriptionsAsync(sd_oid);
        return objDescsInDb?.Select(r => new ObjectDescription(r)).ToList();
    }
   
    public async Task<ObjectDescription?> GetObjectDescriptionAsync(int id)
    {
        ObjectDescriptionInDb? objDescInDb = await _objectRepository.GetObjectDescriptionAsync(id);
        return objDescInDb == null ? null : new ObjectDescription(objDescInDb);
    }    

    // Update data
    public async Task<ObjectDescription?> CreateObjectDescriptionAsync(ObjectDescription objDescContent)
    {
        var objDescContentInDb = new ObjectDescriptionInDb(objDescContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectDescriptionAsync(objDescContentInDb);
        return res == null ? null : new ObjectDescription(res);
    } 

    public async Task<ObjectDescription?> UpdateObjectDescriptionAsync(int id, ObjectDescription objDescContent)
    {
        var objDescContentInDb = new ObjectDescriptionInDb(objDescContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectDescriptionAsync(objDescContentInDb);
        return res == null ? null : new ObjectDescription(res);
    } 
    
    public async Task<int> DeleteObjectDescriptionAsync(int id)
        => await _objectRepository.DeleteObjectDescriptionAsync(id, _user_name);
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectContributor>?> GetObjectContributorsAsync(string sd_oid)
    {
        var objContsInDb = await _objectRepository.GetObjectContributorsAsync(sd_oid);
        return objContsInDb?.Select(r => new ObjectContributor(r)).ToList();
    }
  
    public async Task<ObjectContributor?> GetObjectContributorAsync(int id)
    {
        ObjectContributorInDb? objContInDb = await _objectRepository.GetObjectContributorAsync(id);
        return objContInDb == null ? null : new ObjectContributor(objContInDb);
    }    

    // Update data
    public async Task<ObjectContributor?> CreateObjectContributorAsync(ObjectContributor objContContent)
    {
        var objContContentInDb = new ObjectContributorInDb(objContContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectContributorAsync(objContContentInDb);
        return res == null ? null : new ObjectContributor(res);
    } 

    public async Task<ObjectContributor?> UpdateObjectContributorAsync(int id, ObjectContributor objContContent)
    {
        var objRightContentInDb = new ObjectContributorInDb(objContContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectContributorAsync(objRightContentInDb);
        return res == null ? null : new ObjectContributor(res);
    } 
    
    public async Task<int> DeleteObjectContributorAsync(int id)
        => await _objectRepository.DeleteObjectContributorAsync(id, _user_name);
    
    /****************************************************************
    * Object topics
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectTopic>?> GetObjectTopicsAsync(string sd_oid)
    {
        var objTopicsInDb = await _objectRepository.GetObjectTopicsAsync(sd_oid);
        return objTopicsInDb?.Select(r => new ObjectTopic(r)).ToList();
    }
   
    public async Task<ObjectTopic?> GetObjectTopicAsync(int id)
    {
        ObjectTopicInDb? objTopicInDb = await _objectRepository.GetObjectTopicAsync(id);
        return objTopicInDb == null ? null : new ObjectTopic(objTopicInDb);
    }    

    // Update data
    public async Task<ObjectTopic?> CreateObjectTopicAsync(ObjectTopic objTopicContent)
    {
        var objTopicContentInDb = new ObjectTopicInDb(objTopicContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectTopicAsync(objTopicContentInDb);
        return res == null ? null : new ObjectTopic(res);
    } 

    public async Task<ObjectTopic?> UpdateObjectTopicAsync(int id, ObjectTopic objTopicContent)
    {
        var objTopicContentInDb = new ObjectTopicInDb(objTopicContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectTopicAsync(objTopicContentInDb);
        return res == null ? null : new ObjectTopic(res);
    } 
    
    public async Task<int> DeleteObjectTopicAsync(int id)
        => await _objectRepository.DeleteObjectTopicAsync(id, _user_name);
    
    /****************************************************************
    * Object identifiers
    ****************************************************************/
   
    // Fetch data
    public async Task<List<ObjectIdentifier>?> GetObjectIdentifiersAsync(string sd_oid)
    {
        var objIdentsInDb = await _objectRepository.GetObjectIdentifiersAsync(sd_oid);
        return objIdentsInDb?.Select(r => new ObjectIdentifier(r)).ToList();
    }
     
    public async Task<ObjectIdentifier?> GetObjectIdentifierAsync(int id)
    {
        ObjectIdentifierInDb? objIdentInDb = await _objectRepository.GetObjectIdentifierAsync(id);
        return objIdentInDb == null ? null : new ObjectIdentifier(objIdentInDb);
    }    

    // Update data
    public async Task<ObjectIdentifier?> CreateObjectIdentifierAsync(ObjectIdentifier objIdentContent)
    {
        var objIdentContentInDb = new ObjectIdentifierInDb(objIdentContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectIdentifierAsync(objIdentContentInDb);
        return res == null ? null : new ObjectIdentifier(res);
    } 

    public async Task<ObjectIdentifier?> UpdateObjectIdentifierAsync(int id, ObjectIdentifier objIdentContent)
    {
        var objIdentContentInDb = new ObjectIdentifierInDb(objIdentContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectIdentifierAsync(objIdentContentInDb);
        return res == null ? null : new ObjectIdentifier(res);
    } 
    
    public async Task<int> DeleteObjectIdentifierAsync(int id)
        => await _objectRepository.DeleteObjectIdentifierAsync(id, _user_name);
    
    /****************************************************************
    * Object relationships
    ****************************************************************/
  
    // Fetch data
    public async Task<List<ObjectRelationship>?> GetObjectRelationshipsAsync(string sd_oid)
    {
        var objRelsInDb = await _objectRepository.GetObjectRelationshipsAsync(sd_oid);
        return objRelsInDb?.Select(r => new ObjectRelationship(r)).ToList();
    }
   
    public async Task<ObjectRelationship?> GetObjectRelationshipAsync(int id)
    {
        ObjectRelationshipInDb? objRelInDb = await _objectRepository.GetObjectRelationshipAsync(id);
        return objRelInDb == null ? null : new ObjectRelationship(objRelInDb);
    }    

    // Update data
    public async Task<ObjectRelationship?> CreateObjectRelationshipAsync(ObjectRelationship objRelContent)
    {
        var objRelContentInDb = new ObjectRelationshipInDb(objRelContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectRelationshipAsync(objRelContentInDb);
        return res == null ? null : new ObjectRelationship(res);
    } 
    
    public async Task<ObjectRelationship?> UpdateObjectRelationshipAsync(int id, ObjectRelationship objRelContent)
    {
        var objRelContentInDb = new ObjectRelationshipInDb(objRelContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectRelationshipAsync(objRelContentInDb);
        return res == null ? null : new ObjectRelationship(res);
    } 
    
    public async Task<int> DeleteObjectRelationshipAsync(int id)
                 => await _objectRepository.DeleteObjectRelationshipAsync(id, _user_name);
    
    /****************************************************************
    * Object rights
    ****************************************************************/
  
    // Fetch data

    public async Task<List<ObjectRight>?> GetObjectRightsAsync(string sd_oid)
    {
        var objRightsInDb = await _objectRepository.GetObjectRightsAsync(sd_oid);
        return objRightsInDb?.Select(r => new ObjectRight(r)).ToList();
    }

    public async Task<ObjectRight?> GetObjectRightAsync(int id)
    {
        ObjectRightInDb? objRightInDb = await _objectRepository.GetObjectRightAsync(id);
        return objRightInDb == null ? null : new ObjectRight(objRightInDb);
    }    
    
    // Update data
    
    public async Task<ObjectRight?> CreateObjectRightAsync(ObjectRight objRightContent)
    {
        var objRightContentInDb = new ObjectRightInDb(objRightContent)
        {
            last_edited_by = _user_name
        };
        var res = await _objectRepository.CreateObjectRightAsync(objRightContentInDb);
        return res == null ? null : new ObjectRight(res);
    } 
    
    public async Task<ObjectRight?> UpdateObjectRightAsync(int id, ObjectRight objRightContent)
    {
        var objRightContentInDb = new ObjectRightInDb(objRightContent)
        {
            id = id,
            last_edited_by = _user_name
        };
        var res = await _objectRepository.UpdateObjectRightAsync(objRightContentInDb);
        return res == null ? null : new ObjectRight(res);
    } 
    
    public async Task<int> DeleteObjectRightAsync(int id)
                  => await _objectRepository.DeleteObjectRightAsync(id, _user_name);

}