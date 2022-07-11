using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

public class FullObjectInDb
{
    public DataObjectInDb? core_object { get; set; }
    public List<ObjectContributorInDb>? object_contributors_in_db { get; set; }
    public List<ObjectDatasetInDb>? object_datasets_in_db { get; set; }
    public List<ObjectDateInDb>? object_dates_in_db { get; set; }
    public List<ObjectDescriptionInDb>? object_descriptions_in_db { get; set; }
    public List<ObjectIdentifierInDb>? object_identifiers_in_db { get; set; }
    public List<ObjectInstanceInDb>? object_instances_in_db { get; set; }
    public List<ObjectRelationshipInDb>? object_relationships_in_db { get; set; }
    public List<ObjectRightInDb>? object_rights_in_db { get; set; }
    public List<ObjectTitleInDb>? object_titles_in_db{ get; set; }
    public List<ObjectTopicInDb>? object_topics_in_db{ get; set; }

    public FullObjectInDb() { }
    
    public FullObjectInDb(DataObjectInDb? coreObject, List<ObjectContributorInDb>? objectContributorsInDb,
        List<ObjectDatasetInDb>? objectDatasetsInDb, List<ObjectDateInDb>? objectDatesInDb,
        List<ObjectDescriptionInDb>? objectDescriptionsInDb, List<ObjectIdentifierInDb>? objectIdentifiersInDb,
        List<ObjectInstanceInDb>? objectInstancesInDb, List<ObjectRelationshipInDb>? objectRelationshipsInDb, 
        List<ObjectRightInDb>? objectRightsInDb, List<ObjectTitleInDb>? objectTitlesInDb, 
        List<ObjectTopicInDb>? objectTopicsInDb)
    {
        core_object = coreObject;
        object_contributors_in_db = objectContributorsInDb;
        object_datasets_in_db = objectDatasetsInDb;
        object_dates_in_db = objectDatesInDb;
        object_descriptions_in_db = objectDescriptionsInDb;
        object_identifiers_in_db = objectIdentifiersInDb;
        object_instances_in_db = objectInstancesInDb;
        object_relationships_in_db = objectRelationshipsInDb;
        object_rights_in_db = objectRightsInDb;
        object_titles_in_db = objectTitlesInDb;
        object_topics_in_db = objectTopicsInDb;
    }
}


[Table("mdr.data_objects")]
public class  DataObjectInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public string? sd_sid { get; set; }
    public string? display_title { get; set; }
    public string? version { get; set; }
    public string? doi { get; set; }
    public int? doi_status_id { get; set; }
    public int? publication_year { get; set; }
    public int? object_class_id { get; set; }
    public int? object_type_id { get; set; }
    public int? managing_org_id { get; set; }
    public string? managing_org { get; set; }
    public string? managing_org_ror_id { get; set; }
    public string? lang_code { get; set; }
    public int? access_type_id { get; set; }
    public string? access_details { get; set; }
    public string? access_details_url { get; set; }
    public DateOnly? url_last_checked { get; set; } 
    public int? eosc_category { get; set; }
    public bool? add_study_contribs { get; set; }
    public bool? add_study_topics { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public DataObjectInDb() { }

    public DataObjectInDb(DataObjectData d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        sd_sid = d.SdSid;
        display_title = d.DisplayTitle;
        version = d.Version;
        doi = d.Doi;
        doi_status_id = d.DoiStatusId;
        publication_year = d.PublicationYear;
        object_class_id = d.ObjectClassId;
        object_type_id = d.ObjectTypeId;
        managing_org_id = d.ManagingOrgId;
        managing_org = d.ManagingOrg;
        managing_org_ror_id = d.ManagingOrgRorId;
        lang_code = d.LangCode;
        access_type_id = d.AccessTypeId;
        access_details = d.AccessDetails;
        access_details_url = d.AccessDetailsUrl;
        url_last_checked = d.UrlLastChecked;
        eosc_category = d.EoscCategory;
        add_study_contribs = d.AddStudyContribs;
        add_study_topics = d.AddStudyTopics;
    }
    
    public DataObjectInDb(DataObjectInMdr d, string sdSid, string sdOid)
    {
        sd_oid = sdOid;
        sd_sid = sdSid;
        display_title = d.display_title;
        version = d.version;
        doi = d.doi;
        doi_status_id = d.doi_status_id;
        publication_year = d.publication_year;
        object_class_id = d.object_class_id;
        object_type_id = d.object_type_id;
        managing_org_id = d.managing_org_id;
        managing_org = d.managing_org;
        managing_org_ror_id = d.managing_org_ror_id;
        lang_code = d.lang_code;
        access_type_id = d.access_type_id;
        access_details = d.access_details;
        access_details_url = d.access_details_url;
        url_last_checked = d.url_last_checked;
        eosc_category = d.eosc_category;
        add_study_contribs = d.add_study_contribs;
        add_study_topics = d.add_study_topics;
    }
}


