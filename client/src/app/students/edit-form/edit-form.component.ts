import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-edit-form',
  templateUrl: './edit-form.component.html',
  styleUrls: ['./edit-form.component.css']
  //encapsulation: ViewEncapsulation.None
})
export class EditFormComponent implements OnInit {
  public editStudentForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    //public uniqueNameService: UniqueNameService,
  ) {
    // Conditional that monitors testing for unique name by service.
    // this.uniqueNameService.inDatabase.subscribe(result => {
    //   this.inDatabase = result;  // When set to true it triggers the message.
    //   return result === true ? this.isTaken() : null;
    // });
  }

  ngOnInit() {
    this.createForm();
    // Set the initial user name validation trigger to false - no message.
    //this.inDatabase = this.uniqueNameService.inDatabase.value;
  }


  // The reactive model that is bound to the form.

  private createForm() {
    this.editStudentForm = this.fb.group({
      id: [''],
      firstName: [''],
      lastName: [''],
      email: [''],
      lastUpdated: ['']
    });
  }
  
}