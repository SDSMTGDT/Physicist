namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
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
        private Dictionary<MediaFormat, MediaElementKeyedDictionary<MediaElement>> media = null;

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
                            mediaReferences.Add(mediaInfo.Value);
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

                this.media = new Dictionary<MediaFormat, MediaElementKeyedDictionary<MediaElement>>();

                foreach (MediaFormat typename in Enum.GetValues(typeof(MediaFormat)))
                {
                    this.media.Add(typename, new MediaElementKeyedDictionary<MediaElement>());
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
                        // Don't thorw if the assets are the same
                        if (this.media[assetFormat][assetName].Location != assetPath)
                        {
                            throw;
                        }
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
                if (Enum.TryParse<MediaFormat>(typeof(T).Name, out assetFormat) & this.media.ContainsKey(assetFormat) && this.media[assetFormat].ContainsKey(assetName))
                {
                    asset = this.media[assetFormat][assetName].Asset as T;
                }
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
                reference = this.media[assetFormat].FirstOrDefault(media => media.Value.Asset.Equals(asset)).Value;
            }

            return reference;
        }

        public void Reset()
        {
            this.media.Clear();
            this.IsInitialized = false;
        }
    }
}
