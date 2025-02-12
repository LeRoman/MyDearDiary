﻿using Diary.BLL.Exceptions;
using Diary.WebAPI.Enums;
using System.Net;

namespace Diary.WebAPI.Extentions
{
    public static class ExceptionFilterExtensions
    {
        public static (HttpStatusCode statusCode, ErrorCode errorCode) ParseException(this Exception exception)
        {
            Console.WriteLine(exception.Message);
            return exception switch
            {
                BadRequestException _ => (HttpStatusCode.BadRequest, ErrorCode.BadRequest),
                NotFoundException _ => (HttpStatusCode.NotFound, ErrorCode.NotFound),
                InvalidCredentialsException _ => (HttpStatusCode.Unauthorized, ErrorCode.InvalidUsernameOrPassword),
                InvalidTokenException _ => (HttpStatusCode.Unauthorized, ErrorCode.InvalidToken),
                _ => (HttpStatusCode.InternalServerError, ErrorCode.General),
            };
        }
    }
}
