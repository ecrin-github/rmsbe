using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.Services.Interfaces;
using rmsbe.SysModels;

namespace rmsbe.Controllers.Context;

public class TestDataApiController : BaseApiController
{
    private readonly ITestService _testService;

    public TestDataApiController(ITestService testService)
    {
        _testService = testService ?? throw new ArgumentNullException(nameof(testService));
    }

    /****************************************************************
    * Test related functions only, e.g. for use in Postman scripts
    ****************************************************************/

    /****************************************************************
    * Gets the current max identity value for a table
    ****************************************************************/
    
    [HttpGet("test/get_max_id/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> GetNextId(string tableName)
    {
        // returns a statistic, type 'maxValue', of the current max integer Id
        
        var stats = await _testService.GetMaxId(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", "max id", tableName, "", ""));
    }

    /****************************************************************
    * Stores recently added test record Ids in test_data table
    ****************************************************************/
    
    [HttpPost("test/data/{tableName}/{int oldMaxId:int}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> StoreTestDataDetails(string tableName, int oldMaxId)
    {
        // returns a statistic, type 'numNewRecs', of the number of records
        // stored in test_data table, (i.e. with id > max id from above, in this table)
        
        var stats = await _testService.StoreNewIds(tableName, oldMaxId);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", "max id", tableName, "", ""));
    }
    
    /****************************************************************
    * Deletes the listed test data for the specified table
    ****************************************************************/
    
    [HttpDelete("test/data/{tableName}/{int oldMaxId:int}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> DeleteTestData(string tableName, int oldMaxId)
    {
        // returns a statistic, type 'numDeletedRecs', of the number of records
        // deleted as being recognised as test data in this table
        
        var stats = await _testService.DeleteTestData(tableName, oldMaxId);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", "max id", tableName, "", ""));
    }

    /****************************************************************
    * Sets the next max identity value for a table
    ****************************************************************/
    
    [HttpPut("test/set_next_id/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> SetNextId(string tableName)
    {
        // returns a statistic, type 'newIdentityValue', representing the next
        // identity value to be applied in this table, after it has been set
        
        var stats = await _testService.SetNextId(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", "max id", tableName, "", ""));
    }

}