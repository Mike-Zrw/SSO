using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Core.IRepository
{
    public partial interface IBUserRepository
    {
        void LogOn(int userid);
    }
}
