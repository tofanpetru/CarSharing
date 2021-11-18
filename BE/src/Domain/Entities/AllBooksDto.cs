namespace Domain.Entities
{
    public class AllBooksDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public bool IsPending { get; set; }
    }
}
