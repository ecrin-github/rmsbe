using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

/****************************************************************************
 *
 * These classes correspond to the look up tables in the database, hence all
 * end in '...InDb'.
 * They are designed to be used within processes for
 * 1) retrieving the data from the DB, (minus source and date_added fields),
 * storing it in a matching lookup service file that caches the data within 
 * list<> structures, from where it can easily be retrieved.
 * 2) for creating or updating records in any of these tables, by populating
 * the relevant class and then using it as a parameter in a Dapper.Contrib 
 * Update or Insert statement. (Such editing is not currently done in any 
 * ECRIN system, but may be required in the future.)
 * 
 * They are designed to work work with a matching repository file, or pair
 * of such files, which will include the routines to retrieve the data from
 * the DB, and / or to create Insert, Update and Delete statements against
 * individual tables.
 *
 * v1.0, Steve Canham; 02/06/2022
 ***************************************************************************/

public class BaseLup
{
    public int id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public int? list_order { get; set; }
}

public class BaseLupInDb : BaseLup 
{
    public string? source { get; set; }
    public DateOnly date_added { get; set; }
}

[Table("lup.size_units")]
public class SizeUnitInDb : BaseLupInDb { }
[Table("lup.doi_status_types")]
public class DoiStatusTypeInDb : BaseLupInDb { }
[Table("lup.language_usage_types")]
public class LanguageUsageTypeInDb : BaseLupInDb { }
[Table("lup.dataset_recordkey_types")]
public class DatasetRecordkeyTypeInDb : BaseLupInDb { }
[Table("lup.dataset_deidentification_levels")]
public class DatasetDeidentificationLevelInDb : BaseLupInDb { }
[Table("lup.dataset_consent_types")]
public class DatasetConsentTypeInDb : BaseLupInDb { }
[Table("lup.object_filter_types")]
public class ObjectFilterTypeInDb : BaseLupInDb { }
[Table("lup.object_instance_types")]
public class ObjectInstanceTypeInDb : BaseLupInDb { }
[Table("lup.geog_entity_types")]
public class GeogEntityTypeInDb : BaseLupInDb { }
[Table("lup.link_types")]
public class LinkTypeInDb : BaseLupInDb { }
[Table("lup.org_attribute_datatypes")]
public class OrgAttributeDatatypeInDb : BaseLupInDb { }
[Table("lup.org_classes")]
public class OrgClassInDb : BaseLupInDb { }
[Table("lup.org_link_types")]
public class OrgLinkTypeInDb : BaseLupInDb { }
[Table("lup.org_name_qualifier_types")]
public class OrgNameQualifierTypeInDb : BaseLupInDb { }
[Table("lup.org_relationship_types")]
public class OrgRelationshipTypeInDb : BaseLupInDb { }
[Table("lup.rms_user_types")]
public class RmsUserTypeInDb : BaseLupInDb { }
[Table("lup.role_classes")]
public class RoleClassInDb : BaseLupInDb { }

[Table("lup.study_feature_types")]
public class StudyFeatureTypeInDb : BaseLupInDb
{
    public string? context { get; set; }
}
[Table("lup.study_feature_categories")]
public class StudyFeatureCategoryInDb : BaseLupInDb
{
    public int feature_type_id { get; set; }
}

[Table("lup.composite_hash_types")]
public class CompositeHashTypeInDb : BaseLupInDb
{
    public string? applies_to { get; set; }
}

[Table("lup.role_types")]
public class RoleTypeInDb : BaseLupInDb
{
    public int class_id { get; set; }
}

[Table("lup.org_types")]
public class OrgTypeInDb : BaseLupInDb
{
    public int class_id { get; set; }
}

[Table("lup.org_attribute_types")]
public class OrgAttributeTypeInDb : BaseLupInDb
{
    public int type_id {get; set;}
    public bool can_repeat {get; set;}
    public int parent_id {get; set;}
    public bool for_display {get; set;}
}

public class BaseDataEntryLupInDb : BaseLupInDb
{
    public bool? use_in_data_entry { get; set; } 
}

[Table("lup.description_types")]
public class DescriptionTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.object_classes")]
public class ObjectClassInDb : BaseDataEntryLupInDb { }
[Table("lup.time_units")]
public class TimeUnitInDb : BaseDataEntryLupInDb { }
[Table("lup.study_statuses")]
public class StudyStatusInDb : BaseDataEntryLupInDb { }
[Table("lup.study_types")]
public class StudyTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.resource_types")]
public class ResourceTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.object_access_types")]
public class ObjectAccessTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.gender_eligibility_types")]
public class GenderEligibilityTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.object_relationship_types")]
public class ObjectRelationshipTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.study_relationship_types")]
public class StudyRelationshipTypeInDb : BaseDataEntryLupInDb { }
[Table("lup.topic_types")]
public class TopicTypeInDb : BaseDataEntryLupInDb { }

public class BaseAppliesToLupInDb : BaseDataEntryLupInDb 
{ 
    public string? applies_to { get; set; }
}

[Table("lup.identifier_types")]
public class IdentifierTypeInDb : BaseAppliesToLupInDb { }
[Table("lup.title_types")]
public class TitleTypeInDb : BaseAppliesToLupInDb { }
[Table("lup.contribution_types")]
public class ContributionTypeInDb : BaseAppliesToLupInDb { }

[Table("lup.topic_vocabularies")]
public class TopicVocabularyInDb : BaseDataEntryLupInDb 
{
    public string? url {get; set;}
}

[Table("lup.date_types")]
public class DateTypeInDb : BaseDataEntryLupInDb 
{ 
    public bool? applies_to_papers_only { get; set; }
}

[Table("lup.object_types")]
public class ObjectTypeInDb : BaseDataEntryLupInDb 
{
    public int? object_class_id { get; set; }
    public int? filter_as_id { get; set; }
}

[Table("lup.language_codes")]
public class LanguageCodeInDb
{
    [Key]
    public string? code { get; set; }
    public string? marc_code { get; set; }
    public string? lang_name_en { get; set; }
    public string? lang_name_fr { get; set; }
    public string? lang_name_de { get; set; }
    public string? source { get; set; }
    public DateOnly date_added { get; set; }
}

