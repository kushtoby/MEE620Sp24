//============================================================================
// DatGrid.cs
//============================================================================
using Godot;
using System;

public class DatGrid{
    Container parent;
    GridContainer grid;

    int nDisp;
    int nCol;

    Label[] rLabels;      // row labels;
    Label[] cLabels;      // column labels;
    Label[,] vals;        // values

    bool initialized;


    //------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------
    public DatGrid(Container pContainer)
    {
        parent = pContainer;

        grid = new GridContainer();

        initialized = false;
    }

    //------------------------------------------------------------------------
    // SetGridSize:
    //------------------------------------------------------------------------
    public void SetGridSize(int nRow, int nCol)
    {
        if(initialized)
            return;

        if(nRow < 1 || nCol < 1)
            return;

        grid.Columns = nCol + 1;

        int i,j;
        cLabels = new Label[nCol];
        for(i=0;i<nCol;++i){
            cLabels[i] = new Label();
            cLabels[i].Text = "cLabel";
        }
        rLabels = new Label[nRow];
        for(i=0;i<nRow;++i){
            rLabels[i] = new Label();
            rLabels[i].Text = "rLabel";
        }

        vals = new Label[nRow,nCol];
        for(i=0;i<nRow;++i){
            for(j=0;j<nCol;++j){
                vals[i,j] = new Label();
                vals[i,j].Text = "val";
            }
        }

        grid.AddChild(new Control());
        for(i=0;i<nCol;++i){
            grid.AddChild(cLabels[i]);
        }
        for(i=0;i<nRow;++i){
            grid.AddChild(rLabels[i]);
            for(j=0;j<nCol;++j){
                grid.AddChild(vals[i,j]);
            }
        }

        parent.AddChild(grid);
    }
}