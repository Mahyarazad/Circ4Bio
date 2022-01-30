using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _0_Framework.Application
{
    public static class GenerateCurrencyList
    {
        public static List<string> GetList()
        {
            List<string> cultureList = new List<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    RegionInfo region = new RegionInfo(culture.Name);
                    if (!(cultureList.Contains(region.ISOCurrencySymbol)))
                        cultureList.Add(region.ISOCurrencySymbol);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}" +
                                      $"For{0} a specific culture name is required {culture.Name}");
                }
            }
            cultureList.Sort();
            return cultureList;
        }
    }
}