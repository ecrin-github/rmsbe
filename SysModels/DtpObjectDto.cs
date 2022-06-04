using System;

namespace RmsService.DTO
{
    public class DtpObjectDto
    {
        public int id { get; set; }
        
        public int dtp_id { get; set; }
        
        public string? object_id { get; set; }
        
        public bool? is_dataset { get; set; }
        
        public int? access_type_id { get; set; }
        
        public bool? download_allowed { get; set; }
        
        public string? access_details { get; set; }
        
        public bool? requires_embargo_period { get; set; }
        
        public DateTime? embargo_end_date { get; set; }
        
        public bool? embargo_still_applies { get; set; }
        
        public int? access_check_status_id { get; set; }
        
        public DateTime? access_check_date { get; set; }
        
        public string? access_check_by { get; set; }
        
        public int? md_check_status_id { get; set; }
        
        public DateTime? md_check_date { get; set; }
        
        public string? md_check_by { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}