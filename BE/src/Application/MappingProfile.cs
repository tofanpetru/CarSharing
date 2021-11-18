using Application.Manager;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddBookDTO, Book>(MemberList.Source).ForPath(dest => dest.Language.Name, src => src.MapFrom(i => i.Language))
                                   .ForPath(dest => dest.Author.FullName, src => src.MapFrom(i => i.Author))
                                   .ForMember(dest => dest.Genres, src => src.Ignore()).AfterMap((src, dest, rc) =>
                                   {
                                       dest.Genres = new List<Genre>();
                                       foreach (var genre in src.Genres)
                                       {
                                           dest.Genres.Add(new Genre() { Name = genre });
                                       }
                                   }).
                                   ForSourceMember(i => i.Image, dest => dest.DoNotValidate())
                                   .ReverseMap();
            CreateMap<PendingRequestsDTO, Book>(MemberList.Source)
                                                .ForPath(dest => dest.Author.FullName, src => src.MapFrom(i => i.Author))
                                                .ForPath(dest => dest.Owner.UserName, src => src.MapFrom(i => i.Owner))
                                                .ReverseMap();
            CreateMap<BookDetailsDTO, Book>(MemberList.Source).ForPath(dest => dest.Language.Name, src => src.MapFrom(i => i.Language))
                                                .ForPath(dest => dest.Author.FullName, src => src.MapFrom(i => i.Author))
                                                .ForPath(dest => dest.Owner.UserName, src => src.MapFrom(i => i.Owner))
                                                .ForSourceMember(dest => dest.IsPending, src => src.DoNotValidate())
                                                .ReverseMap();
            CreateMap<BookUpdateDTO, Book>(MemberList.Source).ForPath(dest => dest.Language.Name, src => src.MapFrom(i => i.Language))
                                    .ForPath(dest => dest.Author.FullName, src => src.MapFrom(i => i.Author))
                                    .ForPath(dest => dest.Owner.UserName, src => src.MapFrom(i => i.Owner))
                                    .ReverseMap();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Language, LanguageDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<UserRegistrationDTO, User>(MemberList.Source).ForMember(dest => dest.NormalizedEmail, src => src.MapFrom(i => i.Email))
                                                  .ForMember(dest => dest.NormalizedUserName, src => src.MapFrom(i => i.UserName))
                                                  .ForSourceMember(i => i.Password, dest => dest.DoNotValidate());
            CreateMap<Book, AllBooksDto>(MemberList.Destination).ForPath(dest => dest.ImagePath, src => src.MapFrom(i => ServerUtils.GetBookCoverImageSrc(i.ImagePath))).ReverseMap();
            CreateMap<UserProfileDTO, User>(MemberList.Source).ReverseMap();
            CreateMap<Assignment, UserAssignmentsDTO>(MemberList.Destination).ForPath(dest => dest.BookTitle, src => src.MapFrom(i => i.Book.Title))
                                                    .ForPath(dest => dest.IsExtended, src => src.MapFrom(i => (i.Extend != null)))
                                                    .ForMember(dest => dest.CanBeExtended, src => src.Ignore())
                                                    .ReverseMap();
            CreateMap<WishBook, WishBookDTO>().ForMember(dest => dest.Users, opt => opt.MapFrom(so => so.Users.Select(x => x.UserName).ToList())).ReverseMap();
            CreateMap(typeof(IPagedList<>), typeof(IPagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
            CreateMap<ExtendAssignmentDTO, Extend>(MemberList.Source)
                                               .ForPath(dest => dest.Reason, src => src.MapFrom(i => i.Reason))
                                               .ForPath(dest => dest.EndDate, src => src.MapFrom(i => i.EndDate))
                                               .ForSourceMember(dest => dest.AssignmentId, src => src.DoNotValidate())
                                               .ReverseMap();

            CreateMap<ExtendAssignmentRqDTO, Assignment>(MemberList.Source)
                                   .ForPath(dest => dest.Id, src => src.MapFrom(i => i.Id))
                                   .ForPath(dest => dest.Extend.EndDate, src => src.MapFrom(i => i.EndDate))
                                   .ForPath(dest => dest.Book.Title, src => src.MapFrom(i => i.BookTitle))
                                   .ForPath(dest => dest.Assignee.UserName, src => src.MapFrom(i => i.AssigneeName))
                                   .ForPath(dest => dest.Extend.Reason, src => src.MapFrom(i => i.Reason))
                                   .ReverseMap();
            CreateMap<Review, GetReviewDto>(MemberList.Destination).ForPath(dest => dest.ReviewerAvatarPath, src => src.MapFrom(i => ServerUtils.GetUserAvatarImageSrc(i.User.ProfileImagePath)))
                                                                .ForPath(dest => dest.ReviewerUserName, src => src.MapFrom(i => i.User.UserName)).ReverseMap();
            CreateMap<Review, AddReviewDTO>(MemberList.Destination).ForPath(dest => dest.BookId, src => src.MapFrom(i => i.Book.Id)).ReverseMap();
            CreateMap<User, UserManageDTO>(MemberList.Destination).ForPath(dest => dest.Role, src => src.MapFrom(i => i.UserRoles.Any(r => r.Role.Name == AccessRole.SuperAdmin) ? AccessRole.SuperAdmin : i.UserRoles.Any(r => r.Role.Name == AccessRole.Admin) ? AccessRole.Admin : i.UserRoles.FirstOrDefault().Role.Name))
                                                                   .ReverseMap().ForMember(dest => dest.UserName, src => src.Ignore());

            CreateMap<ReviewListDTO, Review>(MemberList.Source).ForPath(dest => dest.Book.Title, src => src.MapFrom(i => i.BookTitle))
                                                    .ForPath(dest => dest.User.UserName, src => src.MapFrom(i => i.ReviewerUserName)).ReverseMap();
            CreateMap<NotificationDTO, Infrastructure.Persistance.Notification>(MemberList.Source).ReverseMap();
        }
    }
}
