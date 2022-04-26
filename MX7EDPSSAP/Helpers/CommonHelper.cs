using Microsoft.AspNetCore.Mvc.ModelBinding;
using MX7EDPSSAP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using static MX7EDPSSAP.Application.Constants.GeneralModelEnum;

namespace MX7EDPSSAP.Helpers
{
    public class CommonHelper
    {
        public static string ConstructUniformErrMsg<T>(T source, ModelStateDictionary modelDictionary)
        {
            List<string> combinedErrMsgList = new List<string>();
            string finalErrorMsg = "";

            List<string> mandatoryErrMsg = new List<string>();
            List<string> maxLengthErrMsg = new List<string>();
            List<string> uniqueErrMsg = new List<string>();

            if (source == null) return finalErrorMsg;
            PropertyInfo[] propertyInfos = source.GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomizedModelAttribute))).ToArray();

            foreach (var dict in modelDictionary)
            {
                string modelName = dict.Key;
                ModelStateEntry modelState = dict.Value;

                foreach (ModelError err in modelState.Errors)
                {
                    if (string.IsNullOrEmpty(err.ErrorMessage)) continue;

                    if (err.ErrorMessage.Contains(" must be between ") || err.ErrorMessage.Contains(" must be more than "))
                    {
                        uniqueErrMsg.Add(err.ErrorMessage);
                        continue;
                    }

                    var tempProp = propertyInfos.Where(x => x.Name.Equals(modelName)).FirstOrDefault();
                    if (tempProp == null || tempProp.GetCustomAttribute<CustomizedModelAttribute>() == null
                        || string.IsNullOrEmpty(tempProp.GetCustomAttribute<CustomizedModelAttribute>().Name))
                    {
                        if (err.ErrorMessage.Contains(" field is required.")) mandatoryErrMsg.Add(modelName);
                        if (err.ErrorMessage.Contains(" with a maximum length of ")) maxLengthErrMsg.Add(modelName);
                        else uniqueErrMsg.Add(err.ErrorMessage);
                    }
                    else
                    {
                        if (err.ErrorMessage.Contains(" field is required.")) mandatoryErrMsg.Add(tempProp.GetCustomAttribute<CustomizedModelAttribute>().Name);
                        if (err.ErrorMessage.Contains(" with a maximum length of ")) maxLengthErrMsg.Add(tempProp.GetCustomAttribute<CustomizedModelAttribute>().Name);
                        else uniqueErrMsg.Add(err.ErrorMessage);
                    }
                }
            }

            if (mandatoryErrMsg.Count() > 0) combinedErrMsgList.Add($"{CombineStringsWithLinkingVerb(mandatoryErrMsg, ModelValidationType.Required)} required");
            if (maxLengthErrMsg.Count() > 0) combinedErrMsgList.Add($"{CombineStringsWithLinkingVerb(maxLengthErrMsg, ModelValidationType.MaximumValue)} maximum length");
            if (uniqueErrMsg.Count() > 0) combinedErrMsgList.Add(CombineStringsWithLinkingVerb(uniqueErrMsg, null));

            finalErrorMsg = $"{string.Join("; ", combinedErrMsgList)}.";
            return finalErrorMsg;
        }

        public static string ConvertSnakeCaseToCamelCase(string snakeCaseStr)
        {
            string camelCaseResult = snakeCaseStr.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            camelCaseResult = info.ToTitleCase(camelCaseResult).Replace(" ", string.Empty);
            camelCaseResult = $"{camelCaseResult.First().ToString().ToLower()}{camelCaseResult.Substring(1)}";
            return camelCaseResult;
        }

        public static bool CheckIsCustomException(Exception ex)
        {
            if (ex is NoRecordFoundException || ex is InvalidParametersException ||
                ex is RecordAlreadyExistException || ex is InsertOrUpdateFailedException)
            {
                return true;
            }
            return false;
        }

        public static int ReturnCustomExceptionMessage(Exception ex, int errorId)
        {
            int statusCode = errorId != 0 ? errorId : (int)HttpStatusCode.InternalServerError;
            if (CheckIsCustomException(ex))
            {
                switch (ex)
                {
                    case NoRecordFoundException:
                        statusCode = ((NoRecordFoundException)ex).errorCode;
                        break;

                    case InvalidParametersException:
                        statusCode = ((InvalidParametersException)ex).errorCode;
                        break;

                    default:
                        break;
                }
            }

            //if (ex is NoRecordFoundException)
            //{
            //    statusCode = ((NoRecordFoundException)ex).errorCode;
            //}

            return statusCode;
        }

        public static object GenerateStandardErrorResponse(Exception ex, int errorId, string message)
        {
            dynamic result = new ExpandoObject();
            string commonErrMsg = "Encountered system error, please contact system administrator!";

            if (!CheckIsCustomException(ex))
            {
                Console.WriteLine("Unhandled Exception:");
                Console.WriteLine(ex.Message);
            }

            result.errorId = ReturnCustomExceptionMessage(ex, errorId);

            //result.Message = !CheckIsCustomException(ex) ? message : string.IsNullOrEmpty(ex.Message) ? commonErrMsg : ex.Message;
            result.Message = !CheckIsCustomException(ex) ? string.IsNullOrEmpty(message) ? string.IsNullOrEmpty(ex.Message) ? commonErrMsg : ex.Message : message : !string.IsNullOrEmpty(ex.Message) ? ex.Message : commonErrMsg;

            return result;
        }

        private static string CombineStringsWithLinkingVerb(List<string> stringList, ModelValidationType? modType = null)
        {
            string finalCombinedString = "";
            if (!stringList.Any()) return finalCombinedString;

            switch (modType)
            {
                case ModelValidationType.Required:
                    finalCombinedString = (stringList.Count == 1) ? $"{string.Join(", ", stringList)} is" : $"{string.Join(", ", stringList)} are";
                    break;

                case ModelValidationType.MaximumValue:
                    finalCombinedString = (stringList.Count == 1) ? $"{string.Join(", ", stringList)} has exceeded its" : $"{string.Join(", ", stringList)} have exceeded their";
                    break;

                default:
                    finalCombinedString = $"{string.Join(", ", stringList)}";
                    break;
            }
            return finalCombinedString;
        }
    }
}