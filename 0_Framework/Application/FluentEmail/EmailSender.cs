using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace _0_Framework.Application.FluentEmail

{
    public class EmailSender : IEmailSender
    {

        private const string TemplatePath = "ServiceHost.Template.EmailConfirmation.cshtml";
        private readonly IFluentEmail _email;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IFluentEmail email, ILogger<EmailSender> logger)
        {
            _email = email;
            _logger = logger;
        }

        public async Task<bool> SendUsingTemplate(string to, string subject, IEmailSender.EmailTemplate template, object model)
        {
            var result = await _email.To(to)
                .Subject(subject)
                .UsingTemplateFromEmbedded(string.Format(TemplatePath, template), ToExpando(model), GetType().Assembly)
                .SendAsync();

            if (!result.Successful)
            {
                _logger.LogError("Failed to send an email.\n{Errors}", string.Join(Environment.NewLine, result.ErrorMessages));
            }

            return result.Successful;
        }
        private static ExpandoObject ToExpando(object model)
        {
            if (model is ExpandoObject exp)
            {
                return exp;
            }

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyDescriptor in model.GetType().GetTypeInfo().GetProperties())
            {
                var obj = propertyDescriptor.GetValue(model);

                if (obj != null && IsAnonymousType(obj.GetType()))
                {
                    obj = ToExpando(obj);
                }

                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }

        private static bool IsAnonymousType(Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
                .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
                .Any();

            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

    }
}