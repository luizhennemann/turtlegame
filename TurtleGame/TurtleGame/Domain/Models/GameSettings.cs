using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace TurtleGame.Domain.Models
{
    public class GameSettings
    {
        [JsonProperty("board")]
        public Board Board { get; set; }

        [JsonProperty("startingPosition")]
        public StartingPosition StartingPosition { get; set; }
        
        [JsonProperty("exitPosition")]
        public ExitPosition ExitPosition { get; set; }
        
        [JsonProperty("mines")]
        public List<Point?> Mines { get; set; }
    }

    public class Board
    {
        [JsonProperty("rows")]
        public int Rows { get; set; }
        
        [JsonProperty("cols")]
        public int Columns { get; set; }
    }

    public class StartingPosition
    {
        [JsonProperty("tile")]
        public Point? Tile { get; set; }

        [JsonProperty("dir")]
        public Direction? Direction { get; set; }
    }

    public class ExitPosition
    {
        [JsonProperty("tile")]
        public Point? Tile { get; set; }
    }
}
