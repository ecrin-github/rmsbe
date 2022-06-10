using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface ILookupService
{
   Task<List<Lup>?> GetLookUpValuesAsync(string type_name);
    Task<List<LupWithDescription>?> GetLookUpValuesWithDescsAsync(string type_name);
    Task<List<LupWithListOrder>?> GetLookUpValuesWithListOrdersAsync(string type_name);
    Task<List<LupFull>?> GetLookUpValuesWithDescsAndLosAsync(string type_name);

    Task<string?> GetLookUpTextDecodeAsync(string type_name, int code);
    Task<int?> GetLookUpValueAsync(string type_name, string decode);
    
    /*
     Available look up type parameters
     
     contribution-types
     contribution-types-for-individuals
     contribution-types-for-contributions
     dataset-consent-types
     dataset-deidentification-types
     dataset-recordkey-types
     date-types
     description-types
     gender-eligibility-types
     identifier-types
     identifier-types-for-studies
     identifier-types-for-objects
     object-access-types
     object-classes
     object-filter-types
     object-relationship-types
     object-types
     object-types-text
     object-types-data
     object-types-other
     resource-types
     rms-user-types
     role-classes
     role-types
     size-units
     study-feature-types
     study-feature-categories
     study-feature-phase-categories
     study-feature-purpose-categories
     study-feature-allocation-categories
     study-feature-intervention-categories
     study-feature-masking-categories
     study-feature-obs_model-categories
     study-feature-obs_timeframe-categories
     study-feature-samples-categories
     study-relationship-types
     study-statuses
     study-types
     time-units
     title-types     
     title-types-for-studies
     title-types-for-objects
     topic-types
     topic-vocabularies
     check-status-types    
     dtp-status-types  
     dup-status-types    
     legal-status-types   
     prerequisite-types
     repo-access-types
      
*/

}

