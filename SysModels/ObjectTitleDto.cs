using System;

namespace MdmService.DTO.Object
{
    public class ObjectTitleDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? title_type_id { get; set; }
        
        public string? title_text { get; set; }
        
        public string? lang_code { get; set; }
        
        public int lang_usage_id { get; set; }
        
        public bool? is_default { get; set; }
        
        public string? comments { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}