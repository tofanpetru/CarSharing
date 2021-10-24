namespace Domain.Entities.Pagination
{
    public class CarParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        #region private
        private int _pageSize = 10;
        #endregion

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
