using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Manager
{
    public class LanguageManager : ILanguageManager
    {
        private readonly IMapper _mapper;
        private readonly ILanguageRepository _languageRepository;
        public LanguageManager(IMapper mapper, ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public ICollection<LanguageDTO> GetAll()
        {
            return _mapper.Map<ICollection<LanguageDTO>>(_languageRepository.GetAll());
        }

        public bool ValidateLanguage(string language)
        {
            var allLanguages = _languageRepository.GetAll().Select(i => i.Name);
            foreach (var lang in allLanguages)
                if (lang == language)
                    return true;
            return false;
        }
        public List<string> GetLanguagesByName(string term)
        {
            return _languageRepository.GetAll().Where(l => l.Name.ToUpper().Contains(term.ToUpper())).Select(x => x.Name).ToList();
        }

        public Language GetByName(string language)
        {
            return _languageRepository.GetByName(language);
        }
    }
}
