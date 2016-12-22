using System;
using Un4seen.Bass;

namespace TextToSpeech
{
    class SpeechPart
    {
        public string Part;
        public int Start;
        public int Duration;
    }

    class Speech
    {
        bool bassReady = false;
        int stream = 0;

        bool active = false;
        string text = "";
        int position = 0;

        public Speech()
        {
            bassReady = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
        }

        ~Speech()
        {
            if (bassReady)
            {
                Bass.BASS_Free();
            }
        }

        public void Start()
        {
            if (!active)
            {
                active = true;
                stream = Bass.BASS_StreamCreateFile("Sounds.mp3", 0L, 0L, BASSFlag.BASS_DEFAULT);
                if (stream != 0)
                {
                    Bass.BASS_ChannelPlay(stream, false);
                }
            }
        }

        public void Stop()
        {

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
            return new SpeechPart();
        }
        
        void Playpart(SpeechPart Part)
        {

        }
    }
}
