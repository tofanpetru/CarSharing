using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Application.Manager
{
    public class AuthorManager : IAuthorManager
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        public AuthorManager(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public ICollection<AuthorDTO> GetAllNonPendingAuthors()
        {
            return _mapper.Map<ICollection<AuthorDTO>>(_authorRepository.GetAllNonPendingAuthors());
        }

        public bool IsExistingAuthor(string authorName)
        {
            return _authorRepository.FindByName(authorName) != 0;
        }

        public ICollection<AuthorDTO> GetAll()
        {
            return _mapper.Map<ICollection<AuthorDTO>>(_authorRepository.GetAll());
        }

        public int GetAuthorBooksNumber(int id)
        {
            return _authorRepository.GetAuthorBooksNumber(id);
        }

        public void RemoveAuthorById(int id)
        {
            _authorRepository.Remove(_authorRepository.Get(id));
        }

        public bool IsPending(string authorFullName)
        {
            var author = _authorRepository.GetByName(authorFullName);
            if (author != null)
                return author.IsPending;
            return false;
        }

        public void Add(Author author)
        {
            _authorRepository.Add(author);
            _authorRepository.SaveChanges();
        }

        public Author AddAuthorFromPending(Author author)
        {
            if (author.IsPending == true)
                author.IsPending = false;
            _authorRepository.SaveChanges();
            return author;
        }

        public bool UpdatePendingAuthor(Author author, string newAuthorName)
        {
            if (author.IsPending == true && !IsExistingAuthor(newAuthorName))
            {
                author.FullName = newAuthorName;
                _authorRepository.SaveChanges();
                return true;
            }
            else 
                return false;
        }

        public string AddAuthorNotPending(string fullName)
        {
            Author newAuthor = new();
            if (_authorRepository.GetByName(fullName) == null)
            {
                newAuthor.FullName = fullName;
                newAuthor.IsPending = false;
                _authorRepository.Add(newAuthor);
                _authorRepository.SaveChanges();
            }
            else if (_authorRepository.GetByName(fullName).IsPending)
            {
                newAuthor = _authorRepository.GetByName(fullName);
                newAuthor.IsPending = false;
                foreach(Book book in newAuthor.Books)
                {
                    book.IsPending = false;
                    book.IsAvailable = true;
                }
                _authorRepository.SaveChanges();
            }
            return newAuthor.FullName;
        }

        public bool ExistsNotPending(string fullName)
        {
            var authors = _authorRepository.GetAllNonPendingAuthors();
            if (authors.Contains(_authorRepository.GetByName(fullName)))
                return true;
            else
                return false;
        }

        public Author GetFromNotPending(string fullName)
        {
            return _authorRepository.GetFromNotPending(fullName);
        }

        public void DeleteAuthor(Author author)
        {
            _authorRepository.Remove(author);
            _authorRepository.SaveChanges();
        }
        public List<string> GetAuthorsByName(string term)
        {
            return _authorRepository.GetAllNonPendingAuthors().Where(a => a.FullName.ToUpper().Contains(term.ToUpper())).Select(x => x.FullName).ToList();
        }
        
        public Author GetByName(string fullName)
        {
            return _authorRepository.GetByName(fullName);
        }
    }
}
