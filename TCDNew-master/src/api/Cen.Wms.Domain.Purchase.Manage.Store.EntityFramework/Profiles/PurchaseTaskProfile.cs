using AutoMapper;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Domain.Purchase.Models;

namespace Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework.Profiles
{
    public class PurchaseTaskProfile: Profile
    {
        public PurchaseTaskProfile()
        {
            CreateMap<PurchaseTaskHeadRow, PurchaseTaskHeadListModel>()
                .ForMember(e => e.CreatedByUserId, m => m.MapFrom(s => s.CreatedByUserId))
                .ForMember(e => e.CreatedByUserName, m => m.MapFrom(s => s.CreatedByUser.Name));
        }
    }
}