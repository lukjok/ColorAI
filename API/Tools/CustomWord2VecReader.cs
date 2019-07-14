using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tools
{
    /// <summary>
    /// w2v meaning vector
    /// </summary>
    public class Representation
    {
        public string WordOrNull { get; }
        public float[] NumericVector { get; }
        public double MetricLength { get; }

        public Representation(string word, float[] vector)
        {
            WordOrNull = word;
            NumericVector = vector;
            MetricLength = Math.Sqrt(NumericVector.Sum(v => v * v));
        }
        public Representation(float[] numericVector)
        {
            WordOrNull = null;
            NumericVector = numericVector;
            MetricLength = Math.Sqrt(NumericVector.Sum(v => v * v));
        }
    }

    public class CustomWord2VecReader
    {
        public Dictionary<string, Representation> vocabulary { get; }
        private readonly string _filePath;

        public CustomWord2VecReader(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified binary file was not found");

            vocabulary = new Dictionary<string, Representation>();
            _filePath = path;
        }

        public Representation FindOrNull(string word)
        {
            if (vocabulary.TryGetValue(word, out Representation result))
                return result;

            using (var inputStream = new FileStream(_filePath, FileMode.Open))
            {
                return FindOrNull(word, inputStream);
            }
        }

        public Representation FindOrNull(string word, Stream inputStream)
        {
            var readerSream = new BinaryReader(inputStream);

            // header
            var strHeader = Encoding.UTF8.GetString(ReadHead(readerSream));
            var split = strHeader.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 2)
                throw new FormatException("Header of binary must contains two ascii integers: vocabularySize and vectorSize");
            int vocabularySize = int.Parse(split[0]);
            int vectorSize = int.Parse(split[1]);

            var buffer = new List<byte>(); //allocation optimization

            while (true)
            {
                try
                {
                    var wordRep = ReadRepresentation(readerSream, buffer, vectorSize);
                    if(wordRep.WordOrNull == word)
                    {
                        vocabulary.Add(word, wordRep);
                        return wordRep;
                    }
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }
            return null;
        }

        static byte[] ReadHead(BinaryReader reader)
        {
            var buffer = new List<byte>();
            while (true)
            {
                byte symbol = reader.ReadByte();
                if (symbol == (byte)'\n')
                    break;
                else
                    buffer.Add(symbol);
            }
            return buffer.ToArray();
        }

        static Representation ReadRepresentation(BinaryReader reader, List<byte> buffer, int vectorSize)
        {

            buffer.Clear();
            var vectorSizeInByte = vectorSize * sizeof(float);

            while (true)
            {
                var symbol = reader.ReadByte();
                if (buffer.Count == 0 && symbol == (byte)'\n')
                    continue; // ignore newline at start of new entry
                else if (symbol == (byte)' ')
                    break;
                else
                    buffer.Add(symbol);
            }

            string word = Encoding.UTF8.GetString(buffer.ToArray());

            var vectorBytes = reader.ReadBytes(vectorSizeInByte);
            if (vectorBytes.Length < vectorSizeInByte)
                throw new EndOfStreamException("Vector entry is truncated");

            var vector = new float[vectorSize];
            Buffer.BlockCopy(vectorBytes, 0, vector, 0, vectorSizeInByte);

            return new Representation(word, vector);

        }

    }
}