public class DataObjectInMdr
{
    public int id { get; set; }
    public string? display_title { get; set; }
    public string? version { get; set; }
    public string? doi { get; set; }
    public int? doi_status_id { get; set; }
    public int? publication_year { get; set; }
    public int? object_class_id { get; set; }
    public int? object_type_id { get; set; }
    public int? managing_org_id { get; set; }
    public string? managing_org { get; set; }
    public string? managing_org_ror_id { get; set; }
    public string? lang_code { get; set; }
    public int? access_type_id { get; set; }
    public string? access_details { get; set; }
    public string? access_details_url { get; set; }
    public DateOnly? url_last_checked { get; set; }
    public int? eosc_category { get; set; }
    public bool? add_study_contribs { get; set; }
    public bool? add_study_topics { get; set; }
    public string? provenance_string { get; set; }
}


public class DataObjectEntryInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public string? sd_sid { get; set; }
    public string? display_title { get; set; }
    
    public DataObjectEntryInDb() { }

    public DataObjectEntryInDb(DataObjectEntry d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        sd_sid = d.SdSid;
        display_title = d.DisplayTitle;
    }
}


[Table("mdr.object_datasets")]
public class ObjectDatasetInDb
{
    public int id { get; set; }
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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectDatasetInDb() { }

    public ObjectDatasetInDb(ObjectDataset d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        record_keys_type_id = d.RecordKeysTypeId;
        record_keys_details = d.RecordKeysDetails;
        deident_type_id = d.DeidentTypeId;
        deident_direct = d.DeidentDirect;
        deident_hipaa = d.DeidentHipaa;
        deident_dates = d.DeidentDates;
        deident_nonarr = d.DeidentNonarr;
        deident_kanon = d.DeidentKanon;
        deident_details = d.DeidentDetails;
        consent_type_id = d.ConsentTypeId;
        consent_noncommercial = d.ConsentNoncommercial;
        consent_geog_restrict = d.ConsentGeogRestrict;
        consent_research_type = d.ConsentResearchType;
        consent_genetic_only = d.ConsentGeneticOnly;
        consent_no_methods = d.ConsentNoMethods;
        consent_details = d.ConsentDetails;
    }
    
    public ObjectDatasetInDb(ObjectDatasetInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        record_keys_type_id = d.record_keys_type_id;
        record_keys_details = d.record_keys_details;
        deident_type_id = d.deident_type_id;
        deident_direct = d.deident_direct;
        deident_hipaa = d.deident_hipaa;
        deident_dates = d.deident_dates;
        deident_nonarr = d.deident_nonarr;
        deident_kanon = d.deident_kanon;
        deident_details = d.deident_details;
        consent_type_id = d.consent_type_id;
        consent_noncommercial = d.consent_noncommercial;
        consent_geog_restrict = d.consent_geog_restrict;
        consent_research_type = d.consent_research_type;
        consent_genetic_only = d.consent_genetic_only;
        consent_no_methods = d.consent_no_methods;
        consent_details = d.consent_details;
    }
}


public class ObjectDatasetInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
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
}


