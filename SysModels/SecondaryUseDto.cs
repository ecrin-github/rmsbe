using System;

namespace RmsService.DTO
{
    public class SecondaryUseDto
    {
        public int id { get; set; }
        
        public int DupId { get; set; }
        
        public string? secondary_use_type { get; set; }
        
        public string? Publication { get; set; }
        
        public string? doi { get; set; }
        
        public bool? attribution_present { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}