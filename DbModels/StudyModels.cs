using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

public class FullStudyInDb
{
    public StudyInDb? core_study { get; set; }
    public List<StudyContributorInDb>? study_contributors_in_db { get; set; }
    public List<StudyFeatureInDb>? study_features_in_db { get; set; }
    public List<StudyIdentifierInDb>? study_identifiers_in_db { get; set; }
    public List<StudyReferenceInDb>? study_references_in_db { get; set; }
    public List<StudyRelationshipInDb>? study_relationships_in_db { get; set; }
    public List<StudyTitleInDb>? study_titles_in_db { get; set; }
    public List<StudyTopicInDb>? study_topics_in_db { get; set; }
    
    public FullStudyInDb() { }
    
    public FullStudyInDb(StudyInDb? coreStudy, List<StudyContributorInDb>? studyContributorsInDb,
           List<StudyFeatureInDb>? studyFeaturesInDb, List<StudyIdentifierInDb>? studyIdentifiersInDb,
           List<StudyReferenceInDb>? studyReferencesInDb, List<StudyRelationshipInDb>? studyRelationshipsInDb,
           List<StudyTitleInDb>? studyTitlesInDb, List<StudyTopicInDb>? studyTopicsInDb)
    {
        core_study = coreStudy;
        study_contributors_in_db = studyContributorsInDb;
        study_features_in_db = studyFeaturesInDb;
        study_identifiers_in_db = studyIdentifiersInDb;
        study_references_in_db = studyReferencesInDb;
        study_relationships_in_db = studyRelationshipsInDb;
        study_titles_in_db = studyTitlesInDb;
        study_topics_in_db = studyTopicsInDb;
    }
}

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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;} 
    
    public StudyInDb() { }

    public StudyInDb(StudyData d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        mdr_sd_sid = d.MdrSdSid;
        mdr_source_id = d.MdrSourceId;
        display_title = d.DisplayTitle;
        title_lang_code = d.TitleLangCode;
        brief_description = d.BriefDescription;
        data_sharing_statement = d.DataSharingStatement;
        study_start_year = d.StudyStartYear;
        study_start_month = d.StudyStartMonth;
        study_type_id = d.StudyTypeId;
        study_status_id = d.StudyStatusId;
        study_enrolment = d.StudyEnrolment;
        study_gender_elig_id = d.StudyGenderEligId;
        min_age = d.MinAge;
        min_age_units_id = d.MinAgeUnitsId;
        max_age = d.MaxAge;
        max_age_units_id = d.MaxAgeUnitsId;
    }
}


[Table("mdr.study_identifiers")]
public class StudyIdentifierInDb
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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyIdentifierInDb() { }

    public StudyIdentifierInDb(StudyIdentifier d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        identifier_value = d.IdentifierValue;
        identifier_type_id = d.IdentifierTypeId;
        identifier_org_id = d.IdentifierOrgId;
        identifier_org = d.IdentifierOrg;
        identifier_org_ror_id = d.IdentifierOrgRorId;
        identifier_date = d.IdentifierDate;
        identifier_link = d.IdentifierLink;
    }
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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyTitleInDb() { }

    public StudyTitleInDb(StudyTitle d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        title_type_id = d.TitleTypeId;
        title_text = d.TitleText;
        lang_code = d.LangCode;
        lang_usage_id = d.LangUsageId;
        is_default = d.IsDefault;
        comments = d.Comments;
    }
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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyContributorInDb() { }

    public StudyContributorInDb(StudyContributor d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
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
}


[Table("mdr.study_features")]
public class StudyFeatureInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public int? feature_type_id { get; set; }
    public int? feature_value_id { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyFeatureInDb() { }

    public StudyFeatureInDb(StudyFeature d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        feature_type_id = d.FeatureTypeId;
        feature_value_id = d.FeatureValueId;
    }
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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyTopicInDb() { }

    public StudyTopicInDb(StudyTopic d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        topic_type_id = d.TopicTypeId;
        mesh_coded = d.MeshCoded;
        mesh_code = d.MeshCode;
        mesh_value = d.MeshValue;
        original_ct_id = d.OriginalCtId;
        original_ct_code = d.OriginalCtCode;
        original_value = d.OriginalValue;
    }
}


[Table("mdr.study_relationships")]
public class StudyRelationshipInDb
{
    public int id { get; set; }
    public string? sd_sid { get; set; }
    public int? relationship_type_id { get; set; }
    public string? target_sd_sid { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyRelationshipInDb() { }

    public StudyRelationshipInDb(StudyRelationship d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        relationship_type_id = d.RelationshipTypeId;
        target_sd_sid = d.TargetSdSid;
    }
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
    [Computed]
    public DateTime? created_on { get; set; }
    public string? last_edited_by {get; set;}
    
    public StudyReferenceInDb() { }

    public StudyReferenceInDb(StudyReference d)
    {
        id = d.Id;
        sd_sid = d.SdSid;
        pmid = d.Pmid;
        citation = d.Citation;
        doi = d.Doi;
        comments = d.Comments;
    }
}