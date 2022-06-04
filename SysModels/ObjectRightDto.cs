using System;

namespace MdmService.DTO.Object
{
    public class ObjectRightDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public string? rights_name { get; set; }
        
        public string? rights_uri { get; set; }
        
        public string? comments { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}