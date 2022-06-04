using System;

namespace RmsService.DTO
{
    public class ProcessNoteDto
    {
        public int id { get; set; }
        
        public int? ProcessType { get; set; }
        
        public int? ProcessId { get; set; }
        
        public string? Text { get; set; }
        
        public int? Author { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}