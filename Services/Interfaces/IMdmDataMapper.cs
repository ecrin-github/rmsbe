using System.Collections.Generic;
using MdmService.DTO.Object;
using MdmService.DTO.Study;
using MdmService.Models.Object;
using MdmService.Models.Study;

namespace MdmService.Interfaces
{
    public interface IMdmDataMapper
    {
        // Study related data
        ICollection<StudyDataDto> StudyDataDtoBuilder(ICollection<Study> studies);
        StudyDataDto StudyDataDtoMapper(Study study);

        ICollection<StudyContributorDto> StudyContributorDtoBuilder(ICollection<StudyContributor> studyContributors);
        StudyContributorDto StudyContributorDtoMapper(StudyContributor studyContributor);
        
        ICollection<StudyFeatureDto> StudyFeatureDtoBuilder(ICollection<StudyFeature> studyFeatures);
        StudyFeatureDto StudyFeatureDtoMapper(StudyFeature studyFeature);
        
        ICollection<study_identifierDto> study_identifierDtoBuilder(ICollection<study_identifier> study_identifiers);
        study_identifierDto study_identifierDtoMapper(study_identifier study_identifier);
        
        ICollection<StudyReferenceDto> StudyReferenceDtoBuilder(ICollection<StudyReference> studyReferences);
        StudyReferenceDto StudyReferenceDtoMapper(StudyReference studyReference);
        
        ICollection<StudyRelationshipDto> StudyRelationshipDtoBuilder(ICollection<StudyRelationship> studyRelationships);
        StudyRelationshipDto StudyRelationshipDtoMapper(StudyRelationship studyRelationship);
        
        ICollection<StudyTitleDto> StudyTitleDtoBuilder(ICollection<StudyTitle> studyTitles);
        StudyTitleDto StudyTitleDtoMapper(StudyTitle studyTitle);
        
        ICollection<StudyTopicDto> StudyTopicDtoBuilder(ICollection<StudyTopic> studyTopics);
        StudyTopicDto StudyTopicDtoMapper(StudyTopic studyTopic);
        

        // Data object related data
        ICollection<DataObjectDataDto> DataObjectDataDtoBuilder(ICollection<DataObject> dataObjects);
        DataObjectDataDto DataObjectDataDtoMapper(DataObject dataObject);

        ICollection<ObjectContributorDto> ObjectContributorDtoBuilder(ICollection<ObjectContributor> objectContributors);
        ObjectContributorDto ObjectContributorDtoMapper(ObjectContributor objectContributor);
        
        ICollection<ObjectDatasetDto> ObjectDatasetDtoBuilder(ICollection<ObjectDataset> objectDatasets);
        ObjectDatasetDto ObjectDatasetDtoMapper(ObjectDataset objectDataset);
        
        ICollection<ObjectDateDto> ObjectDateDtoBuilder(ICollection<ObjectDate> objectDates);
        ObjectDateDto ObjectDateDtoMapper(ObjectDate objectDate);
        
        ICollection<ObjectDescriptionDto> ObjectDescriptionDtoBuilder(ICollection<ObjectDescription> objectDescriptions);
        ObjectDescriptionDto ObjectDescriptionDtoMapper(ObjectDescription objectDescription);
        
        ICollection<object_identifierDto> object_identifierDtoBuilder(ICollection<object_identifier> object_identifiers);
        object_identifierDto object_identifierDtoMapper(object_identifier object_identifier);
        
        ICollection<ObjectInstanceDto> ObjectInstanceDtoBuilder(ICollection<ObjectInstance> objectInstances);
        ObjectInstanceDto ObjectInstanceDtoMapper(ObjectInstance objectInstance);
        
        ICollection<ObjectRelationshipDto> ObjectRelationshipDtoBuilder(ICollection<ObjectRelationship> objectRelationships);
        ObjectRelationshipDto ObjectRelationshipDtoMapper(ObjectRelationship objectRelationship);
        
        ICollection<ObjectRightDto> ObjectRightDtoBuilder(ICollection<ObjectRight> objectRights);
        ObjectRightDto ObjectRightDtoMapper(ObjectRight objectRight);
        
        ICollection<ObjectTitleDto> ObjectTitleDtoBuilder(ICollection<ObjectTitle> objectTitles);
        ObjectTitleDto ObjectTitleDtoMapper(ObjectTitle objectTitle);
        
        ICollection<ObjectTopicDto> ObjectTopicDtoBuilder(ICollection<ObjectTopic> objectTopics);
        ObjectTopicDto ObjectTopicDtoMapper(ObjectTopic objectTopic);
    }
}