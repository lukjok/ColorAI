using ColorAIML.Model.DataModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Word2vec.Tools;

namespace API.Services
{
    public interface IColorRecognisionService
    {
        string PredictColor(string colorName);
    }

    public class ColorRecognisionService : IColorRecognisionService
    {
        private readonly PredictionEnginePool<ColorData, ColorDataPrediction> _model;
        private readonly Vocabulary _word2Vec;
        private readonly IConfiguration _configuration;
        private readonly string[] commonSeperators = new string[] { " ", ".", "-", ",", ";", ":" };

        public ColorRecognisionService(PredictionEnginePool<ColorData, ColorDataPrediction> model, IConfiguration configuration)
        {
            _model = model;
            _configuration = configuration;
            _word2Vec = new Word2VecBinaryReader().Read(GetAbsolutePath(configuration["MLModel:Word2VecPath"]));
        }

        public string PredictColor(string colorName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(colorName))
                    throw new Exception("Invalid color name");

                var splitWords = colorName.Split(commonSeperators, StringSplitOptions.RemoveEmptyEntries);

                if(splitWords.Length * 300 > 1200)
                    throw new Exception("Specified color name have too much keywords");

                List<float> tmpValues = new List<float>();
                int[] indicesArr = Enumerable.Range(0, 300 * splitWords.Length).ToArray();

                foreach (var word in splitWords)
                {
                    var wordVec = _word2Vec.GetRepresentationOrNullFor(word);

                    if (wordVec != null)
                        tmpValues.AddRange(wordVec.NumericVector);
                    else if (splitWords.Length == 1)
                        throw new Exception("Specified color name was not found in the vocabulary");
                }

                ColorData sampleStatement = new ColorData
                {
                    conv1d_1_input_01 = new VBuffer<float>(1200, 300 * splitWords.Length, tmpValues.ToArray(), indicesArr)
                };

                var resultprediction = _model.Predict(sampleStatement);
                return resultprediction.FormatHexColor();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while predicting color values. Internal error: {ex.Message}");
            }
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }
    }
}
