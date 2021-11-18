using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Manager
{
    public class GenreManager : IGenreManager
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreManager(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public ICollection<GenreDTO> GetAll()
        {
            return _mapper.Map<ICollection<GenreDTO>>(_genreRepository.GetAll());
        }

        public bool ValidateGenres(IEnumerable<string> genres)
        {
            var allGenres = _genreRepository.GetAll().Select(i => i.Name);
            foreach (var genre in genres)
            {
                if (!allGenres.Contains(genre))
                    return false;
            }
            return true;
        }
        public List<string> GetGenresByName(string term)
        {
            var genres = _genreRepository.GetAll().Where(a => a.Name.ToUpper().Contains(term.ToUpper()));

            return genres.Select(x => x.Name).ToList();
        }

        public Genre GetByName(string genre)
        {
            return _genreRepository.GetByName(genre);
        }
    }
}
