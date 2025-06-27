using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Categories.UpdateCategory
{
    public sealed class UpdateCategoryCommand : CommandBase, IRequest
    {
        private static readonly UpdateCategoryCommandValidation s_validation = new();

        public Guid CategoryId { get; }
        public string Name { get; }
        public bool IsActive { get; }
        public int Priority { get; }

        public UpdateCategoryCommand(
            Guid categoryId,
            string name,
            bool isActive,
            int priority
        ) : base(Guid.NewGuid())
        {
            CategoryId = categoryId;
            Name = name;
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
