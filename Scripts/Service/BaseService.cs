using System;

namespace GameFramework.GameStructure.Service
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