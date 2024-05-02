//============================================================================
// DatDisplay2.cs
//============================================================================
using Godot;
using System;

public class DatDisplay2{
    Container parent;
    GridContainer grid;
    VBoxContainer vbox;
    Button testButton;

    int nDisp;
    bool hasTitle;
    bool hasButtons;

    Label[] labels;
    Label[] values;
    String[] fStrings;    // format strings
    String[] decStrings;  // decimal strings
    String[] sfxStrings;  // suffix strings
    Label title;

    public CheckBox[] checkBoxes;
    bool initialized;

    //------------------------------------------------------------------------
    // Constuctor
    //------------------------------------------------------------------------
    public DatDisplay2(Container pContainer)
    {
        //GD.Print("DatDisplay2 constructor");
        parent = pContainer;

        vbox = new VBoxContainer();
        parent.AddChild(vbox);

        hasTitle = false;
        hasButtons = false;
        initialized = false;
        // testButton = new Button();
        // testButton.Text = "Test Button";
        // parent.AddChild(testButton);
    }

    //------------------------------------------------------------------------
    // SetNDisplay:
    //------------------------------------------------------------------------
    public void SetNDisplay(int sz, bool _hasTitle = false, 
        bool _hasButtons = false)
    {
        //GD.Print("Inside SetNDisplay");
        if(initialized)
            return;

        if (sz <= 0)
        {
            return;
        }

        hasTitle = _hasTitle;
        hasButtons = _hasButtons;

        if(hasTitle){
            title = new Label();
            title.Text = "Title";
            vbox.AddChild(title);
        }

        labels = new Label[sz];
        values = new Label[sz];
        fStrings = new String[sz];
        decStrings = new string[sz];
        sfxStrings = new string[sz];

        grid = new GridContainer();
        if(hasButtons){
            checkBoxes = new CheckBox[sz];
            grid.Columns = 3;
        }
        else
            grid.Columns = 2;
        vbox.AddChild(grid);

        for(int i=0;i<sz;++i)
        {
            labels[i] = new Label();
            values[i] = new Label();
            labels[i].Text = "Label not set";
            values[i].Text = "Value not set";
            //vBoxLabels.AddChild(labels[i]);
            //vBoxValues.AddChild(values[i]);
            grid.AddChild(labels[i]);
            grid.AddChild(values[i]);
            if(hasButtons){
                checkBoxes[i] = new CheckBox();
                grid.AddChild(checkBoxes[i]);
            }

            fStrings[i] = "0.00";
            decStrings[i] = "0.00";
            sfxStrings[i] = "";
        }
        //hbox.AddChild(vBoxLabels);
        //hbox.AddChild(vBoxValues);

        nDisp = sz;
        initialized = true;
    }

    //------------------------------------------------------------------------
    // SetTitle
    //------------------------------------------------------------------------
    public void SetTitle(string str)
    {
        if(!hasTitle)
            return;

        title.Text = str;       
    }

    //------------------------------------------------------------------------
    // SetLabel: Sets a label string
    //------------------------------------------------------------------------
    public void SetLabel(int idx, string str)
    {
        if(idx < 0 || idx >= nDisp || !initialized)
        {
            return;
        }

        labels[idx].Text = str;
    }

    //------------------------------------------------------------------------
    // SetValue: Sets a value string
    //------------------------------------------------------------------------
    public void SetValue(int idx, float val)
    {
        if(idx < 0 || idx >= nDisp || !initialized)
        {
            return;
        }

        values[idx].Text = val.ToString(fStrings[idx]);
    }

    public void SetValue(int idx, string str)
    {
        if(idx < 0 || idx >= nDisp || !initialized)
        {
            return;
        }

        values[idx].Text = str;
    }

    //------------------------------------------------------------------------
    // SetDigitsAfterDecimal: Sets number of digits after decimal point
    //------------------------------------------------------------------------
    public void SetDigitsAfterDecimal(int idx, int n)
    {
        if(idx < 0 || idx >= nDisp || !initialized)
            return;

        if(n < 0)
            return;

        if(n > 10)
            n = 10;

        decStrings[idx] = "0.";
        for(int i=0;i<n;++i)
            decStrings[idx] += "0";

        fStrings[idx] = decStrings[idx] + sfxStrings[idx];
    }

    //------------------------------------------------------------------------
    // SetSuffix:
    //------------------------------------------------------------------------
    public void SetSuffix(int idx, String sfx)
    {
        if(idx < 0 || idx >= nDisp || !initialized)
            return;

        sfxStrings[idx] = " " + sfx;

        fStrings[idx] = decStrings[idx] + sfxStrings[idx];
    }

    //------------------------------------------------------------------------
    // SetSuffixDegree:
    //------------------------------------------------------------------------
    public void SetSuffixDegree(int idx)
    {
        if(idx < 0 || idx >= nDisp || !initialized)
            return;

        sfxStrings[idx] = " \u00B0";

        fStrings[idx] = decStrings[idx] + sfxStrings[idx];
    }
}