using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;

namespace rmsbe.DataLayer;

public class ObjectRepository : IObjectRepository
{
    
    private readonly string _dbConnString;
    private readonly Dictionary<string, string> _typeList;
    
    public ObjectRepository(ICredentials creds)
    {
        _dbConnString = creds.GetConnectionString("mdm");
        
        // set up dictionary of table name equivalents for type parameter
        _typeList = new Dictionary<string, string>
        {
            { "ObjectDataset", "mdr.object_datasets" },
            { "ObjectTitle", "mdr.object_titles" },
            { "ObjectInstance", "mdr.object_instances" },
            { "ObjectDate", "mdr.object_dates" },
            { "ObjectDescriptions", "mdr.object_descriptions" },
            { "ObjectTopic", "mdr.object_topics" },           
            { "ObjectContributor", "mdr.object_contributors" },  
            { "ObjectIdentifier", "mdr.object_identifiers" },  
            { "ObjectRight", "mdr.object_rights" },
            { "ObjectRelationship", "mdr.object_relationships" }
        };
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    public async Task<bool> ObjectDoesNotExistAsync(string sd_oid)
    {
        string sqlString = $@"select not exists (select 1 from mdr.data_objects 
                              where sd_id = '{sd_oid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> ObjectAttributeDoesNotExistAsync(string sd_oid, string type_name, int id)
    {
        string sqlString = $@"select not exists (select 1 from {_typeList[type_name]}
                              where id = {id.ToString()} and sd_oid = '{sd_oid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    /****************************************************************
    * Full Data Object data (including attributes in other tables)
    ****************************************************************/
  
    // Fetch data
    public async Task<IEnumerable<FullObjectInDb>?> GetAllFullDataObjectsAsync()
    {
        return null;
    }

    public async Task<FullObjectInDb?> GetFullObjectByIdAsync(string sd_oid)
    {
        return null;
    }

    // Update data
    public async Task<int> DeleteDataObjectAsync(string sd_oid, string user_name)
    {
        return 0;
    }
    
    /****************************************************************
    * Data Object data (without attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DataObjectInDb>?> GetDataObjectsDataAsync()
    {
        return null;
    }

    public async Task<IEnumerable<DataObjectInDb>?> GetRecentObjectDataAsync(int limit)
    {
        return null;
    }

    public async Task<DataObjectInDb?> GetDataObjectDataAsync(string sd_oid)
    {
        return null;
    }

    // Update data
    public async Task<DataObjectInDb?> CreateDataObjectDataAsync(DataObjectInDb dataObjectData)
    {
        return null;
    }

    public async Task<DataObjectInDb?> UpdateDataObjectDataAsync(DataObjectInDb dataObjectData)
    {
        return null;
    }

    
    /****************************************************************
    * Object contributors
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectContributorInDb>?> GetObjectContributorsAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectContributorInDb?> GetObjectContributorAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectContributorInDb?> CreateObjectContributorAsync(ObjectContributorInDb objectContributorInDb)
    {
        return null;
    }

    public async Task<ObjectContributorInDb?> UpdateObjectContributorAsync(ObjectContributorInDb objectContributorInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectContributorAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectDatasetInDb>?> GetObjectDatasetsAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectDatasetInDb?> GetObjectDatasetAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectDatasetInDb?> CreateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb)
    {
        return null;
    }

    public async Task<ObjectDatasetInDb?> UpdateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectDatasetAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectDateInDb>?> GetObjectDatesAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectDateInDb?> GetObjectDateAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectDateInDb?> CreateObjectDateAsync(ObjectDateInDb objectDateInDb)
    {
        return null;
    }

    public async Task<ObjectDateInDb?> UpdateObjectDateAsync(ObjectDateInDb objectDateInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectDateAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectDescriptionInDb>?> GetObjectDescriptionsAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectDescriptionInDb?> GetObjectDescriptionAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectDescriptionInDb?> CreateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb)
    {
        return null;
    }

    public async Task<ObjectDescriptionInDb?> UpdateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectDescriptionAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object identifiers
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectIdentifierInDb>?> GetObjectIdentifiersAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectIdentifierInDb?> GetObjectIdentifierAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectIdentifierInDb?> CreateObjectIdentifierAsync(ObjectIdentifierInDb object_identifierInDb)
    {
        return null;
    }

    public async Task<ObjectIdentifierInDb?> UpdateObjectIdentifierAsync(ObjectIdentifierInDb object_identifierInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectIdentifierAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object instances
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectInstanceInDb>?> GetObjectInstancesAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectInstanceInDb?> GetObjectInstanceAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectInstanceInDb?> CreateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb)
    {
        return null;
    }

    public async Task<ObjectInstanceInDb?> UpdateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectInstanceAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object relationships
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectRelationshipInDb>?> GetObjectRelationshipsAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectRelationshipInDb?> GetObjectRelationshipAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectRelationshipInDb?> CreateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb)
    {
        return null;
    }

    public async Task<ObjectRelationshipInDb?> UpdateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectRelationshipAsync(int id, string user_name)
    {
        return 0;
    }

    /****************************************************************
    * Object rights
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectRightInDb>?> GetObjectRightsAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectRightInDb?> GetObjectRightAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectRightInDb?> CreateObjectRightAsync(ObjectRightInDb objectRightInDb)
    {
        return null;
    }

    public async Task<ObjectRightInDb?> UpdateObjectRightAsync(ObjectRightInDb objectRightInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectRightAsync(int id, string user_name)
    {
        return 0;
    }

   
    /****************************************************************
    * Object titles
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<ObjectTitleInDb>?> GetObjectTitlesAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectTitleInDb?> GetObjectTitleAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectTitleInDb?> CreateObjectTitleAsync(ObjectTitleInDb objectTitleInDb)
    {
        return null;
    }

    public async Task<ObjectTitleInDb?> UpdateObjectTitleAsync(ObjectTitleInDb objectTitleInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectTitleAsync(int id, string user_name)
    {
        return 0;
    }
  
    /****************************************************************
    * Object topics
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectTopicInDb>?> GetObjectTopicsAsync(string sd_oid)
    {
        return null;
    }

    public async Task<ObjectTopicInDb?> GetObjectTopicAsync(int? id)
    {
        return null;
    }

    // Update data
    public async Task<ObjectTopicInDb?> CreateObjectTopicAsync(ObjectTopicInDb objectTopicInDb)
    {
        return null;
    }

    public async Task<ObjectTopicInDb?> UpdateObjectTopicAsync(ObjectTopicInDb objectTopicInDb)
    {
        return null;
    }

    public async Task<int> DeleteObjectTopicAsync(int id, string user_name)
    {
        return 0;
    }

    // Extensions
    /*
    public async Task<PaginationResponse<DataObjectInDb>?> PaginateDataObjects(PaginationRequest paginationRequest);
    public async Task<PaginationResponse<DataObjectInDb>?> FilterDataObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest);
    public async Task<int> GetTotalDataObjects();
    */

        
        
        /*
        private readonly MdmDbConnection _dbConnection;
        private readonly IDataMapper _dataMapper;
        private readonly IAuditService _auditService;
        private readonly IUserIdentityService _userIdentityService;

        public ObjectRepository(
            MdmDbConnection dbConnection, 
            IDataMapper dataMapper,
            IAuditService auditService,
            IUserIdentityService userIdentityService)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _dataMapper = dataMapper ?? throw new ArgumentNullException(nameof(dataMapper));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            _userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        }
        

        public async public async Task<ICollection<ObjectContributorDto>> GetObjectContributors(string sd_oid)
        {
            var data = _dbConnection.ObjectContributors.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectContributorDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectContributorDto> GetObjectContributor(int? id)
        {
            var objectContributor = await _dbConnection.ObjectContributors.FirstOrDefaultAsync(p => p.Id == id);
            return objectContributor != null ? _dataMapper.ObjectContributorDtoMapper(objectContributor) : null;
        }

        public async public async Task<ObjectContributorDto> CreateObjectContributor(ObjectContributorDto objectContributorDto, string accessToken)
        {
            
            await _dbConnection.ObjectContributors.AddAsync(objectContributor);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectContributorDtoMapper(objectContributor);
        }

        public async public async Task<ObjectContributorDto> UpdateObjectContributor(ObjectContributorDto objectContributorDto, string accessToken)
        {
            var dbObjectContributor =
                await _dbConnection.ObjectContributors.FirstOrDefaultAsync(p => p.sd_oid == objectContributorDto.sd_oid);
            if (dbObjectContributor == null) return null;

           
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectContributorDtoMapper(dbObjectContributor);
        }

        public async public async Task<int> DeleteObjectContributor(int id)
        {
            var data = await _dbConnection.ObjectContributors.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectContributors.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectContributors(string sd_oid)
        {
            var data = _dbConnection.ObjectContributors.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectContributors.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ObjectDatasetDto> GetObjectDatasets(string sd_oid)
        {
            var data = _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.sd_oid == sd_oid);
            return data != null ? _dataMapper.ObjectDatasetDtoMapper(await data) : null;
        }

        public async public async Task<ObjectDatasetDto> GetObjectDataset(int? id)
        {
            var objectDataset = await _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.Id == id);
            return objectDataset != null ? _dataMapper.ObjectDatasetDtoMapper(objectDataset) : null;
        }

        public async public async Task<ObjectDatasetDto> CreateObjectDataset(ObjectDatasetDto objectDatasetDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            
            await _dbConnection.ObjectDatasets.AddAsync(objectDataset);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDatasetDtoMapper(objectDataset);
        }

        public async public async Task<ObjectDatasetDto> UpdateObjectDataset(ObjectDatasetDto objectDatasetDto, string accessToken)
        {
            var dbObjectDataset =
                await _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.Id == objectDatasetDto.Id);
            if (dbObjectDataset == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

           
            dbObjectDataset.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDatasetDtoMapper(dbObjectDataset);
        }

        public async public async Task<int> DeleteObjectDataset(int id)
        {
            var data = await _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectDatasets.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectDatasets(string sd_oid)
        {
            var data = _dbConnection.ObjectDatasets.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectDatasets.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<ObjectDateDto>> GetObjectDates(string sd_oid)
        {
            var data = _dbConnection.ObjectDates.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectDateDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectDateDto> GetObjectDate(int? id)
        {
            var objectDate = await _dbConnection.ObjectDates.FirstOrDefaultAsync(p => p.Id == id);
            return objectDate != null ? _dataMapper.ObjectDateDtoMapper(objectDate) : null;
        }

        public async public async Task<ObjectDateDto> CreateObjectDate(ObjectDateDto objectDateDto, string accessToken)
        {
            
            await _dbConnection.ObjectDates.AddAsync(objectDate);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDateDtoMapper(objectDate);
        }

        public async public async Task<ObjectDateDto> UpdateObjectDate(ObjectDateDto objectDateDto, string accessToken)
        {
            var dbObjectDate = await _dbConnection.ObjectDates.FirstOrDefaultAsync(p => p.Id == objectDateDto.Id);
            if (dbObjectDate == null) return null;

                        
            return _dataMapper.ObjectDateDtoMapper(dbObjectDate);
        }

        public async public async Task<int> DeleteObjectDate(int id)
        {
            var data = await _dbConnection.ObjectDates.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectDates.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectDates(string sd_oid)
        {
            var data = _dbConnection.ObjectDates.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectDates.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<ObjectDescriptionDto>> GetObjectDescriptions(string sd_oid)
        {
            var data = _dbConnection.ObjectDescriptions.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectDescriptionDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectDescriptionDto> GetObjectDescription(int? id)
        {
            var objectDescription = await _dbConnection.ObjectDescriptions.FirstOrDefaultAsync(p => p.Id == id);
            return objectDescription != null ? _dataMapper.ObjectDescriptionDtoMapper(objectDescription) : null;
        }

        public async public async Task<ObjectDescriptionDto> CreateObjectDescription(ObjectDescriptionDto objectDescriptionDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

                        await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDescriptionDtoMapper(objectDescription);
        }

        public async public async Task<ObjectDescriptionDto> UpdateObjectDescription(ObjectDescriptionDto objectDescriptionDto, string accessToken)
        {
            var dbObjectDescription =
                await _dbConnection.ObjectDescriptions.FirstOrDefaultAsync(p => p.Id == objectDescriptionDto.Id);
            if (dbObjectDescription == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            dbObjectDescription.last_edited_by = "userData";
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDescriptionDtoMapper(dbObjectDescription);
        }

        public async public async Task<int> DeleteObjectDescription(int id)
        {
            var data = await _dbConnection.ObjectDescriptions.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectDescriptions.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectDescriptions(string sd_oid)
        {
            var data = _dbConnection.ObjectDescriptions.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectDescriptions.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<object_identifierDto>> Getobject_identifiers(string sd_oid)
        {
            var data = _dbConnection.object_identifiers.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.object_identifierDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<object_identifierDto> Getobject_identifier(int? id)
        {
            var object_identifier = await _dbConnection.object_identifiers.FirstOrDefaultAsync(p => p.Id == id);
            return object_identifier != null ? _dataMapper.object_identifierDtoMapper(object_identifier) : null;
        }

        public async public async Task<object_identifierDto> Createobject_identifier(object_identifierDto object_identifierDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            s.AddAsync(object_identifier);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.object_identifierDtoMapper(object_identifier);
        }

        public async public async Task<object_identifierDto> Updateobject_identifier(object_identifierDto object_identifierDto, string accessToken)
        {
            var dbobject_identifier =
                await _dbConnection.object_identifiers.FirstOrDefaultAsync(p => p.Id == object_identifierDto.Id);
            if (dbobject_identifier == null) return null;


            dbobject_identifier.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.object_identifierDtoMapper(dbobject_identifier);
        }

        public async public async Task<int> Deleteobject_identifier(int id)
        {
            var data = await _dbConnection.object_identifiers.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.object_identifiers.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllobject_identifiers(string sd_oid)
        {
            var data = _dbConnection.object_identifiers.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.object_identifiers.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<ObjectInstanceDto>> GetObjectInstances(string sd_oid)
        {
            var data = _dbConnection.ObjectInstances.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectInstanceDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectInstanceDto> GetObjectInstance(int? id)
        {
            var objectInstance = await _dbConnection.ObjectInstances.FirstOrDefaultAsync(p => p.Id == id);
            return objectInstance != null ? _dataMapper.ObjectInstanceDtoMapper(objectInstance) : null;
        }

        public async public async Task<ObjectInstanceDto> CreateObjectInstance(ObjectInstanceDto objectInstanceDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            

            await _dbConnection.ObjectInstances.AddAsync(objectInstance);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectInstanceDtoMapper(objectInstance);
        }

        public async public async Task<ObjectInstanceDto> UpdateObjectInstance(ObjectInstanceDto objectInstanceDto, string accessToken)
        {
            var dbObjectInstance =
                await _dbConnection.ObjectInstances.FirstOrDefaultAsync(p => p.Id == objectInstanceDto.Id);
            if (dbObjectInstance == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectInstanceDtoMapper(dbObjectInstance);
        }

        public async public async Task<int> DeleteObjectInstance(int id)
        {
            var data = await _dbConnection.ObjectInstances.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectInstances.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectInstances(string sd_oid)
        {
            var data = _dbConnection.ObjectInstances.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectInstances.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public async public async Task<ICollection<ObjectRelationshipDto>> GetObjectRelationships(string sd_oid)
        {
            var data = _dbConnection.ObjectRelationships.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectRelationshipDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectRelationshipDto> GetObjectRelationship(int? id)
        {
            var objectRelation = await _dbConnection.ObjectRelationships.FirstOrDefaultAsync(p => p.Id == id);
            return objectRelation != null ? _dataMapper.ObjectRelationshipDtoMapper(objectRelation) : null;
        }

        public async public async Task<ObjectRelationshipDto> CreateObjectRelationship(ObjectRelationshipDto objectRelationshipDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            

            await _dbConnection.ObjectRelationships.AddAsync(objectRelationship);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRelationshipDtoMapper(objectRelationship);
        }

        public async public async Task<ObjectRelationshipDto> UpdateObjectRelationship(ObjectRelationshipDto objectRelationshipDto, string accessToken)
        {
            var dbObjectRelation =
                await _dbConnection.ObjectRelationships.FirstOrDefaultAsync(p => p.Id == objectRelationshipDto.Id);
            if (dbObjectRelation == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

                       
            dbObjectRelation.relationship_type_id = objectRelationshipDto.relationship_type_id;
            dbObjectRelation.target_sd_oid = objectRelationshipDto.target_sd_oid;

            dbObjectRelation.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRelationshipDtoMapper(dbObjectRelation);
        }

        public async public async Task<int> DeleteObjectRelationship(int id)
        {
            var data = await _dbConnection.ObjectRelationships.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectRelationships.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectRelationships(string sd_oid)
        {
            var data = _dbConnection.ObjectRelationships.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectRelationships.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<ObjectRightDto>> GetObjectRights(string sd_oid)
        {
            var data = _dbConnection.ObjectRights.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectRightDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectRightDto> GetObjectRight(int? id)
        {
            var objectRight = await _dbConnection.ObjectRights.FirstOrDefaultAsync(p => p.Id == id);
            return objectRight != null ? _dataMapper.ObjectRightDtoMapper(objectRight) : null;
        }

        public async public async Task<ObjectRightDto> CreateObjectRight(ObjectRightDto objectRightDto, string accessToken)
        {
            
            await _dbConnection.ObjectRights.AddAsync(objectRight);

            
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRightDtoMapper(objectRight);
        }

        public async public async Task<ObjectRightDto> UpdateObjectRight(ObjectRightDto objectRightDto, string accessToken)
        {
            var dbObjectRight = await _dbConnection.ObjectRights.FirstOrDefaultAsync(p => p.Id == objectRightDto.Id);
            if (dbObjectRight == null) return null;
            
            
            dbObjectRight.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRightDtoMapper(dbObjectRight);
        }

        public async public async Task<int> DeleteObjectRight(int id)
        {
            var data = await _dbConnection.ObjectRights.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectRights.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectRights(string sd_oid)
        {
            var data = _dbConnection.ObjectRights.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectRights.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<ObjectTitleDto>> GetObjectTitles(string sd_oid)
        {
            var data = _dbConnection.ObjectTitles.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectTitleDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectTitleDto> GetObjectTitle(int? id)
        {
            var objectTitle = await _dbConnection.ObjectTitles.FirstOrDefaultAsync(p => p.Id == id);
            return objectTitle != null ? _dataMapper.ObjectTitleDtoMapper(objectTitle) : null;
        }

        public async public async Task<ObjectTitleDto> CreateObjectTitle(ObjectTitleDto objectTitleDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

           

            await _dbConnection.ObjectTitles.AddAsync(objectTitle);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectTitleDtoMapper(objectTitle);
        }

        public async public async Task<ObjectTitleDto> UpdateObjectTitle(ObjectTitleDto objectTitleDto, string accessToken)
        {
            // dbObjectTitle.last_edited_by = userData;
                
            await _dbConnection.SaveChangesAsync();
            return _dataMapper.ObjectTitleDtoMapper(dbObjectTitle);
        }

        public async public async Task<int> DeleteObjectTitle(int id)
        {
            var data = await _dbConnection.ObjectTitles.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectTitles.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectTitles(string sd_oid)
        {
            var data = _dbConnection.ObjectTitles.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectTitles.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async public async Task<ICollection<ObjectTopicDto>> GetObjectTopics(string sd_oid)
        {
            var data = _dbConnection.ObjectTopics.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectTopicDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async public async Task<ObjectTopicDto> GetObjectTopic(int? id)
        {
            var objectTopic = await _dbConnection.ObjectTopics.FirstOrDefaultAsync(p => p.Id == id);
            return objectTopic != null ? _dataMapper.ObjectTopicDtoMapper(objectTopic) : null;
        }

        public async public async Task<ObjectTopicDto> CreateObjectTopic(ObjectTopicDto objectTopicDto, string accessToken)
        {
            
            await _dbConnection.ObjectTopics.AddAsync(objectTopic);

             await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectTopicDtoMapper(objectTopic);
        }

        public async public async Task<ObjectTopicDto> UpdateObjectTopic(ObjectTopicDto objectTopicDto, string accessToken)
        {
            var dbObjectTopic = await _dbConnection.ObjectTopics.FirstOrDefaultAsync(p => p.Id == objectTopicDto.Id);
            if (dbObjectTopic == null) return null;

            
            dbObjectTopic.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            return _dataMapper.ObjectTopicDtoMapper(dbObjectTopic);
        }

        public async public async Task<int> DeleteObjectTopic(int id)
        {
            var data = await _dbConnection.ObjectTopics.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectTopics.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<int> DeleteAllObjectTopics(string sd_oid)
        {
            var data = _dbConnection.ObjectTopics.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectTopics.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }


        // DATA OBJECT
        public async public async Task<ICollection<DataObjectDto>> GetAllDataObjects()
        {
            var objectResponses = new List<DataObjectDto>();
            if (!_dbConnection.DataObjects.Any()) return null;
            var dataObjects = await _dbConnection.DataObjects.ToArrayAsync();
            foreach (var dataObject in dataObjects)
            {
                objectResponses.Add(await DataObjectBuilder(dataObject));
            }

            return objectResponses;
        }

        public async public async Task<DataObjectDto> GetObjectById(string sd_oid)
        {
            var dataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == sd_oid);
            if (dataObject == null) return null;
            return await DataObjectBuilder(dataObject);
        }

        public async public async Task<DataObjectDto> CreateDataObject(DataObjectDto dataObjectDto, string accessToken)
        {
            var objId = 300001;
            var lastRecord = await _dbConnection.DataObjects.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (lastRecord != null)
            {
                objId = lastRecord.Id + 1;
            }

            // var userData = await _userIdentityService.GetUserData(accessToken);

            var dataObject = new DataObject
            {
                sd_oid = "RMS-" + objId,
                created_on = DateTime.Now,
                sd_sid = dataObjectDto.sd_sid,
                display_title = dataObjectDto.display_title,
                doi = dataObjectDto.doi,
                Version = dataObjectDto.Version,
                doistatus_id = dataObjectDto.doistatus_id,
                PublicationYear = dataObjectDto.PublicationYear,
                ObjectClassId = dataObjectDto.ObjectClassId,
                ObjectTypeId = dataObjectDto.ObjectTypeId,
                Managingorg_id = dataObjectDto.Managingorg_id,
                ManagingOrg = dataObjectDto.ManagingOrg,
                ManagingOrgRorId = dataObjectDto.ManagingOrgRorId,
                lang_code = dataObjectDto.lang_code,
                access_type_id = dataObjectDto.access_type_id,
                access_details = dataObjectDto.access_details,
                access_detailsurl = dataObjectDto.access_detailsurl,
                url_last_checked = dataObjectDto.url_last_checked,
                EoscCategory = dataObjectDto.EoscCategory,
                AddStudyContribs = dataObjectDto.AddStudyContribs,
                last_edited_by = "userData"
            };

            await _dbConnection.DataObjects.AddAsync(dataObject);
            await _dbConnection.SaveChangesAsync();

            if (dataObjectDto.ObjectContributors is { Count: > 0 })
            {
                foreach (var oc in dataObjectDto.ObjectContributors)
                {
                    if (string.IsNullOrEmpty(oc.sd_oid))
                    {
                        oc.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectContributor(oc, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectDatasets != null)
            {
                if (string.IsNullOrEmpty(dataObjectDto.ObjectDatasets.sd_oid))
                {
                    dataObjectDto.ObjectDatasets.sd_oid = dataObject.sd_oid;
                }
                await CreateObjectDataset(dataObjectDto.ObjectDatasets, accessToken);
            }
            
            if (dataObjectDto.ObjectDates is { Count: > 0 })
            {
                foreach (var od in dataObjectDto.ObjectDates)
                {
                    if (string.IsNullOrEmpty(od.sd_oid))
                    {
                        od.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectDate(od, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectDescriptions is { Count: > 0 })
            {
                foreach (var od in dataObjectDto.ObjectDescriptions)
                {
                    if (string.IsNullOrEmpty(od.sd_oid))
                    {
                        od.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectDescription(od, accessToken);
                }
            }
            
            if (dataObjectDto.object_identifiers is { Count: > 0 })
            {
                foreach (var oi in dataObjectDto.object_identifiers)
                {
                    if (string.IsNullOrEmpty(oi.sd_oid))
                    {
                        oi.sd_oid = dataObject.sd_oid;
                    }
                    await Createobject_identifier(oi, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectInstances is { Count: > 0 })
            {
                foreach (var oi in dataObjectDto.ObjectInstances)
                {
                    if (string.IsNullOrEmpty(oi.sd_oid))
                    {
                        oi.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectInstance(oi, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectRelationships is { Count: > 0 })
            {
                foreach (var or in dataObjectDto.ObjectRelationships)
                {
                    if (string.IsNullOrEmpty(or.sd_oid))
                    {
                        or.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectRelationship(or, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectRights is { Count: > 0 })
            {
                foreach (var or in dataObjectDto.ObjectRights)
                {
                    if (string.IsNullOrEmpty(or.sd_oid))
                    {
                        or.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectRight(or, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectTitles is { Count: > 0 })
            {
                foreach (var ot in dataObjectDto.ObjectTitles)
                {
                    if (string.IsNullOrEmpty(ot.sd_oid))
                    {
                        ot.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectTitle(ot, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectTopics is { Count: > 0 })
            {
                foreach (var ot in dataObjectDto.ObjectTopics)
                {
                    if (string.IsNullOrEmpty(ot.sd_oid))
                    {
                        ot.sd_oid = dataObject.sd_oid;
                    }
                    await CreateObjectTopic(ot, accessToken);
                }
            }
            
            return await DataObjectBuilder(dataObject);
        }

        public async public async Task<DataObjectDto> UpdateDataObject(DataObjectDto dataObjectDto, string accessToken)
        {
            var dbDataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == dataObjectDto.sd_oid);
            if (dbDataObject == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);
            
            dbDataObject.display_title = dataObjectDto.display_title;
            dbDataObject.doi = dataObjectDto.doi;
            dbDataObject.Version = dataObjectDto.Version;
            dbDataObject.doistatus_id = dataObjectDto.doistatus_id;
            dbDataObject.PublicationYear = dataObjectDto.PublicationYear;
            dbDataObject.ObjectClassId = dataObjectDto.ObjectClassId;
            dbDataObject.ObjectTypeId = dataObjectDto.ObjectTypeId;
            dbDataObject.Managingorg_id = dataObjectDto.Managingorg_id;
            dbDataObject.ManagingOrg = dataObjectDto.ManagingOrg;
            dbDataObject.ManagingOrgRorId = dataObjectDto.ManagingOrgRorId;
            dbDataObject.lang_code = dataObjectDto.lang_code;
            dbDataObject.access_type_id = dataObjectDto.access_type_id;
            dbDataObject.access_details = dataObjectDto.access_details;
            dbDataObject.access_detailsurl = dataObjectDto.access_detailsurl;
            dbDataObject.url_last_checked = dataObjectDto.url_last_checked;
            dbDataObject.EoscCategory = dataObjectDto.EoscCategory;
            dbDataObject.AddStudyContribs = dataObjectDto.AddStudyContribs;

            dbDataObject.last_edited_by = "userData";
            
            if (dataObjectDto.ObjectContributors is { Count: > 0 })
            {
                foreach (var oc in dataObjectDto.ObjectContributors)
                {
                    if (oc.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(oc.sd_oid))
                        {
                            oc.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectContributor(oc, accessToken);
                    }
                    else
                    {
                        await UpdateObjectContributor(oc, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectDatasets != null)
            {
                if (dataObjectDto.ObjectDatasets.Id is null or 0)
                {
                    if (string.IsNullOrEmpty(dataObjectDto.ObjectDatasets.sd_oid))
                    {
                        dataObjectDto.ObjectDatasets.sd_oid = dataObjectDto.sd_oid;
                    }
                    await CreateObjectDataset(dataObjectDto.ObjectDatasets, accessToken);
                }
                else
                {
                    await UpdateObjectDataset(dataObjectDto.ObjectDatasets, accessToken);
                }
            }
            
            if (dataObjectDto.ObjectDates is { Count: > 0 })
            {
                foreach (var od in dataObjectDto.ObjectDates)
                {
                    if (od.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(od.sd_oid))
                        {
                            od.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectDate(od, accessToken);
                    }
                    else
                    {
                        await UpdateObjectDate(od, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectDescriptions is { Count: > 0 })
            {
                foreach (var od in dataObjectDto.ObjectDescriptions)
                {
                    if (od.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(od.sd_oid))
                        {
                            od.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectDescription(od, accessToken);
                    }
                    else
                    {
                        await UpdateObjectDescription(od, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.object_identifiers is { Count: > 0 })
            {
                foreach (var oi in dataObjectDto.object_identifiers)
                {
                    if (oi.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(oi.sd_oid))
                        {
                            oi.sd_oid = dataObjectDto.sd_oid;
                        }
                        await Createobject_identifier(oi, accessToken);
                    }
                    else
                    {
                        await Updateobject_identifier(oi, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectInstances is { Count: > 0 })
            {
                foreach (var oi in dataObjectDto.ObjectInstances)
                {
                    if (oi.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(oi.sd_oid))
                        {
                            oi.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectInstance(oi, accessToken);
                    }
                    else
                    {
                        await UpdateObjectInstance(oi, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectRelationships is { Count: > 0 })
            {
                foreach (var or in dataObjectDto.ObjectRelationships)
                {
                    if (or.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(or.sd_oid))
                        {
                            or.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectRelationship(or, accessToken);
                    }
                    else
                    {
                        await UpdateObjectRelationship(or, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectRights is { Count: > 0 })
            {
                foreach (var or in dataObjectDto.ObjectRights)
                {
                    if (or.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(or.sd_oid))
                        {
                            or.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectRight(or, accessToken);
                    }
                    else
                    {
                        await UpdateObjectRight(or, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectTitles is { Count: > 0 })
            {
                foreach (var ot in dataObjectDto.ObjectTitles)
                {
                    if (ot.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(ot.sd_oid))
                        {
                            ot.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectTitle(ot, accessToken);
                    }
                    else
                    {
                        await UpdateObjectTitle(ot, accessToken);
                    }
                }
            }
            
            if (dataObjectDto.ObjectTopics is { Count: > 0 })
            {
                foreach (var ot in dataObjectDto.ObjectTopics)
                {
                    if (ot.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(ot.sd_oid))
                        {
                            ot.sd_oid = dataObjectDto.sd_oid;
                        }
                        await CreateObjectTopic(ot, accessToken);
                    }
                    else
                    {
                        await UpdateObjectTopic(ot, accessToken);
                    }
                }
            }
                
            await _dbConnection.SaveChangesAsync();
            return await DataObjectBuilder(dbDataObject);
        }

        public async public async Task<int> DeleteDataObject(string sd_oid)
        {
            var dataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == sd_oid);
            if (dataObject == null) return 0;
            _dbConnection.DataObjects.Remove(dataObject);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async public async Task<ICollection<DataObjectDataDto>> GetDataObjectsData()
        {
            if (!_dbConnection.DataObjects.Any()) return null;
            var dataObjects = await _dbConnection.DataObjects.ToArrayAsync();

            return dataObjects.Select(dataObject => _dataMapper.DataObjectDataDtoMapper(dataObject)).ToList();
        }

        public async public async Task<DataObjectDataDto> GetDataObjectData(string sd_oid)
        {
            var data = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_sid == sd_oid);
            return data == null ? null : _dataMapper.DataObjectDataDtoMapper(data);
        }

        public async public async Task<ICollection<DataObjectDataDto>> GetRecentObjectData(int limit)
        {
            if (!_dbConnection.DataObjects.Any()) return null;

            var recentObjects = await _dbConnection.DataObjects.OrderByDescending(p => p.Id).Take(limit).ToArrayAsync();
            return _dataMapper.DataObjectDataDtoBuilder(recentObjects);
        }

        public async public async Task<DataObjectDataDto> CreateDataObjectData(DataObjectDataDto dataObjectData, string accessToken)
        {
            var objId = 300001;
            var lastRecord = await _dbConnection.DataObjects.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (lastRecord != null)
            {
                objId = lastRecord.Id + 1;
            }

            // var userData = await _userIdentityService.GetUserData(accessToken);

            var dataObject = new DataObject
            {
                sd_oid = objId.ToString(),
                created_on = DateTime.Now,
                sd_sid = dataObjectData.sd_sid,
                display_title = dataObjectData.display_title,
                doi = dataObjectData.doi,
                Version = dataObjectData.Version,
                doistatus_id = dataObjectData.doistatus_id,
                PublicationYear = dataObjectData.PublicationYear,
                ObjectClassId = dataObjectData.ObjectClassId,
                ObjectTypeId = dataObjectData.ObjectTypeId,
                Managingorg_id = dataObjectData.Managingorg_id,
                ManagingOrg = dataObjectData.ManagingOrg,
                ManagingOrgRorId = dataObjectData.ManagingOrgRorId,
                lang_code = dataObjectData.lang_code,
                access_type_id = dataObjectData.access_type_id,
                access_details = dataObjectData.access_details,
                access_detailsurl = dataObjectData.access_detailsurl,
                url_last_checked = dataObjectData.url_last_checked,
                EoscCategory = dataObjectData.EoscCategory,
                AddStudyContribs = dataObjectData.AddStudyContribs,
                last_edited_by = "userData"
            };

            await _dbConnection.DataObjects.AddAsync(dataObject);

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DataObjectDataDtoMapper(dataObject);
        }
        
        public async public async Task<DataObjectDataDto> UpdateDataObjectData(DataObjectDataDto dataObjectData, string accessToken)
        {
            var dbDataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == dataObjectData.sd_oid);
            if (dbDataObject == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

                      
            dbDataObject.display_title = dataObjectData.display_title;
            dbDataObject.doi = dataObjectData.doi;
            dbDataObject.Version = dataObjectData.Version;
            dbDataObject.doistatus_id = dataObjectData.doistatus_id;
            dbDataObject.PublicationYear = dataObjectData.PublicationYear;
            dbDataObject.ObjectClassId = dataObjectData.ObjectClassId;
            dbDataObject.ObjectTypeId = dataObjectData.ObjectTypeId;
            dbDataObject.Managingorg_id = dataObjectData.Managingorg_id;
            dbDataObject.ManagingOrg = dataObjectData.ManagingOrg;
            dbDataObject.ManagingOrgRorId = dataObjectData.ManagingOrgRorId;
            dbDataObject.lang_code = dataObjectData.lang_code;
            dbDataObject.access_type_id = dataObjectData.access_type_id;
            dbDataObject.access_details = dataObjectData.access_details;
            dbDataObject.access_detailsurl = dataObjectData.access_detailsurl;
            dbDataObject.url_last_checked = dataObjectData.url_last_checked;
            dbDataObject.EoscCategory = dataObjectData.EoscCategory;
            dbDataObject.AddStudyContribs = dataObjectData.AddStudyContribs;

            dbDataObject.last_edited_by = "userData";
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DataObjectDataDtoMapper(dbDataObject);
        }


        private async public async Task<DataObjectDto> DataObjectBuilder(DataObject dataObject)
        {
            return new DataObjectDto()
            {
                Id = dataObject.Id,
                sd_oid = dataObject.sd_oid,
                sd_sid = dataObject.sd_sid,
                display_title = dataObject.display_title,
                Version = dataObject.Version,
                doi = dataObject.doi,
                doistatus_id = dataObject.doistatus_id,
                PublicationYear = dataObject.PublicationYear,
                ObjectClassId = dataObject.ObjectClassId,
                ObjectTypeId = dataObject.ObjectTypeId,
                Managingorg_id = dataObject.Managingorg_id,
                ManagingOrg = dataObject.ManagingOrg,
                ManagingOrgRorId = dataObject.ManagingOrgRorId,
                lang_code = dataObject.lang_code,
                access_type_id = dataObject.access_type_id,
                access_details = dataObject.access_details,
                access_detailsurl = dataObject.access_detailsurl,
                url_last_checked = dataObject.url_last_checked,
                EoscCategory = dataObject.EoscCategory,
                AddStudyContribs = dataObject.AddStudyContribs,
                AddStudyTopics = dataObject.AddStudyTopics,
                created_on = dataObject.created_on,
                ObjectContributors = await GetObjectContributors(dataObject.sd_oid),
                ObjectDatasets = await GetObjectDatasets(dataObject.sd_oid),
                ObjectDates = await GetObjectDates(dataObject.sd_oid),
                ObjectDescriptions = await GetObjectDescriptions(dataObject.sd_oid),
                object_identifiers = await Getobject_identifiers(dataObject.sd_oid),
                ObjectInstances = await GetObjectInstances(dataObject.sd_oid),
                ObjectRelationships = await GetObjectRelationships(dataObject.sd_oid),
                ObjectRights = await GetObjectRights(dataObject.sd_oid),
                ObjectTitles = await GetObjectTitles(dataObject.sd_oid),
                ObjectTopics = await GetObjectTopics(dataObject.sd_oid)
            };
        }

        private static int CalculateSkip(int page, int size)
        {
            var skip = 0;
            if (page > 1)
            {
                skip = (page - 1) * size;
            }

            return skip;
        }

        public async public async Task<PaginationResponse<DataObjectDto>> PaginateDataObjects(PaginationRequest paginationRequest)
        {
            var dataObjects = new List<DataObjectDto>();

            var skip = CalculateSkip(paginationRequest.Page, paginationRequest.Size);
            
            var query = _dbConnection.DataObjects
                .AsNoTracking()
                .OrderBy(arg => arg.Id);
            
            var data = await query.Skip(skip).Take(paginationRequest.Size).ToListAsync();
                        
            if (data is { Count: > 0 })
            {
                foreach (var dataObject in data)
                {
                    dataObjects.Add(await DataObjectBuilder(dataObject));
                }
            }

            var total = await query.CountAsync();

            return new PaginationResponse<DataObjectDto>
            {
                Total = total,
                Data = dataObjects
            };
        }

        public async public async Task<PaginationResponse<DataObjectDto>> FilterDataObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest)
        {
            var dataObjects = new List<DataObjectDto>();

            var skip = CalculateSkip(filteringByTitleRequest.Page, filteringByTitleRequest.Size);
            
            var query = _dbConnection.DataObjects
                .AsNoTracking()
                .Where(p => p.display_title.ToLower().Contains(filteringByTitleRequest.Title.ToLower()))
                .OrderBy(arg => arg.Id);

            var data = await query
                .Skip(skip)
                .Take(filteringByTitleRequest.Size)
                .ToListAsync();

            var total = await query.CountAsync();
                        
            if (data is { Count: > 0 })
            {
                foreach (var dataObject in data)
                {
                    dataObjects.Add(await DataObjectBuilder(dataObject));
                }
            }

            return new PaginationResponse<DataObjectDto>
            {
                Total = total,
                Data = dataObjects
            };
        }

        public async public async Task<int> GetTotalDataObjects()
        {
            return await _dbConnection.DataObjects.AsNoTracking().CountAsync();
        }
*/
}

