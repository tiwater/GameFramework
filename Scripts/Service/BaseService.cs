﻿using System;

namespace GameFramework.Service
{
    public class BaseService
    {
        public string Token
        {
            get { return _token; }
        }

        protected string _token;
    }
}