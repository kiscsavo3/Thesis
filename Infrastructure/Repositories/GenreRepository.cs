using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Internal.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly IAppDatabase database;
        private readonly IMapper mapper;
        public GenreRepository(IAppDatabase database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }
        public async Task<GenreEntity> GetAsync(long id)
        {
            var inode = database.GetNode(id);
            return mapper.Map<GenreEntity>(inode.FirstOrDefault()) ?? throw new Exception("Entity not exist");
        }

        public async Task UpsertAsync(GenreEntity genre)
        {
            var inode = database.GetNodes(genre.EntityId, genre.GetType().Name);
            if (inode.Count == 0) 
            {
                var node = mapper.Map<Node>(genre);
                database.AddNode(node);
            }
            else
            {
                var node = mapper.Map<Node>(genre);
                database.UpdateNode(node);
            }
        }
    }
}
