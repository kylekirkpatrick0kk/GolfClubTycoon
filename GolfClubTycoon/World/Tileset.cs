using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GolfClubTycoon;

public class Tileset
{
    private readonly int _tileSize;
    private readonly Texture2D _texture;

    private class Cluster
    {
        public int StartRow; // top-left row of 3x3 cluster
        public int StartCol; // top-left col of 3x3 cluster
    }

    private readonly Dictionary<TileType, Cluster> _clusters = new();

    public Tileset(Texture2D texture, int tileSize = 16)
    {
        _texture = texture;
        _tileSize = tileSize;

        // Based on user description (rows/cols 0-indexed)
        _clusters[TileType.Rough] = new Cluster { StartRow = 3, StartCol = 0 };      // rows 3-5, cols 0-2
        _clusters[TileType.Fairway] = new Cluster { StartRow = 3, StartCol = 6 };    // rows 3-5, cols 6-8
        _clusters[TileType.Sand] = new Cluster { StartRow = 6, StartCol = 0 };       // rows 6-8, cols 0-2
        _clusters[TileType.Water] = new Cluster { StartRow = 9, StartCol = 0 };      // rows 9-11, cols 0-2
        _clusters[TileType.Green] = _clusters[TileType.Sand]; // reuse bunker (sand) art; tinted when drawn
    }

    public Texture2D Texture => _texture;

    /// <summary>
    /// Returns source rectangle for the center tile of a 3x3 cluster (placeholder until auto-tiling is added).
    /// </summary>
    public Rectangle GetCenterSource(TileType type)
    {
        var c = _clusters[type];
        int row = c.StartRow + 1; // middle row
        int col = c.StartCol + 1; // middle col
        return new Rectangle(col * _tileSize, row * _tileSize, _tileSize, _tileSize);
    }

    public Rectangle GetClusterSource(TileType type, int localRow, int localCol)
    {
        var c = _clusters[type];
        int row = c.StartRow + localRow;
        int col = c.StartCol + localCol;
        return new Rectangle(col * _tileSize, row * _tileSize, _tileSize, _tileSize);
    }
}
