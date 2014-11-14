using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Timers;




namespace WindowsGame7
{
    public class generate
    {
        Random r = new Random();
        int a, b = 0;
        public int x()
        {
            a = r.Next(1, 1000);
            return a;
        }

        public int y()
        {
            b = r.Next(1, 1000);
            return b;
        }
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region define 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera2D m_camera;
        
        generate generator = new generate();
        Texture2D jelTexture;
        Texture2D obj1Texture;
        Texture2D worldTexture;
        Texture2D ammoTexture;
        Rectangle jelRect;
        Rectangle worldRectum;
        Rectangle qRect;
        Rectangle obj1;
        Vector2 spritePosition = new Vector2(80.0f, 450.0f);
        Vector2 spriteSpeed = new Vector2(0.05f, 0.05f);
        Vector2 spriteVel;
        int maxVel;
        GameTime GameT;
        bool jumping;
        float jumpspeed = 0;
        float startY = new float();
        float colMaxX = new float();
        float colMaxY = new float();
        float colMinX = new float();
        float colMinY = new float();


        int a, b;

        int maxX = 0;
        int minX = 0;
        int maxY = 0;
        int minY = 0;
        Rectangle ammoRect;
        #endregion

        public GraphicsDevice graphDev
        {
            get { return graphics.GraphicsDevice; }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            jelRect = new Rectangle(0, 0, 60, 60);
            worldRectum = new Rectangle(0, 0, 3000, 3000);
            ammoRect = new Rectangle(generator.x(), generator.y(), 20, 20);
            m_camera = new Camera2D();
            obj1 = new Rectangle(80, 80, 100, 100);
            base.Initialize();
            IsMouseVisible = true;

            jumping = false;
            jumpspeed = 0;            

        }
        float movVel, posM = new float();

        new bool pos = false;

        bool mLst = false;
        Vector2 target = new Vector2();
        Vector2 tempTarget = new Vector2();
        Vector2 velocity = new Vector2();
        bool collided = false;

        void mouseMov()
        {
            movVel = 4.8f;
            MouseState m = Mouse.GetState();

            if (((spritePosition.X + 60 > obj1.X && (spritePosition.X < obj1.X + obj1.Width)) &&
                    (spritePosition.Y + 60 > obj1.X && (spritePosition.Y < obj1.Y + obj1.Height))) && !collided)
            {
                tempTarget = target;
                coll = "Can't go there";
                mLst = false;
                collided = true;
            }
            else coll = "";

            if (m.X < minX) { m_camera.Move(new Vector2(-1, 0)); minX--; maxX--; }

            if (m.X > maxX) { m_camera.Move(new Vector2(1, 0)); minX++; maxX++; }

            if (m.Y < minY) { m_camera.Move(new Vector2(0, -1)); minY--; maxY--; }

            if (m.Y > maxY) { m_camera.Move(new Vector2(0, 1)); minY++; maxY++; }

            if (m.RightButton == ButtonState.Pressed)
            {
                target.Y = m.Y - 15;
                target.X = m.X - 15;
                coord2 = "X: " + m.X + " Y: " + m.Y;
                mLst = true;
                collided = false;
            }

            coord3 = "X: " + obj1.X + " Y: " + obj1.Y;
            coord = "X: " + spritePosition.X + " Y: " + spritePosition.Y;
            if (!mLst)
            {
                target.Y = m.Y - 15;
                target.X = m.X - 15;
                coord2 = "X: " + m.X + " Y: " + m.Y;
            }

            if (spritePosition != target && mLst == true)
            {

                if (Vector2.Distance(spritePosition, target) >= 1.0f)
                {
                    velocity = target -spritePosition;
                    velocity.Normalize();
                    /// Now no matter which direction we are going we are always moving @ sprite.Speed
                    /// at velocity or speed below 1 - problems occur where the unit may not move at all!!
                    if (movVel < 1)
                    {
                        Vector2 temp = (velocity * 10) * (movVel * 10);
                        spritePosition += temp / 10;
                    }
                    else
                    {
                        Vector2 temp = velocity * movVel;
                        spritePosition += temp;
                    }
                    //convert to int to render sprite to pixel perfect..
                    spritePosition = new Vector2((int)spritePosition.X, (int)spritePosition.Y);
                }
            }
            else mLst = false;
        }


