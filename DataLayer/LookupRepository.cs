using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;

namespace rmsbe.DataLayer;

public class LookupRepository : ILookupRepository
{
    private readonly string _dbConnString;
    private readonly Dictionary<string, string> _luList;
    
    public LookupRepository(ICreds creds)
    {
        _dbConnString = creds.GetConnectionString("context");
        
        // set up dictionary
        _luList = new Dictionary<string, string>
        {
            { "contribution-types", @" lup.contribution_types 
                                           where use_in_data_entry = true " },
            { "contribution-types-for-individuals", @" lup.contribution_types 
                                           where use_in_data_entry = true 
                                           and (applies_to = 'both' or applies_to = 'individual') " },
            { "contribution-types-for-organisations", @" lup.contribution_types 
                                           where use_in_data_entry = true 
                                           and (applies_to = 'both' or applies_to = 'organisation') " },
            { "dataset-consent-types", @" lup.dataset_consent_types " },
            { "dataset-deidentification-types", @" lup.dataset_deidentification_levels " },
            { "dataset-recordkey-types", @" lup.dataset_recordkey_types " },
            { "date-types", @" lup.date_types 
                                            where use_in_data_entry = true " },
            { "date-types-non-papers", @" lup.date_types 
                                            where use_in_data_entry = true 
                                            and applies_to_papers_only = false " },
            { "description-types", @" lup.description_types
                                            where use_in_data_entry = true " },
            { "gender-eligibility-types", @" lup.gender_eligibility_types
                                            where use_in_data_entry = true " },
            { "identifier-types", @" lup.identifier_types
                                            where use_in_data_entry = true " },
            { "identifier-types-for-studies", @" lup.identifier_types 
                                            where use_in_data_entry = true 
                                            and (applies_to = 'All' or applies_to = 'Study') " },
            { "identifier-types-for-objects", @" lup.identifier_types 
                                            where use_in_data_entry = true   
                                            and (applies_to = 'All' or applies_to = 'Data Object') " },
            { "object-access-types", @" lup.object_access_types
                                            where use_in_data_entry = true " },
            { "object-classes", @" lup.object_classes
                                            where use_in_data_entry = true " },
            { "object-filter-types", @" lup.object_filter_types" },
            { "object-relationship-types", @" lup.object_relationship_types
                                            where use_in_data_entry = true " },
            { "object-types", @" lup.object_types
                                            where use_in_data_entry = true " },
            { "object-types-text", @" lup.object_types
                                            where use_in_data_entry = true 
                                            and object_class_id = 23 " },
            { "object-types-data", @" lup.object_types
                                            where use_in_data_entry = true 
                                            and object_class_id = 14 " },
            { "object-types-other", @" lup.object_types
                                            where use_in_data_entry = true 
                                            and object_class_id <> 14 and object_class_id <> 23 " },
            { "resource-types", @" lup.resource_types
                                            where use_in_data_entry = true " },
            { "role-classes", @" lup.role_classes " },
            { "role-types", @" lup.role_types " },
            { "size-units", @" lup.size_units " },
            { "study-feature-types", @" lup.study_feature_types " },
            { "study-feature-categories", @" lup.study_feature_categories " },
            { "study-feature-phase-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 20 " },                   
            { "study-feature-purpose-categories",  @" lup.study_feature_categories 
                                            where feature_type_id = 21 " },               
            { "study-feature-allocation-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 22 " },               
            { "study-feature-intervention-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 23 " },               
            { "study-feature-masking-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 24 " },                
            { "study-feature-obs-model-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 30 " },                
            { "study-feature-obs-timeframe-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 31 " },               
            { "study-feature-samples-categories", @" lup.study_feature_categories 
                                            where feature_type_id = 32 " },                
            { "study-relationship-types", @" lup.study_relationship_types
                                            where use_in_data_entry = true " },
            { "study-statuses", @" lup.study_statuses
                                            where use_in_data_entry = true " },
            { "study-types", @" lup.study_types
                                            where use_in_data_entry = true " },
            { "time-units", @" lup.time_units
                                            where use_in_data_entry = true " },
            { "title-types ", @" lup.title_types
                                            where use_in_data_entry = true " },
            { "title-types-for-studies", @" lup.title_types 
                                            where use_in_data_entry = true 
                                            and (applies_to = 'All' or applies_to = 'Study') " },
            { "title-types-for-objects", @" lup.title_types 
                                            where use_in_data_entry = true 
                                            and (applies_to = 'All' or applies_to = 'Data Object') " },
            { "topic-types", @" lup.topic_types
                                            where use_in_data_entry = true " },
            { "topic-vocabularies", @" lup.topic_vocabularies
                                            where use_in_data_entry = true " },
            
            { "rms-user-types", @" lup.rms_user_types " },
            { "check-status-types", @" lup.check_status_types" },
            { "dtp-status-types", @" lup.dtp_status_types" },
            { "dup-status-types", @" lup.dup_status_types" },
            { "legal-status-types", @" lup.legal_status_types" },
            { "prerequisite-types", @" lup.access_prereq_types"  },
            { "repo-access-types", @" lup.repo_access_types" },
            { "trial-registries", @" lup.trial_registries" },
        };
    }
    
    public async Task<IEnumerable<BaseLup>> GetLupData(string typeName)
    {
        var sqlString = $"Select id, name, description, list_order from {_luList[typeName]}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<BaseLup>(sqlString);
    }
}
