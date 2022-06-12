using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
 

namespace rmsbe.DataLayer;

    public class DtpRepository : IDtpRepository
    {
        /*
        private readonly RmsDbConnection _dbConnection;
        private readonly IDataMapper _dataMapper;

        public DtpRepository(RmsDbConnection dbContext, IDataMapper dataMapper)
        {
            _dbConnection = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dataMapper = dataMapper ?? throw new ArgumentNullException(nameof(dataMapper));
        }

        public IQueryable<Dta> GetQueryableDta()
        {
            return _dbConnection.Dtas;
        }

        public async Task<ICollection<DtaDto>> GetAllDta(int dtp_id)
        {
            var data = _dbConnection.Dtas.Where(p => p.dtp_id == dtp_id);
            return data.Any() ? _dataMapper.DtaDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DtaDto> GetDta(int id)
        {
            var dta = await _dbConnection.Dtas.FirstOrDefaultAsync(p => p.Id == id);
            return dta != null ? _dataMapper.DtaDtoMapper(dta) : null;
        }

        public async Task<DtaDto> CreateDta(int dtp_id, DtaDto dtaDto)
        {
            var dta = new Dta
            {
                dtp_id = dtp_id,
                created_on = DateTime.Now,
                conforms_to_default = dtaDto.conforms_to_default,
                Variations = dtaDto.Variations,
                repo_signatory_1 = dtaDto.repo_signatory_1,
                repo_signatory_2 = dtaDto.repo_signatory_2,
                provider_signatory_1 = dtaDto.provider_signatory_1,
                provider_signatory_2 = dtaDto.provider_signatory_2,
                Notes = dtaDto.Notes
            };

            await _dbConnection.Dtas.AddAsync(dta);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtaDtoMapper(dta);
        }

        public async Task<DtaDto> UpdateDta(DtaDto dtaDto)
        {
            var dbDta = await _dbConnection.Dtas.FirstOrDefaultAsync(p => p.Id == dtaDto.Id);
            if (dbDta == null) return null;
            
            dbDta.conforms_to_default = dtaDto.conforms_to_default;
            dbDta.Variations = dtaDto.Variations;
            dbDta.repo_signatory_1 = dtaDto.repo_signatory_1;
            dbDta.repo_signatory_2 = dtaDto.repo_signatory_2;
            dbDta.provider_signatory_1 = dtaDto.provider_signatory_1;
            dbDta.provider_signatory_2 = dtaDto.provider_signatory_2;
            dbDta.Notes = dtaDto.Notes;
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtaDtoMapper(dbDta);
        }

        public async Task<int> DeleteDta(int id)
        {
            var data = await _dbConnection.Dtas.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.Dtas.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllDta(int dtp_id)
        {
            var data = _dbConnection.Dtas.Where(p => p.dtp_id == dtp_id);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.Dtas.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<DtpDataset> GetQueryableDtpDatasets()
        {
            return _dbConnection.DtpDatasets;
        }

        public async Task<ICollection<DtpDatasetDto>> GetAllDtpDatasets()
        {
            var data = _dbConnection.DtpDatasets;
            return data.Any() ? _dataMapper.DtpDatasetDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DtpDatasetDto> GetDtpDataset(int id)
        {
            var dtpDataset = await _dbConnection.DtpDatasets.FirstOrDefaultAsync(p => p.Id == id);
            return dtpDataset != null ? _dataMapper.DtpDatasetDtoMapper(dtpDataset) : null;
        }

        public async Task<DtpDatasetDto> CreateDtpDataset(string object_id, DtpDatasetDto dtpDatasetDto)
        {
            var dtpDataset = new DtpDataset
            {
                object_id = object_id,
                created_on = DateTime.Now,
                legal_status_id = dtpDatasetDto.legal_status_id,
                legal_status_text = dtpDatasetDto.legal_status_text,
                legal_status_path = dtpDatasetDto.legal_status_path,
                desc_md_check_by = dtpDatasetDto.desc_md_check_by,
                desc_md_check_date = dtpDatasetDto.desc_md_check_date,
                desc_md_check_status_id = dtpDatasetDto.desc_md_check_status_id,
                deident_check_by = dtpDatasetDto.deident_check_by,
                deident_check_date = dtpDatasetDto.deident_check_date,
                deident_check_status_id = dtpDatasetDto.deident_check_status_id,
                Notes = dtpDatasetDto.Notes
            };

            await _dbConnection.DtpDatasets.AddAsync(dtpDataset);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpDatasetDtoMapper(dtpDataset);
        }

        public async Task<DtpDatasetDto> UpdateDtpDataset(DtpDatasetDto dtpDatasetDto)
        {
            var dbDtpDataset = await _dbConnection.DtpDatasets.FirstOrDefaultAsync(p => p.Id == dtpDatasetDto.Id);
            if (dbDtpDataset == null) return null;
            
            dbDtpDataset.legal_status_id = dtpDatasetDto.legal_status_id;
            dbDtpDataset.legal_status_text = dtpDatasetDto.legal_status_text;
            dbDtpDataset.legal_status_path = dtpDatasetDto.legal_status_path;
            dbDtpDataset.desc_md_check_by = dtpDatasetDto.desc_md_check_by;
            dbDtpDataset.desc_md_check_date = dtpDatasetDto.desc_md_check_date;
            dbDtpDataset.desc_md_check_status_id = dtpDatasetDto.desc_md_check_status_id;
            dbDtpDataset.deident_check_by = dtpDatasetDto.deident_check_by;
            dbDtpDataset.deident_check_date = dtpDatasetDto.deident_check_date;
            dbDtpDataset.deident_check_status_id = dtpDatasetDto.deident_check_status_id;
            dbDtpDataset.Notes = dtpDatasetDto.Notes;

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpDatasetDtoMapper(dbDtpDataset);
        }

        public async Task<int> DeleteDtpDataset(int id)
        {
            var data = await _dbConnection.DtpDatasets.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.DtpDatasets.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public IQueryable<DtpObject> GetQueryableDtpObjects()
        {
            return _dbConnection.DtpObjects;
        }

        public async Task<ICollection<DtpObjectDto>> GetAllDtpObjects(int dtp_id)
        {
            var data = _dbConnection.DtpObjects.Where(p => p.dtp_id == dtp_id);
            return data.Any() ? _dataMapper.DtpObjectDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DtpObjectDto> GetDtpObject(int id)
        {
            var dtpObject = await _dbConnection.DtpObjects.FirstOrDefaultAsync(p => p.Id == id);
            return dtpObject != null ? _dataMapper.DtpObjectDtoMapper(dtpObject) : null;
        }

        public async Task<DtpObjectDto> CreateDtpObject(int dtp_id, string object_id, DtpObjectDto dtpObjectDto)
        {
            var dtpObject = new DtpObject
            {
                dtp_id = dtp_id,
                object_id = object_id,
                created_on = DateTime.Now,
                is_dataset = dtpObjectDto.is_dataset,
                access_type_id = dtpObjectDto.access_type_id,
                download_allowed = dtpObjectDto.download_allowed,
                access_details = dtpObjectDto.access_details,
                requires_embargo_period = dtpObjectDto.requires_embargo_period,
                embargo_end_date = dtpObjectDto.embargo_end_date,
                embargo_still_applies = dtpObjectDto.embargo_still_applies,
                access_check_status_id = dtpObjectDto.access_check_status_id,
                access_check_date = dtpObjectDto.access_check_date,
                access_check_by = dtpObjectDto.access_check_by,
                md_check_status_id = dtpObjectDto.md_check_status_id,
                md_check_by = dtpObjectDto.md_check_by,
                md_check_date = dtpObjectDto.md_check_date,
                Notes = dtpObjectDto.Notes
            };

            await _dbConnection.DtpObjects.AddAsync(dtpObject);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpObjectDtoMapper(dtpObject);
        }

        public async Task<DtpObjectDto> UpdateDtpObject(DtpObjectDto dtpObjectDto)
        {
            var dbDtpObject = await _dbConnection.DtpObjects.FirstOrDefaultAsync(p => p.Id == dtpObjectDto.Id);
            if (dbDtpObject == null) return null;
            
            dbDtpObject.is_dataset = dtpObjectDto.is_dataset;
            dbDtpObject.access_type_id = dtpObjectDto.access_type_id;
            dbDtpObject.download_allowed = dtpObjectDto.download_allowed;
            dbDtpObject.access_details = dtpObjectDto.access_details;
            dbDtpObject.requires_embargo_period = dtpObjectDto.requires_embargo_period;
            dbDtpObject.embargo_end_date = dtpObjectDto.embargo_end_date;
            dbDtpObject.embargo_still_applies = dtpObjectDto.embargo_still_applies;
            dbDtpObject.access_check_status_id = dtpObjectDto.access_check_status_id;
            dbDtpObject.access_check_date = dtpObjectDto.access_check_date;
            dbDtpObject.access_check_by = dtpObjectDto.access_check_by;
            dbDtpObject.md_check_status_id = dtpObjectDto.md_check_status_id;
            dbDtpObject.md_check_by = dtpObjectDto.md_check_by;
            dbDtpObject.md_check_date = dtpObjectDto.md_check_date;
            dbDtpObject.Notes = dtpObjectDto.Notes;
            
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpObjectDtoMapper(dbDtpObject);
        }

        public async Task<int> DeleteDtpObject(int id)
        {
            var data = await _dbConnection.DtpObjects.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.DtpObjects.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllDtpObjects(int dtp_id)
        {
            var data = _dbConnection.DtpObjects.Where(p => p.dtp_id == dtp_id);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.DtpObjects.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<DtpStudy> GetQueryableDtpStudies()
        {
            return _dbConnection.DtpStudies;
        }

        public async Task<ICollection<DtpStudyDto>> GetAllDtpStudies(int dtp_id)
        {
            var data = _dbConnection.DtpStudies.Where(p => p.dtp_id == dtp_id);
            return data.Any() ? _dataMapper.DtpStudyDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DtpStudyDto> GetDtpStudy(int id)
        {
            var dtpStudy = await _dbConnection.DtpStudies.FirstOrDefaultAsync(p => p.Id == id);
            return dtpStudy != null ? _dataMapper.DtpStudyDtoMapper(dtpStudy) : null;
        }

        public async Task<DtpStudyDto> CreateDtpStudy(int dtp_id, string study_id, DtpStudyDto dtpStudyDto)
        {
            var dtpStudy = new DtpStudy
            {
                dtp_id = dtp_id,
                study_id = study_id,
                created_on = DateTime.Now,
                md_check_status_id = dtpStudyDto.md_check_status_id,
                md_check_by = dtpStudyDto.md_check_by,
                md_check_date = dtpStudyDto.md_check_date
            };

            await _dbConnection.DtpStudies.AddAsync(dtpStudy);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpStudyDtoMapper(dtpStudy);
        }

        public async Task<DtpStudyDto> UpdateDtpStudy(DtpStudyDto dtpStudyDto)
        {
            var dbDtpStudy = await _dbConnection.DtpStudies.FirstOrDefaultAsync(p => p.Id == dtpStudyDto.Id);
            if (dbDtpStudy == null) return null;
            
            dbDtpStudy.md_check_status_id = dtpStudyDto.md_check_status_id;
            dbDtpStudy.md_check_by = dtpStudyDto.md_check_by;
            dbDtpStudy.md_check_date = dtpStudyDto.md_check_date;

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpStudyDtoMapper(dbDtpStudy);
        }

        public async Task<int> DeleteDtpStudy(int id)
        {
            var data = await _dbConnection.DtpStudies.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.DtpStudies.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllDtpStudies(int dtp_id)
        {
            var data = _dbConnection.DtpStudies.Where(p => p.dtp_id == dtp_id);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.DtpStudies.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<Dtp> GetQueryableDtp()
        {
            return _dbConnection.Dtps;
        }

        public async Task<ICollection<DtpDto>> GetAllDtp()
        {
            return _dbConnection.Dtps.Any() ? _dataMapper.DtpDtoBuilder(await _dbConnection.Dtps.ToArrayAsync()) : null;
        }

        public async Task<DtpDto> GetDtp(int id)
        {
            return _dataMapper.DtpDtoMapper(await _dbConnection.Dtps.FirstOrDefaultAsync(p => p.Id == id));
        }

        public async Task<ICollection<DtpDto>> GetRecentDtp(int limit)
        {
            if (!_dbConnection.Dtps.Any()) return null;

            var recentDtp = await _dbConnection.Dtps.OrderByDescending(p => p.Id).Take(limit).ToArrayAsync();
            return _dataMapper.DtpDtoBuilder(recentDtp);
        }

        public async Task<DtpDto> CreateDtp(DtpDto dtpDto)
        {
            var dtp = new Dtp
            {
                created_on = DateTime.Now,
                org_id = dtpDto.org_id,
                display_name = dtpDto.display_name,
                status_id = dtpDto.status_id,
                initial_contact_date = dtpDto.initial_contact_date,
                set_up_completed = dtpDto.set_up_completed,
                md_access_granted = dtpDto.md_access_granted,
                md_complete_date = dtpDto.md_complete_date,
                dta_agreed_date = dtpDto.dta_agreed_date,
                upload_access_requested = dtpDto.upload_access_requested,
                upload_access_confirmed = dtpDto.upload_access_confirmed,
                uploads_complete = dtpDto.uploads_complete,
                qc_checks_completed = dtpDto.qc_checks_completed,
                md_integrated_with_mdr = dtpDto.md_integrated_with_mdr,
                availability_requested = dtpDto.availability_requested,
                availability_confirmed = dtpDto.availability_confirmed
            };

            await _dbConnection.Dtps.AddAsync(dtp);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpDtoMapper(dtp);
        }

        public async Task<DtpDto> UpdateDtp(DtpDto dtpDto)
        {
            var dbDtp = await _dbConnection.Dtps.FirstOrDefaultAsync(p => p.Id == dtpDto.Id);
            if (dbDtp == null) return null;
            
            dbDtp.org_id = dtpDto.org_id;
            dbDtp.display_name = dtpDto.display_name;
            dbDtp.status_id = dtpDto.status_id;
            dbDtp.initial_contact_date = dtpDto.initial_contact_date;
            dbDtp.set_up_completed = dtpDto.set_up_completed;
            dbDtp.md_access_granted = dtpDto.md_access_granted;
            dbDtp.md_complete_date = dtpDto.md_complete_date;
            dbDtp.dta_agreed_date = dtpDto.dta_agreed_date;
            dbDtp.upload_access_requested = dtpDto.upload_access_requested;
            dbDtp.upload_access_confirmed = dtpDto.upload_access_confirmed;
            dbDtp.uploads_complete = dtpDto.uploads_complete;
            dbDtp.qc_checks_completed = dtpDto.qc_checks_completed;
            dbDtp.md_integrated_with_mdr = dtpDto.md_integrated_with_mdr;
            dbDtp.availability_requested = dtpDto.availability_requested;
            dbDtp.availability_confirmed = dtpDto.availability_confirmed;
                
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DtpDtoMapper(dbDtp);
        }

        public async Task<int> DeleteDtp(int id)
        {
            var data = await _dbConnection.Dtps.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.Dtps.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
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

        public async Task<PaginationResponse<DtpDto>> PaginateDtp(PaginationRequest paginationRequest)
        {
            var dtp = new List<DtpDto>();

            var skip = CalculateSkip(paginationRequest.Page, paginationRequest.Size);

            var query = _dbConnection.Dtps
                .AsNoTracking()
                .OrderBy(arg => arg.Id);

            var data = await query
                .Skip(skip)
                .Take(paginationRequest.Size)
                .ToListAsync();
            
            var total = await query.CountAsync();

            if (data is {Count: > 0})
            {
                foreach (var dtpRecord in data)
                {
                    dtp.Add(_dataMapper.DtpDtoMapper(dtpRecord));
                }
            }

            return new PaginationResponse<DtpDto>
            {
                Total = total,
                Data = dtp
            };
        }

        public async Task<PaginationResponse<DtpDto>> FilterDtpByTitle(FilteringByTitleRequest filteringByTitleRequest)
        {
            var dtp = new List<DtpDto>();

            var skip = CalculateSkip(filteringByTitleRequest.Page, filteringByTitleRequest.Size);

            var query = _dbConnection.Dtps
                .AsNoTracking()
                .Where(p => p.display_name.ToLower().Contains(filteringByTitleRequest.Title.ToLower()))
                .OrderBy(arg => arg.Id);

            var data = await query
                .Skip(skip)
                .Take(filteringByTitleRequest.Size)
                .ToListAsync();
            
            var total = await query.CountAsync();

            if (data is {Count: > 0})
            {
                foreach (var dtpRecord in data)
                {
                    dtp.Add(_dataMapper.DtpDtoMapper(dtpRecord));
                }
            }

            return new PaginationResponse<DtpDto>
            {
                Total = total,
                Data = dtp
            };
        }

        public async Task<int> GetTotalDtp()
        {
            return await _dbConnection.Dtps.AsNoTracking().CountAsync();
        }

        public async Task<int> GetUncompletedDtp()
        {
            return await _dbConnection.Dtps.AsNoTracking().Where(p => p.status_id == 16).CountAsync();
        }
        */
    }
