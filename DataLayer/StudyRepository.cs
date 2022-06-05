using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.DataLayer;

    public class StudyRepository : IStudyRepository
    {
        private readonly MdmDbConnection _dbConnection;
        private readonly IDataMapper _dataMapper;
        private readonly IAuditService _auditService;
        private readonly IUserIdentityService _userIdentityService;

        public StudyRepository(
            MdmDbConnection dbConnection, 
            IDataMapper dataMapper,
            IAuditService auditService,
            IUserIdentityService userIdentityService)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _dataMapper = dataMapper ?? throw new ArgumentNullException(nameof(dataMapper));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            _userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        }
        
        public async Task<ICollection<StudyContributorDto>> GetStudyContributors(string sd_sid)
        {
            var data = _dbConnection.StudyContributors.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.StudyContributorDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<StudyContributorDto> GetStudyContributor(int? id)
        {
            var studyContributor = await _dbConnection.StudyContributors.FirstOrDefaultAsync(p => p.Id == id);
            return studyContributor != null ? _dataMapper.StudyContributorDtoMapper(studyContributor) : null;
        }

        public async Task<StudyContributorDto> CreateStudyContributor(StudyContributorDto studyContributorDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);
            var studyContributor = new StudyContributor
            {
                sd_sid = studyContributorDto.sd_sid,
                contrib_type_id = studyContributorDto.contrib_type_id,
                is_individual = studyContributorDto.is_individual,
                person_id = studyContributorDto.person_id,
                person_given_name = studyContributorDto.person_given_name,
                person_family_name = studyContributorDto.person_family_name,
                person_full_name = studyContributorDto.person_full_name,
                person_affiliation = studyContributorDto.person_affiliation,
                organisation_id = studyContributorDto.organisation_id,
                organisation_name = studyContributorDto.organisation_name,
                organisation_ror_id = studyContributorDto.organisation_ror_id,
                created_on = DateTime.Now,
                last_edited_by = "userData"
            };
            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_contributors",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<StudyContributor>(studyContributor).ToString()
            });
            */
            await _dbConnection.StudyContributors.AddAsync(studyContributor);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyContributorDtoMapper(studyContributor);
        }

        public async Task<StudyContributorDto> UpdateStudyContributor(StudyContributorDto studyContributorDto, string accessToken)
        {
            var dbStudyContributor =
                await _dbConnection.StudyContributors.FirstOrDefaultAsync(p => p.Id == studyContributorDto.Id);

            if (dbStudyContributor == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_contributors",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<StudyContributor>(dbStudyContributor).ToString(),
                Post = JsonSerializer.Serialize<StudyContributorDto>(studyContributorDto).ToString()
            });
            */
            
            dbStudyContributor.orcid_id = studyContributorDto.orcid_id;
            dbStudyContributor.contrib_type_id = studyContributorDto.contrib_type_id;
            dbStudyContributor.is_individual = studyContributorDto.is_individual;
            dbStudyContributor.person_id = studyContributorDto.person_id;
            dbStudyContributor.person_given_name = studyContributorDto.person_given_name;
            dbStudyContributor.person_family_name = studyContributorDto.person_family_name;
            dbStudyContributor.person_full_name = studyContributorDto.person_full_name;
            dbStudyContributor.person_affiliation = studyContributorDto.person_affiliation;
            dbStudyContributor.organisation_id = studyContributorDto.organisation_id;
            dbStudyContributor.organisation_name = studyContributorDto.organisation_name;
            dbStudyContributor.organisation_ror_id = studyContributorDto.organisation_ror_id;

            dbStudyContributor.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyContributorDtoMapper(dbStudyContributor);
        }

        public async Task<int> DeleteStudyContributor(int id)
        {
            var data = await _dbConnection.StudyContributors.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.StudyContributors.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllStudyContributors(string sd_sid)
        {
            var data = _dbConnection.StudyContributors.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.StudyContributors.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<StudyFeatureDto>> GetStudyFeatures(string sd_sid)
        {
            var data = _dbConnection.StudyFeatures.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.StudyFeatureDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<StudyFeatureDto> GetStudyFeature(int? id)
        {
            var studyFeature = await _dbConnection.StudyFeatures.FirstOrDefaultAsync(p => p.Id == id);
            return studyFeature != null ? _dataMapper.StudyFeatureDtoMapper(studyFeature) : null;
        }

        public async Task<StudyFeatureDto> CreateStudyFeature(StudyFeatureDto studyFeatureDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);
            var studyFeature = new StudyFeature
            {
                sd_sid = studyFeatureDto.sd_sid,
                feature_type_id = studyFeatureDto.feature_type_id,
                feature_value_id = studyFeatureDto.feature_value_id,
                created_on = DateTime.Now,
                last_edited_by = "userData"
            };

            await _dbConnection.StudyFeatures.AddAsync(studyFeature);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_features",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<StudyFeature>(studyFeature).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyFeatureDtoMapper(studyFeature);
        }

        public async Task<StudyFeatureDto> UpdateStudyFeature(StudyFeatureDto studyFeatureDto, string accessToken)
        {
            var dbStudyFeature =
                await _dbConnection.StudyFeatures.FirstOrDefaultAsync(p => p.Id == studyFeatureDto.Id);

            if (dbStudyFeature == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_features",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<StudyFeature>(dbStudyFeature).ToString(),
                Post = JsonSerializer.Serialize<StudyFeatureDto>(studyFeatureDto).ToString()
            });
            */
            
            dbStudyFeature.feature_type_id = studyFeatureDto.feature_type_id;
            dbStudyFeature.feature_value_id = studyFeatureDto.feature_value_id;

            dbStudyFeature.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyFeatureDtoMapper(dbStudyFeature);
        }

        public async Task<int> DeleteStudyFeature(int id)
        {
            var data = await _dbConnection.StudyFeatures.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.StudyFeatures.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllStudyFeatures(string sd_sid)
        {
            var data = _dbConnection.StudyFeatures.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.StudyFeatures.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<study_identifierDto>> Getstudy_identifiers(string sd_sid)
        {
            var data = _dbConnection.study_identifiers.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.study_identifierDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<study_identifierDto> Getstudy_identifier(int? id)
        {
            var study_identifier = await _dbConnection.study_identifiers.FirstOrDefaultAsync(p => p.Id == id);
            return study_identifier != null ? _dataMapper.study_identifierDtoMapper(study_identifier) : null;
        }

        public async Task<study_identifierDto> Createstudy_identifier(study_identifierDto study_identifierDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var study_identifier = new study_identifier
            {
                sd_sid = study_identifierDto.sd_sid,
                created_on = DateTime.Now,
                identifier_type_id = study_identifierDto.identifier_type_id,
                identifier_value = study_identifierDto.identifier_value,
                identifier_org_id = study_identifierDto.identifier_org_id,
                identifier_org = study_identifierDto.identifier_org,
                identifier_link = study_identifierDto.identifier_link,
                identifier_org_ror_id = study_identifierDto.identifier_org_ror_id,
                identifier_date = study_identifierDto.identifier_date,
                last_edited_by = "userData"
            };

            await _dbConnection.study_identifiers.AddAsync(study_identifier);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_identifiers",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<study_identifier>(study_identifier).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.study_identifierDtoMapper(study_identifier);
        }

        public async Task<study_identifierDto> Updatestudy_identifier(study_identifierDto study_identifierDto, string accessToken)
        {
            var dbstudy_identifier =
                await _dbConnection.study_identifiers.FirstOrDefaultAsync(p => p.Id == study_identifierDto.Id);
            
            if (dbstudy_identifier == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_identifiers",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<study_identifier>(dbstudy_identifier).ToString(),
                Post = JsonSerializer.Serialize<study_identifierDto>(study_identifierDto).ToString()
            });
            */
            
            dbstudy_identifier.identifier_type_id = study_identifierDto.identifier_type_id;
            dbstudy_identifier.identifier_value = study_identifierDto.identifier_value;
            dbstudy_identifier.identifier_org_id = study_identifierDto.identifier_org_id;
            dbstudy_identifier.identifier_org = study_identifierDto.identifier_org;
            dbstudy_identifier.identifier_org_ror_id = study_identifierDto.identifier_org_ror_id;
            dbstudy_identifier.identifier_date = study_identifierDto.identifier_date;
            dbstudy_identifier.identifier_link = study_identifierDto.identifier_link;

            dbstudy_identifier.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.study_identifierDtoMapper(dbstudy_identifier);
        }

        public async Task<int> Deletestudy_identifier(int id)
        {
            var data = await _dbConnection.study_identifiers.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.study_identifiers.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllstudy_identifiers(string sd_sid)
        {
            var data = _dbConnection.study_identifiers.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.study_identifiers.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<StudyReferenceDto>> GetStudyReferences(string sd_sid)
        {
            var data = _dbConnection.StudyReferences.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.StudyReferenceDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<StudyReferenceDto> GetStudyReference(int? id)
        {
            var studyReference = await _dbConnection.StudyReferences.FirstOrDefaultAsync(p => p.Id == id);
            return studyReference != null ? _dataMapper.StudyReferenceDtoMapper(studyReference) : null;
        }

        public async Task<StudyReferenceDto> CreateStudyReference(StudyReferenceDto studyReferenceDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var studyReference = new StudyReference
            {
                sd_sid = studyReferenceDto.sd_sid,
                created_on = DateTime.Now,
                pmid = studyReferenceDto.pmid,
                doi = studyReferenceDto.doi,
                citation = studyReferenceDto.citation,
                comments = studyReferenceDto.comments,
                last_edited_by = "userData"
            };

            await _dbConnection.StudyReferences.AddAsync(studyReference);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_references",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<StudyReference>(studyReference).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyReferenceDtoMapper(studyReference);
        }

        public async Task<StudyReferenceDto> UpdateStudyReference(StudyReferenceDto studyReferenceDto, string accessToken)
        {
            var dbStudyReference =
                await _dbConnection.StudyReferences.FirstOrDefaultAsync(p => p.Id == studyReferenceDto.Id);

            if (dbStudyReference == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_references",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<StudyReference>(dbStudyReference).ToString(),
                Post = JsonSerializer.Serialize<StudyReferenceDto>(studyReferenceDto).ToString()
            });
            */
            
            dbStudyReference.doi = studyReferenceDto.doi;
            dbStudyReference.pmid = studyReferenceDto.pmid;
            dbStudyReference.comments = studyReferenceDto.comments;
            dbStudyReference.citation = studyReferenceDto.citation;

            dbStudyReference.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyReferenceDtoMapper(dbStudyReference);
        }

        public async Task<int> DeleteStudyReference(int id)
        {
            var data = await _dbConnection.StudyReferences.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.StudyReferences.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllStudyReferences(string sd_sid)
        {
            var data = _dbConnection.StudyReferences.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.StudyReferences.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public async Task<ICollection<StudyRelationshipDto>> GetStudyRelationships(string sd_sid)
        {
            var data = _dbConnection.StudyRelationships.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.StudyRelationshipDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<StudyRelationshipDto> GetStudyRelationship(int? id)
        {
            var studyRelationship = await _dbConnection.StudyRelationships.FirstOrDefaultAsync(p => p.Id == id);
            return studyRelationship != null ? _dataMapper.StudyRelationshipDtoMapper(studyRelationship) : null;
        }

        public async Task<StudyRelationshipDto> CreateStudyRelationship(StudyRelationshipDto studyRelationshipDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var studyRelationship = new StudyRelationship
            {
                sd_sid = studyRelationshipDto.sd_sid,
                created_on = DateTime.Now,
                relationship_type_id = studyRelationshipDto.relationship_type_id,
                target_sd_sid = studyRelationshipDto.target_sd_sid,
                last_edited_by = "userData"
            };

            await _dbConnection.StudyRelationships.AddAsync(studyRelationship);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_relationships",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<StudyRelationship>(studyRelationship).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyRelationshipDtoMapper(studyRelationship);
        }

        public async Task<StudyRelationshipDto> UpdateStudyRelationship(StudyRelationshipDto studyRelationshipDto, string accessToken)
        {
            var dbStudyRelationship = await _dbConnection.StudyRelationships.FirstOrDefaultAsync(p => p.Id == studyRelationshipDto.Id);
            if (dbStudyRelationship == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_relationships",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<StudyRelationship>(dbStudyRelationship).ToString(),
                Post = JsonSerializer.Serialize<StudyRelationshipDto>(studyRelationshipDto).ToString()
            });
            */
            
            dbStudyRelationship.relationship_type_id = studyRelationshipDto.relationship_type_id;
            dbStudyRelationship.target_sd_sid = studyRelationshipDto.target_sd_sid;

            dbStudyRelationship.last_edited_by = "userData";
            
            await _dbConnection.SaveChangesAsync();
            return _dataMapper.StudyRelationshipDtoMapper(dbStudyRelationship);
        }

        public async Task<int> DeleteStudyRelationship(int id)
        {
            var data = await _dbConnection.StudyRelationships.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.StudyRelationships.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllStudyRelationships(string sd_sid)
        {
            var data = _dbConnection.StudyRelationships.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.StudyRelationships.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<StudyTitleDto>> GetStudyTitles(string sd_sid)
        {
            var data = _dbConnection.StudyTitles.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.StudyTitleDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<StudyTitleDto> GetStudyTitle(int? id)
        {
            var studyTitle = await _dbConnection.StudyTitles.FirstOrDefaultAsync(p => p.Id == id);
            return studyTitle != null ? _dataMapper.StudyTitleDtoMapper(studyTitle) : null;
        }

        public async Task<StudyTitleDto> CreateStudyTitle(StudyTitleDto studyTitleDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var studyTitle = new StudyTitle
            {
                sd_sid = studyTitleDto.sd_sid,
                created_on = DateTime.Now,
                is_default = studyTitleDto.is_default,
                lang_code = studyTitleDto.lang_code,
                title_text = studyTitleDto.title_text,
                title_type_id = studyTitleDto.title_type_id,
                lang_usage_id = studyTitleDto.lang_usage_id,
                comments = studyTitleDto.comments,
                last_edited_by = "userData"
            };

            await _dbConnection.StudyTitles.AddAsync(studyTitle);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_titles",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<StudyTitle>(studyTitle).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyTitleDtoMapper(studyTitle);
        }

        public async Task<StudyTitleDto> UpdateStudyTitle(StudyTitleDto studyTitleDto, string accessToken)
        {
            var dbStudyTitle = await _dbConnection.StudyTitles.FirstOrDefaultAsync(p => p.Id == studyTitleDto.Id);
            if (dbStudyTitle == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_titles",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<StudyTitle>(dbStudyTitle).ToString(),
                Post = JsonSerializer.Serialize<StudyTitleDto>(studyTitleDto).ToString()
            });
            */
            
            dbStudyTitle.is_default = studyTitleDto.is_default;
            dbStudyTitle.lang_code = studyTitleDto.lang_code;
            dbStudyTitle.title_text = studyTitleDto.title_text;
            dbStudyTitle.title_type_id = studyTitleDto.title_type_id;
            dbStudyTitle.lang_usage_id = studyTitleDto.lang_usage_id;
            dbStudyTitle.comments = studyTitleDto.comments;

            dbStudyTitle.last_edited_by = "userData";
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyTitleDtoMapper(dbStudyTitle);
        }

        public async Task<int> DeleteStudyTitle(int id)
        {
            var data = await _dbConnection.StudyTitles.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.StudyTitles.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllStudyTitles(string sd_sid)
        {
            var data = _dbConnection.StudyTitles.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.StudyTitles.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }
        
        public async Task<ICollection<StudyTopicDto>> GetStudyTopics(string sd_sid)
        {
            var data = _dbConnection.StudyTopics.Where(p => p.sd_sid == sd_sid);
            return data.Any() ? _dataMapper.StudyTopicDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<StudyTopicDto> GetStudyTopic(int? id)
        {
            var studyTopic = await _dbConnection.StudyTopics.FirstOrDefaultAsync(p => p.Id == id);
            return studyTopic != null ? _dataMapper.StudyTopicDtoMapper(studyTopic) : null;
        }

        public async Task<StudyTopicDto> CreateStudyTopic(StudyTopicDto studyTopicDto, string accessToken)
        {
            // var userData = await _userIdentityService.GetUserData(accessToken);

            var studyTopic = new StudyTopic
            {
                sd_sid = studyTopicDto.sd_sid,
                created_on = DateTime.Now,
                topic_type_id = studyTopicDto.topic_type_id,
                mesh_coded = studyTopicDto.mesh_coded,
                mesh_code = studyTopicDto.mesh_code,
                mesh_value = studyTopicDto.mesh_value,
                original_ct_id = studyTopicDto.original_ct_id,
                original_ct_code = studyTopicDto.original_ct_code,
                original_value = studyTopicDto.original_value,
                last_edited_by = "userData"
            };

            await _dbConnection.StudyTopics.AddAsync(studyTopic);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_topics",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<StudyTopic>(studyTopic).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyTopicDtoMapper(studyTopic);
        }

        public async Task<StudyTopicDto> UpdateStudyTopic(StudyTopicDto studyTopicDto, string accessToken)
        {
            var dbStudyTopic =
                await _dbConnection.StudyTopics.FirstOrDefaultAsync(p => p.Id == studyTopicDto.Id);
            if (dbStudyTopic == null) return null;

            // var userData = await _userIdentityService.GetUserData(accessToken);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "study_topics",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<StudyTopic>(dbStudyTopic).ToString(),
                Post = JsonSerializer.Serialize<StudyTopicDto>(studyTopicDto).ToString()
            });
            */
            
            dbStudyTopic.topic_type_id = studyTopicDto.topic_type_id;
            dbStudyTopic.mesh_coded = studyTopicDto.mesh_coded;
            dbStudyTopic.mesh_code = studyTopicDto.mesh_code;
            dbStudyTopic.mesh_value = studyTopicDto.mesh_value;
            dbStudyTopic.original_ct_id = studyTopicDto.original_ct_id;
            dbStudyTopic.original_ct_code = studyTopicDto.original_ct_code;
            dbStudyTopic.original_value = studyTopicDto.original_value;

            dbStudyTopic.last_edited_by = "userData";
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyTopicDtoMapper(dbStudyTopic);
        }

        public async Task<int> DeleteStudyTopic(int id)
        {
            var data = await _dbConnection.StudyTopics.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.StudyTopics.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllStudyTopics(string sd_sid)
        {
            var data = _dbConnection.StudyTopics.Where(p => p.sd_sid == sd_sid);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.StudyTopics.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }


        // STUDY
        public async Task<ICollection<StudyDto>> GetAllStudies()
        {
            if (!_dbConnection.Studies.Any()) return null;
            
            var studyResponses = new List<StudyDto>();
            var studiesList = await _dbConnection.Studies.ToArrayAsync();
            foreach (var study in studiesList)
            {
                studyResponses.Add(await StudyBuilder(study));
            }

            return studyResponses;
        }

        public async Task<StudyDto> GetStudyById(string sd_sid)
        {
            var study = await _dbConnection.Studies.FirstOrDefaultAsync(s => s.sd_sid == sd_sid);
            if (study == null) return null;
            
            return await StudyBuilder(study);
        }

        public async Task<StudyDto> CreateStudy(StudyDto studyDto, string accessToken)
        {
            var study = new Study();
            var study_id = 10001;

            var lastRecord = await _dbConnection.Studies.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (lastRecord != null)
            {
                study_id = lastRecord.Id + 1;
            }

            // var userData = await _userIdentityService.GetUserData(accessToken);

            study.sd_sid = "RMS-" + study_id;
            study.created_on = DateTime.Now;

            study.Mdrsd_sid = null; 
            study.MdrSourceId = null; 

            study.display_title = studyDto.display_title;
            study.title_lang_code = studyDto.title_lang_code; // ?
            study.brief_description = studyDto.brief_description;
            study.data_sharing_statement = studyDto.data_sharing_statement;
            study.study_start_year = studyDto.study_start_year; // ?
            study.study_start_month = studyDto.study_start_month; // ?
            study.study_type_id = studyDto.study_type_id;
            study.study_status_id = studyDto.study_status_id;
            study.study_enrolment = studyDto.study_enrolment; // ?
            study.study_gender_elig_id = studyDto.study_gender_elig_id;
            study.min_age = studyDto.min_age;
            study.min_age_units_id = studyDto.min_age;
            study.max_age = studyDto.max_age;
            study.max_age_units_id = studyDto.max_age_units_id;

            study.last_edited_by = "userData";

            await _dbConnection.Studies.AddAsync(study);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "studies",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<Study>(study).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();

            if (studyDto.StudyContributors is { Count: > 0 })
            {
                foreach (var stc in studyDto.StudyContributors)
                {
                    if (string.IsNullOrEmpty(stc.sd_sid))
                    {
                        stc.sd_sid = study.sd_sid;
                    }
                    await CreateStudyContributor(stc, accessToken);
                }
            }
            
            if (studyDto.StudyFeatures is { Count: > 0 })
            {
                foreach (var stf in studyDto.StudyFeatures)
                {
                    if (string.IsNullOrEmpty(stf.sd_sid))
                    {
                        stf.sd_sid = study.sd_sid;
                    }
                    await CreateStudyFeature(stf, accessToken);
                }
            }
            
            if (studyDto.study_identifiers is { Count: > 0 })
            {
                foreach (var sti in studyDto.study_identifiers)
                {
                    if (string.IsNullOrEmpty(sti.sd_sid))
                    {
                        sti.sd_sid = study.sd_sid;
                    }
                    await Createstudy_identifier(sti, accessToken);
                }
            }
            
            if (studyDto.StudyReferences is { Count: > 0 })
            {
                foreach (var str in studyDto.StudyReferences)
                {
                    if (string.IsNullOrEmpty(str.sd_sid))
                    {
                        str.sd_sid = study.sd_sid;
                    }
                    await CreateStudyReference(str, accessToken);
                }
            }
            
            if (studyDto.StudyRelationships is { Count: > 0 })
            {
                foreach (var str in studyDto.StudyRelationships)
                {
                    if (string.IsNullOrEmpty(str.sd_sid))
                    {
                        str.sd_sid = study.sd_sid;
                    }
                    await CreateStudyRelationship(str, accessToken);
                }
            }
            
            if (studyDto.StudyTitles is { Count: > 0 })
            {
                foreach (var stt in studyDto.StudyTitles)
                {
                    if (string.IsNullOrEmpty(stt.sd_sid))
                    {
                        stt.sd_sid = study.sd_sid;
                    }
                    await CreateStudyTitle(stt, accessToken);
                }
            }
            
            if (studyDto.StudyTopics is { Count: > 0 })
            {
                foreach (var stt in studyDto.StudyTopics)
                {
                    if (string.IsNullOrEmpty(stt.sd_sid))
                    {
                        stt.sd_sid = study.sd_sid;
                    }
                    await CreateStudyTopic(stt, accessToken);
                }
            }
            
            return await StudyBuilder(study);
        }

        public async Task<StudyDto> UpdateStudy(StudyDto studyDto, string accessToken)
        {
            var dbStudy = await _dbConnection.Studies.FirstOrDefaultAsync(p => p.sd_sid == studyDto.sd_sid);
            if (dbStudy == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData();

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "studies",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<Study>(dbStudy).ToString(),
                Post = JsonSerializer.Serialize<StudyDto>(studyDto).ToString()
            });
            */
            
            dbStudy.display_title = studyDto.display_title;
            dbStudy.title_lang_code = studyDto.title_lang_code; // ?
            dbStudy.brief_description = studyDto.brief_description;
            dbStudy.data_sharing_statement = studyDto.data_sharing_statement;
            dbStudy.study_start_year = studyDto.study_start_year; // ?
            dbStudy.study_start_month = studyDto.study_start_month; // ?
            dbStudy.study_type_id = studyDto.study_type_id;
            dbStudy.study_status_id = studyDto.study_status_id;
            dbStudy.study_enrolment = studyDto.study_enrolment; // ?
            dbStudy.study_gender_elig_id = studyDto.study_gender_elig_id;
            dbStudy.min_age = studyDto.min_age;
            dbStudy.min_age_units_id = studyDto.min_age_units_id;
            dbStudy.max_age = studyDto.max_age;
            dbStudy.max_age_units_id = studyDto.max_age_units_id;

            dbStudy.last_edited_by = "userData";
            
            if (studyDto.StudyContributors is { Count: > 0 })
            {
                foreach (var stc in studyDto.StudyContributors)
                {
                    if (stc.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(stc.sd_sid))
                        {
                            stc.sd_sid = dbStudy.sd_sid;
                        }
                        await CreateStudyContributor(stc, accessToken);
                    }
                    else
                    {
                        await UpdateStudyContributor(stc, accessToken);
                    }
                }
            }
            
            if (studyDto.StudyFeatures is { Count: > 0 })
            {
                foreach (var stf in studyDto.StudyFeatures)
                {
                    if (stf.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(stf.sd_sid))
                        {
                            stf.sd_sid = dbStudy.sd_sid;
                        }
                        await CreateStudyFeature(stf, accessToken);
                    }
                    else
                    {
                        await UpdateStudyFeature(stf, accessToken);
                    }
                }
            }
            
            if (studyDto.study_identifiers is { Count: > 0 })
            {
                foreach (var sti in studyDto.study_identifiers)
                {
                    if (sti.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(sti.sd_sid))
                        {
                            sti.sd_sid = dbStudy.sd_sid;
                        }
                        await Createstudy_identifier(sti, accessToken);
                    }
                    else
                    {
                        await Updatestudy_identifier(sti, accessToken);
                    }
                }
            }
            
            if (studyDto.StudyReferences is { Count: > 0 })
            {
                foreach (var str in studyDto.StudyReferences)
                {
                    if (str.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(str.sd_sid))
                        {
                            str.sd_sid = dbStudy.sd_sid;
                        }
                        await CreateStudyReference(str, accessToken);
                    }
                    else
                    {
                        await UpdateStudyReference(str, accessToken);
                    }
                }
            }
            
            if (studyDto.StudyRelationships is { Count: > 0 })
            {
                foreach (var str in studyDto.StudyRelationships)
                {
                    if (str.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(str.sd_sid))
                        {
                            str.sd_sid = dbStudy.sd_sid;
                        }
                        await CreateStudyRelationship(str, accessToken);
                    }
                    else
                    {
                        await UpdateStudyRelationship(str, accessToken);
                    }
                }
            }
            
            if (studyDto.StudyTitles is { Count: > 0 })
            {
                foreach (var stt in studyDto.StudyTitles)
                {
                    if (stt.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(stt.sd_sid))
                        {
                            stt.sd_sid = dbStudy.sd_sid;
                        }
                        await CreateStudyTitle(stt, accessToken);
                    }
                    else
                    {
                        await UpdateStudyTitle(stt, accessToken);
                    }
                }
            }
            
            if (studyDto.StudyTopics is { Count: > 0 })
            {
                foreach (var stt in studyDto.StudyTopics)
                {
                    if (stt.Id is null or 0)
                    {
                        if (string.IsNullOrEmpty(stt.sd_sid))
                        {
                            stt.sd_sid = dbStudy.sd_sid;
                        }
                        await CreateStudyTopic(stt, accessToken);
                    }
                    else
                    {
                        await UpdateStudyTopic(stt, accessToken);
                    }
                }
            }

            await _dbConnection.SaveChangesAsync();
            
            return await StudyBuilder(dbStudy);
        }

        public async Task<int> DeleteStudy(string sd_sid)
        {
            var dbStudy = await _dbConnection.Studies.FirstOrDefaultAsync(p => p.sd_sid == sd_sid);
            if (dbStudy == null) return 0;
            
            await DeleteAllStudyContributors(sd_sid);
            await DeleteAllStudyFeatures(sd_sid);
            await DeleteAllstudy_identifiers(sd_sid);
            await DeleteAllStudyReferences(sd_sid);
            await DeleteAllStudyRelationships(sd_sid);
            await DeleteAllStudyTitles(sd_sid);
            await DeleteAllStudyTopics(sd_sid);

            _dbConnection.Studies.Remove(dbStudy);
            await _dbConnection.SaveChangesAsync();
            
            return 1;
        }

        public async Task<ICollection<StudyDataDto>> GetStudiesData()
        {
            if (!_dbConnection.Studies.Any()) return null;
            
            var studyResponses = new List<StudyDataDto>();
            var studiesList = await _dbConnection.Studies.ToArrayAsync();
            studyResponses.AddRange(studiesList.Select(study => _dataMapper.StudyDataDtoMapper(study)));

            return studyResponses;
        }

        public async Task<StudyDataDto> GetStudyData(string sd_sid)
        {
            var study = await _dbConnection.Studies.FirstOrDefaultAsync(s => s.sd_sid == sd_sid);
            return study != null ? _dataMapper.StudyDataDtoMapper(study) : null;
        }

        public async Task<ICollection<StudyDataDto>> GetRecentStudyData(int limit)
        {
            if (!_dbConnection.Studies.Any()) return null;

            var recentStudies = await _dbConnection.Studies.OrderByDescending(p => p.Id).Take(limit).ToArrayAsync();
            return _dataMapper.StudyDataDtoBuilder(recentStudies);
        }

        public async Task<StudyDataDto> CreateStudyData(StudyDataDto studyData, string accessToken)
        {
            var study = new Study();
            var study_id = 10001;

            var lastRecord = await _dbConnection.Studies.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (lastRecord != null)
            {
                study_id = lastRecord.Id + 1;
            }

            // var userData = await _userIdentityService.GetUserData(accessToken);

            study.sd_sid = study_id.ToString();
            study.created_on = DateTime.Now;

            study.Mdrsd_sid = null; 
            study.MdrSourceId = null; 

            study.display_title = studyData.display_title;
            study.title_lang_code = studyData.title_lang_code; // ?
            study.brief_description = studyData.brief_description;
            study.data_sharing_statement = studyData.data_sharing_statement;
            study.study_start_year = studyData.study_start_year; // ?
            study.study_start_month = studyData.study_start_month; // ?
            study.study_type_id = studyData.study_type_id;
            study.study_status_id = studyData.study_status_id;
            study.study_enrolment = studyData.study_enrolment; // ?
            study.study_gender_elig_id = studyData.study_gender_elig_id;
            study.min_age = studyData.min_age;
            study.min_age_units_id = studyData.min_age;
            study.max_age = studyData.max_age;
            study.max_age_units_id = studyData.max_age_units_id;

            study.last_edited_by = "userData";

            await _dbConnection.Studies.AddAsync(study);

            /*
            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "studies",
                TableId = null,
                ChangeType = 1,
                UserName = userData,
                Prior = null,
                Post = JsonSerializer.Serialize<Study>(study).ToString()
            });
            */

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyDataDtoMapper(study);
        }
        
        public async Task<StudyDataDto> UpdateStudyData(StudyDataDto studyData, string accessToken)
        {
            var dbStudy = await _dbConnection.Studies.FirstOrDefaultAsync(p => p.sd_sid == studyData.sd_sid);
            if (dbStudy == null) return null;

            /*
            var userData = await _userIdentityService.GetUserData(accessToken);

            await _auditService.AddAuditRecord(new AuditDto
            {
                TableName = "studies",
                TableId = null,
                ChangeType = 2,
                UserName = userData,
                Prior = JsonSerializer.Serialize<Study>(dbStudy).ToString(),
                Post = JsonSerializer.Serialize<StudyDataDto>(studyData).ToString()
            });
            */
            
            dbStudy.display_title = studyData.display_title;
            dbStudy.title_lang_code = studyData.title_lang_code;
            dbStudy.brief_description = studyData.brief_description;
            dbStudy.data_sharing_statement = studyData.data_sharing_statement;
            dbStudy.study_start_year = studyData.study_start_year;
            dbStudy.study_start_month = studyData.study_start_month;
            dbStudy.study_type_id = studyData.study_type_id;
            dbStudy.study_status_id = studyData.study_status_id;
            dbStudy.study_enrolment = studyData.study_enrolment;
            dbStudy.study_gender_elig_id = studyData.study_gender_elig_id;
            dbStudy.min_age = studyData.min_age;
            dbStudy.min_age_units_id = studyData.min_age_units_id;
            dbStudy.max_age = studyData.max_age;
            dbStudy.max_age_units_id = studyData.max_age_units_id;

            dbStudy.last_edited_by = "userData";

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.StudyDataDtoMapper(dbStudy);
        }


        private async Task<StudyDto> StudyBuilder(Study study)
        {
            return new StudyDto()
            {
                Id = study.Id,
                sd_sid = study.sd_sid,
                Mdrsd_sid = study.Mdrsd_sid,
                MdrSourceId = study.MdrSourceId,
                display_title = study.display_title,
                title_lang_code = study.title_lang_code,
                brief_description = study.brief_description,
                data_sharing_statement = study.data_sharing_statement,
                study_start_year = study.study_start_year,
                study_start_month = study.study_start_month,
                study_type_id = study.study_type_id,
                study_status_id = study.study_status_id,
                study_enrolment = study.study_enrolment,
                study_gender_elig_id = study.study_gender_elig_id,
                min_age = study.min_age,
                min_age_units_id = study.min_age_units_id,
                max_age = study.max_age,
                max_age_units_id = study.max_age_units_id,
                created_on = study.created_on.ToString(),
                StudyContributors = await GetStudyContributors(study.sd_sid),
                StudyFeatures = await GetStudyFeatures(study.sd_sid),
                StudyRelationships = await GetStudyRelationships(study.sd_sid),
                study_identifiers = await Getstudy_identifiers(study.sd_sid),
                StudyReferences = await GetStudyReferences(study.sd_sid),
                StudyTitles = await GetStudyTitles(study.sd_sid),
                StudyTopics = await GetStudyTopics(study.sd_sid)
            };
        }

        private static int CalculateSkip(int page, int size)
        {
            var skip = 0;
            if (page > 1)
            {
                skip = (page - 1) * size;
            }

            return skip;
        }

        public async Task<PaginationResponse<StudyDto>> PaginateStudies(PaginationRequest paginationRequest)
        {
            var studies = new List<StudyDto>();

            var skip = CalculateSkip(paginationRequest.Page, paginationRequest.Size);
            
            var query = _dbConnection.Studies
                .AsNoTracking()
                .OrderBy(arg => arg.Id);
                        
            var data = await query
                .Skip(skip)
                .Take(paginationRequest.Size)
                .ToListAsync();

            var total = await query.CountAsync();

            if (data is { Count: > 0 })
            {
                foreach (var study in data)
                {
                    studies.Add(await StudyBuilder(study));
                }
            }

            return new PaginationResponse<StudyDto>
            {
                Total = total,
                Data = studies
            };
        }

        public async Task<PaginationResponse<StudyDto>> FilterStudiesByTitle(FilteringByTitleRequest filteringByTitleRequest)
        {
            var studies = new List<StudyDto>();

            var skip = CalculateSkip(filteringByTitleRequest.Page, filteringByTitleRequest.Size);
            
            var query = _dbConnection.Studies
                .AsNoTracking()
                .Where(p => p.display_title.ToLower().Contains(filteringByTitleRequest.Title.ToLower()))
                .OrderBy(arg => arg.Id);
                
            var data = await query
                .Skip(skip)
                .Take(filteringByTitleRequest.Size)
                .ToListAsync();

            var total = await query.CountAsync();
                        
            if (data is { Count: > 0 })
            {
                foreach (var study in data)
                {
                    studies.Add(await StudyBuilder(study));
                }
            }

            return new PaginationResponse<StudyDto>
            {
                Total = total,
                Data = studies
            };
        }

        public async Task<int> GetTotalStudies()
        {
            return await _dbConnection.Studies.AsNoTracking().CountAsync();
        }
    }
