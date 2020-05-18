using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Neo4j.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppDatabase appDatabase;
        private readonly IMapper mapper;
        public UserRepository(IAppDatabase appDatabase, IMapper mapper)
        {
            this.appDatabase = appDatabase;
            this.mapper = mapper;
        }

        public async Task<bool> CreateRating(Rating rating, string nameIndetifier)
        {
            var query = $"MATCH (u:User{{entityid: \"{nameIndetifier}\" }}), (m:Movie {{tmdbid: {rating.TmdbId} }}) " +
                $"MERGE(m) <-[hr: RATED]-(u) SET hr.score = {rating.Value} WITH m MATCH(m) <-[r: RATED]-(user: User) " +
                $"WITH m, count(r) as counter, avg(r.score) as avger SET m.ratecount = counter, m.rateavg = avger " +
                $"WITH m MATCH (u)-[r:RATED]->() WITH u, avg(r.score) as avger SET u.rateavg = avger";
            return await appDatabase.ExecuteWriteQuery(query);
        }

        public async Task<User> InsertAndGetUser(string nameIdentifier)
        {
            var query = $"MERGE (u :User {{entityid: \"{nameIdentifier}\"}}) ON CREATE SET u.entityid = \"{nameIdentifier}\" RETURN u";
            var nodeUser = await appDatabase.ExecuteReadQuery<INode>(query);
            var user = mapper.Map<User>(nodeUser.First());
            return user;
        }

        public async Task<bool> WriteReview(Review review, string nameIdentifier)
        {
            var query = $"MATCH (u:User {{ entityid: \"{nameIdentifier}\" }}), (m:Movie {{tmdbid: {review.TmdbId} }}) MERGE (u)-[hr:HAS_REVIEW]->(m) SET hr.text = \"{review.Text}\", hr.date = \"{review.Date}\", hr.username = \"{review.UserName}\", hr.entityid = \"{review.EntityId}\"";
            return await appDatabase.ExecuteWriteQuery(query);
        }

        public async Task<float?> GetRating(string nameidentifier, int tmdbId)
        {
            var query = $"MATCH (u:User{{entityid: \"{nameidentifier}\" }})-[r:RATED]->(m:Movie{{tmdbid: {tmdbId} }}) RETURN r.score";
            var valueList = await appDatabase.ExecuteReadQuery<float>(query);
            var value = valueList.FirstOrDefault();
            return value;
        }
    }
}
