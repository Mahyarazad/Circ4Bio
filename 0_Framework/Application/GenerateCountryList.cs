using System.Collections.Generic;
using System.Globalization;

namespace _0_Framework.Application
{
    public static class GenerateCountryList
    {
        public static List<string> GetList()
        {
            List<string> list = new List<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.SpecificCultures);
            foreach (CultureInfo info in cultures)
            {
                if (info.LCID != 127 && !info.IsNeutralCulture)
                {
                    RegionInfo info2 = new RegionInfo(info.LCID);

                    if (!(list.Contains(info2.EnglishName)))
                    {
                        list.Add(info2.EnglishName);
                    }
                }
            }
            list.Sort();
            return list;

        }
    }
}