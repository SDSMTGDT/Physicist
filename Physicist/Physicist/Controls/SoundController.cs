namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public static class SoundController
    {
        private static AudioListener listener;
        private static Dictionary<Guid, SoundEffectInstance> instances = new Dictionary<Guid, SoundEffectInstance>();

        public static Map Map { get; set; }

        public static Guid PlaySound(string name)
        {
            SoundEffect effect = ContentController.Instance.GetContent<SoundEffect>(name);
            if (effect == null)
            {
                throw new PhysicistSoundControllerException("Missing sound content: " + name);
            }

            SoundEffectInstance instance = effect.CreateInstance();

            if (SoundController.instances == null)
            {
                SoundController.instances = new Dictionary<Guid, SoundEffectInstance>();
            }
            
            Guid id = Guid.NewGuid();
            id = Guid.NewGuid();
            SoundController.instances.Add(id, instance);
            instance.Play();
            return id;
        }

        public static Guid PlaySound(string name, bool looping, float volume, float pitch, float pan)
        {
            SoundEffect effect = ContentController.Instance.GetContent<SoundEffect>(name);
            if (effect == null)
            {
                throw new PhysicistSoundControllerException("Missing sound content: " + name);
            }

            SoundEffectInstance instance = effect.CreateInstance();
            
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.IsLooped = looping;

            if (SoundController.instances == null)
            {
                SoundController.instances = new Dictionary<Guid, SoundEffectInstance>();
            }

            Guid id = Guid.NewGuid();
            id = Guid.NewGuid();
            SoundController.instances.Add(id, instance);
            instance.Play();
            return id;
        }

        public static Guid PlayLocalizedSound(string name, AudioEmitter emitter)
        {
            SoundEffect effect = ContentController.Instance.GetContent<SoundEffect>(name);
            SoundEffectInstance instance = effect.CreateInstance();

            if (emitter != null)
            {
                instance.Apply3D(SoundController.listener, emitter);
            }

            if (SoundController.instances == null)
            {
                SoundController.instances = new Dictionary<Guid, SoundEffectInstance>();
            }

            Guid id = new Guid();
            SoundController.instances.Add(id, instance);
            instance.Play();
            return id;
        }

        public static Guid PlayLocalizedSound(string name, AudioEmitter emitter, bool looping, float volume, float pitch, float pan)
        {
            SoundEffect effect = ContentController.Instance.GetContent<SoundEffect>(name);
            SoundEffectInstance instance = effect.CreateInstance();
            
            instance.Volume = volume;
            instance.Pitch = pitch;
            instance.Pan = pan;
            instance.IsLooped = looping;

            if (emitter != null)
            {
                instance.Apply3D(SoundController.listener, emitter);
            }

            if (SoundController.instances == null)
            {
                SoundController.instances = new Dictionary<Guid, SoundEffectInstance>();
            }

            Guid id = new Guid();
            SoundController.instances.Add(id, instance);
            instance.Play();
            return id;
        }

        public static void StopSound(Guid id)
        {
            if (SoundController.instances.ContainsKey(id))
            {
                SoundEffectInstance instance = SoundController.instances[id];
                if (instance.State == SoundState.Playing)
                {
                    instance.Stop();
                    instance.Dispose();
                    SoundController.instances.Remove(id);
                }
            }
        }

        public static void PauseSound(Guid id)
        {
            if (SoundController.instances.ContainsKey(id))
            {
                SoundEffectInstance instance = SoundController.instances[id];
                if (instance.State == SoundState.Playing)
                {
                    instance.Pause();
                }
            }
        }

        public static void ResumeSound(Guid id)
        {
            if (SoundController.instances.ContainsKey(id))
            {
                SoundEffectInstance instance = SoundController.instances[id];
                if (instance.State == SoundState.Paused)
                {
                    instance.Resume();
                }
            }
        }

        public static bool SoundIsPlaying(Guid id)
        {
            if (SoundController.instances.ContainsKey(id))
            {
                SoundEffectInstance instance = SoundController.instances[id];
                return instance.State == SoundState.Playing;
            }

            return false;
        }

        public static void Update()
        {
            for (int i = SoundController.instances.Count - 1; i >= 0; i--)
            {
                var entry = SoundController.instances.ElementAt(i);
                if (entry.Value.State == SoundState.Stopped)
                {
                    entry.Value.Dispose();
                    SoundController.instances.Remove(entry.Key);
                }
            }

            if (Map.Players.Count() > 0 && Map.Players.ElementAt(0) != null)
            {
                SoundController.listener = Map.Players.ElementAt(0).Listener;
            }
        }

        public static bool ContainsInstance(Guid id)
        {
            if (SoundController.instances.ContainsKey(id))
            {
                return true;
            }

            return false;
        }
    }
}