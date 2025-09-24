using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GolfClubTycoon;

public class Camera2D
{
    private readonly GraphicsDevice _graphicsDevice;
    private Vector2 _position; // world position of camera center
    private float _zoom = 2f;  // default zoom (2x pixel scale)
    private readonly float _maxZoom = 4f;
    private readonly float _minZoom = 0.5f;

    public Camera2D(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public float Zoom
    {
        get => _zoom;
        set => _zoom = MathHelper.Clamp(value, _minZoom, _maxZoom);
    }

    public Vector2 Position => _position;

    /// <summary>
    /// Instantly centers camera on target world position (optionally with an offset).
    /// </summary>
    public void Update(Vector2 target, Vector2? offset = null)
    {
        var off = offset ?? Vector2.Zero;
        _position = target + off;
    }

    /// <summary>
    /// Returns the view matrix (translation + zoom) for SpriteBatch.Begin.
    /// Rounds translation to whole pixels (post-zoom) to prevent subpixel jitter.
    /// </summary>
    public Matrix GetViewMatrix()
    {
        var viewport = _graphicsDevice.Viewport;

        // World -> Camera translation (camera center to origin)
        float tx = -_position.X;
        float ty = -_position.Y;

        // Apply integer snapping after zoom scaling compensation.
        // Snap the final screen translation so pixels stay crisp.
        var screenCenterX = viewport.Width * 0.5f;
        var screenCenterY = viewport.Height * 0.5f;

        // Build matrix pieces.
        var translationToOrigin = Matrix.CreateTranslation(tx, ty, 0f);
        var scale = Matrix.CreateScale(_zoom, _zoom, 1f);
        var translationToScreenCenter = Matrix.CreateTranslation(screenCenterX, screenCenterY, 0f);

        var view = translationToOrigin * scale * translationToScreenCenter;

        // Pixel snapping: extract translation, round, rebuild (simple approach)
        view.Translation = new Vector3((int)view.Translation.X, (int)view.Translation.Y, view.Translation.Z);
        return view;
    }
}
