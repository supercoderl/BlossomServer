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
        public int Priority { get; }

        public CreateCategoryCommand(
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
