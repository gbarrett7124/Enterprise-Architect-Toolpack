using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace GlossaryManager {

	public class DataItemTabPage : GlossaryItemTabPage {

    public DataItemTabPage(GlossaryManagerUI ui) : base(ui) {
      this.Text = "Data Items";
    }

    protected override List<string> listHeaders {
      get {
        return new List<string>() {
          "Name", "Description", "Version", "Status", "Updated"
        };
      }
    }

    protected override List<string> AsListItemData(GlossaryItem item) {
      DataItem bi = item as DataItem;
      return new List<string>() {
        bi.Name,
        bi.Description,
        bi.Version,
        bi.Status.ToString(),
        bi.UpdateDate.ToString()
      };
    }

    public List<FieldValue> LogicalDataTypes {
      set {
        this.logicalDataTypesComboBox.DataSource = value;
      }
    }

    private Field logicalDataTypesComboBox;
    
   	public List<FieldValue> BusinessItems 
   	{
      set 
      {
        this.businessItemsCombobox.DataSource = value;
      }
    }

    private Field businessItemsCombobox;

	
	protected override void addSpecificFields()
	{
		this.businessItemsCombobox = this.addField(new Field(
			"Business Item", FieldOptions.WithNull | FieldOptions.WithPicker, this)
			{ Width = 200 });
		this.businessItemsCombobox.pickerType = "Class";
		this.businessItemsCombobox.pickerStereotype = "EDD_BusinessItem";
		
		this.addField(new Field("Label"){ Width = 200 });
		
		this.logicalDataTypesComboBox = this.addField(new Field(
		"Logical Datatype", FieldOptions.WithNull | FieldOptions.WithPicker, this)
		{ Width = 200 });
		this.logicalDataTypesComboBox.pickerType = "DataType";
		this.logicalDataTypesComboBox.pickerStereotype = "EDD_LogicalDatatype";
		
		this.addField(new Field("Size"){ Width = 200 });
		this.addField(new Field("Format"){ Width = 200 });
		this.addField(new Field("Initial Value") {Width = 200 });
		var last = this.addField(new Field("Description") {
		Multiline = true,
		Width     = 200,
		Height    = 100});
		// creates a column break, marking the difference between GI and BI/DI
		this.form.SetFlowBreak(last, true);
	}
	
    protected override void setFields() 
    {
    	base.setFields();
        this.fields["Label"].Value            = ((DataItem)this.Current).Label;
		this.fields["Logical Datatype"].Value = ((DataItem)this.Current).LogicalDatatypeName;
		this.fields["Size"].Value             = ((DataItem)this.Current).Size.ToString();
		this.fields["Format"].Value           = ((DataItem)this.Current).Format;
		this.fields["Description"].Value      = ((DataItem)this.Current).Description;
		this.fields["Initial Value"].Value    = ((DataItem)this.Current).InitialValue;
    }

    protected override void Update(Field field) {
      if( ! this.HasItemSelected ) { return; }
      switch(field.Label.Text) {
        case "Label":            ((DataItem)this.Current).Label           = field.Value; break;
        case "Logical Datatype": ((DataItem)this.Current).LogicalDatatypeName = field.Value; break;
        case "Size":
        	int newSize;
        	((DataItem)this.Current).Size = 
        		int.TryParse(field.Value,out newSize) ?
        		newSize :
        		0;
        	break;
        case "Format":           ((DataItem)this.Current).Format          = field.Value; break;
        case "Description":      ((DataItem)this.Current).Description     = field.Value; break;
        case "Initial Value":    ((DataItem)this.Current).InitialValue    = field.Value; break;        
      }
      base.Update(field);
    }

    internal override void addButtonClick(object sender, EventArgs e) {
      this.add( new DataItem() { Name = "New Data Item" } );
    }

    internal override void exportButtonClick(object sender, EventArgs e) {
      this.export<DataItem>();
    }

    internal override void importButtonClick(object sender, EventArgs e) {
      this.import<DataItem>();
    }
  }
}
