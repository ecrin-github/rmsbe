using System;

namespace RmsService.DTO
{
    public class AccessPrereqDto
    {
        public int id { get; set; }
        
        public int? object_id { get; set; }
        
        public int? pre_requisite_id { get; set; }
        
        public string? pre_requisite_notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}