[Table("mdr.object_titles")]
public class ObjectTitleInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? title_type_id { get; set; }
    public string? title_text { get; set; }
    public string? lang_code { get; set; }
    public int lang_usage_id { get; set; }
    public bool? is_default { get; set; }
    public string? comments { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectTitleInDb() { }

    public ObjectTitleInDb(ObjectTitle d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        title_type_id = d.TitleTypeId;
        title_text = d.TitleText;
        lang_code = d.LangCode;
        lang_usage_id = d.LangUsageId;
        is_default = d.IsDefault;
        comments = d.Comments;
    }
    
    public ObjectTitleInDb(ObjectTitleInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        title_type_id = d.title_type_id;
        title_text = d.title_text;
        lang_code = d.lang_code;
        lang_usage_id = d.lang_usage_id;
        is_default = d.is_default;
        comments = d.comments;
    }
}

public class ObjectTitleInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? title_type_id { get; set; }
    public string? title_text { get; set; }
    public string? lang_code { get; set; }
    public int lang_usage_id { get; set; }
    public bool? is_default { get; set; }
    public string? comments { get; set; }
}


[Table("mdr.object_instances")]
public class ObjectInstanceInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? instance_type_id { get; set; }
    public int? repository_org_id { get; set; }
    public string? repository_org { get; set; }
    public string? url { get; set; }
    public bool? url_accessible { get; set; }
    public DateTime? url_last_checked { get; set; }
    public int? resource_type_id { get; set; }
    public string? resource_size { get; set; }
    public string? resource_size_units { get; set; }
    public string? resource_comments { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectInstanceInDb() { }

    public ObjectInstanceInDb(ObjectInstance d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        instance_type_id = d.InstanceTypeId;
        repository_org_id = d.RepositoryOrgId;
        repository_org = d.RepositoryOrg;
        url = d.Url;
        url_accessible = d.UrlAccessible;
        url_last_checked = d.UrlLastChecked;
        resource_type_id = d.ResourceTypeId;
        resource_size = d.ResourceSize;
        resource_size_units = d.ResourceSizeUnits;
        resource_comments = d.ResourceComments;
    }
    
    public ObjectInstanceInDb(ObjectInstanceInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        instance_type_id = d.instance_type_id;
        repository_org_id = d.repository_org_id;
        repository_org = d.repository_org;
        url = d.url;
        url_accessible = d.url_accessible;
        url_last_checked = d.url_last_checked;
        resource_type_id = d.resource_type_id;
        resource_size = d.resource_size;
        resource_size_units = d.resource_size_units;
        resource_comments = d.resource_comments;
    }
}

public class ObjectInstanceInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? instance_type_id { get; set; }
    public int? repository_org_id { get; set; }
    public string? repository_org { get; set; }
    public string? url { get; set; }
    public bool? url_accessible { get; set; }
    public DateTime? url_last_checked { get; set; }
    public int? resource_type_id { get; set; }
    public string? resource_size { get; set; }
    public string? resource_size_units { get; set; }
    public string? resource_comments { get; set; }
}

[Table("mdr.object_dates")]
public class ObjectDateInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? date_type_id { get; set; }
    public bool? date_is_range { get; set; }
    public string? date_as_string { get; set; }
    public int? start_year { get; set; }
    public int? start_month { get; set; }
    public int? start_day { get; set; }
    public int? end_year { get; set; }
    public int? end_month { get; set; }
    public int? end_day { get; set; }
    public string? details { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectDateInDb() { }

    public ObjectDateInDb(ObjectDate d)
    {
       id = d.Id;
        sd_oid = d.SdOid;
        date_type_id = d.DateTypeId;
        date_is_range = d.DateIsRange;
        date_as_string = d.DateAsString;
        start_year = d.StartYear;
        start_month = d.StartMonth;
        start_day = d.StartDay;
        end_year = d.EndYear;
        end_month = d.EndMonth;
        end_day = d.EndDay;
       details = d.Details;
    }
    
    public ObjectDateInDb(ObjectDateInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        date_type_id = d.date_type_id;
        date_is_range = d.date_is_range;
        date_as_string = d.date_as_string;
        start_year = d.start_year;
        start_month = d.start_month;
        start_day = d.start_day;
        end_year = d.end_year;
        end_month = d.end_month;
        end_day = d.end_day;
        details = d.details;
    }
}

