using System;

namespace MdmService.DTO.Object
{
    public class ObjectTopicDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? topic_type_id { get; set; }
        
        public bool? mesh_coded { get; set; }
        
        public string? mesh_code { get; set; }
        
        public string? mesh_value { get; set; }

        public int? original_ct_id { get; set; }
        
        public string? original_ct_code { get; set; }
        
        public string? original_value { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}