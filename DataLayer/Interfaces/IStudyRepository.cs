using MdmService.Contracts.Requests.Filtering;
using MdmService.DTO.Study;

namespace rmsbe.DataLayer.Interfaces;

public interface IStudyRepository
{
    // Study
    Task<ICollection<StudyDto>> GetAllStudies();
    Task<StudyDto> GetStudyById(string sd_sid);
    Task<StudyDto> CreateStudy(StudyDto studyDto, string accessToken);
    Task<StudyDto> UpdateStudy(StudyDto studyDto, string accessToken);
    Task<int> DeleteStudy(string sd_sid);
    
    // Study data
    Task<ICollection<StudyDataDto>> GetStudiesData();
    Task<StudyDataDto> GetStudyData(string sd_sid);
    Task<ICollection<StudyDataDto>> GetRecentStudyData(int limit);
    Task<StudyDataDto> CreateStudyData(StudyDataDto studyData, string accessToken);
    Task<StudyDataDto> UpdateStudyData(StudyDataDto studyData, string accessToken);

    // Study contributors
    Task<ICollection<StudyContributorDto>> GetStudyContributors(string sd_sid);
    Task<StudyContributorDto> GetStudyContributor(int? id);
    Task<StudyContributorDto> CreateStudyContributor(StudyContributorDto studyContributorDto, string accessToken);
    Task<StudyContributorDto> UpdateStudyContributor(StudyContributorDto studyContributorDto, string accessToken);
    Task<int> DeleteStudyContributor(int id);
    Task<int> DeleteAllStudyContributors(string sd_sid);

    // Study features
    Task<ICollection<StudyFeatureDto>> GetStudyFeatures(string sd_sid);
    Task<StudyFeatureDto> GetStudyFeature(int? id);
    Task<StudyFeatureDto> CreateStudyFeature(StudyFeatureDto studyFeatureDto, string accessToken);
    Task<StudyFeatureDto> UpdateStudyFeature(StudyFeatureDto studyFeatureDto, string accessToken);
    Task<int> DeleteStudyFeature(int id);
    Task<int> DeleteAllStudyFeatures(string sd_sid);

    // Study identifiers
    Task<ICollection<study_identifierDto>> Getstudy_identifiers(string sd_sid);
    Task<study_identifierDto> Getstudy_identifier(int? id);
    Task<study_identifierDto> Createstudy_identifier(study_identifierDto study_identifierDto, string accessToken);
    Task<study_identifierDto> Updatestudy_identifier(study_identifierDto study_identifierDto, string accessToken);
    Task<int> Deletestudy_identifier(int id);
    Task<int> DeleteAllstudy_identifiers(string sd_sid);

    // Study references
    Task<ICollection<StudyReferenceDto>> GetStudyReferences(string sd_sid);
    Task<StudyReferenceDto> GetStudyReference(int? id);
    Task<StudyReferenceDto> CreateStudyReference(StudyReferenceDto studyReferenceDto, string accessToken);
    Task<StudyReferenceDto> UpdateStudyReference(StudyReferenceDto studyReferenceDto, string accessToken);
    Task<int> DeleteStudyReference(int id);
    Task<int> DeleteAllStudyReferences(string sd_sid);

    // Study relationships
    Task<ICollection<StudyRelationshipDto>> GetStudyRelationships(string sd_sid);
    Task<StudyRelationshipDto> GetStudyRelationship(int? id);
    Task<StudyRelationshipDto> CreateStudyRelationship(StudyRelationshipDto studyRelationshipDto, string accessToken);
    Task<StudyRelationshipDto> UpdateStudyRelationship(StudyRelationshipDto studyRelationshipDto, string accessToken);
    Task<int> DeleteStudyRelationship(int id);
    Task<int> DeleteAllStudyRelationships(string sd_sid);

    // Study titles
    Task<ICollection<StudyTitleDto>> GetStudyTitles(string sd_sid);
    Task<StudyTitleDto> GetStudyTitle(int? id);
    Task<StudyTitleDto> CreateStudyTitle(StudyTitleDto studyTitleDto, string accessToken);
    Task<StudyTitleDto> UpdateStudyTitle(StudyTitleDto studyTitleDto, string accessToken);
    Task<int> DeleteStudyTitle(int id);
    Task<int> DeleteAllStudyTitles(string sd_sid);

    // Study topics
    Task<ICollection<StudyTopicDto>> GetStudyTopics(string sd_sid);
    Task<StudyTopicDto> GetStudyTopic(int? id);
    Task<StudyTopicDto> CreateStudyTopic(StudyTopicDto studyTopicDto, string accessToken);
    Task<StudyTopicDto> UpdateStudyTopic(StudyTopicDto studyTopicDto, string accessToken);
    Task<int> DeleteStudyTopic(int id);
    Task<int> DeleteAllStudyTopics(string sd_sid);

    // Extensions
    Task<PaginationResponse<StudyDto>> PaginateStudies(PaginationRequest paginationRequest);
    Task<PaginationResponse<StudyDto>> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalStudies();
}
