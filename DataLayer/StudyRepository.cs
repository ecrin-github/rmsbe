using System.Runtime.Loader;
using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;
using Dapper.Contrib.Extensions;

namespace rmsbe.DataLayer;

public class StudyRepository : IStudyRepository
{
    private readonly string _dbConnString;
    private readonly Dictionary<string, string> _typeList;
    
    public StudyRepository(ICredentials creds)
    {
        _dbConnString = creds.GetConnectionString("mdm");
        
        // set up dictionary of table name equivalents for type parameter
        _typeList = new Dictionary<string, string>
        {
            { "StudyTitles", "mdr.study_titles" },  
            { "StudyIdentifier", "mdr.study_identifiers" },           
            { "StudyContributor", "mdr.study_contributors" },
            { "StudyFeature", "mdr.study_features" },
            { "StudyTopic", "mdr.study_topics" },           
            { "StudyReference", "mdr.study_references" },
            { "StudyRelationship", "mdr.study_relationships" }
        };
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/

    public async Task<bool> StudyDoesNotExistAsync(string sd_sid)
    {
        string sqlString = $@"select not exists (select 1 from mdr.studies 
                              where sd_sid = '{sd_sid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> StudyExistsAsync(string sd_sid)
    {
        string sqlString = $@"select exists (select 1 from mdr.studies 
                              where sd_sid = '{sd_sid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> StudyAttributeDoesNotExistAsync(string sd_sid, string type_name, int id)
    {
        string sqlString = $@"select not exists (select 1 from {_typeList[type_name]}
                              where id = {id.ToString()} and sd_sid = '{sd_sid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> StudyAttributeExistsAsync(string sd_sid, string type_name, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[type_name]}
                              where id = {id.ToString()} and sd_sid = '{sd_sid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<FullStudyInDb?> GetFullStudyByIdAsync(string sd_sid)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $"select * from mdr.studies where sd_sid = '{sd_sid}'";   
        StudyInDb? coreStudy = await conn.QueryFirstOrDefaultAsync<StudyInDb>(sqlString);     
        sqlString = $"select * from mdr.study_contributors where sd_sid = '{sd_sid}'";
        var contribs = (await conn.QueryAsync<StudyContributorInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_features where sd_sid = '{sd_sid}'";
        var features = (await conn.QueryAsync<StudyFeatureInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_identifiers where sd_sid = '{sd_sid}'";
        var idents = (await conn.QueryAsync<StudyIdentifierInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_references where sd_sid = '{sd_sid}'";
        var refs = (await conn.QueryAsync<StudyReferenceInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_relationships where sd_sid = '{sd_sid}'";
        var rels = (await conn.QueryAsync<StudyRelationshipInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_titles where sd_sid = '{sd_sid}'";
        var titles = (await conn.QueryAsync<StudyTitleInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_topics where sd_sid = '{sd_sid}'";
        var topics = (await conn.QueryAsync<StudyTopicInDb>(sqlString)).ToList();
        
        return new FullStudyInDb(coreStudy, contribs, features, idents, refs, rels, titles, topics);
    } 
    
    // Update data
    public async Task<int> DeleteFullStudyAsync(string sd_sid, string user_name)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $@"update mdr.study_identifiers set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                        delete from mdr.study_identifiers where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_titles set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                       delete from mdr.study_titles where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_topics set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                       delete from mdr.study_topics where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_contributors set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                       delete from mdr.study_contributors where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);

        sqlString = $@"update mdr.study_features set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                       delete from mdr.study_features where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_references set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                      delete from mdr.study_references where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_relationships set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                       delete from mdr.study_relationships where sd_sid = '{sd_sid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.studies set last_edited_by = '{user_name}' where sd_sid = '{sd_sid}';
                       delete from mdr.studies where sd_sid = '{sd_sid}';";
        return await conn.ExecuteAsync(sqlString);
    }

    
    /****************************************************************
    * Study Record (studies table data only)
    ****************************************************************/
    
    // Fetch data
    
    public async Task<IEnumerable<StudyInDb>> GetStudiesDataAsync()
    {
        string sqlString = $"select * from mdr.studies";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }

    public async Task<IEnumerable<StudyInDb>> GetRecentStudyDataAsync(int n)
    {
        string sqlString = $@"select * from mdr.studies 
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }

    public async Task<StudyInDb?> GetStudyDataAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.studies where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyInDb?> CreateStudyDataAsync(StudyInDb studyData)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyData);
        string sqlString = $"select * from mdr.studies where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyInDb>(sqlString);
    }

    public async Task<StudyInDb?> UpdateStudyDataAsync(StudyInDb studyData)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyData)) ? studyData : null;
    }

    public async Task<int> DeleteStudyDataAsync(string sd_sid, string user_name)
    {
        string sqlString = $@"update mdr.studies 
                              set last_edited_by = {user_name}
                              where sd_sid = '{sd_sid}';
                              delete from mdr.studies 
                              where sd_sid = '{sd_sid}';";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    
    /****************************************************************
    * Study contributors
    ****************************************************************/  
    
    // Fetch data
    
    public async Task<IEnumerable<StudyContributorInDb>> GetStudyContributorsAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_contributors where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyContributorInDb>(sqlString);
    }

    public async Task<StudyContributorInDb?> GetStudyContributorAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_contributors where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyContributorInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyContributorInDb?> CreateStudyContributorAsync(StudyContributorInDb studyContributorInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyContributorInDb);
        string sqlString = $"select * from mdr.study_contributors where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyContributorInDb>(sqlString);
    }

    public async Task<StudyContributorInDb?> UpdateStudyContributorAsync(StudyContributorInDb studyContributorInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyContributorInDb)) ? studyContributorInDb : null;
    }

