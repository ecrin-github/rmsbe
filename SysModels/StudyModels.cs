using rmsbe.DbModels;
namespace rmsbe.SysModels;

public class FullStudy
{
    public StudyData? CoreStudy { get; set; }
    public List<StudyContributor>? StudyContributors { get; set; }
    public List<StudyFeature>? StudyFeatures { get; set; }
    public List<StudyIdentifier>? StudyIdentifiers { get; set; }
    public List<StudyReference>? StudyReferences { get; set; }
    public List<StudyRelationship>? StudyRelationships { get; set; }
    public List<StudyTitle>? StudyTitles { get; set; }
    public List<StudyTopic>? StudyTopics { get; set; }
    
    public FullStudy() { }

    public FullStudy(FullStudyInDb d)
    {
        CoreStudy = d.core_study == null ? null : new StudyData(d.core_study);
        StudyContributors = d.study_contributors_in_db?.Select(r => new StudyContributor(r)).ToList();
        StudyFeatures = d.study_features_in_db?.Select(r => new StudyFeature(r)).ToList();
        StudyIdentifiers = d.study_identifiers_in_db?.Select(r => new StudyIdentifier(r)).ToList();
        StudyRelationships = d.study_relationships_in_db?.Select(r => new StudyRelationship(r)).ToList();
        StudyTitles = d.study_titles_in_db?.Select(r => new StudyTitle(r)).ToList();
        StudyTopics = d.study_topics_in_db?.Select(r => new StudyTopic(r)).ToList();
    }
}

public class StudyData
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public string? MdrSdSid { get; set; }
    public int? MdrSourceId { get; set; }
    public string? DisplayTitle { get; set; }
    public string? TitleLangCode { get; set; }
    public string? BriefDescription { get; set; }
    public string? DataSharingStatement { get; set; }
    public int? StudyStartYear { get; set; }
    public int? StudyStartMonth { get; set; }
    public int? StudyTypeId { get; set; }
    public int? StudyStatusId { get; set; }
    public string? StudyEnrolment { get; set; }
    public int? StudyGenderEligId { get; set; }
    public int? MinAge { get; set; }
    public int? MinAgeUnitsId { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxAgeUnitsId { get; set; }
    
    public StudyData() { }

    public StudyData(StudyInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        MdrSdSid = d.mdr_sd_sid;
        MdrSourceId = d.mdr_source_id;
        DisplayTitle = d.display_title;
        TitleLangCode = d.title_lang_code;
        BriefDescription = d.brief_description;
        DataSharingStatement = d.data_sharing_statement;
        StudyStartYear = d.study_start_year;
        StudyStartMonth = d.study_start_month;
        StudyTypeId = d.study_type_id;
        StudyStatusId = d.study_status_id;
        StudyEnrolment = d.study_enrolment;
        StudyGenderEligId = d.study_gender_elig_id;
        MinAge = d.min_age;
        MinAgeUnitsId = d.min_age_units_id;
        MaxAge = d.max_age;
        MaxAgeUnitsId = d.max_age_units_id;
    }
}


public class StudyEntry
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public string? DisplayTitle { get; set; }
    
    public StudyEntry() { }

    public StudyEntry(StudyEntryInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        DisplayTitle = d.display_title;
    }
}

public class StudyIdentifier
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public string? IdentifierValue { get; set; }
    public int? IdentifierTypeId { get; set; }
    public int? IdentifierOrgId { get; set; }
    public string? IdentifierOrg { get; set; }
    public string? IdentifierOrgRorId { get; set; }
    public string? IdentifierDate { get; set; }
    public string? IdentifierLink { get; set; }
    
    public StudyIdentifier() { }

    public StudyIdentifier(StudyIdentifierInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        IdentifierValue = d.identifier_value;
        IdentifierTypeId = d.identifier_type_id;
        IdentifierOrgId = d.identifier_org_id;
        IdentifierOrg = d.identifier_org;
        IdentifierOrgRorId = d.identifier_org_ror_id;
        IdentifierDate = d.identifier_date;
        IdentifierLink = d.identifier_link;
    }
}

public class StudyTitle
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public int? TitleTypeId { get; set; }
    public string? TitleText { get; set; }
    public string? LangCode { get; set; }
    public int? LangUsageId { get; set; }
    public bool? IsDefault { get; set; }
    public string? Comments { get; set; }
    
    public StudyTitle() { }

    public StudyTitle(StudyTitleInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        TitleTypeId = d.title_type_id;
        TitleText = d.title_text;
        LangCode = d.lang_code;
        LangUsageId = d.lang_usage_id;
        IsDefault = d.is_default;
        Comments = d.comments;
    }
}

public class StudyContributor
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
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
    
    public StudyContributor() { }

    public StudyContributor(StudyContributorInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
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

public class StudyFeature
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public int? FeatureTypeId { get; set; }
    public int? FeatureValueId { get; set; }
    
    public StudyFeature() { }

    public StudyFeature(StudyFeatureInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        FeatureTypeId = d.feature_type_id;
        FeatureValueId = d.feature_value_id;
    }
}

public class StudyTopic
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public int? TopicTypeId { get; set; }
    public bool? MeshCoded { get; set; }
    public string? MeshCode { get; set; }
    public string? MeshValue { get; set; }
    public int? OriginalCtId { get; set; }
    public string? OriginalCtCode { get; set; }
    public string? OriginalValue { get; set; }
    
    public StudyTopic() { }

    public StudyTopic(StudyTopicInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        TopicTypeId = d.topic_type_id;
        MeshCoded = d.mesh_coded;
        MeshCode = d.mesh_code;
        MeshValue = d.mesh_value;
        OriginalCtId = d.original_ct_id;
        OriginalCtCode = d.original_ct_code;
        OriginalValue = d.original_value;
    }
}

public class StudyRelationship
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public int? RelationshipTypeId { get; set; }
    public string? TargetSdSid { get; set; }
    
    public StudyRelationship() { }

    public StudyRelationship(StudyRelationshipInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        RelationshipTypeId = d.relationship_type_id;
        TargetSdSid = d.target_sd_sid;
    }
}

public class StudyReference
{
    public int Id { get; set; }
    public string? SdSid { get; set; }
    public string? Pmid { get; set; }
    public string? Citation { get; set; }
    public string? Doi { get; set; }
    public string? Comments { get; set; }
    
    public StudyReference() { }

    public StudyReference(StudyReferenceInDb d)
    {
        Id = d.id;
        SdSid = d.sd_sid;
        Pmid = d.pmid;
        Citation = d.citation;
        Doi = d.doi;
        Comments = d.comments;
    }
}