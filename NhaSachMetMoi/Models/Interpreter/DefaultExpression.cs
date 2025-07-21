using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NhaSachMetMoi.Models.Interpreter
{
    public class DefaultExpression : IExpression
    {
        private readonly string _keyword;
        private IExpression _next;

        public DefaultExpression(string keyword)
        {
            _keyword = RemoveDiacritics(keyword);
        }

        public IExpression SetNext(IExpression next)
        {
            _next = next;
            return next;
        }

        public List<Sach> Interpret(List<Sach> books, FormCollection f)
        {
            books = books.Where(b =>
                RemoveDiacritics(b.TenSach).Contains(_keyword) ||
                RemoveDiacritics(b.TL.TenTL).Contains(_keyword)
            ).ToList();

            return _next?.Interpret(books, f) ?? books;
        }

        private string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC).ToLower();
        }
    }
}
