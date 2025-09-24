using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GolfClubTycoon;

public enum FacingDirection { Down = 0, Left = 1, Right = 2, Up = 3 }

public class Player
{
    private readonly Texture2D _sheet;

    private readonly int _frameSize = 16;

    // Player animation subset
    private readonly int _firstColumn = 6;    // column where player frames start (6 * 16 = 96px)
    private readonly int _frameCount = 4;     // number of animation frames per direction

    private readonly float _secondsPerFrame = 0.15f;

    private float _animTimer;
    private int _currentFrame;
    private bool _moving;

    private SpriteEffects _effects = SpriteEffects.None;

    public Vector2 Position;
    public FacingDirection Facing = FacingDirection.Down;
    public float Speed = 120f;

    public Player(Texture2D sheet, Vector2 startPos)
    {
        _sheet = sheet;
        Position = startPos;
    }

    public void Update(GameTime gameTime)
    {
        var k = Keyboard.GetState();
        Vector2 dir = Vector2.Zero;

        if (k.IsKeyDown(Keys.W) || k.IsKeyDown(Keys.Up))
        {
            dir.Y -= 1;
            Facing = FacingDirection.Up;
        }
        else if (k.IsKeyDown(Keys.S) || k.IsKeyDown(Keys.Down))
        {
            dir.Y += 1;
            Facing = FacingDirection.Down;
        }

        if (k.IsKeyDown(Keys.A) || k.IsKeyDown(Keys.Left))
        {
            dir.X -= 1;
            Facing = FacingDirection.Left;
        }
        else if (k.IsKeyDown(Keys.D) || k.IsKeyDown(Keys.Right))
        {
            dir.X += 1;
            Facing = FacingDirection.Right;
        }

        _moving = dir != Vector2.Zero;

        if (_moving)
        {
            dir.Normalize();
            Position += dir * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _animTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_animTimer >= _secondsPerFrame)
            {
                _animTimer -= _secondsPerFrame;
                _currentFrame = (_currentFrame + 1) % _frameCount;
            }
        }
        else
        {
            _currentFrame = 0;
            _animTimer = 0f;
        }
    }

    private Rectangle GetSource()
    {
        int row = GetRowForFacing(Facing);
        bool flip = (Facing == FacingDirection.Right);

        _effects = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        int column = _firstColumn + _currentFrame;
        return new Rectangle(column * _frameSize, row * _frameSize, _frameSize, _frameSize);
    }

    private int GetRowForFacing(FacingDirection facing)
    {
        return facing switch
        {
            FacingDirection.Left => 0, // row 0
            FacingDirection.Up => 1,   // row 1
            FacingDirection.Down => 2, // row 2
            FacingDirection.Right => 0, // reuse left row (flipped)
            _ => 2
        };
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(_sheet, Position, GetSource(), Color.White, 0f, Vector2.Zero, 1f, _effects, 0f);
    }
}