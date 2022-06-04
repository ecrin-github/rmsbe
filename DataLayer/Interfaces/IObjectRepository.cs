using System.Collections.Generic;
using System.Threading.Tasks;
using MdmService.Contracts.Requests.Filtering;
using MdmService.Contracts.Responses;
using MdmService.DTO.Object;

namespace rmsbe.DataLayer.Interfaces;

    public interface IObjectRepository
    {
        // Data objects
        Task<ICollection<DataObjectDto>> GetAllDataObjects();
        Task<DataObjectDto> GetObjectById(string sd_oid);
        Task<DataObjectDto> CreateDataObject(DataObjectDto dataObjectDto, string accessToken);
        Task<DataObjectDto> UpdateDataObject(DataObjectDto dataObjectDto, string accessToken);
        Task<int> DeleteDataObject(string sd_oid);
        
        //Data objects data
        Task<ICollection<DataObjectDataDto>> GetDataObjectsData();
        Task<DataObjectDataDto> GetDataObjectData(string sd_oid);
        Task<ICollection<DataObjectDataDto>> GetRecentObjectData(int limit);
        Task<DataObjectDataDto> CreateDataObjectData(DataObjectDataDto dataObjectData, string accessToken);
        Task<DataObjectDataDto> UpdateDataObjectData(DataObjectDataDto dataObjectData, string accessToken);

        // Object contributors
        Task<ICollection<ObjectContributorDto>> GetObjectContributors(string sd_oid);
        Task<ObjectContributorDto> GetObjectContributor(int? id);
        Task<ObjectContributorDto> CreateObjectContributor(ObjectContributorDto objectContributorDto, string accessToken);
        Task<ObjectContributorDto> UpdateObjectContributor(ObjectContributorDto objectContributorDto, string accessToken);
        Task<int> DeleteObjectContributor(int id);
        Task<int> DeleteAllObjectContributors(string sd_oid);

        // Object datasets
        Task<ObjectDatasetDto> GetObjectDatasets(string sd_oid);
        Task<ObjectDatasetDto> GetObjectDataset(int? id);
        Task<ObjectDatasetDto> CreateObjectDataset(ObjectDatasetDto objectDatasetDto, string accessToken);
        Task<ObjectDatasetDto> UpdateObjectDataset(ObjectDatasetDto objectDatasetDto, string accessToken);
        Task<int> DeleteObjectDataset(int id);
        Task<int> DeleteAllObjectDatasets(string sd_oid);

        // Object dates
        Task<ICollection<ObjectDateDto>> GetObjectDates(string sd_oid);
        Task<ObjectDateDto> GetObjectDate(int? id);
        Task<ObjectDateDto> CreateObjectDate(ObjectDateDto objectDateDto, string accessToken);
        Task<ObjectDateDto> UpdateObjectDate(ObjectDateDto objectDateDto, string accessToken);
        Task<int> DeleteObjectDate(int id);
        Task<int> DeleteAllObjectDates(string sd_oid);

        // Object descriptions
        Task<ICollection<ObjectDescriptionDto>> GetObjectDescriptions(string sd_oid);
        Task<ObjectDescriptionDto> GetObjectDescription(int? id);
        Task<ObjectDescriptionDto> CreateObjectDescription(ObjectDescriptionDto objectDescriptionDto, string accessToken);
        Task<ObjectDescriptionDto> UpdateObjectDescription(ObjectDescriptionDto objectDescriptionDto, string accessToken);
        Task<int> DeleteObjectDescription(int id);
        Task<int> DeleteAllObjectDescriptions(string sd_oid);

        // Object identifiers
        Task<ICollection<object_identifierDto>> Getobject_identifiers(string sd_oid);
        Task<object_identifierDto> Getobject_identifier(int? id);
        Task<object_identifierDto> Createobject_identifier(object_identifierDto object_identifierDto, string accessToken);
        Task<object_identifierDto> Updateobject_identifier(object_identifierDto object_identifierDto, string accessToken);
        Task<int> Deleteobject_identifier(int id);
        Task<int> DeleteAllobject_identifiers(string sd_oid);

        // Object instances
        Task<ICollection<ObjectInstanceDto>> GetObjectInstances(string sd_oid);
        Task<ObjectInstanceDto> GetObjectInstance(int? id);
        Task<ObjectInstanceDto> CreateObjectInstance(ObjectInstanceDto objectInstanceDto, string accessToken);
        Task<ObjectInstanceDto> UpdateObjectInstance(ObjectInstanceDto objectInstanceDto, string accessToken);
        Task<int> DeleteObjectInstance(int id);
        Task<int> DeleteAllObjectInstances(string sd_oid);

        // Object relationships
        Task<ICollection<ObjectRelationshipDto>> GetObjectRelationships(string sd_oid);
        Task<ObjectRelationshipDto> GetObjectRelationship(int? id);
        Task<ObjectRelationshipDto> CreateObjectRelationship(ObjectRelationshipDto objectRelationshipDto, string accessToken);
        Task<ObjectRelationshipDto> UpdateObjectRelationship(ObjectRelationshipDto objectRelationshipDto, string accessToken);
        Task<int> DeleteObjectRelationship(int id);
        Task<int> DeleteAllObjectRelationships(string sd_oid);

        // Object rights
        Task<ICollection<ObjectRightDto>> GetObjectRights(string sd_oid);
        Task<ObjectRightDto> GetObjectRight(int? id);
        Task<ObjectRightDto> CreateObjectRight(ObjectRightDto objectRightDto, string accessToken);
        Task<ObjectRightDto> UpdateObjectRight(ObjectRightDto objectRightDto, string accessToken);
        Task<int> DeleteObjectRight(int id);
        Task<int> DeleteAllObjectRights(string sd_oid);

        // Object titles
        Task<ICollection<ObjectTitleDto>> GetObjectTitles(string sd_oid);
        Task<ObjectTitleDto> GetObjectTitle(int? id);
        Task<ObjectTitleDto> CreateObjectTitle(ObjectTitleDto objectTitleDto, string accessToken);
        Task<ObjectTitleDto> UpdateObjectTitle(ObjectTitleDto objectTitleDto, string accessToken);
        Task<int> DeleteObjectTitle(int id);
        Task<int> DeleteAllObjectTitles(string sd_oid);
        

        // Object topics
        Task<ICollection<ObjectTopicDto>> GetObjectTopics(string sd_oid);
        Task<ObjectTopicDto> GetObjectTopic(int? id);
        Task<ObjectTopicDto> CreateObjectTopic(ObjectTopicDto objectTopicDto, string accessToken);
        Task<ObjectTopicDto> UpdateObjectTopic(ObjectTopicDto objectTopicDto, string accessToken);
        Task<int> DeleteObjectTopic(int id);
        Task<int> DeleteAllObjectTopics(string sd_oid);

        // Extensions
        Task<PaginationResponse<DataObjectDto>> PaginateDataObjects(PaginationRequest paginationRequest);
        Task<PaginationResponse<DataObjectDto>> FilterDataObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest);
        Task<int> GetTotalDataObjects();
    }
