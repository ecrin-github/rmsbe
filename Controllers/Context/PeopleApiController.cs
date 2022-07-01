using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.Context;

public class PeopleApiController : BaseApiController
{
    private readonly IPeopleService _peopleService;
    private readonly IUriService _uriService;
    private readonly string _attType, _attTypes;

    public PeopleApiController(IPeopleService peopleService, IUriService uriService)
    {
        _peopleService = peopleService ?? throw new ArgumentNullException(nameof(peopleService));
        _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        _attType = "person"; _attTypes = "people";
    }
      
    /****************************************************************
    * FETCH person records (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("people")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]
    
    public async Task<IActionResult> GetPeopleData( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedPersonData = await _peopleService.GetPaginatedPeopleDataAsync(validFilter);
            if (pagedPersonData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _peopleService.GetTotalPeople()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedPersonData,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var allPersonData = await _peopleService.GetPeopleDataAsync();
            return allPersonData != null
                ? Ok(ListSuccessResponse(allPersonData.Count, allPersonData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH person entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("people/entries")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]
    
    public async Task<IActionResult> GetPersonEntries( [FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(filter.pagenum, out var n) 
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedPersonEntries = await _peopleService.GetPaginatedPeopleEntriesAsync(validFilter);
            if (pagedPersonEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _peopleService.GetTotalPeople()).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedPersonEntries,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var allPersonEntries = await _peopleService.GetPeopleEntriesAsync();
            return allPersonEntries != null
                ? Ok(ListSuccessResponse(allPersonEntries.Count, allPersonEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered person set
    ****************************************************************/
    
    [HttpGet("people/name_contains/{nameFilter}")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]
    
