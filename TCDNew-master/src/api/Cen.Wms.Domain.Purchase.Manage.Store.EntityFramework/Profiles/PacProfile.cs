using System;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework.Profiles.TypeConverters;
using Cen.Wms.Domain.Purchase.Models;
using MassTransit;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework.Profiles
{
    public class PacProfile: Profile
    {
        public PacProfile()
        {
            CreateMap<PacHeadEditModel, PacHeadRow>()
                .ForMember(e => e.Id, m => m.Ignore());
            CreateMap<PacLineEditModel, PacLineRow>()
                .ForMember(e => e.ChangedAt, m => m.MapFrom(s => SystemClock.Instance.GetCurrentInstant()))
                .ForMember(e => e.Id, m => m.MapFrom(MapPacLineRowId))
                .EqualityComparison((s, d) => s.ExtId == d.ExtId);

            CreateMap<PacHeadRow, PacHeadEditModel>();
            CreateMap<PacLineRow, PacLineEditModel>();
            
            CreateMap<PacHeadRow, PacHeadListModel>()
                .ForMember(e => e.IsBusy, m => m.MapFrom(s => s.PacState.IsBusy))
                .ForMember(e => e.IsProcessed, m => m.MapFrom(s => s.PacState.IsProcessed))
                .ForMember(e => e.IsExported, m => m.MapFrom(s => s.PacState.IsExported))
                .ForMember(e => e.ResponsibleUserName, m => m.MapFrom(s => s.ResponsibleUser.Name));
            CreateMap<PacLineRow, PacLineListModel>()
                .ConvertUsing<PacLineRowToPacLineListModelTypeConverter>();
        }

        private Guid? MapPacLineRowId(PacLineEditModel s, PacLineRow d)
        {
            return d.Id ?? NewId.NextGuid();
        }
    }
}