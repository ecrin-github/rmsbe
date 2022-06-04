using System;

namespace MdmService.DTO.Study
{
    public class study_identifierDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public string? identifier_value { get; set; }
        
        public int? identifier_type_id { get; set; }
        
        public int? identifier_org_id { get; set; }
        
        public string? identifier_org { get; set; }

        public string? identifier_org_ror_id { get; set; }
        
        public string? identifier_date { get; set; }
        
        public string? identifier_link { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}