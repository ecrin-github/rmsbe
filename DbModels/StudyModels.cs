using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

[Table("mdr.studies")]
public class StudyInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public string? mdr_sd_sid { get; set; }
    public int? mdr_source_id { get; set; }
    public string? display_title { get; set; }
    public string? title_lang_code { get; set; }
    public string? brief_description { get; set; }
    public string? data_sharing_statement { get; set; }
    public int? study_start_year { get; set; }
    public int? study_start_month { get; set; }
    public int? study_type_id { get; set; }
    public int? study_status_id { get; set; }
    public string? study_enrolment { get; set; }
    public int? study_gender_elig_id { get; set; }
    public int? min_age { get; set; }
    public int? min_age_units_id { get; set; }
    public int? max_age { get; set; }
    public int? max_age_units_id { get; set; }
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;} 
}


[Table("mdr.study_identifiers")]
public class study_identifierInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public string? identifier_value { get; set; }
    public int? identifier_type_id { get; set; }
    public int? identifier_org_id { get; set; }
    public string? identifier_org { get; set; }
    public string? identifier_org_ror_id { get; set; }
    public string? identifier_date { get; set; }
    public string? identifier_link { get; set; }
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.study_titles")]
public class StudyTitleInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public int? title_type_id { get; set; }
    public string? title_text { get; set; }
    public string? lang_code { get; set; }
    public int? lang_usage_id { get; set; }
    public bool? is_default { get; set; }
    public string? comments { get; set; }
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.study_contributors")]
public class StudyContributorInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
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


[Table("mdr.study_features")]
public class StudyFeatureInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public int? feature_type_id { get; set; }
    public int? feature_value_id { get; set; }
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.study_topics")]
public class StudyTopicInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public int? topic_type_id { get; set; }
    public bool? mesh_coded { get; set; }
    public string? mesh_code { get; set; }
    public string? mesh_value { get; set; }
    public int? original_ct_id { get; set; }
    public string? original_ct_code { get; set; }
    public string? original_value { get; set; }
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
}


[Table("mdr.study_relationships")]
public class StudyRelationshipInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public int? relationship_type_id { get; set; }
    public string? target_sd_sid { get; set; }
    public DateOnly? created_on { get; set; }
    public string? last_edited_by {get; set;}
}

[Table("mdr.study_references")]
public class StudyReferenceInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public string? pmid { get; set; }
    public string? citation { get; set; }
    public string? doi { get; set; }
    public string? comments { get; set; }
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
}