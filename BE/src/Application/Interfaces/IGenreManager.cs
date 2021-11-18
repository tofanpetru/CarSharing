using Domain.Entities;
using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IGenreManager
    {
        ICollection<GenreDTO> GetAll();
        bool ValidateGenres(IEnumerable<string> genres);
        public List<string> GetGenresByName(string term);
        public Genre GetByName(string genre);
    }
}
