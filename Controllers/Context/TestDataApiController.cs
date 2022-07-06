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
    * TEST RELATED functions, e.g. for use in Postman scripts
    ****************************************************************/

    
    /****************************************************************
    * Gets the current total of records for a table
    ****************************************************************/
    
    [HttpGet("test/get_total/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> GetTotal(string tableName)
    {
        // returns a statistic, type 'Total', of the current max integer Id
        
        var stats = await _testService.GetTotal(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", "max id", tableName, "", ""));
    }
    
        
    /****************************************************************
    * Gets the current max identity value for a table, and also
    * stores it for future use
    ****************************************************************/
    
    [HttpGet("test/get_max_id/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> GetMaxId(string tableName)
    {
        // returns a statistic, type 'maxValue', of the current max integer Id
        
        var stats = await _testService.GetMaxId(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("r", "total", tableName, "", ""));
    }
    
    
    /****************************************************************
    * Stores recently added test record Ids in test_data table
    ****************************************************************/
    
    [HttpPost("test/data/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> StoreTestDataDetails(string tableName)
    {
        // returns a statistic, type 'numNewRecs', of the number of records
        // stored in test_data table, (i.e. with id > max id from above, in this table)
        
        var stats = await _testService.StoreNewIds(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("c", "num stored", tableName, "", ""));
    }
    
    
    /****************************************************************
    * Deletes the listed test data for the specified table
    ****************************************************************/
    
    [HttpDelete("test/data/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> DeleteTestData(string tableName)
    {
        // returns a statistic, type 'numDeletedRecs', of the number of records
        // deleted as being recognised as test data in this table
        
        var stats = await _testService.DeleteTestData(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("d", "num deleted", tableName, "", ""));
    }

    
    /****************************************************************
    * Resets the next max identity value for a table, to be one
    * greater than the current max id
    ****************************************************************/
    
    [HttpPut("test/set_next_id/{tableName}")]
    [SwaggerOperation(Tags = new[] { "Test data endpoint" })]

    public async Task<IActionResult> ResetIdentitySequence(string tableName)
    {
        // returns a statistic, type 'newIdentityValue', representing the next
        // identity value to be applied in this table, after it has been set
        
        var stats = await _testService.ResetIdentitySequence(tableName);
        return stats.StatValue >= 0
            ? Ok(SingleSuccessResponse(new List<Statistic>() { stats }))
            : Ok(ErrorResponse("u", "reset identity", tableName, "", ""));
    }

}