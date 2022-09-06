using rmsbe.Services.Interfaces;
using rmsbe.SysModels;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class ContextService : IContextService
{
    private readonly IContextRepository _contextRepository;
    
    private List<LangCode> _enLanguagecodes = new();
    private List<LangCode> _enMajorLanguagecodes = new();
    private List<LangCode> _frLanguagecodes = new();
    private List<LangCode> _frMajorLanguagecodes = new();
    private List<LangCode> _deLanguagecodes = new();
    private List<LangCode> _deMajorLanguagecodes = new();
    
    public ContextService(IContextRepository contextRepository)
    {
        _contextRepository = contextRepository ?? throw new ArgumentNullException(nameof(contextRepository));
    }
    
    /****************************************************************
    * Check functions for organisations
    ****************************************************************/

    public async Task<bool> OrgExists(int id)
        => await _contextRepository.OrgExists(id);
    
    /****************************************************************
    * FETCH organisation name lists, from orgs table
    ****************************************************************/
    
    // All organisations (id, default_name from ctx.organisations)
    public async Task<List<OrgTableData>?> GetOrgsTableData()
    {
        var orgsInDb = (await _contextRepository.GetOrgsTableData()).ToList();
        return !orgsInDb.Any() ? null 
            : orgsInDb.Select(r => new OrgTableData(r)).ToList();
    }
    
    // All organisations containing string (id, default_name from ctx.organisations)
    public async Task<List<OrgTableData>?> GetFilteredOrgsTableData(string filter)
    {
        var orgsInDb = (await _contextRepository.GetFilteredOrgsTableData(filter)).ToList();
        return !orgsInDb.Any() ? null 
            : orgsInDb.Select(r => new OrgTableData(r)).ToList();
    }
    
    /****************************************************************
    * FETCH organisation lists 
    ****************************************************************/
    
    // All organisations (id, default_name)
    public async Task<List<OrgSimple>?> GetOrgs()
    {
        var orgsInDb = (await _contextRepository.GetOrgs()).ToList();
        return !orgsInDb.Any() ? null 
            : orgsInDb.Select(r => new OrgSimple(r)).ToList();
    }
    
    // All organisations containing string (id, default_name)
    public async Task<List<OrgSimple>?> GetFilteredOrgs(string filter)
    {
        var orgsInDb = (await _contextRepository.GetFilteredOrgs(filter)).ToList();
        return !orgsInDb.Any() ? null 
            : orgsInDb.Select(r => new OrgSimple(r)).ToList();
    }
    
    /****************************************************************
    * FETCH organisation lists using all org names
    ****************************************************************/
    
    // All organisation names (id, name, org_id, default_name)
    public async Task<List<OrgWithNames>?> GetOrgNames()
    {
        var orgsNamesInDb = (await _contextRepository.GetOrgNames()).ToList();
        return !orgsNamesInDb.Any() ? null 
            : orgsNamesInDb.Select(r => new OrgWithNames(r)).ToList();
    }
    
    // All organisation names containing string (id, name, org id, default name)
    public async Task<List<OrgWithNames>?> GetFilteredOrgNames(string filter)
    {
        var orgsNamesInDb = (await _contextRepository.GetFilteredOrgNames(filter)).ToList();
        return !orgsNamesInDb.Any() ? null 
            : orgsNamesInDb.Select(r => new OrgWithNames(r)).ToList();
    }
    
    /****************************************************************
    * FETCH organisation details from org id
    ****************************************************************/
    
    // to do
    
    /****************************************************************
    * Check functions for languages
    ****************************************************************/

    public async Task<bool> LangCodeExists(string code)
           => await _contextRepository.LangCodeExists(code);

    public async Task<bool> LangNameExists(string name, string nameLang)
        => await _contextRepository.LangNameExists(name, nameLang);

    
    /****************************************************************
    * FETCH lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/

    public async Task<List<LangCode>?> GetLangCodes(string nameLang)
    {
        List<LangCode> codes = GetListLangCodes(nameLang);
        
        if (codes.Count == 0)
        {
            var langCodesInDb = (await _contextRepository.GetLangCodes(nameLang)).ToList();
            if (langCodesInDb.Count > 0)
            {
                codes = langCodesInDb.Select(r => new LangCode(r))
                    .OrderBy(r => r.Name)
                    .ToList();
            }
        }
        return codes;
    }
    
    
    public async Task<List<LangCode>?> GetMajorLangCodes(string nameLang)
    {
        List<LangCode> codes = GetListMajorLangCodes(nameLang);
        
        if (codes.Count == 0)
        {
            var langCodesInDb = (await _contextRepository.GetMajorLangCodes(nameLang)).ToList();
            if (langCodesInDb.Count > 0)
            {
                codes = langCodesInDb.Select(r => new LangCode(r))
                    .OrderBy(r => r.Name)
                    .ToList();
            }
        }
        return codes;
    }

    
    private List<LangCode> GetListLangCodes(string nameLang)
    {
        var langCodeList = _enLanguagecodes;  // default
        if (nameLang.ToLower().StartsWith("fr"))
        {
            langCodeList = _frLanguagecodes;
        }
        else if (nameLang.ToLower().StartsWith("de"))
        {
            langCodeList = _deLanguagecodes;
        }
        return langCodeList;
    }
    
    private List<LangCode> GetListMajorLangCodes(string nameLang)
    {
        var langCodeList = _enMajorLanguagecodes;  // default
        if (nameLang.ToLower().StartsWith("fr"))
        {
            langCodeList = _frMajorLanguagecodes;
        }
        else if (nameLang.ToLower().StartsWith("de"))
        {
            langCodeList = _deMajorLanguagecodes;
        }
        return langCodeList;
    }
  
    /****************************************************************
    * FETCH lang details 
    ****************************************************************/

    public async Task<LangDetails?> GetLangDetailsFromCode(string code)
    {
        var langInDb = await _contextRepository.GetLangDetailsFromCode(code);
        return langInDb == null ? null : new LangDetails(langInDb);
    }
    
    public async Task<LangDetails?> GetLangDetailsFromName(string name, string nameLang)
    {
        var langInDb = await _contextRepository.GetLangDetailsFromName(name, nameLang);
        return langInDb == null ? null : new LangDetails(langInDb);
    }

}
 
