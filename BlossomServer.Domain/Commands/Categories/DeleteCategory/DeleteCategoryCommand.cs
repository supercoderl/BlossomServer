using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Categories.DeleteCategory
{
    public sealed class DeleteCategoryCommand : CommandBase, IRequest
    {
        private static readonly DeleteCategoryCommandValidation s_validation = new();

        public Guid CategoryId { get; }

        public DeleteCategoryCommand(Guid categoryId) : base(Guid.NewGuid())
        {
            CategoryId = categoryId;
        }

        public override bool IsValid()
        {
            ValidationResult = s_validation.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
