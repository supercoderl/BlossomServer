using BlossomServer.Application.ViewModels.ServiceImages;
using BlossomServer.Application.ViewModels.Services;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.SortProviders
{
    public sealed class ServiceImageViewModelSortProvider : ISortingExpressionProvider<ServiceImageViewModel, ServiceImage>
    {
        private static readonly Dictionary<string, Expression<Func<ServiceImage, object>>> s_expressions = new()
        {

        };

        public Dictionary<string, Expression<Func<ServiceImage, object>>> GetSortingExpressions()
        {
            return s_expressions;
        }
    }
}
