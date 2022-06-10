using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;
using rmsbe.SysModels;

namespace rmsbe.Services;

public class LupService : ILookupService
{
    private ILookupRepository  _lupRepo;

    // Collection of Lists of look up data, 
    // which act as the in-memory cache of 
    // the look up data...
    // the data points are retrieved from the data
    // layer on first use...
    // The lists are all initialised as non-null
    // but empty lists
    
    private List<LupFull> contribution_types = new List<LupFull>();
    private List<LupFull> contribution_types_for_individuals = new List<LupFull>();
    private List<LupFull> contribution_types_for_organisations = new List<LupFull>();
    private List<LupFull> dataset_consent_types = new List<LupFull>();
    private List<LupFull> dataset_deidentification_types = new List<LupFull>();
    private List<LupFull> dataset_recordkey_types = new List<LupFull>();
    private List<LupFull> date_types = new List<LupFull>();
    private List<LupFull> description_types = new List<LupFull>();
    private List<LupFull> gender_eligibility_types = new List<LupFull>();
    private List<LupFull> identifier_types = new List<LupFull>();
    private List<LupFull> identifier_types_for_studies = new List<LupFull>();
    private List<LupFull> identifier_types_for_objects = new List<LupFull>();
    private List<LupFull> object_access_types = new List<LupFull>();
    private List<LupFull> object_classes = new List<LupFull>();
    private List<LupFull> object_filter_types = new List<LupFull>();
    private List<LupFull> object_relationship_types = new List<LupFull>();
    private List<LupFull> object_types = new List<LupFull>();
    private List<LupFull> object_types_text = new List<LupFull>();
    private List<LupFull> object_types_data = new List<LupFull>();
    private List<LupFull> object_types_other = new List<LupFull>();
    private List<LupFull> resource_types = new List<LupFull>();
    private List<LupFull> rms_user_types = new List<LupFull>();
    private List<LupFull> role_classes = new List<LupFull>();
    private List<LupFull> role_types = new List<LupFull>();
    private List<LupFull> size_units = new List<LupFull>();
    private List<LupFull> study_feature_types = new List<LupFull>();
    private List<LupFull> study_feature_categories = new List<LupFull>();
    private List<LupFull> study_feature_phase_categories = new List<LupFull>();
    private List<LupFull> study_feature_purpose_categories = new List<LupFull>();
    private List<LupFull> study_feature_allocation_categories = new List<LupFull>();
    private List<LupFull> study_feature_intervention_categories = new List<LupFull>();
    private List<LupFull> study_feature_masking_categories = new List<LupFull>();
    private List<LupFull> study_feature_obs_model_categories = new List<LupFull>();
    private List<LupFull> study_feature_obs_timeframe_categories = new List<LupFull>();
    private List<LupFull> study_feature_samples_categories = new List<LupFull>();
    private List<LupFull> study_relationship_types = new List<LupFull>();
    private List<LupFull> study_statuses = new List<LupFull>();
    private List<LupFull> study_types = new List<LupFull>();
    private List<LupFull> time_units = new List<LupFull>();
    private List<LupFull> title_types = new List<LupFull>();
    private List<LupFull> title_types_for_studies = new List<LupFull>();
    private List<LupFull> title_types_for_objects = new List<LupFull>();
    private List<LupFull> topic_types = new List<LupFull>();
    private List<LupFull> topic_vocabularies = new List<LupFull>();
    private List<LupFull> check_status_types = new List<LupFull>();
    private List<LupFull> dtp_status_types = new List<LupFull>();
    private List<LupFull> dup_status_types = new List<LupFull>();
    private List<LupFull> legal_status_types = new List<LupFull>();
    private List<LupFull> prerequisite_types = new List<LupFull>();
    private List<LupFull> repo_access_types = new List<LupFull>();

    // each list can be used by accessing it using the dictionary object
    private Dictionary<string, List<LupFull>> LUList;
    
