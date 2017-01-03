using System;
using Un4seen.Bass;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;

namespace TextToSpeech
{
    [DataContract]
    class SpeechPart
    {
        [DataMember]
        public string Part;
        [DataMember]
        public int Start;
        [DataMember]
        public int Duration;
    }

    class Speech
    {
        bool bassReady = false;
        int stream = 0;

        bool active = false;
        bool terminated = false;
        string text = "";
        int position = 0;

        SpeechPart[] parts;

        public Speech()
        {
            DataContractJsonSerializer serialazer = new DataContractJsonSerializer(typeof(SpeechPart[]));
            using (FileStream stream = new FileStream("Sounds.json", FileMode.Open))
            {
                parts = (SpeechPart[])serialazer.ReadObject(stream);
            }

            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            stream = Bass.BASS_StreamCreateFile("Sounds.mp3", 0L, 0L, BASSFlag.BASS_DEFAULT);
        }

        ~Speech()
        {
            Bass.BASS_Free();
        }

        public void Start()
        {
            /*
                stream = Bass.BASS_StreamCreateFile("Sounds.mp3", 0L, 0L, BASSFlag.BASS_DEFAULT);
                if (stream != 0)
                {
                    Bass.BASS_ChannelPlay(stream, false);
                }   
            */
            while (!terminated && position < text.Length)
            {
                SpeechPart part = GetPart();
                if (part != null)
                {
                    PlayPart(part);
                    position += part.Part.Length;
                }
                else
                {
                    position++;
                }
            }
        }

        public void Stop()
        {
            Bass.BASS_ChannelStop(stream);
        }

        public bool Active => active;

        public string Text
        {
            get { return text; }
            set { text = value; position = 0; }
        }
        
        public int Position
        {
            get { return position; }
            set
            {
                if (value < 0 || value >= text.Length)
                {
                    position = 0;
                }
                else
                {
                    position = value;
                }
            }
        }
        
        public event EventHandler Started;
        public event EventHandler Stopped;

        SpeechPart GetPart()
        {
            foreach (SpeechPart part in parts)
            {
                int p = text.IndexOf(part.Part, position, StringComparison.OrdinalIgnoreCase);
                if (p == position)
                {
                    return part;
                }
            }

            return null;
        }
        
        void PlayPart(SpeechPart part)
        {
            if (part.Start >= 0)
            {
                Bass.BASS_ChannelSetPosition(stream, part.Start / 1000.0);
                Bass.BASS_ChannelPlay(stream, false);
                Thread.Sleep(part.Duration);
                Bass.BASS_ChannelStop(stream);
            }
            else
            {
                Thread.Sleep(part.Duration);
            }
        }
    }
}
