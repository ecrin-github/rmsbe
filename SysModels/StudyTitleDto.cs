using System;

namespace MdmService.DTO.Study
{
    public class StudyTitleDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public int? title_type_id { get; set; }
        
        public string? title_text { get; set; }
        
        public string? lang_code { get; set; }
        
        public int? lang_usage_id { get; set; }
        
        public bool? is_default { get; set; }
        
        public string? comments { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}