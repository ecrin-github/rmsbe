using System;
using System.Collections.Generic;

namespace MdmService.DTO.Object
{
    public class DataObjectDto
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
        
        public ICollection<ObjectContributorDto>? ObjectContributors { get; set; }
        
        public ObjectDatasetDto? ObjectDatasets { get; set; }
        
        public ICollection<ObjectDateDto>? ObjectDates { get; set; }
        
        public ICollection<ObjectDescriptionDto>? ObjectDescriptions { get; set; }
        
        public ICollection<object_identifierDto>? object_identifiers { get; set; }
        
        public ICollection<ObjectInstanceDto>? ObjectInstances { get; set; }
        
        public ICollection<ObjectRelationshipDto>? ObjectRelationships { get; set; }
        
        public ICollection<ObjectRightDto>? ObjectRights { get; set; }
        
        public ICollection<ObjectTitleDto>? ObjectTitles { get; set; }
        
        public ICollection<ObjectTopicDto>? ObjectTopics { get; set; }
    }
}