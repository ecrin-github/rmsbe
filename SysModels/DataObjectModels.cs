using rmsbe.DbModels;
namespace rmsbe.SysModels;

public class DataObjectDto
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public string? SdSid { get; set; }
    public string? DisplayTitle { get; set; }
    public string? Version { get; set; }
    public string? Doi { get; set; }
    public int? DoiStatusId { get; set; }
    public int? PublicationYear { get; set; }
    public int? ObjectClassId { get; set; }
    public int? ObjectTypeId { get; set; }
    public int? ManagingOrgId { get; set; }
    public string? ManagingOrg { get; set; }
    public string? ManagingOrgRorId { get; set; }
    public string? LangCode { get; set; }
    public int? AccessTypeId { get; set; }
    public string? AccessDetails { get; set; }
    public string? AccessDetailsUrl { get; set; }
    public DateOnly? UrlLastChecked { get; set; } 
    public int? EoscCategory { get; set; }
    public bool? AddStudyContribs { get; set; }
    public bool? AddStudyTopics { get; set; }
    
    public List<ObjectContributor>? ObjectContributors { get; set; }
    public List<ObjectDataset>?ObjectDatasets { get; set; }
    public List<ObjectDate>? ObjectDates { get; set; }
    public List<ObjectDescription>? ObjectDescriptions { get; set; }
    public List<ObjectIdentifier>? ObjectIdentifiers { get; set; }
    public List<ObjectInstance>? ObjectInstances { get; set; }
    public List<ObjectRelationship>? ObjectRelationships { get; set; }
    public List<ObjectRight>? ObjectRights { get; set; }
    public List<ObjectTitle>? ObjectTitles { get; set; }
    public List<ObjectTopic>? ObjectTopics { get; set; }
}

public class DataObjectData
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public string? SdSid { get; set; }
    public string? DisplayTitle { get; set; }
    public string? Version { get; set; }
    public string? Doi { get; set; }
    public int? DoiStatusId { get; set; }
    public int? PublicationYear { get; set; }
    public int? ObjectClassId { get; set; }
    public int? ObjectTypeId { get; set; }
    public int? ManagingOrgId { get; set; }
    public string? ManagingOrg { get; set; }
    public string? ManagingOrgRorId { get; set; }
    public string? LangCode { get; set; }
    public int? AccessTypeId { get; set; }
    public string? AccessDetails { get; set; }
    public string? AccessDetailsUrl { get; set; }
    public DateOnly? UrlLastChecked { get; set; } 
    public int? EoscCategory { get; set; }
    public bool? AddStudyContribs { get; set; }
    public bool? AddStudyTopics { get; set; }
    
    public DataObjectData() { }

    public DataObjectData(DataObjectInDb d)
    {   Id = d.id;
        SdOid = d.sd_oid;
        SdSid = d.sd_sid;
        DisplayTitle = d.display_title;
        Version = d.version;
        Doi = d.doi;
        DoiStatusId = d.doi_status_id;
        PublicationYear = d.publication_year;
        ObjectClassId = d.object_class_id;
        ObjectTypeId = d.object_type_id;
        ManagingOrgId = d.managing_org_id;
        ManagingOrg = d.managing_org;
        ManagingOrgRorId = d.managing_org_ror_id;
        LangCode = d.lang_code;
        AccessTypeId = d.access_type_id;
        AccessDetails = d.access_details;
        AccessDetailsUrl = d.access_details_url;
        UrlLastChecked = d.url_last_checked;
        EoscCategory = d.eosc_category;
        AddStudyContribs = d.add_study_contribs;
        AddStudyTopics = d.add_study_topics;
    }
}

