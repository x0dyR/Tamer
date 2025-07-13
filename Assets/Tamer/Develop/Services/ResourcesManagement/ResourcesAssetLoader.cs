using UnityEngine;

namespace Tamer.Develop.Services.ResourcesManagement
{
    public class ResourcesAssetLoader
    {
        public T Load<T>(string path) where T : UnityEngine.Object
            => Resources.Load<T>(path);
    }
}