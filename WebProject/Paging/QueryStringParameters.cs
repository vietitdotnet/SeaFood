

namespace WebProject.Paging
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = 50;
        public virtual int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public virtual int PageSize
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