public class ObjectDataset
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? RecordKeysTypeId { get; set; }
    public string? RecordKeysDetails { get; set; }
    public int? DeidentTypeId { get; set; }
    public bool? DeidentDirect { get; set; }
    public bool? DeidentHipaa { get; set; }
    public bool? DeidentDates { get; set; }
    public bool? DeidentNonarr { get; set; }
    public bool? DeidentKanon { get; set; }
    public string? DeidentDetails { get; set; }
    public int? ConsentTypeId { get; set; }
    public bool? ConsentNoncommercial { get; set; }
    public bool? ConsentGeogRestrict { get; set; }
    public bool? ConsentResearchType { get; set; }
    public bool? ConsentGeneticOnly { get; set; }
    public bool? ConsentNoMethods { get; set; }
    public string? ConsentDetails { get; set; }
    
    public ObjectDataset() { }

    public ObjectDataset(ObjectDatasetInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        RecordKeysTypeId = d.record_keys_type_id;
        RecordKeysDetails = d.record_keys_details;
        DeidentTypeId = d.deident_type_id;
        DeidentDirect = d.deident_direct;
        DeidentHipaa = d.deident_hipaa;
        DeidentDates = d.deident_dates;
        DeidentNonarr = d.deident_nonarr;
        DeidentKanon = d.deident_kanon;
        DeidentDetails = d.deident_details;
        ConsentTypeId = d.consent_type_id;
        ConsentNoncommercial = d.consent_noncommercial;
        ConsentGeogRestrict = d.consent_geog_restrict;
        ConsentResearchType = d.consent_research_type;
        ConsentGeneticOnly = d.consent_genetic_only;
        ConsentNoMethods = d.consent_no_methods;
        ConsentDetails = d.consent_details;
    }
}

public class ObjectTitle
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? TitleTypeId { get; set; }
    public string? TitleText { get; set; }
    public string? LangCode { get; set; }
    public int LangUsageId { get; set; }
    public bool? IsDefault { get; set; }
    public string? Comments { get; set; }
    
    public ObjectTitle() { }

    public ObjectTitle(ObjectTitleInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        TitleTypeId = d.title_type_id;
        TitleText = d.title_text;
        LangCode = d.lang_code;
        LangUsageId = d.lang_usage_id;
        IsDefault = d.is_default;
        Comments = d.comments;
    }
}

public class ObjectInstance
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? InstanceTypeId { get; set; }
    public int? RepositoryOrgId { get; set; }
    public string? RepositoryOrg { get; set; }
    public string? Url { get; set; }
    public bool? UrlAccessible { get; set; }
    public DateTime? UrlLastChecked { get; set; }
    public int? ResourceTypeId { get; set; }
    public string? ResourceSize { get; set; }
    public string? ResourceSizeUnits { get; set; }
    public string? ResourceComments { get; set; }
    
    public ObjectInstance() { }

    public ObjectInstance(ObjectInstanceInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        InstanceTypeId = d.instance_type_id;
        RepositoryOrgId = d.repository_org_id;
        RepositoryOrg = d.repository_org;
        Url = d.url;
        UrlAccessible = d.url_accessible;
        UrlLastChecked = d.url_last_checked;
        ResourceTypeId = d.resource_type_id;
        ResourceSize = d.resource_size;
        ResourceSizeUnits = d.resource_size_units;
        ResourceComments = d.resource_comments;
    }
}

public class ObjectDate
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? DateTypeId { get; set; }
    public bool? DateIsRange { get; set; }
    public string? DateAsString { get; set; }
    public int? StartYear { get; set; }
    public int? StartMonth { get; set; }
    public int? StartDay { get; set; }
    public int? EndYear { get; set; }
    public int? EndMonth { get; set; }
    public int? EndDay { get; set; }
    public string? Details { get; set; }
    
    public ObjectDate() { }

    public ObjectDate(ObjectDateInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        DateTypeId = d.date_type_id;
        DateIsRange = d.date_is_range;
        DateAsString = d.date_as_string;
        StartYear = d.start_year;
        StartMonth = d.start_month;
        StartDay = d.start_day;
        EndYear = d.end_year;
        EndMonth = d.end_month;
        EndDay = d.end_day;
        Details = d.details;
    }
}

