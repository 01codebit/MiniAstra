using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

using model;

namespace jsonloader
{
    public class JsonLoader<T>
    {
        public void LoadObject(string filepath, Action<T> onLoaded)
        {
            string content = File.ReadAllText(filepath);
            T obj = JsonConvert.DeserializeObject<T>(content);

            onLoaded(obj);
        }

        public List<T> LoadObjectsList(string filepath)
        {
            string content = File.ReadAllText(filepath);
            List<T> objs = JsonConvert.DeserializeObject<List<T>>(content);

            return objs;
        }
/*
        public void LoadObjectsJson(string filepath, Action< JsonListResponse<T> > onLoaded)
        {
            string content = File.ReadAllText(filepath);
            JsonListResponse<T> vJson = JsonConvert.DeserializeObject< JsonListResponse<T> >(content);

            onLoaded(vJson);
        }
*/

        public T[] LoadObjectsJson(string filepath)
        {
            string content = File.ReadAllText(filepath);
            JsonListResponse<T> vJson = JsonConvert.DeserializeObject< JsonListResponse<T> >(content);

            return vJson.response;
        }

    }
}
