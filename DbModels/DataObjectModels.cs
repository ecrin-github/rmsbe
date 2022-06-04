using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
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
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.object_identifiers")]
public class object_identifierInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public string? identifier_value { get; set; }
    public int? identifier_type_id { get; set; }
    public int? identifier_org_id { get; set; }
    public string? identifier_org { get; set; }
    public string? identifier_org_ror_id { get; set; }
    public string? identifier_date { get; set; }
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.object_relationships")]
public class ObjectRelationshipInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public int? relationship_type_id { get; set; }
    public string? target_sd_oid { get; set; }
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.object_rights")]
public class ObjectRightInDb
{
    public int id { get; set; }
    public string? sd_oid { get; set; }
    public string? rights_name { get; set; }
    public string? rights_uri { get; set; }
    public string? comments { get; set; }
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
}
