using System.Text.Json;

namespace GameShopApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }

        public bool IsCompleted { get; set; }

        public object Clone()
        {
            var json = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Game>(json)??new Game();
        }
    }
}
