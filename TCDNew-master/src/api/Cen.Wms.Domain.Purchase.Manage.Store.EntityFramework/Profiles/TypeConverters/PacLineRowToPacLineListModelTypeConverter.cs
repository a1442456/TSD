using AutoMapper;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Domain.Purchase.Models;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework.Profiles.TypeConverters
{
    public class PacLineRowToPacLineListModelTypeConverter: ITypeConverter<PacLineRow, PacLineListModel>
    {
        public PacLineListModel Convert(PacLineRow source, PacLineListModel destination, ResolutionContext context)
        {
            var result = destination ?? new PacLineListModel();
            result.Id = source.Id.Value;
            result.ExtId = source.ExtId;
            result.LineNum = source.LineNum;
            result.ProductId = source.ProductId;
            result.ProductName = source.ProductName;
            result.ProductUnitOfMeasure = source.ProductUnitOfMeasure;
            result.ProductBarcodeMain = source.ProductBarcodeMain;
            result.ProductBarcodes = source.ProductBarcodes;
            result.QtyExpected = source.QtyExpected;
            LocalDate? worstDate = null;
            foreach (var pacLineStateRow in source.PacLineStates)
            {
                result.QtyNormal += pacLineStateRow.QtyNormal;
                result.QtyBroken += pacLineStateRow.QtyBroken;
                var currentPalletDate = pacLineStateRow.ExpirationDate?.PlusDays(pacLineStateRow.ExpirationDaysPlus);
                var newWorstDate =
                    currentPalletDate != null
                        ? worstDate == null
                            ? currentPalletDate
                            : LocalDate.Min(currentPalletDate.Value, worstDate.Value)
                        : worstDate;
                if (newWorstDate != worstDate)
                {
                    worstDate = newWorstDate;
                    result.ExpirationDate = pacLineStateRow.ExpirationDate;
                    result.ExpirationDaysPlus = pacLineStateRow.ExpirationDaysPlus;
                }
            }

            return result;
        }
    }
}