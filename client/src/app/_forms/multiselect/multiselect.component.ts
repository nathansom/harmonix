import { Component, EventEmitter, Input, OnInit, Output, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

@Component({
  selector: 'app-multiselect',
  templateUrl: './multiselect.component.html',
  styleUrls: ['./multiselect.component.css']
})
export class MultiselectComponent implements OnInit, ControlValueAccessor {
    @Input() dropdownList = [];
    @Input() label: string = "Select";
    @Input() singleSelection: boolean;
    selectedItems = [];
    dropdownSettings:IDropdownSettings = {};

    constructor(@Self() public ngControl: NgControl) {
        this.ngControl.valueAccessor = this
    }

    ngOnInit() {
      this.dropdownSettings = {
        singleSelection: this.singleSelection,
        idField: 'item_id',
        textField: 'item_text',
        enableCheckAll: false,
        allowSearchFilter: !this.singleSelection,        
      };
    }

    onItemSelect(item: any) {
    }
    onSelectAll(items: any) {
    }
    writeValue(obj: any): void {
    }
    registerOnChange(fn: any): void {
    }
    registerOnTouched(fn: any): void {
    }

}
