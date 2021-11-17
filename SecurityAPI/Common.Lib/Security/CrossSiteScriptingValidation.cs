using Common.Lib.Extenstion;
using Microsoft.AspNetCore.Http;

namespace Common.Lib.Security;

/// <summary>
/// Imported from System.Web.CrossSiteScriptingValidation Class
/// </summary>
public static class CrossSiteScriptingValidation
{
    private static readonly char[] StartingChars = {'<', '&'};

    #region Public methods

    public static bool IsDangerousString(string s, out int matchIndex)
    {
        matchIndex = 0;

        for (var i = 0;;)
        {
            // Look for the start of one of our patterns 
            var n = s.IndexOfAny(StartingChars, i);

            // If not found, the string is safe
            if (n < 0) return false;

            // If it's the last char, it's safe 
            if (n == s.Length - 1) return false;

            matchIndex = n;

            switch (s[n])
            {
                case '<':
                    // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                    if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?') return true;
                    break;
                case '&':
                    // If the & is followed by a #, it's unsafe (e.g. S) 
                    if (s[n + 1] == '#') return true;
                    break;
            }

            // Continue searching
            i = n + 1;
        }
    }

    #endregion

    #region Private methods

    private static bool IsAtoZ(char c)
    {
        return c is >= 'a' and <= 'z' || c is >= 'A' and <= 'Z';
    }

    #endregion

    public static void AddHeaders(this IHeaderDictionary headers)
    {
        if (headers["P3P"].IsNullOrEmpty())
            headers.Add("P3P", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
    }
}