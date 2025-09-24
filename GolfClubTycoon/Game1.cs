using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GolfClubTycoon;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player _player;
    private Texture2D _spriteSheet;
    private Camera2D _camera;
    private Tileset _tileset;
    private TileMap _tileMap;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _spriteSheet = Content.Load<Texture2D>("golf-removebg");
        _player = new Player(_spriteSheet, new Vector2(200, 200));
        _camera = new Camera2D(GraphicsDevice);
        // Center camera on initial player position (add half frame to center on sprite middle)
        _camera.Update(_player.Position + new Vector2(8, 8));

        // World / tileset setup (reuse same sheet for now)
        _tileset = new Tileset(_spriteSheet, 16);
        _tileMap = new TileMap(64, 64, _tileset, 16);
        _tileMap.GenerateHole(new Point(8, 54), new Point(52, 10));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _player.Update(gameTime);
        // Update camera after player movement
        _camera.Update(_player.Position + new Vector2(8, 8)); // 16px frame -> +8 centers

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
        _tileMap.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
