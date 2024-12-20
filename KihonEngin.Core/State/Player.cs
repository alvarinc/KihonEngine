
namespace KihonEngine.Core.State
{
    public class Player
    {
        public string Guid { get; set; }
        public string Name { get; set; }

        public string ConnectionKey => $"{Guid}:{Name}";
    }
}
