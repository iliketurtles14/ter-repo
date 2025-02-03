using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static bool ConvertWavToPCM(byte[] wavData, out float[] pcmData, out int sampleRate, out int channels)
    {
        using (MemoryStream stream = new MemoryStream(wavData))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            // Check "RIFF" and "WAVE" headers
            if (reader.ReadInt32() != 0x46464952) { pcmData = null; sampleRate = 0; channels = 0; return false; }
            reader.ReadInt32(); // Skip file size
            if (reader.ReadInt32() != 0x45564157) { pcmData = null; sampleRate = 0; channels = 0; return false; }

            // Search for "fmt " chunk
            while (reader.ReadInt32() != 0x20746D66)
            {
                int chunkSize = reader.ReadInt32();
                reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
            }

            int fmtChunkSize = reader.ReadInt32();
            int audioFormat = reader.ReadInt16();
            channels = reader.ReadInt16();
            sampleRate = reader.ReadInt32();
            reader.ReadInt32(); // Byte rate
            reader.ReadInt16(); // Block align
            int bitsPerSample = reader.ReadInt16();

            if (audioFormat != 1 || bitsPerSample != 16) // PCM format & 16-bit only
            {
                pcmData = null;
                return false;
            }

            // Search for "data" chunk
            while (reader.ReadInt32() != 0x61746164)
            {
                int chunkSize = reader.ReadInt32();
                reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
            }

            int dataSize = reader.ReadInt32();
            int sampleCount = dataSize / (bitsPerSample / 8);
            pcmData = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
                pcmData[i] = reader.ReadInt16() / 32768f; // Convert 16-bit PCM to float (-1 to 1)

            return true;
        }
    }
}
