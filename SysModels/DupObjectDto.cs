using System;

namespace RmsService.DTO
{
    public class DupObjectDto
    {
        public int id { get; set; }
        
        public int DupId { get; set; }
        
        public string? object_id { get; set; }
        
        public int? access_type_id { get; set; }
        
        public string? access_details { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}