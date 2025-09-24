using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GolfClubTycoon;

public class TileMap
{
    private readonly TileType[,] _tiles;
    private readonly Tileset _tileset;
    private readonly int _width;
    private readonly int _height;
    private readonly int _tileSize;
    public Point TeeTile { get; private set; }
    public Point CupTile { get; private set; }
    private readonly List<Point> _fairwayPath = new();

    public int Width => _width;
    public int Height => _height;
    public int TileSize => _tileSize;

    public TileMap(int width, int height, Tileset tileset, int tileSize = 16)
    {
        _width = width;
        _height = height;
        _tileset = tileset;
        _tileSize = tileSize;
        _tiles = new TileType[width, height];
    }

    public void GenerateHole(Point tee, Point cup, int fairwayHalfWidth = 3, int greenRadius = 5)
    {
        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                _tiles[x, y] = TileType.Rough;

        TeeTile = tee;
        CupTile = cup;

        PlotLine(tee.X, tee.Y, cup.X, cup.Y, (px, py) => _fairwayPath.Add(new Point(px, py)));

        foreach (var p in _fairwayPath)
        {
            for (int ox = -fairwayHalfWidth; ox <= fairwayHalfWidth; ox++)
            {
                for (int oy = -fairwayHalfWidth; oy <= fairwayHalfWidth; oy++)
                {
                    int tx = p.X + ox;
                    int ty = p.Y + oy;
                    if (!InBounds(tx, ty)) continue;
                    if (ox * ox + oy * oy <= fairwayHalfWidth * fairwayHalfWidth + 1)
                        _tiles[tx, ty] = TileType.Fairway;
                }
            }
        }

        int gr2 = greenRadius * greenRadius;
        for (int y = cup.Y - greenRadius; y <= cup.Y + greenRadius; y++)
        {
            for (int x = cup.X - greenRadius; x <= cup.X + greenRadius; x++)
            {
                if (!InBounds(x, y)) continue;
                int dx = x - cup.X; int dy = y - cup.Y;
                if (dx * dx + dy * dy <= gr2)
                    _tiles[x, y] = TileType.Green;
            }
        }

        PlaceBunker(new Point(cup.X - greenRadius - 3, cup.Y));
        PlaceBunker(new Point(cup.X + greenRadius + 3, cup.Y - 2));
    }

    private void PlaceBunker(Point center, int radius = 3)
    {
        int r2 = radius * radius;
        for (int y = center.Y - radius; y <= center.Y + radius; y++)
        {
            for (int x = center.X - radius; x <= center.X + radius; x++)
            {
                if (!InBounds(x, y)) continue;
                int dx = x - center.X; int dy = y - center.Y;
                if (dx * dx + dy * dy <= r2)
                    _tiles[x, y] = TileType.Sand;
            }
        }
    }

    private bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < _width && y < _height;

    private void PlotLine(int x0, int y0, int x1, int y1, Action<int, int> plot)
    {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy;
        while (true)
        {
            if (InBounds(x0, y0)) plot(x0, y0);
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var tex = _tileset.Texture;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var type = _tiles[x, y];
                var src = _tileset.GetCenterSource(type == TileType.Green ? TileType.Sand : type);
                var destPos = new Vector2(x * _tileSize, y * _tileSize);
                var color = type switch
                {
                    TileType.Green => new Color(150, 255, 150), // lighter tint
                    _ => Color.White
                };
                spriteBatch.Draw(tex, destPos, src, color);
            }
        }

        if (InBounds(CupTile.X, CupTile.Y))
        {
            var cupPos = new Vector2(CupTile.X * _tileSize, CupTile.Y * _tileSize);
            var src = _tileset.GetCenterSource(TileType.Sand);
            spriteBatch.Draw(tex, cupPos, src, new Color(60, 110, 60));
        }
    }
}
