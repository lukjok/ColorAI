//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using Microsoft.ML.Data;

namespace ColorAIML.Model.DataModels
{
    public class ColorDataPrediction
    {
        [ColumnName("dense_1_BiasAdd_0")]
        public float[] dense_1_BiasAdd_0 { get; set; }

        public string FormatColor()
        {
            return $"R: {dense_1_BiasAdd_0[0]}, G: {dense_1_BiasAdd_0[1]}, B: {dense_1_BiasAdd_0[2]}";
        }

        public string FormatHexColor()
        {
            return $"#{((int)dense_1_BiasAdd_0[0]).ToString("X")}{((int)dense_1_BiasAdd_0[1]).ToString("X")}{((int)dense_1_BiasAdd_0[2]).ToString("X")}";
        }

    }
}
