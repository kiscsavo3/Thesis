using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Extensions;
using Domain.Internal.DTO;
using Domain.Relationships;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public Task<MovieEntity> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task UpsertAsync(MovieEntity movieEntity)
        {
            var nodeMovie = mapper.Map<Node>(movieEntity);
            var nodeMovieFromDb = UpsertNode(nodeMovie);
            var propertyInfos = movieEntity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.GetGenericArguments().Count() > 0)
                {
                    if (propertyInfo.PropertyType == typeof(List<GenreRelationship>)) { EntityDisassembler<GenreRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<KeyWordRelationship>)) {  EntityDisassembler<KeyWordRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<CastRelationship>)) {  EntityDisassembler<CastRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<CrewRelationship>)) {  EntityDisassembler<CrewRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<PersonRelationship>)) {  EntityDisassembler<PersonRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<ProductionCompanyRelationship>)) {  EntityDisassembler<ProductionCompanyRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<ProductionCountyRelationship>)) {  EntityDisassembler<ProductionCountyRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                    else if (propertyInfo.PropertyType == typeof(List<SpokenLanguageRelationship>)) {  EntityDisassembler<SpokenLanguageRelationship>(propertyInfo, movieEntity, nodeMovieFromDb); }
                }
            }
        }

        private void EntityDisassembler<T>(PropertyInfo propertyInfo, MovieEntity movieEntity, INode nodeMovieFromDb) where T : BaseRelationship
        {
            var relationships = (List<T>)propertyInfo.GetValue(movieEntity);
            foreach (var relationship in relationships)
            {
                var nodeGeneric = mapper.Map<Node>(relationship.EndNode);
                var nodeGenricFromDb =  UpsertNode(nodeGeneric);
                relationship.StartNodeId = nodeMovieFromDb.Id;
                relationship.EndNodeId = nodeGenricFromDb.Id;
                var relationshipToDb = mapper.Map<Relationship>(relationship);
                UpsertRelationship(relationshipToDb);
            }
        }

        private INode UpsertNode(Node node)
        {
            List<INode> nodesFromDb = null;
            var nodeGenericFromDb = appDatabase.GetNodes((string)node.Properties.GetValue("EntityId"), node.Label);
            if (nodeGenericFromDb.Count == 0) { nodesFromDb = appDatabase.AddNode(node); }
            else if (nodeGenericFromDb.Count == 1) { node.Id = nodeGenericFromDb.First().Id; nodesFromDb = appDatabase.UpdateNode(node); }
            else throw new Exception("More than one entity has found!");
            return nodesFromDb.First();
        }
        private IRelationship UpsertRelationship(Relationship relationship)
        {
            List<IRelationship> relationsFromDb = null;
            var relationshipFromDb = appDatabase.GetRelationship(relationship);
            if (relationshipFromDb.Count == 0) { relationsFromDb = appDatabase.AddRelationship(relationship); }
            else if (relationshipFromDb.Count == 1) { relationship.Id = relationshipFromDb.First().Id; relationsFromDb = appDatabase.UpdateRelationship(relationship); }
            else throw new Exception("More then one relation has found!");
            return relationsFromDb.First();
        }
    }
}
