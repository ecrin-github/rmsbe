using System;

namespace MdmService.DTO.Object
{
    public class DataObjectDataDto
    {
        public int? Id { get; set; }
        
        public string? sd_oid { get; set; }
        
        public string? sd_sid { get; set; }
        
        public string? display_title { get; set; }
        
        public string? Version { get; set; }
        
        public string? doi { get; set; }
        
        public int? doistatus_id { get; set; }
        
        public int? PublicationYear { get; set; }
        
        public int? ObjectClassId { get; set; }
        
        public int? ObjectTypeId { get; set; }
        
        public int? Managingorg_id { get; set; }
        
        public string? ManagingOrg { get; set; }
        
        public string? ManagingOrgRorId { get; set; }
        
        public string? lang_code { get; set; }
        
        public int? access_type_id { get; set; }
        
        public string? access_details { get; set; }
        
        public string? access_detailsurl { get; set; }
        
        public DateTime? url_last_checked { get; set; } 
        
        public int? EoscCategory { get; set; }
        
        public bool? AddStudyContribs { get; set; }
        
        public bool? AddStudyTopics { get; set; }
        
        public DateTime? created_on { get; set; }
    }
}