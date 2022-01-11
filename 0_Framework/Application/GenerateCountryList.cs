using System.Collections.Generic;
using System.Globalization;

namespace _0_Framework.Application
{
    public static class GenerateCountryList
    {
        public static List<string> GetList()
        {
            List<string> cultureList = new List<string>();

            //create an array of CultureInfo to hold all the cultures found, these include the users local cluture, and all the
            //cultures installed with the .Net Framework
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            //loop through all the cultures found
            foreach (CultureInfo culture in cultures)
            {
                //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                //to the RegionInfo contructor to gain access to the information for that culture
                RegionInfo region = new RegionInfo(culture.LCID);
                //make sure out generic list doesnt already
                //contain this country
                if (!(cultureList.Contains(region.EnglishName)))
                    //not there so add the EnglishName (http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.englishname.aspx)
                    //value to our generic list
                    cultureList.Add(region.EnglishName);
            }
            cultureList.Sort();
            return cultureList;

        }
    }
}