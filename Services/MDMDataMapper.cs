using System.Collections.Generic;
using System.Linq;
using MdmService.DTO.Object;
using MdmService.DTO.Study;
using MdmService.Interfaces;
using MdmService.Models.Object;
using MdmService.Models.Study;

namespace MdmService.Helpers
{
    public class DataMapper : IDataMapper
    {
        public ICollection<StudyDataDto> StudyDataDtoBuilder(ICollection<Study> studies)
        {
            return studies is not { Count: > 0 } ? null : studies.Select(StudyDataDtoMapper).ToList();
        }

        public StudyDataDto StudyDataDtoMapper(Study study)
        {
            var studyDataDto = new StudyDataDto()
            {
                Id = study.Id,
                sd_sid = study.sd_sid,
                Mdrsd_sid = study.Mdrsd_sid,
                MdrSourceId = study.MdrSourceId,
                display_title = study.display_title,
                title_lang_code = study.title_lang_code,
                brief_description = study.brief_description,
                data_sharing_statement = study.data_sharing_statement,
                study_start_year = study.study_start_year,
                study_start_month = study.study_start_month,
                study_type_id = study.study_type_id,
                study_status_id = study.study_status_id,
                study_enrolment = study.study_enrolment,
                study_gender_elig_id = study.study_gender_elig_id,
                min_age = study.min_age,
                min_age_units_id = study.min_age_units_id,
                max_age = study.max_age,
                max_age_units_id = study.max_age_units_id,
                created_on = study.created_on.ToString()
            };
            return studyDataDto;
        }

        public ICollection<StudyContributorDto> StudyContributorDtoBuilder(ICollection<StudyContributor> studyContributors)
        {
            return studyContributors is not { Count: > 0 } ? null : studyContributors.Select(StudyContributorDtoMapper).ToList();
        }

        public StudyContributorDto StudyContributorDtoMapper(StudyContributor studyContributor)
        {
            if (studyContributor == null) return null;
            
            var studyContributorDto = new StudyContributorDto
            {
                Id = studyContributor.Id,
                sd_sid = studyContributor.sd_sid,
                contrib_type_id = studyContributor.contrib_type_id,
                is_individual = studyContributor.is_individual,
                person_id = studyContributor.person_id,
                person_given_name = studyContributor.person_given_name,
                person_family_name = studyContributor.person_family_name,
                person_full_name = studyContributor.person_full_name,
                person_affiliation = studyContributor.person_affiliation,
                organisation_id = studyContributor.organisation_id,
                organisation_name = studyContributor.organisation_name,
                organisation_ror_id = studyContributor.organisation_ror_id,
                created_on = studyContributor.created_on
            };

            return studyContributorDto;
        }

        public ICollection<StudyFeatureDto> StudyFeatureDtoBuilder(ICollection<StudyFeature> studyFeatures)
        {
            return studyFeatures is not { Count: > 0 } ? null : studyFeatures.Select(StudyFeatureDtoMapper).ToList();
        }

        public StudyFeatureDto StudyFeatureDtoMapper(StudyFeature studyFeature)
        {
            if (studyFeature == null) return null;
            
            var studyFeatureDto = new StudyFeatureDto
            {
                Id = studyFeature.Id,
                sd_sid = studyFeature.sd_sid,
                feature_type_id = studyFeature.feature_type_id,
                feature_value_id = studyFeature.feature_value_id,
                created_on = studyFeature.created_on
            };

            return studyFeatureDto;
        }

        public ICollection<study_identifierDto> study_identifierDtoBuilder(ICollection<study_identifier> study_identifiers)
        {
            return study_identifiers is not { Count: > 0 } ? null : study_identifiers.Select(study_identifierDtoMapper).ToList();
        }

        public study_identifierDto study_identifierDtoMapper(study_identifier study_identifier)
        {
            if (study_identifier == null) return null;
            
            var study_identifierDto = new study_identifierDto
            {
                Id = study_identifier.Id,
                sd_sid = study_identifier.sd_sid,
                created_on = study_identifier.created_on,
                identifier_type_id = study_identifier.identifier_type_id,
                identifier_value = study_identifier.identifier_value,
                identifier_org_id = study_identifier.identifier_org_id,
                identifier_org = study_identifier.identifier_org,
                identifier_org_ror_id = study_identifier.identifier_org_ror_id,
                identifier_date = study_identifier.identifier_date,
                identifier_link = study_identifier.identifier_link
            };

            return study_identifierDto;
        }

        public ICollection<StudyReferenceDto> StudyReferenceDtoBuilder(ICollection<StudyReference> studyReferences)
        {
            return studyReferences is not { Count: > 0 } ? null : studyReferences.Select(StudyReferenceDtoMapper).ToList();
        }

