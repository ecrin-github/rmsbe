using System;

namespace MdmService.DTO.Object
{
    public class ObjectInstanceDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? instance_type_id { get; set; }
        
        public int? repository_org_id { get; set; }
        
        public string? repository_org { get; set; }
        
        public string? url { get; set; }
        
        public bool? url_accessible { get; set; }
        
        public DateTime? url_last_checked { get; set; }
        
        public int? resource_type_id { get; set; }
        
        public string? resource_size { get; set; }
        
        public string? resource_size_units { get; set; }
        
        public string? resource_comments { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}