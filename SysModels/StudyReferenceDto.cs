using System;

namespace MdmService.DTO.Study
{
    public class StudyReferenceDto
    {
        public int? Id { get; set; }
        
        public string? sd_sid { get; set; }
        
        public string? pmid { get; set; }
        
        public string? citation { get; set; }
        
        public string? doi { get; set; }
        
        public string? comments { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}