public class ObjectDateInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? date_type_id { get; set; }
    public bool? date_is_range { get; set; }
    public string? date_as_string { get; set; }
    public int? start_year { get; set; }
    public int? start_month { get; set; }
    public int? start_day { get; set; }
    public int? end_year { get; set; }
    public int? end_month { get; set; }
    public int? end_day { get; set; }
    public string? details { get; set; }
}


[Table("mdr.object_descriptions")]
public class ObjectDescriptionInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? description_type_id { get; set; }
    public string? label { get; set; }
    public string? description_text { get; set; }
    public string? lang_code { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectDescriptionInDb() { }

    public ObjectDescriptionInDb(ObjectDescription d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        description_type_id = d.DescriptionTypeId;
        label = d.Label;
        description_text = d.DescriptionText;
        lang_code = d.LangCode;
    }
    
    public ObjectDescriptionInDb(ObjectDescriptionInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        description_type_id = d.description_type_id;
        label = d.label;
        description_text = d.description_text;
        lang_code = d.lang_code;
    }
}

public class ObjectDescriptionInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? description_type_id { get; set; }
    public string? label { get; set; }
    public string? description_text { get; set; }
    public string? lang_code { get; set; }
}


[Table("mdr.object_contributors")]
public class ObjectContributorInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? contrib_type_id { get; set; }
    public bool? is_individual { get; set; }
    public int? person_id { get; set; }
    public string? person_given_name { get; set; }
    public string? person_family_name { get; set; }
    public string? person_full_name { get; set; }
    public string? orcid_id { get; set; }
    public string? person_affiliation { get; set; }
    public int? organisation_id { get; set; }
    public string? organisation_name { get; set; }
    public string? organisation_ror_id { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectContributorInDb() { }

    public ObjectContributorInDb(ObjectContributor d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        contrib_type_id = d.ContribTypeId;
        is_individual = d.IsIndividual;
        person_id = d.PersonId;
        person_given_name = d.PersonGivenName;
        person_family_name = d.PersonFamilyName;
        person_full_name = d.PersonFullName;
        orcid_id = d.OrcidId;
        person_affiliation = d.PersonAffiliation;
        organisation_id = d.OrganisationId;
        organisation_name = d.OrganisationName;
        organisation_ror_id = d.OrganisationRorId;
    }
    
    public ObjectContributorInDb(ObjectContributorInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        contrib_type_id = d.contrib_type_id;
        is_individual = d.is_individual;
        person_id = d.person_id;
        person_given_name = d.person_given_name;
        person_family_name = d.person_family_name;
        person_full_name = d.person_full_name;
        orcid_id = d.orcid_id;
        person_affiliation = d.person_affiliation;
        organisation_id = d.organisation_id;
        organisation_name = d.organisation_name;
        organisation_ror_id = d.organisation_ror_id;
    }
}


public class ObjectContributorInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? contrib_type_id { get; set; }
    public bool? is_individual { get; set; }
    public int? person_id { get; set; }
    public string? person_given_name { get; set; }
    public string? person_family_name { get; set; }
    public string? person_full_name { get; set; }
    public string? orcid_id { get; set; }
    public string? person_affiliation { get; set; }
    public int? organisation_id { get; set; }
    public string? organisation_name { get; set; }
    public string? organisation_ror_id { get; set; }
}



