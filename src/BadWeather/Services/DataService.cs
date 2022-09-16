using Newtonsoft.Json;

namespace BadWeather.Services
{
    public class DataService
    {
        private readonly IDictionary<string, IList<object>> _cache = new Dictionary<string, IList<object>>();
        private readonly IDictionary<string, object?> _sources = new Dictionary<string, object?>();

        public void RegisterSource(string key, object? source)
        {
            if (_sources.ContainsKey(key) == false)
            {
                _sources.Add(key, source);
            }
        }

        public async Task<IList<T>> GetDataAsync<T>(string key)
        {
            if (_sources.ContainsKey(key) == true)
            {
                if (_cache.ContainsKey(key) == false)
                {
                    if (_sources[key] is string path)
                    {
                        var list = await Task.Run(() => DeserializeFromStream<T>(path).Cast<object>().ToList());
                        _cache.Add(key, list);
                    }
                    else if (_sources[key] is Stream stream)
                    {
                        var list = await Task.Run(() => DeserializeFromStream<T>(stream).Cast<object>().ToList());
                        _cache.Add(key, list);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                return _cache[key].Cast<T>().ToList();
            }

            return new List<T>();
        }

        public IList<T> GetData<T>(string key)
        {
            if (_sources.ContainsKey(key) == true)
            {
                if (_cache.ContainsKey(key) == false)
                {
                    if (_sources[key] is string path)
                    {
                        var list = DeserializeFromStream<T>(path).Cast<object>().ToList();
                        _cache.Add(key, list);
                    }
                    else if (_sources[key] is Stream stream)
                    {
                        var list = DeserializeFromStream<T>(stream).Cast<object>().ToList();
                        _cache.Add(key, list);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                return _cache[key].Cast<T>().ToList();
            }

            return new List<T>();
        }

        private static List<T> DeserializeFromStream<T>(string path)
        {
            using StreamReader file = File.OpenText(path);
            var serializer = new JsonSerializer();
            return (List<T>)(serializer.Deserialize(file, typeof(List<T>)) ?? new List<T>());
        }

        private static List<T> DeserializeFromStream<T>(Stream stream)
        {
            using var file = new StreamReader(stream);
            var serializer = new JsonSerializer();
            return (List<T>)(serializer.Deserialize(file, typeof(List<T>)) ?? new List<T>());
        }
    }
}