public class ObjectDescription
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? DescriptionTypeId { get; set; }
    public string? Label { get; set; }
    public string? DescriptionText { get; set; }
    public string? LangCode { get; set; }
    
    public ObjectDescription() { }

    public ObjectDescription(ObjectDescriptionInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        DescriptionTypeId = d.description_type_id;
        Label = d.label;
        DescriptionText = d.description_text;
        LangCode = d.lang_code;
    }
}

public class ObjectContributor
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? ContribTypeId { get; set; }
    public bool? IsIndividual { get; set; }
    public int? PersonId { get; set; }
    public string? PersonGivenName { get; set; }
    public string? PersonFamilyName { get; set; }
    public string? PersonFullName { get; set; }
    public string? OrcidId { get; set; }
    public string? PersonAffiliation { get; set; }
    public int? OrganisationId { get; set; }
    public string? OrganisationName { get; set; }
    public string? OrganisationRorId { get; set; }
    
    public ObjectContributor() { }

    public ObjectContributor(ObjectContributorInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        ContribTypeId = d.contrib_type_id;
        IsIndividual = d.is_individual;
        PersonId = d.person_id;
        PersonGivenName = d.person_given_name;
        PersonFamilyName = d.person_family_name;
        PersonFullName = d.person_full_name;
        OrcidId = d.orcid_id;
        PersonAffiliation = d.person_affiliation;
        OrganisationId = d.organisation_id;
        OrganisationName = d.organisation_name;
        OrganisationRorId = d.organisation_ror_id;
    }
}

public class ObjectTopic
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? TopicTypeId { get; set; }
    public bool? MeshCoded { get; set; }
    public string? MeshCode { get; set; }
    public string? MeshValue { get; set; }
    public int? OriginalCtId { get; set; }
    public string? OriginalCtCode { get; set; }
    public string? OriginalValue { get; set; }
    
    public ObjectTopic() { }

    public ObjectTopic(ObjectTopicInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        TopicTypeId = d.topic_type_id;
        MeshCoded = d.mesh_coded;
        MeshCode = d.mesh_code;
        MeshValue = d.mesh_value;
        OriginalCtId = d.original_ct_id;
        OriginalCtCode = d.original_ct_code;
        OriginalValue = d.original_value;
    }
}

public class ObjectIdentifier
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public string? IdentifierValue { get; set; }
    public int? IdentifierTypeId { get; set; }
    public int? IdentifierOrgId { get; set; }
    public string? IdentifierOrg { get; set; }
    public string? IdentifierOrgRorId { get; set; }
    public string? IdentifierDate { get; set; }
    
    public ObjectIdentifier() { }

    public ObjectIdentifier(ObjectIdentifierInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        IdentifierValue = d.identifier_value;
        IdentifierTypeId = d.identifier_type_id;
        IdentifierOrgId = d.identifier_org_id;
        IdentifierOrg = d.identifier_org;
        IdentifierOrgRorId = d.identifier_org_ror_id;
        IdentifierDate = d.identifier_date;
    }
}

public class ObjectRelationship
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public int? RelationshipTypeId { get; set; }
    public string? TargetSdOid { get; set; }
    
    public ObjectRelationship() { }

    public ObjectRelationship(ObjectRelationshipInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        RelationshipTypeId = d.relationship_type_id;
        TargetSdOid = d.target_sd_oid;
    }
}

public class ObjectRight
{
    public int Id { get; set; }
    public string? SdOid { get; set; }
    public string? RightsName { get; set; }
    public string? RightsUri { get; set; }
    public string? Comments { get; set; }
    
    public ObjectRight() { }

    public ObjectRight(ObjectRightInDb d)
    {
        Id = d.id;
        SdOid = d.sd_oid;
        RightsName = d.rights_name;
        RightsUri = d.rights_uri;
        Comments = d.comments;
    }
}
