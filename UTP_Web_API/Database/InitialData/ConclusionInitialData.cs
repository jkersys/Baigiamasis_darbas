using UTP_Web_API.Models;

namespace UTP_Web_API.Database.InitialData
{
    public class ConclusionInitialData
    {
        public static readonly Conclusion [] DataSeed = new Conclusion[] {
            new Conclusion
            {
               ConclusionId = 1,
               Decision = "Skundas atmestas"
            },
              new Conclusion
            {
               ConclusionId = 2,
               Decision = "Skundas priimtas"
            },
              new Conclusion
            {
               ConclusionId = 3,
               Decision = "Pažeidimų nenustatyta"
            },
              new Conclusion
            {
               ConclusionId = 4,
               Decision = "Nutraukta dėl mažareikšmiškumo"
            },
               new Conclusion
            {
               ConclusionId = 5,
               Decision = "Nustatyti pažeidimai, byla perduota komisijai"
            },
        };
    }
}
   
