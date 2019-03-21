import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { Student } from 'src/app/shared/interfaces';
import { EditFormComponent } from '../edit-form/edit-form.component';
import { RepositoryService } from 'src/app/services/repository.service';
import{ MessagesService} from 'src/app/services/messages-service/messages.service'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { from } from 'rxjs';

@Component({
  selector: 'app-edit-student',
  templateUrl: './edit-student.component.html',
  styleUrls: ['./edit-student.component.css']
})
export class EditStudentComponent  implements AfterViewInit {

  private formValue: Student;
  private recordId: number;
  private paginator;
  private dataSource;
  private dateTime = this.dateTimeNow();


  // This is a form group from FormBuilder.
  @ViewChild(EditFormComponent) 
  private editForm: EditFormComponent;



  constructor(
    private repository: RepositoryService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<EditStudentComponent>,   
    private router: Router,  
    //private updateDatatableService: UpdateDatatableService,
    private messagesService: MessagesService,
  ) {}

  ngAfterViewInit() {
    setTimeout(() => {
      if (this.data)
        this.fetchRecord();
    }, 200);
  }

  public fetchRecord() {

    this.recordId = this.data.recordId;
    this.paginator = this.data.paginator;
    this.dataSource = this.data.dataSource;

    this.repository.getData(`api/students/${this.recordId}`)
        .subscribe(data => {
            this.fillForm(data);
          },
          (err: HttpErrorResponse) => {
            console.log(err.error);
            console.log(err.message);
            this.handleError(err);
          });
  }

  public dateTimeNow() {
    var date = new Date();
    var day = date.getDate();      
    var month = date.getMonth() + 1;    
    var year = date.getFullYear();  
    var hour = date.getHours();     
    var minute = date.getMinutes(); 
    var second = date.getSeconds();
    return day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;
  }

  // Populate the form, called above in fetchRecord().

  private fillForm(studentData) {
    
    this.editForm.editStudentForm.setValue({
      id: studentData.id,
      firstName: studentData.firstName,
      lastName: studentData.lastName,
      email: studentData.email,
      lastUpdated:  studentData.lastUpdated
    });
  }

  public update(formValue) {
    if (formValue.id != ""){
    this.repository.update(`api/students`, formValue)
    .subscribe(() => this.success());    
    }else{
      //TODO: Fix this.
      formValue.lastUpdated = "2019-03-04T02:14:58.849Z" 
      this.repository.create(`api/students`, formValue)
      .subscribe(() => this.success());  
    }
  }



  private reset() {
    //this.repository.reset();
  }

  private success() {
    this.messagesService.openDialog('Success!', 'Saved to database.');
    this.dialogRef.close(true);
  }

  private handleError(error) {
    this.messagesService.openDialog('Error', 'Please check your connection.');
  }  
}