using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface IContextService
{
    /****************************************************************
    * FETCH org records
    ****************************************************************/
    
    // All organisations (id, default_name)
    
    
    
    
    // All organisation names (id, name, org_id, default_name)
    
    
    
    
    // All organisation names containing string (id, name, org id, default name)
    
    
    /****************************************************************
    * FETCH filtered org records ????
    ****************************************************************/
    
    // All organisations (id, default_name) of a specific type
    
    
    
    
    // All organisation names (id, name, org_id, default_name) of a specific type
    
    
    
    
    // All organisation names containing string (id, name, org id,
    // default name) of a specific type
    
    
    
    
    
    // All organisations (id, default_name) in a defined geog entity
    
    
    
    
    
    
    // All organisation names (id, name, org_id, default_name)
    // in a defined geog entity
    
    
    
    
    
    
    // All organisation names containing string (id, name, org id,
    // in a defined geog entity
    

    
    
    
    /****************************************************************
    * FETCH org details
    ****************************************************************/
    
    
    // single organisation, from org_name id
    
    
    
    
    // single organisation, from org id
    
    
    
    
    // All names for organisation, by org id
    
    
    /****************************************************************
    * Check functions for languages
    ****************************************************************/

    Task<bool> LangCodeExistsAsync(string code);
    Task<bool> LangNameExistsAsync(string name, string nameLang);
    
    /****************************************************************
    * FETCH lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/

    Task<List<LangCode>?> GetLangCodes(string nameLang);
    Task<List<LangCode>?> GetMajorLangCodes(string nameLang);
  
    /****************************************************************
    * FETCH lang details 
    ****************************************************************/

    Task<LangDetails?> GetLangDetailsFromCodeAsync(string code);
    Task<LangDetails?> GetLangDetailsFromNameAsync(string name, string nameLang);

}

