using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper;
using Dapper.Contrib.Extensions;

namespace rmsbe.DataLayer;

public class StudyRepository : IStudyRepository
{
    private readonly string _dbConnString, _dbMdrConnString;
    private readonly Dictionary<string, string> _typeList;
    
    public StudyRepository(ICreds creds)
    {
        _dbConnString = creds.GetConnectionString("mdm");
        _dbMdrConnString = creds.GetConnectionString("mdr");
        
        // set up dictionary of table name equivalents for type parameter
        _typeList = new Dictionary<string, string>
        {
            { "StudyTitle", "mdr.study_titles" },  
            { "StudyIdentifier", "mdr.study_identifiers" },           
            { "StudyContributor", "mdr.study_contributors" },
            { "StudyFeature", "mdr.study_features" },
            { "StudyTopic", "mdr.study_topics" },           
            { "StudyReference", "mdr.study_references" },
            { "StudyRelationship", "mdr.study_relationships" }
        };
        
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
    
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    public async Task<bool> StudyExists(string sdSid)
    {
        string sqlString = $@"select exists (select 1 from mdr.studies 
                              where sd_sid = '{sdSid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    public async Task<bool> StudyAttributeExists(string sdSid, string typeName, int id)
    {
        string sqlString = $@"select exists (select 1 from {_typeList[typeName]}
                              where id = {id.ToString()} and sd_sid = '{sdSid}')";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    /****************************************************************
    * All Study Records and study entries
    ****************************************************************/
    
    public async Task<IEnumerable<StudyInDb>> GetAllStudyRecords()
    {
        string sqlString = $"select * from mdr.studies";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }
    
    public async Task<IEnumerable<StudyEntryInDb>> GetAllStudyEntries()
    {
        string sqlString = $"select id, sd_sid, display_title from mdr.studies";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Paginated Study Records and study entries
    ****************************************************************/    
    
    public async Task<IEnumerable<StudyInDb>> GetPaginatedStudyRecords(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from mdr.studies 
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }
    
    public async Task<IEnumerable<StudyEntryInDb>> GetPaginatedStudyEntries(int pNum, int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select id, sd_sid, display_title from mdr.studies 
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Filtered Study Records and study entries
    ****************************************************************/    
    
    public async Task<IEnumerable<StudyInDb>> GetFilteredStudyRecords(string titleFilter)
    {
        string sqlString = $@"select * from mdr.studies
                            where display_title ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }
    
    public async Task<IEnumerable<StudyEntryInDb>> GetFilteredStudyEntries(string titleFilter)
    {
        string sqlString = $@"select id, sd_sid, display_title from mdr.studies
                            where display_title ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Paginated and filtered Study Records and study entries
    ****************************************************************/    
    
    public async Task<IEnumerable<StudyInDb>> GetPaginatedFilteredStudyRecords(string titleFilter, int pNum,
        int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select * from mdr.studies 
                              where display_title ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }
    
    public async Task<IEnumerable<StudyEntryInDb>> GetPaginatedFilteredStudyEntries(string titleFilter, int pNum,
        int pSize)
    {
        int offset = pNum == 1 ? 0 : (pNum - 1) * pSize;
        string sqlString = $@"select id, sd_sid, display_title from mdr.studies 
                              where display_title ilike '%{titleFilter}%'
                              order by created_on DESC
                              offset {offset.ToString()}
                              limit {pSize.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyEntryInDb>(sqlString);
    }

    /****************************************************************
    * Recent Study Records and study entries
    ****************************************************************/   

    public async Task<IEnumerable<StudyInDb>> GetRecentStudyRecords(int n)
    {
        string sqlString = $@"select * from mdr.studies 
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }

    public async Task<IEnumerable<StudyEntryInDb>> GetRecentStudyEntries(int n)
    {
        string sqlString = $@"select id, sd_sid, display_title from mdr.studies 
                              order by created_on DESC
                              limit {n.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Study Records and study entries by Organisation
    ****************************************************************/    
    
    public async Task<IEnumerable<StudyInDb>> GetStudyRecordsByOrg(int orgId)
    {
        var sqlString = $@"select b.* from mdr.studies b
                              inner join 
                                 (select db.study_id from rms.dtp_studies db
                                  inner join rms.dtps d 
                                  on db.dtp_id = d.id
                                  where d.org_id = {orgId.ToString()}
                                  union 
                                  select du.study_id from rms.dup_studies du
                                  inner join rms.dups s 
                                  on du.dup_id = s.id
                                  where s.org_id = {orgId.ToString()}) u
                              on b.sd_sid = u.study_id
                              order by b.created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyInDb>(sqlString);
    }

    public async Task<IEnumerable<StudyEntryInDb>> GetStudyEntriesByOrg(int orgId)
    {
        var sqlString = $@"select id, sd_sid, display_title from mdr.studies b
                              inner join 
                                 (select db.study_id from rms.dtp_studies db
                                  inner join rms.dtps d 
                                  on db.dtp_id = d.id
                                  where d.org_id = {orgId.ToString()}
                                  union 
                                  select du.study_id from rms.dup_studies du
                                  inner join rms.dups s 
                                  on du.dup_id = s.id
                                  where s.org_id = {orgId.ToString()}) u
                              on b.sd_sid = u.study_id
                              order by b.created_on DESC";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyEntryInDb>(sqlString);
    }
    
    /****************************************************************
    * Fetch a single Study record 
    ****************************************************************/    
    
    public async Task<StudyInDb?> GetStudyData(string sdSid)
    {
        string sqlString = $"select * from mdr.studies where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyInDb>(sqlString);
    }

    /****************************************************************
    * Update Study record data
    ****************************************************************/
    
    public async Task<StudyInDb?> CreateStudyData(StudyInDb studyData)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyData);
        string sqlString = $"select * from mdr.studies where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyInDb>(sqlString);
    }

    public async Task<StudyInDb?> UpdateStudyData(StudyInDb studyData)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyData)) ? studyData : null;
    }

    public async Task<int> DeleteStudyData(string sdSid, string userName)
    {
        string sqlString = $@"update mdr.studies 
                              set last_edited_by = {userName}
                              where sd_sid = '{sdSid}';
                              delete from mdr.studies 
                              where sd_sid = '{sdSid}';";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }

    /****************************************************************
    * Full Study data (including attributes in other tables)
    ****************************************************************/
    
    public async Task<FullStudyInDb?> GetFullStudyById(string sdSid)
    {
        // fetch data
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $"select * from mdr.studies where sd_sid = '{sdSid}'";   
        StudyInDb? coreStudy = await conn.QueryFirstOrDefaultAsync<StudyInDb>(sqlString);     
        sqlString = $"select * from mdr.study_contributors where sd_sid = '{sdSid}'";
        var contribs = (await conn.QueryAsync<StudyContributorInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_features where sd_sid = '{sdSid}'";
        var features = (await conn.QueryAsync<StudyFeatureInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_identifiers where sd_sid = '{sdSid}'";
        var idents = (await conn.QueryAsync<StudyIdentifierInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_relationships where sd_sid = '{sdSid}'";
        var rels = (await conn.QueryAsync<StudyRelationshipInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_titles where sd_sid = '{sdSid}'";
        var titles = (await conn.QueryAsync<StudyTitleInDb>(sqlString)).ToList();
        sqlString = $"select * from mdr.study_topics where sd_sid = '{sdSid}'";
        var topics = (await conn.QueryAsync<StudyTopicInDb>(sqlString)).ToList();
        
        return new FullStudyInDb(coreStudy, contribs, features, idents, rels, titles, topics);
    } 
    
    // delete data
    public async Task<int> DeleteFullStudy(string sdSid, string userName)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var sqlString = $@"update mdr.study_identifiers set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                        delete from mdr.study_identifiers where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_titles set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                       delete from mdr.study_titles where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_topics set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                       delete from mdr.study_topics where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_contributors set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                       delete from mdr.study_contributors where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);

        sqlString = $@"update mdr.study_features set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                       delete from mdr.study_features where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_references set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                      delete from mdr.study_references where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.study_relationships set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                       delete from mdr.study_relationships where sd_sid = '{sdSid}';";
        await conn.ExecuteAsync(sqlString);
        
        sqlString = $@"update mdr.studies set last_edited_by = '{userName}' where sd_sid = '{sdSid}';
                       delete from mdr.studies where sd_sid = '{sdSid}';";
        return await conn.ExecuteAsync(sqlString);
    }
    
    /****************************************************************
    * Study details from the MDR
    ****************************************************************/
    
    public async Task<StudyMdrDetails?> GetStudyDetailsFromMdr(int regId, string sdSid)
    {
        await using var conn = new NpgsqlConnection(_dbMdrConnString);
        string sqlString = $@"select study_id from nk.study_ids
                            where source_id = {regId.ToString()}
                            and sd_sid = '{sdSid}';";
        int? studyId = await conn.QueryFirstOrDefaultAsync<int?>(sqlString);
        if (studyId != null)
        {
            sqlString = $@"select study_id, source_id, sd_sid from nk.study_ids
                            where study_id = {studyId.ToString()}
                            and is_preferred = true;";
            return await conn.QueryFirstOrDefaultAsync<StudyMdrDetails>(sqlString);
        }
        else
        {
            return null;
        }
    }
    
    
    public async Task<StudyInMdr?> GetStudyDataFromMdr(int mdrId)
    {
        await using var conn = new NpgsqlConnection(_dbMdrConnString);
        var sqlString = $"select * from core.studies where id = {mdrId.ToString()}";   
        return await conn.QueryFirstOrDefaultAsync<StudyInMdr>(sqlString); 
    }
    
    
    public async Task<FullStudyFromMdrInDb?> GetFullStudyDataFromMdr(StudyInDb importedStudy, int mdrId)
    {
        if (importedStudy.sd_sid == null) return null;
        var sdSid = importedStudy.sd_sid;
        await using var mdrConn = new NpgsqlConnection(_dbMdrConnString);
        
        string sqlString = $"select * from core.study_contributors where study_id = {mdrId.ToString()}";
        var contribs = (await mdrConn.QueryAsync<StudyContributorInMdr>(sqlString)).ToList();
        sqlString = $"select * from core.study_features where study_id = {mdrId.ToString()}";
        var features = (await mdrConn.QueryAsync<StudyFeatureInMdr>(sqlString)).ToList();
        sqlString = $"select * from core.study_identifiers where study_id = {mdrId.ToString()}";
        var idents = (await mdrConn.QueryAsync<StudyIdentifierInMdr>(sqlString)).ToList();
        sqlString = $"select * from core.study_titles where study_id = {mdrId.ToString()}";
        var titles = (await mdrConn.QueryAsync<StudyTitleInMdr>(sqlString)).ToList();
        sqlString = $"select * from core.study_topics where study_id = {mdrId.ToString()}";
        var topics = (await mdrConn.QueryAsync<StudyTopicInMdr>(sqlString)).ToList();
        sqlString = $@"select b.id as id, 
                    case when k.object_type_id = 12 then
                       '{sdSid}'||'::'||k.object_type_id::varchar||'::'||k.sd_oid
                    else 
                       '{sdSid}'||'::'||k.object_type_id::varchar||'::'||k.title 
                    end as sd_oid, 
                    '{sdSid}' as sd_sid, b.display_title 
                    from nk.data_object_ids k
                    inner join core.data_objects b
                    on k.object_id = b.id
                    where k.parent_study_id = {mdrId.ToString()}
                    and b.object_type_id not in (13, 28, 86, 134)";
        var linkedObjects = (await mdrConn.QueryAsync<DataObjectEntryInDb>(sqlString)).ToList();
   
        await using var conn = new NpgsqlConnection(_dbConnString);
        
        var userName = importedStudy.last_edited_by;
        
        List<StudyContributorInDb>? contribsInDb = null;
        if (contribs.Any())
        {
            contribsInDb = contribs.Select(c => new StudyContributorInDb(c, sdSid)).ToList();
            foreach (var cdb in contribsInDb)
            {
                cdb.last_edited_by = userName;
                cdb.id = await conn.InsertAsync(cdb);
            }
        }
        
        List<StudyFeatureInDb>? featuresInDb = null;
        if (features.Any())
        {
            featuresInDb = features.Select(c => new StudyFeatureInDb(c, sdSid)).ToList();
            foreach (var cdb in featuresInDb)
            {
                cdb.last_edited_by = userName;
                cdb.id = await conn.InsertAsync(cdb);
            }
        }
        
        List<StudyIdentifierInDb>? identsInDb = null;
        if (idents.Any())
        {
            identsInDb = idents.Select(c => new StudyIdentifierInDb(c, sdSid)).ToList();
            foreach (var cdb in identsInDb)
            {
                cdb.last_edited_by = userName;
                cdb.id = await conn.InsertAsync(cdb);
            }
        }

        List<StudyTitleInDb>? titlesInDb = null;
        if (titles.Any())
        {
            titlesInDb = titles.Select(c => new StudyTitleInDb(c, sdSid)).ToList();
            foreach (var cdb in titlesInDb)
            {
                cdb.last_edited_by = userName;
                cdb.id = await conn.InsertAsync(cdb);
            }
        }
        
        List<StudyTopicInDb>? topicsInDb = null;
        if (topics.Any())
        {
            topicsInDb = topics.Select(c => new StudyTopicInDb(c, sdSid)).ToList();
            foreach (var cdb in topicsInDb)
            {
                cdb.last_edited_by = userName;
                cdb.id = await conn.InsertAsync(cdb);
            }
        }
        return new FullStudyFromMdrInDb(importedStudy, contribsInDb, featuresInDb, identsInDb, titlesInDb, 
            topicsInDb, linkedObjects);
    }

    
    /****************************************************************
    * Study statistics
    ****************************************************************/
    
    public async Task<int> GetTotalStudies()
    {
        string sqlString = $@"select count(*) from mdr.studies;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<int> GetTotalFilteredStudies(string titleFilter)
    {
        string sqlString = $@"select count(*) from mdr.studies
                             where display_title ilike '%{titleFilter}%'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteScalarAsync<int>(sqlString);
    }
    
    public async Task<IEnumerable<StatisticInDb>> GetStudiesByType()
    {
        string sqlString = $@"select study_type_id as stat_type, 
                             count(id) as stat_value 
                             from mdr.studies group by study_type_id;";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StatisticInDb>(sqlString);
    }
    
    /****************************************************************
    * Study contributors
    ****************************************************************/  
    
    // Fetch data
    
    public async Task<IEnumerable<StudyContributorInDb>> GetStudyContributors(string sdSid)
    {
        string sqlString = $"select * from mdr.study_contributors where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyContributorInDb>(sqlString);
    }

    public async Task<StudyContributorInDb?> GetStudyContributor(int? id)
    {
        string sqlString = $"select * from mdr.study_contributors where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyContributorInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyContributorInDb?> CreateStudyContributor(StudyContributorInDb studyContributorInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyContributorInDb);
        string sqlString = $"select * from mdr.study_contributors where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyContributorInDb>(sqlString);
    }

    public async Task<StudyContributorInDb?> UpdateStudyContributor(StudyContributorInDb studyContributorInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyContributorInDb)) ? studyContributorInDb : null;
    }

    public async Task<int> DeleteStudyContributor(int id, string userName)
    {
        string sqlString = $@"update mdr.study_contributors 
                              set last_edited_by = '{userName}'
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
    
    public async  Task<IEnumerable<StudyFeatureInDb>> GetStudyFeatures(string sdSid)
    {
        string sqlString = $"select * from mdr.study_features where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyFeatureInDb>(sqlString);
    }

    public async Task<StudyFeatureInDb?> GetStudyFeature(int? id)
    {
        string sqlString = $"select * from mdr.study_features where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyFeatureInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyFeatureInDb?> CreateStudyFeature(StudyFeatureInDb studyFeatureInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyFeatureInDb);
        string sqlString = $"select * from mdr.study_features where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyFeatureInDb>(sqlString);
    }

    public async Task<StudyFeatureInDb?> UpdateStudyFeature(StudyFeatureInDb studyFeatureInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyFeatureInDb)) ? studyFeatureInDb : null;
    }

    public async Task<int> DeleteStudyFeature(int id, string userName)
    {
        string sqlString = $@"update mdr.study_features 
                              set last_edited_by = '{userName}'
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
    
    public async Task<IEnumerable<StudyIdentifierInDb>> GetStudyIdentifiers(string sdSid)
    {
        string sqlString = $"select * from mdr.study_identifiers where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyIdentifierInDb>(sqlString);
    }

    public async Task<StudyIdentifierInDb?> GetStudyIdentifier(int? id)
    {
        string sqlString = $"select * from mdr.study_identifiers where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyIdentifierInDb>(sqlString);
    }
    
    // Update data
    
    public async Task<StudyIdentifierInDb?> CreateStudyIdentifier(StudyIdentifierInDb studyIdentifierInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyIdentifierInDb);
        string sqlString = $"select * from mdr.study_identifiers where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyIdentifierInDb>(sqlString);
    }

    public async Task<StudyIdentifierInDb?> UpdateStudyIdentifier(StudyIdentifierInDb studyIdentifierInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyIdentifierInDb)) ? studyIdentifierInDb : null;
    }

    public async Task<int> DeleteStudyIdentifier(int id, string userName)
    {
        string sqlString = $@"update mdr.study_identifiers 
                              set last_edited_by = '{userName}'
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
    
    public async Task<IEnumerable<StudyReferenceInDb>> GetStudyReferences(string sdSid)
    {
        string sqlString = $"select * from mdr.study_references where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyReferenceInDb>(sqlString);
    }

    public async Task<StudyReferenceInDb?> GetStudyReference(int? id)
    {
        string sqlString = $"select * from mdr.study_references where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyReferenceInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyReferenceInDb?> CreateStudyReference(StudyReferenceInDb studyReferenceInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyReferenceInDb);
        string sqlString = $"select * from mdr.study_references where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyReferenceInDb>(sqlString);
    }

    public async Task<StudyReferenceInDb?> UpdateStudyReference(StudyReferenceInDb studyReferenceInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyReferenceInDb)) ? studyReferenceInDb : null;
    }

    public async Task<int> DeleteStudyReference(int id, string userName)
    {
        string sqlString = $@"update mdr.study_references 
                              set last_edited_by = '{userName}'
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
    
    public async Task<IEnumerable<StudyRelationshipInDb>> GetStudyRelationships(string sdSid)
    {
        string sqlString = $"select * from mdr.study_relationships where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyRelationshipInDb>(sqlString);
    }

    public async Task<StudyRelationshipInDb?> GetStudyRelationship(int? id)
    {
        string sqlString = $"select * from mdr.study_relationships where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyRelationshipInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyRelationshipInDb?> CreateStudyRelationship(StudyRelationshipInDb studyRelationshipInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyRelationshipInDb);
        string sqlString = $"select * from mdr.study_relationships where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyRelationshipInDb>(sqlString);
    }

    public async Task<StudyRelationshipInDb?> UpdateStudyRelationship(StudyRelationshipInDb studyRelationshipInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyRelationshipInDb)) ? studyRelationshipInDb : null;
    }

    public async Task<int> DeleteStudyRelationship(int id, string userName)
    {
        string sqlString = $@"update mdr.study_relationships 
                              set last_edited_by = '{userName}'
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
    
    public async Task<IEnumerable<StudyTitleInDb>> GetStudyTitles(string sdSid)
    {
        string sqlString = $"select * from mdr.study_titles where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyTitleInDb>(sqlString);
    }

    public async Task<StudyTitleInDb?> GetStudyTitle(int? id)
    {
        string sqlString = $"select * from mdr.study_titles where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyTitleInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyTitleInDb?> CreateStudyTitle(StudyTitleInDb studyTitleInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyTitleInDb);
        string sqlString = $"select * from mdr.study_titles where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyTitleInDb>(sqlString);
    }

    public async Task<StudyTitleInDb?> UpdateStudyTitle(StudyTitleInDb studyTitleInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyTitleInDb)) ? studyTitleInDb : null;
    }