    public LupService(ILookupRepository lupRepo)
    {
        _lupRepo = lupRepo ?? throw new ArgumentNullException(nameof(lupRepo));
        
        // set up dictionary
        LUList = new Dictionary<string, List<LupFull>>
        {
            { "contribution-types", contribution_types },
            { "contribution-types-for-individuals", contribution_types_for_individuals },
            { "contribution-types-for-organisations", contribution_types_for_organisations },
            { "dataset-consent-types", dataset_consent_types },
            { "dataset-deidentification-types", dataset_deidentification_types },
            { "dataset-recordkey-types", dataset_recordkey_types },
            { "date-types", date_types },
            { "description-types", description_types },
            { "gender-eligibility-types", gender_eligibility_types },
            { "identifier-types", identifier_types },
            { "identifier-types-for-studies", identifier_types_for_studies },
            { "identifier-types-for-objects", identifier_types_for_objects },
            { "object-access-types", object_access_types },
            { "object-classes", object_classes },
            { "object-filter-types", object_filter_types },
            { "object-relationship-types", object_relationship_types },
            { "object-types", object_types },
            { "object-types-text", object_types_text },
            { "object-types-data", object_types_data },
            { "object-types-other", object_types_other },
            { "resource-types", resource_types },
            { "rms-user-types", rms_user_types },
            { "role-classes", role_classes },
            { "role-types", role_types },
            { "size-units", size_units },
            { "study-feature-types", study_feature_types },
            { "study-feature-categories", study_feature_categories },
            { "study-feature-phase-categories", study_feature_phase_categories },
            { "study-feature-purpose-categories",  study_feature_purpose_categories },
            { "study-feature-allocation-categories", study_feature_allocation_categories },
            { "study-feature-intervention-categories", study_feature_intervention_categories },
            { "study-feature-masking-categories", study_feature_masking_categories },
            { "study-feature-obs_model-categories", study_feature_obs_model_categories },
            { "study-feature-obs_timeframe-categories", study_feature_obs_timeframe_categories },
            { "study-feature-samples-categories", study_feature_samples_categories },
            { "study-relationship-types", study_relationship_types },
            { "study-statuses", study_statuses },
            { "study-types", study_types },
            { "time-units", time_units },
            { "title-types ", title_types  },
            { "title-types-for-studies", title_types_for_studies },
            { "title-types-for-objects", title_types_for_objects },
            { "topic-types", topic_types },
            { "topic-vocabularies", topic_vocabularies },
            { "check-status-types", check_status_types },
            { "dtp-status-types", dtp_status_types },
            { "dup-status-types", dup_status_types },
            { "legal-status-types", legal_status_types },
            { "prerequisite-types",  prerequisite_types },
            { "repo-access-types", repo_access_types },
        };
    }

    private async Task<List<LupFull>?> GetLookupListAsync(string type_name)
    {
        // ensure type name is in dictionary list - if not simply return null
        if (!LUList.ContainsKey(type_name))
        {
            return null;
        }
        
        // retrieve the relevant list of lookup data
        List<LupFull> lupList = LUList[type_name];
            
        // If it currently has no data fill it from the data layer
        if (lupList.Count == 0)
        {
            var lupValues = await _lupRepo.GetLupDataAsync(type_name);
            if (lupValues == null)
            {
                return null;  // unable to fill this Lup list...
            }
            lupList = lupValues.Select(r => new LupFull(r)).ToList();
            LUList[type_name] = lupList;
        }
        
        return lupList;
    }

    public async Task<List<Lup>?> GetLookUpValuesAsync(string type_name)
    {
        var lupValues = await GetLookupListAsync(type_name);

        // select the relevant values from the list using Linq (if non null
        return lupValues?.Select(r => new Lup(r.Id, r.Name)).ToList();
    }
    
    
    public async Task<List<LupWithDescription>?> GetLookUpValuesWithDescsAsync(string type_name)
    {
        var lupValues = await GetLookupListAsync(type_name);

        // select the relevant values from the list using Linq (if non null
        return lupValues?.Select(r => new LupWithDescription(r.Id, r.Name, r.Description)).ToList();
    }

    public async Task<List<LupWithListOrder>?> GetLookUpValuesWithListOrdersAsync(string type_name)
    {
        var lupValues = await GetLookupListAsync(type_name);

        // select the relevant values from the list using Linq (if non null)
        return lupValues?.Select(r => new LupWithListOrder(r.Id, r.Name, r.ListOrder)).ToList();
    }
    
    public async Task<List<LupFull>?> GetLookUpValuesWithDescsAndLosAsync(string type_name)
    {
        // can return the result of the list fetch immediately
        return await GetLookupListAsync(type_name);
    }

    public async Task<string?> GetLookUpTextDecodeAsync(string type_name, int code)
    {
        var lupValues = await GetLookupListAsync(type_name);
        string? result = null;
        if (lupValues != null)
        {
            foreach (LupFull p in lupValues)
            {
                if (p.Id == code)
                {
                    result = p.Name;
                    break;
                }
            }
        }
        return result;
    }

    public async Task<int?> GetLookUpValueAsync(string type_name, string decode)
    {
        var lupValues = await GetLookupListAsync(type_name);
        int? result = null;
        if (lupValues != null)
        {
            foreach (LupFull p in lupValues)
            {
                if (p.Name == decode)
                {
                    result = p.Id;
                    break;
                }
            }
        }
        return result;
    }
}