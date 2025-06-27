using BlossomServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Promotion : Entity<Guid>
    {
        public string Code { get; private set; }
        public string? Description { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public decimal DiscountValue { get; private set; }
        public decimal MinimumSpend { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int MaxUsage { get; private set; }
        public int CurrentUsage { get; private set; }
        public bool IsActive { get; private set; }

        public Promotion(
            Guid id,
            string code,
            string? description,
            DiscountType discountType,
            decimal discountValue,
            decimal minimumSpend,
            DateTime startDate,
            DateTime endDate,
            int maxUsage,
            int currentUsage,
            bool isActive
        ) : base(id)
        {
            Code = code;
            Description = description;
            DiscountType = discountType;
            DiscountValue = discountValue;
            MinimumSpend = minimumSpend;
            StartDate = startDate;
            EndDate = endDate;
            MaxUsage = maxUsage;
            CurrentUsage = currentUsage;
            IsActive = isActive;
        }

        public void SetCode( string code ) { Code = code; }
        public void SetDescription( string? description ) { Description = description; }
        public void SetDiscountType( DiscountType discountType ) { DiscountType = discountType; }
        public void SetDiscountValue( decimal discountValue ) { DiscountValue = discountValue; }
        public void SetMinimumSpend( decimal minimumSpend ) { MinimumSpend = minimumSpend; }
        public void SetStartDate( DateTime startDate ) { StartDate = startDate; }
        public void SetEndDate( DateTime endDate ) { EndDate = endDate; }
        public void SetMaxUsage( int maxUsage ) { MaxUsage = maxUsage; }
        public void SetCurrentUsage( int currentUsage ) { CurrentUsage = currentUsage; }
        public void SetIsActive( bool isActive ) { IsActive = isActive; }
    }
}
