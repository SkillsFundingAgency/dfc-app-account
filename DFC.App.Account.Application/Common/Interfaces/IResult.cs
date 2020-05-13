using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.Account.Application.Common.Interfaces
{
    public interface IResult
    {
        string Error { get; }
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }

}
