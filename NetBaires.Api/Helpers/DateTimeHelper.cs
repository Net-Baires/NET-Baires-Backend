using System;

namespace NetBaires.Api.Helpers
{
    public static class DateTimeHelper
    {

        public static DateTime FromExcelSerialDate(int SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1970, 1, 1).AddSeconds(SerialDate);
        }
    }
}