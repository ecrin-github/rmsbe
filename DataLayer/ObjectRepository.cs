using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;
using Dapper.Contrib.Extensions;

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
    public async Task<FullObjectInDb?> GetFullObjectByIdAsync(string sd_oid)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $"select * from mdr.data_objects where sd_oid = '{sd_oid}'";   
        DataObjectInDb? coreStudy = await conn.QueryFirstOrDefaultAsync<DataObjectInDb>(sqlString);     
        sqlString = $"select * from mdr.object_contributors where sd_oid = '{sd_oid}'";
        var contribs = (await conn.QueryAsync<ObjectContributorInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_datasets where sd_oid = '{sd_oid}'";
        var datasets = (await conn.QueryAsync<ObjectDatasetInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_dates where sd_oid = '{sd_oid}'";
        var dates = (await conn.QueryAsync<ObjectDateInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_descriptions where sd_oid = '{sd_oid}'";
        var descs = (await conn.QueryAsync<ObjectDescriptionInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_identifiers where sd_oid = '{sd_oid}'";
        var idents = (await conn.QueryAsync<ObjectIdentifierInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_instances where sd_oid = '{sd_oid}'";
        var insts = (await conn.QueryAsync<ObjectInstanceInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_relationships where sd_oid = '{sd_oid}'";
        var rels = (await conn.QueryAsync<ObjectRelationshipInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_rights where sd_oid = '{sd_oid}'";
        var rights = (await conn.QueryAsync<ObjectRightInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_titles where sd_oid = '{sd_oid}'";
        var titles = (await conn.QueryAsync<ObjectTitleInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.object_topics where sd_oid = '{sd_oid}'";
        var topics = (await conn.QueryAsync<ObjectTopicInDb>(sqlString)).ToList();
        
        return new FullObjectInDb(coreStudy, contribs, datasets, dates, descs, 
                                  idents, insts, rels, rights, titles, topics);
    }

    // Update data
    public async Task<int> DeleteFullObjectAsync(string sd_oid, string user_name)
    {
         await using var conn = new NpgsqlConnection(_dbConnString);
        
         var sqlString = $@"update mdr.object_contributors set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                           delete from mdr.object_contributors where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
        
         sqlString = $@"update mdr.object_datasets set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                        delete from mdr.object_datasets where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
         
         sqlString = $@"update mdr.object_dates set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                        delete from mdr.object_dates where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
         
         sqlString = $@"update mdr.object_descriptions set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                        delete from mdr.object_descriptions where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
                
         sqlString = $@"update mdr.object_identifiers set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                        delete from mdr.object_identifiers where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
         
         sqlString = $@"update mdr.object_instances set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                        delete from mdr.object_instances where sd_sid = '{sd_oid}';";
         
         await conn.ExecuteAsync(sqlString);
         sqlString = $@"update mdr.object_relationships set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                              delete from mdr.object_relationships where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
         
         sqlString = $@"update mdr.object_rights set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                              delete from mdr.object_rights where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
         
         sqlString = $@"update mdr.object_titles set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                              delete from mdr.object_titles where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
        
         sqlString = $@"update mdr.object_topics set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                              delete from mdr.object_topics where sd_sid = '{sd_oid}';";
         await conn.ExecuteAsync(sqlString);
        
         sqlString = $@"update mdr.data_objects set last_edited_by = '{user_name}' where sd_sid = '{sd_oid}';
                              delete from mdr.data_objects where sd_sid = '{sd_oid}';";
         return await conn.ExecuteAsync(sqlString);
    }
    
    /****************************************************************
    * Data Object data (without attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<DataObjectInDb>?> GetDataObjectsDataAsync()
    {
        string sqlString = $"select * from mdr.data_objects";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DataObjectInDb>(sqlString);
    }

    public async Task<IEnumerable<DataObjectInDb>?> GetRecentObjectDataAsync(int n)
    {
        string sqlString = $@"select * from mdr.data_objects 
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<DataObjectInDb>(sqlString);
    }

    public async Task<DataObjectInDb?> GetDataObjectDataAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.data_objects where sd_oid = {sd_oid}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<DataObjectInDb>(sqlString);
    }

    // Update data
    public async Task<DataObjectInDb?> CreateDataObjectDataAsync(DataObjectInDb dataObjectData)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(dataObjectData);
        string sqlString = $"select * from mdr.data_objects where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<DataObjectInDb>(sqlString);
    }

    public async Task<DataObjectInDb?> UpdateDataObjectDataAsync(DataObjectInDb dataObjectData)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(dataObjectData)) ? dataObjectData : null;
    }

    public async Task<int> DeleteDataObjectDataAsync(string sd_oid, string user_name)
    {
        string sqlString = $@"update mdr.data_objects 
                              set last_edited_by = {user_name}
                              where sd_oid = '{sd_oid}';
                              delete from mdr.studies 
                              where sd_oid = '{sd_oid}';";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
    
    /****************************************************************
    * Object contributors
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectContributorInDb>?> GetObjectContributorsAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_contributors where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectContributorInDb>(sqlString);
    }

    public async Task<ObjectContributorInDb?> GetObjectContributorAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_contributors where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectContributorInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectContributorInDb?> CreateObjectContributorAsync(ObjectContributorInDb objectContributorInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectContributorInDb);
        string sqlString = $"select * from mdr.object_contributors where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectContributorInDb>(sqlString);
    }

    public async Task<ObjectContributorInDb?> UpdateObjectContributorAsync(ObjectContributorInDb objectContributorInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectContributorInDb)) ? objectContributorInDb : null;
    }

    public async Task<int> DeleteObjectContributorAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_contributors 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_contributors 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object datasets
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectDatasetInDb>?> GetObjectDatasetsAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_datasets where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectDatasetInDb>(sqlString);
    }

    public async Task<ObjectDatasetInDb?> GetObjectDatasetAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_datasets where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectDatasetInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectDatasetInDb?> CreateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectDatasetInDb);
        string sqlString = $"select * from mdr.object_datasets where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectDatasetInDb>(sqlString);
    }

    public async Task<ObjectDatasetInDb?> UpdateObjectDatasetAsync(ObjectDatasetInDb objectDatasetInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectDatasetInDb)) ? objectDatasetInDb : null;
    }

    public async Task<int> DeleteObjectDatasetAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_datasets 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_datasets 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object dates 
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectDateInDb>?> GetObjectDatesAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_dates where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectDateInDb>(sqlString);
    }

    public async Task<ObjectDateInDb?> GetObjectDateAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_dates where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectDateInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectDateInDb?> CreateObjectDateAsync(ObjectDateInDb objectDateInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectDateInDb);
        string sqlString = $"select * from mdr.object_dates where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectDateInDb>(sqlString);
    }

    public async Task<ObjectDateInDb?> UpdateObjectDateAsync(ObjectDateInDb objectDateInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectDateInDb)) ? objectDateInDb : null;
    }

    public async Task<int> DeleteObjectDateAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_dates 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_dates 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object descriptions
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectDescriptionInDb>?> GetObjectDescriptionsAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_descriptions where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectDescriptionInDb>(sqlString);
    }

    public async Task<ObjectDescriptionInDb?> GetObjectDescriptionAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_descriptions where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectDescriptionInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectDescriptionInDb?> CreateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectDescriptionInDb);
        string sqlString = $"select * from mdr.object_descriptions where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectDescriptionInDb>(sqlString);
    }

    public async Task<ObjectDescriptionInDb?> UpdateObjectDescriptionAsync(ObjectDescriptionInDb objectDescriptionInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectDescriptionInDb)) ? objectDescriptionInDb : null;
    }

    public async Task<int> DeleteObjectDescriptionAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_descriptions 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_descriptions 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object identifiers
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectIdentifierInDb>?> GetObjectIdentifiersAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_identifiers where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectIdentifierInDb>(sqlString);
    }

    public async Task<ObjectIdentifierInDb?> GetObjectIdentifierAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_identifiers where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectIdentifierInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectIdentifierInDb?> CreateObjectIdentifierAsync(ObjectIdentifierInDb objectIdentifierInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectIdentifierInDb);
        string sqlString = $"select * from mdr.object_identifiers where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectIdentifierInDb>(sqlString);
    }

    public async Task<ObjectIdentifierInDb?> UpdateObjectIdentifierAsync(ObjectIdentifierInDb objectIdentifierInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectIdentifierInDb)) ? objectIdentifierInDb : null;
    }

    public async Task<int> DeleteObjectIdentifierAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_identifiers 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_identifiers 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object instances
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectInstanceInDb>?> GetObjectInstancesAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_instances where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectInstanceInDb>(sqlString);
    }

    public async Task<ObjectInstanceInDb?> GetObjectInstanceAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_instances where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectInstanceInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectInstanceInDb?> CreateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectInstanceInDb);
        string sqlString = $"select * from mdr.object_instances where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectInstanceInDb>(sqlString);
    }

    public async Task<ObjectInstanceInDb?> UpdateObjectInstanceAsync(ObjectInstanceInDb objectInstanceInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectInstanceInDb)) ? objectInstanceInDb : null;
    }

    public async Task<int> DeleteObjectInstanceAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_instances 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_instances 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object relationships
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectRelationshipInDb>?> GetObjectRelationshipsAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_relationships where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectRelationshipInDb>(sqlString);
    }

    public async Task<ObjectRelationshipInDb?> GetObjectRelationshipAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_relationships where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectRelationshipInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectRelationshipInDb?> CreateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectRelationshipInDb);
        string sqlString = $"select * from mdr.object_relationships where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectRelationshipInDb>(sqlString);
    }

    public async Task<ObjectRelationshipInDb?> UpdateObjectRelationshipAsync(ObjectRelationshipInDb objectRelationshipInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectRelationshipInDb)) ? objectRelationshipInDb : null;
    }

    public async Task<int> DeleteObjectRelationshipAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_relationships 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_relationships 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Object rights
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectRightInDb>?> GetObjectRightsAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_rights where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectRightInDb>(sqlString);
    }

    public async Task<ObjectRightInDb?> GetObjectRightAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_rights where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectRightInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectRightInDb?> CreateObjectRightAsync(ObjectRightInDb objectRightInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectRightInDb);
        string sqlString = $"select * from mdr.object_rights where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectRightInDb>(sqlString);
    }

    public async Task<ObjectRightInDb?> UpdateObjectRightAsync(ObjectRightInDb objectRightInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectRightInDb)) ? objectRightInDb : null;
    }

    public async Task<int> DeleteObjectRightAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_rights 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_rights 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

   
    /****************************************************************
    * Object titles
    ****************************************************************/

    // Fetch data
    public async Task<IEnumerable<ObjectTitleInDb>?> GetObjectTitlesAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_titles where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectTitleInDb>(sqlString);
    }

    public async Task<ObjectTitleInDb?> GetObjectTitleAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_titles where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectTitleInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectTitleInDb?> CreateObjectTitleAsync(ObjectTitleInDb objectTitleInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectTitleInDb);
        string sqlString = $"select * from mdr.object_titles where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectTitleInDb>(sqlString);
    }

    public async Task<ObjectTitleInDb?> UpdateObjectTitleAsync(ObjectTitleInDb objectTitleInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectTitleInDb)) ? objectTitleInDb : null;
    }

    public async Task<int> DeleteObjectTitleAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_titles 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_titles 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
  
    /****************************************************************
    * Object topics
    ****************************************************************/
    
    // Fetch data
    public async Task<IEnumerable<ObjectTopicInDb>?> GetObjectTopicsAsync(string sd_oid)
    {
        string sqlString = $"select * from mdr.object_topics where sd_oid = '{sd_oid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<ObjectTopicInDb>(sqlString);
    }

    public async Task<ObjectTopicInDb?> GetObjectTopicAsync(int? id)
    {
        string sqlString = $"select * from mdr.object_topics where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<ObjectTopicInDb>(sqlString);
    }

    // Update data
    public async Task<ObjectTopicInDb?> CreateObjectTopicAsync(ObjectTopicInDb objectTopicInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(objectTopicInDb);
        string sqlString = $"select * from mdr.object_topics where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<ObjectTopicInDb>(sqlString);
    }

    public async Task<ObjectTopicInDb?> UpdateObjectTopicAsync(ObjectTopicInDb objectTopicInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(objectTopicInDb)) ? objectTopicInDb : null;
    }

    public async Task<int> DeleteObjectTopicAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.object_topics 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.object_topics 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    // Extensions
    /*
    public async Task<PaginationResponse<DataObjectInDb>?> PaginateDataObjects(PaginationRequest paginationRequest);
    public async Task<PaginationResponse<DataObjectInDb>?> FilterDataObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest);
    public async Task<int> GetTotalDataObjects();
    */

}

