using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuKeeper.Github
{
    public class OctokitWrapper : IGithub
    {
        public Task<OpenPullRequestResult> OpenPullRequest(OpenPullRequestRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GithubRepository>> GetRepositoriesForOrganisation(string organisationName)
        {
            throw new NotImplementedException();
        }
    }
}
