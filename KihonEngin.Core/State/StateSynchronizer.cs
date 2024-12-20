using KihonEngine.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KihonEngine.Core.State
{
    public class StateSynchronizer<TSynchronized>
    {
        private JObject _initial;
        private TSynchronized _synchronized;

        public StateSynchronizer(TSynchronized initial)
        {
            _initial = JObject.FromObject(initial);
            _synchronized = initial;
        }

        public string GetJsonPatch()
        {
            var updated = JObject.FromObject(_synchronized);
            var patch = JsonDiffPatch.Diff(_initial, updated);
            var jsonPatch = JsonConvert.SerializeObject(patch);
            return jsonPatch;
        }

        public string GetJson()
        {
            var json = JsonConvert.SerializeObject(_synchronized);
            return json;
        }
    }
}
