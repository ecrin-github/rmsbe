using System;

namespace MdmService.DTO.Object
{
    public class ObjectDatasetDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public int? record_keys_type_id { get; set; }
        
        public string? record_keys_details { get; set; }
        
        public int? deident_type_id { get; set; }
        
        public bool? deident_direct { get; set; }
        
        public bool? deident_hipaa { get; set; }
        
        public bool? deident_dates { get; set; }
        
        public bool? deident_nonarr { get; set; }
        
        public bool? deident_kanon { get; set; }
        
        public string? deident_details { get; set; }
        
        public int? consent_type_id { get; set; }
        
        public bool? consent_noncommercial { get; set; }
        
        public bool? consent_geog_restrict { get; set; }
        
        public bool? consent_research_type { get; set; }
        
        public bool? consent_genetic_only { get; set; }
        
        public bool? consent_no_methods { get; set; }
        
        public string? consent_details { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}