        public StudyReferenceDto StudyReferenceDtoMapper(StudyReference studyReference)
        {
            if (studyReference == null) return null;
            
            var studyReferenceDto = new StudyReferenceDto
            {
                Id = studyReference.Id,
                sd_sid = studyReference.sd_sid,
                created_on = studyReference.created_on,
                pmid = studyReference.pmid,
                doi = studyReference.doi,
                citation = studyReference.citation,
                comments = studyReference.comments
            };

            return studyReferenceDto;
        }

        public ICollection<StudyRelationshipDto> StudyRelationshipDtoBuilder(ICollection<StudyRelationship> studyRelationships)
        {
            return studyRelationships is not { Count: > 0 } ? null : studyRelationships.Select(StudyRelationshipDtoMapper).ToList();
        }

        public StudyRelationshipDto StudyRelationshipDtoMapper(StudyRelationship studyRelationship)
        {
            if (studyRelationship == null) return null;
            
            var studyRelationshipDto = new StudyRelationshipDto
            {
                Id = studyRelationship.Id,
                sd_sid = studyRelationship.sd_sid,
                created_on = studyRelationship.created_on,
                relationship_type_id = studyRelationship.relationship_type_id,
                target_sd_sid = studyRelationship.target_sd_sid
            };

            return studyRelationshipDto;
        }

        public ICollection<StudyTitleDto> StudyTitleDtoBuilder(ICollection<StudyTitle> studyTitles)
        {
            return studyTitles is not { Count: > 0 } ? null : studyTitles.Select(StudyTitleDtoMapper).ToList();
        }

        public StudyTitleDto StudyTitleDtoMapper(StudyTitle studyTitle)
        {
            if (studyTitle == null) return null;
            
            var studyTitleDto = new StudyTitleDto
            {
                Id = studyTitle.Id,
                sd_sid = studyTitle.sd_sid,
                created_on = studyTitle.created_on,
                is_default = studyTitle.is_default,
                lang_code = studyTitle.lang_code,
                title_text = studyTitle.title_text,
                title_type_id = studyTitle.title_type_id,
                lang_usage_id = studyTitle.lang_usage_id,
                comments = studyTitle.comments
            };

            return studyTitleDto;
        }

        public ICollection<StudyTopicDto> StudyTopicDtoBuilder(ICollection<StudyTopic> studyTopics)
        {
            return studyTopics is not { Count: > 0 } ? null : studyTopics.Select(StudyTopicDtoMapper).ToList();
        }

        public StudyTopicDto StudyTopicDtoMapper(StudyTopic studyTopic)
        {
            if (studyTopic == null) return null;
            
            var studyTopicDto = new StudyTopicDto
            {
                Id = studyTopic.Id,
                sd_sid = studyTopic.sd_sid,
                created_on = studyTopic.created_on,
                topic_type_id = studyTopic.topic_type_id,
                mesh_coded = studyTopic.mesh_coded,
                mesh_code = studyTopic.mesh_code,
                mesh_value = studyTopic.mesh_value,
                original_ct_id = studyTopic.original_ct_id,
                original_ct_code = studyTopic.original_ct_code,
                original_value = studyTopic.original_value,
            };

            return studyTopicDto;
        }

        public ICollection<DataObjectDataDto> DataObjectDataDtoBuilder(ICollection<DataObject> dataObjects)
        {
            return dataObjects is not { Count: > 0 } ? null : dataObjects.Select(DataObjectDataDtoMapper).ToList();
        }

        public DataObjectDataDto DataObjectDataDtoMapper(DataObject dataObject)
        {
            var dataObjectDataDto = new DataObjectDataDto()
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
                created_on = dataObject.created_on
            };
            return dataObjectDataDto;
        }

        public ICollection<ObjectContributorDto> ObjectContributorDtoBuilder(ICollection<ObjectContributor> objectContributors)
        {
            return objectContributors is not { Count: > 0 } ? null : objectContributors.Select(ObjectContributorDtoMapper).ToList();
        }

        public ObjectContributorDto ObjectContributorDtoMapper(ObjectContributor objectContributor)
        {
            if (objectContributor == null) return null;
            
            var objectContributorDto = new ObjectContributorDto
            {
                Id = objectContributor.Id,
                sd_oid = objectContributor.sd_oid,
                created_on = objectContributor.created_on,
                contrib_type_id = objectContributor.contrib_type_id,
                is_individual = objectContributor.is_individual,
                organisation_id = objectContributor.organisation_id,
                organisation_name = objectContributor.organisation_name,
                person_id = objectContributor.person_id,
                person_family_name = objectContributor.person_family_name,
                person_given_name = objectContributor.person_given_name,
                person_full_name = objectContributor.person_full_name,
                person_affiliation = objectContributor.person_affiliation,
                orcid_id = objectContributor.orcid_id
            };

            return objectContributorDto;
        }

