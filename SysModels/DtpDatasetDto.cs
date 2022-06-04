using System;

namespace RmsService.DTO
{
    public class DtpDatasetDto
    {
        public int id { get; set; }
        
        public string? object_id { get; set; }
        
        public int? legal_status_id { get; set; }
        
        public string? legal_status_text { get; set; }
        
        public string? legal_status_path { get; set; }
        
        public int? desc_md_check_status_id { get; set; }
        
        public DateTime? desc_md_check_date { get; set; }
        
        public int? desc_md_check_by { get; set; }
        
        public int? deident_check_status_id { get; set; }
        
        public DateTime? deident_check_date { get; set; }
        
        public int? deident_check_by { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}