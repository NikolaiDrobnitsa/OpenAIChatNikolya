using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NAudio.Wave;
namespace ChatNikolya
{
    public class AudioRecorder : IDisposable
    {
        private WaveInEvent _waveIn;
        private WaveFileWriter _waveWriter;
        private string _outputFolder;

        public AudioRecorder(string outputFolder)
        {
            _outputFolder = outputFolder;
            Directory.CreateDirectory(_outputFolder);
        }
        public string PathToFile;
        public string FileNames;
        public void StartRecording()
        {
            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}.wav";
            var filePath = Path.Combine(_outputFolder, fileName);
            PathToFile = filePath;
            FileNames = fileName;

            _waveIn = new WaveInEvent();
            _waveIn.DeviceNumber = 0;
            _waveIn.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(_waveIn.DeviceNumber).Channels);
            _waveIn.DataAvailable += WaveIn_DataAvailable;

            _waveWriter = new WaveFileWriter(filePath, _waveIn.WaveFormat);

            _waveIn.StartRecording();
        }

        public void StopRecording()
        {
            _waveIn.StopRecording();
            _waveWriter.Dispose();
            _waveWriter = null;

        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            _waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }

        public void Dispose()
        {
            if (_waveIn != null)
            {
                _waveIn.Dispose();
                _waveIn = null;
            }

            if (_waveWriter != null)
            {
                _waveWriter.Dispose();
                _waveWriter = null;
            }
        }
    }
}
