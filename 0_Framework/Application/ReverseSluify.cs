using System.Linq;
using System.Text.RegularExpressions;

namespace _0_Framework.Application
{
    public static class ReverseSlugify
    {
        public static string Reversing(this string value)
        {
            string Word;
            string title = value;
            title = Regex.Replace(title, @"\-+", " ");
            Word = string.Join(" ", title.Split(' ').ToList()
                .ConvertAll(word =>
                    word.Substring(0, 1).ToUpper() + word.Substring(1)));
            return Word;
        }
    }
}