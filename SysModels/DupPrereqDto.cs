using System;

namespace RmsService.DTO
{
    public class DupPrereqDto
    {
        public int id { get; set; }
        
        public int DupId { get; set; }
        
        public string? object_id { get; set; }
        
        public int? pre_requisite_id { get; set; }
        
        public DateTime? prerequisite_met { get; set; }
        
        public string? met_notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}