    public async Task<int> DeleteStudyContributorAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_contributors 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_contributors 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }


    /****************************************************************
    * Study features
    ****************************************************************/
    
    // Fetch data
    
    public async  Task<IEnumerable<StudyFeatureInDb>> GetStudyFeaturesAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_features where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyFeatureInDb>(sqlString);
    }

    public async Task<StudyFeatureInDb?> GetStudyFeatureAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_features where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyFeatureInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyFeatureInDb?> CreateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyFeatureInDb);
        string sqlString = $"select * from mdr.study_features where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyFeatureInDb>(sqlString);
    }

    public async Task<StudyFeatureInDb?> UpdateStudyFeatureAsync(StudyFeatureInDb studyFeatureInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyFeatureInDb)) ? studyFeatureInDb : null;
    }

    public async Task<int> DeleteStudyFeatureAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_features 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_features 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Study identifiers
    ****************************************************************/
    
    // Fetch data
    
    public async Task<IEnumerable<StudyIdentifierInDb>> GetStudyIdentifiersAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_identifiers where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyIdentifierInDb>(sqlString);
    }

    public async Task<StudyIdentifierInDb?> GetStudyIdentifierAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_identifiers where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyIdentifierInDb>(sqlString);
    }
    
    // Update data
    
    public async Task<StudyIdentifierInDb?> CreateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyIdentifierInDb);
        string sqlString = $"select * from mdr.study_identifiers where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyIdentifierInDb>(sqlString);
    }

    public async Task<StudyIdentifierInDb?> UpdateStudyIdentifierAsync(StudyIdentifierInDb studyIdentifierInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyIdentifierInDb)) ? studyIdentifierInDb : null;
    }

    public async Task<int> DeleteStudyIdentifierAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_identifiers 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_identifiers 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }


    /****************************************************************
    * Study References
    ****************************************************************/
    
    // Fetch data
    
    public async Task<IEnumerable<StudyReferenceInDb>> GetStudyReferencesAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_references where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyReferenceInDb>(sqlString);
    }

    public async Task<StudyReferenceInDb?> GetStudyReferenceAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_references where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyReferenceInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyReferenceInDb?> CreateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyReferenceInDb);
        string sqlString = $"select * from mdr.study_references where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyReferenceInDb>(sqlString);
    }

    public async Task<StudyReferenceInDb?> UpdateStudyReferenceAsync(StudyReferenceInDb studyReferenceInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyReferenceInDb)) ? studyReferenceInDb : null;
    }

    public async Task<int> DeleteStudyReferenceAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_references 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_references 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }


    /****************************************************************
    * Study Relationships
    ****************************************************************/
    
    // Fetch data
    
    public async Task<IEnumerable<StudyRelationshipInDb>> GetStudyRelationshipsAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_relationships where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyRelationshipInDb>(sqlString);
    }

    public async Task<StudyRelationshipInDb?> GetStudyRelationshipAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_relationships where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyRelationshipInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyRelationshipInDb?> CreateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyRelationshipInDb);
        string sqlString = $"select * from mdr.study_relationships where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyRelationshipInDb>(sqlString);
    }

    public async Task<StudyRelationshipInDb?> UpdateStudyRelationshipAsync(StudyRelationshipInDb studyRelationshipInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyRelationshipInDb)) ? studyRelationshipInDb : null;
    }

    public async Task<int> DeleteStudyRelationshipAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_relationships 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_relationships 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }


    /****************************************************************
    * Study titles
    ****************************************************************/  
    
    // Fetch data
    
    public async Task<IEnumerable<StudyTitleInDb>> GetStudyTitlesAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_titles where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyTitleInDb>(sqlString);
    }

    public async Task<StudyTitleInDb?> GetStudyTitleAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_titles where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyTitleInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyTitleInDb?> CreateStudyTitleAsync(StudyTitleInDb studyTitleInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyTitleInDb);
        string sqlString = $"select * from mdr.study_titles where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyTitleInDb>(sqlString);
    }

    public async Task<StudyTitleInDb?> UpdateStudyTitleAsync(StudyTitleInDb studyTitleInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyTitleInDb)) ? studyTitleInDb : null;
    }

    public async Task<int> DeleteStudyTitleAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_titles 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_titles 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }


    /****************************************************************
    * Study topics
    ****************************************************************/
    
    // Fetch data
    
    public async Task<IEnumerable<StudyTopicInDb>> GetStudyTopicsAsync(string sd_sid)
    {
        string sqlString = $"select * from mdr.study_topics where sd_sid = '{sd_sid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyTopicInDb>(sqlString);
    }

    public async Task<StudyTopicInDb?> GetStudyTopicAsync(int? id)
    {
        string sqlString = $"select * from mdr.study_topics where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyTopicInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyTopicInDb?> CreateStudyTopicAsync(StudyTopicInDb studyTopicInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyTopicInDb);
        string sqlString = $"select * from mdr.study_topics where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyTopicInDb>(sqlString);
    }

    public async Task<StudyTopicInDb?> UpdateStudyTopicAsync(StudyTopicInDb studyTopicInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyTopicInDb)) ? studyTopicInDb : null;
    }

    public async Task<int> DeleteStudyTopicAsync(int id, string user_name)
    {
        string sqlString = $@"update mdr.study_topics 
                              set last_edited_by = '{user_name}'
                              where id = {id.ToString()};
                              delete from mdr.study_topics 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    // Extensions
    /*
    Task<PaginationResponse<StudyInDb>> PaginateStudies(PaginationRequest paginationRequest);
    Task<PaginationResponse<StudyInDb>> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest);
    Task<int> GetTotalStudies();
    */
}
