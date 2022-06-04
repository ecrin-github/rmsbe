using System.Collections.Generic;

namespace MdmService.DTO.Study
{
    public class StudyDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public string? Mdrsd_sid { get; set; }
        
        public int? MdrSourceId { get; set; }
        
        public string? display_title { get; set; }
        
        public string? title_lang_code { get; set; }
        
        public string? brief_description { get; set; }
        
        public string? data_sharing_statement { get; set; }
        
        public int? study_start_year { get; set; }
        
        public int? study_start_month { get; set; }
        
        public int? study_type_id { get; set; }
        
        public int? study_status_id { get; set; }
        
        public string? study_enrolment { get; set; }
        
        public int? study_gender_elig_id { get; set; }
        
        public int? min_age { get; set; }
        
        public int? min_age_units_id { get; set; }
        
        public int? max_age { get; set; }
        
        public int? max_age_units_id { get; set; }
        
        public string? created_on { get; set; }
        
        public ICollection<StudyContributorDto>? StudyContributors { get; set; }
        
        public ICollection<StudyFeatureDto>? StudyFeatures { get; set; }
        
        public ICollection<study_identifierDto>? study_identifiers { get; set; }
        
        public ICollection<StudyReferenceDto>? StudyReferences { get; set; }
        
        public ICollection<StudyRelationshipDto>? StudyRelationships { get; set; }
        
        public ICollection<StudyTitleDto>? StudyTitles { get; set; }
        
        public ICollection<StudyTopicDto>? StudyTopics { get; set; }
    }
}