        public ICollection<ObjectDatasetDto> ObjectDatasetDtoBuilder(ICollection<ObjectDataset> objectDatasets)
        {
            return objectDatasets is not { Count: > 0 } ? null : objectDatasets.Select(ObjectDatasetDtoMapper).ToList();
        }

        public ObjectDatasetDto ObjectDatasetDtoMapper(ObjectDataset objectDataset)
        {
            if (objectDataset == null) return null;
            
            var objectDatasetDto = new ObjectDatasetDto
            {
                Id = objectDataset.Id,
                sd_oid = objectDataset.sd_oid,
                created_on = objectDataset.created_on,
                record_keys_type_id = objectDataset.record_keys_type_id,
                record_keys_details = objectDataset.record_keys_details,
                deident_type_id = objectDataset.deident_type_id,
                deident_hipaa = objectDataset.deident_hipaa,
                deident_direct = objectDataset.deident_direct,
                deident_dates = objectDataset.deident_dates,
                deident_nonarr = objectDataset.deident_nonarr,
                deident_kanon = objectDataset.deident_kanon,
                deident_details = objectDataset.deident_details,
                consent_type_id = objectDataset.consent_type_id,
                consent_noncommercial = objectDataset.consent_noncommercial,
                consent_geog_restrict = objectDataset.consent_geog_restrict,
                consent_genetic_only = objectDataset.consent_genetic_only,
                consent_research_type = objectDataset.consent_research_type,
                consent_no_methods = objectDataset.consent_no_methods,
                consent_details = objectDataset.consent_details
            };

            return objectDatasetDto;
        }

        public ICollection<ObjectDateDto> ObjectDateDtoBuilder(ICollection<ObjectDate> objectDates)
        {
            return objectDates is not { Count: > 0 } ? null : objectDates.Select(ObjectDateDtoMapper).ToList();
        }

        public ObjectDateDto ObjectDateDtoMapper(ObjectDate objectDate)
        {
            if (objectDate == null) return null;
            
            var objectDateDto = new ObjectDateDto
            {
                Id = objectDate.Id,
                sd_oid = objectDate.sd_oid,
                created_on = objectDate.created_on,
                date_type_id = objectDate.date_type_id,
                date_is_range = objectDate.date_is_range,
                date_as_string = objectDate.date_as_string,
                start_year = objectDate.start_year,
                start_month = objectDate.start_month,
                start_day = objectDate.start_day,
                end_year = objectDate.end_year,
                end_month = objectDate.end_month,
                end_day = objectDate.end_day,
                details = objectDate.details
            };

            return objectDateDto;
        }

        public ICollection<ObjectDescriptionDto> ObjectDescriptionDtoBuilder(ICollection<ObjectDescription> objectDescriptions)
        {
            return objectDescriptions is not { Count: > 0 } ? null : objectDescriptions.Select(ObjectDescriptionDtoMapper).ToList();
        }

        public ObjectDescriptionDto ObjectDescriptionDtoMapper(ObjectDescription objectDescription)
        {
            if (objectDescription == null) return null;
            
            var objectDescriptionDto = new ObjectDescriptionDto
            {
                Id = objectDescription.Id,
                sd_oid = objectDescription.sd_oid,
                created_on = objectDescription.created_on,
                description_type_id = objectDescription.description_type_id,
                description_text = objectDescription.description_text,
                lang_code = objectDescription.lang_code,
                label = objectDescription.label
            };

            return objectDescriptionDto;
        }

        public ICollection<object_identifierDto> object_identifierDtoBuilder(ICollection<object_identifier> object_identifiers)
        {
            return object_identifiers is not { Count: > 0 } ? null : object_identifiers.Select(object_identifierDtoMapper).ToList();
        }

        public object_identifierDto object_identifierDtoMapper(object_identifier object_identifier)
        {
            if (object_identifier == null) return null;
            
            var object_identifierDto = new object_identifierDto
            {
                Id = object_identifier.Id,
                sd_oid = object_identifier.sd_oid,
                created_on = object_identifier.created_on,
                identifier_value = object_identifier.identifier_value,
                identifier_type_id = object_identifier.identifier_type_id,
                identifier_org = object_identifier.identifier_org,
                identifier_org_id = object_identifier.identifier_org_id,
                identifier_date = object_identifier.identifier_date,
                identifier_org_ror_id = object_identifier.identifier_org_ror_id
            };

            return object_identifierDto;
        }

