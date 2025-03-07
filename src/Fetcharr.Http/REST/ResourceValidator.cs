﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;
using Fetcharr.Http.ClientSchema;

namespace Fetcharr.Http.REST
{
    public class ResourceValidator<TResource> : AbstractValidator<TResource>
    {
        public IRuleBuilderInitial<TResource, TProperty> RuleForField<TProperty>(Expression<Func<TResource, IEnumerable<Field>>> fieldListAccessor, string fieldName)
        {
            var rule = new PropertyRule(fieldListAccessor.GetMember(), c => GetValue(c, fieldListAccessor.Compile(), fieldName), null, () => CascadeMode.Continue, typeof(TProperty), typeof(TResource));
            rule.PropertyName = fieldName;
            rule.SetDisplayName(fieldName);

            AddRule(rule);
            return new RuleBuilder<TResource, TProperty>(rule, this);
        }

        private static object GetValue(object container, Func<TResource, IEnumerable<Field>> fieldListAccessor, string fieldName)
        {
            var resource = fieldListAccessor((TResource)container).SingleOrDefault(c => c.Name == fieldName);

            return resource?.Value;
        }
    }
}
