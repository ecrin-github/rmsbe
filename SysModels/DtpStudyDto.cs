using System;

namespace RmsService.DTO
{
    public class DtpStudyDto
    {
        public int id { get; set; }
        
        public int dtp_id { get; set; }
        
        public string? study_id { get; set; }
        
        public int? md_check_status_id { get; set; }
        
        public DateTime? md_check_date { get; set; }
        
        public int? md_check_by { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}