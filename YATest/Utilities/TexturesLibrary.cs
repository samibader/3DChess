using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YATest.Utilities
{
    class TexturesLibrary
    {
        private ContentManager content;

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// For Blinn Effect
        /// </summary>
        private Texture2D blinnColorTextureDark;

        public Texture2D BlinnColorTextureDark
        {
            get { return blinnColorTextureDark; }
            set { blinnColorTextureDark = value; }
        }
        private Texture2D blinnColorTextureLight;

        public Texture2D BlinnColorTextureLight
        {
            get { return blinnColorTextureLight; }
            set { blinnColorTextureLight = value; }
        }
        /// <summary>
        /// For Phong Effect
        /// </summary>
        private Texture2D phongAvailable;

        public Texture2D PhongAvailable
        {
            get { return phongAvailable; }
            set { phongAvailable = value; }
        }

        private Texture2D phongSelected;

        public Texture2D PhongSelected
        {
            get { return phongSelected; }
            set { phongSelected = value; }
        }

        private Texture2D phongThreatened;

        public Texture2D PhongThreatened
        {
            get { return phongThreatened; }
            set { phongThreatened = value; }
        }

        private Texture2D phongHighLighted;

        public Texture2D PhongHighLighted
        {
            get { return phongHighLighted; }
            set { phongHighLighted = value; }
        }
        private Texture2D phongColorTextureLight;

        public Texture2D PhongColorTextureLight
        {
            get { return phongColorTextureLight; }
            set { phongColorTextureLight = value; }
        }

        private Texture2D phongColorTextureDark;

        public Texture2D PhongColorTextureDark
        {
            get { return phongColorTextureDark; }
            set { phongColorTextureDark = value; }
        }
        private Texture2D phongColorTextureBorder;

        public Texture2D PhongColorTextureBorder
        {
            get { return phongColorTextureBorder; }
            set { phongColorTextureBorder = value; }
        }
        private Texture2D phongColorTextureCorner;

        public Texture2D PhongColorTextureCorner
        {
            get { return phongColorTextureCorner; }
            set { phongColorTextureCorner = value; }
        }
        private Texture2D phongNormalTexture;

        public Texture2D PhongNormalTexture
        {
            get { return phongNormalTexture; }
            set { phongNormalTexture = value; }
        }
        private TextureCube phongEnvTexture;

        public TextureCube PhongEnvTexture
        {
            get { return phongEnvTexture; }
            set { phongEnvTexture = value; }
        }

        /// <summary>
        /// for Vevelty effect
        /// </summary>
        /// <param name="content"></param>

        private Texture2D velvetyColorTexture;

        public Texture2D VelvetyColorTexture
        {
            get { return velvetyColorTexture; }
            set { velvetyColorTexture = value; }
        }

        public TexturesLibrary(ContentManager content)
        {

            phongThreatened = content.Load<Texture2D>("Textures/border");
            phongAvailable = content.Load<Texture2D>("Textures/available");
            phongHighLighted = content.Load<Texture2D>("Textures/highlighted");
            phongSelected = content.Load<Texture2D>("Textures/selected");

            phongColorTextureBorder = content.Load<Texture2D>("Textures/sil");
            phongColorTextureCorner = content.Load<Texture2D>("Textures/sil");

            phongColorTextureDark = content.Load<Texture2D>("Textures/sil2");
            phongColorTextureLight = content.Load<Texture2D>("Textures/sil");
            phongNormalTexture = content.Load<Texture2D>("Textures/default_bump_normal");
            phongEnvTexture = content.Load<TextureCube>("Textures/default_reflection");
        }
    }
}
