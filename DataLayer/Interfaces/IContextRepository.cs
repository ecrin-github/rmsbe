using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IContextRepository
{
    /****************************************************************
    * Check functions for organisations
    ****************************************************************/

    Task<bool> OrgExists(int id);
    
    /****************************************************************
    * FETCH org records
    ****************************************************************/
    
    // All organisations (id, default_name)
    Task<IEnumerable<OrgSimpleInDb>> GetOrgs();
    
    
    // All organisations containing string (id, default_name)
    Task<IEnumerable<OrgSimpleInDb>> GetFilteredOrgs(string filter);
    
    
    // All organisation names (id, name, org_id, default_name)
    Task<IEnumerable<OrgWithNamesInDb>> GetOrgNames();
    
    
    // All organisation names containing string (id, name, org id, default name)
    Task<IEnumerable<OrgWithNamesInDb>> GetFilteredOrgNames(string filter);
    
    
    /****************************************************************
    * Check functions for languages
    ****************************************************************/

    Task<bool> LangCodeExists(string code);
    Task<bool> LangNameExists(string name, string nameLang);
    
    /****************************************************************
    * FETCH lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/

    Task<IEnumerable<LangCodeInDb>> GetLangCodes(string nameLang);
    Task<IEnumerable<LangCodeInDb>> GetMajorLangCodes(string nameLang);
  
    /****************************************************************
    * FETCH lang details 
    ****************************************************************/

    Task<LangDetailsInDb?> GetLangDetailsFromCode(string code);
    Task<LangDetailsInDb?> GetLangDetailsFromName(string name, string nameLang);

    

}
