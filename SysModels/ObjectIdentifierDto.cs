using System;

namespace MdmService.DTO.Object
{
    public class object_identifierDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public string? identifier_value { get; set; }
        
        public int? identifier_type_id { get; set; }
        
        public int? identifier_org_id { get; set; }
        
        public string? identifier_org { get; set; }

        public string? identifier_org_ror_id { get; set; }
        
        public string? identifier_date { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}