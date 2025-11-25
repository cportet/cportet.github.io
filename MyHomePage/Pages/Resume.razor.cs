using Microsoft.AspNetCore.Components;
using MyHomePage.Helpers;

namespace MyHomePage.Pages;

[Route("resume")]
[Route("cv")]
[Route("curriculumvitae")]
[Route("experience")]
public partial class Resume
{
    private readonly Dictionary<string, string> _enBrefVariables = new()
        {
            {"age",DisplayHelper.AgeCyril.ToString()},
            {"anciennete",DisplayHelper.AnciennetePro.ToString()}
        };
}
