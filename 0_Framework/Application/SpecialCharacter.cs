using System.ComponentModel.DataAnnotations;

namespace _0_Framework.Application
{
    public class SpecialChracter : RegularExpressionAttribute
    {
        public SpecialChracter() : base(@"^(?=.*\d)")
        {
        }
    }
}