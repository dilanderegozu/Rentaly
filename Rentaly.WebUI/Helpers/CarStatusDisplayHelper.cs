using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Helpers
{
    public static class CarStatusDisplayHelper
    {
        public static (string Text, string BgClass, string TextClass, string DotClass) GetDisplay(CarStatus status)
        {
            return status switch
            {
                CarStatus.Available => ("Müsait", "bg-[#DCFCE7]", "text-[#166534]", "bg-[#22C55E]"),
                CarStatus.Rented => ("Kirada", "bg-[#DBEAFE]", "text-[#1E40AF]", "bg-[#3B82F6]"),
                CarStatus.Maintenance => ("Bakımda", "bg-[#FEF9C3]", "text-[#854D0E]", "bg-[#EAB308]"),
                _ => ("Bilinmiyor", "bg-surface-container", "text-on-surface-variant", "bg-on-surface-variant")
            };
        }
    }
}