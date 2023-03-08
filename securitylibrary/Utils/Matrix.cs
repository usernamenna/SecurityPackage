﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class Matrix
    {
        private int[,] data { get; }
        private int _rows {get;}
        private int _cols {get;}

        public enum DIMS
        {
            ROW, COL
        }

        /* Controls how the conversion between 1d and 2d structures
         ROW: 1D structure elements are filled row by row in the 2D structure
         COLUMNAR: 1D structure elements are filled column by column in the 2D structure
        */
        public enum CONVERSION_TYPE
        {
            ROW, COLUMNAR
        }

        public Matrix(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            data = new int[_rows, _cols];
        }

        public Matrix(List<int> items, int rows, int cols, CONVERSION_TYPE filltype) : this(rows, cols)
        {
            fillData(items, filltype);
        }

        public Matrix(List<int> items, int dim1, DIMS type, CONVERSION_TYPE filltype)
        {
            _rows = type == DIMS.ROW ? dim1 : inferOtherDim(dim1, items.Count);
            _cols = type == DIMS.COL ? dim1 : inferOtherDim(dim1, items.Count);

            data = new int[_rows, _cols];

            fillData(items, filltype);
        }

        public Matrix mul(Matrix m2)
        {
            if (!confirmDims(this, m2))
            {
                throw new Exception("Incompatible Dimensions! ( " + this._rows + " , " + this._cols + ") and ( " + m2._rows + " , " + m2._cols + ")");
            }

            Matrix res = new Matrix(this._rows, m2._cols);
            

            for (int row = 0; row < res._rows; row++)
            {
                for (int col = 0; col < res._cols; col++)
                {
                    res.data[row, col] = mulVectors(this.data, row, m2.data, col) % 26;
                }
            }

            return res;
        }

        public List<int> to1D(CONVERSION_TYPE type)
        {
            if (type == CONVERSION_TYPE.ROW) 
                return data.Cast<int>().ToList();
            else
            {
                List<int> res = new List<int>();

                int indx1D = 0;
                for (int i = 0; i < _cols; i++)
                {
                    for (int j = 0; j < _rows; j++)
                    {
                        res.Add(data[j, i]);
                        indx1D++;
                    }

                }
                return res;
            }

            
        }

        private int mulVectors(int[,] data1, int targetRow, int[,] data2, int targetCol)
        {
            int sum = 0;
            for (int i = 0; i < data1.GetLength(0); i++)
            {
                sum += data1[targetRow, i] * data2[i, targetCol];
            }

            return sum;
        }
        /*
         The ordinary of filling a matrix, first row the second etc..*/
        private void fillData(List<int> items, CONVERSION_TYPE type)
        {
            if (_rows == 0 || _cols == 0)
                throw new ArgumentException("Rows and Columns haven't been set properly to fill data");

            if (type == CONVERSION_TYPE.ROW)
                fillRowWise(items);
            else
                fillColumnar(items);
        }

        /*
         The ordinary of filling a matrix, first row the second etc..*/
        private void fillRowWise(List<int> items)
        {
            if (_rows == 0 || _cols == 0)
                throw new ArgumentException("Rows and Columns haven't been set properly to fill data");

            int k = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    data[i, j] = items[k];
                    k++;
                }


            }
        }

        /* Fill column wise, first columns then second etc..*/
        private void fillColumnar(List<int> items)
        {
            if (_rows == 0 || _cols == 0)
                throw new ArgumentException("Rows and Columns haven't been set properly to fill data");

            int k = 0;
            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    data[j, i] = items[k];
                    k++;
                }


            }
        }

        private int inferOtherDim(int dim1, int itemsCount)
        {
            return (int) Math.Ceiling(itemsCount * 1.0 / dim1);
        }

        private static bool confirmDims(Matrix m1, Matrix m2)
        {
            return m1._cols == m2._rows;
        }


    }
}
