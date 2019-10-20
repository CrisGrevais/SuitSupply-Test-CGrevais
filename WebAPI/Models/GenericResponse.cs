using Domain.Contracts.Errors;
using Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Http.ModelBinding;

namespace WebAPI.Models
{
    public class GenericResponse<T>
    {
        #region ctor

        public GenericResponse()
        {
            Errors = new List<IErrorMessage>().ToArray();
        }

        public static GenericResponse<T> FromModel(T model)
        {
            return new GenericResponse<T>
            {
                Result = model
            };
        }

        public static GenericResponse<object> FromError(ModelStateDictionary ModelState, int code)
        {
            var errors = ModelState.SelectMany(x => x.Value.Errors.Select(y => new ErrorMessage("BadRequest", y.ErrorMessage))).ToArray();

            return new GenericResponse<object>
            {
                Errors = errors,
                Result = null,
                Code = code
            };
        }

        public static GenericResponse<T> FromError(string[] errors, int code)
        {
            return new GenericResponse<T>
            {
                Errors = errors.Select(x => new ErrorMessage(code.ToString(), x)).ToArray(),
                Result = default(T),
                Code = code
            };
        }

        public static GenericResponse<T> FromErrorCollector(IErrorCollector errorCollector, int code)
        {
            return new GenericResponse<T>
            {
                Result = default(T),
                Code = code,
                Errors = errorCollector.Errors.Select((x) => new ErrorMessage(x.Code, x.Message, x.Type)).ToArray()
            };
        }

        public static GenericResponse<T> FromException(Exception ex, int code)
        {
            var errorMessage = new List<ErrorMessage>
            {
                new ErrorMessage(Marshal.GetHRForException(ex).ToString(), ex.Message, ErrorTypes.Processing)
            };

            return new GenericResponse<T>
            {
                Errors = errorMessage.ToArray(),
                Result = default(T),
                Code = code
            };
        }

        public static GenericResponse<T> GenericExceptionResponse()
        {
            return new GenericResponse<T>
            {
                Errors = new[]
                {
                    new ErrorMessage("500", "An error has occurred during the execution of the request.", ErrorTypes.Processing)
                },
                Result = default(T),
                Code = 500
            };
        }

        #endregion

        #region Properties

        public int Code { get; set; }
        public IErrorMessage[] Errors { get; set; }
        public T Result { get; set; }

        #endregion
    }
}