    public async Task<IActionResult> GetPersonDataFiltered ( string nameFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _peopleService.GetPaginatedFilteredPeopleAsync(nameFilter, validFilter);
            if (pagedFilteredData != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _peopleService.GetTotalFilteredPeople(nameFilter)).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedFilteredData,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var filteredData = await _peopleService.GetFilteredPeopleAsync(nameFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH filtered person entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("people/entries/name_contains/{nameFilter}")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]  
    
    public async Task<IActionResult> GetPersonEntriesFiltered ( string nameFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } } 
            && int.TryParse(pageFilter.pagenum, out var n) 
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries = await _peopleService.GetPaginatedFilteredPeopleEntriesAsync(nameFilter, validFilter);
            if (pagedFilteredEntries != null)
            {
                var route = Request.Path.Value ?? "";
                var totalRecords = (await _peopleService.GetTotalFilteredPeople(nameFilter)).StatValue ?? 0;
                var pagedResponse = PagedResponseBuilder.CreatePagedResponse(pagedFilteredEntries,
                    validFilter, _uriService, totalRecords, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(NoAttributesResponse(_attTypes));
            }
        }
        else
        {
            var filteredEntries = await _peopleService.GetFilteredPeopleEntriesAsync(nameFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }
    
    /****************************************************************
    * FETCH n MOST RECENT person data (without attributes)
    ****************************************************************/
    
    [HttpGet("people/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]
    
    public async Task<IActionResult> GetRecentPersonData(int n)
    {
        var recentPersonData = await _peopleService.GetRecentPeopleAsync(n);
        return recentPersonData != null
            ? Ok(ListSuccessResponse(recentPersonData.Count, recentPersonData))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH n MOST RECENT person entries (id, sd_sid, name)
    ****************************************************************/
    
    [HttpGet("people/entries/recent/{n:int}")]
    [SwaggerOperation(Tags = new []{"Person data endpoint"})]
    
    public async Task<IActionResult> GetRecentPersonEntries(int n)
    {
        var recentPersonEntries = await _peopleService.GetRecentPeopleEntriesAsync(n);
        return recentPersonEntries != null
            ? Ok(ListSuccessResponse(recentPersonEntries.Count, recentPersonEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }
      
    /****************************************************************
    * FETCH data for a single person (including attribute data)
    ****************************************************************/
    /*
    [HttpGet("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"People endpoint"})]
    
    public async Task<IActionResult> GetFullPerson(string sd_sid)
    {
        var fullPerson = await _peopleService.GetFullPersonByIdAsync(sd_sid);
        return fullPerson != null
            ? Ok(SingleSuccessResponse(new List<FullPerson>() { fullPerson }))
            : Ok(NoEntityResponse(_fattType, sd_sid));
    }
    */
    
    /****************************************************************
    * DELETE an entire person record (with attributes)
    ****************************************************************/
    /*
    [HttpDelete("studies/{sd_sid}")]
    [SwaggerOperation(Tags = new []{"People endpoint"})]
    
    public async Task<IActionResult> DeleteFullPerson(string sd_sid)
    {
        if (await _peopleService.PersonExistsAsync(sd_sid)) {
            var count = await _peopleService.DeleteFullPersonAsync(sd_sid);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _fattType, "", sd_sid))
                : Ok(ErrorResponse("d", _fattType, "", "", sd_sid));
        } 
        return Ok(NoEntityResponse(_fattType, sd_sid));
    }
    */
    
    /****************************************************************
    * FETCH person statistics - total number of people
    ****************************************************************/

    [HttpGet("people/total")]
    [SwaggerOperation(Tags = new[] { "People data endpoint" })]

    public async Task<IActionResult> GetPeopleTotalNumber()
    {
        var stats = await _peopleService.GetTotalPeople();
        return stats.StatValue > 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }
    
    /****************************************************************
    * FETCH people statistics - number of people by role
    ****************************************************************/
    
    [HttpGet("people/by_role")]
    [SwaggerOperation(Tags = new[] { "People data endpoint" })]

    public async Task<IActionResult> GetStudiesByType()
    {
        var stats = await _peopleService.GetPeopleByRole();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }
    
    /****************************************************************
    * FETCH single person record (without attributes in other tables)
    ****************************************************************/
    
    [HttpGet("people/{id}/data")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]
    
    public async Task<IActionResult> GetPersonData(int id)
    {
        if (await _peopleService.PersonExistsAsync(id)) {
            var person = await _peopleService.GetPersonDataAsync(id);
            return person != null
                ? Ok(SingleSuccessResponse(new List<Person>() { person }))
                : Ok(ErrorResponse("r", _attType, "", id.ToString(), id.ToString()));
        }
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new person record (in people table only)
    ****************************************************************/

    [HttpPost("people")]
    [SwaggerOperation(Tags = new []{"People data endpoint"})]
    
    public async Task<IActionResult> CreatePersonData( 
                 [FromBody] Person personDataContent)
    {
        var newPersonData = await _peopleService.CreatePersonAsync(personDataContent);
        return newPersonData != null
            ? Ok(SingleSuccessResponse(new List<Person>() { newPersonData }))
            : Ok(ErrorResponse("c", _attType, "", "", ""));
    }
    
    /****************************************************************
    * UPDATE a specified person record (in people table only)
    ****************************************************************/

    [HttpPut("people/{id:int}")]
    [SwaggerOperation(Tags = new []{"Person data endpoint"})]
    
    public async Task<IActionResult> UpdatePerson(int id, 
                 [FromBody] Person personDataContent)
    {
        if (await _peopleService.PersonExistsAsync(id)) {
            var updatedPersonData = await _peopleService.UpdatePersonAsync(personDataContent);
            return (updatedPersonData != null)
                ? Ok(SingleSuccessResponse(new List<Person>() { updatedPersonData }))
                : Ok(ErrorResponse("u", _attType, "", id.ToString(), id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified person record (from people table only) 
    ****************************************************************/

    [HttpDelete("people/{id:int}")]
    [SwaggerOperation(Tags = new[] { "Person data endpoint" })]

    public async Task<IActionResult> DeletePerson(int id)
    {
        if (await _peopleService.PersonExistsAsync(id)) {
            var count = await _peopleService.DeletePersonAsync(id);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", id.ToString(), id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
}