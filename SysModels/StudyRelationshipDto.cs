using System;

namespace MdmService.DTO.Study
{
    public class StudyRelationshipDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public int? relationship_type_id { get; set; }
        
        public string? target_sd_sid { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}