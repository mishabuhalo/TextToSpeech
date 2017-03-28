using System;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;
using System.ComponentModel;

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
        int stream;
        string text = "";
        int position = 0;
        BackgroundWorker worker;

        SpeechPart[] parts;

        public Speech()
        {
            DataContractJsonSerializer serialazer = new DataContractJsonSerializer(typeof(SpeechPart[]));
            using (FileStream stream = new FileStream("Sounds.json", FileMode.Open))
            {
                parts = (SpeechPart[])serialazer.ReadObject(stream);
            }

            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            stream = Bass.BASS_StreamCreateFile("Sounds.wav", 0L, 0L, BASSFlag.BASS_STREAM_DECODE);
            stream = BassFx.BASS_FX_TempoCreate(stream, BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_FX_FREESOURCE);
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_TEMPO, 0);

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Run;
            worker.RunWorkerCompleted += Done;
        }

        ~Speech()
        {
            Bass.BASS_Free();
        }

        void Run(object sender, DoWorkEventArgs e)
        {
            while (!(sender as BackgroundWorker).CancellationPending && position < text.Length)
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

        void Done(object sender, RunWorkerCompletedEventArgs e)
        {
            Stopped?.Invoke(this, new EventArgs());
        }

        public void Start()
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
                Started?.Invoke(this, new EventArgs());
            }
        }

        public void Stop()
        {
            worker.CancelAsync();
        }

        public bool Active => worker.IsBusy;

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
