using System;

namespace RmsService.DTO
{
    public class DupDto
    {
        public int id { get; set; }
        
        public int org_id { get; set; }
        
        public string? display_name { get; set; }
        
        public int? status_id { get; set; }
        
        public DateTime? initial_contact_date { get; set; }
        
        public DateTime? set_up_completed { get; set; }
        
        public DateTime? prereqs_met { get; set; }
        
        public DateTime? dua_agreed_date { get; set; }
        
        public DateTime? availability_requested { get; set; }
        
        public DateTime? availability_confirmed { get; set; }
        
        public DateTime? access_confirmed { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}