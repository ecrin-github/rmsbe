using System;

namespace RmsService.DTO
{
    public class DtpDto
    {
        public int id { get; set; }
        
        public int? org_id { get; set; }
        
        public string? display_name { get; set; }
        
        public int? status_id { get; set; }
        
        public DateTime? initial_contact_date { get; set; }
        
        public DateTime? set_up_completed { get; set; }
        
        public DateTime? md_access_granted { get; set; }
        
        public DateTime? md_complete_date { get; set; }

        public DateTime? dta_agreed_date { get; set; }
        
        public DateTime? upload_access_requested { get; set; }
        
        public DateTime? upload_access_confirmed { get; set; }
        
        public DateTime? uploads_complete { get; set; }
        
        public DateTime? qc_checks_completed { get; set; }
        
        public DateTime? md_integrated_with_mdr { get; set; }
        
        public DateTime? availability_requested { get; set; }
        
        public DateTime? availability_confirmed { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}