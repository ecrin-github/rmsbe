using System;

namespace MdmService.DTO.Object
{
    public class ObjectDateDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? date_type_id { get; set; }
        
        public bool? date_is_range { get; set; }
        
        public string? date_as_string { get; set; }
        
        public int? start_year { get; set; }
        
        public int? start_month { get; set; }
        
        public int? start_day { get; set; }
        
        public int? end_year { get; set; }
        
        public int? end_month { get; set; }
        
        public int? end_day { get; set; }
        
        public string? details { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}