        #region Constructors&MovementVoids
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            jelTexture = this.Content.Load<Texture2D>("jel1");
            worldTexture = this.Content.Load<Texture2D>("starfield");
            obj1Texture = this.Content.Load<Texture2D>("Desert");
            minY = 0;
            minX = 0;
            maxX = graphics.GraphicsDevice.Viewport.Width - jelRect.Width;
            maxY = graphics.GraphicsDevice.Viewport.Height - jelRect.Height;
            


        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }        
        public void camControls()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
                m_camera.Move(new Vector2(-1, 0));
            if (keyState.IsKeyDown(Keys.Left))
                m_camera.Move(new Vector2(1, 0));
            if (keyState.IsKeyDown(Keys.Up))
                m_camera.Move(new Vector2(0, 1));
            if (keyState.IsKeyDown(Keys.Down))
                m_camera.Move(new Vector2(0, -1));
        }
        public void exit()
        {
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

        }
        protected int count = 0;
        public void move2()
        {
            if (jelRect.Intersects(obj1))
            {
                exit();
            }
            KeyboardState kState = Keyboard.GetState();
            
                if (kState.IsKeyDown(Keys.W))
                {
                    if (!obj1.Intersects(new Rectangle(
                                   (int)spritePosition.X,
                                   (int)spritePosition.Y,
                                   jelRect.Width,
                                   jelRect.Height)))
                    if (spritePosition.Y > minY + 10)
                        spritePosition.Y -= 5.0f;
                    else { m_camera.Move(new Vector2(0, -1)); minY--; maxY--; }
                }
                if (kState.IsKeyDown(Keys.A))
                {
                    if (!obj1.Intersects(new Rectangle(
                                   (int)spritePosition.X,
                                   (int)spritePosition.Y,
                                   jelRect.Width,
                                   jelRect.Height)))
                    if (spritePosition.X > minX + 10)
                            spritePosition.X -= 5.0f;
                    else { m_camera.Move(new Vector2(-1, 0)); minX--; maxX--; }
                }
                if (kState.IsKeyDown(Keys.D))
                {
                    if (!obj1.Intersects(new Rectangle(
                                   (int)spritePosition.X,
                                   (int)spritePosition.Y,
                                   jelRect.Width,
                                   jelRect.Height)))
                        if (spritePosition.X < maxX - 10)
                            spritePosition.X += 5.0f;
                        else { m_camera.Move(new Vector2(1, 0)); minX++; maxX++; }
                }
                if (kState.IsKeyDown(Keys.S))
                {
                    if (!obj1.Intersects(new Rectangle(
                                   (int)spritePosition.X,
                                   (int)spritePosition.Y,
                                   jelRect.Width,
                                   jelRect.Height)))
                    if (spritePosition.Y < maxY - 10)
                        spritePosition.Y += 5.0f;
                    else { m_camera.Move(new Vector2(0, 1)); minY++; maxY++; }
                }
            }

        public bool intersW, intersA, intersS, intersD;
        #endregion

        protected override void Update(GameTime gameTime)
        {
            if (!jumping) startY = spritePosition.Y;
            KeyboardState keyState = Keyboard.GetState();
            mouseMov();
            camControls();
            exit();
            move2();

            if (jelRect.Intersects(qRect))
            {
            }
            base.Update(gameTime);
        }

        string coord, coord2, coord3, coll = "test";
        SpriteFont Font1;

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(worldTexture, 
                             new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), 
                             Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, 
                              null, null, m_camera.Transform(graphics.GraphicsDevice));
            //spriteBatch.Draw(ammoTexture, new Vector2(generator.x(), generator.y()), Color.White);

            Font1 = Content.Load<SpriteFont>("Courier New");
            Vector2 FontOrigin = Font1.MeasureString(coord) / 2;

            spriteBatch.Draw(jelTexture, spritePosition, jelRect, Color.White);
            spriteBatch.Draw(obj1Texture, obj1, Color.White);

            spriteBatch.DrawString(Font1, coord, new Vector2(40, 25), Color.Cyan,
                                   0, FontOrigin, 0.8f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(Font1, coord2, new Vector2(40, 10), Color.Cyan,
                                   0, FontOrigin, 0.8f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(Font1, coord3, new Vector2(40, 40), Color.Cyan,
                                   0, FontOrigin, 0.8f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(Font1, coll, new Vector2(40, 60), Color.Cyan,
                                   0, FontOrigin, 0.8f, SpriteEffects.None, 0.5f);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }

    class Camera2D
    {
        //Members
        private Vector2 m_CameraPosition;
        private float m_Zoom;
        private float m_Rotation;
        private Matrix m_Transform;
        KeyboardState kState = Keyboard.GetState();

        public Camera2D()
        {
            m_Zoom = 1f;
            m_Rotation = 0;
            m_CameraPosition = Vector2.Zero;
        }

        #region Set/Get

        
        public float Zoom
        {
            get { return m_Zoom; }
            set { m_Zoom = value; if (m_Zoom < 0.1f) m_Zoom = 0.1f; } // Negative zoom will flip image
        }

        
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            m_CameraPosition += amount;
        }
        
        public Vector2 Position
        {
            get { return m_CameraPosition; }
            set { m_CameraPosition = value; }
        }

        #endregion
        public Matrix Transform(GraphicsDevice graphicsDevice)
        {
            float ViewportWidth = graphicsDevice.Viewport.Width;
            float ViewportHeight = graphicsDevice.Viewport.Height;

            m_Transform =
              Matrix.CreateTranslation(new Vector3(-m_CameraPosition.X, -m_CameraPosition.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                                         Matrix.CreateTranslation(new Vector3(0, 0, 0));
            return m_Transform;
        }
    }
}