using System.Diagnostics.CodeAnalysis;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;
using rmsbe.SysModels;

namespace rmsbe.Services;

[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
public class LookupService : ILookupService
{
    private ILookupRepository  _lupRepo;

    // Collection of Lists of look up data, 
    // which act as the in-memory cache of 
    // the look up data...
    // the data points are retrieved from the data
    // layer on first use...
    // The lists are all initialised as non-null
    // but empty lists
    
    private List<LupFull> _contributionTypes = new List<LupFull>();
    private List<LupFull> _contributionTypesForIndividuals = new List<LupFull>();
    private List<LupFull> _contributionTypesForOrganisations = new List<LupFull>();
    private List<LupFull> _datasetConsentTypes = new List<LupFull>();
    private List<LupFull> _datasetDeidentificationTypes = new List<LupFull>();
    private List<LupFull> _datasetRecordkeyTypes = new List<LupFull>();
    private List<LupFull> _dateTypes = new List<LupFull>();
    private List<LupFull> _dateTypesNonPapers = new List<LupFull>();
    private List<LupFull> _descriptionTypes = new List<LupFull>();
    private List<LupFull> _genderEligibilityTypes = new List<LupFull>();
    private List<LupFull> _identifierTypes = new List<LupFull>();
    private List<LupFull> _identifierTypesForStudies = new List<LupFull>();
    private List<LupFull> _identifierTypesForObjects = new List<LupFull>();
    private List<LupFull> _objectAccessTypes = new List<LupFull>();
    private List<LupFull> _objectClasses = new List<LupFull>();
    private List<LupFull> _objectFilterTypes = new List<LupFull>();
    private List<LupFull> _objectRelationshipTypes = new List<LupFull>();
    private List<LupFull> _objectTypes = new List<LupFull>();
    private List<LupFull> _objectTypesText = new List<LupFull>();
    private List<LupFull> _objectTypesData = new List<LupFull>();
    private List<LupFull> _objectTypesOther = new List<LupFull>();
    private List<LupFull> _resourceTypes = new List<LupFull>();
    private List<LupFull> _rmsUserTypes = new List<LupFull>();
    private List<LupFull> _roleClasses = new List<LupFull>();
    private List<LupFull> _roleTypes = new List<LupFull>();
    private List<LupFull> _sizeUnits = new List<LupFull>();
    private List<LupFull> _studyFeatureTypes = new List<LupFull>();
    private List<LupFull> _studyFeatureCategories = new List<LupFull>();
    private List<LupFull> _studyFeaturePhaseCategories = new List<LupFull>();
    private List<LupFull> _studyFeaturePurposeCategories = new List<LupFull>();
    private List<LupFull> _studyFeatureAllocationCategories = new List<LupFull>();
    private List<LupFull> _studyFeatureInterventionCategories = new List<LupFull>();
    private List<LupFull> _studyFeatureMaskingCategories = new List<LupFull>();
    private List<LupFull> _studyFeatureObsModelCategories = new List<LupFull>();
    private List<LupFull> _studyFeatureObsTimeframeCategories = new List<LupFull>();
    private List<LupFull> _studyFeatureSamplesCategories = new List<LupFull>();
    private List<LupFull> _studyRelationshipTypes = new List<LupFull>();
    private List<LupFull> _studyStatuses = new List<LupFull>();
    private List<LupFull> _studyTypes = new List<LupFull>();
    private List<LupFull> _timeUnits = new List<LupFull>();
    private List<LupFull> _titleTypes = new List<LupFull>();
    private List<LupFull> _titleTypesForStudies = new List<LupFull>();
    private List<LupFull> _titleTypesForObjects = new List<LupFull>();
    private List<LupFull> _topicTypes = new List<LupFull>();
    private List<LupFull> _topicVocabularies = new List<LupFull>();
    private List<LupFull> _checkStatusTypes = new List<LupFull>();
    private List<LupFull> _dtpStatusTypes = new List<LupFull>();
    private List<LupFull> _dupStatusTypes = new List<LupFull>();
    private List<LupFull> _legalStatusTypes = new List<LupFull>();
    private List<LupFull> _prerequisiteTypes = new List<LupFull>();
    private List<LupFull> _repoAccessTypes = new List<LupFull>();

    // each list can be used by accessing it using the dictionary object
    private Dictionary<string, List<LupFull>> _luList;
    
    public LookupService(ILookupRepository lupRepo)
    {
        _lupRepo = lupRepo ?? throw new ArgumentNullException(nameof(lupRepo));
        
        // set up dictionary
        _luList = new Dictionary<string, List<LupFull>>
        {
            { "contribution-types", _contributionTypes },
            { "contribution-types-for-individuals", _contributionTypesForIndividuals },
            { "contribution-types-for-organisations", _contributionTypesForOrganisations },
            { "dataset-consent-types", _datasetConsentTypes },
            { "dataset-deidentification-types", _datasetDeidentificationTypes },
            { "dataset-recordkey-types", _datasetRecordkeyTypes },
            { "date-types", _dateTypes },
            { "date-types-non-papers", _dateTypesNonPapers },
            { "description-types", _descriptionTypes },
            { "gender-eligibility-types", _genderEligibilityTypes },
            { "identifier-types", _identifierTypes },
            { "identifier-types-for-studies", _identifierTypesForStudies },
            { "identifier-types-for-objects", _identifierTypesForObjects },
            { "object-access-types", _objectAccessTypes },
            { "object-classes", _objectClasses },
            { "object-filter-types", _objectFilterTypes },
            { "object-relationship-types", _objectRelationshipTypes },
            { "object-types", _objectTypes },
            { "object-types-text", _objectTypesText },
            { "object-types-data", _objectTypesData },
            { "object-types-other", _objectTypesOther },
            { "resource-types", _resourceTypes },
            { "rms-user-types", _rmsUserTypes },
            { "role-classes", _roleClasses },
            { "role-types", _roleTypes },
            { "size-units", _sizeUnits },
            { "study-feature-types", _studyFeatureTypes },
            { "study-feature-categories", _studyFeatureCategories },
            { "study-feature-phase-categories", _studyFeaturePhaseCategories },
            { "study-feature-purpose-categories",  _studyFeaturePurposeCategories },
            { "study-feature-allocation-categories", _studyFeatureAllocationCategories },
            { "study-feature-intervention-categories", _studyFeatureInterventionCategories },
            { "study-feature-masking-categories", _studyFeatureMaskingCategories },
            { "study-feature-obs-model-categories", _studyFeatureObsModelCategories },
            { "study-feature-obs-timeframe-categories", _studyFeatureObsTimeframeCategories },
            { "study-feature-samples-categories", _studyFeatureSamplesCategories },
            { "study-relationship-types", _studyRelationshipTypes },
            { "study-statuses", _studyStatuses },
            { "study-types", _studyTypes },
            { "time-units", _timeUnits },
            { "title-types ", _titleTypes  },
            { "title-types-for-studies", _titleTypesForStudies },
            { "title-types-for-objects", _titleTypesForObjects },
            { "topic-types", _topicTypes },
            { "topic-vocabularies", _topicVocabularies },
            { "check-status-types", _checkStatusTypes },
            { "dtp-status-types", _dtpStatusTypes },
            { "dup-status-types", _dupStatusTypes },
            { "legal-status-types", _legalStatusTypes },
            { "prerequisite-types",  _prerequisiteTypes },
            { "repo-access-types", _repoAccessTypes },
        };
    }

    private async Task<List<LupFull>?> GetLookupListAsync(string type_name)
    {
        // ensure type name is in dictionary list - if not simply return null
        if (!_luList.ContainsKey(type_name))
        {
            return null;
        }
        
        // retrieve the relevant list of lookup data
        List<LupFull> lupList = _luList[type_name];
            
        // If it currently has no data fill it from the data layer
        if (lupList.Count == 0)
        {
            var lupValues = await _lupRepo.GetLupDataAsync(type_name);
            if (lupValues == null)
            {
                return null;  // unable to fill this Lup list...
            }
            lupList = lupValues.Select(r => new LupFull(r))
                               .OrderBy(r => r.ListOrder)
                               .ToList();
            _luList[type_name] = lupList;   // update the dictionary entry
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
        return lupValues?.Select(r => new LupWithListOrder(r.Id, r.Name, r.ListOrder))
                         .OrderBy(r => r.ListOrder)
                         .ToList();
    }
    
    
    public async Task<List<LupFull>?> GetLookUpValuesWithDescsAndLosAsync(string type_name)
    {
        var lupValues = await GetLookupListAsync(type_name);
        // Select but order at the same time
        return lupValues?.OrderBy(r => r.ListOrder)
                         .ToList();
    }

    
    public async Task<string?> GetLookUpTextDecodeAsync(string type_name, int code)
    {
        var lupValues = await GetLookupListAsync(type_name);
        return lupValues == null ? null 
               : (from p in lupValues where p.Id == code select p.Name).FirstOrDefault();
    }

    
    public async Task<int?> GetLookUpValueAsync(string type_name, string decode)
    {
        var lupValues = await GetLookupListAsync(type_name);
        return lupValues == null ? null
               : (from p in lupValues where p.Name == decode select p.Id).FirstOrDefault();
    }
}