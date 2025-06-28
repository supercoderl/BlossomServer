using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Categories.CreateCategory
{
    public sealed class CreateCategoryCommand : CommandBase, IRequest
    {
        private static readonly CreateCategoryCommandValidation s_validation = new();

        public Guid CategoryId { get; }
        public string Name { get; }
        public bool IsActive { get; }
        public string Icon { get; }
        public string Url { get; }
        public int Priority { get; }

        public CreateCategoryCommand(
            Guid categoryId,
            string name,
            string icon,
            string url,
            bool isActive,
            int priority
        ) : base(Guid.NewGuid())
        {
            CategoryId = categoryId;
            Name = name;
            Url = url;
            Icon = icon;
            IsActive = isActive;
            Priority = priority;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
