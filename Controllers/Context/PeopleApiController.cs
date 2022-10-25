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
        _attType = "person";
        _attTypes = "people";
    }

    /****************************************************************
    * FETCH person records (without attributes in other tables)
    ****************************************************************/

    [HttpGet("people/data")]  
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPeopleData([FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } }
            && int.TryParse(filter.pagenum, out var n)
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedPersonData = await _peopleService.GetPaginatedPeople(validFilter);
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
            var allPersonData = await _peopleService.GetAllPeopleData();
            return allPersonData != null
                ? Ok(ListSuccessResponse(allPersonData.Count, allPersonData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }

    /****************************************************************
    * FETCH person list entries (id, sd_sid, name)
    ****************************************************************/

    [HttpGet("people/list")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPersonEntries([FromQuery] PaginationQuery? filter)
    {
        if (filter is { pagesize: { }, pagenum: { } }
            && int.TryParse(filter.pagenum, out var n)
            && int.TryParse(filter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedPersonEntries = await _peopleService.GetPaginatedPeopleEntries(validFilter);
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
            var allPersonEntries = await _peopleService.GetAllPeopleEntries();
            return allPersonEntries != null
                ? Ok(ListSuccessResponse(allPersonEntries.Count, allPersonEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }

    /****************************************************************
    * FETCH filtered person set
    ****************************************************************/

    [HttpGet("people/data/name-contains/{nameFilter}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPersonDataFiltered(string nameFilter, [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } }
            && int.TryParse(pageFilter.pagenum, out var n)
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredData = await _peopleService.GetPaginatedFilteredPeople(nameFilter, validFilter);
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
            var filteredData = await _peopleService.GetFilteredPeople(nameFilter);
            return filteredData != null
                ? Ok(ListSuccessResponse(filteredData.Count, filteredData))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }

    /****************************************************************
    * FETCH filtered person list (id, sd_sid, name)
    ****************************************************************/

    [HttpGet("people/list/name-contains/{nameFilter}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPersonEntriesFiltered(string nameFilter,
        [FromQuery] PaginationQuery? pageFilter)
    {
        if (pageFilter is { pagesize: { }, pagenum: { } }
            && int.TryParse(pageFilter.pagenum, out var n)
            && int.TryParse(pageFilter.pagesize, out var s))
        {
            var validFilter = new PaginationRequest(n, s);
            var pagedFilteredEntries =
                await _peopleService.GetPaginatedFilteredPeopleEntries(nameFilter, validFilter);
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
            var filteredEntries = await _peopleService.GetFilteredPeopleEntries(nameFilter);
            return filteredEntries != null
                ? Ok(ListSuccessResponse(filteredEntries.Count, filteredEntries))
                : Ok(NoAttributesResponse(_attTypes));
        }
    }

    
    /****************************************************************
    * FETCH People records linked to an organisation
    ****************************************************************/ 

    [HttpGet("people/data/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"People endpoint"})]
    
    public async Task<IActionResult> GetDtpsByOrg(int orgId)
    {
        var peopleByOrg = await _peopleService.GetPeopleByOrg(orgId);
        return peopleByOrg != null
            ? Ok(ListSuccessResponse(peopleByOrg.Count, peopleByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }
    
    /****************************************************************
    * FETCH People list linked to an organisation
    ****************************************************************/
    
    [HttpGet("people/list/by-org/{orgId:int}")]
    [SwaggerOperation(Tags = new []{"People endpoint"})]
    
    public async Task<IActionResult> GetDtpEntriesByOrg(int orgId)
    {
        var personEntriesByOrg = await _peopleService.GetPeopleEntriesByOrg(orgId);
        return personEntriesByOrg != null
            ? Ok(ListSuccessResponse(personEntriesByOrg.Count, personEntriesByOrg))
            : Ok(NoAttributesResponse(_attTypes));
    }

    
    /****************************************************************
    * FETCH n MOST RECENT person data (without attributes)
    ****************************************************************/

    [HttpGet("people/data/recent/{n:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetRecentPersonData(int n)
    {
        var recentPersonData = await _peopleService.GetRecentPeople(n);
        return recentPersonData != null
            ? Ok(ListSuccessResponse(recentPersonData.Count, recentPersonData))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH n MOST RECENT person entries (id, sd_sid, name)
    ****************************************************************/

    [HttpGet("people/list/recent/{n:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetRecentPersonEntries(int n)
    {
        var recentPersonEntries = await _peopleService.GetRecentPeopleEntries(n);
        return recentPersonEntries != null
            ? Ok(ListSuccessResponse(recentPersonEntries.Count, recentPersonEntries))
            : Ok(NoAttributesResponse(_attTypes));
    }

    /****************************************************************
    * FETCH data for a single person (including attribute data)
    ****************************************************************/
    
    [HttpGet("people/full/{id:int}")]
    [SwaggerOperation(Tags = new []{"People endpoint"})]
    
    public async Task<IActionResult> GetFullPerson(int id)
    {
        var fullPerson = await _peopleService.GetFullPersonById(id);
        return fullPerson != null
            ? Ok(SingleSuccessResponse(new List<FullPerson>() { fullPerson }))
            : Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * DELETE an entire person record (with attributes)
    ****************************************************************/

    [HttpDelete("people/full/{id:int}")]
    [SwaggerOperation(Tags = new []{"People endpoint"})]
    
    public async Task<IActionResult> DeleteFullPerson(int id)
    {
        if (await _peopleService.PersonExists(id)) {
            var count = await _peopleService.DeleteFullPerson(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, "", id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", "", id.ToString()));
        } 
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }

    /****************************************************************
    * FETCH person statistics - total number of people
    ****************************************************************/

    [HttpGet("people/total")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPeopleTotalNumber()
    {
        var stats = await _peopleService.GetTotalPeople();
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", _attType, "", "", "total numbers"));
    }

    /****************************************************************
    * FETCH people statistics - number of people by role
    ****************************************************************/

    [HttpGet("people/by-role")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPeopleByRole()
    {
        var stats = await _peopleService.GetPeopleByRole();
        return stats != null
            ? Ok(ListSuccessResponse(stats.Count, stats))
            : Ok(ErrorResponse("r", _attType, "", "", "numbers by type"));
    }
    
    /****************************************************************
    * FETCH involvement statistics - number of DTPs, DUPs
    * an individual person is / has been involved in
    ****************************************************************/

    [HttpGet("people/involvement/{id:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]
    
    public async Task<IActionResult> GetPersonInvolvement(int id)
    {
        if (await _peopleService.PersonExists(id))
        {
            var stats = await _peopleService.GetPersonInvolvement(id);
            return Ok(ListSuccessResponse(stats.Count, stats));
        }
        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * FETCH single person record (without attributes in other tables)
    ****************************************************************/

    [HttpGet("people/{id:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> GetPersonData(int id)
    {
        if (await _peopleService.PersonExists(id))
        {
            var person = await _peopleService.GetPersonData(id);
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
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> CreatePersonData(
        [FromBody] Person personDataContent)
    {
        var newPersonData = await _peopleService.CreatePerson(personDataContent);
        return newPersonData != null
            ? Ok(SingleSuccessResponse(new List<Person>() { newPersonData }))
            : Ok(ErrorResponse("c", _attType, "", "", ""));
    }

    /****************************************************************
    * UPDATE a specified person record (in people table only)
    ****************************************************************/

    [HttpPut("people/{id:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> UpdatePerson(int id,
                 [FromBody] Person personDataContent)
    {
        if (await _peopleService.PersonExists(id))
        {
            personDataContent.Id = id;
            var updatedPersonData = await _peopleService.UpdatePerson(personDataContent);
            return (updatedPersonData != null)
                ? Ok(SingleSuccessResponse(new List<Person>() { updatedPersonData }))
                : Ok(ErrorResponse("u", _attType, "", id.ToString(), id.ToString()));
        }

        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * CHECK if a person is a signatory to a DUA or DUP
    ****************************************************************/

    [HttpDelete("people/check-if-signatory/{id:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> CheckPersonIsSignatory(int id)
    {
        if (await _peopleService.PersonExists(id))
        {
            var stats = await _peopleService.PersonIsSignatory(id);
            return Ok(ListSuccessResponse(stats.Count, stats));
        }

        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a specified person record (from people table only) 
    ****************************************************************/

    [HttpDelete("people/{id:int}")]
    [SwaggerOperation(Tags = new[] { "People endpoint" })]

    public async Task<IActionResult> DeletePerson(int id)
    {
        if (await _peopleService.PersonExists(id))
        {
            var count = await _peopleService.DeletePerson(id);
            return (count > 0)
                ? Ok(DeletionSuccessResponse(count, _attType, "", id.ToString()))
                : Ok(ErrorResponse("d", _attType, "", id.ToString(), id.ToString()));
        }

        return Ok(NoEntityResponse(_attType, id.ToString()));
    }
}