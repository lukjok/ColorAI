using ColorAIML.Model.DataModels;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tools
{
    public class OnnxModelConfigurator
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _mlModel;

        public OnnxModelConfigurator(string onnxModelFilePath)
        {
            _mlContext = new MLContext();
            // Model creation and pipeline definition for images needs to run just once, so calling it from the constructor:
            _mlModel = SetupMlNetModel(onnxModelFilePath);
        }

        public ITransformer SetupMlNetModel(string onnxModelFilePath)
        {
            var dataView = CreateEmptyDataView(_mlContext);

            var estimator = _mlContext.Transforms.ApplyOnnxModel(inputColumnName: "conv1d_1_input_01", outputColumnName: "dense_1_BiasAdd_0", modelFile: onnxModelFilePath);

            var model = estimator.Fit(dataView);
            return model;
        }

        public static IDataView CreateEmptyDataView(MLContext mlContext)
        {
            var dataEnum = new List<ColorData>();
            dataEnum.Add(new ColorData());
            return mlContext.Data.LoadFromEnumerable(dataEnum);
        }

        public void SaveMLNetModel(string mlnetModelFilePath)
        {
            // Save/persist the model to a .ZIP file to be loaded by the PredictionEnginePool
            _mlContext.Model.Save(_mlModel, null, mlnetModelFilePath);
        }
    }
}
