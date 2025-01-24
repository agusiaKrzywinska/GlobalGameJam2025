using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVParser : MonoBehaviour
{
    /// <summary>
    /// The csv file can be dragged through the inspector.
    /// </summary>
    public TextAsset csvFile;

    /// <summary>
    /// The grid in which the CSV File would be parsed.
    /// </summary>
    string[,] grid;


    void Start()
    {
        grid = GetCSVGrid(csvFile.text);
    }

    /// <summary>
    /// splits a CSV file into a 2D string array
    /// </summary>
    /// <returns> 2 day array of the csv file.</returns>
    /// <param name="csvText">the CSV data as string</param>
    static public string[,] GetCSVGrid(string csvText)
    {
        //split the data on split line character
        string[] lines = csvText.Split("\n"[0]);

        // find the max number of columns
        int totalColumns = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');
            totalColumns = Mathf.Max(totalColumns, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[totalColumns + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = lines[y].Split(',');
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];
            }
        }

        return outputGrid;
    }

    /// <summary>
    /// Gets the value from the CSV File at index(row,col).
    /// </summary>
    /// <param name="row">Row.</param>
    /// <param name="col">Col.</param>
    void GetValueAtIndex(int row, int col)
    {
        Debug.Log(grid[row, col]);
    }

    /// <summary>
    /// outputs the content of a 2D array.
    /// </summary>
    /// <param name="grid">2D array , here the CSV grid.</param>
    static public void DisplayGrid(string[,] grid)
    {
        string textOutput = "";
        for (int y = 0; y < grid.GetUpperBound(1); y++)
        {
            for (int x = 0; x < grid.GetUpperBound(0); x++)
            {

                textOutput += grid[x, y];
                textOutput += ",";
            }
            textOutput += "\n";
        }
        Debug.Log(textOutput);
    }
}

