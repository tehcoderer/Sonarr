﻿using FluentValidation.Validators;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Tv;

namespace NzbDrone.Core.Validation.Paths
{
    public class SeriesPathValidator : PropertyValidator
    {
        private readonly ISeriesService _seriesService;

        public SeriesPathValidator(ISeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        protected override string GetDefaultMessageTemplate() => "Path '{path}' is already configured for another series";

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return true;
            }

            context.MessageFormatter.AppendArgument("path", context.PropertyValue.ToString());

            dynamic instance = context.ParentContext.InstanceToValidate;
            var instanceId = (int)instance.Id;

            return !_seriesService.GetAllSeries().Exists(s => s.Path.PathEquals(context.PropertyValue.ToString()) && s.Id != instanceId);
        }
    }
}
