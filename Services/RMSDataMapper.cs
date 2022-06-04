using System.Collections.Generic;
using System.Linq;
using RmsService.DTO;
using RmsService.Interfaces;
using RmsService.Models;

namespace RmsService.Helpers
{
    public class DataMapper : IDataMapper
    {
        public ICollection<DtaDto> DtaDtoBuilder(ICollection<Dta> dtas)
        {
            return dtas is not { Count: > 0 } ? null : dtas.Select(DtaDtoMapper).ToList();
        }

        public DtaDto DtaDtoMapper(Dta dta)
        {
            if (dta == null) return null;
             
            var dtaDto = new DtaDto
            {
                Id = dta.Id,
                dtp_id = dta.dtp_id,
                created_on = dta.created_on,
                conforms_to_default = dta.conforms_to_default,
                Variations = dta.Variations,
                repo_signatory_1 = dta.repo_signatory_1,
                repo_signatory_2 = dta.repo_signatory_2,
                provider_signatory_1 = dta.provider_signatory_1,
                provider_signatory_2 = dta.provider_signatory_2,
                Notes = dta.Notes
            };

            return dtaDto;
        }

        public ICollection<DtpDto> DtpDtoBuilder(ICollection<Dtp> dtps)
        {
            return dtps is not { Count: > 0 } ? null : dtps.Select(DtpDtoMapper).ToList();
        }

        public DtpDto DtpDtoMapper(Dtp dtp)
        {
            if (dtp == null) return null;
            
            var dtpDto = new DtpDto
            {
                Id = dtp.Id,
                created_on = dtp.created_on,
                org_id = dtp.org_id,
                display_name = dtp.display_name,
                status_id = dtp.status_id,
                initial_contact_date = dtp.initial_contact_date,
                set_up_completed = dtp.set_up_completed,
                md_access_granted = dtp.md_access_granted,
                md_complete_date = dtp.md_complete_date,
                dta_agreed_date = dtp.dta_agreed_date,
                upload_access_requested = dtp.upload_access_requested,
                upload_access_confirmed = dtp.upload_access_confirmed,
                uploads_complete = dtp.uploads_complete,
                qc_checks_completed = dtp.qc_checks_completed,
                md_integrated_with_mdr = dtp.md_integrated_with_mdr,
                availability_requested = dtp.availability_requested,
                availability_confirmed = dtp.availability_confirmed
            };

            return dtpDto;
        }

        public ICollection<DtpDatasetDto> DtpDatasetDtoBuilder(ICollection<DtpDataset> dtpDatasets)
        {
            return dtpDatasets is not { Count: > 0 } ? null : dtpDatasets.Select(DtpDatasetDtoMapper).ToList();
        }

        public DtpDatasetDto DtpDatasetDtoMapper(DtpDataset dtpDataset)
        {
            if (dtpDataset == null) return null;
            
            var dtpDatasetDto = new DtpDatasetDto
            {
                Id = dtpDataset.Id,
                object_id = dtpDataset.object_id,
                created_on = dtpDataset.created_on,
                legal_status_id = dtpDataset.legal_status_id,
                legal_status_text = dtpDataset.legal_status_text,
                legal_status_path = dtpDataset.legal_status_path,
                desc_md_check_by = dtpDataset.desc_md_check_by,
                desc_md_check_date = dtpDataset.desc_md_check_date,
                desc_md_check_status_id = dtpDataset.desc_md_check_status_id,
                deident_check_by = dtpDataset.deident_check_by,
                deident_check_date = dtpDataset.deident_check_date,
                deident_check_status_id = dtpDataset.deident_check_status_id,
                Notes = dtpDataset.Notes
            };

            return dtpDatasetDto;
        }

        public ICollection<DtpObjectDto> DtpObjectDtoBuilder(ICollection<DtpObject> dtpObjects)
        {
            return dtpObjects is not { Count: > 0 } ? null : dtpObjects.Select(DtpObjectDtoMapper).ToList();
        }

