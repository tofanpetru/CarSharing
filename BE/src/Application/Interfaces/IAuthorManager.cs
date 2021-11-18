using Domain.Entities;
using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IAuthorManager
    {
        ICollection<AuthorDTO> GetAllNonPendingAuthors();
        bool IsExistingAuthor(string name);
        public ICollection<AuthorDTO> GetAll();
        public int GetAuthorBooksNumber(int id);
        public void RemoveAuthorById(int id);
        public bool IsPending(string authorFullName);
        public void Add(Author author);
        public Author AddAuthorFromPending(Author author);
        public bool UpdatePendingAuthor(Author author, string newAuthorName);
        public string AddAuthorNotPending(string fullName);
        public Author GetByName(string fullName);
        public bool ExistsNotPending(string fullName);
        public Author GetFromNotPending(string fullName);
        public void DeleteAuthor(Author author);
        public List<string> GetAuthorsByName(string term);
    }
}
