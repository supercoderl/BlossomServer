using BlossomServer.SharedKernel.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlossomServer.Domain.Entities
{
    public class Service : Entity<Guid>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Guid? CategoryId { get; private set; }
        public decimal? Price { get; private set; }
        public int? DurationMinutes { get; private set; }
        public string? RepresentativeImage { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Services")]
        public virtual Category? Category { get; set; }

        [InverseProperty("Service")]
        public virtual ICollection<ServiceImage> ServiceImages { get; set; } = new List<ServiceImage>();

        [InverseProperty("Service")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

        [InverseProperty("Service")]
        public virtual ICollection<ServiceOption> ServiceOptions { get; set; } = new List<ServiceOption>();

        public Service(
            Guid id,
            string name,
            string? description,
            Guid? categoryId,
            decimal? price,
            int? durationMinutes,
            string? representativeImage
        ) : base(id)
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Price = price;
            DurationMinutes = durationMinutes;
            RepresentativeImage = representativeImage;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetName(string name) { Name = name; }
        public void SetDescription(string? description) { Description = description; }
        public void SetCategoryId(Guid? categoryId) { CategoryId = categoryId; }
        public void SetDurationMinutes(int? durationMinutes) { DurationMinutes = durationMinutes; }
        public void SetPrice(decimal? price) { Price = price; }
        public void SetRepresentativeImage(string? representativeImage) { RepresentativeImage = representativeImage; }
        public void SetUpdatedAt() { UpdatedAt = TimeZoneHelper.GetLocalTimeNow(); }
    }
}
