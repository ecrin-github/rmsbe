using System;
using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;


namespace rmsbe.DataLayer;

    public class DupRepository : IDupRepository
    {
        /*
        private readonly RmsDbConnection _dbConnection;
        private readonly IDataMapper _dataMapper;

        public DupRepository(RmsDbConnection dbContext, IDataMapper dataMapper)
        {
            _dbConnection = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dataMapper = dataMapper ?? throw new ArgumentNullException(nameof(dataMapper));
        }

        public IQueryable<DupObject> GetQueryableDupObjects()
        {
            return _dbConnection.DupObjects;
        }

        public async Task<ICollection<DupObjectDto>> GetDupObjects(int dupId)
        {
            var data = _dbConnection.DupObjects.Where(p => p.DupId == dupId);
            return data.Any() ? _dataMapper.DupObjectDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DupObjectDto> GetDupObject(int id)
        {
            var dupObject = await _dbConnection.DupObjects.FirstOrDefaultAsync(p => p.Id == id);
            return dupObject != null ? _dataMapper.DupObjectDtoMapper(dupObject) : null;
        }

        public async Task<DupObjectDto> CreateDupObject(int dupId, DupObjectDto dupObjectDto)
        {
            var dupObject = new DupObject
            {
                DupId = dupId,
                created_on = DateTime.Now,
                object_id = dupObjectDto.object_id,
                access_type_id = dupObjectDto.access_type_id,
                access_details = dupObjectDto.access_details,
                Notes = dupObjectDto.Notes
            };

            await _dbConnection.DupObjects.AddAsync(dupObject);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DupObjectDtoMapper(dupObject);
        }

        public async Task<DupObjectDto> UpdateDupObject(DupObjectDto dupObjectDto)
        {
            var dbDupObject = await _dbConnection.DupObjects.FirstOrDefaultAsync(p => p.Id == dupObjectDto.Id);
            if (dbDupObject == null) return null;
            
            dbDupObject.object_id = dupObjectDto.object_id;
            dbDupObject.access_type_id = dupObjectDto.access_type_id;
            dbDupObject.access_details = dupObjectDto.access_details;
            dbDupObject.Notes = dupObjectDto.Notes;

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DupObjectDtoMapper(dbDupObject);
        }

        public async Task<int> DeleteDupObject(int id)
        {
            var data = await _dbConnection.DupObjects.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.DupObjects.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllDupObjects(int dupId)
        {
            var data = _dbConnection.DupObjects.Where(p => p.DupId == dupId);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.DupObjects.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<DupPrereq> GetQueryableDupPrereqs()
        {
            return _dbConnection.DupPrereqs;
        }

        public async Task<ICollection<DupPrereqDto>> GetDupPrereqs(int dupId)
        {
            var data = _dbConnection.DupPrereqs.Where(p => p.DupId == dupId);
            return data.Any() ? _dataMapper.DupPrereqDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DupPrereqDto> GetDupPrereq(int id)
        {
            var dupPrereq = await _dbConnection.DupPrereqs.FirstOrDefaultAsync(p => p.Id == id);
            return dupPrereq != null ? _dataMapper.DupPrereqDtoMapper(dupPrereq) : null;
        }

        public async Task<DupPrereqDto> CreateDupPrereq(int dupId, DupPrereqDto dupPrereqDto)
        {
            var dupPrereq = new DupPrereq
            {
                DupId = dupId,
                created_on = DateTime.Now,
                object_id = dupPrereqDto.object_id,
                met_notes = dupPrereqDto.met_notes,
                pre_requisite_id = dupPrereqDto.pre_requisite_id,
                prerequisite_met = dupPrereqDto.prerequisite_met
            };

            await _dbConnection.DupPrereqs.AddAsync(dupPrereq);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DupPrereqDtoMapper(dupPrereq);
        }

        public async Task<DupPrereqDto> UpdateDupPrereq(DupPrereqDto dupPrereqDto)
        {
            var dbDupPrereq = await _dbConnection.DupPrereqs.FirstOrDefaultAsync(p => p.Id == dupPrereqDto.Id);
            if (dbDupPrereq == null) return null;
            
            dbDupPrereq.object_id = dupPrereqDto.object_id;
            dbDupPrereq.met_notes = dupPrereqDto.met_notes;
            dbDupPrereq.pre_requisite_id = dupPrereqDto.pre_requisite_id;
            dbDupPrereq.prerequisite_met = dupPrereqDto.prerequisite_met;

            await _dbConnection.SaveChangesAsync();
            return _dataMapper.DupPrereqDtoMapper(dbDupPrereq);
        }

        public async Task<int> DeleteDupPrereq(int id)
        {
            var data = await _dbConnection.DupPrereqs.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.DupPrereqs.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllDupPrereqs(int dupId)
        {
            var data = _dbConnection.DupPrereqs.Where(p => p.DupId == dupId);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.DupPrereqs.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<SecondaryUse> GetQueryableSecondaryUses()
        {
            return _dbConnection.SecondaryUses;
        }

        public async Task<ICollection<SecondaryUseDto>> GetSecondaryUses(int dupId)
        {
            var data = _dbConnection.SecondaryUses.Where(p => p.DupId == dupId);
            return data.Any() ? _dataMapper.SecondaryUseDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<SecondaryUseDto> GetSecondaryUse(int id)
        {
            var secUse = await _dbConnection.SecondaryUses.FirstOrDefaultAsync(p => p.Id == id);
            return secUse != null ? _dataMapper.SecondaryUseDtoMapper(secUse) : null;
        }

        public async Task<SecondaryUseDto> CreateSecondaryUse(int dupId, SecondaryUseDto secondaryUseDto)
        {
            var secondaryUse = new SecondaryUse
            {
                DupId = dupId,
                created_on = DateTime.Now,
                secondary_use_type = secondaryUseDto.secondary_use_type,
                Publication = secondaryUseDto.Publication,
                doi = secondaryUseDto.doi,
                attribution_present = secondaryUseDto.attribution_present,
                Notes = secondaryUseDto.Notes
            };

            await _dbConnection.SecondaryUses.AddAsync(secondaryUse);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.SecondaryUseDtoMapper(secondaryUse);
        }

        public async Task<SecondaryUseDto> UpdateSecondaryUse(SecondaryUseDto secondaryUseDto)
        {
            var dbSecondaryUse =
                await _dbConnection.SecondaryUses.FirstOrDefaultAsync(p => p.Id == secondaryUseDto.Id);
            if (dbSecondaryUse == null) return null;
            
            dbSecondaryUse.secondary_use_type = secondaryUseDto.secondary_use_type;
            dbSecondaryUse.Publication = secondaryUseDto.Publication;
            dbSecondaryUse.doi = secondaryUseDto.doi;
            dbSecondaryUse.attribution_present = secondaryUseDto.attribution_present;
            dbSecondaryUse.Notes = secondaryUseDto.Notes;

            await _dbConnection.SaveChangesAsync();
            return _dataMapper.SecondaryUseDtoMapper(dbSecondaryUse);
        }

        public async Task<int> DeleteSecondaryUse(int id)
        {
            var data = await _dbConnection.SecondaryUses.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.SecondaryUses.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllSecondaryUses(int dupId)
        {
            var data = _dbConnection.SecondaryUses.Where(p => p.DupId == dupId);
            if (!data.Any()) return 0;

            var count = data.Count();
            _dbConnection.SecondaryUses.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<Dua> GetQueryableDua()
        {
            return _dbConnection.Duas;
        }

        public async Task<ICollection<DuaDto>> GetAllDua(int dupId)
        {
            var data = _dbConnection.Duas.Where(p => p.DupId == dupId);
            return data.Any() ? _dataMapper.DuaDtoBuilder(await data.ToArrayAsync()) : null;
        }

        public async Task<DuaDto> GetDua(int id)
        {
            var dua = await _dbConnection.Duas.FirstOrDefaultAsync(p => p.Id == id);
            return dua != null ? _dataMapper.DuaDtoMapper(dua) : null;
        }

        public async Task<DuaDto> CreateDua(int dupId, DuaDto duaDto)
        {
            var dua = new Dua
            {
                DupId = dupId,
                created_on = DateTime.Now,
                conforms_to_default = duaDto.conforms_to_default,
                Variations = duaDto.Variations,
                repo_as_proxy = duaDto.repo_as_proxy,
                repo_signatory_1 = duaDto.repo_signatory_1,
                repo_signatory_2 = duaDto.repo_signatory_2,
                provider_signatory_1 = duaDto.provider_signatory_1,
                provider_signatory_2 = duaDto.provider_signatory_2,
                requester_signatory_1 = duaDto.requester_signatory_1,
                requester_signatory_2 = duaDto.requester_signatory_2,
                Notes = duaDto.Notes
            };

            await _dbConnection.Duas.AddAsync(dua);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DuaDtoMapper(dua);
        }

        public async Task<DuaDto> UpdateDua(DuaDto duaDto)
        {
            var dbDua = await _dbConnection.Duas.FirstOrDefaultAsync(p => p.Id == duaDto.Id);
            if (dbDua == null) return null;
            
            dbDua.conforms_to_default = duaDto.conforms_to_default;
            dbDua.Variations = duaDto.Variations;
            dbDua.repo_as_proxy = duaDto.repo_as_proxy;
            dbDua.repo_signatory_1 = duaDto.repo_signatory_1;
            dbDua.repo_signatory_2 = duaDto.repo_signatory_2;
            dbDua.provider_signatory_1 = duaDto.provider_signatory_1;
            dbDua.provider_signatory_2 = duaDto.provider_signatory_2;
            dbDua.requester_signatory_1 = duaDto.requester_signatory_1;
            dbDua.requester_signatory_2 = duaDto.requester_signatory_2;
            dbDua.Notes = duaDto.Notes;

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DuaDtoMapper(dbDua);
        }

        public async Task<int> DeleteDua(int id)
        {
            var data = await _dbConnection.Duas.FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return 0;
            _dbConnection.Duas.Remove(data);
            await _dbConnection.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteAllDua(int dupId)
        {
            var data = _dbConnection.Duas.Where(p => p.DupId == dupId);
            if (!data.Any()) return 0;
            
            var count = data.Count();
            _dbConnection.Duas.RemoveRange(data);
            await _dbConnection.SaveChangesAsync();
            return count;
        }

        public IQueryable<Dup> GetQueryableDup()
        {
            return _dbConnection.Dups;
        }

        public async Task<ICollection<DupDto>> GetAllDup()
        {
            return _dbConnection.Dups.Any() ? _dataMapper.DupDtoBuilder(await _dbConnection.Dups.ToArrayAsync()) : null;
        }

        public async Task<DupDto> GetDup(int id)
        {
            var dup = await _dbConnection.Dups.FirstOrDefaultAsync(p => p.Id == id);
            return dup == null ? null : _dataMapper.DupDtoMapper(dup);
        }

        public async Task<ICollection<DupDto>> GetRecentDup(int limit)
        {
            if (!_dbConnection.Dups.Any()) return null;

            var recentDup = await _dbConnection.Dups.OrderByDescending(p => p.Id).Take(limit).ToArrayAsync();
            return _dataMapper.DupDtoBuilder(recentDup);
        }

        public async Task<DupDto> CreateDup(DupDto dupDto)
        {
            var dup = new Dup
            {
                created_on = DateTime.Now,
                org_id = dupDto.org_id,
                display_name = dupDto.display_name,
                status_id = dupDto.status_id,
                initial_contact_date = dupDto.initial_contact_date,
                set_up_completed = dupDto.set_up_completed,
                prereqs_met = dupDto.prereqs_met,
                dua_agreed_date = dupDto.dua_agreed_date,
                availability_requested = dupDto.availability_requested,
                availability_confirmed = dupDto.availability_confirmed,
                access_confirmed = dupDto.access_confirmed
            };

            await _dbConnection.Dups.AddAsync(dup);
            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DupDtoMapper(dup);
        }

        public async Task<DupDto> UpdateDup(DupDto dupDto)
        {
            var dbDup = await _dbConnection.Dups.FirstOrDefaultAsync(p => p.Id == dupDto.Id);
            if (dbDup == null) return null;
            
            dbDup.org_id = dupDto.org_id;
            dbDup.display_name = dupDto.display_name;
            dbDup.status_id = dupDto.status_id;
            dbDup.initial_contact_date = dupDto.initial_contact_date;
            dbDup.set_up_completed = dupDto.set_up_completed;
            dbDup.prereqs_met = dupDto.prereqs_met;
            dbDup.dua_agreed_date = dupDto.dua_agreed_date;
            dbDup.availability_requested = dupDto.availability_requested;
            dbDup.availability_confirmed = dupDto.availability_confirmed;
            dbDup.access_confirmed = dupDto.access_confirmed;

            await _dbConnection.SaveChangesAsync();
            
            return _dataMapper.DupDtoMapper(dbDup);
        }

        public async Task<int> DeleteDup(int id)
        {
            var dup = await _dbConnection.Dups.FirstOrDefaultAsync(p => p.Id == id);
            if (dup == null) return 0;
            
            await DeleteAllDua(id);
            await DeleteAllDupObjects(id);
            await DeleteAllDupPrereqs(id);
            await DeleteAllSecondaryUses(id);

            _dbConnection.Dups.Remove(dup);
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

        public async Task<PaginationResponse<DupDto>> PaginateDup(PaginationRequest paginationRequest)
        {
            var dup = new List<DupDto>();

            var skip = CalculateSkip(paginationRequest.Page, paginationRequest.Size);

            var query = _dbConnection.Dups
                .AsNoTracking()
                .OrderBy(arg => arg.Id);

            var data = await query
                .Skip(skip)
                .Take(paginationRequest.Size)
                .ToListAsync();
            
            var total = await query.CountAsync();

            if (data is {Count: > 0})
            {
                foreach (var dupRecord in data)
                {
                    dup.Add(_dataMapper.DupDtoMapper(dupRecord));
                }
            }

            return new PaginationResponse<DupDto>
            {
                Total = total,
                Data = dup
            };
        }

        public async Task<PaginationResponse<DupDto>> FilterDupByTitle(FilteringByTitleRequest filteringByTitleRequest)
        {
            var dup = new List<DupDto>();

            var skip = CalculateSkip(filteringByTitleRequest.Page, filteringByTitleRequest.Size);

            var query = _dbConnection.Dups
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
                foreach (var dupRecord in data)
                {
                    dup.Add(_dataMapper.DupDtoMapper(dupRecord));
                }
            }

            return new PaginationResponse<DupDto>
            {
                Total = total,
                Data = dup
            };
        }

        public async Task<int> GetTotalDup()
        {
            return await _dbConnection.Dups.AsNoTracking().CountAsync();
        }

        public async Task<int> GetUncompletedDup()
        {
            return await _dbConnection.Dups.AsNoTracking().Where(p => p.status_id == 16).CountAsync();
        }
        */
    }
