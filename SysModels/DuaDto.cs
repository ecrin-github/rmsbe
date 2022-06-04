using System;

namespace RmsService.DTO
{
    public class DuaDto
    {
        public int id { get; set; }
        
        public int DupId { get; set; }
        
        public int? conforms_to_default { get; set; }
        
        public string? Variations { get; set; }
        
        public bool? repo_as_proxy { get; set; }
        
        public int? repo_signatory_1 { get; set; }
        
        public int? repo_signatory_2 { get; set; }
        
        public int? provider_signatory_1 { get; set; }
        
        public int? provider_signatory_2 { get; set; }
        
        public int? requester_signatory_1 { get; set; }
        
        public int? requester_signatory_2 { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}