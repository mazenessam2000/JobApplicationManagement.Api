using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationManagement.Application.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
