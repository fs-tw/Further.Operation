﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Further.Operation.Pages;

public class IndexModel : OperationPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
