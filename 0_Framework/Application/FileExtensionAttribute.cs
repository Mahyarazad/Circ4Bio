﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _0_Framework.Application
{
    public class FileExtensionLimit : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] _validFileExtension;

        public FileExtensionLimit(string[] validFileExtension)
        {
            _validFileExtension = validFileExtension;
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file == null) return true;
            var fileExtension = Path.GetExtension(file.FileName);
            return _validFileExtension.Contains(fileExtension);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-fileExtensionLimit", ErrorMessage);
        }

    }

}