    using System;
using System.Collections.Generic;
using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper.QueryableExtensions;

namespace HireMe.Mapping.Utility
{
    public static class AsyncMappingExtensions
    {
        /*
        public static EnumerableQuery<TDestination> ToAsync<TDestination>( this EnumerableQuery source, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(AutoMapperConfig.MapperInstance.ConfigurationProvider, null, membersToExpand);
        }

        public static EnumerableQuery<TDestination> ToAsync<TDestination>(this EnumerableQuery source, object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(AutoMapperConfig.MapperInstance.ConfigurationProvider, parameters);
        }
        */
    }
}