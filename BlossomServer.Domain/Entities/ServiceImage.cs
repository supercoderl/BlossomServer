using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class ServiceImage : Entity<Guid>
    {
        public string ImageName { get; private set; }
        public string ImageUrl { get; private set; }
        public Guid ServiceId { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("ServiceImages")]
        public virtual Service? Service { get; set; }

        public ServiceImage(
            Guid id,
            string imageName,
            string imageUrl,
            Guid serviceId,
            string? description
        ) : base(id)
        {
            ImageName = imageName;
            ImageUrl = imageUrl;
            ServiceId = serviceId;
            Description = description;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetImageName( string imageName ) { ImageName = imageName; }
        public void SetImageUrl( string imageUrl ) { ImageUrl = imageUrl; }
        public void SetServiceId( Guid serviceId ) { ServiceId = serviceId; }
        public void SetDescription( string? description ) { Description = description; }
    }
}
