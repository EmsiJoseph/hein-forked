using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Hein.Helpers;

public static class SonnerHelper
{
    public const string SuccessKey = "SuccessMessage";
    public const string ErrorKey = "ErrorMessage";
    public const string InfoKey = "InfoMessage";
    public const string WarningKey = "WarningMessage";

    public static void Success(ITempDataDictionary tempData, string message)
    {
        tempData[SuccessKey] = message;
    }

    public static void Error(ITempDataDictionary tempData, string message)
    {
        tempData[ErrorKey] = message;
    }

    public static void Info(ITempDataDictionary tempData, string message)
    {
        tempData[InfoKey] = message;
    }

    public static void Warning(ITempDataDictionary tempData, string message)
    {
        tempData[WarningKey] = message;
    }
}
