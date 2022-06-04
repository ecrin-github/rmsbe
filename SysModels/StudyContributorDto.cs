using System;

namespace MdmService.DTO.Study
{
    public class StudyContributorDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public int? contrib_type_id { get; set; }
        
        public bool? is_individual { get; set; }
        
        public int? person_id { get; set; }
        
        public string? person_given_name { get; set; }
        
        public string? person_family_name { get; set; }
        
        public string? person_full_name { get; set; }
        
        public string? orcid_id { get; set; }
        
        public string? person_affiliation { get; set; }
        
        public int? organisation_id { get; set; }
        
        public string? organisation_name { get; set; }
        
        public string? organisation_ror_id { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}