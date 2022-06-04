using System;

namespace MdmService.DTO.Study
{
    public class StudyFeatureDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public int? feature_type_id { get; set; }

        public int? feature_value_id { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}