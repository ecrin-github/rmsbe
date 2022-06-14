using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

    public interface   IDtpRepository
    {
        Task<bool> DtpDoesNotExistAsync(int id);
        Task<bool> DtpAttributeDoesNotExistAsync(int dup_id, string type_name, int id);
        Task<bool> DtpObjectDoesNotExistAsync(int dup_id, string sd_oid);
        Task<bool> DtpAttributePrereqDoesNotExistAsync(int dup_id, string sd_oid, int id);
        Task<bool> ObjectDatasetDoesNotExistAsync(string sd_oid, int id);
        
        Task<IEnumerable<DtpInDb>> GetAllDtpsAsync();

        /*
        IQueryable<Dtp> GetQueryableDtp();
        Task<ICollection<DtpDto>> GetAllDtp();
        Task<DtpDto> GetDtp(int id);
        Task<ICollection<DtpDto>> GetRecentDtp(int limit);
        Task<DtpDto> CreateDtp(DtpDto dtpDto);
        Task<DtpDto> UpdateDtp(DtpDto dtpDto);
        Task<int> DeleteDtp(int id);

        IQueryable<Dta> GetQueryableDta();
        Task<ICollection<DtaDto>> GetAllDta(int dtp_id);
        Task<DtaDto> GetDta(int id);
        Task<DtaDto> CreateDta(int dtp_id, DtaDto dtaDto);
        Task<DtaDto> UpdateDta(DtaDto dtaDto);
        Task<int> DeleteDta(int id);
        Task<int> DeleteAllDta(int dtp_id);
        
        IQueryable<DtpDataset> GetQueryableDtpDatasets();
        Task<ICollection<DtpDatasetDto>> GetAllDtpDatasets();
        Task<DtpDatasetDto> GetDtpDataset(int id);
        Task<DtpDatasetDto> CreateDtpDataset(string object_id, DtpDatasetDto dtpDatasetDto);
        Task<DtpDatasetDto> UpdateDtpDataset(DtpDatasetDto dtpDatasetDto);
        Task<int> DeleteDtpDataset(int id);
        
        IQueryable<DtpObject> GetQueryableDtpObjects();
        Task<ICollection<DtpObjectDto>> GetAllDtpObjects(int dtp_id);
        Task<DtpObjectDto> GetDtpObject(int id);
        Task<DtpObjectDto> CreateDtpObject(int dtp_id, string object_id, DtpObjectDto dtpObjectDto);
        Task<DtpObjectDto> UpdateDtpObject(DtpObjectDto dtpObjectDto);
        Task<int> DeleteDtpObject(int id);
        Task<int> DeleteAllDtpObjects(int dtp_id);
        
        IQueryable<DtpStudy> GetQueryableDtpStudies();
        Task<ICollection<DtpStudyDto>> GetAllDtpStudies(int dtp_id);
        Task<DtpStudyDto> GetDtpStudy(int id);
        Task<DtpStudyDto> CreateDtpStudy(int dtp_id, string study_id, DtpStudyDto dtpStudyDto);
        Task<DtpStudyDto> UpdateDtpStudy(DtpStudyDto dtpStudyDto);
        Task<int> DeleteDtpStudy(int id);
        Task<int> DeleteAllDtpStudies(int dtp_id);


        // Statistics
        Task<PaginationResponse<DtpDto>> PaginateDtp(PaginationRequest paginationRequest);
        Task<PaginationResponse<DtpDto>> FilterDtpByTitle(FilteringByTitleRequest filteringByTitleRequest);
        Task<int> GetTotalDtp();
        Task<int> GetUncompletedDtp();
        */
    }
