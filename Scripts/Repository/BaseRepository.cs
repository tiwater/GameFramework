using System;
using System.Threading.Tasks;

namespace GameFramework.Repository
{
    public abstract class BaseRepository
    {

        public string Token
        {
            get { return _token; }
        }

        protected string _token;
    }
}
