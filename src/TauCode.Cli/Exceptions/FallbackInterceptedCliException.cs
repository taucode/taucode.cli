﻿using System;

namespace TauCode.Cli.Exceptions
{
    public class FallbackInterceptedCliException : CliException
    {
        public FallbackInterceptedCliException(string message) : base(message)
        {
        }

        public FallbackInterceptedCliException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