        public DtpObjectDto DtpObjectDtoMapper(DtpObject dtpObject)
        {
            if (dtpObject == null) return null;
            
            var dtpObjectDto = new DtpObjectDto
            {
                Id = dtpObject.Id,
                dtp_id = dtpObject.dtp_id,
                object_id = dtpObject.object_id,
                created_on = dtpObject.created_on,
                is_dataset = dtpObject.is_dataset,
                access_type_id = dtpObject.access_type_id,
                download_allowed = dtpObject.download_allowed,
                access_details = dtpObject.access_details,
                requires_embargo_period = dtpObject.requires_embargo_period,
                embargo_end_date = dtpObject.embargo_end_date,
                embargo_still_applies = dtpObject.embargo_still_applies,
                access_check_status_id = dtpObject.access_check_status_id,
                access_check_date = dtpObject.access_check_date,
                access_check_by = dtpObject.access_check_by,
                md_check_status_id = dtpObject.md_check_status_id,
                md_check_by = dtpObject.md_check_by,
                md_check_date = dtpObject.md_check_date,
                Notes = dtpObject.Notes
            };

            return dtpObjectDto;
        }

        public ICollection<DtpStudyDto> DtpStudyDtoBuilder(ICollection<DtpStudy> dtpStudies)
        {
            return dtpStudies is not { Count: > 0 } ? null : dtpStudies.Select(DtpStudyDtoMapper).ToList();
        }

        public DtpStudyDto DtpStudyDtoMapper(DtpStudy dtpStudy)
        {
            if (dtpStudy == null) return null;
            
            var dtpStudyDto = new DtpStudyDto
            {
                Id = dtpStudy.Id,
                dtp_id = dtpStudy.dtp_id,
                study_id = dtpStudy.study_id,
                created_on = dtpStudy.created_on,
                md_check_status_id = dtpStudy.md_check_status_id,
                md_check_by = dtpStudy.md_check_by,
                md_check_date = dtpStudy.md_check_date
            };

            return dtpStudyDto;
        }

        public ICollection<DuaDto> DuaDtoBuilder(ICollection<Dua> duas)
        {
            return duas is not { Count: > 0 } ? null : duas.Select(DuaDtoMapper).ToList();
        }

        public DuaDto DuaDtoMapper(Dua dua)
        {
            if (dua == null) return null;
            
            var duaDto = new DuaDto
            {
                Id = dua.Id,
                DupId = dua.DupId,
                created_on = dua.created_on,
                conforms_to_default = dua.conforms_to_default,
                Variations = dua.Variations,
                repo_as_proxy = dua.repo_as_proxy,
                repo_signatory_1 = dua.repo_signatory_1,
                repo_signatory_2 = dua.repo_signatory_2,
                provider_signatory_1 = dua.provider_signatory_1,
                provider_signatory_2 = dua.provider_signatory_2,
                requester_signatory_1 = dua.requester_signatory_1,
                requester_signatory_2 = dua.requester_signatory_2,
                Notes = dua.Notes
            };

            return duaDto;
        }

        public ICollection<DupDto> DupDtoBuilder(ICollection<Dup> dups)
        {
            return dups is not { Count: > 0 } ? null : dups.Select(DupDtoMapper).ToList();
        }

        public DupDto DupDtoMapper(Dup dup)
        {
            if (dup == null) return null;
            
            var dupDto = new DupDto
            {
                Id = dup.Id,
                created_on = dup.created_on,
                org_id = dup.org_id,
                display_name = dup.display_name,
                status_id = dup.status_id,
                initial_contact_date = dup.initial_contact_date,
                set_up_completed = dup.set_up_completed,
                prereqs_met = dup.prereqs_met,
                dua_agreed_date = dup.dua_agreed_date,
                availability_requested = dup.availability_requested,
                availability_confirmed = dup.availability_confirmed,
                access_confirmed = dup.access_confirmed
            };

            return dupDto;
        }

        public ICollection<DupObjectDto> DupObjectDtoBuilder(ICollection<DupObject> dupObjects)
        {
            return dupObjects is not { Count: > 0 } ? null : dupObjects.Select(DupObjectDtoMapper).ToList();
        }

        public DupObjectDto DupObjectDtoMapper(DupObject dupObject)
        {
            if (dupObject == null) return null;
            
            var dupObjectDto = new DupObjectDto
            {
                Id = dupObject.Id,
                DupId = dupObject.DupId,
                created_on = dupObject.created_on,
                object_id = dupObject.object_id,
                access_type_id = dupObject.access_type_id,
                access_details = dupObject.access_details,
                Notes = dupObject.Notes
            };

            return dupObjectDto;
        }

