using System.Collections.Generic;
using System.Linq;

namespace NhaSachMetMoi.Models.Composite
{
    public class AllFilter : IFilter
    {
        private readonly int? _category;
        private readonly string _keyword;
        private readonly decimal? _minPrice;
        private readonly decimal? _maxPrice;

        public AllFilter(int? category, string keyword, decimal? minPrice = null, decimal? maxPrice = null)
        {
            _category = category;
            _keyword = keyword;
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public IEnumerable<Sach> Apply(IEnumerable<Sach> books)
        {
            var composite = new CompositeFilter();

            // Lọc theo thể loại nếu có
            if (_category.HasValue)
                composite.Add(new CategoryFilter(_category));

            // Lọc theo từ khóa nếu có
            if (!string.IsNullOrWhiteSpace(_keyword))
                composite.Add(new NameFilter(_keyword));

            

       

            return composite.Apply(books);
        }
    }
}
