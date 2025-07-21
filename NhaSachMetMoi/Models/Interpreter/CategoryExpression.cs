using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Models.Interpreter
{
    public class CategoryExpression : IExpression
    {
        private readonly string _category;
        private IExpression _next;

        public CategoryExpression(string category)
        {
            _category = RemoveDiacritics(category);
        }

        public IExpression SetNext(IExpression next)
        {
            _next = next;
            return next;
        }

        public List<Sach> Interpret(List<Sach> books, FormCollection f)
        {
            books = books.Where(b => RemoveDiacritics(b.TL.TenTL).Contains(_category)).ToList();
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
