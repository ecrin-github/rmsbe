using rmsbe.Services.Interfaces;
using rmsbe.SysModels;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class ContextService : IContextService
{
    private readonly IContextRepository _contextRepository;
    
    private List<LangCode> _languagecodes = new();
    private List<LangCode> _majorLanguagecodes = new();

    public ContextService(IContextRepository contextRepository)
    {
        _contextRepository = contextRepository ?? throw new ArgumentNullException(nameof(contextRepository));
    }
    
    /****************************************************************
    * Check functions for organisations
    ****************************************************************/

    public async Task<bool> OrgExistsAsync(int id)
        => await _contextRepository.OrgExistsAsync(id);
    
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

    public async Task<bool> LangCodeExistsAsync(string code)
           => await _contextRepository.LangCodeExistsAsync(code);

    public async Task<bool> LangNameExistsAsync(string name, string nameLang)
        => await _contextRepository.LangNameExistsAsync(name, nameLang);

    
    /****************************************************************
    * FETCH lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/

    public async Task<List<LangCode>?> GetLangCodes(string nameLang)
    {
        if (_languagecodes.Count == 0)
        {
            var langCodesInDb = (await _contextRepository.GetLangCodes(nameLang)).ToList();
            if (langCodesInDb.Count > 0)
            {
                _languagecodes = langCodesInDb.Select(r => new LangCode(r))
                    .OrderBy(r => r.Name)
                    .ToList();
            }
        }
        return _languagecodes;
    }
    
    public async Task<List<LangCode>?> GetMajorLangCodes(string nameLang)
    {
        if (_majorLanguagecodes.Count == 0)
        {
            var langCodesInDb = (await _contextRepository.GetMajorLangCodes(nameLang)).ToList();
            if (langCodesInDb.Count > 0)
            {
                _majorLanguagecodes = langCodesInDb.Select(r => new LangCode(r))
                    .OrderBy(r => r.Name)
                    .ToList();
            }
        }
        return _majorLanguagecodes;
    }

  
    /****************************************************************
    * FETCH lang details 
    ****************************************************************/

    public async Task<LangDetails?> GetLangDetailsFromCodeAsync(string code)
    {
        var langInDb = await _contextRepository.GetLangDetailsFromCodeAsync(code);
        return langInDb == null ? null : new LangDetails(langInDb);
    }
    
    public async Task<LangDetails?> GetLangDetailsFromNameAsync(string name, string nameLang)
    {
        var langInDb = await _contextRepository.GetLangDetailsFromNameAsync(name, nameLang);
        return langInDb == null ? null : new LangDetails(langInDb);
    }

}
 