    public async Task<int> DeleteStudyTitle(int id, string userName)
    {
        string sqlString = $@"update mdr.study_titles 
                              set last_edited_by = '{userName}'
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
    
    public async Task<IEnumerable<StudyTopicInDb>> GetStudyTopics(string sdSid)
    {
        string sqlString = $"select * from mdr.study_topics where sd_sid = '{sdSid}'";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryAsync<StudyTopicInDb>(sqlString);
    }

    public async Task<StudyTopicInDb?> GetStudyTopic(int? id)
    {
        string sqlString = $"select * from mdr.study_topics where id = {id.ToString()}";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.QueryFirstOrDefaultAsync<StudyTopicInDb>(sqlString);
    }

    // Update data
    
    public async Task<StudyTopicInDb?> CreateStudyTopic(StudyTopicInDb studyTopicInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        long id = conn.Insert(studyTopicInDb);
        string sqlString = $"select * from mdr.study_topics where id = {id.ToString()}";
        return await conn.QueryFirstOrDefaultAsync<StudyTopicInDb>(sqlString);
    }

    public async Task<StudyTopicInDb?> UpdateStudyTopic(StudyTopicInDb studyTopicInDb)
    {
        await using var conn = new NpgsqlConnection(_dbConnString);
        return (await conn.UpdateAsync(studyTopicInDb)) ? studyTopicInDb : null;
    }

    public async Task<int> DeleteStudyTopic(int id, string userName)
    {
        string sqlString = $@"update mdr.study_topics 
                              set last_edited_by = '{userName}'
                              where id = {id.ToString()};
                              delete from mdr.study_topics 
                              where id = {id.ToString()};";
        await using var conn = new NpgsqlConnection(_dbConnString);
        return await conn.ExecuteAsync(sqlString);
    }
    
}
