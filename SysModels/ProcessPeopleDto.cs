using System;

namespace RmsService.DTO
{
    public class ProcessPeopleDto
    {
        public int id { get; set; }
        
        public int? ProcessType { get; set; }
        
        public int? ProcessId { get; set; }
        
        public int? person_id { get; set; }
        
        public bool? IsAUser { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}