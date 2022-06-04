using System;

namespace MdmService.DTO.Object
{
    public class ObjectRelationshipDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? relationship_type_id { get; set; }
        
        public string? target_sd_oid { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}