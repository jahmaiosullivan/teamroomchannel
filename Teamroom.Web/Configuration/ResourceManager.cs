using System;

namespace HobbyClue.Web.Configuration
{
    public class ResourceManager : IResourceManager
    {
        public string GetResource(string resourceName)
        {
            return Resources.Resources.ResourceManager.GetString(resourceName);
        }
    }
}