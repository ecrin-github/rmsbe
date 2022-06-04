using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MdmService.DTO.Audit;
using MdmService.Contracts.Requests.Filtering;
using MdmService.Contracts.Responses;
using MdmService.DTO.Object;
using MdmService.Interfaces;
using MdmService.Models.DbConnection;
using MdmService.Models.Object;
using Microsoft.EntityFrameworkCore;

namespace rmsbe.DataLayer;

    public class ObjectRepository : IObjectRepository
    {
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
        

        public async Task<ICollection<ObjectContributorDto>> GetObjectContributors(string sd_oid)
        {
            var data = _dbConnection.ObjectContributors.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectContributorDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectContributorDto> GetObjectContributor(int? id)
        {
            var objectContributor = await _dbConnection.ObjectContributors.FirstOrDefaultAsync(p => p.Id == id);
            return objectContributor != null ? _dataMapper.ObjectContributorDtoMapper(objectContributor) : null;
        }

        public async Task<ObjectContributorDto> CreateObjectContributor(ObjectContributorDto objectContributorDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectContributor = new ObjectContributor
            {
                sd_oid = objectContributorDto.sd_oid,
                created_on = DateTime.Now,
                contrib_type_id = objectContributorDto.contrib_type_id,
                is_individual = objectContributorDto.is_individual,
                organisation_id = objectContributorDto.organisation_id,
                organisation_name = objectContributorDto.organisation_name,
                person_id = objectContributorDto.person_id,
                person_family_name = objectContributorDto.person_family_name,
                person_given_name = objectContributorDto.person_given_name,
                person_full_name = objectContributorDto.person_full_name,
                person_affiliation = objectContributorDto.person_affiliation,
                orcid_id = objectContributorDto.orcid_id,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectContributors.AddAsync(objectContributor);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_contributors",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectContributor>(objectContributor).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectContributorDtoMapper(objectContributor);
        }

        public async Task<ObjectContributorDto> UpdateObjectContributor(ObjectContributorDto objectContributorDto, string accessToken)
        {
            var dbObjectContributor =
                await _dbConnection.ObjectContributors.FirstOrDefaultAsync(p => p.sd_oid == objectContributorDto.sd_oid);
            if (dbObjectContributor == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_contributors",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectContributor>(dbObjectContributor).ToString(),
                Post = JsonSerializer.Serialize<ObjectContributorDto>(objectContributorDto).ToString()
            });
            */
            
            dbObjectContributor.contrib_type_id = objectContributorDto.contrib_type_id;
            dbObjectContributor.is_individual = objectContributorDto.is_individual;
            dbObjectContributor.organisation_id = objectContributorDto.organisation_id;
            dbObjectContributor.organisation_name = objectContributorDto.organisation_name;
            dbObjectContributor.person_id = objectContributorDto.person_id;
            dbObjectContributor.person_family_name = objectContributorDto.person_family_name;
            dbObjectContributor.person_given_name = objectContributorDto.person_given_name;
            dbObjectContributor.person_full_name = objectContributorDto.person_full_name;
            dbObjectContributor.person_affiliation = objectContributorDto.person_affiliation;
            dbObjectContributor.orcid_id = objectContributorDto.orcid_id;
            
            dbObjectContributor.last_edited_by = "userData";
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectContributorDtoMapper(dbObjectContributor);
        }

        public async Task<int> DeleteObjectContributor(int id)
        {
            var data = await _dbConnection.ObjectContributors.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectContributors.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectContributors(string sd_oid)
        {
            var data = _dbConnection.ObjectContributors.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectContributors.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ObjectDatasetDto> GetObjectDatasets(string sd_oid)
        {
            var data = _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.sd_oid == sd_oid);
            return data != null ? _dataMapper.ObjectDatasetDtoMapper(await data) : null;
        }

        public async Task<ObjectDatasetDto> GetObjectDataset(int? id)
        {
            var objectDataset = await _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.Id == id);
            return objectDataset != null ? _dataMapper.ObjectDatasetDtoMapper(objectDataset) : null;
        }

        public async Task<ObjectDatasetDto> CreateObjectDataset(ObjectDatasetDto objectDatasetDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectDataset = new ObjectDataset
            {
                sd_oid = objectDatasetDto.sd_oid,
                created_on = DateTime.Now,
                record_keys_type_id = objectDatasetDto.record_keys_type_id,
                record_keys_details = objectDatasetDto.record_keys_details,
                deident_type_id = objectDatasetDto.deident_type_id,
                deident_hipaa = objectDatasetDto.deident_hipaa,
                deident_direct = objectDatasetDto.deident_direct,
                deident_dates = objectDatasetDto.deident_dates,
                deident_nonarr = objectDatasetDto.deident_nonarr,
                deident_kanon = objectDatasetDto.deident_kanon,
                deident_details = objectDatasetDto.deident_details,
                consent_type_id = objectDatasetDto.consent_type_id,
                consent_noncommercial = objectDatasetDto.consent_noncommercial,
                consent_geog_restrict = objectDatasetDto.consent_geog_restrict,
                consent_genetic_only = objectDatasetDto.consent_genetic_only,
                consent_research_type = objectDatasetDto.consent_research_type,
                consent_no_methods = objectDatasetDto.consent_no_methods,
                consent_details = objectDatasetDto.consent_details,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectDatasets.AddAsync(objectDataset);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_datasets",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectDataset>(objectDataset).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDatasetDtoMapper(objectDataset);
        }

        public async Task<ObjectDatasetDto> UpdateObjectDataset(ObjectDatasetDto objectDatasetDto, string accessToken)
        {
            var dbObjectDataset =
                await _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.Id == objectDatasetDto.Id);
            if (dbObjectDataset == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_datasets",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectDataset>(dbObjectDataset).ToString(),
                Post = JsonSerializer.Serialize<ObjectDatasetDto>(objectDatasetDto).ToString()
            });
            */
            
            dbObjectDataset.record_keys_type_id = objectDatasetDto.record_keys_type_id;
            dbObjectDataset.record_keys_details = objectDatasetDto.record_keys_details;
            
            dbObjectDataset.deident_type_id = objectDatasetDto.deident_type_id;
            dbObjectDataset.deident_hipaa = objectDatasetDto.deident_hipaa;
            dbObjectDataset.deident_direct = objectDatasetDto.deident_direct;
            dbObjectDataset.deident_dates = objectDatasetDto.deident_dates;
            dbObjectDataset.deident_nonarr = objectDatasetDto.deident_nonarr;
            dbObjectDataset.deident_kanon = objectDatasetDto.deident_kanon;
            dbObjectDataset.deident_details = objectDatasetDto.deident_details;
            
            dbObjectDataset.consent_type_id = objectDatasetDto.consent_type_id;
            dbObjectDataset.consent_noncommercial = objectDatasetDto.consent_noncommercial;
            dbObjectDataset.consent_geog_restrict = objectDatasetDto.consent_geog_restrict;
            dbObjectDataset.consent_genetic_only = objectDatasetDto.consent_genetic_only;
            dbObjectDataset.consent_research_type = objectDatasetDto.consent_research_type;
            dbObjectDataset.consent_no_methods = objectDatasetDto.consent_no_methods;
            dbObjectDataset.consent_details = objectDatasetDto.consent_details;

            dbObjectDataset.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDatasetDtoMapper(dbObjectDataset);
        }

        public async Task<int> DeleteObjectDataset(int id)
        {
            var data = await _dbConnection.ObjectDatasets.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectDatasets.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectDatasets(string sd_oid)
        {
            var data = _dbConnection.ObjectDatasets.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectDatasets.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<ObjectDateDto>> GetObjectDates(string sd_oid)
        {
            var data = _dbConnection.ObjectDates.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectDateDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectDateDto> GetObjectDate(int? id)
        {
            var objectDate = await _dbConnection.ObjectDates.FirstOrDefaultAsync(p => p.Id == id);
            return objectDate != null ? _dataMapper.ObjectDateDtoMapper(objectDate) : null;
        }

        public async Task<ObjectDateDto> CreateObjectDate(ObjectDateDto objectDateDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectDate = new ObjectDate
            {
                sd_oid = objectDateDto.sd_oid,
                created_on = DateTime.Now,
                date_type_id = objectDateDto.date_type_id,
                date_is_range = objectDateDto.date_is_range,
                date_as_string = objectDateDto.date_as_string,
                start_year = objectDateDto.start_year,
                start_month = objectDateDto.start_month,
                start_day = objectDateDto.start_day,
                end_year = objectDateDto.end_year,
                end_month = objectDateDto.end_month,
                end_day = objectDateDto.end_day,
                details = objectDateDto.details,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectDates.AddAsync(objectDate);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_dates",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectDate>(objectDate).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDateDtoMapper(objectDate);
        }

        public async Task<ObjectDateDto> UpdateObjectDate(ObjectDateDto objectDateDto, string accessToken)
        {
            var dbObjectDate = await _dbConnection.ObjectDates.FirstOrDefaultAsync(p => p.Id == objectDateDto.Id);
            if (dbObjectDate == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);
            

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_dates",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectDate>(dbObjectDate).ToString(),
                Post = JsonSerializer.Serialize<ObjectDateDto>(objectDateDto).ToString()
            });
            */
            
            dbObjectDate.date_type_id = objectDateDto.date_type_id;
            dbObjectDate.date_is_range = objectDateDto.date_is_range;
            dbObjectDate.date_as_string = objectDateDto.date_as_string;
            dbObjectDate.start_year = objectDateDto.start_year;
            dbObjectDate.start_month = objectDateDto.start_month;
            dbObjectDate.start_day = objectDateDto.start_day;
            dbObjectDate.end_year = objectDateDto.end_year;
            dbObjectDate.end_month = objectDateDto.end_month;
            dbObjectDate.end_day = objectDateDto.end_day;
            dbObjectDate.details = objectDateDto.details;

            dbObjectDate.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDateDtoMapper(dbObjectDate);
        }

        public async Task<int> DeleteObjectDate(int id)
        {
            var data = await _dbConnection.ObjectDates.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectDates.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectDates(string sd_oid)
        {
            var data = _dbConnection.ObjectDates.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectDates.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<ObjectDescriptionDto>> GetObjectDescriptions(string sd_oid)
        {
            var data = _dbConnection.ObjectDescriptions.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectDescriptionDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectDescriptionDto> GetObjectDescription(int? id)
        {
            var objectDescription = await _dbConnection.ObjectDescriptions.FirstOrDefaultAsync(p => p.Id == id);
            return objectDescription != null ? _dataMapper.ObjectDescriptionDtoMapper(objectDescription) : null;
        }

        public async Task<ObjectDescriptionDto> CreateObjectDescription(ObjectDescriptionDto objectDescriptionDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectDescription = new ObjectDescription
            {
                sd_oid = objectDescriptionDto.sd_oid,
                created_on = DateTime.Now,
                description_type_id = objectDescriptionDto.description_type_id,
                description_text = objectDescriptionDto.description_text,
                lang_code = objectDescriptionDto.lang_code,
                label = objectDescriptionDto.label,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectDescriptions.AddAsync(objectDescription);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_descriptions",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectDescription>(objectDescription).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDescriptionDtoMapper(objectDescription);
        }

        public async Task<ObjectDescriptionDto> UpdateObjectDescription(ObjectDescriptionDto objectDescriptionDto, string accessToken)
        {
            var dbObjectDescription =
                await _dbConnection.ObjectDescriptions.FirstOrDefaultAsync(p => p.Id == objectDescriptionDto.Id);
            if (dbObjectDescription == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_descriptions",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectDescription>(dbObjectDescription).ToString(),
                Post = JsonSerializer.Serialize<ObjectDescriptionDto>(objectDescriptionDto).ToString()
            });
            */
            
            dbObjectDescription.description_type_id = objectDescriptionDto.description_type_id;
            dbObjectDescription.description_text = objectDescriptionDto.description_text;
            dbObjectDescription.lang_code = objectDescriptionDto.lang_code;
            dbObjectDescription.label = objectDescriptionDto.label;

            dbObjectDescription.last_edited_by = "userData";
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectDescriptionDtoMapper(dbObjectDescription);
        }

        public async Task<int> DeleteObjectDescription(int id)
        {
            var data = await _dbConnection.ObjectDescriptions.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectDescriptions.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectDescriptions(string sd_oid)
        {
            var data = _dbConnection.ObjectDescriptions.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectDescriptions.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<object_identifierDto>> Getobject_identifiers(string sd_oid)
        {
            var data = _dbConnection.object_identifiers.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.object_identifierDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<object_identifierDto> Getobject_identifier(int? id)
        {
            var object_identifier = await _dbConnection.object_identifiers.FirstOrDefaultAsync(p => p.Id == id);
            return object_identifier != null ? _dataMapper.object_identifierDtoMapper(object_identifier) : null;
        }

        public async Task<object_identifierDto> Createobject_identifier(object_identifierDto object_identifierDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var object_identifier = new object_identifier
            {
                sd_oid = object_identifierDto.sd_oid,
                created_on = DateTime.Now,
                identifier_value = object_identifierDto.identifier_value,
                identifier_type_id = object_identifierDto.identifier_type_id,
                identifier_org = object_identifierDto.identifier_org,
                identifier_org_id = object_identifierDto.identifier_org_id,
                identifier_date = object_identifierDto.identifier_date,
                identifier_org_ror_id = object_identifierDto.identifier_org_ror_id,
                last_edited_by = "userData"
            };

            await _dbConnection.object_identifiers.AddAsync(object_identifier);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_identifiers",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<object_identifier>(object_identifier).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.object_identifierDtoMapper(object_identifier);
        }

        public async Task<object_identifierDto> Updateobject_identifier(object_identifierDto object_identifierDto, string accessToken)
        {
            var dbobject_identifier =
                await _dbConnection.object_identifiers.FirstOrDefaultAsync(p => p.Id == object_identifierDto.Id);
            if (dbobject_identifier == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_identifiers",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<object_identifier>(dbobject_identifier).ToString(),
                Post = JsonSerializer.Serialize<object_identifierDto>(object_identifierDto).ToString()
            });
            */
            
            dbobject_identifier.identifier_value = object_identifierDto.identifier_value;
            dbobject_identifier.identifier_type_id = object_identifierDto.identifier_type_id;
            dbobject_identifier.identifier_org = object_identifierDto.identifier_org;
            dbobject_identifier.identifier_org_id = object_identifierDto.identifier_org_id;
            dbobject_identifier.identifier_date = object_identifierDto.identifier_date;
            dbobject_identifier.identifier_org_ror_id = object_identifierDto.identifier_org_ror_id;

            dbobject_identifier.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.object_identifierDtoMapper(dbobject_identifier);
        }

        public async Task<int> Deleteobject_identifier(int id)
        {
            var data = await _dbConnection.object_identifiers.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.object_identifiers.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllobject_identifiers(string sd_oid)
        {
            var data = _dbConnection.object_identifiers.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.object_identifiers.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<ObjectInstanceDto>> GetObjectInstances(string sd_oid)
        {
            var data = _dbConnection.ObjectInstances.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectInstanceDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectInstanceDto> GetObjectInstance(int? id)
        {
            var objectInstance = await _dbConnection.ObjectInstances.FirstOrDefaultAsync(p => p.Id == id);
            return objectInstance != null ? _dataMapper.ObjectInstanceDtoMapper(objectInstance) : null;
        }

        public async Task<ObjectInstanceDto> CreateObjectInstance(ObjectInstanceDto objectInstanceDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectInstance = new ObjectInstance
            {
                sd_oid = objectInstanceDto.sd_oid,
                created_on = DateTime.Now,
                instance_type_id = objectInstanceDto.instance_type_id,
                repository_org_id = objectInstanceDto.repository_org_id,
                repository_org = objectInstanceDto.repository_org,
                url = objectInstanceDto.url,
                url_accessible = objectInstanceDto.url_accessible,
                url_last_checked = objectInstanceDto.url_last_checked,
                resource_type_id = objectInstanceDto.resource_type_id,
                resource_size = objectInstanceDto.resource_size,
                resource_size_units = objectInstanceDto.resource_size_units,
                resource_comments = objectInstanceDto.resource_comments,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectInstances.AddAsync(objectInstance);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_instances",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectInstanceDto>(objectInstanceDto).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectInstanceDtoMapper(objectInstance);
        }

        public async Task<ObjectInstanceDto> UpdateObjectInstance(ObjectInstanceDto objectInstanceDto, string accessToken)
        {
            var dbObjectInstance =
                await _dbConnection.ObjectInstances.FirstOrDefaultAsync(p => p.Id == objectInstanceDto.Id);
            if (dbObjectInstance == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_instances",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectInstance>(dbObjectInstance).ToString(),
                Post = JsonSerializer.Serialize<ObjectInstanceDto>(objectInstanceDto).ToString()
            });
            */
            
            dbObjectInstance.instance_type_id = objectInstanceDto.instance_type_id;
            dbObjectInstance.repository_org_id = objectInstanceDto.repository_org_id;
            dbObjectInstance.repository_org = objectInstanceDto.repository_org;
            dbObjectInstance.url = objectInstanceDto.url;
            dbObjectInstance.url_accessible = objectInstanceDto.url_accessible;
            dbObjectInstance.url_last_checked = objectInstanceDto.url_last_checked;
            dbObjectInstance.resource_type_id = objectInstanceDto.resource_type_id;
            dbObjectInstance.resource_size = objectInstanceDto.resource_size;
            dbObjectInstance.resource_size_units = objectInstanceDto.resource_size_units;
            dbObjectInstance.resource_comments = objectInstanceDto.resource_comments;
            
            dbObjectInstance.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectInstanceDtoMapper(dbObjectInstance);
        }

        public async Task<int> DeleteObjectInstance(int id)
        {
            var data = await _dbConnection.ObjectInstances.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectInstances.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectInstances(string sd_oid)
        {
            var data = _dbConnection.ObjectInstances.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectInstances.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public async Task<ICollection<ObjectRelationshipDto>> GetObjectRelationships(string sd_oid)
        {
            var data = _dbConnection.ObjectRelationships.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectRelationshipDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectRelationshipDto> GetObjectRelationship(int? id)
        {
            var objectRelation = await _dbConnection.ObjectRelationships.FirstOrDefaultAsync(p => p.Id == id);
            return objectRelation != null ? _dataMapper.ObjectRelationshipDtoMapper(objectRelation) : null;
        }

        public async Task<ObjectRelationshipDto> CreateObjectRelationship(ObjectRelationshipDto objectRelationshipDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectRelationship = new ObjectRelationship
            {
                sd_oid = objectRelationshipDto.sd_oid,
                created_on = DateTime.Now,
                relationship_type_id = objectRelationshipDto.relationship_type_id,
                target_sd_oid = objectRelationshipDto.target_sd_oid,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectRelationships.AddAsync(objectRelationship);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_relationships",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectRelationship>(objectRelationship).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRelationshipDtoMapper(objectRelationship);
        }

        public async Task<ObjectRelationshipDto> UpdateObjectRelationship(ObjectRelationshipDto objectRelationshipDto, string accessToken)
        {
            var dbObjectRelation =
                await _dbConnection.ObjectRelationships.FirstOrDefaultAsync(p => p.Id == objectRelationshipDto.Id);
            if (dbObjectRelation == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_relationships",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectRelationship>(dbObjectRelation).ToString(),
                Post = JsonSerializer.Serialize<ObjectRelationshipDto>(objectRelationshipDto).ToString()
            });
            */
            
            dbObjectRelation.relationship_type_id = objectRelationshipDto.relationship_type_id;
            dbObjectRelation.target_sd_oid = objectRelationshipDto.target_sd_oid;

            dbObjectRelation.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRelationshipDtoMapper(dbObjectRelation);
        }

        public async Task<int> DeleteObjectRelationship(int id)
        {
            var data = await _dbConnection.ObjectRelationships.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectRelationships.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectRelationships(string sd_oid)
        {
            var data = _dbConnection.ObjectRelationships.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectRelationships.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<ObjectRightDto>> GetObjectRights(string sd_oid)
        {
            var data = _dbConnection.ObjectRights.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectRightDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectRightDto> GetObjectRight(int? id)
        {
            var objectRight = await _dbConnection.ObjectRights.FirstOrDefaultAsync(p => p.Id == id);
            return objectRight != null ? _dataMapper.ObjectRightDtoMapper(objectRight) : null;
        }

        public async Task<ObjectRightDto> CreateObjectRight(ObjectRightDto objectRightDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectRight = new ObjectRight
            {
                sd_oid = objectRightDto.sd_oid,
                created_on = DateTime.Now,
                rights_name = objectRightDto.rights_name,
                rights_uri = objectRightDto.rights_uri,
                comments = objectRightDto.comments,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectRights.AddAsync(objectRight);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_rights",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectRight>(objectRight).ToString()
            });*/

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRightDtoMapper(objectRight);
        }

        public async Task<ObjectRightDto> UpdateObjectRight(ObjectRightDto objectRightDto, string accessToken)
        {
            var dbObjectRight = await _dbConnection.ObjectRights.FirstOrDefaultAsync(p => p.Id == objectRightDto.Id);
            if (dbObjectRight == null) return null;
            
            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_rights",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectRight>(dbObjectRight).ToString(),
                Post = JsonSerializer.Serialize<ObjectRightDto>(objectRightDto).ToString()
            });
            */

            dbObjectRight.rights_name = objectRightDto.rights_name;
            dbObjectRight.rights_uri = objectRightDto.rights_uri;
            dbObjectRight.comments = objectRightDto.comments;

            dbObjectRight.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectRightDtoMapper(dbObjectRight);
        }

        public async Task<int> DeleteObjectRight(int id)
        {
            var data = await _dbConnection.ObjectRights.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectRights.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectRights(string sd_oid)
        {
            var data = _dbConnection.ObjectRights.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectRights.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<ObjectTitleDto>> GetObjectTitles(string sd_oid)
        {
            var data = _dbConnection.ObjectTitles.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectTitleDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectTitleDto> GetObjectTitle(int? id)
        {
            var objectTitle = await _dbConnection.ObjectTitles.FirstOrDefaultAsync(p => p.Id == id);
            return objectTitle != null ? _dataMapper.ObjectTitleDtoMapper(objectTitle) : null;
        }

        public async Task<ObjectTitleDto> CreateObjectTitle(ObjectTitleDto objectTitleDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectTitle = new ObjectTitle
            {
                sd_oid = objectTitleDto.sd_oid,
                created_on = DateTime.Now,
                title_type_id = objectTitleDto.title_type_id,
                is_default = objectTitleDto.is_default,
                title_text = objectTitleDto.title_text,
                lang_code = objectTitleDto.lang_code,
                lang_usage_id = objectTitleDto.lang_usage_id,
                comments = objectTitleDto.comments,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectTitles.AddAsync(objectTitle);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_titles",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectTitle>(objectTitle).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectTitleDtoMapper(objectTitle);
        }

        public async Task<ObjectTitleDto> UpdateObjectTitle(ObjectTitleDto objectTitleDto, string accessToken)
        {
            var dbObjectTitle = await _dbConnection.ObjectTitles.FirstOrDefaultAsync(p => p.Id == objectTitleDto.Id);
            if (dbObjectTitle == null) return null;

            //var userData = await _userIdentityService.GetUserData(accessToken);
            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_titles",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectTitle>(dbObjectTitle).ToString(),
                Post = JsonSerializer.Serialize<ObjectTitleDto>(objectTitleDto).ToString()
            });
            */
            
            dbObjectTitle.title_type_id = objectTitleDto.title_type_id;
            dbObjectTitle.is_default = objectTitleDto.is_default;
            dbObjectTitle.title_text = objectTitleDto.title_text;
            dbObjectTitle.lang_code = objectTitleDto.lang_code;
            dbObjectTitle.lang_usage_id = objectTitleDto.lang_usage_id;
            dbObjectTitle.comments = objectTitleDto.comments;

            // dbObjectTitle.last_edited_by = userData;
                
            await _dbConnection.SaveChangesAsync();
            return _dataMapper.ObjectTitleDtoMapper(dbObjectTitle);
        }

        public async Task<int> DeleteObjectTitle(int id)
        {
            var data = await _dbConnection.ObjectTitles.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectTitles.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectTitles(string sd_oid)
        {
            var data = _dbConnection.ObjectTitles.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectTitles.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<ObjectTopicDto>> GetObjectTopics(string sd_oid)
        {
            var data = _dbConnection.ObjectTopics.Where(p => p.sd_oid == sd_oid);
            return data.Any() ? _dataMapper.ObjectTopicDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<ObjectTopicDto> GetObjectTopic(int? id)
        {
            var objectTopic = await _dbConnection.ObjectTopics.FirstOrDefaultAsync(p => p.Id == id);
            return objectTopic != null ? _dataMapper.ObjectTopicDtoMapper(objectTopic) : null;
        }

        public async Task<ObjectTopicDto> CreateObjectTopic(ObjectTopicDto objectTopicDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var objectTopic = new ObjectTopic
            {
                sd_oid = objectTopicDto.sd_oid,
                created_on = DateTime.Now,
                topic_type_id = objectTopicDto.topic_type_id,
                mesh_coded = objectTopicDto.mesh_coded,
                mesh_code = objectTopicDto.mesh_code,
                mesh_value = objectTopicDto.mesh_value,
                original_ct_id = objectTopicDto.original_ct_id,
                original_ct_code = objectTopicDto.original_ct_code,
                original_value = objectTopicDto.original_value,
                last_edited_by = "userData"
            };

            await _dbConnection.ObjectTopics.AddAsync(objectTopic);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_topics",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<ObjectTopic>(objectTopic).ToString()
            });
*/
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.ObjectTopicDtoMapper(objectTopic);
        }

        public async Task<ObjectTopicDto> UpdateObjectTopic(ObjectTopicDto objectTopicDto, string accessToken)
        {
            var dbObjectTopic = await _dbConnection.ObjectTopics.FirstOrDefaultAsync(p => p.Id == objectTopicDto.Id);
            if (dbObjectTopic == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "object_topics",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<ObjectTopic>(dbObjectTopic).ToString(),
                Post = JsonSerializer.Serialize<ObjectTopicDto>(objectTopicDto).ToString()
            });
            */
            
            dbObjectTopic.topic_type_id = objectTopicDto.topic_type_id;
            dbObjectTopic.mesh_coded = objectTopicDto.mesh_coded;
            dbObjectTopic.mesh_code = objectTopicDto.mesh_code;
            dbObjectTopic.mesh_value = objectTopicDto.mesh_value;
            dbObjectTopic.original_ct_id = objectTopicDto.original_ct_id;
            dbObjectTopic.original_ct_code = objectTopicDto.original_ct_code;
            dbObjectTopic.original_value = objectTopicDto.original_value;

            dbObjectTopic.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            return _dataMapper.ObjectTopicDtoMapper(dbObjectTopic);
        }

        public async Task<int> DeleteObjectTopic(int id)
        {
            var data = await _dbConnection.ObjectTopics.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.ObjectTopics.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllObjectTopics(string sd_oid)
        {
            var data = _dbConnection.ObjectTopics.Where(p => p.sd_oid == sd_oid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.ObjectTopics.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }


        // DATA OBJECT
        public async Task<ICollection<DataObjectDto>> GetAllDataObjects()
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

        public async Task<DataObjectDto> GetObjectById(string sd_oid)
        {
            var dataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == sd_oid);
            if (dataObject == null) return null;
            return await DataObjectBuilder(dataObject);
        }

        public async Task<DataObjectDto> CreateDataObject(DataObjectDto dataObjectDto, string accessToken)
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

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "data_objects",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<DataObject>(dataObject).ToString()
            });
            */

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

        public async Task<DataObjectDto> UpdateDataObject(DataObjectDto dataObjectDto, string accessToken)
        {
            var dbDataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == dataObjectDto.sd_oid);
            if (dbDataObject == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "data_objects",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<DataObject>(dbDataObject).ToString(),
                Post = JsonSerializer.Serialize<DataObjectDto>(dataObjectDto).ToString()
            });
            */
            
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

        public async Task<int> DeleteDataObject(string sd_oid)
        {
            var dataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == sd_oid);
            if (dataObject == null) return 0;
            _dbConnection.DataObjects.Remove(dataObject);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<ICollection<DataObjectDataDto>> GetDataObjectsData()
        {
            if (!_dbConnection.DataObjects.Any()) return null;
            var dataObjects = await _dbConnection.DataObjects.ToArrayAsync();

            return dataObjects.Select(dataObject => _dataMapper.DataObjectDataDtoMapper(dataObject)).ToList();
        }

        public async Task<DataObjectDataDto> GetDataObjectData(string sd_oid)
        {
            var data = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_sid == sd_oid);
            return data == null ? null : _dataMapper.DataObjectDataDtoMapper(data);
        }

        public async Task<ICollection<DataObjectDataDto>> GetRecentObjectData(int limit)
        {
            if (!_dbConnection.DataObjects.Any()) return null;

            var recentObjects = await _dbConnection.DataObjects.OrderByDescending(p => p.Id).Take(limit).ToArrayAsync();
            return _dataMapper.DataObjectDataDtoBuilder(recentObjects);
        }

        public async Task<DataObjectDataDto> CreateDataObjectData(DataObjectDataDto dataObjectData, string accessToken)
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

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "data_objects",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<DataObject>(dataObject).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DataObjectDataDtoMapper(dataObject);
        }
        
        public async Task<DataObjectDataDto> UpdateDataObjectData(DataObjectDataDto dataObjectData, string accessToken)
        {
            var dbDataObject = await _dbConnection.DataObjects.FirstOrDefaultAsync(p => p.sd_oid == dataObjectData.sd_oid);
            if (dbDataObject == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "data_objects",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<DataObject>(dbDataObject).ToString(),
                Post = JsonSerializer.Serialize<DataObjectDataDto>(dataObjectData).ToString()
            });
            */
            
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


        private async Task<DataObjectDto> DataObjectBuilder(DataObject dataObject)
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

        public async Task<PaginationResponse<DataObjectDto>> PaginateDataObjects(PaginationRequest paginationRequest)
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

        public async Task<PaginationResponse<DataObjectDto>> FilterDataObjectsByTitle(FilteringByTitleRequest filteringByTitleRequest)
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

        public async Task<int> GetTotalDataObjects()
        {
            return await _dbConnection.DataObjects.AsNoTracking().CountAsync();
        }
    }
