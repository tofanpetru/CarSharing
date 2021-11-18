using Domain.Entities;
using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IWishBookManager
    {
        WishBookResultDTO AddWishBook(WishBookDTO wishBookDTO, User user);
        bool DeleteWishBook(User user, int id);
        ICollection<WishBookDTO> GetWishedBooks(int firstItem, int pageSize, string sortOrder, string searchString, User user);
        bool RemoveExistingBookFromWishList(string title, string author, int bookId);
        WishBookResultDTO UpdateWishBook(WishBookDTO wishBook, User user);
    }
}