[Table("mdr.object_topics")]
public class ObjectTopicInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? topic_type_id { get; set; }
    public bool? mesh_coded { get; set; }
    public string? mesh_code { get; set; }
    public string? mesh_value { get; set; }
    public int? original_ct_id { get; set; }
    public string? original_ct_code { get; set; }
    public string? original_value { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectTopicInDb() { }

    public ObjectTopicInDb(ObjectTopic d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        topic_type_id = d.TopicTypeId;
        mesh_coded = d.MeshCoded;
        mesh_code = d.MeshCode;
        mesh_value = d.MeshValue;
        original_ct_id = d.OriginalCtId;
        original_ct_code = d.OriginalCtCode;
        original_value = d.OriginalValue;
    }
    
    public ObjectTopicInDb(ObjectTopicInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        topic_type_id = d.topic_type_id;
        mesh_coded = d.mesh_coded;
        mesh_code = d.mesh_code;
        mesh_value = d.mesh_value;
        original_ct_id = d.original_ct_id;
        original_ct_code = d.original_ct_code;
        original_value = d.original_value;
    }
}

public class ObjectTopicInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? topic_type_id { get; set; }
    public bool? mesh_coded { get; set; }
    public string? mesh_code { get; set; }
    public string? mesh_value { get; set; }
    public int? original_ct_id { get; set; }
    public string? original_ct_code { get; set; }
    public string? original_value { get; set; }
}


[Table("mdr.object_identifiers")]
public class ObjectIdentifierInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public string? identifier_value { get; set; }
    public int? identifier_type_id { get; set; }
    public int? identifier_org_id { get; set; }
    public string? identifier_org { get; set; }
    public string? identifier_org_ror_id { get; set; }
    public string? identifier_date { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectIdentifierInDb() { }

    public ObjectIdentifierInDb(ObjectIdentifier d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        identifier_value = d.IdentifierValue;
        identifier_type_id = d.IdentifierTypeId;
        identifier_org_id = d.IdentifierOrgId;
        identifier_org = d.IdentifierOrg;
        identifier_org_ror_id = d.IdentifierOrgRorId;
        identifier_date = d.IdentifierDate;
    }
    
    public ObjectIdentifierInDb(ObjectIdentifierInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        identifier_value = d.identifier_value;
        identifier_type_id = d.identifier_type_id;
        identifier_org_id = d.identifier_org_id;
        identifier_org = d.identifier_org;
        identifier_org_ror_id = d.identifier_org_ror_id;
        identifier_date = d.identifier_date;
    }
}

public class ObjectIdentifierInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public string? identifier_value { get; set; }
    public int? identifier_type_id { get; set; }
    public int? identifier_org_id { get; set; }
    public string? identifier_org { get; set; }
    public string? identifier_org_ror_id { get; set; }
    public string? identifier_date { get; set; }
}


[Table("mdr.object_relationships")]
public class ObjectRelationshipInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? relationship_type_id { get; set; }
    public string? target_sd_oid { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectRelationshipInDb() { }

    public ObjectRelationshipInDb(ObjectRelationship d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        relationship_type_id = d.RelationshipTypeId;
        target_sd_oid = d.TargetSdOid;
    }
    
    public ObjectRelationshipInDb(ObjectRelationshipInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        relationship_type_id = d.relationship_type_id;
        
        // Needs sorting out
        // target_sd_oid = d.target_object_id;
    }
}

public class ObjectRelationshipInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public int? relationship_type_id { get; set; }
    public string? target_object_id { get; set; }
}


[Table("mdr.object_rights")]
public class ObjectRightInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public string? rights_name { get; set; }
    public string? rights_uri { get; set; }
    public string? comments { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public ObjectRightInDb() { }

    public ObjectRightInDb(ObjectRight d)
    {
        id = d.Id;
        sd_oid = d.SdOid;
        rights_name = d.RightsName;
        rights_uri = d.RightsUri;
        comments = d.Comments;
    }
    
    public ObjectRightInDb(ObjectRightInMdr d, string sdOid)
    {
        sd_oid = sdOid;
        rights_name = d.rights_name;
        rights_uri = d.rights_uri;
        comments = d.comments;
    }
}

public class ObjectRightInMdr
{
    public int id { get; set; }
    public int object_id { get; set; }
    public string? rights_name { get; set; }
    public string? rights_uri { get; set; }
    public string? comments { get; set; }
}