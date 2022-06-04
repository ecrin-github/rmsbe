using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rmsbe.Interfaces;
using rmsbe.Models;
using ContextService.Models.Ctx;
using ContextService.Models.DbConnection;
// using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ContextService : IContextService
    {
        private readonly ContextDbConnection _dbConnection;

        public ContextService(ContextDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<ICollection<Organisation>> GetOrganisations()
        {
            var organisations = await _dbConnection.Organisations.AnyAsync();
            if (!organisations) return null;
            return await _dbConnection.Organisations.ToArrayAsync();
        }

        public async Task<Organisation> GetOrganisation(int id)
        {
            var organisations = await _dbConnection.Organisations.AnyAsync();
            if (!organisations) return null;
            return await _dbConnection.Organisations
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ICollection<OrgAttribute>> GetOrgAttributes(int org_id)
        {
            var orgAttributes = await _dbConnection.OrgAttributes.AnyAsync();
            if (!orgAttributes) return null;
            return await _dbConnection.OrgAttributes
                .AsNoTracking()
                .Where(p => p.org_id == org_id).ToArrayAsync();
        }

        public async Task<ICollection<OrgLink>> GetOrgLinks(int org_id)
        {
            var orgLinks = await _dbConnection.OrgLinks.AnyAsync();
            if (!orgLinks) return null;
            return await _dbConnection.OrgLinks
                .AsNoTracking()
                .Where(p => p.org_id == org_id).ToArrayAsync();
        }

        public async Task<ICollection<OrgLocation>> GetOrgLocations(int org_id)
        {
            var orgLocations = await _dbConnection.OrgLocations.AnyAsync();
            if (!orgLocations) return null;
            return await _dbConnection.OrgLocations
                .AsNoTracking()
                .Where(p => p.org_id == org_id).ToArrayAsync();
        }

        public async Task<ICollection<OrgName>> GetOrgNames(int org_id)
        {
            var orgNames = await _dbConnection.OrgNames.AnyAsync();
            if (!orgNames) return null;
            return await _dbConnection.OrgNames
                .AsNoTracking()
                .Where(p => p.org_id == org_id).ToArrayAsync();
        }

        public async Task<ICollection<OrgRelationship>> GetOrgRelationships(int org_id)
        {
            var orgRels = await _dbConnection.OrgRelationships.AnyAsync();
            if (!orgRels) return null;
            return await _dbConnection.OrgRelationships
                .AsNoTracking()
                .Where(p => p.org_id == org_id).ToArrayAsync();
        }

        public async Task<ICollection<People>> GetPeople()
        {
            var people = await _dbConnection.People.AnyAsync();
            if (!people) return null;
            return await _dbConnection.People
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<People> GetPerson(int id)
        {
            var people = await _dbConnection.People.AnyAsync();
            if (!people) return null;
            return await _dbConnection.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ICollection<PeopleLink>> GetPersonLinks(int person_id)
        {
            var personLinks = await _dbConnection.PeopleLinks.AnyAsync();
            if (!personLinks) return null;
            return await _dbConnection.PeopleLinks
                .AsNoTracking()
                .Where(p => p.person_id == person_id).ToArrayAsync();
        }

        public async Task<ICollection<PeopleRole>> GetPersonRoles(int person_id)
        {
            var personRoles = await _dbConnection.PeopleRoles.AnyAsync();
            if (!personRoles) return null;
            return await _dbConnection.PeopleRoles
                .AsNoTracking()
                .Where(p => p.person_id == person_id).ToArrayAsync();
        }

        public async Task<ICollection<OrgTypeMembership>> GetOrgTypeMemberships(int org_id)
        {
            var orgTypeMemberships = await _dbConnection.OrgTypeMemberships.AnyAsync();
            if (!orgTypeMemberships) return null;
            return await _dbConnection.OrgTypeMemberships
                .AsNoTracking()
                .Where(p => p.org_id == org_id).ToArrayAsync();
        }

        public async Task<ICollection<GeogEntity>> GetGeogEntities()
        {
            if (!_dbConnection.GeogEntities.Any()) return null;
            return await _dbConnection.GeogEntities
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<GeogEntity> GetGeogEntity(int id)
        {
            if (!_dbConnection.GeogEntities.Any()) return null;
            return await _dbConnection.GeogEntities
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ICollection<PublishedJournal>> GetPublishedJournals()
        {
            if (!_dbConnection.PublishedJournals.Any()) return null;
            return await _dbConnection.PublishedJournals
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<PublishedJournal> GetPublishedJournal(int id)
        {
            if (!_dbConnection.PublishedJournals.Any()) return null;
            return await _dbConnection.PublishedJournals
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.JournalId == id);
        }

        public async Task<ICollection<Organisation>> GetOrganisationsByName(string name)
        {
            if (!_dbConnection.Organisations.Any()) return null;
            return await _dbConnection.Organisations
                .AsNoTracking()
                .Where(p => p.DefaultName.ToLower().Contains(name.ToLower())).ToArrayAsync();
        }
    }
}