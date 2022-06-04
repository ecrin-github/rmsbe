using System;

namespace MdmService.DTO.Object
{
    public class ObjectDescriptionDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? description_type_id { get; set; }
        
        public string? label { get; set; }
        
        public string? description_text { get; set; }
        
        public string? lang_code { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}