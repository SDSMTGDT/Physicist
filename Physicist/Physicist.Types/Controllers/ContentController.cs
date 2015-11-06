namespace Physicist.Types.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Common;
    using Physicist.Types.Enums;
    using Physicist.Types.Util;

    public class ContentController
    {
        private static object lockObject = new object();
        private static ContentController instance = null;
        private Dictionary<MediaFormat, MediaElementKeyedCollection<MediaElement>> media = null;

        private ContentController()
        {
            this.IsInitialized = false;
        }

        public static ContentController Instance
        {
            get
            {
                if (ContentController.instance == null)
                {
                    lock (ContentController.lockObject)
                    {
                        if (ContentController.instance == null)
                        {
                            ContentController.instance = new ContentController();
                        }
                    }
                }

                return ContentController.instance;
            }
        }

        public bool IsInitialized
        {
            get;
            private set;
        }

        public IEnumerable<IMediaInfo> MediaReferences
        {
            get
            {
                List<IMediaInfo> mediaReferences = new List<IMediaInfo>();
                if (this.IsInitialized)
                {
                    foreach (var pair in this.media)
                    {
                        foreach (var mediaInfo in pair.Value)
                        {
                            mediaReferences.Add(mediaInfo);
                        }
                    }
                }

                return mediaReferences;
            }
        }

        private ContentManager Content { get; set; }

        public void Initialize(ContentManager content, string rootDirectory)
        {
            if (content != null)
            {
                this.Content = content;
                this.Content.RootDirectory = rootDirectory;

                this.media = new Dictionary<MediaFormat, MediaElementKeyedCollection<MediaElement>>();

                foreach (MediaFormat typename in Enum.GetValues(typeof(MediaFormat)))
                {
                    this.media.Add(typename, new MediaElementKeyedCollection<MediaElement>());
                }

                this.IsInitialized = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Follows Monogame Content Pattern")]
        public void LoadContent<T>(string assetName, string assetPath) where T : class
        {
            if (this.IsInitialized)
            {
                MediaFormat assetFormat = (MediaFormat)Enum.Parse(typeof(MediaFormat), typeof(T).Name);
                if (this.media.ContainsKey(assetFormat))
                {
                    try
                    {
                        this.media[assetFormat].Add(new MediaElement(assetName, assetPath, this.Content.Load<T>(assetPath)));
                    }
                    catch (ArgumentException)
                    {
                        // Don't throw if the assets are the same
                        if (string.Compare(this.media[assetFormat][assetName].Location, assetPath, StringComparison.CurrentCultureIgnoreCase) != 0)
                        {
                            throw;
                        }
                    }
                    catch (ContentLoadException)
                    {
                        this.media[assetFormat].Add(new MediaElement(assetName, assetPath, this.media[MediaFormat.Texture2D]["ContentLoadError"].Asset));
                    }
                }
                else
                {
                    throw new ContentLoadException("Error, content of type: " + typeof(T).Name + " is not supported!");
                }
            }
        }

        public void UnloadContent(string assetName, MediaFormat assetFormat)
        {
            if (this.media.ContainsKey(assetFormat))
            {
                this.media[assetFormat].Remove(this.media[assetFormat][assetName]);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Follows Monogame Content Pattern")]
        public void UnloadContent<T>(string assetName) where T : class
        {
            if (this.IsInitialized)
            {
                MediaFormat assetFormat = (MediaFormat)Enum.Parse(typeof(MediaFormat), typeof(T).Name);
                if (this.media.ContainsKey(assetFormat))
                {
                    this.media[assetFormat].Remove(this.media[assetFormat][assetName]);
                }
            }
        }

        public void AddContent<T>(string assetName, T asset) where T : class
        {
            if (this.IsInitialized)
            {
                MediaFormat assetFormat = (MediaFormat)Enum.Parse(typeof(MediaFormat), typeof(T).Name);
                if (this.media.ContainsKey(assetFormat))
                {
                    this.media[assetFormat].Add(new MediaElement(assetName, null, asset));
                }
                else
                {
                    throw new ContentLoadException("Error, content of type: " + typeof(T).Name + " is not supported!");
                }
            }
        }

        public T GetContent<T>(string assetName) where T : class
        {
            T asset = null;
            if (this.IsInitialized)
            {
                MediaFormat assetFormat;
                if (Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat) && this.media.ContainsKey(assetFormat) && this.media[assetFormat].Contains(assetName))
                {
                    asset = this.media[assetFormat][assetName].Asset as T;
                }
                else
                {
                    Console.WriteLine("Failed to Load Content: " + assetName);
                }
            }

            if (asset == null)
            {
                throw new KeyNotFoundException();
            }

            return asset;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Follows Monogame Content Pattern")]
        public IMediaInfo GetMediaReference<T>(string key) where T : class
        {
            IMediaInfo reference = null;
            MediaFormat assetFormat;
            if (this.IsInitialized && Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat))
            {
                reference = this.media[assetFormat][key];
            }

            return reference;
        }

        public IMediaInfo GetMediaReference<T>(T asset) where T : class
        {
            IMediaInfo reference = null;
            MediaFormat assetFormat;
            if (this.IsInitialized & asset != null && Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat))
            {
                reference = this.media[assetFormat].FirstOrDefault(media => media.Asset.Equals(asset));
            }

            return reference;
        }

        public Texture2D GetTextureMaterial(GraphicsDevice graphicsDevice, string assetName)
        {
            Texture2D materialTexture = this.GetContent<Texture2D>(assetName);
            if (materialTexture.Width != materialTexture.Height)
            {
                Texture2D texture = null;
                int minBound = (int)MathHelper.Min(materialTexture.Width, materialTexture.Height);
                Color[] materialColors = new Color[minBound * minBound];
                Color[] textureColors = new Color[materialTexture.Width * materialTexture.Height];
                materialTexture.GetData(textureColors);

                for (int i = 0; i < minBound; i++)
                {
                    for (int j = 0; j < minBound; j++)
                    {
                        materialColors[(i * minBound) + j] = textureColors[(i * materialTexture.Width) + j];
                    }
                }

                try
                {
                    texture = new Texture2D(graphicsDevice, minBound, minBound);
                    texture.SetData(materialColors);
                    materialTexture = texture;
                    texture = null;
                }
                finally
                {
                    if (texture != null)
                    {
                        texture.Dispose();
                    }
                }
            }

            return materialTexture;
        }

        public void Reset()
        {
            this.media.Clear();
            this.IsInitialized = false;
        }
    }
}