        public ICollection<ObjectInstanceDto> ObjectInstanceDtoBuilder(ICollection<ObjectInstance> objectInstances)
        {
            return objectInstances is not { Count: > 0 } ? null : objectInstances.Select(ObjectInstanceDtoMapper).ToList();
        }

        public ObjectInstanceDto ObjectInstanceDtoMapper(ObjectInstance objectInstance)
        {
            if (objectInstance == null) return null;
            
            var objectInstanceDto = new ObjectInstanceDto
            {
                Id = objectInstance.Id,
                sd_oid = objectInstance.sd_oid,
                created_on = objectInstance.created_on,
                instance_type_id = objectInstance.instance_type_id,
                repository_org_id = objectInstance.repository_org_id,
                repository_org = objectInstance.repository_org,
                url = objectInstance.url,
                url_accessible = objectInstance.url_accessible,
                url_last_checked = objectInstance.url_last_checked,
                resource_type_id = objectInstance.resource_type_id,
                resource_size = objectInstance.resource_size,
                resource_size_units = objectInstance.resource_size_units,
                resource_comments = objectInstance.resource_comments
            };

            return objectInstanceDto;
        }

        public ICollection<ObjectRelationshipDto> ObjectRelationshipDtoBuilder(ICollection<ObjectRelationship> objectRelationships)
        {
            return objectRelationships is not { Count: > 0 } ? null : objectRelationships.Select(ObjectRelationshipDtoMapper).ToList();
        }

        public ObjectRelationshipDto ObjectRelationshipDtoMapper(ObjectRelationship objectRelationship)
        {
            if (objectRelationship == null) return null;
            
            var objectRelationshipDto = new ObjectRelationshipDto
            {
                Id = objectRelationship.Id,
                sd_oid = objectRelationship.sd_oid,
                created_on = objectRelationship.created_on,
                relationship_type_id = objectRelationship.relationship_type_id,
                target_sd_oid = objectRelationship.target_sd_oid
            };

            return objectRelationshipDto;
        }

        public ICollection<ObjectRightDto> ObjectRightDtoBuilder(ICollection<ObjectRight> objectRights)
        {
            return objectRights is not { Count: > 0 } ? null : objectRights.Select(ObjectRightDtoMapper).ToList();
        }

        public ObjectRightDto ObjectRightDtoMapper(ObjectRight objectRight)
        {
            if (objectRight == null) return null;
            
            var objectRightDto = new ObjectRightDto
            {
                Id = objectRight.Id,
                sd_oid = objectRight.sd_oid,
                created_on = objectRight.created_on,
                rights_name = objectRight.rights_name,
                rights_uri = objectRight.rights_uri,
                comments = objectRight.comments
            };

            return objectRightDto;
        }

        public ICollection<ObjectTitleDto> ObjectTitleDtoBuilder(ICollection<ObjectTitle> objectTitles)
        {
            return objectTitles is not { Count: > 0 } ? null : objectTitles.Select(ObjectTitleDtoMapper).ToList();
        }

        public ObjectTitleDto ObjectTitleDtoMapper(ObjectTitle objectTitle)
        {
            if (objectTitle == null) return null;
            
            var objectTitleDto = new ObjectTitleDto
            {
                Id = objectTitle.Id,
                sd_oid = objectTitle.sd_oid,
                created_on = objectTitle.created_on,
                title_type_id = objectTitle.title_type_id,
                is_default = objectTitle.is_default,
                title_text = objectTitle.title_text,
                lang_code = objectTitle.lang_code,
                lang_usage_id = objectTitle.lang_usage_id,
                comments = objectTitle.comments
            };

            return objectTitleDto;
        }

        public ICollection<ObjectTopicDto> ObjectTopicDtoBuilder(ICollection<ObjectTopic> objectTopics)
        {
            return objectTopics is not { Count: > 0 } ? null : objectTopics.Select(ObjectTopicDtoMapper).ToList();
        }

        public ObjectTopicDto ObjectTopicDtoMapper(ObjectTopic objectTopic)
        {
            if (objectTopic == null) return null;
            
            var objectTopicDto = new ObjectTopicDto
            {
                Id = objectTopic.Id,
                sd_oid = objectTopic.sd_oid,
                created_on = objectTopic.created_on,
                topic_type_id = objectTopic.topic_type_id,
                mesh_coded = objectTopic.mesh_coded,
                mesh_code = objectTopic.mesh_code,
                mesh_value = objectTopic.mesh_value,
                original_ct_id = objectTopic.original_ct_id,
                original_ct_code = objectTopic.original_ct_code,
                original_value = objectTopic.original_value,
            };

            return objectTopicDto;
        }
    }
}