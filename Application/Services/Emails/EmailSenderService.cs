using Domain.Enums;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Application.Services.Emails
{
    public class EmailSenderService : IEmailSenderService
    {
        private const string TemplatePath = "Application.Services.Emails.Templates.{0}.cshtml";
        private readonly IFluentEmail _email;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(IFluentEmail email, ILogger<EmailSenderService> logger)
        {
            _email = 
                email ?? throw new ArgumentNullException(nameof(email));

            _logger =
                logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<bool> Send(string to, string subject, EmailTemplate template, object model)
        {
            var templatePath = string.Format(TemplatePath, template);
            var expandoModel = ToExpando(model);
            var assembly = GetType().Assembly;

            var result = await _email
                                    .To(to)
                                    .Subject(subject)
                                    .UsingTemplateFromEmbedded(templatePath, expandoModel, assembly)
                                    .SendAsync();

            if(!result.Successful)
            {
                _logger.LogError("Failed to send email /n{Errors}", String.Join(Environment.NewLine, result.ErrorMessages));
            }

            return result.Successful;
        }

        #region Helper methods

        /// <summary>
        /// Zamienia obiekt typu object na ExpandoObject, który akcptowalny jest przez silnik Razor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        #endregion
    }
}
