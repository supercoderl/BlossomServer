using BlossomServer.SharedKernel.Utils;

namespace BlossomServer.Application.ViewModels
{
    public sealed class DateRange
    {
        private string _dateStart = TimeZoneHelper.GetLocalTimeNow().ToString("yyyy-MM-dd");
        private string _dateEnd = TimeZoneHelper.GetLocalTimeNow().ToString("yyyy-MM-dd");

        public string DateStart
        {
            get => _dateStart;
            set => _dateStart = value;
        }

        public string DateEnd
        {
            get => _dateEnd;
            set => _dateEnd = value;
        }
    }
}
