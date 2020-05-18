using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IAppDatabase appDatabase;
        private readonly IMapper mapper;
        public MovieRepository(IAppDatabase appDatabase, IMapper mapper)
        {
            this.appDatabase = appDatabase;
            this.mapper = mapper;
        }
        public async Task<Movie> GetMovieById(int tmdbId)
        {
            var query = $"MATCH (m:Movie{{ tmdbid: {tmdbId}}}) RETURN m";
            var movieNode = await appDatabase.ExecuteReadQuery<INode>(query);
            var movie = movieNode.Select(n => mapper.Map<Movie>(n)).First();
            return movie;
        }

        public async Task<List<Movie>> GetMoviesByTitle(string title)
        {
            var query = $"MATCH(m: Movie) WHERE m.searchtitle CONTAINS reduce(result = \"\", n IN Split(toLower(\"{title}\"), ' ') | result + n) WITH m, m.ratecount * m.rateavg as pop ORDER BY pop DESC RETURN m LIMIT 66";
            //var query = $"CALL db.index.fulltext.queryNodes(\"title\", \"{title}*\") YIELD node, score WITH node, score RETURN node LIMIT 150";
            var movieNodes = await appDatabase.ExecuteReadQuery<INode>(query);
            var movie = movieNodes.Select(n => mapper.Map<Movie>(n)).ToList();
            return movie;

        }

        public async Task<List<Review>> GetReviews(int tmdbId)
        {
            var query = $"MATCH (m:Movie {{ tmdbid: {tmdbId} }})<-[hr: HAS_REVIEW]-(u:User) RETURN hr";
            var reviewRelationships = await appDatabase.ExecuteReadQuery<IRelationship>(query);
            var reviews = reviewRelationships.Select(r => mapper.Map<Review>(r)).ToList();
            reviews.ForEach(x => x.TmdbId = tmdbId);
            return reviews;
        }

        public async Task<List<string>> GetTitlesByTerm(string term)
        {
            //var query = $"CALL {{ MATCH(m: Movie) WHERE m.SearchTitle CONTAINS reduce(result = \"\", n IN Split(toLower(\"{term}\"), ' ') | result + n) AND m.Popularity is not null RETURN m, m.Popularity ORDER BY m.Popularity DESC }} RETURN m.Title LIMIT 5";
            var query = $"CALL db.index.fulltext.queryNodes(\"title\", \"{term}*\") YIELD node, score WITH node, score RETURN node.title LIMIT 7";
            var movieTitles = await appDatabase.ExecuteReadQuery<string>(query);
            return movieTitles;
        }

        public async Task<List<Movie>> GetRecommendationsByTag(int tmdbId)
        {
            var query = $"MATCH (g: Genre)<--(n: Movie {{ tmdbid: {tmdbId} }})-->(t: Tag)<--(m: Movie)-->(g) WITH m, count((m)) as cont ORDER BY cont DESC LIMIT 20 RETURN m";
            var movieNodes = await appDatabase.ExecuteReadQuery<INode>(query);
            var movies = movieNodes.Select(n => mapper.Map<Movie>(n)).ToList();
            return movies;
        }

        public async Task<List<Movie>> GetDiscoveredMovies(string entityid)
        {
            var query = $"MATCH (origin:User{{entityid:'{entityid}'}})-[r:RATED]->(m1:Movie) " +
            $"MATCH(m1)<-[r1: RATED]-(user)-[r2: RATED]->(m2) " +
            $"WITH m1, r, algo.similarity.cosine(collect(r1.score - user.rateavg), collect(r2.score - user.rateavg)) as sim, m2, count(user) as ucount " +
            $"WHERE sim > 0.8 " +
            $"WITH m2 as reco, sim* r.score as score, ucount " +
            $"WHERE score > 4.0 WITH reco, score, ucount " +
            $"ORDER BY ucount DESC LIMIT 21 " +
            $"RETURN reco";
            var movieNodes = await appDatabase.ExecuteReadQuery<INode>(query);
            var movie = movieNodes.Select(n => mapper.Map<Movie>(n)).ToList();
            return movie;
        }

        public async Task<List<Movie>> GetRecMovies(string entityid)
        {
            var query = $"MATCH (p1:User {{entityid: '{entityid}'}})-[rated:RATED]->(movie) " +
            $"WITH p1, algo.similarity.asVector(movie, rated.score) AS p1Vector, collect(id(movie)) AS p1JVector " +
            $"MATCH(p2: User) -[rated: RATED]->(movie) " +
            $"WHERE p2<> p1 " +
            $"WITH p1, p2, p1Vector, p1JVector, algo.similarity.asVector(movie, rated.score) AS p2Vector, collect(id(movie)) AS p2JVector " +
            $"WITH p1 AS from, p2 AS to, algo.similarity.pearson(p1Vector, p2Vector, {{ vectorType: \"maps\"}}) AS similarity, algo.similarity.jaccard(p1JVector, p2JVector) AS similarityj " +
            $"WHERE similarity > 0.6 " +
            $"WITH from, to, similarity, similarityj " +
            $"ORDER BY similarityj DESC LIMIT 50 " +
            $"MATCH (to)-[r: RATED]->(reco: Movie) " +
            $"WHERE NOT (from)-->(reco) " +
            $"WITH reco as reco, SUM(r.score * similarity - to.rateavg) / SUM(similarity) + from.rateavg as score, COUNT(r) as counter " +
            $"WHERE score > 4.0 " +
            $"WITH reco, score, counter " +
            $"ORDER BY counter DESC LIMIT 21 " +
            $"RETURN reco";
            var movieNodes = await appDatabase.ExecuteReadQuery<INode>(query);
            var movie = movieNodes.Select(n => mapper.Map<Movie>(n)).ToList();
            return movie;
        }

        public async Task<List<Movie>> GetDefaultMovies(string entityid)
        {
            var query = $"MATCH(u: User) -[r: RATED]->(m: Movie) WHERE NOT(:User {{ entityid: '{entityid}'}})-[:RATED]->(m) " +
                $"WITH distinct m, m.rateavg* log(m.ratecount) as score ORDER BY score DESC LIMIT 21 RETURN m";
            var movieNodes = await appDatabase.ExecuteReadQuery<INode>(query);
            var movie = movieNodes.Select(n => mapper.Map<Movie>(n)).ToList();
            return movie;
        }
    }
}
