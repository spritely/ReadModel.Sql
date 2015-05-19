// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryExtensions.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Spritely.Cqrs;
    using Spritely.Recipes;

    internal static class QueryExtensions
    {
        public static IEnumerable<KeyValuePair<string, object>> GetProperties<TResult>(this IQuery<TResult> query)
        {
            var type = query.GetType();

            var properties = type.GetProperties().Where(p => p.Name != "ModelType");

            return properties.Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(query)));
        }

        public static IEnumerable<string> GetWhereParts(this IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return parameters.Select(p => string.Format("[{0}] = @{0}", p.Key));
        }

        public static string JoinAllWithAnd(this IEnumerable<string> parts)
        {
            var result = new StringBuilder();

            var allParts = parts.ToList();
            allParts.Take(allParts.Count - 1).ForEach(p => result.AppendFormat("{0} and ", p));
            result.Append(allParts.Last());

            return result.ToString();
        }
    }
}