        public ICollection<DupPrereqDto> DupPrereqDtoBuilder(ICollection<DupPrereq> dupPrereqs)
        {
            return dupPrereqs is not { Count: > 0 } ? null : dupPrereqs.Select(DupPrereqDtoMapper).ToList();
        }

        public DupPrereqDto DupPrereqDtoMapper(DupPrereq dupPrereq)
        {
            if (dupPrereq == null) return null;
            
            var dupPrereqDto = new DupPrereqDto
            {
                Id = dupPrereq.Id,
                DupId = dupPrereq.DupId,
                created_on = dupPrereq.created_on,
                object_id = dupPrereq.object_id,
                met_notes = dupPrereq.met_notes,
                pre_requisite_id = dupPrereq.pre_requisite_id,
                prerequisite_met = dupPrereq.prerequisite_met
            };

            return dupPrereqDto;
        }
        
        public ICollection<SecondaryUseDto> SecondaryUseDtoBuilder(ICollection<SecondaryUse> secondaryUses)
        {
            return secondaryUses is not { Count: > 0 } ? null : secondaryUses.Select(SecondaryUseDtoMapper).ToList();
        }

        public SecondaryUseDto SecondaryUseDtoMapper(SecondaryUse secondaryUse)
        {
            if (secondaryUse == null) return null;
            
            var secondaryUseDto = new SecondaryUseDto
            {
                Id = secondaryUse.Id,
                DupId = secondaryUse.DupId,
                created_on = secondaryUse.created_on,
                secondary_use_type = secondaryUse.secondary_use_type,
                Publication = secondaryUse.Publication,
                doi = secondaryUse.doi,
                attribution_present = secondaryUse.attribution_present,
                Notes = secondaryUse.Notes
            };

            return secondaryUseDto;
        }
        
        
        
        public ICollection<AccessPrereqDto> AccessPrereqDtoBuilder(ICollection<AccessPrereq> accessPrereq)
        {
            return accessPrereq is not { Count: > 0 } ? null : accessPrereq.Select(AccessPrereqDtoMapper).ToList();
        }

        public AccessPrereqDto AccessPrereqDtoMapper(AccessPrereq accessPrereq)
        {
            if (accessPrereq == null) return null;
            
            var accessPrereqDto = new AccessPrereqDto
            {
                Id = accessPrereq.Id,
                object_id = accessPrereq.object_id,
                pre_requisite_id = accessPrereq.pre_requisite_id,
                pre_requisite_notes = accessPrereq.pre_requisite_notes,
                created_on = accessPrereq.created_on
            };
            return accessPrereqDto;
        }
        
        public ICollection<ProcessNoteDto> ProcessNoteDtoBuilder(ICollection<ProcessNote> processNotes)
        {
            return processNotes is not { Count: > 0 } ? null : processNotes.Select(ProcessNoteDtoMapper).ToList();
        }

        public ProcessNoteDto ProcessNoteDtoMapper(ProcessNote processNote)
        {
            if (processNote == null) return null;
            
            var processNoteDto = new ProcessNoteDto
            {
                Id = processNote.Id,
                ProcessId = processNote.ProcessId,
                ProcessType = processNote.ProcessType,
                created_on = processNote.created_on
            };
            return processNoteDto;
        }

        public ICollection<ProcessPeopleDto> ProcessPeopleDtoBuilder(ICollection<ProcessPeople> processPeople)
        {
            return processPeople is not { Count: > 0 } ? null : processPeople.Select(ProcessPeopleDtoMapper).ToList();
        }

        public ProcessPeopleDto ProcessPeopleDtoMapper(ProcessPeople processPeople)
        {
            if (processPeople == null) return null;
            
            var processPeopleDto = new ProcessPeopleDto
            {
                Id = processPeople.Id,
                person_id = processPeople.person_id,
                ProcessId = processPeople.ProcessId,
                ProcessType = processPeople.ProcessType,
                IsAUser = processPeople.IsAUser,
                created_on = processPeople.created_on
            };
            return processPeopleDto;
        }
    }
}