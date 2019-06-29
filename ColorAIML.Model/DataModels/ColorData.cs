//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;

namespace ColorAIML.Model.DataModels
{
    public class ColorData
    {
        //[ColumnName("Name"), LoadColumn(0)]
        //public string Name { get; set; }

        [ColumnName("conv1d_1_input_01"), VectorType(new int[] { 1, 4, 300 })]
        public VBuffer<float> conv1d_1_input_01 { get; set; }

        //[ColumnName("Label"), LoadColumn(2, 4)]
        //[VectorType(3)]
        //public float[] Color { get; set; }


        //[ColumnName("r"), LoadColumn(2)]
        //public float R { get; set; }


        //[ColumnName("g"), LoadColumn(3)]
        //public float G { get; set; }


        //[ColumnName("b"), LoadColumn(4)]
        //public float B { get; set; }
    }
}
