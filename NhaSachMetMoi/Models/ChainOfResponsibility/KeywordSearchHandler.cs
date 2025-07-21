using System.Web.Mvc;
using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.Mvc;



public class KeywordSearchHandler : ISearchHandler
{
    private ISearchHandler _next;

    public ISearchHandler SetNext(ISearchHandler next)
    {
        _next = next;
        return next;
    }

    public List<Sach> Handle(List<Sach> saches, FormCollection f)
    {
        string keyword = f["txtTim"];
        if (!string.IsNullOrEmpty(keyword))
        {
            string normalized = RemoveDiacritics(keyword);
            saches = saches.Where(s => RemoveDiacritics(s.TenSach).Contains(normalized)).ToList();
        }

        return _next?.Handle(saches, f) ?? saches;
    }

    private string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC).ToLower();
    }
}
