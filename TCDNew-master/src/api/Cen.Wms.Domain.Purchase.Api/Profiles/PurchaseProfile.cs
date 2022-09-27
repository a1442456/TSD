using System;
using System.Linq;
using AutoMapper;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Cen.Wms.Domain.Purchase.Models;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Api.Profiles
{
    public class PurchaseProfile: Profile
    {
        public PurchaseProfile()
        {
            CreateMap<PacHeadEditModel, PacHeadDto>()
                .ForMember(e => e.PacId, m => m.MapFrom(s => s.Id))
                .ForMember(e => e.PacCode, m => m.MapFrom(s => s.ExtId))
                .ForMember(e => e.DeliveryDate, m => m.MapFrom(s => s.PurchaseBookingDate.AtStartOfDayInZone(DateTimeZone.Utc).ToInstant()))
                .ForMember(e => e.SupplierName, m => m.MapFrom(s => s.SupplierName))
                .ForMember(e => e.Gate, m => m.MapFrom(s => string.Empty))
                .ForMember(e => e.TotalLinesSum, m => m.MapFrom(s => s.Lines.Sum(sl => sl.QtyExpected * 0)))
                .ForMember(e => e.TotalLinesCount, m => m.MapFrom(s => s.Lines.Count));

            CreateMap<PurchaseTaskLineRow, PurchaseTaskLineListModel>()
                .ForMember(e => e.ExpirationDate, m => m.MapFrom(s => s.PurchaseTaskLineState.ExpirationDate))
                .ForMember(e => e.ExpirationDaysPlus, m => m.MapFrom(s => s.PurchaseTaskLineState.ExpirationDaysPlus))
                .ForMember(e => e.QtyNormal, m => m.MapFrom(s => s.PurchaseTaskLineState.QtyNormal))
                .ForMember(e => e.QtyBroken, m => m.MapFrom(s => s.PurchaseTaskLineState.QtyBroken));

            CreateMap<PurchaseTaskPalletRow, PurchaseTaskPalletListModel>();
            CreateMap<PurchaseTaskLineUpdatePostReq, PurchaseTaskLineUpdateRow>()
                .ForMember(e => e.PurchaseTaskHeadId, m => m.MapFrom(s => s.PurchaseTaskId))
                .ForMember(e => e.UserId, m => m.MapFrom(s => Guid.Empty))
                .ForMember(e => e.PurchaseTaskLineId, m => m.MapFrom(s => s.PurchaseTaskLineUpdate.PurchaseTaskLineId))
                .ForMember(e => e.PurchaseTaskLineUpdateType, m => m.MapFrom(s => s.PurchaseTaskLineUpdate.PurchaseTaskLineUpdateType))
                .ForMember(e => e.ExpirationDate, m => m.MapFrom(s => s.PurchaseTaskLineUpdate.PurchaseTaskLineState != null ? s.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate.HasValue ? (LocalDate?)s.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate.Value.InUtc().Date : null : null))
                .ForMember(e => e.ExpirationDaysPlus, m => m.MapFrom(s => s.PurchaseTaskLineUpdate.PurchaseTaskLineState != null ? s.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDaysPlus : 0))
                .ForMember(e => e.QtyNormal, m => m.MapFrom(s => s.PurchaseTaskLineUpdate.PurchaseTaskLineState != null ? s.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyNormal : 0))
                .ForMember(e => e.QtyBroken, m => m.MapFrom(s => s.PurchaseTaskLineUpdate.PurchaseTaskLineState != null ? s.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyBroken : 0))
                .ForMember(e => e.CreatedAt, m => m.MapFrom(s => SystemClock.Instance.GetCurrentInstant()));
            
            CreateMap<PurchaseTaskHeadListModel, PurchaseTaskDto>();
            CreateMap<PurchaseTaskLineUpdateRow, PurchaseTaskLineUpdateListModel>()
                .ForMember(e => e.ProductExtId, m => m.MapFrom(s => s.PurchaseTaskLine.ProductExtId))
                .ForMember(e => e.ProductName, m => m.MapFrom(s => s.PurchaseTaskLine.ProductName))
                .ForMember(e => e.ProductAbc, m => m.MapFrom(s => s.PurchaseTaskLine.ProductAbc));
        }
    }
}