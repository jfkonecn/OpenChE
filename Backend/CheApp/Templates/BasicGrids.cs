using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates
{
    /// <summary>
    /// creates simple grids
    /// </summary>
    class BasicGrids
    {

        /// <summary>
        /// Creates a simple gird
        /// <para>Note that the first and last rows and columns are used for the margins</para>
        /// <para>The heights and widths of all cells are stars</para>
        /// </summary>
        /// <param name="rows">Total rows in the grid</param>
        /// <param name="cols">Total cols grid</param>
        public static Grid SimpleGrid(int rows, int cols)
        {
            int rowMargin = (int)Application.Current.Resources["standardRowMargin"];
            int columnMargin = (int)Application.Current.Resources["standardColumnMargin"];
            return SimpleGrid(rows, cols, rowMargin, columnMargin);
        }
        
        
        
        /// <summary>
        /// Creates a simple gird
        /// <para>Note that the first and last rows and columns are used for the margins</para>
        /// <para>The heights and widths of all cells are stars</para>
        /// </summary>
        /// <param name="rows">Total rows in the grid</param>
        /// <param name="cols">Total cols grid</param>
        /// <param name="rowMargin">The length of the row margin</param>
        /// <param name="columnMargin">The length of the column margin</param>
        public static Grid SimpleGrid(int rows, int cols, int rowMargin, int columnMargin)
        {


            // create rows
            RowDefinitionCollection rowDefs = new RowDefinitionCollection();
            rowDefs.Add(new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) });

            for (int i = 0; i < rows; i++)
            {
                rowDefs.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            rowDefs.Add(new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) });

            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();
            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });

            for (int i = 0; i < cols; i++)
            {
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });



            return new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = rowDefs,
                ColumnDefinitions = colDefs
            };
        }
    }
}
