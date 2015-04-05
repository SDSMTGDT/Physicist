﻿namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Media;
    using Physicist.Enums;
    using Physicist.Extensions;
    using Physicist.Extensions.Primitives;

    public class ContentController
    {
        private static object lockObject = new object();
        private static ContentController instance = null;
        private Dictionary<MediaFormat, MediaElementKeyedCollection<MediaElement>> media = null;
        private Dictionary<MediaFormat, Dictionary<string, string>> mediaAlias = null;

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
                else if (!ContentController.instance.IsInitialized)
                {
                    Console.WriteLine("Warning: Content Controller is not yet initialized");
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
                this.mediaAlias = new Dictionary<MediaFormat, Dictionary<string, string>>();

                foreach (MediaFormat typename in Enum.GetValues(typeof(MediaFormat)))
                {
                    this.media.Add(typename, new MediaElementKeyedCollection<MediaElement>());
                    this.mediaAlias.Add(typename, new Dictionary<string, string>());
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
                        bool inMedia = this.media[assetFormat].Contains(assetName);
                        bool inAlias = this.mediaAlias[assetFormat].ContainsKey(assetName);
                        bool pathExists = this.media[assetFormat].ContainsLocation(assetPath);
                        if (!inMedia && !inAlias)
                        {
                            if (!pathExists)
                            {
                                this.media[assetFormat].Add(new MediaElement(assetName, assetPath, this.Content.Load<T>(assetPath)));
                            }
                            else
                            {
                                this.mediaAlias[assetFormat].Add(assetName, this.media[assetFormat].KeyForLocation(assetPath));
                            }
                        }
                        else if (pathExists)
                        {
                            var info = this.GetMediaReference<T>(assetName);
                            if (string.Compare(info.Location, assetPath, StringComparison.CurrentCulture) != 0)
                            {
                                throw new ContentLoadException("Load results in overwrite of content located at: " +
                                                                this.media[assetFormat][assetName].Location +
                                                                " check that this content was unloaded successfully");
                            }
                        }
                    }
                    catch (ContentLoadException e)
                    {
                        this.mediaAlias[assetFormat].Add(assetName, this.media[assetFormat]["ContentLoadError"].Name);
                        Console.WriteLine("Error while loading content: " + assetName + ", " + e.Message);
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
            if (this.IsInitialized)
            {
                if (this.media.ContainsKey(assetFormat))
                {
                    if (this.media[assetFormat].Contains(assetName))
                    {
                        string aliasKey = null;
                        string location = this.media[assetFormat][assetName].Location;
                        foreach (var alias in this.mediaAlias[assetFormat])
                        {
                            if (string.Compare(location, alias.Value, StringComparison.CurrentCulture) == 0)
                            {
                                aliasKey = alias.Key;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(aliasKey))
                        {
                            this.mediaAlias[assetFormat].Remove(aliasKey);
                            this.media[assetFormat].ChangeItemKey(this.media[assetFormat][assetName], aliasKey);
                        }
                        else
                        {
                            this.media[assetFormat].Remove(assetName);
                        }
                    }
                    else if (this.mediaAlias[assetFormat].ContainsKey(assetName))
                    {
                        this.mediaAlias[assetFormat].Remove(assetName);
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Follows Monogame Content Pattern")]
        public void UnloadContent<T>(string assetName) where T : class
        {
            if (this.IsInitialized)
            {
                MediaFormat assetFormat = (MediaFormat)Enum.Parse(typeof(MediaFormat), typeof(T).Name);
                this.UnloadContent(assetName, assetFormat);
            }
        }

        public T GetContent<T>(string assetName) where T : class
        {
            T asset = null;
            if (this.IsInitialized)
            {
                MediaFormat assetFormat;
                if (Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat) && this.media.ContainsKey(assetFormat))
                {
                    string key = assetName;
                    if (this.mediaAlias[assetFormat].ContainsKey(key))
                    {
                        key = this.mediaAlias[assetFormat][key];
                    }

                    if (this.media[assetFormat].Contains(key))
                    {
                        asset = this.media[assetFormat][key].Asset as T;
                    }

                    if (asset == null)
                    {
                        Console.WriteLine("Failed to get Content: " + assetName + ", content not found!");
                        asset = this.media[assetFormat]["ContentLoadError"].Asset as T;
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get Content: " + assetName + ", content of type: " + typeof(T).Name + " is not supported!");
                }
            }

            return asset;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Follows Monogame Content Pattern")]
        public IMediaInfo GetMediaReference<T>(string assetName) where T : class
        {
            IMediaInfo reference = null;
            if (this.IsInitialized)
            {
                MediaFormat assetFormat;
                if (Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat) && this.media.ContainsKey(assetFormat))
                {
                    string key = assetName;
                    if (this.mediaAlias[assetFormat].ContainsKey(key))
                    {
                        key = this.mediaAlias[assetFormat][key];
                    }

                    if (this.media[assetFormat].Contains(key))
                    {
                        reference = this.media[assetFormat][key];
                    }

                    if (reference == null)
                    {
                        Console.WriteLine("Failed to get reference for content: " + assetName + ", content not found!");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get reference for content: " + assetName + ", content of type: " + typeof(T).Name + " is not supported!");
                }
            }

            return reference;
        }

        public IMediaInfo GetMediaReference<T>(T asset) where T : class
        {
            IMediaInfo reference = null;
            MediaFormat assetFormat;
            if (this.IsInitialized && asset != null)
            {
                if (Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat))
                {
                    reference = this.media[assetFormat].FirstOrDefault(media => media.Asset.Equals(asset));

                    if (reference == null)
                    {
                        Console.WriteLine("Failed to get reference for asset, content not found!");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get reference for asset, content of type: " + typeof(T).Name + " is not supported!");
                }
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
    }
}
